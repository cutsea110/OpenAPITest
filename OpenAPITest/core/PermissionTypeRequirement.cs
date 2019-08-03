using Microsoft.AspNetCore.Authorization;
using OpenAPITest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAPITest.core
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
