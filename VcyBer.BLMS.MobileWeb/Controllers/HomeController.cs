using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using System.Linq;
using Vcyber.BLMS.Entity.Generated;
using System.Collections.Generic;
using System.Configuration;
using Vcyber.BLMS.Entity;
using System;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Index
        public ActionResult Index()
        {
            //匹配web还是pc
            var agents = new string[] { "android", "iphone", "symbianos", "windows phone", "ipad", "ipod", "micromessenger" };
            var userAgent = Request.UserAgent.ToLower();
            if (agents.Count(x => userAgent.Contains(x)) == 0)
            {
                return Redirect(string.Format("http://www.bluemembers.com.cn"));//跳转到web页面
            }
            return View();
        }
        /// <summary>
        /// 焕颜迎新
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberActivity()
        {
            return View();
        }
        /// <summary>
        /// 置换政策
        /// </summary>
        /// <returns></returns>
        public ActionResult ExChangePolicy()
        {
            Models.ExChangePolicy ex = new Models.ExChangePolicy()
            {
                Activity = _AppContext.ActivitiesApp.GetActivitiesById(2),//活动
                CarTypeList = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar), //车型
                ProvinceList = _AppContext.DealerApp.GetProvinceList()//省
            };

            //中奖用户，页面上为静态写入
            //int activityid = int.Parse(ConfigurationManager.AppSettings["ActivityId"]);
            //IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityid, 0, 0);
            //var result = lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾");
            return View(ex);
        }
        /// <summary>
        /// 会员体系升级
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberSystemUpGrade()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetCityListByProvince(string province)
        {
            var provinceList=_AppContext.DealerApp.GetCityListByProvince(province);
            return Json(new { success=true,data=provinceList});
        }
        public JsonResult GetDealerShipList(string province, string city, int Istestserver=0, int IsDingChe=0, int IsWeibao=0)
        {
            var list=_AppContext.DealerApp.GetDealerShipList(province, city, Istestserver, IsDingChe, IsWeibao);
            return Json(new {success=true,data=list });
        }
        /// <summary>
        /// 添加预约置换
        /// </summary>
        /// <param name="xDOrderChange"></param>
        /// <returns></returns>
        public JsonResult AddOrderChange(XDOrderChange xDOrderChange)
        {
            try
            {
                //判断是否登录
                if (!this.User.Identity.IsAuthenticated)
                {
                    return Json(new { code = "1", msg = Request.RawUrl });
                }
                xDOrderChange.CreaterId = "0";
                xDOrderChange.CreaterName = xDOrderChange.Name;
                xDOrderChange.CreaterTime = DateTime.Now;
                xDOrderChange.UpdaterId = "0";
                xDOrderChange.UpdaterName = xDOrderChange.Name;
                xDOrderChange.UpdaterTime = DateTime.Now;
                xDOrderChange.OrderChangeTime = DateTime.Now;
                xDOrderChange.IsValid = 1;
                bool isExistLottery = _AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserMobile(xDOrderChange.Mobile, xDOrderChange.ActivityId, 0);
                if (_AppContext.XDOrderChangeApp.IsCanOrderChange(xDOrderChange.CarSeriers, xDOrderChange.Mobile, xDOrderChange.ShopCode))
                {
                    return Json(new { code = "0", msg = "您已经预约过，请耐心等待，经销商会主动与您取得联系！" });
                }
                bool result = _AppContext.XDOrderChangeApp.AddOrderChange(xDOrderChange);
                if (!result)
                {
                    return Json(new { code = "0", msg = "预约置换失败！" });
                }
                else
                {
                    if (isExistLottery)
                    {
                        return Json(new { code = "201", msg = "已抽奖！" });
                    }
                    else
                    {
                        return Json(new { code = "200", msg = "恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "预约置换异常！" });
            }
        }
    }
}