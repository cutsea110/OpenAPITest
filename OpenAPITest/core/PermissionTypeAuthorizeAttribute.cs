using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

using OpenAPITest.Domain;

namespace OpenAPITest.CustomPolicyProvider
{
    internal class PermissionTypeAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionType[] PermissionTypes
        {
            get
            {
                return Policy
                    .Split(',')
                    .Select(s => s.Trim().ToPermissionType().Value)
                    .ToArray();
            }
            set
            {
                Policy = string.Join(',', value.Select(p => p.Val()));
            }
        }

        public PermissionTypeAuthorizeAttribute(string perms)
        {
            Policy = perms;
        }
    }
}
