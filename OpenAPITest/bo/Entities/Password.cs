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
	/// パスワード認証
	/// </summary>
	public partial class Password
	{
        /// <summary>
        /// ハッシュ化方式に応じてパスワードが合致するかをチェック
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Match(string password)
        {
            switch (HashType)
            {
                case HashMethod.SHA256:
                    return password_hash == password.ToSha256Sum();
                case HashMethod.平文:
                    return password_hash == password;
                default:
                    return false; // FIXME
            }
        }
        /// <summary>
        /// パスワード化方式に応じてパスワードの暗号化
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encrypt(string password)
        {
            switch (HashType)
            {
                case HashMethod.SHA256:
                    return password.ToSha256Sum();
                case HashMethod.平文:
                    return password;
                default:
                    throw new Exception("Unknown HashMethod.");
            }
        }
        /// <summary>
        /// 新しい余命を計算して返す
        /// ただし現在の余命と余命の延長日数が両方定義されるている必要がある
        /// どちらかがnullの場合には計算結果もnullになる
        /// </summary>
        public DateTime? NewLifeExpectancy =>
            expiration_on.HasValue && password_life_days.HasValue ?
            expiration_on.Value.AddDays(password_life_days.Value) :
            (DateTime?)null;
        /// <summary>
        /// ロックされているか
        /// </summary>
        public bool IsLocked => lock_flg == 1;
        /// <summary>
        /// 認証OKかどうかの判断
        /// 1. 削除されてないこと
        /// 2. 有効期限内であること
        /// 3. ロックされていないこと
        /// 4. パスワードが合致すること
        /// </summary>
        /// <param name="password"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Authenticate(string password, DateTime dt) =>
            removed_at == null &&
            (expiration_on == null || expiration_on >= dt) &&
            IsLocked == false &&
            Match(password);
	}

	/// <summary>
	/// パスワード認証条件
	/// </summary>
	public partial class PasswordCondition
	{
		#region properties
		// [DataMember]
		// public int? uid_eq { get; set; }
		#endregion

		/// <summary>
		/// 検索条件式の生成
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<Password, bool>> CreatePredicate()
		{
			var predicate = base.CreatePredicate();

			#region extra
			// if (uid_eq != null)
			// 	 predicate = predicate.And(_ => _.uid == uid_eq);
			#endregion

			return predicate;
		}
	}
}