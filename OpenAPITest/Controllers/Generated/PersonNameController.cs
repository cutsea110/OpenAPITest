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

using peppa.util;
using OpenAPITest.Domain;

namespace OpenAPITest.Controllers
{
	/// <summary>
	/// 人名のWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class PersonNameController : ControllerBase
	{

		/// <summary>
		/// 人名の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
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
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<PersonName>), 200)]
		public IActionResult Search([FromQuery]PersonNameCondition c, [FromQuery]bool with_PersonNameType, [FromQuery]bool with_Staff, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonName;

				#region LoadWith
				if (with_PersonNameType)
					q = q.LoadWith(_ => _.PersonNameType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 人名の取得
		/// </summary>
		/// <param name="with_PersonNameType">PersonNameTypeをLoadWithするか</param>
		/// <param name="with_Staff">StaffをLoadWithするか</param>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[HttpGet("get/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(PersonName), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int userType, string genericUserNo, int seq, [FromQuery]bool with_PersonNameType, [FromQuery]bool with_Staff)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonName;

				#region LoadWith
				if (with_PersonNameType)
					q = q.LoadWith(_ => _.PersonNameType);
				if (with_Staff)
					q = q.LoadWith(_ => _.Staff);
				#endregion

				var o = q.Find(userType, genericUserNo, seq);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 人名の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">PersonNameオブジェクト</returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]PersonName o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<PersonName>(o);
                return CreatedAtAction(nameof(Get), new { userType = o.user_type, genericUserNo = o.generic_user_no, seq = o.seq }, o);
			}
		}

		/// <summary>
		/// 人名の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]PersonName o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<PersonName>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<PersonName> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<PersonName>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// 人名のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<PersonName> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<PersonName>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名の更新
		/// </summary>
		/// <param name="userType">利用者種別(user_type)</param>
		/// <param name="genericUserNo">利用者番号(generic_user_no)</param>
		/// <param name="seq">連番(seq)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{userType}/{genericUserNo}/{seq}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int userType, string genericUserNo, int seq, [FromBody]PersonName o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<PersonName>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名の削除(物理)
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
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
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
