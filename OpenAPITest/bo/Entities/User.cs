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
	/// 他ユーザ
	/// </summary>
	public partial class User
	{

	}

	/// <summary>
	/// 他ユーザ条件
	/// </summary>
	public partial class UserCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<User, bool>> CreatePredicate()
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
	/// 他ユーザ拡張メソッド用クラス
	/// </summary>
	static public partial class UserExtension
	{
		#region static methods
		/// <summary>
		/// サンプル実装
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		// static public IQueryable<User> TheMethod(this ITable<User> list)
		// {
		// 	throw new NotImplementedException();
		// }
		#endregion
	}
}