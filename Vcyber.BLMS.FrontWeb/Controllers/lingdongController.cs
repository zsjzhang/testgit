using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Repository;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class lingdongController : Controller
    {
        const string ACTIVITY_TYPE = "LingDong_wap";

        const LotteryDrawPoolType lotteryType_testdrive = LotteryDrawPoolType.LingDong_TestDeive_Wap;
        const int testdrive_probability = 15;

        const LotteryDrawPoolType lotteryType_recommend = LotteryDrawPoolType.LingDong_Recommend_Wap;
        const int recommend_probability = 15;
        //
        // GET: /Elantra/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult pc()
        {
            try
            {
                //经销商选择列表
                string[] provinces = _AppContext.DealerApp.GetProvinceList().ToArray();
                provinces = provinces ?? new string[] { };
                ViewBag.ListProvince = provinces;

                List<LotteryModel> drvAwardList = new List<LotteryModel>();
                List<LotteryModel> rmdAwardList = new List<LotteryModel>();

                GetAwardList(string.Empty, out drvAwardList, out rmdAwardList);

                ViewBag.drvAwardList = drvAwardList;
                ViewBag.rmdAwardList = rmdAwardList;

            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetCityListByProvince(string province)
        {
            var result = _AppContext.DealerApp.GetCityListByProvince(province).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetDealerList(string province, string city)
        {
            var list = _AppContext.DealerApp.GetDealerList(province, city).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult wap(string source)
        {
            return View();
        }

        //public ActionResult Share(string p)
        //{
        //    //需要根据流程判断
        //    //ViewBag.sharePageContent = 1;//试驾分享
        //    //ViewBag.sharePageContent = 2;//推荐分享
        //    ViewBag.sharePageContent = p;
        //    return View();
        //}

        //public ActionResult ShareGd(string p)
        //{
        //    ViewBag.sharePageContent = p;
        //    return View();
        //}

        public ActionResult Show(string openid)
        {
            ViewBag.oid = openid;
            return View();
        }

        public ActionResult Readme()
        {
            try
            {
                ViewBag.isover = 0;
                ViewBag.isBind = 0;

                ViewBag.isSubDrive = 0;
                ViewBag.isSubDriveAward = 0;
                ViewBag.awardTypeOfSubDriv = 0;
                ViewBag.awardNameOfSubDriv = "";

                ViewBag.isRecommand = 0;
                ViewBag.isRecommandAward = 0;
                ViewBag.awardTypeOfRecommand = 0;
                ViewBag.awardNameOfRecommand = "";

                //经销商选择列表
                string[] provinces = _AppContext.DealerApp.GetProvinceList().ToArray();
                provinces = provinces ?? new string[] { };
                ViewBag.ListProvince = provinces;

                List<LotteryModel> drvAwardList = new List<LotteryModel>();
                List<LotteryModel> rmdAwardList = new List<LotteryModel>();

                GetAwardList(string.Empty, out drvAwardList, out rmdAwardList);

                ViewBag.drvAwardList = drvAwardList;
                ViewBag.rmdAwardList = rmdAwardList;

                return View();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Bind(string openId, string phone, string validateCode,
               string idcard)
        {


            var isOk = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, validateCode).IsSuccess;

            if (!isOk)
                return Json(new { ret = -1, msg = "请输入正确的验证码" });
            //用身份证去核对
            var cust = _AppContext.UserInfoApp.GetInfoWhenBuyCar(idcard);
            if (cust != null && !string.IsNullOrEmpty(cust.CustId))
            {
                return Json(new { ret = 1, msg = "认证成功" });
            }
            return Json(new { ret = 0, msg = "验证失败" });
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="num">位数</param>
        /// <returns>1：成功，0：失败</returns>
        [HttpPost]
        public ActionResult SendValidateCode(string tel, int num = 6)
        {
            if (String.IsNullOrEmpty(tel))
            {
                return Json(new { msg = "手机号不能为空" });
            }

            var result = _AppContext.UserSecurityApp.SendMobileVerifyCode(tel, num, "");
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, msg = "发送失败" });
        }


        /// <summary>
        /// 填写邮寄地址
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="openId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostAddress(string name, string phoneNumber, string openId, string address)
        {
            var postAd = new PostAddress()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                OpenId = openId,
                Address = address
            };

            PostAddressRepository repository = new PostAddressRepository();
            if (repository.Add(postAd) > 0)
            {
                return Json(new { code = 1 });
            }
            else
            {
                return Json(new { code = 0, msg = "发送失败" });
            }
        }

        /// <summary>
        /// 推荐新车主
        /// </summary>
        /// <param name="listRcmd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Recommend(RecommendString temp)
        {
            try
            {
                List<RecommendViewModel> model = new List<RecommendViewModel>();
                model.Add(new RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name1, PhoneNumber = temp.PhoneNumber1 });
                model.Add(new RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name2, PhoneNumber = temp.PhoneNumber2 });
                model.Add(new RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name3, PhoneNumber = temp.PhoneNumber3 });

                RecommendRepository repository = new RecommendRepository();
                Recommend entity;

                //using (TransactionScope scope = new TransactionScope())
                //{
                var recommd = repository.GetByPhone4Wap(ACTIVITY_TYPE, model.FirstOrDefault().OpenId);
                if (recommd != null && recommd.Id > 0)
                    return Json(new { Code = "0", Message = "您已参加过推荐活动，不能重复参加。" });

                foreach (RecommendViewModel vm in model)
                {
                    if (!string.IsNullOrEmpty(vm.Name) && !string.IsNullOrEmpty(vm.PhoneNumber))
                    {
                        entity = new Entity.Recommend
                        {
                            OpenId = vm.OpenId,
                            ActivityType = ACTIVITY_TYPE,
                            Name = vm.Name,
                            PhoneNumber = vm.PhoneNumber,
                            DataSource = "lingdong_pc"
                        };
                        repository.Add(entity);
                    }
                }

                //抽奖
                LotteryDrawBL BL = new LotteryDrawBL();
                LotteryDrawPool LDP = null;//BL.Execute(lotteryType_recommend, recommend_probability);
                if (LDP != null && LDP.Award != null)
                {
                    //更新奖品池数量
                    int result = BL.UpdateLotteryDrawPool(LDP.Id, BL.GetVersionNumber(LDP.Id).VersionNumber);
                    if (result <= 0)
                    {
                        return Json(new Award());
                    }
                    //中奖信息写入数据库
                    bool visualSendState = false;
                    if (LDP.Award.AwardType == AwardType.visual)
                    {
                        //虚拟奖品需要立即发放
                        visualSendState = BL.SendVisualAward(LDP, model.FirstOrDefault().OpenId);
                    }
                    //记录奖品发放记录
                    BL.AwardSendRecord(LDP, model.FirstOrDefault().OpenId, visualSendState);
                }
                if (LDP != null && LDP.Award != null)
                {
                    return Json(new { Code = "1", Message = "中奖", AwardObj = LDP.Award });
                }
                else
                {
                    return Json(new { Code = "1", Message = "没有中奖" });
                }

                //}
            }
            catch (Exception ex)
            {
                return Json(new { Code = "0", Message = "提交失败" });
            }
        }

        /// <summary>
        /// 预约试驾
        /// </summary>
        /// <param name="carSeries"></param>
        /// <param name="openId"></param>
        /// <param name="dealerId"></param>
        /// <param name="scheduleDate"></param>
        /// <param name="userName"></param>
        /// <param name="phone"></param>
        /// <param name="dealerCity"></param>
        /// <param name="dealerProvince"></param>
        /// <param name="dealerName"></param>
        /// <param name="userSex"></param>
        /// <param name="valiateCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TestDrive4Wx(string carSeries, string openId, string dealerId,
               DateTime scheduleDate,
               string userName, string phone,
               string dealerCity,
               string dealerProvince,
               string dealerName,
               string userSex,
               string valiateCode)
        {
            var model = new TestDriveEntity
            {
                CarSeries = carSeries,
                OpenId = phone,
                DealerId = dealerId,
                ScheduleDate = scheduleDate,
                UserName = userName,
                Phone = phone,
                DealerCity = dealerCity,
                DealerProvince = dealerProvince,
                DealerName = dealerName,
                UserSex = string.IsNullOrEmpty(userSex) ? 1 : Convert.ToInt32(userSex),
                UserId = "0",
                DataSource = "wap",
                ActivityName = ACTIVITY_TYPE
            };

            var isOk = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, valiateCode).IsSuccess;

            if (!isOk)
                return Json(new { ret = 0, msg = "请输入正确的验证码" });

            TestDriveRepository tRepository = new TestDriveRepository();
            var tData = tRepository.GetOne4Wap(ACTIVITY_TYPE, model.Phone, model.DataSource);
            if (tData != null && tData.Id > 0)
                return Json(new { ret = 1, msg = "您已参加此活动，不能重复参加。" });

            int retval = _AppContext.TestDriveApp.Add(model);

            Award award = null;
            if (retval > 0)
            {
                //抽奖
                LotteryDrawBL BL = new LotteryDrawBL();
                LotteryDrawPool LDP = BL.Execute(lotteryType_testdrive, testdrive_probability);
                if (LDP != null && LDP.Award != null)
                {
                    //更新奖品池数量
                    int result = BL.UpdateLotteryDrawPool(LDP.Id, BL.GetVersionNumber(LDP.Id).VersionNumber);
                    if (result <= 0)
                    {
                        return null;
                    }
                    //记录奖品发放记录
                    BL.AwardSendRecord(LDP, phone, null);
                    award = LDP.Award;
                }
            }

            return Json(new { retcode = 1, msg = "成功", AwardObj = award });
        }


        [HttpPost]
        public ActionResult TestDrive4PC(string carSeries, string openId, string dealerId,
               DateTime scheduleDate,
               string userName, string phone,
               string dealerCity,
               string dealerProvince,
               string dealerName,
               string userSex,
               string valiateCode)
        {
            var model = new TestDriveEntity
            {
                CarSeries = carSeries,
                OpenId = phone,
                DealerId = dealerId,
                ScheduleDate = scheduleDate,
                UserName = userName,
                Phone = phone,
                DealerCity = dealerCity,
                DealerProvince = dealerProvince,
                DealerName = dealerName,
                UserSex = string.IsNullOrEmpty(userSex) ? 1 : Convert.ToInt32(userSex),
                UserId = "0",
                DataSource = "lingdong_pc",
                ActivityName = ACTIVITY_TYPE
            };

            var isOk = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, valiateCode).IsSuccess;

            if (!isOk)
                return Json(new { ret = 0, msg = "请输入正确的验证码" });

            TestDriveRepository tRepository = new TestDriveRepository();
            var tData = tRepository.GetOne4Wap(ACTIVITY_TYPE, model.Phone, model.DataSource);
            if (tData != null && tData.Id > 0)
                return Json(new { ret = 1, msg = "您已参加此活动，不能重复参加。" });

            int retval = _AppContext.TestDriveApp.Add(model);

            Award award = null;
            if (retval > 0)
            {
                //抽奖
                LotteryDrawBL BL = new LotteryDrawBL();
                LotteryDrawPool LDP = BL.Execute(lotteryType_testdrive, testdrive_probability);
                if (LDP != null && LDP.Award != null)
                {
                    //更新奖品池数量
                    int result = BL.UpdateLotteryDrawPool(LDP.Id, BL.GetVersionNumber(LDP.Id).VersionNumber);
                    if (result <= 0)
                    {
                        return null;
                    }
                    //记录奖品发放记录
                    BL.AwardSendRecord(LDP, phone, null);
                    award = LDP.Award;
                }
            }

            return Json(new { retcode = 1, msg = "成功", AwardObj = award });
        }

        [HttpGet]
        public ActionResult GetAward(string phone)
        {
            List<LotteryModel> drvAwardList;
            List<LotteryModel> rmdAwardList;
            GetAwardList(phone, out drvAwardList, out rmdAwardList);

            //var drvAwardName = drvAwardList.Count > 0 ? drvAwardList[0].AwardName : string.Empty;
            //var rcmdAwardName = rmdAwardList.Count > 0 ? rmdAwardList[0].AwardName : string.Empty;

            return Json(new { Ret = 1, drvAwardList = drvAwardList, rmdAwardList = rmdAwardList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据手机号获取奖品列表
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="drvAwardList"></param>
        /// <param name="rmdAwardList"></param>
        private void GetAwardList(string phone, out List<LotteryModel> drvAwardList, out List<LotteryModel> rmdAwardList)
        {
            drvAwardList = new List<LotteryModel>();
            rmdAwardList = new List<LotteryModel>();

            AwardSendRecordRepository repository = new AwardSendRecordRepository();
            var result = repository.GetAllLottery(phone);
            if (result != null)
            {
                drvAwardList = result["LingDong_TestDeive"].Take(10).ToList();
                rmdAwardList = result["LingDong_Recommend"].Take(10).ToList();
            }

        }

        public ActionResult Introduction()
        {
            return View();
        }
    }
}