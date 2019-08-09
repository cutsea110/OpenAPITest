using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using peppa.util;

using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
    public class Auth
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class TokenInputModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class TokenViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
    public class PasswordInputModel
    {
        [Required]
        public int AccountId { get; set; }
        public HashMethod Method { get; set; } = HashMethod.SHA256;
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public int? PasswordLifeDays { get; set; } = 90;
        public int CanFailTimes { get; set; } = 3;
    }
    public class ChangePasswordInputModel
    {
        [Required]
        public string OrigPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }

    public partial class PasswordController : ControllerBase
    {
        /// <summary>
        /// 現在のアカウントID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);


        private JwtSecurityToken CreateJwtSecurityToken(Auth user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.ID),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(ClaimTypes.Sid, user.ID),
                new Claim(ClaimTypes.Name, user.Name),
            };
            var token = new JwtSecurityToken(
                issuer: AppConfiguration.JwtSecret.SiteUri,
                audience: AppConfiguration.JwtSecret.SiteUri,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(AppConfiguration.JwtSecret.Life),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.JwtSecret.SecretKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            return token;
        }

        /// <summary>
        /// ログイン認証をする
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Token([FromBody]TokenInputModel inputModel)
        {
#if DEBUG
            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
            if (ModelState.IsValid)
            {
                using (var db = new peppaDB())
                {
                    var q = new AccountCondition
                    {
                        staff_no_eq = inputModel.ID,
                    };
                    var accs = db.Account
                        .LoadWith(_ => _.Staff)
                        .LoadWith(_ => _.PasswordList)
                        .Where(q.CreatePredicate())
                        .ToList();

                    var now = DateTime.UtcNow;
                    var validAccPw = accs.SelectMany(a => a.PasswordList.Select(p => (Acc: a, Pw: p))).FirstOrDefault(_ => _.Pw.IsActive(now));

                    if (validAccPw.Pw != null)
                    {
                        if (validAccPw.Pw.Authenticate(inputModel.Password, now))
                        {
                            // 連続失敗回数は初期化
                            if (validAccPw.Pw.fail_times != 0)
                            {
                                var ret = db.Password
                                .Where(_ => _.uid == validAccPw.Pw.uid)
                                .Update(_ => new Password
                                {
                                    fail_times = 0,
                                });
                            }
                            var token = CreateJwtSecurityToken(new Auth
                            {
                                ID = validAccPw.Acc.AccountID,
                                Name = validAccPw.Acc.staff_no,
                            });
                            return Ok(new TokenViewModel
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                Expiration = token.ValidTo,
                            });
                        }
                        else
                        {
                            // 連続失敗回数をインクリしつつ回数上限に達していたらロックフラグも立てる
                            var ret = db.Password
                                .Where(_ => _.uid == validAccPw.Pw.uid)
                                .Update(_ => new Password
                                {
                                    fail_times = Math.Min(_.can_fail_times, _.fail_times + 1),
                                    lock_flg = _.can_fail_times <= _.fail_times + 1 ? 1 : 0,
                                });
                            return Unauthorized();
                        }
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [Authorize("Create_Password")]
        [HttpPost("init-password")]
        [ProducesResponseType(typeof(Password), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Init([FromBody]PasswordInputModel inputModel)
        {
#if DEBUG
            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
            if (ModelState.IsValid)
            {
                using (var db = new peppaDB())
                {
                    var pw = new Password
                    {
                        account_id = inputModel.AccountId,
                        HashType = inputModel.Method,
                        expiration_on = inputModel.ExpiredOn,
                        can_fail_times = inputModel.CanFailTimes,
                    };
                    pw.password_hash = pw.Encrypt(inputModel.NewPassword);

                    var ret = db.Insert<Password>(pw);
                    return Ok(ret);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// ログインユーザが自分のパスワードを変更する
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [Authorize("Change_Password")]
        [HttpPut("change-password")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Change([FromBody]ChangePasswordInputModel inputModel)
        {
#if DEBUG
            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
            if (ModelState.IsValid)
            {
                using (var db = new peppaDB())
                {
                    var q = new PasswordCondition
                    {
                        account_id_eq = CurrentAccountId,
                    };
                    var pw = db.Password.SingleOrDefault(q.CreatePredicate());

                    if (pw != null)
                    {
                        var new_password = pw.Encrypt(inputModel.NewPassword);
                        var new_life = pw.NewLifeExpectancy;
                        var ret = db.Password
                            .Where(q.CreatePredicate())
                            .Update(_ => new Password
                            {
                                password_hash = new_password,
                                expiration_on = new_life,
                                modified_by = CurrentAccountId,
                            });

                        return Ok(ret);
                    }
                }
            }
            return BadRequest();
        }

    }
}