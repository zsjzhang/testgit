using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Generated;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using Vcyber.BLMS.ManageWeb.Models;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using Vcyber.BLMS.Repository;
using Vcyber.BLMS.Common;
using System.Configuration;
using Vcyber.BLMS.IRepository;
using System.Data;
//using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.OleDb;
using System.Text;
using System.Reflection;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class AfterSaleServiceController : Controller
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

        //
        // GET: /AfterSaleService/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Repair()
        {
            return View();
        }

        public JsonResult SelectRepairList(SC_ServiceCardUsedRecordSearchParam param, int pageindex, int pagesize)
        {
            int total = 0;

            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(dealerId))
            {
                param.DealerId = dealerId;
            }

            var cardList = _AppContext.ServiceCardUsedRecordApp.SelectRepairList(param).ToList<Remeal>();

            total = cardList.Count();

            var list = cardList.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = list, pos = pageindex, total_count = total };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsMember(string PhoneNumber)
        {
            var member = _DbSession.PrizesInfoStorager.GetMembershipMode(PhoneNumber);
            if (member != null)
            {
                return Content("{\"date\":\"Y\",\"leve\":" + member.MLevel + "}");
            }
            return Content("{\"date\":\"N\",\"leve\":\"-1\"}");
        }

        [HttpPost]
        public JsonResult GetCardInfo(string cardType, string cardNo, string activity)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            result = _AppContext.ServiceCardUsedRecordApp.GetServiceCardInfo(cardType, cardNo, activity);


            RecommendCustomer RecommendInfo = null;


            //结果正常，有卡劵返回信息
            if (result.IsSuccess && result.Data != null)
            {
                AfterSaleServiceWXModel model = (AfterSaleServiceWXModel)result.Data;


                if (model != null)
                {
                    //todo: 上线修改
                    if (activity == "悦己纳新-售后服务活动")
                    {
                        //2瓶玻璃水兑换券,50元保养券,机油滤芯兑换券
                        var cardTypesNoDisplay = ConfigurationManager.AppSettings["cardTypesNoDisplay"].Split(',');
                        if (!cardTypesNoDisplay.Contains(cardType))
                        {
                            //通过手机号获取YC活动信息
                            RecommendInfo = _AppContext.ServiceCardUsedRecordApp.GetRecommendNameByPhone(model.data.tel);
                        }
                    }

                    var card = new
                    {
                        CardNo = model.data.code,
                        id = model.data.id,
                        PhoneNumber = model.data.tel,
                        OpenId = model.data.openId,
                        CardInfo = model.data.remark,
                        RecommendInfo = RecommendInfo
                    };

                    result.Data = card;
                }
            }
            else if (activity == "lingdong")//新的实现，不从微信前台验证，放到后台业务中
            {
                string openid_new = string.Empty;
                string error = string.Empty;
                if (CheckWxCard_New(cardType, cardNo, out openid_new, out error))
                {
                    var card = new
                    {
                        CardNo = cardNo,
                        PhoneNumber = GetPhoneNumberByOpenId(openid_new),
                        OpenId = openid_new,
                        RecommendInfo = RecommendInfo
                    };
                    result.IsSuccess = true;
                    result.Message = "";
                    result.Data = card;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetPhoneNumberByOpenId(string openid_new)
        {
            WXCardRepository repository = new WXCardRepository();
            return repository.GetPhoneNumber(openid_new);
        }

        private bool CheckWxCard_New(string cardid, string code, out string openid, out string error)
        {
            WXCardRepository repository = new WXCardRepository();
            return repository.ValideWxCardCode(cardid, code, out openid, out error);
        }

        /// <summary>
        /// 提交核销请求到微信
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool WxCardHXSubmit(string code, out string openid, out string cardId, out string error)
        {
            WXCardRepository repository = new WXCardRepository();
            return repository.WxCardHX(code, out openid, out cardId, out error);
        }

        private Award GetCardFromDb(string cardid)
        {
            LotteryDrawPoolRepository repository = new LotteryDrawPoolRepository();
            return repository.GetWxCardByCardId(cardid);
        }


        [HttpPost]
        public JsonResult GetCustAndCarInfoBuy(string vin, string activity)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //验证此次活动是否已经核销过
            //if (ConfigurationManager.AppSettings["CardActivityTag"] == null)
            //{
            //    result.IsSuccess = false;
            //    result.Message = "活动标记配置信息未设置，请确认配置文件";
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}

            string activityTag = activity;//ConfigurationManager.AppSettings["CardActivityTag"];
            bool isExist = _AppContext.ServiceCardUsedRecordApp.SelectRemealByVin(vin, activityTag).Count() > 0;//todo:车架号验证

            if (isExist)
            {
                result.IsSuccess = false;
                result.Message = "该VIN码在此次活动中已经购买过套餐";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //客户信息
            IFCustomer cust = null;

            //车辆信息
            Car car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out cust);

           

            //VIN验证失败
            if (car == null)
            {
                result.IsSuccess = false;
                result.Message = "你输入的VIN码验证失败，请确认是否输入正确";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //List<string> carList = new List<string> { "悦纳", "悦动" };
            string[] carList = ConfigurationManager.AppSettings["FourCarCategory"].Split(',');
            if (!carList.Contains(car.CarCategory))
            {
                result.IsSuccess = false;
                result.Message = "此套餐暂时不支持该款车型";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if ((car .CarCategory== "悦纳" && (car.BuyTime < Convert.ToDateTime("2017-3-1") || car.BuyTime > Convert.ToDateTime("2017-4-30")))
                ||
                (car.CarCategory == "全新悦动" && (car.BuyTime < Convert.ToDateTime("2017-4-1") || car.BuyTime > Convert.ToDateTime("2017-4-30")))
                )
            {
                result.IsSuccess = false;
                result.Message = "购车时间不符合,无法购买该套餐";
                return Json(result);
            }

            //客户姓名
            string custName = string.Empty;
            string userId = string.Empty;

            if (cust != null)
            {
                custName = cust.CustName;

                //用户信息
                var frontUserStore = new FrontUserStore<FrontIdentityUser>();
                var membership = frontUserStore.FindByIdentityNumber(cust.IdentityNumber);

                if (membership.Result != null)
                {
                    userId = membership.Result.Id;
                }
            }

            //返回数据
            var returnData = new
            {
                CustName = custName,
                BuyTime = car.BuyTime.Value.ToString("yyyy-MM-dd"),
                CarCategory = car.CarCategory,
                UserId = userId
            };

            result.Data = returnData;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RepairSelect(Remeal model)
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = false;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = true;
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetCustAndCarInfo(string vin, string activity, string cardNo)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //验证此次活动是否已经核销过
            //if (ConfigurationManager.AppSettings["CardActivityTag"] == null)
            //{
            //    result.IsSuccess = false;
            //    result.Message = "活动标记配置信息未设置，请确认配置文件";
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}

            string activityTag = activity;//ConfigurationManager.AppSettings["CardActivityTag"];


            IEnumerable<SC_ServiceCardUsedRecord> UsedRecord =
                _AppContext.ServiceCardUsedRecordApp.SelectRecordByVinAndCardType(vin, activityTag);
            bool isExist = false;//todo:核销


            var arrays = ConfigurationManager.AppSettings["reparicust"].Split(',');
            if (arrays.Contains(activityTag)) //如果是这9个活动
            {
                var scr = _DbSession.SC_ServiceCardUsedRecordStorager.GetSCServiceCardUsedRecord(activity,
                    cardNo);

                if (scr != null)
                {
                    result.IsSuccess = false;
                    result.Message = "该VIN码在此次活动中已经核销过，不能重复使用";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (UsedRecord.Count() > 0 && UsedRecord.Count(i => i.CreateTime.ToShortDateString() == DateTime.Now.ToShortDateString()) == 1)
                {
                    result.Message = "今天您已经使用过";
                    result.IsSuccess = false;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                isExist = UsedRecord != null && UsedRecord.Count() > 0;
            }


            if (isExist)
            {
                result.IsSuccess = false;
                result.Message = "该VIN码在此次活动中已经核销过，不能重复使用";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //客户信息
            IFCustomer cust = null;

            //车辆信息
            Car car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out cust);

            //VIN验证失败
            if (car == null)
            {
                result.IsSuccess = false;
                result.Message = "你输入的VIN码验证失败，请确认是否输入正确";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //客户姓名
            string custName = string.Empty;
            string userId = string.Empty;

            if (cust != null)
            {
                custName = cust.CustName;

                //用户信息
                var frontUserStore = new FrontUserStore<FrontIdentityUser>();
                var membership = frontUserStore.FindByIdentityNumber(cust.IdentityNumber);

                if (membership.Result != null)
                {
                    userId = membership.Result.Id;
                }
            }

            //返回数据
            var returnData = new
            {
                CustName = custName,
                CarCategory = car.CarCategory,
                UserId = userId
            };

            result.Data = returnData;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConfirmUseCard(SC_ServiceCardUsedRecord record)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                //if (user.DealerId == null || user.DealerId == "-1")
                //{
                //    result.IsSuccess = false;
                //    result.Message = "非经销商用户不能操作";
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                bool isExist = false;//todo:核销
                IEnumerable<SC_ServiceCardUsedRecord> UsedRecord = _AppContext.ServiceCardUsedRecordApp.SelectRecordByVinAndCardType(record.VIN, record.CardType);
                //"86683d9c-0314-4a8f-bf74-246c73460c7e
                var arrays = ConfigurationManager.AppSettings["reparicust"].Split(',');
                if (arrays.Contains(record.CardType)) //如果是这9个活动
                {
                    var scr = _DbSession.SC_ServiceCardUsedRecordStorager.BuySCServiceCardUsedRecord(ConfigurationManager.AppSettings["reparicust"]
                        );

                    if (scr != null && scr.Count() > 0 && scr.Count(i => i.CardNo == record.CardNo) > 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "该卡券在此次活动中已经核销过，不能重复使用";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (scr.Count(i => i.CreateTime.ToShortDateString() == DateTime.Now.ToShortDateString() && i.VIN.ToLower() == record.VIN.ToLower()) >= 1)
                    {
                        result.Message = "今天您已经使用过";
                        result.IsSuccess = false;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    isExist = UsedRecord != null && UsedRecord.Count() > 0;
                }


                if (isExist)
                {
                    result.IsSuccess = false;
                    result.Message = "该VIN码在此次活动中已经核销过，不能重复使用";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //当前活动标识
                string activityTag = record.ActivityTag;//ConfigurationManager.AppSettings["CardActivityTag"];

                if (activityTag == "Y" || activityTag == "spring" || activityTag == "lingdong")
                {
                    result = _AppContext.ServiceCardUsedRecordApp.ServiceCardConsum(record.CardType, record.CardNo,
        record.ActivityTag);

                }
                else
                {
                    try
                    {
                        if (arrays.Contains(record.CardType))
                        {
                            // IEnumerable<CustomCardInfo> cards=_DbSession.SC_ServiceCardUsedRecordStorager.BuyCustCard(record.CardType, record.VIN);
                            Remeal re = _AppContext.CustomCardApp.GetRemeal(record.VIN, ConfigurationManager.AppSettings["reparicust"], record.CardNo);
                            if (re == null)
                            {
                                result.IsSuccess = false;
                                result.Message = "只有购买人才能使用，请您再次核对信息，如有疑问，请联系微信在线客服";
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            if (re.DearID != user.DealerId)
                            {
                                result.IsSuccess = false;
                                result.Message = "只有在您购买服务的店内使用，请您再次核对信息，如有疑问，请联系微信在线客服";
                            }
                            if (re.PhoneNumber != record.PhoneNumber || re.Vin != record.VIN 
                                || re.CardType1 != record.CardType)
                            {
                                result.IsSuccess = false;
                                result.Message = "只有购买人才能使用，请您再次核对信息，如有疑问，请联系微信在线客服";
                            }

                            if (!result.IsSuccess)
                            {
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }

                        result = _AppContext.ActivitiesApp.ServiceCardConsum(record.Id);//卡券号码改为已核销CustomCard
                    }
                    catch (Exception ex)
                    {

                        result.IsSuccess = false;
                        result.Message = "核销发生异常";
                        Common.LogService.Instance.Error("核销成功,发生异常,异常信息：" + ex.StackTrace + ",失败数据如下：" + JsonConvert.SerializeObject(record));
                    }
                }


                record.DealerId = user.DealerId;
                record.ActivityTag = activityTag;
                record.VIN = record.VIN.ToUpper();

                //微信核销成功
                if (result.IsSuccess)
                {
                    try
                    {
                     
                        result = _AppContext.ServiceCardUsedRecordApp.ConfirmUseCard(record);//todo:核销
                       
                        result.Data = record;

                        if (!result.IsSuccess)
                        {
                            Common.LogService.Instance.Error("核销成功,数据保存失败,失败数据如下：" + JsonConvert.SerializeObject(record));
                        }
                    }
                    catch (Exception ex)
                    {
                        result.IsSuccess = false;
                        result.Message = "核销发生异常";
                        Common.LogService.Instance.Error("核销成功,发生异常,异常信息：" + ex.StackTrace + ",失败数据如下：" + JsonConvert.SerializeObject(record));
                    }
                }
                else//LingDong上市活动 后台核销 
                {
                    string openid; string cardId; string error;
                    var cardAward = GetCardFromDb(record.CardType);
                    if (cardAward != null && cardAward.Id > 0)
                    {
                        var hxresult = WxCardHXSubmit(record.CardNo, out openid, out cardId, out error);
                        if (hxresult)
                        {
                            try
                            {
                                record.CardInfo = GetWxCardName(cardId);
                                result = _AppContext.ServiceCardUsedRecordApp.ConfirmUseCard(record);
                                result.Data = record;
                            }
                            catch (Exception ex)
                            {
                                LogService.Instance.Error("Weixin 核销成功，后台保存记录失败。卡券数据：" + JsonConvert.SerializeObject(record), ex);
                            }
                        }
                    }
                }
                //(2瓶玻璃水兑换券,50元保养券,机油滤芯兑换券) 
                var cardTypesNoDisplay = ConfigurationManager.AppSettings["cardTypesNoDisplay"].Split(',');
                string cardCode = "";
                //核销成功以后如果是YC活动并且试乘试驾礼品券已经核销 再发送其他试乘试驾券
                if (record.ActivityTag == "悦己纳新-售后服务活动" && !cardTypesNoDisplay.Contains(record.CardType) && result.IsSuccess)
                {
                    cardCode = RandomNumberHelper.GetUserCustomCardCode();
                    CustomCardInfo cust = _AppContext.ServiceCardUsedRecordApp.GetCustomTypeByVin(record.VIN, record.ActivityTag);
                    if (cust != null)
                    {
                        ReturnResult re = ReissueCard(cust.CardType);
                        if (re.IsSuccess)
                        {
                            string userId = "";
                            var member = _AppContext.DealerMembershipApp.SelectMemberListByphoneNumber(record.PhoneNumber).ToList();
                            if (member != null && member.Count > 0)
                            {
                                userId = member.FirstOrDefault().Id;
                            }
                            SendCustom(cust, cardCode, userId, record.PhoneNumber);
                        }
                        else
                        {
                            LogService.Instance.Error("YC活动：" + record.CardType + re.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogService.Instance.Info("卡券核销日志异常：" + ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //发送卡券
        public void SendCustom(CustomCardInfo customCardInfo, string cardCode, string userId, string phone)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };

            var customcard = new CustomCard()
            {
                CardType = customCardInfo.CardType,
                CardCode = cardCode,
                CardId = customCardInfo.Id,
                CreateTime = DateTime.Now,
                IsSave = true,
                IsCancel = false,
                UserId = userId,
                IsReissue = false,
                Tel = phone,
                IsSend = true,
                OpenId = "",
                Source = "blms"
            };

            // 用户卡券信息入库
            res = _AppContext.CustomCardApp.AddCustomCard(customcard);
            if (res.IsSuccess)
            {
                //发送卡券短信信息；
                _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = cardCode }, phone);

                //更新卡券信息库存；
                _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
            }
        }

        public ReturnResult ReissueCard(string cardName)
        {
            ReturnResult res = new ReturnResult { IsSuccess = true };
            var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardName);
            if (customCardInfo == null)
            {
                res.IsSuccess = false;
                res.Message = "卡券信息不正确";
                return res;
            }
            if (customCardInfo != null)
            {
                var today = DateTime.Now;
                if (today < customCardInfo.CardBeginDate || today > customCardInfo.CardEndDate)
                {
                    res.IsSuccess = false;
                    res.Message = "卡券不在有效期内，不能补发请联系管理员。";
                    return res;
                }
            }
            //领取兑奖码
            var cardCode = "";
            var resCardCode = "";
            if (customCardInfo.CardSource == 1)
            {
                var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                if (customCardInfo.Quantity - usedCount < 0)
                {
                    res.IsSuccess = false;
                    res.Message = "券码库存不足了";
                    return res;
                }
            }
            return res;
        }

        private string GetWxCardName(string cardId)
        {
            LotteryDrawPoolRepository repository = new LotteryDrawPoolRepository();
            var ldp = repository.GetWxCardByCardId(cardId);
            if (ldp != null)
                return ldp.Name;
            return string.Empty;
        }

        public ViewResult ReportViewer(ReportCard record)
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                 Response.Redirect("/Content/error.htm");
            }
            if (!string.IsNullOrEmpty(record.Type))
            {
                var tempCard = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfo(record.Type);
                record.DealerId = (record.DealerId == "null" ? "" : record.DealerId);
                if (tempCard != null)
                {
                    record.ActName = tempCard.ActivityName;
                }
            }
            ViewBag.Data = record;
            return View();
        }

        public FileContentResult GenerateAndDisplayReport(ReportCard record)
        {
            LocalReport localReport = new LocalReport();
            var cardTypesNoDisplay = ConfigurationManager.AppSettings["cardTypesNoDisplay"].Split(',');
            if (record != null && record.ActName == "悦己纳新-售后服务活动" && !cardTypesNoDisplay.Contains(record.Type))
            {
                localReport.ReportPath = Server.MapPath("~/Content/Report4.rdlc");
            }
            else
            {
                localReport.ReportPath = Server.MapPath("~/Content/Report1.rdlc");
            }


            IList<ReportCard> cardList = new List<ReportCard>();

            cardList.Add(record);

            ReportDataSource reportDataSource = new ReportDataSource();
            if (record != null && record.ActName == "悦己纳新-售后服务活动" && !cardTypesNoDisplay.Contains(record.Type))
            {
                reportDataSource.Name = "DataSet4";
            }
            else
            {
                reportDataSource.Name = "DataSet1";
            }
            reportDataSource.Value = cardList;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>jpeg</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 

            return File(renderedBytes, "image/jpeg");
        }

        public ActionResult RecordList()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = false;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateRecord(int id, string custName, int Mileage)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            result = _AppContext.ServiceCardUsedRecordApp.UpdateRecord(id, custName, Mileage);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectRecordList(SC_ServiceCardUsedRecordSearchParam param, int pageindex, int pagesize)
        {
            int total = 0;

            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(dealerId))
            {
                param.DealerId = dealerId;
            }

            var cardList = _AppContext.ServiceCardUsedRecordApp.SelectRecordList(param).ToList<SC_ServiceCardUsedRecord>();

            total = cardList.Count();

            var list = cardList.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = list, pos = pageindex, total_count = total };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ExportAfterSaleService(SC_ServiceCardUsedRecordSearchParam param)
        {
            //根据查询参数获取数据
            var cardList = _AppContext.ServiceCardUsedRecordApp.SelectRepairList(param).ToList<Remeal>();
            List<IList<Remeal>> list = new List<IList<Remeal>>();
            list.Add(cardList);
            List<List<string>> propertyList = new List<List<string>>() { new List<string>() { "DearID", "PhoneNumber", "CardType", "CarCategory", "Vin", "CustName", "CreateTime1", "DearName", "BuyTime", "Mlevel" } };
            List<List<string>> columnList = new List<List<string>>() { new List<string>() { "店代码", "手机号", "套餐类型", "车型", "车架号", "姓名", "购买时间", "经销商名称", "购车时间", "会员等级" } };
            NPOIHelper<Remeal>.ListToExcelEX(list, "afterSaleService.xls", propertyList, columnList, new List<string>() { "afterSale" });
        }
        public FileResult Export(SC_ServiceCardUsedRecordSearchParam param)
        {

            //处理微信卡券， 活动名称对应的活动类型；
            //if (param.isactivity == "三八活动")
            //{
            //    param.isactivity = "Y";
            //}
            //else if (param.isactivity == "春季免检活动")
            //{
            //    param.isactivity = "spring";
            //}
            //else if (param.isactivity == "1瓶玻璃水兑换券")
            //{
            //    param.isactivity = "lingdong";
            //}
            //else if (param.isactivity == "推荐有理")
            //{
            //    param.isactivity = "TuiJian";
            //}
            //else if (param.isactivity == "北京车展")
            //{
            //    param.isactivity = "carshow";
            //}
            //else
            //{
            //    param.isactivity = "";
            //}
           
            int cou = 0;

            //创建Excel文件的对象
            //NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            NPOI.XSSF.UserModel.XSSFWorkbook book = new NPOI.XSSF.UserModel.XSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            List<SC_ServiceCardUsedRecord> cards = new List<SC_ServiceCardUsedRecord>();

            //获取list数据
            cards = _AppContext.ServiceCardUsedRecordApp.SelectRecordList(param).ToList<SC_ServiceCardUsedRecord>();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("卡券号码");
            row1.CreateCell(2).SetCellValue("手机号");
            row1.CreateCell(3).SetCellValue("姓名");
            row1.CreateCell(4).SetCellValue("车架号");
            row1.CreateCell(5).SetCellValue("经销商");
            row1.CreateCell(6).SetCellValue("行驶里程");
            row1.CreateCell(7).SetCellValue("创建时间");
            row1.CreateCell(8).SetCellValue("卡劵类型");
            row1.CreateCell(9).SetCellValue("车型");
            row1.CreateCell(10).SetCellValue("来源");
            row1.CreateCell(11).SetCellValue("经销商名称");
            row1.CreateCell(12).SetCellValue("发卡时间");
            row1.CreateCell(13).SetCellValue("购车时间");
            row1.CreateCell(14).SetCellValue("会员等级");
            row1.CreateCell(15).SetCellValue("区域");
            row1.CreateCell(16).SetCellValue("办事处");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < cards.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                
                rowtemp.CreateCell(0).SetCellValue(cards[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(cards[i].CardNo.ToString());
                rowtemp.CreateCell(2).SetCellValue(cards[i].PhoneNumber.ToString());
                rowtemp.CreateCell(3).SetCellValue(cards[i].CustName == null ? "" : cards[i].CustName.ToString());
                rowtemp.CreateCell(4).SetCellValue(cards[i].VIN == null ? "" : cards[i].VIN.ToString());
                rowtemp.CreateCell(5).SetCellValue(cards[i].DealerId == null ? "" : cards[i].DealerId.ToString());
                rowtemp.CreateCell(6).SetCellValue(cards[i].Mileage == null ? "" : cards[i].Mileage.ToString());
                rowtemp.CreateCell(7).SetCellValue(cards[i].CreateTime.ToLongDateString());
                rowtemp.CreateCell(8).SetCellValue(cards[i].CardTypeName == null ? "" : cards[i].CardTypeName.ToString());
                rowtemp.CreateCell(9).SetCellValue(cards[i].CarCategory == null ? "" : cards[i].CarCategory.ToString());
                rowtemp.CreateCell(10).SetCellValue(cards[i].ActivityTag == null ? "" : cards[i].ActivityTag.ToString());               
                rowtemp.CreateCell(11).SetCellValue(cards[i].DealerName == null ? "" : cards[i].DealerName.ToString());
                rowtemp.CreateCell(12).SetCellValue(cards[i].kqCreateTime == null ? "" : cards[i].kqCreateTime.ToString());
                rowtemp.CreateCell(13).SetCellValue(cards[i].BuyTime == null ? "" : cards[i].BuyTime.ToString());
                rowtemp.CreateCell(14).SetCellValue(cards[i].MLevel == null ? "" : cards[i].MLevel.ToString());
                  
                rowtemp.CreateCell(15).SetCellValue(cards[i].Area == null ? "" : cards[i].Area.ToString());
                rowtemp.CreateCell(16).SetCellValue(cards[i].Region == null ? "" : cards[i].Region.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();          
            book.Write(ms);
            var buf = ms.ToArray();
            //ms.Seek(0, SeekOrigin.Begin);
            
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            //return File(ms, "application/vnd.ms-excel", "Card.xlsx");
            return File(buf, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Card.xlsx");
        }

         
       

       
    }
}