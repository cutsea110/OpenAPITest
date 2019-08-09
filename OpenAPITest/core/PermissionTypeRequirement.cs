using Microsoft.AspNetCore.Authorization;

using OpenAPITest.Domain;

namespace OpenAPITest.CustomPolicyProvider
{
    /// <summary>
    /// PermissionTypeによる独自ポリシーのための要件定義
    /// </summary>
    public class PermissionTypeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 権限リスト
        /// </summary>
        public PermissionType[] Permissions { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionTypeRequirement(PermissionType[] permissions)
        {
            Permissions = permissions;
        }
    }
}
