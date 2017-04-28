using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PetaPoco;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using System.IO;
using Vcyber.BLMS.Common.City;
using Webdiyer.WebControls.Mvc;
using Vcyber.BLMS.Entity.BLMSMoney;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class MyCenterController : Controller
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

        #region ==== 构造函数 ====

        public MyCenterController()
        {
        }

        public MyCenterController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion


        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/Index"
                });
            }
            ApplicationUser _result = null;
            int _totalScore = 0;

            int _totalCard = 0;
            int _reIntegralType = -1;//0： D+S 首次 100元 返 4000积分  1：  D+S 增换 返7000积分  2： 非 D+S  首 50 元 返 2000积分 3 ： 增换 返4000积分
            int _unReadMsgCount = 0;
            int _registerUserGetIntegral = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                string userid = this.User.Identity.GetUserId();
                var createTime = DateTime.Now;
                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(userid);

                _result = UserManager.FindById(userid);
                //_reIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(_result.IdentityNumber);
                //判断是：-1：不符合返积分条件  0： D+S 首次 100元 返 4000积分  1：  D+S 增换 返7000积分  2： 非 D+S  首 50 元 返 2000积分 3 ： 增换 返4000积分
                _reIntegralType = (int)_AppContext.CarServiceUserApp.GetRegisterIntegralTypeByIdentity(_result.IdentityNumber,createTime);
                #region 计算新注册并且绑定车的用户应获取的积分
                _registerUserGetIntegral = _AppContext.CarServiceUserApp.GetRegisterIntegralByIdentity(_result.IdentityNumber, createTime);
                #endregion

                _unReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(userid);
            }
            bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(_result.IdentityNumber);
            if (!flag)
            {
                _reIntegralType = -1;
            }
            ViewBag.totalScore = _totalScore;
            ViewBag.totalCard = _totalCard;

            ViewBag.registerUserGetIntegral = _registerUserGetIntegral/10;

            ViewBag.reIntegralType = _reIntegralType;
            ViewBag.UnReadMsgCount = _unReadMsgCount;
            ViewBag.totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
            ViewBag.registerIdentityNo = _result.IdentityNumber;
            return View(_result);
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserInfo()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/SetUserInfo"
                });
            }
            ApplicationUser _result = null;
            int _totalScore = 0;
            int _unReadMsgCount = 0;
            int _totalCard = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                string userid = this.User.Identity.GetUserId();
                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(userid);
                _result = UserManager.FindById(this.User.Identity.GetUserId());
                _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
                _unReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(userid);
            }
            ViewBag.totalScore = _totalScore;
            ViewBag.totalCard = _totalCard;
            ViewBag.UnReadMsgCount = _unReadMsgCount;

            //ViewData["provinceList"] = CityService.Instance.GetProvince();
            return View(_result);
        }

        /// <summary>
        /// 服务记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MyServiceRecords()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/MyServiceRecords"
                });
            }
            ApplicationUser _result = null;
            int _totalScore = 0;
            int _totalBlueBean = 0;
            int _totalCard = 0;
            Page<CSSonataServiceV> _serviceV = new Page<CSSonataServiceV>();
            List<RepairRecord> _records = new List<RepairRecord>();
            Page<CSConsume> _consumeV = new Page<CSConsume>();
            var totalCount = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();

                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                _result = UserManager.FindById(this.User.Identity.GetUserId());

                _serviceV = _AppContext.ScheduleServiceApp.QueryUserOrdersByType(this.User.Identity.GetUserId(), 1,
                    10000);
                _records =
                    _AppContext.RepairRecordApp.GetRepirRecordList(_result.IdentityNumber, null, null, null, null, 0,
                        10000, null, out totalCount).ToList();
                ConsumeQueryParamEntity consumeQueryEntity = new ConsumeQueryParamEntity()
                {
                    Phone = _result.PhoneNumber,
                    HasAttachment = EAttachmentStatus.All,
                    PointApproveStatus = EPointApproveStatus.All,
                    PointStatus = EPointStatus.All,
                    EConsumeType = EConsumeType.ALL
                };
                _consumeV = _AppContext.ConsumeApp.QueryOrders(consumeQueryEntity, 1, 10000);

            }
            ViewBag.ServiceV = _serviceV;
            ViewBag.RepairRecord = _records;
            ViewBag.ConsumeV = _consumeV;
            ViewBag.totalScore = _totalScore;
            ViewBag.totalBlueBean = _totalBlueBean;
            ViewBag.totalCard = _totalCard;
            return View(_result);
        }

        /// <summary>
        /// 钱包记录
        /// </summary>
        /// <returns></returns>
        public ActionResult WalletRecords(FormCollection collection ,int pageIndex = 1)
        {
            #region old
            //int pageIndex = 0;
            //if (pageNum == null || pageNum <= 0)
            //{
            //    pageNum = 1;
            //}

            //pageIndex = pageNum ?? 1;

            //if (!this.User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("LogonPage", "Account", new
            //    {
            //        returnUrl = "/MyCenter/WalletRecords"
            //    });
            //}
            //ApplicationUser _result = null;
            //int _totalScore = 0;
            //int _totalBlueBean = 0;
            //int _totalCard = 0;
            //int _totalEmpiric = 0;
            //List<UserblueBean> _userBlueBeanList = new List<UserblueBean>();
            //List<UserIntegral> _userIntegralList = new List<UserIntegral>();
            ////用户积分消费记录
            //var _tradeflowList = new List<Tradeflow>();
            //List<Vcyber.BLMS.Entity.SNCard> _userCardList = new List<Entity.SNCard>();

            //if (this.User.Identity.IsAuthenticated)
            //{
            //    _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
            //    _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
            //    _result = UserManager.FindById(this.User.Identity.GetUserId());
            //    _userBlueBeanList = _AppContext.UserBlueBeanApp.GetList(this.User.Identity.GetUserId()).ToList();
            //    _userIntegralList = _AppContext.UserIntegralApp.GetList(this.User.Identity.GetUserId()).ToList();
            //    //用户积分消费记录
            //    _tradeflowList = _AppContext.TradeFlowApp.GetList(this.User.Identity.GetUserId()).ToList();
            //    _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
            //    _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
            //}

            //ViewBag.totalScore = _totalScore;
            //ViewBag.totalBlueBean = _totalBlueBean;
            ////ViewBag.totalCard = _userCardList==null?0:_userCardList.Count();
            //ViewBag.totalCard = _totalCard;
            //ViewBag.totalEmpiric = _totalEmpiric;

            //ViewBag.totalCount = _userBlueBeanList == null ? 0 : _userBlueBeanList.Count();

            //ViewBag.blueBeanList = _userBlueBeanList.Skip(pageIndex * PAGESIZE_LIST - PAGESIZE_LIST).Take(PAGESIZE_LIST).ToList();
            //ViewBag.integralList = _userIntegralList;
            ////用户积分消费记录
            //foreach (var item in _tradeflowList)
            //{
            //    if (item.tradeintegral == 0)
            //        continue;
            //    var ent = new UserIntegral();
            //    ent.CreateTime = item.CreateTime;
            //    ent.value = item.tradeintegral;
            //    if (item.TradeType == 1)
            //        ent.integralSource = "订单支付";
            //    else if (item.TradeType == 2)
            //        ent.integralSource = "订单退款";
            //    ent.IntegralType = 2;//消费积分 标记

            //    if (!_userIntegralList.Contains(ent))
            //        _userIntegralList.Add(ent);
            //}
            ////按时间排序
            //var query = from items in _userIntegralList orderby items.CreateTime select items;
            //ViewBag.userIntegralTradeflowList = query.ToList();

            //ViewBag.pageIndex = pageNum ?? 1;
            //ViewBag.pageSize = PAGESIZE_LIST;
            #endregion

            int pageSurplus = 0;
            if (!string.IsNullOrEmpty(collection["pageSurplus"]))
                pageSurplus =int.Parse(collection["pageSurplus"]);
            int total = 0;
            IEnumerable<UserIntegralRecord> userIntegralList = _AppContext.UserIntegralApp.SelectUserIntegral(this.User.Identity.GetUserId(), pageIndex, PAGESIZE_LIST, out total,out pageSurplus);
            userIntegralList = userIntegralList.ToPagedList(pageIndex, PAGESIZE_LIST);
            PagedList<UserIntegralRecord> _pageresult = new PagedList<UserIntegralRecord>(userIntegralList, pageIndex, PAGESIZE_LIST, total);
            ViewData["curUserEntity"] = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
            ViewBag.pageSurplus = pageSurplus;
            return View(_pageresult);
        }

        public ActionResult WalletRecordsForPager(int? pageNum)
        {
            int pageIndex = 0;
            if (pageNum == null || pageNum <= 0)
            {
                pageNum = 1;
            }

            pageIndex = pageNum ?? 1;

            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/WalletRecords"
                });
            }

            List<UserblueBean> _userBlueBeanList = new List<UserblueBean>();
            if (this.User.Identity.IsAuthenticated)
            {
                _userBlueBeanList = _AppContext.UserBlueBeanApp.GetList(this.User.Identity.GetUserId()).ToList();
            }

            //ViewBag.blueBeanList = _userBlueBeanList;

            ViewBag.totalCount = _userBlueBeanList == null ? 0 : _userBlueBeanList.Count();
            ViewBag.pageIndex = pageNum ?? 1;
            ViewBag.pageSize = PAGESIZE_LIST;

            return View(_userBlueBeanList.Skip(pageIndex * PAGESIZE_LIST - PAGESIZE_LIST).Take(PAGESIZE_LIST).ToList());
        }

        /// <summary>
        /// 设置用户基础信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SetBaseInfo()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/Mycenter/Index"
                });
            }
            ApplicationUser _userEntity = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.FaceImage = Session["FaceImage"];

            ViewData["provinceList"] = CityService.Instance.GetProvince();

            return View(_userEntity);
        }

        //上传头像
        [HttpPost]
        public ActionResult UpLoadUserFaceImg(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Content("没有文件！", "text/plain");
            }
            var fileName = Path.Combine(Request.MapPath("~/Upload"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
            try
            {
                file.SaveAs(fileName);

                imagePath = "/Upload/" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                ViewBag.FaceImage = imagePath;

                Session["FaceImage"] = imagePath;

                var _userEntity = UserManager.FindById(this.User.Identity.GetUserId());

                int _value = 0;
                _AppContext.BreadApp.EmpiricBread(EEmpiricRule.更换头像, _userEntity.Id, out _value);
                //Response.Write("<script>window.parent.update_upload_img_src('" + imagePath + "')</script>");
                return  new HttpStatusCodeResult(404);
            }
            catch
            {
                return Content("上传异常！", "text/plain");
            }
        }

        [HttpPost]
        public ActionResult SetBaseInfoSave(ApplicationUser userEntity)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    code = "401",
                    msg = "账号登陆异常"
                });
            }


            userEntity.FaceImage = Session["FaceImage"] as string ;

            //验证用户名的唯一性
            var user = new FrontUserStore<FrontIdentityUser>().FindByNickNameAsync(userEntity.NickName);
            if (user != null && user.Result != null)
            {
                if (this.User.Identity.GetUserId() != user.Result.Id)
                {
                    return Json(new { code = "401", msg = "用户名已存在,请更换" });
                }
            }
            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            //个人信息
            _curUser.RealName = userEntity.RealName;
            _curUser.NickName = userEntity.NickName;
            _curUser.Birthday = userEntity.Birthday;
            _curUser.Email = userEntity.Email;
            _curUser.Address = userEntity.Address;
            _curUser.FaceImage = userEntity.FaceImage;
            _curUser.Provency = userEntity.Provency;
            _curUser.City = userEntity.City;
            _curUser.Area = userEntity.Area;
            _curUser.Gender = userEntity.Gender;
            _curUser.IdentityNumber = userEntity.IdentityNumber;

            UserManager.Update(_curUser);
#warning ==== 同步车主论坛 ====
            try
            {

                BBSUtil.CheckAndCreateDefaultBBSMember(_curUser);
            }
            catch (Exception e)
            {

            }


            //详细信息
            _curUser.PaperWork = userEntity.PaperWork;
            _curUser.MainContact = userEntity.MainContact;
            _curUser.MainTelePhone = userEntity.MainTelePhone;
            _curUser.TelePhone = userEntity.TelePhone;
            _curUser.TransactionTime = userEntity.TransactionTime;
            _curUser.Industry = userEntity.Industry;
            _curUser.Job = userEntity.Job;
            _curUser.IsMarriage = userEntity.IsMarriage;
            _curUser.MarriageDay = userEntity.MarriageDay;
            _curUser.Educational = userEntity.Educational;
            _curUser.SendSms = userEntity.SendSms;
            _curUser.SendLetter = userEntity.SendLetter;
            _curUser.MakePhone = userEntity.MakePhone;
            _curUser.SendEmail = userEntity.SendEmail;
            _curUser.Remark = userEntity.Remark;

            var appro = new FrontUserStore<FrontIdentityUser>().Update_Or_Insert_Membership_Schedule(_curUser);

            int _value = 0;

            //_AppContext.BreadApp.BlueBeanBread(EBRuleType.完善个人信息, _curUser.Id, (MemshipLevel)_curUser.MLevel, out _value);
            //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善个人信息, _curUser.Id, out _value);

            if (!string.IsNullOrEmpty(_curUser.MainContact) && !string.IsNullOrEmpty(_curUser.MainTelePhone) && !string.IsNullOrEmpty(_curUser.TelePhone) && !string.IsNullOrEmpty(_curUser.TransactionTime) && !string.IsNullOrEmpty(_curUser.Industry) && !string.IsNullOrEmpty(_curUser.Job) && !string.IsNullOrEmpty(_curUser.MarriageDay) && !string.IsNullOrEmpty(_curUser.Educational))
            {
                //_AppContext.BreadApp.BlueBeanBread(EBRuleType.完善详细信息, _curUser.Id, (MemshipLevel)_curUser.MLevel, out _value);
                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善详细信息, _curUser.Id, out _value);
            }




            return Json(new
            {
                code = "200",
                msg = "保存成功"
            });
        }

        public ActionResult MyCenterResetPasswd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyCenterResetPasswdSave(ApplicationUser userEntity, string captcha)
        {
            ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(userEntity.PhoneNumber, captcha);
            if (!_captchaResult.IsSuccess)
            {
                return Json(new
                {
                    code = "402",
                    msg = "验证码失败"
                });
            }

            //FrontUserStore<FrontIdentityUser> store = new FrontUserStore<FrontIdentityUser>();

            //UserManager<FrontIdentityUser> UserManager = new UserManager<FrontIdentityUser>(store);
            //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(userEntity.NewPassword);

            //store.SetPasswordHashAsync(user, hashedNewPassword);

            //user.IsNeedModifyPw = 0;
            //var result = userManager.Update(user);

            //return Ok(result.Succeeded);

            ApplicationUser _resetPasswdUserEntity = UserManager.FindByName(userEntity.PhoneNumber);
            if (_resetPasswdUserEntity == null || string.IsNullOrEmpty(_resetPasswdUserEntity.Id))
            {
                return Json(new
                {
                    code = "401",
                    msg = "用户账号登陆异常"
                });
            }
            _resetPasswdUserEntity.IsNeedModifyPw = 0;
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(userEntity.Password);
            _resetPasswdUserEntity.Password = userEntity.Password;
            _resetPasswdUserEntity.PasswordHash = hashedNewPassword;

            UserManager.Update(_resetPasswdUserEntity);
            return Json(new
            {
                code = "200"
            });
        }




        public ActionResult SetAccount()
        {


            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/Mycenter/SetUserInfo"
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult IsExist_Mobile(string mobile)
        {
            ApplicationUser _userEntity = UserManager.FindByName(mobile);
            if (_userEntity != null)
            {
                return Json(new
                {
                    code = "400",
                    msg = "用户账号已存在"
                });
            }
            return Json(new
            {
                code = "200",
                msg = "用户账不存在"
            });
        }


        [HttpPost]
        public ActionResult SetAccountSave(string mobile, string sms_code)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    code = "400",
                    msg = "账号登陆异常"
                });
            }

            ApplicationUser _userEntity = UserManager.FindByName(mobile);
            if (_userEntity != null)
            {
                return Json(new
                {
                    code = "401",
                    msg = "用户账号已存在"
                });
            }

            ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(mobile, sms_code);
            if (!_captchaResult.IsSuccess)
            {
                return Json(new
                {
                    code = "402",
                    msg = "验证码失败"
                });
            }

            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            _curUser.UserName = mobile;
            _curUser.PhoneNumber = mobile;
            UserManager.Update(_curUser);

            return Json(new
            {
                code = "200",
                msg = "保存成功"
            });
        }



        /// <summary>
        /// 会员申请制度
        /// </summary>
        /// <returns></returns>
        public ActionResult Progress()
        {
            ApplicationUser _userEntity = null;
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/Index"
                });
            }
            _userEntity = UserManager.FindById(this.User.Identity.GetUserId());
            return View(_userEntity);
        }

        /// <summary>
        /// 账户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountInfo()
        {
            ApplicationUser _result = UserManager.FindById(this.User.Identity.GetUserId());
            return View(_result);
        }
        /// <summary>
        /// 车主信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CarOwnerInfo()
        {
            ApplicationUser _result = UserManager.FindById(this.User.Identity.GetUserId());
            return View(_result);
        }
        /// <summary>
        /// 我的爱车
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCar()
        {
            IEnumerable<Car> _mycarList = null;
            ApplicationUser _userEntity = UserManager.FindById(this.User.Identity.GetUserId());
            if (_userEntity.SystemMType == 2 || _userEntity.SystemMType == 3)
            {
                _mycarList = _AppContext.CarServiceUserApp.SelectCarListByUserId(this.User.Identity.GetUserId());
            }
            ViewData["mycarlist"] = _mycarList;
            return View(_userEntity);
        }
        #region==用户凭证上传业务=====
        /// <summary>
        /// 上传身份凭证视图
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadProof()
        {
            return View();
        }
        /// <summary>
        /// 用户上传身份凭证
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadUserProof()
        {

            return Json(new { code = 304, msg = "请正确输入证件号码" });
        }
        
        /// <summary>
        /// 判断当前登录用户是否已上传过凭证
        /// </summary>
        /// <returns></returns>
        public JsonResult IsUploadProof()
        {
            ReturnResult result = new ReturnResult() { IsSuccess = false, Data = 400, Message = "未知错误" };

            //获取当前用户帐号
            string userId = User.Identity.GetUserId();
            //判断用户是否已绑定身份证号
            //userProof 未绑定身份证号直接不显示凭证上传业务

            //判断是否已上传过
            var isupload = _AppContext.InvoiceForReserveApp.GetProofInfoById(userId);

            if (isupload != null)
            {
                
                result.IsSuccess = true;

                result.Data = isupload;

                result.Message = "您已上传过凭证。";
                //判断凭证是否已审核通过 1:已通过 2:未通过  
                //已通过凭证审核业务只显示进度条
                if (isupload.ApproveStatus == 1)
                {
                    //用错误码表示 205 已审核通过
                    result.Errors = 205;
                }
                //未通过凭证审核业务上传图片部分显示，不显示进度条部分(ApproveStatus==2)
                //未审核的记录被删除，上传功能激活，不显示进度条部分(isupload.ApproveStatus==0&&isupload.IsDelete==1)
                if(isupload.ApproveStatus==2||(isupload.ApproveStatus==0&&isupload.IsDelete==1))
                {
                    result.Errors = 204;

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// 用户上传凭证 提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]

        public ActionResult ImgUpload()
        {
            string ErrorTips = "上传图片格式不对";
            string ContentTypeTips = "单张图片最大不超过3M";
            //图片的扩展名
            string _imageExtendName = string.Empty;
            //获取当前用户帐号
            string userId = User.Identity.GetUserId();
            //图片访问的域名
            string _imageDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["ImgPath"];
            //图片的保存路径
            string _imagePath = HttpContext.Server.MapPath("/upload/userImage");
            //图片的访问url
            string _imageUrl = string.Empty;
            //string _serviceType = Request["ServiceType"];
            string[] imgPath = new string[3];
            HttpFileCollectionBase _files = Request.Files;


            if (_files.Count < 3)
            {
                return Json(new { code = 400, msg = "请上传完整的身份凭证" }, "text/html", JsonRequestBehavior.AllowGet);
            }

            try 
            {
                if (_files.Count > 0)
                {
                    for (int i = 0; i < _files.Count; i++)
                    {
                        string _tmpFileName = _files[i].FileName;
                        //校验后缀名
                        if (!string.IsNullOrEmpty(_tmpFileName))
                        {
                            _imageExtendName = _tmpFileName.Substring(_tmpFileName.LastIndexOf("."));
                        }
                        if (!".jpg".Equals(_imageExtendName) && !".png".Equals(_imageExtendName) && !".jpeg".Equals(_imageExtendName))
                        {
                            return Json(new { code = 400, imgurl = imgPath, msg = ErrorTips }, "text/html", JsonRequestBehavior.AllowGet);
                        }
                        //校验图片大小
                        int _fileLength = _files[i].ContentLength;
                        int _maxFileLength = 3 * 1024 * 1024;
                        if (_fileLength >= _maxFileLength)
                        {
                            return Json(new { code = 400, imgurl = imgPath, msg = ContentTypeTips }, "text/html", JsonRequestBehavior.AllowGet);
                        }

                        //图片的名称
                        string _imageName = Guid.NewGuid().ToString() + _imageExtendName;

                        //图片的全名称
                        string _imageFullName = System.IO.Path.Combine(_imagePath, _imageName);
                        _files[i].SaveAs(_imageFullName);

                        //_imageUrl = string.Format("{0}/{1}/{2}", _imageDomain, "Upload", _imageName);
                        imgPath[i] = string.Format("{0}/{1}", "/upload/userImage", _imageName);
                        //imgPath[i] = _imageFullName;
                    }
                    bool updateResult=false;
                    int _result=-1;
                    //修改 在原有基础上更新
                    var  result = _AppContext.InvoiceForReserveApp.GetProofInfoById(userId);

                    if (result != null)
                    {
                        
                        result.MembershipId = userId;
                        //if (imgPath[2] == null)
                        //{
                           
                        //    result.ImageProofFront = imgPath[0];
                        //    result.ImageProofVerso = imgPath[2];
                        //    result.ImageProofByHand = imgPath[1];
                        //}
                        //else
                        //{}
                            result.ImageProofFront = imgPath[0];
                            result.ImageProofVerso = imgPath[1];
                            result.ImageProofByHand = imgPath[2];
                        
                        updateResult = _AppContext.InvoiceForReserveApp.UpdateUserProofRecord(result);
                    }
                    else//新增
                    {

                        
                         Vcyber.BLMS.Entity.InvoiceForReserve _entity = new Vcyber.BLMS.Entity.InvoiceForReserve();

                        _entity.MembershipId = userId;
                        _entity.ApproveStatus = (int)EApproveStatus.NoBegin;
                        _entity.CreateTime = DateTime.Now;
                        _entity.UpdateTime = DateTime.Now;
                        _entity.IsDelete = 0;
                        //if (imgPath[2] == null)
                        //{
                        //    _entity.ImageProofFront = imgPath[0];
                        //    _entity.ImageProofVerso = imgPath[2];
                        //    _entity.ImageProofByHand = imgPath[1];
                        //}
                        //else
                        //{}
                            _entity.ImageProofFront = imgPath[0];
                            _entity.ImageProofVerso = imgPath[1];
                            _entity.ImageProofByHand = imgPath[2];
                        
                        _result = _AppContext.InvoiceForReserveApp.InsertUserProofRecord(_entity);
                    }
                    if((updateResult==true) ||(_result>0))
                    {
                        return Json(new { code = 200, imgurl = imgPath, msg = "上传成功" }, "text/html", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception ex)
            {

                Common.LogService.Instance.Info(string.Format("错误信息:{0}", ex.Message));
            }
            return Json(new { code = 400, imgurl = imgPath, msg = "请上传完整的身份凭证" }, "text/html", JsonRequestBehavior.AllowGet);

        }

        #endregion

        [HttpPost]
        public ActionResult ModifyCarInfo(string vin, string LicencePlate, string Mileage)
        {
            var result = new ReturnResult();
            result.IsSuccess = false;
            result.Message = "更新失败";
            try
            {
                //增加蓝豆及经验值
                if (this.User.Identity.IsAuthenticated)
                {
                    //更新车辆信息
                    result.IsSuccess = _AppContext.CarServiceUserApp.UpdateCarInfo(vin, LicencePlate, Mileage);
                    var _userid = this.User.Identity.GetUserId();
                    var frontUser = UserManager.FindById(_userid);
                    int blueBeanValue;
                    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.完善车辆信息, _userid, (MemshipLevel)frontUser.MLevel, out blueBeanValue);
                    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善车辆信息, _userid, out blueBeanValue);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 我的爱车详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCarDetail(string id)
        {
            int _totalScore = 0;
            int _totalBlueBean = 0;
            int _totalCard = 0;
            int _totalEmpiric = 0;
            int _unReadMsgCount = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
                _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
                _unReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            }
            ViewBag.UnReadMsgCount = _unReadMsgCount;
            ViewBag.totalScore = _totalScore;
            ViewBag.totalBlueBean = _totalBlueBean;
            ViewBag.totalCard = _totalCard;
            ViewBag.totalEmpiric = _totalEmpiric;

            var info = new UserCarDetailModel();
            if (string.IsNullOrEmpty(id.ToString()))
                return null;
            IEnumerable<Car> _mycarList = null;
            ApplicationUser _userEntity = UserManager.FindById(this.User.Identity.GetUserId());
            if (_userEntity.SystemMType == 2 || _userEntity.SystemMType == 3)
            {
                _mycarList = _AppContext.CarServiceUserApp.SelectCarListByUserId(this.User.Identity.GetUserId());
            }
            var myCarDetail = _mycarList.Where(e => e.VIN == id).ToList<Car>();
            if (myCarDetail.Count() > 0)
            {
                info.CarInfo = myCarDetail[0];
                info.CurrentUser = UserManager.FindById(this.User.Identity.GetUserId());
            }
            return View(info);
        }

        /// <summary>
        /// 我的卡券
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCardRecord(string id)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new
                {
                    returnUrl = "/MyCenter/Index"
                });
            }
            //IEnumerable<Entity.SNCard> _result = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId());

            //获取单个候机服务券详情
            var entity = _AppContext.AirportServiceApp.GetCardByUserDetails(this.User.Identity.GetUserId(),id);

            int _totalScore = 0;
            int _totalBlueBean = 0;
            int _totalCard = 0;
            int _totalEmpiric = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
                _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
            }
            ViewBag.totalScore = _totalScore;
            ViewBag.totalBlueBean = _totalBlueBean;
            ViewBag.totalCard = _totalCard;
            ViewBag.totalEmpiric = _totalEmpiric;
            ViewData["curUserEntity"] = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            return View(entity);
        }

        /// <summary>
        /// 去进行车主认证
        /// </summary>
        /// <returns></returns>
        public ActionResult ToCheckCarowner()
        {
            return View();
        }
        /// <summary>
        /// 去进行车主认证-进行认证保存
        /// 我要绑定车辆
        /// </summary>
        /// <returns></returns>
        public JsonResult ToCheckCarownerSave(string identityNumber, int mtype)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            //判断身份证是否合法
            string birthday = string.Empty;
            int Gender = -1;
            int Age = -1;
            if (mtype == (int)Vcyber.BLMS.Entity.Enum.ECustomerIdentificationType.IdentityCard)
            {                
                switch (identityNumber.Length)
                {
                    case 15:
                        birthday = "19" + identityNumber.Substring(6, 2) + "-" + identityNumber.Substring(8, 2) + "-" + identityNumber.Substring(10, 2);
                        Gender = (Convert.ToInt32(identityNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                        Age = DateTime.Now.Year - Convert.ToInt32("19" + identityNumber.Substring(6, 2));
                        break;
                    case 18:
                        birthday = identityNumber.Substring(6, 4) + "-" + identityNumber.Substring(10, 2) + "-" + identityNumber.Substring(12, 2);
                        Gender = (Convert.ToInt32(identityNumber.Substring(16, 1)) % 2 == 0 ? 2 : 1);
                        Age = DateTime.Now.Year - Convert.ToInt32(identityNumber.Substring(6, 4));
                        break;
                    default:
                        return Json(new { code = 304, msg = "请正确输入证件号码" });
                }
                DateTime time;
                if (!DateTime.TryParse(birthday, out time))
                {
                    return Json(new { code = 304, msg = "请正确输入证件号码" });
                }
            }
            string userName = this.User.Identity.GetUserName();
            var gradeUser = UserManager.FindByName(userName);
            gradeUser.PaperWork = mtype.ToString();
            gradeUser.IdentityNumber = identityNumber;
            //获取用户车辆信息
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
            var returnintegral = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(gradeUser.IdentityNumber);
            if (store.IsIdentityNumberRepeate(identityNumber))
            {
                //根据身份证获取用户对象
                var userModel = store.FindByIdentityNumber(identityNumber).Result;
                if (userModel == null || userModel.SystemMType == 2)
                {
                    // string phone = userModel.PhoneNumber.Substring(3, 4);  如果要需求此行注释注意非空判断
                    return Json(new
                    {
                        code = 301,
                        //msg = string.Format("您的证件号已经和{0}绑定,如有疑问请拨打:400-800-1100", userModel.PhoneNumber.Replace(phone,"****"))
                        msg = CommonConst.NOCARTIP
                    });
                }
            }
            if (carList == null || carList.Count() <= 0)
            {
                // UserManager.Update(gradeUser);
                //提示未匹配到您的车辆，请拨打客服电话
                return Json(new
                {
                    code = 301,
                    msg = CommonConst.NOCARTIP
                });
            }
            else
            {
                //查到车时增加车主认证时间
                gradeUser.AuthenticationTime = DateTime.Now;
                gradeUser.AuthenticationSource = "blms_pc_web";
                gradeUser.MLevelBeginDate = DateTime.Now;
            }
            gradeUser.MLevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(identityNumber));
            gradeUser.SystemMType = (int)MembershipType.WhitCar;
            if (!string.IsNullOrEmpty(birthday))
            {
                gradeUser.Birthday = birthday;
            }

            //判断交费级别
            gradeUser.Age = Age;
            gradeUser.Gender = Gender.ToString();
            UserManager.Update(gradeUser);
            var userStore = new FrontUserTable<FrontIdentityUser>();
            userStore.AddPaperworkToMembership_Schedule(gradeUser);

            if (returnintegral != -1)
            {
                return Json(new
                {
                    code = 302,
                    msg = "缴费返积分",
                    reintegralType = returnintegral
                });

            }
            return Json(new
            {
                code = 200,
                msg = "车主认证成功"
            });

        }
        /// <summary>
        /// 激活会员
        /// </summary>
        /// <returns></returns>
        public ActionResult CarownerActive()
        {
            return View();
        }


        /// <summary>
        /// 经销商交费页
        /// </summary>
        /// <returns></returns>
        public ActionResult PayForDealer()
        {

            var _result = UserManager.FindById(this.User.Identity.GetUserId());
            var _reIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(_result.IdentityNumber);

            ViewBag.ReIntegralType = _reIntegralType;
            return View();
        }
        /// <summary>
        /// 向经销商交纳会费
        /// </summary>
        /// <param name="applayDearID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApplayToDealer_Register(string applayDealerID, string type)
        {
            try
            {

                string userName = this.User.Identity.GetUserName();
                var gradeUser = UserManager.FindByName(userName);
                var userStore = new FrontUserStore<FrontIdentityUser>();
                userStore.AddMembershipDealerRecord(gradeUser.Id, applayDealerID);
                userStore.CreateMembershipRequest(gradeUser.Id, gradeUser.IdentityNumber, applayDealerID, string.Empty, "blms");
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 200, data = "向经销商申请交费成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyCustCardIndex(string phoneNumber, string verifyCode)
        {
             string userId = this.User.Identity.GetUserId();
             if (CookieHelper.GetCookie("MyCenterID") == null || (CookieHelper.GetCookie("MyCenterID") != null && CookieHelper.GetCookie("MyCenterID").Value!=userId))
            {
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phoneNumber, verifyCode);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 400, msg = _captchaResult.Message });
                }
            }
            
            List<UserCustomCardModel> list = new List<UserCustomCardModel>();
           
            UserCustomCardModel partnerCustomCardModel = new UserCustomCardModel();
            //未使用
            partnerCustomCardModel.ReceivedCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 1).ToList();

            //已使用
            partnerCustomCardModel.UsedCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 2).ToList();

            //已过期
            partnerCustomCardModel.ExpiredCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 3).ToList();
            list.Add(partnerCustomCardModel);

            Session["msg"] = "noVliade";

            CookieHelper.SetNewCookie("MyCenterID", userId);
            CookieHelper.SetNewCookie("MyCenterEnco", userId);

            return View(list);
        }

        public ActionResult MyCustomCardIndex()
        {
            string userId = this.User.Identity.GetUserId();
            List<UserCustomCardModel> list = new List<UserCustomCardModel>();

            //北京现代卡券
            UserCustomCardModel CustomCardModel = new UserCustomCardModel();
            //未使用
            CustomCardModel.ReceivedCustomCardList = new List<ReturnUserCustomCardModel>();
            CustomCardModel.ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 1).ToList();

            //已使用
            CustomCardModel.UsedCustomCardList = new List<ReturnUserCustomCardModel>();
            CustomCardModel.UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 2).ToList();

            //已过期
            CustomCardModel.ExpiredCustomCardList = new List<ReturnUserCustomCardModel>();
            CustomCardModel.ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 3).ToList();

            list.Add(CustomCardModel);

            //合作商卡券
            UserCustomCardModel partnerCustomCardModel = new UserCustomCardModel();
            //未使用
            partnerCustomCardModel.ReceivedCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 1).ToList();

            //已使用
            partnerCustomCardModel.UsedCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 2).ToList();

            //已过期
            partnerCustomCardModel.ExpiredCustomCardList = new List<ReturnUserCustomCardModel>();
            partnerCustomCardModel.ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 3).ToList();
            list.Add(partnerCustomCardModel);

            //候机服务券(未使用)
            var NoUseList= _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 0, 2).ToList();//第二个参数值卡券是否已经过期：0指未过期；1指已过期
            //候机服务券(已使用)
            var UseList = _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 0, 3).ToList();//第二个参数值卡券是否已经过期：0指未过期；1指已过期
            //候机服务券(已过期)
            var OverUseList = _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 1, 2).ToList();//第二个参数值卡券是否已经过期：0指未过期；1指已过期

            ViewBag.NoUseList = NoUseList;
            ViewBag.UseList = UseList;
            ViewBag.OverUseList = OverUseList;

            ViewData["curUserEntity"] = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
            return View(list);
        }


        public FileResult GetQCode(string id)
        {
            return File(new QrCodeHelper().GetRCode(id), "image/jpg");
        }

        public ActionResult MyCustomCardDetail(string id)
        {
            string userId = this.User.Identity.GetUserId();

            var info = _AppContext.CustomCardInfoApp.GetSingleUserCustomCardInfoByIdAndUserId(id, userId);
            ViewData["curUserEntity"] = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
            //ViewBag.Fileimg = GetQCode(info.CardCode);
            return View(info);
        }
        public ActionResult MyCustomCardInstructions(string id, string userId)
        {
            //var info = _AppContext.CustomCardInfoApp.GetSingleUserCustomCardInfoByIdAndUserId(id, userId);
            ViewData["curUserEntity"] = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
            return View();
        }
    }
}