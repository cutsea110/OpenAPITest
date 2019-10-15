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
	/// アカウント
	/// </summary>
	public partial class Account
	{
        /// <summary>
        /// アカウントIDの文字列表現
        /// </summary>
        public string AccountID => account_id.ToString();
	}

    /// <summary>
    /// アカウント条件
    /// </summary>
    public partial class AccountCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<Account, bool>> CreatePredicate()
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
    /// アカウント拡張メソッド用クラス
    /// </summary>
    static public partial class AccountExtention
    {
        #region static methods
        /// <summary>
        /// UserTypeとそのユーザ種別に応じたidを指定してAccountを取得する
        /// その際に適切にユーザリソースもLoadWithされる
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="userType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static public IQueryable<Account> GetAccounts(this ITable<Account> accounts, UserType userType, string id)
        {
            var q = new AccountCondition();
            switch (userType)
            {
                case UserType.職員:
                    accounts.LoadWith(_ => _.Staff);
                    q.staff_no_eq = id;
                    break;
                case UserType.教員:
                    accounts.LoadWith(_ => _.Teacher);
                    q.teacher_no_eq = id;
                    break;
                case UserType.他ユーザ:
                    accounts.LoadWith(_ => _.User);
                    q.user_no_eq = id;
                    break;
                case UserType.ゴースト:
                    q.account_id_eq = int.Parse(id);
                    break;
                default:
                    break;
            }
            return accounts.Where(q.CreatePredicate());
        }
        #endregion
    }
}