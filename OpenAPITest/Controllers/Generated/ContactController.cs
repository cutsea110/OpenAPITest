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
	/// 連絡先のWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class ContactController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// 連絡先の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_Contact")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]ContactCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Contact.Count() :
					db.Contact.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 連絡先の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_ContactType">ContactTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[PermissionTypeAuthorize("Read_Contact")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]ContactCondition c, [FromQuery]bool with_ContactType, [FromQuery]bool with_Staff, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Contact;

				#region LoadWith
				if (with_ContactType)
					q = q.LoadWith(_ => _.ContactType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 連絡先の取得
		/// </summary>
		/// <param name="with_ContactType">ContactTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[PermissionTypeAuthorize("Read_Contact")]
		[HttpGet("get/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int userType, string genericUserNo, int seq, [FromQuery]bool with_ContactType, [FromQuery]bool with_Staff)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Contact;

				#region LoadWith
				if (with_ContactType)
					q = q.LoadWith(_ => _.ContactType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

				var o = q.Find(userType, genericUserNo, seq);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 連絡先の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Contactオブジェクト</returns>
		[PermissionTypeAuthorize("Create_Contact")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]Contact o)
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
					o.uid = db.InsertWithInt32Identity<Contact>(o);
					return CreatedAtAction(nameof(Get), new { userType = o.user_type, genericUserNo = o.generic_user_no, seq = o.seq }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 連絡先の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Contact")]
		[PermissionTypeAuthorize("Update_Contact")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]Contact o)
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
					int count = db.InsertOrReplace<Contact>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 連絡先の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_Contact")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Contact> os)
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

					var ret = db.BulkCopy<Contact>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 連絡先のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Contact")]
		[PermissionTypeAuthorize("Update_Contact")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<Contact> os)
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
					var count = db.Merge<Contact>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 連絡先の更新
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_Contact")]
		[HttpPut, Route("modify/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int userType, string genericUserNo, int seq, [FromBody]Contact o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<Contact>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 連絡先の削除(物理)
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Contact")]
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
				var count = db.Contact
					.Where(_ => _.user_type == userType && _.generic_user_no == genericUserNo && _.seq == seq)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 連絡先の削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Contact")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]ContactCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Contact
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}

	}
}
