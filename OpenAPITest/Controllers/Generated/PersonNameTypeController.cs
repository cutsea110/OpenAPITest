//---------------------------------------------------------------------------------------------------
// <auto-generated>
//	This code was generated by peppapig from database table definition.
//	Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
	/// 人名種別のWebAPI
	/// </summary>
    [ServiceFilter(typeof(ClientIpCheckFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public partial class PersonNameTypeController : ControllerBase
	{

		/// <summary>
		/// 人名種別の件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[PermissionTypeAuthorize("Read_PersonNameType")]
		[HttpGet("count")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		/// <param name="order">Prop0[.Prop1.Prop2...] [Asc|Desc], ...</param>
		/// <returns></returns>
		[PermissionTypeAuthorize("Read_PersonNameType")]
		[HttpGet("search")]
		[ProducesResponseType(typeof(IEnumerable<PersonNameType>), StatusCodes.Status200OK)]
		public IActionResult Search([FromQuery]PersonNameTypeCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.PersonNameType;
                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}

		/// <summary>
		/// 人名種別の取得
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[PermissionTypeAuthorize("Read_PersonNameType")]
		[HttpGet("get/{personNameTypeId}")]
		[ProducesResponseType(typeof(PersonNameType), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
		/// <returns code="201">PersonNameTypeオブジェクト</returns>
		[PermissionTypeAuthorize("Create_PersonNameType")]
		[HttpPost("create")]
		[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Create([FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					o.uid = db.InsertWithInt32Identity<PersonNameType>(o);
					return CreatedAtAction(nameof(Get), new { personNameTypeId = o.person_name_type_id }, o);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名種別の更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_PersonNameType")]
		[PermissionTypeAuthorize("Update_PersonNameType")]
		[HttpPost("upsert")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Upsert([FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					int count = db.InsertOrReplace<PersonNameType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名種別の一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[PermissionTypeAuthorize("Create_PersonNameType")]
		[HttpPost("massive-new")]
		[ProducesResponseType(typeof(BulkCopyRowsCopied), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult MassiveCreate([FromBody]IEnumerable<PersonNameType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var ret = db.BulkCopy<PersonNameType>(os);
					return Ok(ret);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名種別のマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Create_PersonNameType")]
		[PermissionTypeAuthorize("Update_PersonNameType")]
		[HttpPost("merge")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Merge([FromBody]IEnumerable<PersonNameType> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Merge<PersonNameType>(os);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名種別の更新
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[PermissionTypeAuthorize("Update_PersonNameType")]
		[HttpPut, Route("modify/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Modify(int personNameTypeId, [FromBody]PersonNameType o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			if (ModelState.IsValid) {
				using (var db = new peppaDB())
				{
					var count = db.Update<PersonNameType>(o);
					return Ok(count);
				}
			}
			return BadRequest();
		}

		/// <summary>
		/// 人名種別の削除(論理)
		/// </summary>
		/// <param name="personNameTypeId">人名種別ID(person_name_type_id)</param>
		/// <returns>件数</returns>
		[PermissionTypeAuthorize("Delete_PersonNameType")]
		[HttpDelete("remove/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		[PermissionTypeAuthorize("Delete_PersonNameType")]
		[HttpDelete("remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		[PermissionTypeAuthorize("Delete_PersonNameType")]
		[HttpDelete("physically-remove/{personNameTypeId}")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
		[PermissionTypeAuthorize("Delete_PersonNameType")]
		[HttpDelete("physically-remove")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
