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
	/// 人名のWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class PersonNameController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// 人名の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns code="200">ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_PersonName")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]PersonNameCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.PersonName.Count() :
					db.PersonName.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_PersonNameType">PersonNameTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="with_Teacher">TeacherをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <param name="currentPage">ページ指定</param>
		/// <param name="pageSize">ページサイズ</param>
		/// <param name="p_when">この指定日時において有効なデータのみに限定する.null(デフォルト)なら限定しない.</param>
		/// <returns code="200">PersonNameのリスト</returns>
		[PermissionTypeAuthorize("Read_PersonName")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<PersonName>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]PersonNameCondition c, [FromQuery]bool with_PersonNameType, [FromQuery]bool with_Staff, [FromQuery]bool with_Teacher, [FromQuery]string[] order, int currentPage = 1, int pageSize = 10, DateTime? p_when = null)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonName
					.LoadWith(with_PersonNameType, _ => _.PersonNameType)
					.LoadWith(with_Staff, _ => _.Staff)
					.LoadWith(with_Teacher, _ => _.Teacher)
					.IsActiveAt(p_when)
					;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;
				var result = ordered.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                return Ok(result);
			}
		}

		/// <summary>
		/// 人名の取得
		/// </summary>
		/// <param name="with_PersonNameType">PersonNameTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="with_Teacher">TeacherをLoadWithするか</param>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns code="200">PersonNameオブジェクト</returns>
		/// <returns code="404">無効な識別子</returns>
		[PermissionTypeAuthorize("Read_PersonName")]
		[HttpGet("get/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(PersonName), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int userType, string genericUserNo, int seq, [FromQuery]bool with_PersonNameType, [FromQuery]bool with_Staff, [FromQuery]bool with_Teacher)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonName
					.LoadWith(with_PersonNameType, _ => _.PersonNameType)
					.LoadWith(with_Staff, _ => _.Staff)
					.LoadWith(with_Teacher, _ => _.Teacher)
					;
				var o = q.Find(userType, genericUserNo, seq);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 人名の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">PersonNameオブジェクト</returns>
		[PermissionTypeAuthorize("Create_PersonName")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]PersonName o)
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
					o.uid = db.InsertWithInt32Identity<PersonName>(o);
					return CreatedAtAction(nameof(Get), new { userType = o.user_type, genericUserNo = o.generic_user_no, seq = o.seq }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="200">ヒットした件数</returns>
		/// <returns code="404"></returns>
		[PermissionTypeAuthorize("Create_PersonName")]
		[PermissionTypeAuthorize("Update_PersonName")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]PersonName o)
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
					int count = db.InsertOrReplace<PersonName>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_PersonName")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<PersonName> os)
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

					var ret = db.BulkCopy<PersonName>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_PersonName")]
		[PermissionTypeAuthorize("Update_PersonName")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<PersonName> os)
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
					var count = db.Merge<PersonName>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名の更新
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_PersonName")]
		[HttpPut, Route("modify/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int userType, string genericUserNo, int seq, [FromBody]PersonName o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<PersonName>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名の削除(物理)
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_PersonName")]
		[HttpDelete("remove/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(int userType, string genericUserNo, int seq)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonName
					.Where(_ => _.user_type == userType && _.generic_user_no == genericUserNo && _.seq == seq)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名の削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_PersonName")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]PersonNameCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonName
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}

	}
}
