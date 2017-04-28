using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using System.Text;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.FrontWeb.Models.WeChat;
using Vcyber.BLMS.Entity.CarService;
using log4net;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class AirportServiceController : Controller
    {
        private String userInfoState = "airport_UserInfo";
        private String strAppid = "wx8ba75eb0ebbb764b";
        private String strSecret = "db00357b1a0cff6494aedea9a081a69b";

        //public String UserOpenId = ""; //记录用户的Openid
        public string source = "airport_other";
        //
        // GET: /AirportService/
        public ActionResult Index(string source, string oid)
        {
            if (String.IsNullOrEmpty(oid))
            {
                //oid = Guid.NewGuid().ToString();
                return RedirectToAction("userInfo", new { source = source });
            }
            this.source = source;
            ViewBag.oid = oid;
            ViewBag.source = source;
            Common.LogService.Instance.Info("Action Index: ViewBag.source=" + source);
            Common.LogService.Instance.Info("Action Index: UserOpenId=" + oid);
            return View();
        }

        /// <summary>
        /// 最新政策
        /// </summary>
        /// <returns></returns>
        public ActionResult NewPolicy(string source, string oid)
        {
            Common.LogService.Instance.Info("Action NewPolicy:time=" + DateTime.Now.ToString());

            ViewBag.source = source;
            ViewBag.oid = oid;
            return View();
        }

        /// <summary>
        /// 机场列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AirportList(string source, string oid)
        {
            IEnumerable<Airport> _tempList = _AppContext.AirportServiceApp.SelectAirportList();
            ViewBag.source = source;
            ViewBag.oid = oid;
            return View(_tempList);
        }

        /// <summary>
        /// 活动抽奖
        /// </summary>
        /// <returns></returns>
        public ActionResult AirportActivity(string source, string oid)
        {
            ViewBag.source = source;
            ViewBag.oid = oid;
            return View();
        }
        /// <summary>
        /// 完善用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CompleteUserInfo(int prizeid, int prizeType, string oid, string source)
        {
            ViewBag.oid = oid;
            ViewBag.source = source;
            //prizeType(1:候机服务码;2:旅游套装)
            ViewBag.prizeId = prizeid;
            ViewBag.prizeType = prizeType;
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
            return View();
        }

        /// <summary>
        /// 试驾主页
        /// </summary>
        /// <returns></returns>
        public ActionResult TestDriveInfo(string oid, string source)
        {
            ViewBag.oid = oid;
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 中奖名单
        /// </summary>
        /// <returns></returns>
        public ActionResult WinningInfo()
        {
            IEnumerable<WinningInfo> _tempList = _AppContext.WinningInfoApp.GetWinningInfosByActivityId(5);
            return View(_tempList);
        }

        /// <summary>
        /// 特约店选择
        /// </summary>
        /// <returns></returns>
        public ActionResult AirportProvinceCity()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        public ActionResult AirportCarType()
        {
            IEnumerable<Vcyber.BLMS.Entity.Generated.CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return View(_result);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RadomPrize()
        {
            float notWin = 0.4f;
            float singleWin = 0.3f;
            float doubleWin = 0.3f;
            int poolCount = (100 * doubleWin) % 1 == 0 ? 100 : 1000;
            //填充奖品池
            List<int> prizepool = new List<int>();
            int _index = 0;
            for (int i = 0; i < poolCount * notWin; i++)
            {
                prizepool.Add(0);
            }
            _index = (int)(poolCount * notWin + 1);
            for (int j = _index; j < _index + poolCount * singleWin; j++)
            {
                prizepool.Add(1);
            }
            _index = (int)(_index + poolCount * singleWin + 1);
            for (int i = _index; i < poolCount; i++)
            {
                prizepool.Add(2);
            }
            int _radomIndex = new Random().Next(0, poolCount - 1);

            return Json(prizepool[_radomIndex], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AirportRoomInfo(string province, string city, string airport)
        {
            IEnumerable<Airport> _tempList = _AppContext.AirportServiceApp.SelectAirportRoomList(province, city, airport);
            string _html = "";
            foreach (var item in _tempList)
            {
                _html += "<p class='airport-room-title'>" + item.AirportRoomName + "</p>";
                _html += "<p class='airport-room-conent'>" + item.AirportAddress + "</p>";
            }
            return Json(_html, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断当前用户当日是否参加过活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckJoinActivity(int activityId, string oid)
        {
            Common.LogService.Instance.Info("Action CheckJoinActivity: oid=" + oid);
            //获取微信OpenId
            string _OpenId = oid;
            if (_AppContext.JoinActivityApp.IsUserJoinActivityByDay(activityId, _OpenId))
            {
                return Json(new { code = 400, msg = "您好，每天只有一次抽奖机会哦，明天再来试试吧!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 200, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 抽奖活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult StartActivity(int activityId, string oid, string source)
        {
            //获取微信OpenId    添加活动参与表(抽奖活动参与的标志为完整用户信息或未中奖)
            string _openId = oid;
            //判断当前用户是否已经重复参加判断（避免单个账号再两地同时登录）
            if (_AppContext.JoinActivityApp.IsUserJoinActivityByDay(activityId, _openId))
            {
                return Json(new { code = "401", msg = "重复抽奖" }, JsonRequestBehavior.AllowGet);
            }
            //获取奖品列表
            List<PrizesInfo> _prizesList = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
            //int _maxindex = _prizesList.Count;
            //随机生成中奖奖品
            //int _index = new Random().Next(0, _maxindex + 1);
            int _currentPrizeIndex = _prizesList.Count;
            PrizesInfo _currentPrizeInfo = null;
            _currentPrizeInfo = _prizesList[0];
            //概率计算
            int _poolCount = (double)(_currentPrizeInfo.Rate) >= 0.01 ? 100 : 1000;
            int _prizeCount = (int)(_currentPrizeInfo.Rate * _poolCount);
            int _newResutlIndex = new Random().Next(1, _poolCount);
            //判断未中奖
            if (_newResutlIndex > _prizeCount)
            {
                //未中奖时算是完整一次抽奖活动
                _AppContext.JoinActivityApp.AddJoinActivity(new JoinActivity()
                {
                    Results1 = _openId,
                    UserId = "",
                    DeviceType = 1,
                    ActivityId = activityId,
                    Results3 = source
                });
                return Json(new { code = "400", msg = "未中奖" }, JsonRequestBehavior.AllowGet);
            }

            //判断中奖则判断当前用户是否曾中奖
            if (_AppContext.WinningInfoApp.IsWinningByActivity(activityId, _openId))
            {
                _AppContext.JoinActivityApp.AddJoinActivity(new JoinActivity()
                {
                    Results1 = _openId,
                    UserId = "",
                    DeviceType = 1,
                    ActivityId = activityId,
                    Results3 = source
                });
                return Json(new { code = "400", msg = "未中奖" }, JsonRequestBehavior.AllowGet);
            }
            //判断中奖结果是否超过上限
            _currentPrizeIndex = CheckOverUpPrizeLimit(_prizesList, activityId, _currentPrizeIndex);
            if (_currentPrizeIndex < 0)
            {
                _AppContext.JoinActivityApp.AddJoinActivity(new JoinActivity()
                {
                    Results1 = _openId,
                    UserId = "",
                    DeviceType = 1,
                    ActivityId = activityId,
                    Results3 = source
                });
                return Json(new { code = "400", msg = "未中奖" }, JsonRequestBehavior.AllowGet);
                //
            }
            //返回中奖结果
            return Json(new { code = 200, msg = _currentPrizeInfo.Title, prizeid = _currentPrizeIndex }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult IsRepeatWin(int activityId, string oid)
        {
            bool _IsRepeat = _AppContext.WinningInfoApp.IsWinningByActivity(activityId, oid);
            return Json(_IsRepeat, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult IsRepeatTestDrive(string _tel, string _source)
        {
            //bool _IsRepeat = _AppContext.TestDriveApp..IsWinningByActivity(activityId, oid);
            return Json(new { });
        }

        /// <summary>
        /// 检测奖品是否达到上限
        /// </summary>
        /// <param name="_prizesList"></param>
        /// <param name="prizeTag"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public int CheckOverUpPrizeLimit(List<PrizesInfo> _prizesList, int activityId, int prizeTag)
        {
            bool result = true;
            int newPrizeTag = -1;
            //二选一
            if (prizeTag == _prizesList.Count)
            {
                for (int i = 0; i < _prizesList.Count; i++)
                {
                    result = result && (_prizesList[i].CyclesUnuseNum > 0);
                    if (_prizesList[i].CyclesUnuseNum > 0)
                        newPrizeTag = i;
                }
                if (result)
                {
                    return prizeTag;
                }
                else
                {
                    return newPrizeTag;
                }
            }
            else        //验证奖品剩余数量
            {
                return _prizesList[prizeTag].CyclesUnuseNum > 0 ? prizeTag : -1;
            }
        }

        /// <summary>
        /// 完善用户参与信息和获奖信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="activityId"></param>
        /// <param name="prizeId"></param>
        /// <param name="_name"></param>
        /// <param name="_tel"></param>
        /// <param name="_province"></param>
        /// <param name="_city"></param>
        /// <param name="_area"></param>
        /// <param name="_address"></param>
        /// <returns></returns>
        public JsonResult CompleteWinningInfo(int activityId, int prizeId, string _name, string _tel, string _province, string _city, string _area, string _address, int prizeType, string oid, string source)
        {
            //prizeType（1：虚拟奖品；2：实物奖品）
            JoinActivity joinEntity = new JoinActivity();
            WinningInfo winningEntity = new WinningInfo();
            //活动参与表复制
            joinEntity.ActivityId = activityId;
            joinEntity.Results1 = oid;
            joinEntity.Name = _name;
            joinEntity.UserId = _tel;
            joinEntity.Tel = _tel;
            //实物奖品
            if (prizeType == 2)
            {
                joinEntity.Province = _province;
                joinEntity.City = _city;
                joinEntity.Area = _area;
                joinEntity.Address = _address;
            }

            //获奖表复制
            winningEntity.ActivityId = activityId;
            winningEntity.InvoiceNO = oid;
            winningEntity.UserName = _name;
            winningEntity.UserId = _tel;
            winningEntity.UserTel = _tel;
            winningEntity.PrizesId = prizeId;
            //实物奖品
            if (prizeType == 2)
            {
                winningEntity.Province = _province;
                winningEntity.City = _city;
                winningEntity.Area = _area;
                winningEntity.Address = _address;
            }
            //减去奖品数量
            joinEntity.Results3 = source;
            _AppContext.PrizesInfoApp.CutDownPrizeCyclesUnuseNum(prizeId, activityId, 1);
            bool isSuccess = _AppContext.JoinActivityApp.AddJoinActivity(joinEntity) && _AppContext.WinningInfoApp.AddWinningInfo(winningEntity);
            if (isSuccess && prizeId == 29)
            {
                _AppContext.AirportServiceApp.GetFreeServiceCard("81A187AF-F961-4616-BBF9-DE80B35CFE3F", _tel, 1, 4, 0);
            }
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SelectWinningInfos(int activityId, int pageIndex, int pageSize)
        {
            PageData pageData = new PageData() { Size = pageSize, Index = pageIndex };
            int total = 0;
            IEnumerable<WinningInfo> _tempList = _AppContext.WinningInfoApp.GetWinningInfoByActivityId(activityId, pageData, out total);
            return Json(new { Data = _tempList, TotalCount = total }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// form表单ajax预约试驾请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoTestDrive(TestDriveEntity cstestDriveEntity)
        {
            if (cstestDriveEntity == null)
            {
                return Json(new { code = 400, msg = "请输入试驾信息" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(cstestDriveEntity.Phone))
            {
                return Json(new { code = 400, msg = "请输入手机号码" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(cstestDriveEntity.UserName))
            {
                return Json(new { code = 400, msg = "请输入姓名" }, JsonRequestBehavior.AllowGet);
            }
            if (cstestDriveEntity.ScheduleDate == null || cstestDriveEntity.ScheduleDate.Value < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                return Json(new { code = 400, msg = "请正确填写试驾日期" }, JsonRequestBehavior.AllowGet);
            }

            //string userId = string.Empty;
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    userId = User.Identity.GetUserId();
            //}
            //cstestDriveEntity.UserId = userId;

            Common.LogService.Instance.Info("Action DoTestDrive: cstestDriveEntity.DataSource=" + source);
            cstestDriveEntity.DataSource = this.source;
            int _code = 200;
            int _result = _AppContext.TestDriveApp.Add(cstestDriveEntity);
            if (_result <= 0)
            {
                _code = 400;
            }

            //if (User.Identity.IsAuthenticated)
            //{
            //    int outValue;
            //    var account =
            //        new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId()).Result;
            //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.预约保养, account.Id, (MemshipLevel)account.MLevel,
            //        out outValue);
            //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.预约保养, account.Id, out outValue);
            //}

            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }

        #region 微信Oauth2 验证

        public ActionResult userInfo(string source)
        {
            var returnUrl = "https://www.bluemembers.com.cn/AirportService/userInfoOauth2?source=" + source;
            var url = this.GetRedirectUrl(returnUrl, "snsapi_userinfo", userInfoState);
            return Redirect(url);
        }
        public ActionResult userInfoOauth2(string code, string state, string source)
        {
            if (String.IsNullOrEmpty(code))
            {   //没有授权
                return Content("您拒绝了授权");
            }
            if (String.IsNullOrEmpty(state) || state != userInfoState)
            {   //错误的返回参数
                return Content("返回参数错误");
            }
            Common.LogService.Instance.Info("Action userInfoOauth2:code=" + code);
            //做用户是否关注公众号验证，做snsapi_userinfo用户授权
            var tokenInfo = GetAccess_Token(code);
            if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.access_token))
            {

                Common.LogService.Instance.Info("Action userInfoOauth2:tokenInfo.access_token=" + tokenInfo.access_token);
                Common.LogService.Instance.Info("Action userInfoOauth2:tokenInfo.openid=" + tokenInfo.openid);
                //var user = GetSignUserInfo(tokenInfo.openid, tokenInfo.access_token);
                var user = this.getUserInfo1(tokenInfo.access_token, tokenInfo.openid);
                if (user != null && !String.IsNullOrEmpty(user.openid))
                {
                    ///TODO 记录用户信息等操作
                    Common.LogService.Instance.Info("Action userInfoOauth2:source=" + source);
                    return RedirectToAction("Index", new { source = source, oid = user.openid });
                }
                else
                {   //获取用户信息失败
                    return Content("请求出现错误");
                }
            }
            else
            {   //用户的code错误
                return Content("参数code错误!");
            }

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取带是否关注标识的用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private WeChatUserInfo GetSignUserInfo(String openId, string token)
        {
            if (String.IsNullOrEmpty(openId))
            {
                return null;
            }
            //var token = WeChatHelper.GetWeChatBaseAccess_Token();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", token, openId);
            var json = WebUtils.GET_WebRequestHTML("utf-8", url);
            Common.LogService.Instance.Info("Action GetSignUserInfo: json=" + json);
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }
            var userInfo = WebUtils.JsonToObj<WeChatUserInfo>(json, null);
            return userInfo;
        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private SussCode GetAccess_Token(String code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return null;
            }
            StringBuilder url = new StringBuilder();
            url.Append("https://api.weixin.qq.com/sns/oauth2/access_token?");
            url.AppendFormat("appid={0}", this.strAppid);
            url.AppendFormat("&secret={0}", this.strSecret);
            url.AppendFormat("&code={0}", code);
            url.AppendFormat("&grant_type={0}", "authorization_code");

            var json = WebUtils.GET_WebRequestHTML("utf-8", url.ToString());
            Common.LogService.Instance.Info(json);
            if (String.IsNullOrEmpty(json))
            {   //请求失败
                return null;
            }
            return WebUtils.JsonToObj<SussCode>(json, null);
        }

        private String GetRedirectUrl(String backUrl, String scope, String state)
        {
            var responseType = "code";
            StringBuilder url = new StringBuilder();
            url.Append("https://open.weixin.qq.com/connect/oauth2/authorize?");
            url.AppendFormat("appid={0}", this.strAppid);
            url.AppendFormat("&redirect_uri={0}", HttpUtility.UrlEncode(backUrl));
            url.AppendFormat("&response_type={0}", responseType);
            url.AppendFormat("&scope={0}", scope);
            url.AppendFormat("&state={0}", state);
            url.AppendFormat("#wechat_redirect");

            return url.ToString();
        }


        private WeChatUserInfo getUserInfo1(string token, string openId)
        {
            string url = String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", token, openId);
            var json = WebUtils.GET_WebRequestHTML("utf-8", url);
            Common.LogService.Instance.Info("Action getUserInfo1: json=" + json);
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }
            var userInfo = WebUtils.JsonToObj<WeChatUserInfo>(json, null);
            return userInfo;
        }
        #endregion

    }
}