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
	/// 職員のWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class StaffController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// 職員の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns code="200">ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_Staff")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]StaffCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Staff.Count() :
					db.Staff.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 職員の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <param name="currentPage">ページ指定</param>
		/// <param name="pageSize">ページサイズ</param>
		/// <returns code="200">Staffのリスト</returns>
		[PermissionTypeAuthorize("Read_Staff")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Staff>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]StaffCondition c, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList, [FromQuery]string[] order, int currentPage = 1, int pageSize = 10)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Staff;

				#region LoadWith
				q = q
					.LoadWith(with_SexType, _ => _.SexType)
					.LoadWith(with_AccountList, _ => _.AccountList)
					.LoadWith(with_NameList, _ => _.NameList)
					.LoadWith(with_AddressList, _ => _.AddressList)
					.LoadWith(with_ContactList, _ => _.ContactList)
					;
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList());
			}
		}

		/// <summary>
		/// 職員の取得
		/// </summary>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="staffNo">職員番号(staff_no)</param>
		/// <returns code="200">Staffオブジェクト</returns>
		/// <returns code="404">無効な識別子</returns>
		[PermissionTypeAuthorize("Read_Staff")]
		[HttpGet("get/{staffNo}")]
		[ProducesResponseType(typeof(Staff), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(string staffNo, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Staff;

				#region LoadWith
				q = q
					.LoadWith(with_SexType, _ => _.SexType)
					.LoadWith(with_AccountList, _ => _.AccountList)
					.LoadWith(with_NameList, _ => _.NameList)
					.LoadWith(with_AddressList, _ => _.AddressList)
					.LoadWith(with_ContactList, _ => _.ContactList)
					;
				#endregion

				var o = q.Find(staffNo);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 職員の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Staffオブジェクト</returns>
		[PermissionTypeAuthorize("Create_Staff")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]Staff o)
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
					o.uid = db.InsertWithInt32Identity<Staff>(o);
					return CreatedAtAction(nameof(Get), new { staffNo = o.staff_no }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 職員の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="200">ヒットした件数</returns>
		/// <returns code="404"></returns>
		[PermissionTypeAuthorize("Create_Staff")]
		[PermissionTypeAuthorize("Update_Staff")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]Staff o)
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
					int count = db.InsertOrReplace<Staff>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 職員の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_Staff")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Staff> os)
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

					var ret = db.BulkCopy<Staff>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 職員のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_Staff")]
		[PermissionTypeAuthorize("Update_Staff")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<Staff> os)
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
					var count = db.Merge<Staff>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 職員の更新
		/// </summary>
		/// <param name="staffNo">職員番号(staff_no)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_Staff")]
		[HttpPut, Route("modify/{staffNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(string staffNo, [FromBody]Staff o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<Staff>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 職員の削除(論理)
		/// </summary>
		/// <param name="staffNo">職員番号(staff_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Staff")]
		[HttpDelete("remove/{staffNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(string staffNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Staff
					.Where(_ => _.staff_no == staffNo)
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 職員の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Staff")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]StaffCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Staff
					.Where(c.CreatePredicate())
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 職員の物理削除
		/// </summary>
		/// <param name="staffNo">職員番号(staff_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Staff")]
		[HttpDelete("physically-remove/{staffNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove(string staffNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Staff
					.Where(_ => _.staff_no == staffNo)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 職員の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_Staff")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove([FromQuery]StaffCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Staff
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
