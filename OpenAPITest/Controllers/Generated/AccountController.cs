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

using peppa.util;
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// アカウントのWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class AccountController : ControllerBase
	{

		/// <summary>
		/// アカウントの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]AccountCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Account.Count() :
					db.Account.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="with_AccountRoleList">AccountRoleListをLoadWithするか</param>
		/// <param name="with_PasswordList">PasswordListをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Account>), 200)]
		public IActionResult Search([FromQuery]AccountCondition c, [FromQuery]bool with_Staff, [FromQuery]bool with_AccountRoleList, [FromQuery]bool with_PasswordList, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Account;

				#region LoadWith
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				if (with_AccountRoleList)
					q = q.LoadWith(_ => _.AccountRoleList);
				if (with_PasswordList)
					q = q.LoadWith(_ => _.PasswordList);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// アカウントの取得
		/// </summary>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="with_AccountRoleList">AccountRoleListをLoadWithするか</param>
		/// <param name="with_PasswordList">PasswordListをLoadWithするか</param>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[HttpGet("get/{accountId}")]
		[ProducesResponseType(typeof(Account), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int accountId, [FromQuery]bool with_Staff, [FromQuery]bool with_AccountRoleList, [FromQuery]bool with_PasswordList)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Account;

				#region LoadWith
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				if (with_AccountRoleList)
					q = q.LoadWith(_ => _.AccountRoleList);
				if (with_PasswordList)
					q = q.LoadWith(_ => _.PasswordList);
				#endregion

				var o = q.Find(accountId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// アカウントの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Accountオブジェクト</returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]Account o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<Account>(o);
                return CreatedAtAction(nameof(Get), new { accountId = o.account_id }, o);
			}
		}

		/// <summary>
		/// アカウントの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]Account o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<Account>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Account> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<Account>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// アカウントのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<Account> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<Account>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの更新
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{accountId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int accountId, [FromBody]Account o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<Account>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの削除(論理)
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <returns>件数</returns>
		[HttpDelete("remove/{accountId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(int accountId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Account
					.Where(_ => _.account_id == accountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]AccountCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Account
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの物理削除
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <returns>件数</returns>
		[HttpDelete("physically-remove/{accountId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove(int accountId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Account
					.Where(_ => _.account_id == accountId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントの物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove([FromQuery]AccountCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Account
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
