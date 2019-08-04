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
	/// 住所種別のWebAPI
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class AddressTypeController : ControllerBase
	{

		/// <summary>
		/// 住所種別の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[Authorize(Policy = "Read_AddressType")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Count([FromQuery]AddressTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.AddressType.Count() :
					db.AddressType.Count(predicate: c.CreatePredicate());
				return Ok(count);
			}
		}

		/// <summary>
		/// 住所種別の検索
		/// </summary>
		/// <param name="c"></param>
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[Authorize(Policy = "Read_AddressType")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<AddressType>), 200)]
		public IActionResult Search([FromQuery]AddressTypeCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.AddressType;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 住所種別の取得
		/// </summary>
		/// <param name="addressTypeId">住所種別ID(address_type_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[Authorize(Policy = "Read_AddressType")]
		[HttpGet("get/{addressTypeId}")]
		[ProducesResponseType(typeof(AddressType), 200)]
		[ProducesResponseType(404)]
		public IActionResult Get(int addressTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.AddressType;
				var o = q.Find(addressTypeId);
				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

		/// <summary>
		/// 住所種別の作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns code="201">AddressTypeオブジェクト</returns>
		[Authorize(Policy = "Create_AddressType")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), 201)]
		[ProducesResponseType(400)]
		public IActionResult Create([FromBody]AddressType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<AddressType>(o);
					return CreatedAtAction(nameof(Get), new { addressTypeId = o.address_type_id }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 住所種別の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_AddressType")]
		[Authorize(Policy = "Update_AddressType")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Upsert([FromBody]AddressType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<AddressType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 住所種別の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[Authorize(Policy = "Create_AddressType")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), 200)]
		[ProducesResponseType(400)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<AddressType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<AddressType>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 住所種別のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Create_AddressType")]
		[Authorize(Policy = "Update_AddressType")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Merge([FromBody]IEnumerable<AddressType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<AddressType>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 住所種別の更新
		/// </summary>
		/// <param name="addressTypeId">住所種別ID(address_type_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[Authorize(Policy = "Update_AddressType")]
		[HttpPut, Route("modify/{addressTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		[ProducesResponseType(400)]
		public IActionResult Modify(int addressTypeId, [FromBody]AddressType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<AddressType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 住所種別の削除(論理)
		/// </summary>
		/// <param name="addressTypeId">住所種別ID(address_type_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AddressType")]
		[HttpDelete("remove/{addressTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove(int addressTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AddressType
					.Where(_ => _.address_type_id == addressTypeId)
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 住所種別の削除(論理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AddressType")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult Remove([FromQuery]AddressTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AddressType
					.Where(c.CreatePredicate())
					.Set(_ => _.removed_at, Sql.CurrentTimestampUtc)
					.Update();
				return Ok(count);
			}
		}

		/// <summary>
		/// 住所種別の物理削除
		/// </summary>
		/// <param name="addressTypeId">住所種別ID(address_type_id)</param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AddressType")]
		[HttpDelete("physically-remove/{addressTypeId}")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove(int addressTypeId)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AddressType
					.Where(_ => _.address_type_id == addressTypeId)
					.Delete();
				return Ok(count);
			}
		}

		/// <summary>
		/// 住所種別の物理削除
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[Authorize(Policy = "Delete_AddressType")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PhysicallyRemove([FromQuery]AddressTypeCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.AddressType
					.Where(c.CreatePredicate())
					.Delete();
				return Ok(count);
			}
		}
	}
}
