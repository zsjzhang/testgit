using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Entity;
using System.Web.Script.Serialization;
using VcyBer.BLMS.MobileWeb.Models;
using System.Text;
using System.Collections;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Member;
using System.Configuration;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class LifeServiceController : BaseController
    {
        #region 会员信息
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        #endregion

        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }
            return View();
        }
        #region 机场服务
        public ActionResult AirportIndex()
        {
            return View();
        }
        #region 获取所有机场
        public JsonResult GetAllAirports()
        {
            IEnumerable<Airport> _tempList = _AppContext.AirportServiceApp.SelectAirportList();
            return Json(_tempList);
        }
        #endregion
        #region 机场服务预约
        public ActionResult AirportReserve(string source)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }
            //积分兑换服务次数
            int ServiceCardIntegral = Convert.ToInt32(ConfigurationManager.AppSettings["ServiceCardIntegral"]);
            string userId = this.User.Identity.GetUserId(); //"02ecb613-0fa8-4567-b8ba-b326348c0df8";
            ViewBag.userId = userId;
            //用户总积分
            var accountInfo = _AppContext.UserIntegralApp.GetTotalIntegral(userId);
            ViewBag.AccountInfo = accountInfo;
            //用户已经领取的免费张数(不包括系统下发和积分兑换的)
            int makeFreeNum = _AppContext.AirportServiceApp.GetSNCardNumber(userId);
            ApplicationUser _userEntity = UserManager.FindById(userId);
            if (_userEntity.MLevel == 11)
            {
                //银卡会员可领取次数
                makeFreeNum = 2 - makeFreeNum;
            }
            else if (_userEntity.MLevel == 12)
            {
                //金卡会员可领取次数
                makeFreeNum = 3 - makeFreeNum;
            }
            else
            {
                makeFreeNum = 0;
            }
            ViewBag.MakeNum = makeFreeNum;
            //积分兑换的次数
            var integralNum = (accountInfo / ServiceCardIntegral);
            ViewBag.IntegralNum = integralNum;
            //获取机场所在的省份
            var listProvince = _AppContext.AirportServiceApp.SelectAirportProvince();
            ViewBag.listProvince = listProvince ?? new string[] { };

            return View();
        }
        #endregion
        #region 根据省获取机场
        public string GetAirportsByProvince(string province)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportList(province, string.Empty);
            List<Airport> list = _result.ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.Append("{");
                    str.AppendFormat("\"Id\":\"{0}\",", list[i].Id);
                    str.AppendFormat("\"AirportName\":\"{0}\"", list[i].AirportName);
                    str.Append("}");
                    if (i < list.Count - 1)
                    {
                        str.Append(",");
                    }
                }
            }
            str.Append("]");
            return str.ToString();
        }
        #endregion
        #region 根据机场名称查询所有机场候机室
        public string GetAirportRoomsByAirportName(string airportName)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportRoomList(airportName);
            List<Airport> list = _result.ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str.Append("{");
                    str.AppendFormat("\"Id\":\"{0}\",", list[i].Id);
                    str.AppendFormat("\"AirportAllName\":\"{0}\"", list[i].AirportAllName);
                    str.Append("}");
                    if (i < list.Count - 1)
                    {
                        str.Append(",");
                    }
                }
            }
            str.Append("]");
            return str.ToString();
        }
        #endregion
        #region 机场服务预约表单提交
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult LiveReserve(string userId, string phoneNumber, int freeCount, int scoreCount, int airportId)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new ReturnResult() { IsSuccess = false, Message = "您是注册用户或未登录，暂时无法预约该服务", Data = Request.RawUrl });
            }
            ReturnResult _entity = _AppContext.AirportServiceApp.GetServiceCard(userId, phoneNumber, freeCount, scoreCount, airportId, "N", "blms_wap");
            Session["SNCODES"] = (IEnumerable<SNCard>)(_entity.Data);
            return Json(_entity);
        }
        #endregion
        #region 预约成功跳转
        public ActionResult ReserveSuccess()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }
            #region old
            //ReturnResult _result = null;
            //HttpCookie _httpCookieEntity = HttpContext.Request.Cookies.Get("sncodes");
            //if (_httpCookieEntity != null)
            //{
            //    string _resultValue = HttpContext.Server.UrlDecode(_httpCookieEntity.Value);
            //    JavaScriptSerializer jss = new JavaScriptSerializer();
            //    _result = jss.Deserialize<ReturnResult>(_resultValue);
            //}
            #endregion
            ViewBag.SNCodes = Session["SNCODES"];
            return View();
        }
        #endregion
        #endregion

        #region  途牛
        //途牛首页
        public ActionResult TuniuIndex()
        {
            return View();
        }
        public JsonResult GetGiftBag()
        {
            //验证是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 404, msg = "请登录/注册bluemembers车主会员账号，领取途牛五星会员权益礼包！", toUrl = Request.RawUrl }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string userId = this.User.Identity.GetUserId();//"c94d4b8c-dc1e-4563-be47-96f858ba69f3"
                //会员等级
                ApplicationUser _curUser = UserManager.FindById(userId);
                if (_curUser.SystemMType < 2)
                {
                    //权限不足
                    return Json(new { code = 401, msg = "尊敬的会员您好，非车主会员暂无法领取途牛五星权益礼包哦~！" }, JsonRequestBehavior.AllowGet);
                }
                //根据用户信息获取权益码
                ReturnResult _ret = _AppContext.BrandServiceApp.GetBrandServiceCode(_curUser.Id, _curUser.PhoneNumber, "Tuniu");
                if (_ret.IsSuccess)
                {
                    //成功，返回权益码
                    return Json(new { code = 200, data = _ret.Data, msg = "获取权益码！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_ret.Data == null)
                    {
                        return Json(new { code = 400, msg = "获取权益码失败！" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 201, data = _ret.Data, msg = _ret.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        #endregion

        #region 礼悦会
        public ActionResult LiyueIndex()
        {
            return View();
        }
        public JsonResult BecomeLiyueMember()
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };
            if (!this.User.Identity.IsAuthenticated)
            {
                result.Message = "请登录/注册bluemembers车主会员账号，升级礼悦会钻石会员！";
                result.Data = 404;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                #region 验证车主/用户信息
                var user = UserManager.FindById(this.User.Identity.GetUserId());
                if (user.SystemMType < 2)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "尊敬的会员您好，非车主会员暂无法领取礼悦会钻石会员权益哦~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //if (user.IdentityNumber.Length < 15)
                //{
                //    result.IsSuccess = false;
                //    result.Data = 400;
                //    result.Message = "集团用户暂时无法完成注册";
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                //判断是否为会员
                if (_AppContext.BrandServiceApp.SelectMembershipBrandByUserId(user.Id).Where(x => x.BrandName == "HaiHang" && x.IsMember == "Y").Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "尊敬的车主会员，您已成为礼悦会钻石会员了~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var customer = _AppContext.CarServiceUserApp.GetCustomer(user.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "车主信息有误~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #endregion
                #region 处理姓名
                var family_name = "";
                var first_name = "";
                if (customer.CustName.Length < 1)
                {
                    family_name = "蓝";
                    first_name = "缤";
                }
                else if (customer.CustName.Length == 1)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = "某";
                }
                else if (customer.CustName.Length == 2)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 1);
                }
                else
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 2);
                }
                #endregion
                #region 服务调用
                var client = new LiyueServiceReference.MemberServiceClient();
                var request = new LiyueServiceReference.RecruitmentMemberRequest
                {
                    securityKey = "BJ123",
                    civilization_title_code = customer.Gender == "男" ? "Mr" : "Mrs",
                    family_name = family_name,
                    first_name = first_name,
                    mobile_phone = user.UserName,
                    password = "999999",
                    email_option = 0,
                    email_optionSpecified = true,
                    segmentSpecified = true,
                    language = "CN",
                    segment = 4
                };
                //发送请求，获取结果
                var response = new LiyueServiceReference.RecruitmentMemberResponse();
                response = client.submitRecruitmentMember(request);
                if (response.exitCode == 0)
                {
                    result.IsSuccess = true;
                    result.Data = response.account_number;
                    _AppContext.BrandServiceApp.AddMembershipBrand(new MembershipBrand
                    {
                        UserId = user.Id,
                        BrandName = "HaiHang",
                        IsMember = "Y",
                        JoinTime = DateTime.Now
                    });
                }
                else if (response.exitCode == -1 || response.exitCode == -9)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "接口发生内部错误";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "未知礼悦会接口异常";
                }
                #endregion
                #region 接口日志添加纪录
                //TO-DO 保存记录
                IF_RequestLog logEntity = new IF_RequestLog()
                {
                    UserId = user.Id,
                    RequestData = _AppContext.RequestLogApp.ConverterToJson(request),
                    ResponseData = _AppContext.RequestLogApp.ConverterToJson(response)
                };
                _AppContext.RequestLogApp.Add(logEntity);
                #endregion
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 途家
        public ActionResult TujiaIndex()
        {
            return View();
        }
        public JsonResult GetTujiaMemberRule()
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };
            if (!this.User.Identity.IsAuthenticated)
            {
                result.Message = "请登录/注册bluemembers车主会员账号，领取途家会员权益！";
                result.Data = 404;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                #region 会员/车主验证
                var user = UserManager.FindById(this.User.Identity.GetUserId());
                if (user.SystemMType < 2)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "尊敬的会员您好，非车主会员暂无法领取途家会员权益哦~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var customer = _AppContext.CarServiceUserApp.GetCustomer(user.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "车主信息有误~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #endregion
                ReceiveRecord record = new ReceiveRecord()
                {
                    UserId = user.Id,
                    BrandName = "tujia",
                    Ifinish = 1,
                    IsMember = 1,
                    ServiceCode = string.Empty,
                    JoinTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                };
                //添加领取记录
                var isTrue = _AppContext.ReceiveRecordApp.Add(record);

                if (isTrue)
                {
                    result.IsSuccess = true;
                    result.Data = 200;
                    result.Message = "您已成功领取途家会员权益";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}