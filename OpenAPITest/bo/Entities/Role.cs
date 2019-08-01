using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

using LinqToDB;
using LinqToDB.Mapping;

using peppa.util;

namespace OpenAPITest.Domain
{
	/// <summary>
	/// ロールマスタ
	/// </summary>
	public partial class Role
	{

	}

	/// <summary>
	/// ロールマスタ条件
	/// </summary>
	public partial class RoleCondition
	{
		#region properties
        /// <summary>
        /// ロール権限リストをLoadWithしていることが前提
        /// </summary>
        public RolePermissionCondition rpc { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<Role, bool>> CreatePredicate()
		{
			var predicate = base.CreatePredicate();

            #region extra
            if (rpc != null)
                predicate = predicate.And(_ => _.RolePermissionList.AsQueryable().Any(rpc.CreatePredicate()));
			#endregion

			return predicate;
		}
	}
}