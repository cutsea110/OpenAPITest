using Microsoft.AspNetCore.Authorization;

namespace OpenAPITest.CustomPolicyProvider
{
    internal class IPFilterAuthorizeAttribute : AuthorizeAttribute
    {
        public IPFilterAuthorizeAttribute()
        {

        }
    }
}
