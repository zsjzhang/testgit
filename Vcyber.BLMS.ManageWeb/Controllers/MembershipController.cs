using System.Threading.Tasks;
using System.Web.Helpers;
using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.ManageWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.ManageWeb.EF;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using NPOI.SS.Formula.Functions;
using Vcyber.BLMS.ManageWeb.Models.Membership;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class MembershipController : Controller
    {
        private ApplicationUserManager _userManager;
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
        /// <summary>
        /// 会员列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = true;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = false;
            }
            return View();
        }

        /// <summary>
        /// 创建会员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateNormal()
        {
            return View();
        }

        /// <summary>
        /// 获取会员详情的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            ViewBag.admin = this.User.Identity.Name;
            return View();
        }

        /// <summary>
        /// 会员激活页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Active()
        {
            return View();
        }

        /// <summary>
        /// 会员审批页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Approving()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = true;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = false;
            }
            return View();
        }
        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetMembershipDetail(string id)
        {
            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var identity = frontUserStore.FindByIdAsync(id);

            //FrontIdentityUser model =new FrontIdentityUser();
            //model.Industry=identity.Result.Industry;

            identity.Result.MTypeName = ((MembershipType)identity.Result.SystemMType).GetDiscribe();
            identity.Result.MLevelName = ((MemshipLevel)(identity.Result.MLevel == 3 && !string.IsNullOrEmpty(identity.Result.No) ? 9 : identity.Result.MLevel)).GetDiscribe();
            identity.Result.GenderName = identity.Result.GenderName;
            identity.Result.IsPay = identity.Result.IsPay;
            identity.Result.PayStatus = ((Vcyber.BLMS.Entity.Enum.MembershipPayStatus)identity.Result.IsPay).GetDiscribe();
            identity.Result.No = identity.Result.No;

            identity.Result.RealName = identity.Result.RealName;
            identity.Result.Email = identity.Result.Email;
            identity.Result.Address = identity.Result.Address;
            identity.Result.Gender = identity.Result.Gender;

            identity.Result.MainTelePhone = identity.Result.MainTelePhone;
            identity.Result.TelePhone = identity.Result.TelePhone;
            identity.Result.TransactionTime = identity.Result.TransactionTime;
            identity.Result.Remark = identity.Result.Remark;
            identity.Result.MarriageDay = identity.Result.MarriageDay;

            //Vcyber.BLMS.Entity.Enum.ECustomerIdentificationType.GetName(typeof(ECustomerIdentificationType), 2);

            identity.Result.Industry = string.IsNullOrEmpty(identity.Result.Industry) ? "" : ((Vcyber.BLMS.Entity.Enum.ECustomerJob)int.Parse(identity.Result.Industry)).GetDiscribe();
            identity.Result.Job = string.IsNullOrEmpty(identity.Result.Job) ? "" : ((Vcyber.BLMS.Entity.Enum.ECustomerPost)int.Parse(identity.Result.Job)).GetDiscribe();
            identity.Result.Educational = string.IsNullOrEmpty(identity.Result.Educational) ? "" : ((Vcyber.BLMS.Entity.Enum.ECustomerEducational)int.Parse(identity.Result.Educational)).GetDiscribe();
            identity.Result.MainContact = string.IsNullOrEmpty(identity.Result.MainContact) ? "" : ((Vcyber.BLMS.Entity.Enum.ECustomerMainContact)int.Parse(identity.Result.MainContact)).GetDiscribe();
            identity.Result.IsMarriage = string.IsNullOrEmpty(identity.Result.IsMarriage) ? "" : ((Vcyber.BLMS.Entity.Enum.ECustomerMarriageType)int.Parse(identity.Result.IsMarriage)).GetDiscribe();


            string newtime = _AppContext.LoginMemRecordApp.GetNewLoginTime(id);
            identity.Result.NewLoginTime = string.IsNullOrEmpty(newtime) ? "" : DateTime.Parse(newtime).ToString("yyyy/MM/dd HH:mm:ss");
            identity.Result.MarriageDay = identity.Result.MLevelBeginDate.ToString("yyyy-MM-dd");
            identity.Result.strMLevelBeginDate = identity.Result.MLevelBeginDate.ToString("yyyy/MM/dd HH:mm:ss");
            identity.Result.strMlevelInvalidDate = identity.Result.MLevelInvalidDate.ToString("yyyy/MM/dd HH:mm:ss");
           
            //var customer = _AppContext.CarServiceUserApp.GetCustomer(identity.Result.IdentityNumber);
            //identity.Result.RealName = customer != null ? customer.CustName : string.Empty;
            //identity.Result.GenderName = customer != null ? customer.Gender : string.Empty;
            //identity.Result.Address = customer != null ? customer.Address : string.Empty;
            //identity.Result.Email = customer != null ? customer.Email : string.Empty;

            return Json(identity.Result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult CreateMembershipjoinMember(MembershipModel model)
        {
            bool Issucceed = false;
            
            IdentityResult CreateResult = null;
            bool ActiveResult;
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                {
                    if (e.FirstOrDefault() != null)
                        msg += e.FirstOrDefault().ErrorMessage + "\n";
                });
                return Json(new { success = false, msg = msg });
            }
         

            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);


            Regex reg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            if (!reg.IsMatch(model.IdentityNumber))
            {
                return Json(new { success = false, msg = "输入的身份证号码不正确！" });
            }

            string birthday = "";
            int Gender = -1;
            int Age = -1;
            if (model.IdentityNumber.Length == 15)
            {
                birthday = "19" + model.IdentityNumber.Substring(6, 2) + "-" + model.IdentityNumber.Substring(8, 2) + "-" + model.IdentityNumber.Substring(10, 2);
                Gender = (Convert.ToInt32(model.IdentityNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                Age = DateTime.Now.Year-Convert.ToInt32("19" + model.IdentityNumber.Substring(6, 2));
            }
            if (model.IdentityNumber.Length == 18)
            {
                birthday = model.IdentityNumber.Substring(6, 4) + "-" + model.IdentityNumber.Substring(10, 2) + "-" + model.IdentityNumber.Substring(12, 2);
                Gender = (Convert.ToInt32(model.IdentityNumber.Substring(16,1)) % 2 == 0 ? 2 : 1);
                Age = DateTime.Now.Year-Convert.ToInt32(model.IdentityNumber.Substring(6, 4));
            }

            if (store.IsIdentityNumberRepeate(model.IdentityNumber))
                return Json(new { success = false, msg = "系统中已存在此身份证号" });
        
            if (store.CheckUserNameIsExist(model.PhoneNumber))
                return Json(new { success = false, msg = "系统中已存在此手机号" });

            //var member = store.FindByNameAsync(model.PhoneNumber).Result;
            //var member = store.FindByName(model.PhoneNumber);
            //var member = _DbSession.PrizesInfoStorager.GetMembershipMode(model.PhoneNumber);
            //if (member == null)// || string.IsNullOrEmpty(member.Id)
            //{
                var mlevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(model.IdentityNumber));
                var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber).ToList<Car>();
                var membershipIdentity = new FrontIdentityUser
                {
                    No = _AppContext.MemberNumberApp.GetNumber("1"),
                    NickName = CommonUtilitys.GetNikeName(),
                    SystemMType = (carList != null && carList.Count > 0 ? (int)MembershipType.WhitCar : (int)MembershipType.WhitoutCar),
                    RealName = model.RealName,
                    //NickName = model.NickName,
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    IdentityNumber = model.IdentityNumber,
                    Mid = model.IdentityNumber,
                    Password = "Bm" + model.PhoneNumber.Substring(5),//这里由原来的身份证改为手机号后6微
                    Status = (int)MembershipStatus.Nomal,
                    CreateTime = DateTime.Now.ToLongTimeString(),
                    CreatedPerson = this.User.Identity.Name,
                    MType = (int)MembershipType.WhitCar,
                    MLevel = mlevel,// (int)MemshipLevel.CommonCard,//级别
                    IsPay = (int)MembershipPayStatus.Paid,//经销商新增的会员均为已缴纳100付费
                    ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                    ActiveWay = (int)MembershipActiveWay.ManageWeb, //后台提交激活流程
                    IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                    MLevelBeginDate = DateTime.Now,
                    MLevelInvalidDate = mlevel > 10 ? DateTime.Now.AddYears(1) : DateTime.MaxValue,
                    PasswordHash = membershipManager.PasswordHasher.HashPassword("Bm" + model.PhoneNumber.Substring(5)),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    AuthenticationTime = (carList != null && carList.Count > 0 ? DateTime.Now : Convert.ToDateTime("1900-1-1")),
                    AuthenticationSource = this.User.Identity.Name,
                    Birthday = birthday,
                    Gender = Gender + "",
                    Age = Age
                };
                try
                {
                   // CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
                    Issucceed = store.CreateMembership(membershipIdentity);
                    var userStore = new FrontUserTable<FrontIdentityUser>();
                    membershipIdentity.PaperWork = "1";
                    userStore.AddPaperworkToMembership_Schedule(membershipIdentity);
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error(ex.Message + "\r\n" + ex.StackTrace);
                    return Json(new { success = false, msg = ex.Message });
                }
                var user = store.FindByNameAsync(model.PhoneNumber).Result;


                bool flag = _DbSession.DealerMembershipStorager.IsPersonalUser(model.IdentityNumber);
                if (flag)
                {
                    List<Car> Vins = new List<Car>();
                    var intergration =
                        _AppContext.CarServiceUserApp.GetIntegralByIdentity(membershipIdentity.IdentityNumber,
                            membershipIdentity.IsPay.ToString(), Vins); //添加多少积分
                    UpdateUserIntegralNewMember(user, intergration, model.IsAutoJoin, Vins); //添加积分
                }
                else
                {
                    _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, user.UserName,
               new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
                }
                AddMembershipDealerRecord(store, membershipIdentity);
               
            //}

            if (!Issucceed)
            {
                //var message = "";
                //foreach (var error in CreateResult.Errors)
                //{
                //    message += error;
                //}
                return Json(new { success = false, msg = "添加失败" });
            }

            return Json(new { success = true, msg = "操作成功，新账号已添加！" });
        }
        

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateMembership(MembershipModel model)
        {
            
            bool Issucceed = false;
            IdentityResult CreateResult = null;
            bool ActiveResult;
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                {
                    if (e.FirstOrDefault() != null)
                        msg += e.FirstOrDefault().ErrorMessage + "\n";
                });
                return Json(new { success = false, msg = msg });
            }
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (model.IdentityNumber.Length != 15 && model.IdentityNumber.Length != 18)
            {
                return Json(new { success = false, msg = "输入的身份证号码不正确！" });
            }
            Regex reg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            if (!reg.IsMatch(model.IdentityNumber))
            {
                return Json(new { success = false, msg = "输入的身份证号码不正确！" });
            }

            if (model.IsAutoJoin == null)
            {
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.PhoneNumber, model.ValideCode);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { success = false, msg = "验证码错误或已过期，请重新获取" });
                }
            }

            string birthday = "";
            int Gender = -1;
            int Age = -1;
            if (model.IdentityNumber.Length==15)
            {
                 birthday = "19" + model.IdentityNumber.Substring(6, 2) + "-" + model.IdentityNumber.Substring(8, 2) + "-" + model.IdentityNumber.Substring(10, 2);
                //[1]男 [2]:女
                 Gender = (Convert.ToInt32(model.IdentityNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                 Age = DateTime.Now.Year-Convert.ToInt32("19" + model.IdentityNumber.Substring(6, 2));
            }
            if (model.IdentityNumber.Length==18)
            {
                birthday = model.IdentityNumber.Substring(6, 4) + "-" + model.IdentityNumber.Substring(10, 2) + "-" + model.IdentityNumber.Substring(12, 2);
                Gender = (Convert.ToInt32(model.IdentityNumber.Substring(16,1)) % 2 == 0 ? 2 : 1);
                Age = DateTime.Now.Year-Convert.ToInt32(model.IdentityNumber.Substring(6, 4));
            }
            
            if (store.IsIdentityNumberRepeate(model.IdentityNumber))
                return Json(new { success = false, msg = "系统中已存在此身份证号" });
       
            if (store.CheckUserNameIsExist(model.PhoneNumber))
                return Json(new { success = false, msg = "系统中已存在此手机号" });

                var mlevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(model.IdentityNumber));
                var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber).ToList<Car>();
                //if (carList.Count==0)
                //{
                //    return Json(new { success = false, msg = "认证不成功，CRM系统录入信息有误或出库时间在2天以内，请核实信息正确无误后2天后查看" });
                //}
            var membershipIdentity = new Vcyber.BLMS.Entity.UserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    No = _AppContext.MemberNumberApp.GetNumber("1"),
                    NickName = CommonUtilitys.GetNikeName(),
                    SystemMType = (carList != null && carList.Count > 0 ? (int)MembershipType.WhitCar : (int)MembershipType.WhitoutCar),
                    RealName = model.RealName,
                    //NickName = model.NickName,
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    IdentityNumber = model.IdentityNumber,
                    Mid = model.IdentityNumber,
                    Password = "Bm" + model.PhoneNumber.Substring(5),//这里由原来的身份证改为手机号后6微
                    Status = (int)MembershipStatus.Nomal,
                    CreateTime = DateTime.Now.ToLongTimeString(),
                    CreatedPerson = this.User.Identity.Name,
                    MType = (int)MembershipType.WhitCar,
                    MLevel = mlevel,// (int)MemshipLevel.CommonCard,//级别
                    IsPay = (model.Agree ? (int)MembershipPayStatus.Paid : 0),//经销商新增的会员均为已缴纳100付费 //支付状态
                    ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                    ActiveWay = (int)MembershipActiveWay.ManageWeb, //后台提交激活流程
                    IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                    MLevelBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
                    MLevelInvalidDate = mlevel > 10 ? DateTime.Parse(DateTime.Now.ToShortDateString()).AddYears(1) : DateTime.Parse(DateTime.MaxValue.ToShortDateString()),
                    PasswordHash = membershipManager.PasswordHasher.HashPassword("Bm" + model.PhoneNumber.Substring(5)),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    AuthenticationTime = ((carList != null && carList.Count > 0) ? DateTime.Now : Convert.ToDateTime("1900-1-1")),
                  AuthenticationSource= ((carList != null && carList.Count > 0) ?this.User.Identity.Name:""),
                    Birthday = birthday,
                    Gender=Gender+"",
                    Age=Age
                };
                if (model.IsAutoJoin != null)
                {
                    membershipIdentity.IsPay = (int)MembershipPayStatus.Paid;
                }
            
                try
                {
                    Issucceed = _AppContext.DealerMembershipApp.CreateMembership(membershipIdentity);//store.CreateMembership(membershipIdentity);
                    if (!Issucceed)
                    {
                        return Json(new { success = false, msg = "插入数据失败,请联系管理员" });
                    }
                    var user = store.FindByNameAsync(model.PhoneNumber).Result;
                    if (model.Agree || model.IsAutoJoin != null)//符合缴费条件
                    {
                        bool flag = _DbSession.DealerMembershipStorager.IsPersonalUser(model.IdentityNumber);
                        if (flag)
                        {
                            List<Car> Vins = new List<Car>();
                            var intergration =
                                _AppContext.CarServiceUserApp.GetIntegralByIdentity(membershipIdentity.IdentityNumber,
                                    membershipIdentity.IsPay.ToString(), Vins); //添加多少积分
                            UpdateUserIntegralNewMember(user, intergration, model.IsAutoJoin, Vins); //添加积分
                        }
                        else
                        {
                            _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, user.UserName,
                       new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
                        }

                    }
                    else
                    {
                        _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, model.PhoneNumber, new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
                    }

                    _DbSession.DealerMembershipStorager.AddMembershipDealerRecord(user.Id, this.User.Identity.Name);
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error(ex.Message + "\r\n" + ex.StackTrace);
                    return Json(new { success = false, msg = ex.Message });
                }
                if (carList.Count == 0)
                {
                    return Json(new { success = true, msg = "注册成功！可进入店内会员查看信息。但当前认证车主匹配不成功，CRM系统录入信息有误或出库时间在2天以内，请核实信息正确无误后2天后查看" });
                }
            return Json(new { success = true, msg = "操作成功，新账号已添加！" });
        }

        [HttpPost]
        public JsonResult CreateMembershipNormal(MembershipModel model)
        {
            IdentityResult CreateResult = null;
            bool ActiveResult;
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                {
                    if (e.FirstOrDefault() != null)
                        msg += e.FirstOrDefault().ErrorMessage + "\n";
                });
                return Json(new { success = false, msg = msg });
            }

            //手机验证码校验
            var valideCodeResult =
                _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.PhoneNumber, model.ValideCode);
            if (!valideCodeResult.IsSuccess)
                return Json(new { success = false, msg = valideCodeResult.Message });

            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.IsIdentityNumberRepeate(model.IdentityNumber))
                return Json(new { success = false, msg = "系统中已存在此身份证号" });
            if (store.CheckNickNameIsExist(model.NickName))
                return Json(new { success = false, msg = "系统中已存在此用户名，请更换一个新的用户名" });
            if (store.CheckUserNameIsExist(model.PhoneNumber))
                return Json(new { success = false, msg = "系统中已存在此手机号" });

            var member = store.FindByNameAsync(model.PhoneNumber).Result;

            if (member == null || string.IsNullOrEmpty(member.Id))
            {
                var membershipIdentity = new FrontIdentityUser
                {
                    RealName = model.RealName,
                    NickName = model.NickName,
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    IdentityNumber = model.IdentityNumber,
                    Password = "Bm" + model.IdentityNumber.Substring(model.IdentityNumber.Length - 6, 6),
                    Mid = model.IdentityNumber,
                    Status = (int)MembershipStatus.Nomal,
                    CreateTime = DateTime.Now.ToLongTimeString(),
                    CreatedPerson = this.User.Identity.Name,
                    MType = (int)MembershipType.WhitCar,//车主
                    MLevel = (int)MemshipLevel.CommonCard,//级别
                    IsPay = (int)MembershipPayStatus.NotPay,//经销商新增的会员均为已缴纳100付费
                    ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                    ActiveWay = (int)MembershipActiveWay.ManageWeb, //后台提交激活流程
                    IsNeedModifyPw = (int)MembershipNeedModifyPw.No
                };
                CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);

                var user = store.FindByNameAsync(model.PhoneNumber).Result;

                UpdateNickNameForUse(model.NickName);

                //int blueBean;
                //int expiric;
                //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, user.Id, (MemshipLevel)user.MLevel, out blueBean);
                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, user.Id, out expiric);
            }
            else
            {
                ActiveResult = WithoutCarMemberSubmit(model);
                var user = store.FindByNameAsync(model.PhoneNumber).Result;
                //UpdateUserIntegral(user);//添加积分
            }

            //var result = store.CreateAsync(membership);
            if (CreateResult != null && !CreateResult.Succeeded)
            {
                var message = "";
                foreach (var error in CreateResult.Errors)
                {
                    message += error;
                }
                return Json(new { success = false, msg = message });
            }

            // 添加轮询服务
            //1.用户轮询车辆并成为会员
            // _AppContext.AirportServiceApp.MembershipMonitor(model.IdentityNumber);

            //2.用户轮询车辆并添加积分
            //_AppContext.AirportServiceApp.MembershipMonitorIntegral(model.IdentityNumber);

            return Json(new { success = true, msg = "操作成功，新账号已添加！" });
        }

        private void UpdateNickNameForUse(string nickName)
        {
            try
            {
                BLMS_Entities_Connection entity = new BLMS_Entities_Connection();
                var nick = entity.NickNameLibrary.Where(e => e.NickName == nickName).FirstOrDefault();
                if (nick != null)
                {
                    nick.IsUse = 1;
                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }
        }



        private void UpdateUserIntegralNewMember(FrontIdentityUser user, int Integral, string remark, List<Car> Vins)
        {

            //var item = 0;
            //if(carlist!=null && carlist.Count()>0)
            //{
            //    item = (carlist.Count()==1?"1":2);
            //}

            #region   会员体系升级


            IEnumerable<Car> carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(user.IdentityNumber);
            int cartype = 30;
            if (carList != null && carList.Count() > 0)
            {
                cartype = (carList.Count() == 1 ? 9 : 10);
            }

            //bool flag = _DbSession.DealerMembershipStorager.IsComUser(user.IdentityNumber);
            //if (flag)
            //{
            //    return;
            //}
            if (remark != null && remark.ToLower() == "y")
            {
                remark = "一键入会返积分";
            }
            else
            {
            remark = "经销商入会返积分";
           
            }
            _AppContext.UserIntegralApp.Add(new UserIntegral
            {
                CreateTime = DateTime.Now,
                datastate = 0, //这里逻辑默认0
                UpdateTime = DateTime.Now,
                userId = user.Id,
                value = Integral, //新购4000积分
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                integralSource = cartype + "",
                remark = remark
            });

            //添加统计记录表 
            _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
            {
                CreateTime = DateTime.Now,
                datastate = 0, //这里逻辑默认0
                UpdateTime = DateTime.Now,
                userId = user.Id,
                value = Integral, //新购4000积分
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                integralSource = cartype + "",
                remark = remark
            });
            _AppContext.UserIntegralApp.AddVin(Vins, user.IdentityNumber, user.Id);
            if (Integral > 0)
            {
                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.后台新增会员, user.UserName,
                    new string[] { (Integral / 10) + "", user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
            }
            else
            {
                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, user.UserName,
                    new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
            }

            #endregion
        }



        /// <summary>
        /// 添加用户积分
        /// </summary>
        /// <param name="identityNumber"></param>
        private void UpdateUserIntegral(FrontIdentityUser user)
        {
            IEnumerable<Car> carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(user.IdentityNumber);
            var s9CarList = carList.Where(e => e.CarCategory == Common.CommonConst.SONATA9 /*|| e.CarCategory == Common.CommonConst.SONATA9_1 || e.CarCategory == Common.CommonConst.SONATA9_2*/);
            int s9Count = s9CarList.Count();
            if (carList.Count() >= 2 && s9CarList.Count() >= 1)//增购
            {
                _AppContext.UserIntegralApp.Add((new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0,
                    UpdateTime = DateTime.Now,
                    userId = user.Id,
                    value = 7000 //新购4000积分

                }), EIRuleType.增购);
                //添加记录
                BLMS_Entities_Connection entity = new BLMS_Entities_Connection();
                UserCarIntegralRecord newEntity = new UserCarIntegralRecord
                {
                    UserId = user.Id,
                    CarCategory = CommonConst.SONATA9,
                    Value = 7000,
                    CreateTime = DateTime.Now,
                    VIN = s9CarList.First().VIN
                };
                entity.UserCarIntegralRecord.Add(newEntity);
                entity.SaveChanges();

                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.换购增购LF车主, user.UserName, new string[] { user.UserName, "Bm" + user.IdentityNumber.Substring(user.IdentityNumber.Length - 6) });
            }
            else if (carList.Count() == 1 && s9CarList.Count() == 1) //新购
            {
                _AppContext.UserIntegralApp.Add((new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0,
                    UpdateTime = DateTime.Now,
                    userId = user.Id,
                    value = 4000

                }), EIRuleType.新购);
                //添加记录
                BLMS_Entities_Connection entity = new BLMS_Entities_Connection();
                UserCarIntegralRecord newEntity = new UserCarIntegralRecord
                {
                    UserId = user.Id,
                    CarCategory = CommonConst.SONATA9,
                    Value = 4000,
                    CreateTime = DateTime.Now,
                    VIN = s9CarList.First().VIN
                };

                entity.UserCarIntegralRecord.Add(newEntity);
                entity.SaveChanges();

                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.新购LF车主, user.UserName, new string[] { user.UserName, "Bm" + user.IdentityNumber.Substring(user.IdentityNumber.Length - 6) });
            }
        }

        /// <summary>
        /// 会员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult MembershipList(MembershipQueryViewModel query)
        {
            var totalCount = 0;
            //如果当前用户不是管理员admin就取当前用户的DealerId，否则使用query中的DealerId
            var DealerIdItem = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(DealerIdItem))//经销商登录
            {
                query.DealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            }


            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var membershipList = frontUserStore.GetUsers(query.ToEntity(), out totalCount).Result;

            foreach (var item in membershipList)
            {
                //获取用户积分 
                item.jf = _AppContext.UserIntegralApp.GetTotalIntegral(item.Id);
                item.Isadmin = "";
                item.Job = (item.AuthenticationTime == Convert.ToDateTime("1900-1-1") ? "" : item.AuthenticationTime.ToString("yyyy-MM-dd"));//job这里代替认证时间
                if (!string.IsNullOrEmpty(DealerIdItem)) //经销商登录
                {
                    item.Isadmin = "none";
                }
            }

            membershipList.ForEach((a) =>
            {
                a.StatusName = ((MembershipStatus)a.Status).GetDiscribe();
                a.MLevelName = ((MemshipLevel)a.MLevel).GetDiscribe();
            });
            var result = new { data = membershipList, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 会员精确查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult MembershipExtraList(MembershipQueryViewModel query)
        {
            if (string.IsNullOrEmpty(query.VIN))
                return Json(new { success = false, msg = "请填写VIN码" }, JsonRequestBehavior.AllowGet);
            //if (string.IsNullOrEmpty(query.RealName))
            //    return Json(new { success = false, msg = "请填写车主姓名" }, JsonRequestBehavior.AllowGet);
            var totalCount = 0;
            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var membershipList = frontUserStore.GetExtraUsers(query.ToEntity(), out totalCount).Result;
            membershipList.ForEach((a) =>
            {
                a.StatusName = ((MembershipStatus)a.Status).GetDiscribe();
                a.MLevelName = ((MemshipLevel)a.MLevel).GetDiscribe();
            });
            var result = new { success = true, data = membershipList, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 服务记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="serviceType"></param>
        ///  <returns></returns>
        public JsonResult GetServiceList(string userid, int? serviceType, int skip, int count)
        {
            var listModel = new List<RepairRecordModel>();
            var totalCount = 0;

            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var user = frontUserStore.FindByIdAsync(userid).Result;


            var identityNumber = "";
            if (user != null)
            {
                identityNumber = user.IdentityNumber;
                if (string.IsNullOrEmpty(identityNumber))
                {
                    return Json(new { data = listModel, total_count = totalCount }, JsonRequestBehavior.AllowGet);
                }
            }

            var serviceTypeValue = "";
            if (serviceType == null || serviceType == -1)
            {
                serviceTypeValue = string.Empty;
            }
            else
            {
                serviceTypeValue = ((ERepairServiceType)serviceType).ToString();
                if (serviceTypeValue.StartsWith("_"))
                {
                    serviceTypeValue = serviceTypeValue.Remove(1);
                }


            }
            var list = _AppContext.RepairRecordApp.GetRepirRecordList(identityNumber, null, null, null, serviceTypeValue, skip, count, null, out totalCount);

            if (list != null)
            {
                foreach (var item in list)
                {
                    var model = new RepairRecordModel();
                    model.Id = item.Id;
                    model.IdentityNumber = item.IdentityNumber;
                    model.ServiceType = item.ServiceType;
                    // model.ReserveType = item.ReserveType;
                    model.RepairTime = item.RepairTime.ToShortDateString();
                    model.FinishTime = item.FinishTime.ToShortDateString();
                    model.Status = item.Status;
                    listModel.Add(model);
                }


            }
            var result = new { data = listModel, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 积分明细
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult GetScoreList(string userid, int skip, int count)
        {
            var totalCount = 0;
            var scoreList = _AppContext.UserIntegralApp.SelectIntegralRecordPage(userid, skip + 1, count, out totalCount);
            var result = new { data = scoreList, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 卡券列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="skip"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GetCardList(string userid, int skip, int count)
        {
            var totalCount = 0;
            var cardList = _AppContext.AirportServiceApp.SelectSNCardByUser(userid);
            totalCount = cardList.Count();
            var result = new { data = cardList, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预约记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult GetScheduleList(string userid, int page, int count)
        {
            var totalCount = 0;
            var scheduleServiceList = _AppContext.ScheduleServiceApp.QueryOrders(userid, page, count);
            var result = new { data = scheduleServiceList.Items, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region==用户凭证审核功能======================================================
        /// <summary>
        /// 上传凭证的用户列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProofCheck()
        {
            return View();
        }

        /// <summary>
        /// 获取已上传凭证的用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserProofList(string phone, string mlevel, string paperwork, string identitynumber, string status, string StrCreateTime, string StrEnd, int pageCount, int currentPage)
        {
            ReturnResult result = new ReturnResult();
            int totalCount;
            var proofListData = new List<MemberShipProofModel>();

            var createTime = Convert.ToDateTime(StrCreateTime);
            var end = Convert.ToDateTime(StrEnd);

            if (end < createTime)
            {
                result.IsSuccess = false;
                result.Message = "填写日期数据错误";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {


                var proofList = _AppContext.InvoiceForReserveApp.GetSearch(phone, mlevel, paperwork, identitynumber, status, StrCreateTime, StrEnd, pageCount+1, currentPage, out totalCount);

                if (proofList != null)
                {

                    foreach (var item in proofList)
                    {
                        var proofModel = new MemberShipProofModel();

                        proofModel.PhoneNumber = item.PhoneNumber;
                        proofModel.UserName = item.RealName;
                        proofModel.PaperWork = item.PaperWork;
                        proofModel.IdentityNumber = item.IdentityNumber;
                        proofModel.CreateTime = item.CreateTime.ToString();
                        proofModel.MLevelDisc = item.MLevelDisc;
                        proofModel.ApproveStatusDiscribe = item.ApproveStatusDiscribe;

                        proofModel.Id = item.Id;
                        proofModel.MembershipId = item.MembershipId;
                        proofModel.ImageProofFront = item.ImageProofFront;
                        proofModel.ImageProofVerso = item.ImageProofVerso;
                        proofModel.ImageProofByHand = item.ImageProofByHand;
                        proofModel.ApproveStatus = item.ApproveStatus;

                        proofListData.Add(proofModel);
                    }

                }
                result.IsSuccess = true;
                result.Data = proofListData;
                result.Errors = totalCount;//总条数用Error表示

                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult ExportProofList(string phone, string mlevel, string paperwork, string identitynumber, string status, string StrCreateTime, string StrEnd, int pageCount, int currentPage)
        {
             //ReturnResult result = new ReturnResult();
            int totalCount;
            var proofListData = new List<MemberShipProofModel>();
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("凭证审核表");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("手机号");
            row1.CreateCell(1).SetCellValue("姓名");
            row1.CreateCell(2).SetCellValue("证件类型");
            row1.CreateCell(3).SetCellValue("证件号");
            row1.CreateCell(4).SetCellValue("审核状态");
            row1.CreateCell(5).SetCellValue("会员等级");
            row1.CreateCell(6).SetCellValue("提交时间");
            DateTime createTime = DateTime.Now;
            DateTime end = DateTime.Now;
            if (!string.IsNullOrEmpty(StrCreateTime) || !string.IsNullOrEmpty(StrEnd))
            {
                createTime = Convert.ToDateTime(StrCreateTime);
                end = Convert.ToDateTime(StrEnd);
            }

            if (end < createTime)
            {
                //result.IsSuccess = false;
                //result.Message = "填写日期数据错误";
                //return Json(result, JsonRequestBehavior.AllowGet);
                return Content("<script type='text/javascript'>alert('填写日期数据错误');window.opener=null;window.open('','_self');window.close();</script>");
            }

            var proofList = _AppContext.InvoiceForReserveApp.GetSearch(phone, mlevel, paperwork, identitynumber, status, StrCreateTime, StrEnd, 0, 500000, out totalCount).ToList();

            if (proofList != null)
            {
                for (int i = 0; i < proofList.Count(); i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(proofList[i].PhoneNumber);
                    rowtemp.CreateCell(1).SetCellValue(proofList[i].RealName);
                    rowtemp.CreateCell(2).SetCellValue(proofList[i].PaperWork);
                    rowtemp.CreateCell(3).SetCellValue(proofList[i].IdentityNumber);
                    rowtemp.CreateCell(4).SetCellValue(proofList[i].ApproveStatusDiscribe);
                    rowtemp.CreateCell(5).SetCellValue(proofList[i].MLevelDisc);
                    rowtemp.CreateCell(6).SetCellValue(proofList[i].CreateTime.ToString());
                }

            }
            //sheet1.SetColumnWidth(0, 150 * 20);
            //sheet1.SetColumnWidth(1, 150 * 20);
            //sheet1.SetColumnWidth(2, 150 * 20);
            //sheet1.SetColumnWidth(3, 150 * 20);
            //sheet1.SetColumnWidth(4, 150 * 20);
            //sheet1.SetColumnWidth(5, 150 * 20);
            //sheet1.SetColumnWidth(6, 150 * 20);
            for (int i = 0; i <= proofList.Count()+1; i++)
            {
                sheet1.AutoSizeColumn(i);
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "用户凭证审核表.xls");
            //return Content("<script type='text/javascript'>alert('导出数据出错');window.opener=null;window.open('','_self');window.close();</script>");
        }
         //<summary>
        //根据UserID获得会员上传的凭证信息
         //</summary>
         //<param name="userId"></param>
         //<returns></returns>
        public JsonResult GetProofInfo(string userId)
        {

            var data = _AppContext.InvoiceForReserveApp.GetProofInfoById(userId);
            var proofModel = new MemberShipProofModel();
            if (data != null) 
            { 
                proofModel.MembershipId = data.MembershipId;
                proofModel.ImageProofFront = data.ImageProofFront;
                proofModel.ImageProofVerso = data.ImageProofVerso;
                proofModel.ImageProofByHand = data.ImageProofByHand;
                proofModel.ApproveStatus = data.ApproveStatus;
            }
            return Json(proofModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 审核用户照片
        /// 更改审核状态 0：未审核 1：审核通过 2：审核未通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpdateProofStatus(string id, string status)
        {
            var data = _AppContext.InvoiceForReserveApp.GetProofInfoById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "无此数据，无法修改！" });
            }
            else
            {
            var success = _AppContext.InvoiceForReserveApp.UpdateProofStatus(id, status);
            if (success)
            {
                if (status == "1")
                {
                    _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
                    {
                        UserId = id,
                        MsgType = MessageType.IntegralConsum,
                        MsgContent = string.Format(@" 您好，您上传的凭证已于{0}通过审核。", DateTime.Now)
                    });
            
                        return Json(new { success = true, msg = "审核通过！" });
                }
                if (status == "2")
                {
                    _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
                    {
                        UserId = id,
                        MsgType = MessageType.IntegralConsum,
                        MsgContent = string.Format(@" 您好，您的凭证未通过审核，请重新上传符合要求的凭证图片。", DateTime.Now)
                    });
                        return Json(new { success = true, msg = "审核未通过！" });
                }
            }
            else
            {
                    return Json(new { success = false, msg = "未能完成修改！" });
            }
        }
            return Json(new { success = false, msg = "未能完成修改！" });
        }
        /// <summary>
        /// 删除选中的要删除的认证用户
        /// </summary>
        /// <param name="ids">选中的列表中的ID数组</param>
        /// <returns></returns>
        public JsonResult DeleteProofInfos(string[] ids)
        {
            if(ids==null)
            {
                return Json(new { success = false, msg = "请选择要删除的数据！" });
            }
            bool flag = false;
            for (int i=0; i < ids.Length; i++)
            {
                var result = _AppContext.InvoiceForReserveApp.DeleteProofInfo(ids[i]);
                flag = result;
            }
            if (flag)
            {
                return Json(new { success = true, msg = "删除成功！" });
            }
            else
            {
                return Json(new { success = false, msg = "删除失败！" });

            }
        }
        #endregion
        /// <summary>
        /// 消费记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GetConsumeList(string userid, int page, int count)
        {
            var totalCount = 0;
            var mem = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userid).Result; //会员
            var consumeList =
                _AppContext.ConsumeApp.QueryOrders(
                    new ConsumeQueryParamEntity
                    {
                        Phone = mem.PhoneNumber,
                        PointApproveStatus = EPointApproveStatus.All,
                        PointStatus = EPointStatus.All,
                        HasAttachment = EAttachmentStatus.All
                    }, page, count);
            var result = new { data = consumeList.Items, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 会员认证申请列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMembershipApprovingList(string PayNumber, string phoneNumber, string IdentityNumber, string ApproveType, int skip, int count, int? IsPay, decimal? Amount, string DealerId, string PaperWork, string VINNumber,string No)
        {
            var totalCount = 0;

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (string.IsNullOrEmpty(user.DealerId))//管理员
            {
                if (DealerId != null)
                {
                    user.DealerId = DealerId;
                }
            }
            if (user.DealerId == "-1")
                user.DealerId = string.Empty;
            IsPay = IsPay == null ? -1 : IsPay.Value;
            Amount = Amount == null ? -1 : Amount.Value;
            var approvingList = new FrontUserStore<FrontIdentityUser>().FindApprovingMembership(PayNumber, phoneNumber, user.DealerId, IdentityNumber, ApproveType, skip, count, out totalCount, IsPay.Value, Amount.Value, PaperWork, VINNumber,No);
            approvingList.Result.ForEach((e) =>
            {
                e.StatusName = ((MembershipApplyApprovalStatus)(int.Parse(e.Status))).GetDiscribe();
            });

            var result = new { data = approvingList.Result, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ApprovalMembershipRequest(string id, string phone, string SubmitTime)
        {
            string message;
            var appro = new FrontUserStore<FrontIdentityUser>().ManagerMembershipRequest(id, out message, phone, SubmitTime);

            var result = new { success = appro.Result, msg = message };

            //if (appro.Result)
            //{
            //    var user = UserManager.FindById(User.Identity.GetUserId());

            //    // 添加轮询服务
            //    //1.用户轮询车辆并成为会员
            //    _AppContext.AirportServiceApp.MembershipMonitor(user.identitynumber);

            //    //2.用户轮询车辆并添加积分
            //    _AppContext.AirportServiceApp.MembershipMonitorIntegral(user.UserName);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 拒绝审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult RejectMembershipRequest(string id)
        {
            string message;
            var appro = new FrontUserStore<FrontIdentityUser>().RejectMembershipRequest(id, out message);
            var result = new { success = appro.Result, msg = message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 会员认证列表页面 --认证失败页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MembershipRequestFailed()
        {
            return View();
        }

        /// <summary>
        /// 认证失败列表查询 --认证失败页面
        /// </summary>
        /// <param name="status"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GetApprovedFailedList(int? status, string phonenumber, int? start, int? count)
        {
            int total = 0;
            var approveFailedList = new List<MemberShipRequestFailedModel>();
            var list = new FrontUserStore<FrontIdentityUser>().GetMembershipRequestFailedList(status, phonenumber, start ?? 0, count ?? 0, out total).Result;
            if (list != null)
            {
                foreach (var obj in list)
                {
                    var item = new MemberShipRequestFailedModel();
                    item.Id = obj.Id;
                    item.UserId = obj.UserId;
                    item.UserName = obj.UserName;
                    item.IdentityNumber = obj.IdentityNumber;
                    item.PayNumber = obj.PayNumber;
                    item.OperationTime = obj.OperationTime;
                    item.Operator = obj.Operator;
                    item.Status = obj.Status;
                    item.RequestTime = obj.RequestTime;
                    item.VIN = obj.VIN;
                    item.IsPay = obj.IsPay;
                    item.CarCategory = obj.CarCategory;
                    approveFailedList.Add(item);
                }
            }

            var result = new { data = approveFailedList.OrderBy(e => e.Status), pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 认证失败列表导出 --认证失败页面
        /// </summary>
        /// <param name="clState">处理状态</param>
        /// <returns></returns>
        public ActionResult ExportMembershipFailedData(int? clState)
        {
            try
            {
                var approveFailedList = new List<MemberShipRequestFailedModel>();
                var list = new FrontUserStore<FrontIdentityUser>().GetMembershipRequestFailedListAll(clState).Result;
                if (list != null)
                {
                    if (list.Count == 0)
                    {
                        return Content("<script type='text/javascript'>alert('当前没有可导出的数据');window.opener=null;window.open('','_self');window.close();</script>");
                    }
                    foreach (var obj in list)
                    {
                        var item = new MemberShipRequestFailedModel();
                        item.Id = obj.Id;
                        item.UserId = obj.UserId;
                        item.UserName = obj.UserName;
                        item.IdentityNumber = obj.IdentityNumber;
                        item.PayNumber = obj.PayNumber;
                        item.OperationTime = obj.OperationTime;
                        item.Operator = obj.Operator;
                        item.Status = obj.Status;
                        item.RequestTime = obj.RequestTime;
                        item.VIN = obj.VIN;
                        item.IsPay = obj.IsPay;
                        item.CarCategory = obj.CarCategory;
                        approveFailedList.Add(item);
                    }
                }
                List<string> propertyName = new List<string> { "Id", "UserName", "RequestTime", "IdentityNumber", "IsPay", "CarCategory", "PayNumber" };
                List<string> columName = new List<string> { "编号", "姓名", "申请时间", "身份证号", "是否已付费", "车型", "付款码" };

                string fileName = string.Format("认证失败列表{0}", DateTime.Now.ToString("yyyyMMdd")) + ".xls";

                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("认证失败列表");
                sheet1.SetColumnWidth(1, 150 * 36);
                sheet1.SetColumnWidth(2, 150 * 36);
                sheet1.SetColumnWidth(3, 300 * 36);
                if (approveFailedList != null)
                {

                    //给sheet1添加第一行的头部标题
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    for (int i = 0; i < columName.Count; i++)
                    {
                        row1.CreateCell(i).SetCellValue(columName[i]);
                    }


                    //将数据逐步写入sheet1各个行
                    for (int i = 0; i < approveFailedList.Count(); i++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                        rowtemp.CreateCell(0, NPOI.SS.UserModel.CellType.String).SetCellValue(approveFailedList[i].Id.ToString());
                        rowtemp.CreateCell(1).SetCellValue(approveFailedList[i].UserName);
                        rowtemp.CreateCell(2).SetCellValue(approveFailedList[i].RequestTime);
                        rowtemp.CreateCell(3).SetCellValue(approveFailedList[i].IdentityNumber);
                        rowtemp.CreateCell(4).SetCellValue(approveFailedList[i].IsPay);
                        rowtemp.CreateCell(5).SetCellValue(approveFailedList[i].CarCategory);
                        rowtemp.CreateCell(6).SetCellValue(approveFailedList[i].PayNumber);
                    }
                }

                else
                {
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    row1.CreateCell(0).SetCellValue("导出数据出错");

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return File(ms, "application/ms-excel", fileName);
            }
            catch (Exception e)
            {
                return Content("<script type='text/javascript'>alert('导出数据出错');window.opener=null;window.open('','_self');window.close();</script>");
            }

        }

        /// <summary>
        /// 更新认证请求失败状态 --认证失败页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpdateRequestStauts(string id)
        {
            string message;
            var appro = new FrontUserStore<FrontIdentityUser>().UpdateMembershipRequestStatus(id, HttpContext.User.Identity.Name, out message);
            var result = new { success = appro.Result, msg = message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 重新激活认证 --认证失败页面
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public JsonResult Activate(string id)
        //{
        //    string message;
        //    var appro = new FrontUserStore<FrontIdentityUser>().Activate(id, HttpContext.User.Identity.Name, out message);
        //    var result = new { success = appro, msg = message };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 更改会员身份证号 --会员列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult UpdateIdentityNumberBy4S(string id, string identityNumber)
        {
            string message;
            var appro = new FrontUserStore<FrontIdentityUser>().UpdateIdentityNumberBy4S(id, identityNumber, HttpContext.User.Identity.Name, out message);
            var result = new { success = appro, msg = message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置用户手机号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult updatePhoneNumberModal(string id, string phoneNumber)
        {
            string message = string.Empty;
            var appro = new FrontUserStore<FrontIdentityUser>().updatePhoneNumberModal(id, phoneNumber, HttpContext.User.Identity.Name, out message);
            var result = new { success = appro, msg = message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult UpdatePassword(string id, string password)
        {
            try
            {
                var userStore = new FrontUserStore<FrontIdentityUser>();
                string newpw = UserManager.PasswordHasher.HashPassword(password);

                var user = userStore.FindByIdAsync(id);
                user.Result.PasswordHash = newpw;
                user.Result.IsNeedModifyPw = 0;
                user.Result.UpdateTime = DateTime.Now.ToString();
                userStore.UpdateAsync(user.Result);
                return Json(new { code = "200", msg = "重置成功", });
            }
            catch (Exception ex)
            {
                return Json(new { code = "400", msg = "重置失败", });
            }
        }
      
        /// <summary>
        /// 查询会员财富
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FindWorth(string id)
        {
            int integralValue = _AppContext.UserIntegralApp.GetTotalIntegral(id);
            //int blueBeanValue = _AppContext.UserBlueBeanApp.GetTotalBlueBean(id);
            //int empiricValue = _AppContext.UserEmpiricApp.TotalValue(id);
            //return Json(new { Integral = integralValue, BlueBean = blueBeanValue, Empiric = empiricValue });
            return Json(new { Integral = integralValue });
        }

        public JsonResult SaveWorth(string id, int mlevel, string integral)
        {
            int integralValue;
            //int blueBeanValue;
            //int empiricValue;

            if (!this.ValidateWordth(integral, out integralValue))
            {
                return Json(new { status = "fail", message = "请输入合法的积分值" });
            }

            //if (!this.ValidateWordth(blueBean, out blueBeanValue))
            //{
            //    return Json(new { status = "fail", message = "请输入合法的蓝豆值" });
            //}

            //if (!this.ValidateWordth(empiric, out empiricValue))
            //{
            //    return Json(new { status = "fail", message = "请输入合法的经验值" });
            //}

            //if (mlevel == -1 && integralValue == 0 && blueBeanValue == 0 && empiricValue == 0)
            //{
            //    return Json(new { status = "fail", message = "请输入要修改的财富值" });
            //}
            if (mlevel == -1 && integralValue == 0)
            {
                return Json(new { status = "fail", message = "请输入要修改的财富值" });
            }

            if (mlevel != -1)
            {
                var userStore = new FrontUserStore<FrontIdentityUser>();
                var userData = userStore.FindByIdAsync(id);

                if (userData != null && userData.Result != null)
                {
                    userData.Result.MLevel = mlevel;
                    userStore.UpdateAsync(userData.Result);
                }
            }

            if (integralValue != 0)
            {
                _AppContext.UserIntegralApp.Add(new UserIntegral() { CreateTime = DateTime.Now, datastate = 0, integralSource = EIRuleType.管理员下发.ToInt32().ToString(), remark = "管理员下发,操作人员：" + HttpContext.User.Identity.Name, UpdateTime = DateTime.Now, userId = id, usevalue = 0, value = integralValue });
                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral() { CreateTime = DateTime.Now, datastate = 0, integralSource = EIRuleType.管理员下发.ToInt32().ToString(), remark = "管理员下发,操作人员：" + HttpContext.User.Identity.Name, UpdateTime = DateTime.Now, userId = id, usevalue = 0, value = integralValue });
            }

            //if (blueBeanValue != 0)
            //{
            //    _AppContext.UserBlueBeanApp.Add(new UserblueBean() { CreateTime = DateTime.Now, datastate = 0, integralSource = "12", remark = "管理员下发,操作人员：" + HttpContext.User.Identity.Name, UpdateTime = DateTime.Now, userId = id, usevalue = 0, value = blueBeanValue });
            //}

            //if (empiricValue != 0)
            //{
            //    _AppContext.UserEmpiricApp.Add(new UserEmpiricRecord() { CreateTime = DateTime.Now, DataState = 0, Remark = "操作人员：" + HttpContext.User.Identity.Name, SourceId = "管理员下发", UserId = id, UseValue = 0, Value = empiricValue });
            //}

            return Json(new { status = "success", message = "财富升级成功" });
        }

        /// <summary>
        /// 更改会员身份证号 --认证失败页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        public JsonResult UpdateIdentityNumber(string id, string identityNumber)
        {
            string message;
            var appro = new FrontUserStore<FrontIdentityUser>().UpdateIdentityNumber(id, identityNumber, HttpContext.User.Identity.Name, out message);
            var result = new { success = appro, msg = message };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 创建论坛用户
        ///// </summary>
        ///// <param name="model"></param>
        //private void CreateForumUser(MembershipModel model)
        //{
        //    var forum = new ForumRegister
        //    {
        //        UserName = model.PhoneNumber,
        //        Password = model.IdentityNumber.Substring(model.IdentityNumber.Length - 6, 6),
        //        PageBoardId = 1,
        //        Email = string.IsNullOrEmpty(model.Email) ? string.Format("{0}@mail.com", model.PhoneNumber) : model.Email,
        //        Question = "default",
        //        Answer = "default"
        //    };
        //    forum.CreateForumUser();
        //}

        /// <summary>
        /// 会员账户认证通过激活 --会员列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult MembershipActive(string id)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var membership = userStore.FindByIdAsync(id).Result;
            membership.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
            membership.IsPay = (int)MembershipPayStatus.Paid;
            userStore.UpdateAsync(membership);

            //建立4S店与银卡会员的归属关系
            var currMananger = this.UserManager.FindById(this.User.Identity.GetUserId());
            if (currMananger != null && !string.IsNullOrEmpty(currMananger.DealerId) && currMananger.DealerId != "-1")
            {
                userStore.AddMembershipDealerRecord(id, currMananger.DealerId);
            }


            return Json(new { success = true });
        }

        ///// <summary>
        ///// 会员账户状态激活 --会员列表页面
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public JsonResult UserWithoutCarActive(MembershipModel model)
        //{
        //    return WithoutCarMemberSubmit(model);
        //}

        /// <summary>
        /// 非车主激活索九会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool WithoutCarMemberSubmit(MembershipModel model)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var member = store.FindByNameAsync(model.PhoneNumber).Result;

            //member.MType = (int)MembershipType.Sonata9;//索9车主
            member.MLevel = (int)MemshipLevel.SilverCard;//级别
            member.IsPay = (int)MembershipPayStatus.Paid;
            member.ApprovalStatus = (int)MembershipApprovalStatus.Activing; //激活中
            member.IdentityNumber = model.IdentityNumber;
            member.NickName = model.NickName;
            member.ActiveWay = (int)MembershipActiveWay.ManageWeb;
            store.UpdateAsync(member);

            AddMembershipDealerRecord(store, member);
            return true;
        }

        /// <summary>
        /// 会员状态冻结 --会员列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FrozenMembership(string id)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var membership = userStore.FindByIdAsync(id).Result;
            membership.Status = (int)MembershipStatus.Freezon;//冻结账户
            userStore.UpdateAsync(membership);
            return Json(new { success = true });
        }

        /// <summary>
        /// 会员账户状态激活 --会员列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UserStatusActive(string id)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var membership = userStore.FindByIdAsync(id).Result;
            membership.Status = (int)MembershipStatus.Nomal;//激活账户
            userStore.UpdateAsync(membership);
            return Json(new { success = true });
        }
        //冻结积分
        public JsonResult FreezeUserintegral(string id)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var membership1 = userStore.FindByIdUserintegral(id).Result;
            membership1.datastate = (int)Userintegraldatastate.nocan;//冻结积分
            userStore.UpdateUserintegral(membership1);
            return Json(new { success = true });
        }
        /// <summary>
        /// 删除会员 --会员列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var membership = userStore.FindByIdAsync(id).Result;

            //删除积分
            var integralResult = _AppContext.UserIntegralApp.DeleteUserIntegral(id);

            //删除用户
            var result = userStore.DeleteAsync(membership);

            return Json(new { success = true });
        }

        /// <summary>
        /// 获取会员车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetMembershipCars(string id)
        {
            var carList = _AppContext.CarServiceUserApp.SelectCarListByUserId(id);
            return Json(carList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNickName()
        {
            BLMS_Entities_Connection entities = new BLMS_Entities_Connection();
            var nickNameList = entities.NickNameLibrary.Where(e => e.IsUse == 0).ToList();
            Random r = new Random();
            var nickName = nickNameList[(r.Next(0, nickNameList.Count - 1))];
            return Json(new { success = true, Name = nickName }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 缴费获取积分会员 列表导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult ExportApproving(string phoneNumber, string PayNumber, string IdentityNumber, string ApproveType, int? IsPay, decimal? Amount, int Skip, int Count, string PaperWork, string DealerId, string VINNumber,string No)
        {
            int total = 0;
            int isPay = -1;
            decimal amount = 0;
            if (IsPay != null)
            {
                int.TryParse(IsPay.ToString(), out isPay);
            }
            if (Amount != null)
            {
                Decimal.TryParse(Amount.ToString(), out amount);
            }

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("会员列表");
            var user = UserManager.FindById(User.Identity.GetUserId());
            string dealerId = string.Empty;
            if (string.IsNullOrEmpty(DealerId))
            {
                dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            }
            else
            {
                dealerId = DealerId;
            }
             
            var approvingList = new FrontUserStore<FrontIdentityUser>().FindApprovingMembership(PayNumber, phoneNumber, dealerId, IdentityNumber, ApproveType, 0,50000, out total, isPay, (decimal)amount, PaperWork, VINNumber,No);
            approvingList.Result.ForEach((e) =>
            {
                e.StatusName = ((MembershipApplyApprovalStatus)(int.Parse(e.Status))).GetDiscribe();
            });

            var result = approvingList.Result.ToList();

            //获取list数据
            //var cards = _AppContext.AirportServiceApp.SelectSNCardCount(phoneNumber, state ?? 0, out cou).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("店代码");
            row1.CreateCell(1).SetCellValue("手机号");
            row1.CreateCell(2).SetCellValue("证件号码");
            row1.CreateCell(3).SetCellValue("应付金额");
            row1.CreateCell(4).SetCellValue("车主姓名");
            row1.CreateCell(5).SetCellValue("是否已付费");
            row1.CreateCell(6).SetCellValue("付款码");
            row1.CreateCell(7).SetCellValue("状态");
            row1.CreateCell(8).SetCellValue("申请提交时间");
           //row1.CreateCell(9).SetCellValue("会员卡号");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < result.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(result[i].DealerId);
                rowtemp.CreateCell(1).SetCellValue(result[i].PhoneNumber);
                rowtemp.CreateCell(2).SetCellValue(result[i].IdentityNumber);
                rowtemp.CreateCell(3).SetCellValue(result[i].Amount.ToString());
                rowtemp.CreateCell(4).SetCellValue(result[i].RealName);
                rowtemp.CreateCell(5).SetCellValue(result[i].IsPay);
                rowtemp.CreateCell(6).SetCellValue(result[i].PayNumber);
                rowtemp.CreateCell(7).SetCellValue(result[i].StatusName);
                rowtemp.CreateCell(8).SetCellValue(result[i].SubmitTime);
                //rowtemp.CreateCell(9).SetCellValue(result[i].No);
              
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "MembershipApproving.xls");
        }

        /// <summary>
        /// 用户列表 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ExportUser(MembershipQueryViewModel query)
        {
            var totalCount = 0;
            List<FrontIdentityUser> resultList = null;
            //if (string.IsNullOrEmpty(query.VIN))
            //{
                query.DealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
                var frontUserStore = new FrontUserStore<FrontIdentityUser>();
                var membershipList = frontUserStore.ExportGetUsers(query.ToEntity(), out totalCount).Result;
                membershipList.ForEach((a) =>
                {
                    a.StatusName = ((MembershipStatus)a.Status).GetDiscribe();
                    a.MLevelName = ((MemshipLevel)a.MLevel).GetDiscribe();
                });
                resultList = membershipList;
           // }
            //else
            //{
            //    var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            //    var membershipList = frontUserStore.GetExtraUsers(query.ToEntity(), out totalCount).Result;
            //    membershipList.ForEach((a) =>
            //    {
            //        a.StatusName = ((MembershipStatus)a.Status).GetDiscribe();
            //        a.MLevelName = ((MemshipLevel)a.MLevel).GetDiscribe();
            //    });
            //    resultList = membershipList;
            //}
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("用户列表");
            if (resultList != null)
            {

                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("手机号");
                row1.CreateCell(1).SetCellValue("积分");
                row1.CreateCell(2).SetCellValue("用户名");
                row1.CreateCell(3).SetCellValue("证件号");
                row1.CreateCell(4).SetCellValue("会员卡号");
                row1.CreateCell(5).SetCellValue("付款码");
                row1.CreateCell(6).SetCellValue("创建时间");
                row1.CreateCell(7).SetCellValue("用户状态");
                row1.CreateCell(8).SetCellValue("用户等级");
                row1.CreateCell(9).SetCellValue("创建人");
                row1.CreateCell(10).SetCellValue("车主类型");
                row1.CreateCell(11).SetCellValue("认证时间");
                row1.CreateCell(12).SetCellValue("年龄");
                row1.CreateCell(13).SetCellValue("性别");
                row1.CreateCell(14).SetCellValue("城市");
                row1.CreateCell(15).SetCellValue("地区");
                row1.CreateCell(16).SetCellValue("认证来源");
                row1.CreateCell(17).SetCellValue("车架号");


                //将数据逐步写入sheet1各个行
                for (int i = 0; i < resultList.Count(); i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0, NPOI.SS.UserModel.CellType.String).SetCellValue(resultList[i].PhoneNumber.ToString());
                    rowtemp.CreateCell(1).SetCellValue(_AppContext.UserIntegralApp.GetTotalIntegral(resultList[i].Id));
                    rowtemp.CreateCell(2).SetCellValue(resultList[i].NickName);
                    rowtemp.CreateCell(3).SetCellValue(resultList[i].IdentityNumber);
                    rowtemp.CreateCell(4).SetCellValue(resultList[i].No);
                    rowtemp.CreateCell(5).SetCellValue(resultList[i].PayNumber);
                    rowtemp.CreateCell(6).SetCellValue(resultList[i].CreateTime);
                    rowtemp.CreateCell(7).SetCellValue(resultList[i].StatusName);
                    rowtemp.CreateCell(8).SetCellValue(resultList[i].MLevelName);
                    rowtemp.CreateCell(9).SetCellValue(resultList[i].CreatedPerson);
                    rowtemp.CreateCell(10).SetCellValue(resultList[i].AccntType);
                    rowtemp.CreateCell(11).SetCellValue(resultList[i].AuthenticationTime == Convert.ToDateTime("1900-1-1") ? "" : resultList[i].AuthenticationTime.ToString("yyyy-MM-dd"));
                    rowtemp.CreateCell(12).SetCellValue(resultList[i].Age);
                    rowtemp.CreateCell(13).SetCellValue(resultList[i].GenderName);
                    rowtemp.CreateCell(14).SetCellValue(resultList[i].City);
                    rowtemp.CreateCell(15).SetCellValue(resultList[i].Area);
                    rowtemp.CreateCell(16).SetCellValue(resultList[i].AuthenticationSource);
                    rowtemp.CreateCell(17).SetCellValue(resultList[i].VIN);
                }
            }

            else
            {
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("导出数据出错");

            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            //return File(ms, "application/vnd.ms-excel", "MembershipUser.xls");
            return File(ms, "application/ms-excel", "MembershipUser.xls");
        }
        //-----Private-----------------------------------------------------------------------//
        private void AddMembershipDealerRecord(FrontUserStore<FrontIdentityUser> store, FrontIdentityUser membershipIdentity)
        {
            try
            {
                var newUser = store.FindByNameAsync(membershipIdentity.UserName).Result;
                var currMananger = this.UserManager.FindById(this.User.Identity.GetUserId());
                if (currMananger != null && !string.IsNullOrEmpty(currMananger.DealerId) && currMananger.DealerId != "-1")
                {
                    store.AddMembershipDealerRecord(newUser.Id, currMananger.DealerId);
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message);
            }
        }

        /// <summary>
        /// 验证财富值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="breadValue"></param>
        /// <returns></returns>
        private bool ValidateWordth(string value, out int breadValue)
        {
            breadValue = 0;

            int av = 0;
            int.TryParse(value, out av);

            if (value.Equals("0") || string.IsNullOrEmpty(value) || (av > 0 && !value.StartsWith("+") && !value.StartsWith("-")))
            {
                breadValue = av;
                return true;
            }

            //Regex regex = new Regex("^(\\+{1}|\\-{1}){1}[1-9]{1,}[0-9]{1,}$");
            Regex regex = new Regex("^(\\+{1}|\\-{1}){1}[0-9]{1,}$");
            if (regex.IsMatch(value))
            {
                string tempValue = regex.Match(value).Value;
                tempValue = tempValue.StartsWith("+") ? tempValue.Substring(1) : tempValue;
                breadValue = int.Parse(tempValue);
                return true;
            }

            return false;
        }
    }
}