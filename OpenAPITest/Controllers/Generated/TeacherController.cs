//---------------------------------------------------------------------------------------------------
// <auto-generated>
//	This code was generated by peppapig from database table definition.
//	Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
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
	/// 教員のWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class TeacherController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// 教員の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_Teacher")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]TeacherCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Teacher.Count() :
					db.Teacher.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_WorkStyle">WorkStyleをLoadWithするか</param>
		/// <param name="with_Position">PositionをLoadWithするか</param>
		/// <param name="with_TeacherLisence">TeacherLisenceをLoadWithするか</param>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[PermissionTypeAuthorize("Read_Teacher")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Teacher>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]TeacherCondition c, [FromQuery]bool with_WorkStyle, [FromQuery]bool with_Position, [FromQuery]bool with_TeacherLisence, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Teacher;

				#region LoadWith
				if (with_WorkStyle)
					q = q.LoadWith(_ => _.WorkStyle);
				if (with_Position)
					q = q.LoadWith(_ => _.Position);
				if (with_TeacherLisence)
					q = q.LoadWith(_ => _.TeacherLisence);
				if (with_SexType)
					q = q.LoadWith(_ => _.SexType);
				if (with_AccountList)
					q = q.LoadWith(_ => _.AccountList);
				if (with_NameList)
					q = q.LoadWith(_ => _.NameList);
				if (with_AddressList)
					q = q.LoadWith(_ => _.AddressList);
				if (with_ContactList)
					q = q.LoadWith(_ => _.ContactList);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 教員の取得
		/// </summary>
		/// <param name="with_WorkStyle">WorkStyleをLoadWithするか</param>
		/// <param name="with_Position">PositionをLoadWithするか</param>
		/// <param name="with_TeacherLisence">TeacherLisenceをLoadWithするか</param>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="teacherNo">教員番号(teacher_no)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[PermissionTypeAuthorize("Read_Teacher")]
		[HttpGet("get/{teacherNo}")]
		[ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(string teacherNo, [FromQuery]bool with_WorkStyle, [FromQuery]bool with_Position, [FromQuery]bool with_TeacherLisence, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Teacher;

				#region LoadWith
				if (with_WorkStyle)
					q = q.LoadWith(_ => _.WorkStyle);
				if (with_Position)
					q = q.LoadWith(_ => _.Position);
				if (with_TeacherLisence)
					q = q.LoadWith(_ => _.TeacherLisence);
				if (with_SexType)
					q = q.LoadWith(_ => _.SexType);
				if (with_AccountList)
					q = q.LoadWith(_ => _.AccountList);
				if (with_NameList)
					q = q.LoadWith(_ => _.NameList);
				if (with_AddressList)
					q = q.LoadWith(_ => _.AddressList);
				if (with_ContactList)
					q = q.LoadWith(_ => _.ContactList);
				#endregion

				var o = q.Find(teacherNo);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 教員の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Teacherオブジェクト</returns>
		[PermissionTypeAuthorize("Create_Teacher")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]Teacher o)
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
					o.uid = db.InsertWithInt32Identity<Teacher>(o);
					return CreatedAtAction(nameof(Get), new { teacherNo = o.teacher_no }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Teacher")]
		[PermissionTypeAuthorize("Update_Teacher")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]Teacher o)
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
					int count = db.InsertOrReplace<Teacher>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_Teacher")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Teacher> os)
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

					var ret = db.BulkCopy<Teacher>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Teacher")]
		[PermissionTypeAuthorize("Update_Teacher")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<Teacher> os)
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
					var count = db.Merge<Teacher>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員の更新
		/// </summary>
		/// <param name="teacherNo">教員番号(teacher_no)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_Teacher")]
		[HttpPut, Route("modify/{teacherNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(string teacherNo, [FromBody]Teacher o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<Teacher>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 教員の削除(論理)
		/// </summary>
		/// <param name="teacherNo">教員番号(teacher_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Teacher")]
		[HttpDelete("remove/{teacherNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(string teacherNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Teacher
					.Where(_ => _.teacher_no == teacherNo)
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Teacher")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]TeacherCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Teacher
					.Where(c.CreatePredicate())
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員の物理削除
		/// </summary>
		/// <param name="teacherNo">教員番号(teacher_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Teacher")]
		[HttpDelete("physically-remove/{teacherNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove(string teacherNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Teacher
					.Where(_ => _.teacher_no == teacherNo)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 教員の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Teacher")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove([FromQuery]TeacherCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Teacher
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
