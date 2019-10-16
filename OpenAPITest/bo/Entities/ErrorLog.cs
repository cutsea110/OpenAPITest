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
	/// エラーログ
	/// </summary>
	public partial class ErrorLog
	{

	}

	/// <summary>
	/// エラーログ条件
	/// </summary>
	public partial class ErrorLogCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<ErrorLog, bool>> CreatePredicate()
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
	/// エラーログ拡張メソッド用クラス
	/// </summary>
	static public partial class ErrorLogExtension
	{
		#region static methods
		/// <summary>
		/// サンプル実装
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		// static public IQueryable<ErrorLog> TheMethod(this ITable<ErrorLog> list)
		// {
		// 	throw new NotImplementedException();
		// }
		#endregion
	}
}