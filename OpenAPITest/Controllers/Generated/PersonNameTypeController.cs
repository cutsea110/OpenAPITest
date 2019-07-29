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
	/// 人名種別のWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class PersonNameTypeController : ControllerBase
	{

		/// <summary>
		/// 人名種別の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]PersonNameTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.PersonNameType.Count() :
					db.PersonNameType.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の検索
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<PersonNameType>), 200)]
		public IActionResult Search([FromQuery]PersonNameTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonNameType;
				var list = (c == null ? q : q.Where(c.CreatePredicate())).ToList();
				return Ok(list);
			}
		}

		/// <summary>
		/// 人名種別の取得
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[HttpGet("get/{personNameTypeId}")]
		[ProducesResponseType(typeof(PersonNameType), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int personNameTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonNameType;
				var o = q.Find(personNameTypeId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 人名種別の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns>uid</returns>
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Create([FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				o.uid = db.InsertWithInt32Identity<PersonNameType>(o);
                return CreatedAtAction(nameof(Get), new { personNameTypeId = o.person_name_type_id }, o);
			}
		}

		/// <summary>
		/// 人名種別の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Upsert([FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<PersonNameType>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<PersonNameType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<PersonNameType>(os);
				return Ok(ret);
			}
		}

		/// <summary>
		/// 人名種別のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Merge([FromBody]IEnumerable<PersonNameType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<PersonNameType>(os);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の更新
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Modify(int personNameTypeId, [FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<PersonNameType>(o);
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の削除(論理)
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <returns>件数</returns>
		[HttpDelete("remove/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(int personNameTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonNameType
					.Where(_ => _.person_name_type_id == personNameTypeId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]PersonNameTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonNameType
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の物理削除
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <returns>件数</returns>
		[HttpDelete("physically-remove/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove(int personNameTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonNameType
					.Where(_ => _.person_name_type_id == personNameTypeId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 人名種別の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove([FromQuery]PersonNameTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.PersonNameType
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
