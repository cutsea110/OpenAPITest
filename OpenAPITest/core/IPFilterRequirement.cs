using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Net;

namespace OpenAPITest.CustomPolicyProvider
{
    /// <summary>
    /// クライアントIPによる制限をする独自ポリシーのための要件定義
    /// </summary>
    internal class IPFilterRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 明示的に許可されている
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool explicitAllowed(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.AllowedIpAddresses.Contains(ip) || AppConfiguration.AccessControl.AllowedNetworks.Any(net => net.Contains(ip))
                ;
        }

        /// <summary>
        /// 暗黙的に許可されている
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool implicitAllowed(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.AllowedIpAddresses.Length == 0 && AppConfiguration.AccessControl.AllowedNetworks.Length == 0
                ;
        }
        /// <summary>
        /// 明示的もしくは暗黙的に許可されている
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool allowed(IPAddress ip)
        {
            return explicitDenied(ip) || implicitAllowed(ip);
        }
        /// <summary>
        /// 明示的に拒否されている
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool explicitDenied(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.DeniedIpAddresses.Contains(ip) || AppConfiguration.AccessControl.DeniedNetworks.Any(net => net.Contains(ip))
                ;
        }

        /// <summary>
        /// 内部アクセスなら常に許可される
        /// 内部アクセスでない場合、アクセス許可されていてかつ拒否されていない場合のみ真となる
        /// </summary>
        /// <param name="clientIp"></param>
        /// <returns></returns>
        public bool Allow(IPAddress clientIp)
        {
            return
                AppConfiguration.AccessControl.InsiderIpAddresses.Contains(clientIp) ||
                (allowed(clientIp) && explicitDenied(clientIp) == false)
                ;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IPFilterRequirement()
        {
        }
    }
}
