//---------------------------------------------------------------------------------------------------
// <auto-generated>
//	This code was generated by peppapig from database table definition.
//	Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;

using peppa.Domain;

namespace MockWebAPI.Controllers
{
	/// <summary>
	/// 住所のWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class AddressController : ControllerBase
	{

		/// <summary>
		/// 住所の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		public int Count([FromQuery]AddressCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Address.Count() :
					db.Address.Count(predicate: c.CreatePredicate());
				return count;
			}
		}

		/// <summary>
		/// 住所の検索
		/// </summary>
		/// <param name="with_AddressType">AddressTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="c"></param>
		/// <returns></returns>
		[HttpGet("search")]
		public IEnumerable<Address> Search([FromQuery]bool with_AddressType, [FromQuery]bool with_Staff, [FromQuery]AddressCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Address;

				#region LoadWith
				if (with_AddressType)
					q = q.LoadWith(_ => _.AddressType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

				var list = (c == null ? q : q.Where(c.CreatePredicate())).ToList();
				return list;
			}
		}

		/// <summary>
		/// 住所の取得
		/// </summary>
		/// <param name="with_AddressType">AddressTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns></returns>
		[HttpGet("get/{userType}/{genericUserNo}/{seq}")]
		public Address Get([FromQuery]bool with_AddressType, [FromQuery]bool with_Staff, int userType, string genericUserNo, int seq)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Address;

				#region LoadWith
				if (with_AddressType)
					q = q.LoadWith(_ => _.AddressType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

				var o = q.Find(userType, genericUserNo, seq);
				return o;
			}
		}

		/// <summary>
		/// 住所の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns>uid</returns>
		[HttpPost("create")]
		public int Create([FromBody]Address o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int uid = db.InsertWithInt32Identity<Address>(o);
				return uid;
			}
		}

		/// <summary>
		/// 住所の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		public int Upsert([FromBody]Address o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<Address>(o);
				return count;
			}
		}

		/// <summary>
		/// 住所の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		public BulkCopyRowsCopied MassiveCreate([FromBody]IEnumerable<Address> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<Address>(os);
				return ret;
			}
		}

		/// <summary>
		/// 住所のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		public int Merge([FromBody]IEnumerable<Address> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<Address>(os);
				return count;
			}
		}

		/// <summary>
		/// 住所の更新
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{userType}/{genericUserNo}/{seq}")]
		public int Modify(int userType, string genericUserNo, int seq, [FromBody]Address o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<Address>(o);
				return count;
			}
		}

		/// <summary>
		/// 住所の削除(物理)
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns>件数</returns>
		[HttpDelete("remove/{userType}/{genericUserNo}/{seq}")]
		public int Remove(int userType, string genericUserNo, int seq)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Address
					.Where(_ => _.user_type == userType && _.generic_user_no == genericUserNo && _.seq == seq)
					.Delete();
				return count;
			}
		}

		/// <summary>
		/// 住所の削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("remove")]
		public int Remove([FromQuery]AddressCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Address
					.Where(c.CreatePredicate())
					.Delete();
				return count;
			}
		}

	}
}
