using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class DealerNewController : Controller
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        private int PAGESIZE_LIST = 10;

        private string imagePath = string.Empty;

        #endregion

        #region ==== 公共属性 ====

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
            private set
            {
                _signInManager = value;
            }
        }


        #endregion

        public DealerNewController() { }

        public DealerNewController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        //
        // GET: /DealerNew/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult liftmovie()
        {
            return View();
        }

        public ActionResult ExchangeTicket()
        {
            return View();
        }
        public ActionResult TujiaMemberRules()
        {
            return View();
        }
        public ActionResult  CoffeeofStarbucks()
        {
            return View();
        }
        public ActionResult LiyueIndex()
        {
            return View();
        }

        public ActionResult TuniuIndex()
        {
            return View();
        }

        public ActionResult MembershipTerms()
        {
            return View();
        }

        public ActionResult AlertDealerInfo()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetTuniuRightCode()
        {
            //搜集当前用户的信息
            ApplicationUser _curUser;
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 404, msg = "用户未登录！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _curUser = UserManager.FindById(this.User.Identity.GetUserId());
                if (_curUser.MLevel < 2)
                {
                    //权限不足
                    return Json(new { code = 401, msg = "权限不足！" }, JsonRequestBehavior.AllowGet);
                }
                ReturnResult _ret = _AppContext.BrandServiceApp.GetBrandServiceCode(_curUser.Id, _curUser.PhoneNumber, "Tuniu");
                if (_ret.IsSuccess)
                {
                    //获取权益码
                    return Json(new { code = 200, data = _ret.Data, msg = "获取权益码！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_ret.Data == null)
                    {
                        return Json(new { code = 400, msg = "获取失败！" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 201, data = _ret.Data, msg = _ret.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult TobeMasonryMember()
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };
            if (!this.User.Identity.IsAuthenticated)
            {
                result.Message = "用户未登录！";
                result.Data = 404;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //获取权益码
                var user = UserManager.FindById(this.User.Identity.GetUserId());
                if (user.MLevel < 2 || user.IdentityNumber == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "尊敬的会员您好，非车主会员暂无法领取礼悦会钻石会员权益哦~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (user.IdentityNumber.Length < 15)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "集团用户暂时无法完成注册";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

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
                //处理姓名
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


                var client = new HaiHangServiceReference.MemberServiceClient();

                var request = new HaiHangServiceReference.RecruitmentMemberRequest
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
                var response = new HaiHangServiceReference.RecruitmentMemberResponse();

                //模拟接口
                //response.exitCode = 0;
                //response.account_number = "960289881942";

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

                //TO-DO 保存记录
                IF_RequestLog logEntity = new IF_RequestLog()
                {
                    UserId = user.Id,
                    RequestData = _AppContext.RequestLogApp.ConverterToJson(request),
                    ResponseData = _AppContext.RequestLogApp.ConverterToJson(response)
                };
                _AppContext.RequestLogApp.Add(logEntity);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 途家会员权益领取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetTujiaMemberRule()
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };
            if (!this.User.Identity.IsAuthenticated)
            {
                result.Message = "用户未登录！";
                result.Data = 404;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //获取权益码
                var user = UserManager.FindById(this.User.Identity.GetUserId());
                if (user.MLevel < 2 || user.IdentityNumber == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "尊敬的会员您好，非车主会员暂无法领取途家会员权益哦~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                ////判断是否为会员
                //if (_AppContext.BrandServiceApp.SelectMembershipBrandByUserId(user.Id).Where(x => x.BrandName == "HaiHang" && x.IsMember == "Y").Count() > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Data = 400;
                //    result.Message = "尊敬的车主会员，您已成为礼悦会钻石会员了~";
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                var customer = _AppContext.CarServiceUserApp.GetCustomer(user.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "车主信息有误~";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                Entity.Member.ReceiveRecord GetTujiaMemberRuleRecord = new Entity.Member.ReceiveRecord()
                {
                    UserId=user.Id,
                    BrandName="tujia",
                    Ifinish=1,
                    IsMember=1,
                    ServiceCode=string.Empty,
                    JoinTime=DateTime.Now,
                    CreateTime=DateTime.Now,
                };
                //插入领取记录
                var isTrue=_AppContext.ReceiveRecordApp.Add(GetTujiaMemberRuleRecord);

                if (isTrue)
                {
                    result.IsSuccess = true;
                    result.Data=200;
                    result.Message = "您已成功领取途家会员权益";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
                
            }
        
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult IsDealerMemberShip()
        {
            //搜集当前用户的信息
            ApplicationUser _curUser;

            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _curUser = UserManager.FindById(this.User.Identity.GetUserId());
                IEnumerable<MembershipBrand> _list = _AppContext.BrandServiceApp.SelectMembershipBrandByUserId(_curUser.Id);
                bool isTuniu = false;
                bool isHaihang = false;
                _list.ToList().ForEach(f =>
                {
                    if (f.BrandName.Equals("Tuniu") && (f.IsMember.Equals("Y")))
                    {
                        isTuniu = true;
                    }
                    if (f.BrandName.Equals("HaiHang") && (f.IsMember.Equals("Y")))
                    {
                        isHaihang = true;
                    }
                });
                //获取权益码
                return Json(new { code = 200, Tuniu = isTuniu, Haihang = isHaihang }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ToBeFiveStarMember()
        {
            ApplicationUser _curUser;

            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _curUser = UserManager.FindById(this.User.Identity.GetUserId());
                IEnumerable<MembershipBrand> _list = _AppContext.BrandServiceApp.SelectMembershipBrandByUserId(_curUser.Id);
                bool isTuniu = false;
                var code = _AppContext.BrandServiceApp.SelectBrandServiceCodeByUserId(_curUser.Id, "Tuniu");
                //_list.ToList().ForEach(f =>
                //{
                //    if (f.BrandName.Equals("Tuniu") && (f.IsMember.Equals("Y")))
                //    {
                //        isTuniu = true;
                //    }
                //});
                //判断是否获取权益码
                isTuniu = (code != null);
                if (isTuniu)
                {
                    //判断是否为重复注册
                    if (_AppContext.BrandServiceApp.SelectMembershipBrandByUserId(_curUser.Id).Where(x => x.BrandName == "Tuniu" && x.IsMember == "Y").Count() > 0)
                    {
                        return Json(new { code = 202 }, JsonRequestBehavior.AllowGet);
                    }
                    //获取权益码
                    return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    //未获取权益码
                    return Json(new { code = 201 }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}