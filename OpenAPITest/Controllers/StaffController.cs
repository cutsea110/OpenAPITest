using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAPITest.CustomPolicyProvider;
using OpenAPITest.Domain;
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
        [PermissionTypeAuthorize("Read_Staff")]
        [HttpGet("get-full/{staffNo}")]
		[ProducesResponseType(typeof(Staff), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetFull(string staffNo)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
                var o = db.Staff
                    .LoadWith(_ => _.SexType)
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
        /// <param name="order">例) birth_date Desc, gender Asc, staff_no</param>
        /// <returns></returns>
        [PermissionTypeAuthorize("Read_Staff")]
        [HttpGet("search-full")]
		[ProducesResponseType(typeof(IEnumerable<Staff>), StatusCodes.Status200OK)]
		public IActionResult SearchFull([FromQuery]StaffCondition c, [FromQuery]string[] order)
		{
#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
#endif
			using (var db = new peppaDB())
			{
                var q = db.Staff
                    .LoadWith(_ => _.SexType)
                    .LoadWith(_ => _.NameList.First().PersonNameType)
                    .LoadWith(_ => _.AddressList.First().AddressType)
                    .LoadWith(_ => _.ContactList.First().ContactType);

                var filtered = c == null ? q : q.Where(c.CreatePredicate());
                var ordered = order.Any() ? filtered.SortBy(order) : filtered;

                return Ok(ordered.ToList());
			}
		}
	}
}