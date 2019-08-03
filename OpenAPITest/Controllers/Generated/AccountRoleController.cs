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
	/// アカウントロールのWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class AccountRoleController : ControllerBase
	{

		/// <summary>
		/// アカウントロールの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[Authorize(Policy = "Read_AccountRole")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]AccountRoleCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.AccountRole.Count() :
					db.AccountRole.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントロールの検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_Role">RoleをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[Authorize(Policy = "Read_AccountRole")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<AccountRole>), 200)]
		public IActionResult Search([FromQuery]AccountRoleCondition c, [FromQuery]bool with_Role, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.AccountRole;

				#region LoadWith
				if (with_Role)
					q = q.LoadWith(_ => _.Role);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// アカウントロールの取得
		/// </summary>
		/// <param name="with_Role">RoleをLoadWithするか</param>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[Authorize(Policy = "Read_AccountRole")]
		[HttpGet("get/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(AccountRole), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int accountId, string roleId, [FromQuery]bool with_Role)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.AccountRole;

				#region LoadWith
				if (with_Role)
					q = q.LoadWith(_ => _.Role);
				#endregion

				var o = q.Find(accountId, roleId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// アカウントロールの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">AccountRoleオブジェクト</returns>
		[Authorize(Policy = "Create_AccountRole")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<AccountRole>(o);
                return CreatedAtAction(nameof(Get), new { accountId = o.account_id, roleId = o.role_id }, o);
			}
		}

		/// <summary>
		/// アカウントロールの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_AccountRole")]
		[Authorize(Policy = "Update_AccountRole")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<AccountRole>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントロールの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[Authorize(Policy = "Create_AccountRole")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<AccountRole> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<AccountRole>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// アカウントロールのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_AccountRole")]
		[Authorize(Policy = "Update_AccountRole")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<AccountRole> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<AccountRole>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントロールの更新
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[Authorize(Policy = "Update_AccountRole")]
		[HttpPut, Route("modify/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int accountId, string roleId, [FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<AccountRole>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントロールの削除(物理)
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AccountRole")]
		[HttpDelete("remove/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(int accountId, string roleId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AccountRole
					.Where(_ => _.account_id == accountId && _.role_id == roleId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// アカウントロールの削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AccountRole")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]AccountRoleCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AccountRole
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}

	}
}
