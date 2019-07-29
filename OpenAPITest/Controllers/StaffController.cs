using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using peppa.Domain;
using peppa.util;

namespace OpenAPITest.Controllers
{
	public partial class StaffController : ControllerBase
	{
		/// <summary>
		/// 職員の取得(フルセット)
		/// </summary>
		/// <param name="staffNo">職員番号(staff_no)</param>
		/// <returns code="200">Found the Object</returns>
		/// <returns code="404">Invalid identifiers</returns>
		[HttpGet("get-full/{staffNo}")]
		[ProducesResponseType(typeof(Staff), 200)]
		[ProducesResponseType(404)]
		public IActionResult GetFull(string staffNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
				var o = db.Staff
					.LoadWith(_ => _.NameList.First().PersonNameType)
					.LoadWith(_ => _.AddressList.First().AddressType)
					.LoadWith(_ => _.ContactList.First().ContactType)
					.Find(staffNo);

				return o == null ? (IActionResult)NotFound() : Ok(o);
			}
		}

        /// <summary>
        /// 職員の検索
        /// </summary>
        /// <param name="c"></param>
        /// <param name="order">例) kana, name, staff_no Desc</param>
        /// <returns></returns>
        [HttpGet("search-full")]
		[ProducesResponseType(typeof(IEnumerable<Staff>), 200)]
		public IActionResult SearchFull([FromQuery]StaffCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
                var q = db.Staff
                    .LoadWith(_ => _.NameList.First().PersonNameType)
                    .LoadWith(_ => _.AddressList.First().AddressType)
                    .LoadWith(_ => _.ContactList.First().ContactType);

                var list = (c == null ? q : q.Where(c.CreatePredicate()));
                return Ok(order.Any() ? list.SortBy(order).ToList() : list.ToList());
			}
		}
	}
}