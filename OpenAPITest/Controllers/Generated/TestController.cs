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

namespace MockWebAPI.Controllers
{
	/// <summary>
	/// テストのWebAPI
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public partial class TestController : ControllerBase
	{

		/// <summary>
		/// テストの件数
		/// </summary>
		/// <param name="c"></param>
		/// <returns>ヒットした件数</returns>
		[HttpGet("count")]
		public int Count([FromQuery]TestCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count =
					c == null ? db.Test.Count() :
					db.Test.Count(predicate: c.CreatePredicate());
				return count;
			}
		}

		/// <summary>
		/// テストの検索
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		[HttpGet("search")]
		public IEnumerable<Test> Search([FromQuery]TestCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Test;
				var list = (c == null ? q : q.Where(c.CreatePredicate())).ToList();
				return list;
			}
		}

		/// <summary>
		/// テストの取得
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns></returns>
		[HttpGet("get/{uid}")]
		public Test Get(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var q = db.Test;
				var o = q.Find(uid);
				return o;
			}
		}

		/// <summary>
		/// テストの作成
		/// </summary>
		/// <param name="o"></param>
		/// <returns>uid</returns>
		[HttpPost("create")]
		public int Create([FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int uid = db.InsertWithInt32Identity<Test>(o);
				return uid;
			}
		}

		/// <summary>
		/// テストの更新(必要時作成)
		/// </summary>
		/// <param name="o"></param>
		/// <returns>件数</returns>
		[HttpPost("upsert")]
		public int Upsert([FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				int count = db.InsertOrReplace<Test>(o);
				return count;
			}
		}

		/// <summary>
		/// テストの一括作成
		/// </summary>
		/// <param name="os"></param>
		/// <returns>BulkCopyRowsCopied</returns>
		[HttpPost("massive-new")]
		public BulkCopyRowsCopied MassiveCreate([FromBody]IEnumerable<Test> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var ret = db.BulkCopy<Test>(os);
				return ret;
			}
		}

		/// <summary>
		/// テストのマージ
		/// </summary>
		/// <param name="os"></param>
		/// <returns>件数</returns>
		[HttpPost("merge")]
		public int Merge([FromBody]IEnumerable<Test> os)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Merge<Test>(os);
				return count;
			}
		}

		/// <summary>
		/// テストの更新
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <param name="o"></param>
		/// <returns>更新件数</returns>
		[HttpPut, Route("modify/{uid}")]
		public int Modify(int uid, [FromBody]Test o)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Update<Test>(o);
				return count;
			}
		}

		/// <summary>
		/// テストの削除(物理)
		/// </summary>
		/// <param name="uid">ユニークID(uid)</param>
		/// <returns>件数</returns>
		[HttpDelete("remove/{uid}")]
		public int Remove(int uid)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Test
					.Where(_ => _.uid == uid)
					.Delete();
				return count;
			}
		}

		/// <summary>
		/// テストの削除(物理)
		/// </summary>
		/// <param name="c"></param>
		/// <returns>件数</returns>
		[HttpDelete("remove")]
		public int Remove([FromQuery]TestCondition c)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var count = db.Test
					.Where(c.CreatePredicate())
					.Delete();
				return count;
			}
		}

	}
}
