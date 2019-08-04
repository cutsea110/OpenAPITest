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
	/// ロール権限のWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class RolePermissionController : ControllerBase
	{

		/// <summary>
		/// ロール権限の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[Authorize(Policy = "Read_RolePermission")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]RolePermissionCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.RolePermission.Count() :
					db.RolePermission.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// ロール権限の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[Authorize(Policy = "Read_RolePermission")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<RolePermission>), 200)]
		public IActionResult Search([FromQuery]RolePermissionCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.RolePermission;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// ロール権限の取得
		/// </summary>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <param name="permissionId">権限ID(permission_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[Authorize(Policy = "Read_RolePermission")]
		[HttpGet("get/{roleId}/{permissionId}")]
		[ProducesResponseType(typeof(RolePermission), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(string roleId, string permissionId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.RolePermission;
				var o = q.Find(roleId, permissionId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// ロール権限の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">RolePermissionオブジェクト</returns>
		[Authorize(Policy = "Create_RolePermission")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Create([FromBody]RolePermission o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<RolePermission>(o);
					return CreatedAtAction(nameof(Get), new { roleId = o.role_id, permissionId = o.permission_id }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// ロール権限の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_RolePermission")]
		[Authorize(Policy = "Update_RolePermission")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Upsert([FromBody]RolePermission o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<RolePermission>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// ロール権限の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[Authorize(Policy = "Create_RolePermission")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		[ProducesResponseType(400)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<RolePermission> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<RolePermission>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// ロール権限のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_RolePermission")]
		[Authorize(Policy = "Update_RolePermission")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Merge([FromBody]IEnumerable<RolePermission> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<RolePermission>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// ロール権限の更新
		/// </summary>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <param name="permissionId">権限ID(permission_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[Authorize(Policy = "Update_RolePermission")]
		[HttpPut, Route("modify/{roleId}/{permissionId}")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Modify(string roleId, string permissionId, [FromBody]RolePermission o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<RolePermission>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// ロール権限の削除(物理)
		/// </summary>
		/// <param name="roleId">ロールID(role_id)</param>
		/// <param name="permissionId">権限ID(permission_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_RolePermission")]
		[HttpDelete("remove/{roleId}/{permissionId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(string roleId, string permissionId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.RolePermission
					.Where(_ => _.role_id == roleId && _.permission_id == permissionId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// ロール権限の削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_RolePermission")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]RolePermissionCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.RolePermission
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}

	}
}
