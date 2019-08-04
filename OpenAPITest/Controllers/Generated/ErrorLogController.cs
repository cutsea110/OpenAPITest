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
	/// エラーログのWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class ErrorLogController : ControllerBase
	{

		/// <summary>
		/// エラーログの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[Authorize(Policy = "Read_ErrorLog")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]ErrorLogCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.ErrorLog.Count() :
					db.ErrorLog.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// エラーログの検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[Authorize(Policy = "Read_ErrorLog")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<ErrorLog>), 200)]
		public IActionResult Search([FromQuery]ErrorLogCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.ErrorLog;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// エラーログの取得
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[Authorize(Policy = "Read_ErrorLog")]
		[HttpGet("get/{uid}")]
		[ProducesResponseType(typeof(ErrorLog), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.ErrorLog;
				var o = q.Find(uid);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// エラーログの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">ErrorLogオブジェクト</returns>
		[Authorize(Policy = "Create_ErrorLog")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Create([FromBody]ErrorLog o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<ErrorLog>(o);
					return CreatedAtAction(nameof(Get), new { uid = o.uid }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// エラーログの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_ErrorLog")]
		[Authorize(Policy = "Update_ErrorLog")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Upsert([FromBody]ErrorLog o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<ErrorLog>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// エラーログの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[Authorize(Policy = "Create_ErrorLog")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		[ProducesResponseType(400)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<ErrorLog> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<ErrorLog>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// エラーログのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_ErrorLog")]
		[Authorize(Policy = "Update_ErrorLog")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Merge([FromBody]IEnumerable<ErrorLog> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<ErrorLog>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// エラーログの更新
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[Authorize(Policy = "Update_ErrorLog")]
		[HttpPut, Route("modify/{uid}")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Modify(int uid, [FromBody]ErrorLog o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<ErrorLog>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// エラーログの削除(論理)
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_ErrorLog")]
		[HttpDelete("remove/{uid}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.ErrorLog
					.Where(_ => _.uid == uid)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// エラーログの削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_ErrorLog")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]ErrorLogCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.ErrorLog
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// エラーログの物理削除
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_ErrorLog")]
		[HttpDelete("physically-remove/{uid}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.ErrorLog
					.Where(_ => _.uid == uid)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// エラーログの物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_ErrorLog")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove([FromQuery]ErrorLogCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.ErrorLog
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
