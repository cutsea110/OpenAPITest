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
	/// 人名種別
	/// </summary>
	public partial class PersonNameType
	{

	}

	/// <summary>
	/// 人名種別条件
	/// </summary>
	public partial class PersonNameTypeCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<PersonNameType, bool>> CreatePredicate()
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
	/// 人名種別拡張メソッド用クラス
	/// </summary>
	static public partial class PersonNameTypeExtension
	{
		#region static methods
		/// <summary>
		/// サンプル実装
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		// static public IQueryable<PersonNameType> TheMethod(this ITable<PersonNameType> list)
		// {
		// 	throw new NotImplementedException();
		// }
		#endregion
	}
}