using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Webdiyer.WebControls.Mvc;
using Vcyber.BLMS.Common;
using System.IO;
using System.Text;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class CustomCardController : Controller
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

        [HttpGet]
        public ActionResult Index(string merchant, string actType, string cardName, string reduceCost, int pageIndex = 1, int source = 0, int status = 0)
        {
             var  cookieValue=CookieHelper.GetCookieValue("CustomCookie");
             if (!string.IsNullOrWhiteSpace(cookieValue) &&  cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }

            int pageSize = 10;
            int totalCount;
            var list = _AppContext.CustomCardInfoApp.GetCustomCardInfoList(source, merchant, actType, cardName, status, reduceCost, pageIndex, pageSize, out totalCount);
            PagedList<CustomCardInfo> CustomCardList = new PagedList<CustomCardInfo>(list, pageIndex, Convert.ToInt32(pageSize), totalCount);
            return View(CustomCardList);
        }

        public ActionResult PartialCustomCard(string merchant, string actType, string cardName, string reduceCost, int pageIndex = 1, int source = 0, int status = 0)
        {
            int pageSize = 10;
            int total = 0;
            PageData page = new PageData() { Index = pageIndex, Size = pageSize };
            var list = _AppContext.CustomCardInfoApp.GetCustomCardInfoList(source, merchant, actType, cardName, status, reduceCost, pageIndex, pageSize, out total);
            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < count ? (pageIndex + 1) : count;
            ViewBag.EndPage = count;
            ViewBag.status = status;
            return PartialView(list);
        }

        public JsonResult ExportCustomCard(string merchant, string actType, string cardName, string reduceCost, int pageIndex = 1, int source = 0, int status = 0)
        {
            int pageSize = 10;
            int total = 0;
            PageData page = new PageData() { Index = pageIndex, Size = pageSize };
            var list = _AppContext.CustomCardInfoApp.GetCustomCardInfoList(source, merchant, actType, cardName, status,
                reduceCost, pageIndex, pageSize, out total);

            return Json("", JsonRequestBehavior.AllowGet);

        }

        public ActionResult Add()
        {
            Questionnaire aa = new Questionnaire();
            return View(aa);
        }

        public ActionResult Edit(string cardType)
        {
            var info = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardType);
            return View(info);
        }
        /// <summary>
        /// 修改卡券信息
        /// </summary>
        /// <param name="modle">卡券信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveEditCustomCard(CustomCardInfo modle)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            var info = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(modle.CardType);
            if (info != null)
            {
                //判断卡券库存问题
                if (modle.Quantity - info.Used > 10 || info.Used == 0)
                {
                    modle.Id = info.Id;
                    res = _AppContext.CustomCardInfoApp.UpdateCustomCardInfo(modle);
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = "输入的库存数量必须大于当前库存";
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "修改库存失败";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新增卡券信息
        /// </summary>
        /// <param name="modle">卡券信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveCustomCard(CustomCardInfo modle)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true };
            //该活动下，是否已经有同名的卡券
            var isExist = _AppContext.CustomCardInfoApp.IsExistsCustomCardInfo(modle.ActivityType, modle.CardName, modle.CardSource);
            if (!isExist.IsSuccess)
            {
                return Json(isExist, JsonRequestBehavior.AllowGet);
            }
            modle.UserId = User.Identity.GetUserId();

            //添加卡券信息入库
            result = _AppContext.CustomCardInfoApp.AddCustomCardInfo(modle);
            if (result.IsSuccess)
            {
                //卡券活动类型入库
                _AppContext.SCServiceCardTypeApp.AddSCServiceCardType(
                    new SCServiceCardType()
                    {
                        CardType = modle.CardType,
                        CardTypeName = modle.CardName,
                        ActivityType = modle.ActivityType
                    });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private string imgExtends = ".bmp.png.jpeg.jpg.gif";

        /// <summary>
        /// 上传主视觉图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImage()
        {
            HttpContextBase context = this.HttpContext;

            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = context.Request.Files[0];
                string extendsName = Path.GetExtension(file.FileName);

                if (!this.imgExtends.Contains(extendsName.ToLower()) || file.ContentLength > 10485760)
                {
                    return Content("");
                }
                string newFileName = Guid.NewGuid().ToString("N") + extendsName;

                if (!Directory.Exists(HttpContext.Server.MapPath("/UploadImg")))
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("/UploadImg"));
                }
                var filePath = Path.Combine(HttpContext.Server.MapPath("/UploadImg/"), newFileName);
                file.SaveAs(filePath);
                return Content("/UploadImg/" + newFileName);
            }
            return Content("");
        }



        [HttpGet]
        public ActionResult GetCustomCardList(int source, string merchant, string actType, string cardName, int status, string reduceCost, int pageindex)
        {
            int pageSize = 10;
            int totalCount;
            var list = _AppContext.CustomCardInfoApp.GetCustomCardInfoList(source, merchant, actType, cardName, status, reduceCost, pageindex, pageSize, out totalCount);
            PagedList<CustomCardInfo> CustomCardList = new PagedList<CustomCardInfo>(list, pageindex, pageSize, totalCount);
            return View(CustomCardList);
        }

        [HttpGet]
        public JsonResult GetSCServiceCardTypeList(int type, int source, int iswx = 0)
        {
            var list = _AppContext.SCServiceCardTypeApp.GetSCServiceCardTypeList(type, source, iswx).ToList();
            var activityTypelist = list.Select(a => a.ActivityType);
            return Json(activityTypelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetActiveTagName()
        {
            var list= _AppContext.SCServiceCardTypeApp.GetActiveTagName();
            var ActiveTagNames= list.Select(i=>i.ActivityType).Distinct();
            return Json(ActiveTagNames, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSCServiceCardNameList(string name)
        {
            var list = _AppContext.SCServiceCardTypeApp.GetScServiceCardTypeNameListByActivityType(name).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 卡券核销 活动名称列表
        /// </summary>
        /// <param name="actName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCustomCardListByAct(string actName)
        {
            var list = _AppContext.SCServiceCardTypeApp.GetScServiceCardTypeNameListByActivityType(actName).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(string cardType)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            var card = _AppContext.CustomCardApp.GetUserCustomCardByCardType(cardType);
            if (card == null)
            {
                _AppContext.CustomCardInfoApp.DeleteCustomCardById(cardType);
            }
            else
            {
                res.Message = "该卡券已被使用不能删除";
                res.IsSuccess = false;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #region  卡券补发；
        public ActionResult ReissueIndex()
        {
            return View();
        }

        public ActionResult PartialUserCustomCard(string phone, string actType, string cardName, int pageIndex = 1)
        {
            string userId = "";
            var member = _AppContext.DealerMembershipApp.SelectMemberListByphoneNumber(phone).ToList();
            if (member != null && member.Count > 0)
            {
                userId = member.FirstOrDefault().Id;
            }
            int pageSize = 10;
            int total = 0;
            PageData page = new PageData() { Index = pageIndex, Size = pageSize };
            var list = _AppContext.CustomCardApp.GetUserCustomCardListByPhone(userId, actType, cardName, pageIndex, pageSize, out total);
            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < count ? (pageIndex + 1) : count;
            ViewBag.EndPage = count;
            return PartialView(list);
        }

        public JsonResult AddUserCustCardRemel(Remeal model)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            bool flag = _AppContext.CustomCardApp.GetCountRepar(model.Vin, model.CardType) > 0;//悦纳保养套餐券
            if (flag)
            {
                res.IsSuccess = false;
                res.Message = "该车架号已经购买过套餐";
                return Json(res,JsonRequestBehavior.AllowGet);
            }
            var member = _AppContext.DealerMembershipApp.SelectMemberListByphoneNumber(model.PhoneNumber).ToList();
            if (member != null && member.Count>0)
            {
                var userId = member.FirstOrDefault().Id;
                //领取兑奖码
                var cardCode = "";
                var resCardCode = "";

                var DealerIdItem = UserManager.FindById(this.User.Identity.GetUserId());
                if (DealerIdItem != null)//经销商登录
                {
                    model.DearID = DealerIdItem.DealerId;
                    CSCarDealerShip _dealer = _AppContext.DealerApp.GetDealerByDealerId(DealerIdItem.DealerId);
                    if (_dealer!=null)
                      model.DearName = _dealer.Name;
                }
                string level = member.FirstOrDefault().MLevel;
                switch (level)
                {
                    case "10":
                        model.Mlevel = "普卡";
                        break;
                    case "11":
                        model.Mlevel = "银卡";
                        break;
                    case "12":
                        model.Mlevel = "金卡";
                        break;
                    default:
                        model.Mlevel = "注册用户";
                        break;

                }
                ReturnResult res1 = _AppContext.CustomCardApp.AddRepair(model);

                if (!res1.IsSuccess)
                {
                    res.IsSuccess = false;
                    res.Message = "购买套餐失败";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                var usedCount = _AppContext.CustomCardApp.GetCarsByCardName(model.CardType);
                if (usedCount!=null && usedCount.Count()>0)
                {
                    var usedCounts=usedCount.OrderByDescending(i => i.CreateDate).ToList();
                    foreach (CustomCardInfo cardinfo in usedCounts)
                    {
                        var ucount=_AppContext.CustomCardApp.GetCardUsedCount(cardinfo.CardType);
                        if (cardinfo.Quantity - ucount > 0)
                        {
                            SendCustom(cardinfo, RandomNumberHelper.GetUserCustomCardCode(), userId, model.PhoneNumber, model.Vin);
                        } 
                    }
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "该手机号未注册";
                return Json(res,JsonRequestBehavior.AllowGet);
            }
            return Json(res,JsonRequestBehavior.AllowGet);

        }

        public void SendCustom(CustomCardInfo customCardInfo, string cardCode, string userId, string phone, string Vin)
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
                OpenId = Vin,
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

        public JsonResult AddUserCustomCard(string phone, string actType, string cardName)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            int serReissueCount = _AppContext.CustomCardApp.GetUserReissueCount(cardName, phone);
            if (serReissueCount > 0)
            {
                res.IsSuccess = false;
                res.Message = "用户已经补发过一次了";
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            var member = _AppContext.DealerMembershipApp.SelectMemberListByphoneNumber(phone).ToList();
            if (member.Count > 0)
            {
                var userId = member.FirstOrDefault().Id;
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardName);//single table select
                if (customCardInfo == null)
                {
                    res.IsSuccess = false;
                    res.Message = "卡券信息不正确";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                if (customCardInfo != null)
                {
                    var today = DateTime.Now;
                    if (today < customCardInfo.CardBeginDate || today > customCardInfo.CardEndDate)
                    {
                        res.IsSuccess = false;
                        res.Message = "卡券不在有效期内，不能补发请联系管理员。";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                //领取兑奖码
                var cardCode = "";
                var resCardCode = "";
                if (customCardInfo.CardSource == 1)
                {
                    var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                    if (customCardInfo.Quantity - usedCount > 0)
                    {
                        cardCode = RandomNumberHelper.GetUserCustomCardCode();
                        resCardCode = cardCode;
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = "券码库存不足了";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(
                             customCardInfo.CardType);
                    if (merchant != null)
                    {
                        resCardCode = merchant.CardCode;
                        cardCode = string.Format("{0}[{1}]", merchant.CardType, merchant.CardCode);
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = "商户券码库存不足了";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }

                //构建用户卡券信息；
                var customcard = new CustomCard()
                {
                    CardType = customCardInfo.CardType,
                    CardCode = cardCode,
                    CardId = customCardInfo.Id,
                    CreateTime = DateTime.Now,
                    IsSave = true,
                    IsCancel = false,
                    UserId = userId,
                    IsReissue = true,
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
                    _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = resCardCode }, phone);

                    //更新卡券信息库存；
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "该手机号码未注册会员";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult CardCodeIndex(string actType, string cardType)
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            var model = new CustomCardMerchantConsumeCode() { CardType = cardType, ActivityType = actType };
            return View(model);
        }

        [HttpPost]
        public JsonResult Import(string path, string cardType)
        {
            var result = new ReturnResult() { IsSuccess = true };

            result = _AppContext.AirportServiceApp.ImportSNCard(path);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void Template()
        {
            string path = HttpContext.Server.MapPath("../Content/File/Template.txt");
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + path);//这个响应头实现下载保存

            //路径还有编码
            Response.Write(System.IO.File.ReadAllText(path, System.Text.Encoding.GetEncoding(936)));
        }


        #region 导入商户券码信息
        public JsonResult CardCodeImport(string path, string cardType)
        {
         
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            if (string.IsNullOrEmpty(path))
            {
                res.Message = "文件不能为空";
                res.IsSuccess = false;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            StreamReader sr = new StreamReader(HttpContext.Server.MapPath(path), Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var cardCode = string.Format("{0}[{1}]", cardType, line);
                    var info =
                        _DbSession.CustomCardMerchantConsumeCodeStorager.GetSingleCardMerchantConsumeCodeByCode(
                            cardType,
                            cardCode);
                    if (info == null)
                    {
                        CustomCardMerchantConsumeCode model = new CustomCardMerchantConsumeCode()
                        {
                            CardType = cardType,
                            CardCode = cardCode,
                            IsDel = true
                        };

                        bool isSuccess =
                            _DbSession.CustomCardMerchantConsumeCodeStorager.AddCustomCardMerchantConsumeCode(model);
                    }
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UploadCardCode(HttpPostedFileBase filebase)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            HttpPostedFileBase file = Request.Files["file-Portrait"];

            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                res.Message = "文件不能为空";
                res.IsSuccess = false;
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                string FileType = ".txt";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    res.Message = "文件类型不对，只能导入.txt格式的文件";
                    res.IsSuccess = false;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else if (filesize >= Maxsize)
                {
                    res.Message = "上传文件超过4M，不能上传";
                    res.IsSuccess = false;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                string filePath = "../upload/file/" + DateTime.Today.ToString("yyyyMMdd") + "/";
                string diskPath = HttpContext.Server.MapPath(filePath);
                if (!Directory.Exists(diskPath)) Directory.CreateDirectory(diskPath);
                savePath = Path.Combine(diskPath, FileName);
                try
                {
                    file.SaveAs(savePath);
                }
                catch (Exception)
                {
                    res.Message = "上传文件失败";
                    res.IsSuccess = false;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                res.Message = filePath + FileName;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PartialCustomCode(string cardType, int pageIndex = 1)
        {
            int pageSize = 10;
            int total = 0;
            PageData page = new PageData() { Index = pageIndex, Size = pageSize };
            var list = _AppContext.CustomCardMerchantConsumeCodeApp.GetCardMerchantConsumeCodeList(cardType, pageSize, pageIndex, out total);
            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < count ? (pageIndex + 1) : count;
            ViewBag.EndPage = count;
            return PartialView(list);
        }
        #endregion
    }
}