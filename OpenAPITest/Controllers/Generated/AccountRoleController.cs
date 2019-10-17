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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LinqToDB;
using LinqToDB.Data;

using peppa.util;
using OpenAPITest.CustomPolicyProvider;
using OpenAPITest.CustomFilter;
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// アカウントロールのWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class AccountRoleController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// アカウントロールの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns code="200">ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_AccountRole")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		/// <param name="currentPage">ページ指定</param>
		/// <param name="pageSize">ページサイズ</param>
		/// <param name="p_when">この指定日時において有効なデータのみに限定する.null(デフォルト)なら限定しない.</param>
		/// <returns code="200">AccountRoleのリスト</returns>
		[PermissionTypeAuthorize("Read_AccountRole")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<AccountRole>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]AccountRoleCondition c, [FromQuery]bool with_Role, [FromQuery]string[] order, int currentPage = 1, int pageSize = 10, DateTime? p_when = null)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.AccountRole
					.LoadWith(with_Role, _ => _.Role)
					.IsActiveAt(p_when)
					;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;
				var result = ordered.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();


                return Ok(result);
			}
		}

		/// <summary>
		/// アカウントロールの取得
		/// </summary>
		/// <param name="with_Role">RoleをLoadWithするか</param>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <returns code="200">AccountRoleオブジェクト</returns>
		/// <returns code="404">無効な識別子</returns>
		[PermissionTypeAuthorize("Read_AccountRole")]
		[HttpGet("get/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(AccountRole), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
				q = q
					.LoadWith(with_Role, _ => _.Role)
					;
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
		[PermissionTypeAuthorize("Create_AccountRole")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.created_by = CurrentAccountId;
					o.modified_by = CurrentAccountId;
					o.uid = db.InsertWithInt32Identity<AccountRole>(o);
					return CreatedAtAction(nameof(Get), new { accountId = o.account_id, roleId = o.role_id }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// アカウントロールの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="200">ヒットした件数</returns>
		/// <returns code="404"></returns>
		[PermissionTypeAuthorize("Create_AccountRole")]
		[PermissionTypeAuthorize("Update_AccountRole")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					if (o.uid == 0)
						o.created_by = CurrentAccountId;
					o.modified_by = CurrentAccountId;
					int count = db.InsertOrReplace<AccountRole>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// アカウントロールの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_AccountRole")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<AccountRole> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					foreach (var o in os)
					{
						o.created_by = CurrentAccountId;
						o.modified_by = CurrentAccountId;
					}

					var ret = db.BulkCopy<AccountRole>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// アカウントロールのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_AccountRole")]
		[PermissionTypeAuthorize("Update_AccountRole")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<AccountRole> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					foreach (var o in os)
					{
						if (o.uid == 0)
							o.created_by = CurrentAccountId;
						o.modified_by = CurrentAccountId;
					}
					var count = db.Merge<AccountRole>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// アカウントロールの更新
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_AccountRole")]
		[HttpPut, Route("modify/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int accountId, string roleId, [FromBody]AccountRole o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<AccountRole>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// アカウントロールの削除(物理)
		/// </summary>
		/// <param name="accountId">アカウントID(account_id)</param>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_AccountRole")]
		[HttpDelete("remove/{accountId}/{roleId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		[PermissionTypeAuthorize("Delete_AccountRole")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
