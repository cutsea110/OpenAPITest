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
        [ProducesResponseType(typeof(TokenViewModel), 200)]
        [ProducesResponseType(400)]
        public IActionResult Token([FromBody]TokenInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new peppaDB())
                {
                    var accs = db.Account
                        .LoadWith(_ => _.Staff)
                        .LoadWith(_ => _.PasswordList)
                        .Where(_ => _.removed_at == null && _.staff_no == inputModel.ID)
                        .ToList();
                    var validAcc = accs.FirstOrDefault(_ => _.PasswordList.Any(p => p.Authenticate(inputModel.Password, DateTime.UtcNow)));

                    if (validAcc != null)
                    {
                        var auth = new Auth
                        {
                            ID = validAcc.AccountID,
                            Name = validAcc.staff_no,
                        };
                        var token = CreateJwtSecurityToken(auth);
                        return Ok(new TokenViewModel
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo,
                        });
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// 現在のアカウントID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);


        /// <summary>
        /// ログインユーザが自分のパスワードを変更する
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [Authorize("Change_Password")]
        [HttpPut("change-password")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public IActionResult Change([FromBody]ChangePasswordInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new peppaDB())
                {
                    var pw = db.Password.Find(CurrentAccountId);

                    if (pw != null)
                    {
                        var new_password = pw.Encrypt(inputModel.NewPassword);
                        var new_life = pw.NewLifeExpectancy;
                        var count = db.Password
                            .Where(_ => _.removed_at == null && _.account_id == CurrentAccountId)
                            .Update(_ => new Password
                            {
                                password_hash = new_password,
                                expiration_on = new_life,
                                modified_by = CurrentAccountId,
                            });

                        return Ok(count);
                    }
                }
            }
            return BadRequest();
        }

    }
}