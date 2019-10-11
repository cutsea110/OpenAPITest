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
using OpenAPITest.CustomPolicyProvider;

namespace OpenAPITest.Controllers
{
    #region Models
    /// <summary>
    /// 認証
    /// </summary>
    public class Auth
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// アカウント名
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// パスワード認証用入力モデル
    /// </summary>
    public class TokenInputModel
    {
        /// <summary>
        /// 利用者種別
        /// </summary>
        [Required]
        public UserType UserType { get; set; }
        /// <summary>
        /// アカウントID
        /// </summary>
        [Required]
        public string ID { get; set; }
        /// <summary>
        /// パスワード
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    /// <summary>
    /// 認証トークン
    /// </summary>
    public class TokenViewModel
    {
        /// <summary>
        /// トークン
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime Expiration { get; set; }
    }
    /// <summary>
    /// パスワード入力モデル
    /// </summary>
    public class PasswordInputModel
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        [Required]
        public int AccountId { get; set; }
        /// <summary>
        /// パスワードハッシュ化方式
        /// </summary>
        public HashMethod Method { get; set; } = HashMethod.SHA256;
        /// <summary>
        /// 新しいパスワード
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
        /// <summary>
        /// 新しいパスワード(確認)
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime? ExpiredOn { get; set; }
        /// <summary>
        /// パスワード更新時の有効期間の延長日数
        /// </summary>
        public int? PasswordLifeDays { get; set; } = 90;
        /// <summary>
        /// 連続での認証失敗を許容する回数の上限
        /// </summary>
        public int CanFailTimes { get; set; } = 3;
    }
    /// <summary>
    /// パスワード変更入力モデル
    /// </summary>
    public class ChangePasswordInputModel
    {
        /// <summary>
        /// パスワードのハッシュ化方式
        /// </summary>
        [Required]
        public HashMethod Method { get; set; } = HashMethod.SHA256;
        /// <summary>
        /// 現在のパスワード
        /// </summary>
        [Required]
        public string OrigPassword { get; set; }
        /// <summary>
        /// 新しいパスワード
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
        /// <summary>
        /// 新しいパスワード(確認)
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
    #endregion

    public partial class PasswordController : ControllerBase
    {

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
                    var now = DateTime.UtcNow;

                    var accs = db.Account
                        .LoadWith(_ => _.PasswordList)
                        .GetAccounts(inputModel.UserType, inputModel.ID)
                        .ToList();

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

        /// <summary>
        /// パスワードを初期化する
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [PermissionTypeAuthorize("Create_Password")]
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
        [PermissionTypeAuthorize("Change_Password")]
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
                        var new_password = pw.Encrypt(inputModel.Method, inputModel.NewPassword);
                        var new_life = pw.NewLifeExpectancy;
                        var ret = db.Password
                            .Where(q.CreatePredicate())
                            .Update(_ => new Password
                            {
                                HashType = inputModel.Method,
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