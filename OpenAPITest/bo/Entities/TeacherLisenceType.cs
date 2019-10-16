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
	/// 教員資格種別
	/// </summary>
	public partial class TeacherLisenceType
	{

	}

	/// <summary>
	/// 教員資格種別条件
	/// </summary>
	public partial class TeacherLisenceTypeCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<TeacherLisenceType, bool>> CreatePredicate()
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
	/// 教員資格種別拡張メソッド用クラス
	/// </summary>
	static public partial class TeacherLisenceTypeExtension
	{
		#region static methods
		/// <summary>
		/// サンプル実装
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		// static public IQueryable<TeacherLisenceType> TheMethod(this ITable<TeacherLisenceType> list)
		// {
		// 	throw new NotImplementedException();
		// }
		#endregion
	}
}