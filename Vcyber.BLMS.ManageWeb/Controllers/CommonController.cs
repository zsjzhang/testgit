using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;
using WebGrease.Css.Extensions;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [AllowAnonymous]
    public class CommonController : Controller
    {
        [HttpPost]
        public JsonResult GetMembershipLevelJsonResult()
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            foreach (int item in Enum.GetValues(typeof(MemshipLevel)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((MemshipLevel) item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMembershipStatusJsonResult()
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            foreach (int item in Enum.GetValues(typeof(MembershipStatus)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((MembershipStatus)item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //获取会员审批状态
        [HttpPost]
        public JsonResult GetMembershipApproveType()
        {
            var list = new List<OptionType>();
           // list.Add(new OptionType("-1", "请选择"));
            foreach (int item in Enum.GetValues(typeof(MembershipApplyApprovalStatus)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((MembershipApplyApprovalStatus)item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDealerProvinceListJsonResult()
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            _AppContext.DealerApp.GetProvinceList().ForEach((p) => list.Add(new OptionType
            {
                id = p,
                value = p
            }));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDealerCityListJsonResult(string province)
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            _AppContext.DealerApp.GetCityListByProvince(province).ForEach((p) => list.Add(new OptionType
            {
                id = p,
                value = p
            }));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDealerListJsonResult(string province, string city)
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            _AppContext.DealerApp.GetDealerList(province, city).ForEach((p) => list.Add(new OptionType
            {
                id = p.DealerId,
                value = p.Name
            }));
            
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetServiceTypeJsonResult()
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            foreach (int item in Enum.GetValues(typeof(EDMSServiceType)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((EDMSServiceType)item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceStatusJsonResult()
        {
            var list = new List<OptionType>();
            foreach (int item in Enum.GetValues(typeof(EServiceStatusType)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((EServiceStatusType)item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetImageCarouselTypeJsonResult()
        {
            var list = new List<OptionType>();
            list.Add(new OptionType("-1", "请选择"));
            foreach (int item in Enum.GetValues(typeof(EImageCarouselType)))
            {
                var optionType = new OptionType
                {
                    id = item.ToString(CultureInfo.InvariantCulture),
                    value = ((EImageCarouselType)item).GetDiscribe()
                };
                list.Add(optionType);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}