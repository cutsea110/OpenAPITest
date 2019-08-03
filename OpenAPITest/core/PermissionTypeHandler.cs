using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OpenAPITest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenAPITest.core
{
    /// <summary>
    /// PermissionTypeRequirement要件ハンドラクラス
    /// </summary>
    public class PermissionTypeHandler : AuthorizationHandler<PermissionTypeRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public PermissionTypeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// PermissionTypeRequirement要件を実際に確認するハンドラ
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionTypeRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            using (var db = new peppaDB())
            {
                // 現在の(認証済み)アカウントID
                var currentAccountID = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value);
                // 所持している権限リスト
                var permissions =
                    db.Account
                    .LoadWith(_ => _.AccountRoleList.First().Role.RolePermissionList)
                    .Where(_ => _.account_id == currentAccountID)
                    .SelectMany(a => a.AccountRoleList.SelectMany(r => r.Role.RolePermissionList.Select(rp => rp.permission_id)))
                    .Distinct();

                if (requirement.Permissions.Select(_ => _.Val()).Intersect(permissions).Any())
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                return Task.CompletedTask;
            }
        }
    }
}
