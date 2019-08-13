using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace OpenAPITest.CustomPolicyProvider
{
    internal class IPFilterPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public IPFilterPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [IPFilterAuthorize] in this sample)
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new IPFilterRequirement());
            return Task.FromResult(policy.Build());
        }
    }
}
