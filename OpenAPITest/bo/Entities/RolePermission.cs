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
	/// ロール権限
	/// </summary>
	public partial class RolePermission
	{

	}

	/// <summary>
	/// ロール権限条件
	/// </summary>
	public partial class RolePermissionCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<RolePermission, bool>> CreatePredicate()
		{
			var predicate = base.CreatePredicate();

			#region extra
			// if (uid_eq != null)
			// 	 predicate = predicate.And(_ => _.uid == uid_eq);
			#endregion

			return predicate;
		}
	}

	/// <summary>
	/// ロール権限拡張メソッド用クラス
	/// </summary>
	static public partial class RolePermissionExtension
	{
		#region static methods
		/// <summary>
		/// サンプル実装
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		// static public IQueryable<RolePermission> TheMethod(this ITable<RolePermission> list)
		// {
		// 	throw new NotImplementedException();
		// }
		#endregion
	}
}