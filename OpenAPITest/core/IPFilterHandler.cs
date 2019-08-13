using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OpenAPITest.CustomPolicyProvider
{
    /// <summary>
    /// IP制限の要件ハンドラクラス
    /// </summary>
    internal class IPFilterHandler : AuthorizationHandler<IPFilterRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public IPFilterHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IPFilterRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            IPAddress clientIP = httpContext.Connection.RemoteIpAddress;
            
            if (requirement.Allow(clientIP))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
