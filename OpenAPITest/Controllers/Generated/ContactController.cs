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
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;

using peppa.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// 連絡先のWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class ContactController : ControllerBase
	{

		/// <summary>
		/// 連絡先の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
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
		/// <param name="with_ContactType">ContactTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="c"></param>
		/// <returns></returns>
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<Contact>), 200)]
		public IActionResult Search([FromQuery]bool with_ContactType, [FromQuery]bool with_Staff, [FromQuery]ContactCondition c)
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

				var list = (c == null ? q : q.Where(c.CreatePredicate())).ToList();
				return Ok(list);
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
		[HttpGet("get/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(Contact), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get([FromQuery]bool with_ContactType, [FromQuery]bool with_Staff, int userType, string genericUserNo, int seq)
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
		/// <returns>uid</returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]Contact o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<Contact>(o);
                return CreatedAtAction(nameof(Get), new { userType = o.user_type, genericUserNo = o.generic_user_no, seq = o.seq }, o);
			}
		}

		/// <summary>
		/// 連絡先の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]Contact o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<Contact>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 連絡先の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<Contact> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<Contact>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// 連絡先のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<Contact> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<Contact>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// 連絡先の更新
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int userType, string genericUserNo, int seq, [FromBody]Contact o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<Contact>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 連絡先の削除(物理)
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns>件数</returns>
		[HttpDelete("remove/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), 200)]
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
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
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
