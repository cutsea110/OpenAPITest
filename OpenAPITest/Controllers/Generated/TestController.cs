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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LinqToDB;
using LinqToDB.Data;

using peppa.util;
using OpenAPITest.CustomPolicyProvider;
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// テストのWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class TestController : ControllerBase
	{

		/// <summary>
		/// テストの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_Test")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]TestCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Test.Count() :
					db.Test.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// テストの検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[PermissionTypeAuthorize("Read_Test")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Test>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]TestCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Test;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// テストの取得
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[PermissionTypeAuthorize("Read_Test")]
		[HttpGet("get/{uid}")]
		[ProducesResponseType(typeof(Test), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Test;
				var o = q.Find(uid);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// テストの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Testオブジェクト</returns>
		[PermissionTypeAuthorize("Create_Test")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<Test>(o);
					return CreatedAtAction(nameof(Get), new { uid = o.uid }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// テストの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Test")]
		[PermissionTypeAuthorize("Update_Test")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<Test>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// テストの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_Test")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Test> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<Test>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// テストのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Test")]
		[PermissionTypeAuthorize("Update_Test")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<Test> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<Test>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// テストの更新
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_Test")]
		[HttpPut, Route("modify/{uid}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int uid, [FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<Test>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// テストの削除(物理)
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Test")]
		[HttpDelete("remove/{uid}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Test
					.Where(_ => _.uid == uid)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// テストの削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Test")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]TestCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Test
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}

	}
}
