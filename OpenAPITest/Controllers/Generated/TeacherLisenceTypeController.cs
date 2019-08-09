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
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// 教員資格種別のWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class TeacherLisenceTypeController : ControllerBase
	{

		/// <summary>
		/// 教員資格種別の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[Authorize(Policy = "Read_TeacherLisenceType")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]TeacherLisenceTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.TeacherLisenceType.Count() :
					db.TeacherLisenceType.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員資格種別の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[Authorize(Policy = "Read_TeacherLisenceType")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<TeacherLisenceType>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]TeacherLisenceTypeCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.TeacherLisenceType;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 教員資格種別の取得
		/// </summary>
		/// <param name="teacherLisenceTypeId">教員資格種別ID(teacher_lisence_type_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[Authorize(Policy = "Read_TeacherLisenceType")]
		[HttpGet("get/{teacherLisenceTypeId}")]
		[ProducesResponseType(typeof(TeacherLisenceType), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int teacherLisenceTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.TeacherLisenceType;
				var o = q.Find(teacherLisenceTypeId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 教員資格種別の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">TeacherLisenceTypeオブジェクト</returns>
		[Authorize(Policy = "Create_TeacherLisenceType")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]TeacherLisenceType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<TeacherLisenceType>(o);
					return CreatedAtAction(nameof(Get), new { teacherLisenceTypeId = o.teacher_lisence_type_id }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員資格種別の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_TeacherLisenceType")]
		[Authorize(Policy = "Update_TeacherLisenceType")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]TeacherLisenceType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<TeacherLisenceType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員資格種別の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[Authorize(Policy = "Create_TeacherLisenceType")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<TeacherLisenceType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<TeacherLisenceType>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員資格種別のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_TeacherLisenceType")]
		[Authorize(Policy = "Update_TeacherLisenceType")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<TeacherLisenceType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<TeacherLisenceType>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員資格種別の更新
		/// </summary>
		/// <param name="teacherLisenceTypeId">教員資格種別ID(teacher_lisence_type_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[Authorize(Policy = "Update_TeacherLisenceType")]
		[HttpPut, Route("modify/{teacherLisenceTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int teacherLisenceTypeId, [FromBody]TeacherLisenceType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<TeacherLisenceType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員資格種別の削除(論理)
		/// </summary>
		/// <param name="teacherLisenceTypeId">教員資格種別ID(teacher_lisence_type_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_TeacherLisenceType")]
		[HttpDelete("remove/{teacherLisenceTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(int teacherLisenceTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.TeacherLisenceType
					.Where(_ => _.teacher_lisence_type_id == teacherLisenceTypeId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員資格種別の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_TeacherLisenceType")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]TeacherLisenceTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.TeacherLisenceType
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員資格種別の物理削除
		/// </summary>
		/// <param name="teacherLisenceTypeId">教員資格種別ID(teacher_lisence_type_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_TeacherLisenceType")]
		[HttpDelete("physically-remove/{teacherLisenceTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove(int teacherLisenceTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.TeacherLisenceType
					.Where(_ => _.teacher_lisence_type_id == teacherLisenceTypeId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員資格種別の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_TeacherLisenceType")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove([FromQuery]TeacherLisenceTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.TeacherLisenceType
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
