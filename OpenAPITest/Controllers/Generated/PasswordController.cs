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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LinqToDB;
using LinqToDB.Data;

using peppa.util;
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// パスワード認証のWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class PasswordController : ControllerBase
	{

		/// <summary>
		/// パスワード認証の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]PasswordCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Password.Count() :
					db.Password.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_Account">AccountをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Password>), 200)]
		public IActionResult Search([FromQuery]PasswordCondition c, [FromQuery]bool with_Account, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Password;

				#region LoadWith
				if (with_Account)
					q = q.LoadWith(_ => _.Account);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// パスワード認証の取得
		/// </summary>
		/// <param name="with_Account">AccountをLoadWithするか</param>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[HttpGet("get/{accountId}")]
		[ProducesResponseType(typeof(Password), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int accountId, [FromQuery]bool with_Account)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Password;

				#region LoadWith
				if (with_Account)
					q = q.LoadWith(_ => _.Account);
				#endregion

				var o = q.Find(accountId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// パスワード認証の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Passwordオブジェクト</returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]Password o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<Password>(o);
                return CreatedAtAction(nameof(Get), new { accountId = o.account_id }, o);
			}
		}

		/// <summary>
		/// パスワード認証の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]Password o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<Password>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Password> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<Password>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// パスワード認証のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<Password> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<Password>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の更新
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{accountId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int accountId, [FromBody]Password o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<Password>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の削除(論理)
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
				var count = db.Password
					.Where(_ => _.account_id == accountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]PasswordCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Password
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の物理削除
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
				var count = db.Password
					.Where(_ => _.account_id == accountId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// パスワード認証の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove([FromQuery]PasswordCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Password
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
