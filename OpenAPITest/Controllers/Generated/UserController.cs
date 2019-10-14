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
	/// 他ユーザのWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class UserController : ControllerBase
	{
        /// <summary>
        /// Current Account ID
        /// </summary>
        public int CurrentAccountId => int.Parse(this.User.FindFirst(ClaimTypes.Name).Value);

		/// <summary>
		/// 他ユーザの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns code="200">ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_User")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Count([FromQuery]UserCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.User.Count() :
					db.User.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 他ユーザの検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns code="200">Userのリスト</returns>
		[PermissionTypeAuthorize("Read_User")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]UserCondition c, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.User;

				#region LoadWith
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
		/// 他ユーザの取得
		/// </summary>
		/// <param name="with_SexType">SexTypeをLoadWithするか</param>
		/// <param name="with_AccountList">AccountListをLoadWithするか</param>
		/// <param name="with_NameList">NameListをLoadWithするか</param>
		/// <param name="with_AddressList">AddressListをLoadWithするか</param>
		/// <param name="with_ContactList">ContactListをLoadWithするか</param>
		/// <param name="userNo">利用者番号(user_no)</param>
		/// <returns code="200">Userオブジェクト</returns>
		/// <returns code="404">無効な識別子</returns>
		[PermissionTypeAuthorize("Read_User")]
		[HttpGet("get/{userNo}")]
		[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(string userNo, [FromQuery]bool with_SexType, [FromQuery]bool with_AccountList, [FromQuery]bool with_NameList, [FromQuery]bool with_AddressList, [FromQuery]bool with_ContactList)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.User;

				#region LoadWith
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

				var o = q.Find(userNo);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 他ユーザの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">Userオブジェクト</returns>
		[PermissionTypeAuthorize("Create_User")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]User o)
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
					o.uid = db.InsertWithInt32Identity<User>(o);
					return CreatedAtAction(nameof(Get), new { userNo = o.user_no }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 他ユーザの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="200">ヒットした件数</returns>
		/// <returns code="404"></returns>
		[PermissionTypeAuthorize("Create_User")]
		[PermissionTypeAuthorize("Update_User")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]User o)
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
					int count = db.InsertOrReplace<User>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 他ユーザの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_User")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<User> os)
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

					var ret = db.BulkCopy<User>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 他ユーザのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_User")]
		[PermissionTypeAuthorize("Update_User")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<User> os)
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
					var count = db.Merge<User>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 他ユーザの更新
		/// </summary>
		/// <param name="userNo">利用者番号(user_no)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_User")]
		[HttpPut, Route("modify/{userNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(string userNo, [FromBody]User o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.modified_by = CurrentAccountId;
					var count = db.Update<User>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 他ユーザの削除(論理)
		/// </summary>
		/// <param name="userNo">利用者番号(user_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_User")]
		[HttpDelete("remove/{userNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove(string userNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.User
					.Where(_ => _.user_no == userNo)
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 他ユーザの削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_User")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult Remove([FromQuery]UserCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.User
					.Where(c.CreatePredicate())
					.Set(_ => _.modified_by, CurrentAccountId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 他ユーザの物理削除
		/// </summary>
		/// <param name="userNo">利用者番号(user_no)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_User")]
		[HttpDelete("physically-remove/{userNo}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove(string userNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.User
					.Where(_ => _.user_no == userNo)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 他ユーザの物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_User")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		public IActionResult PhysicallyRemove([FromQuery]UserCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.User
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
