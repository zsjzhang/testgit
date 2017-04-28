using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using System.Xml;
using System.Linq;
using AspNet.Identity.SQL;

using Microsoft.AspNet.Identity;
using System.Web.Mvc;
namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class GetVinCustTimeController : Controller
    {
        /// <summary>
        /// 返回银卡会员的姓名，VIN，购车日期
        /// </summary>
        /// <returns>测试</returns>     
        public   JsonResult GetVinCustTimeList()
        {
            var list = _AppContext.GetVinCustTime.GetVinCustTime().ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回银卡会员的姓名，VIN，购车日期
        /// </summary>
        /// <param name="startDate">购车开始日期</param>
        /// <param name="endDate">购车结束日期</param>
        /// <returns></returns>
        public JsonResult GetVinCustTimeListByDate(string startdate , string  enddate)
        {
            var list = _AppContext.GetVinCustTime.GetVinCustTime(startdate, enddate).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}