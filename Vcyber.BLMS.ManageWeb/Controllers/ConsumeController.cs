namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;

    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using NPOI.SS.Formula.Functions;
    using NPOI.SS.Formula.PTG;

    using PetaPoco;
    using System.Web.Mvc;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.ManageWeb.Models;
    using System.IO;
    using Microsoft.Reporting.WebForms;
    using System.Configuration;
    using Vcyber.BLMS.IRepository;
    using Vcyber.BLMS.Entity.SelectCondition;

    [MvcAuthorize]
    public class ConsumeController : Controller
    {
        private bool isDealer = false;

        private CSCarDealerShip currentDealer;

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
        //public ActionResult QueryOrders(QueryParamEntity entity, long page = 1, long itemsPerPage = 20)
        //{
        //    Page<CSMaintenance> list = _AppContext.ScheduleMaintApp.QueryOrders(entity, page, itemsPerPage);
        //    ViewData.Add("data", list);
        //    return this.View(entity);
        //}

        //public ActionResult Update(int id, QueryParamEntity entity, long page, long itemsPerPage)
        //{
        //    _AppContext.ScheduleMaintApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { entity, page, itemsPerPage });
        //}

        public void Init()
        {

            string dealerId = this.UserManager.FindById(User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                isDealer = true;
                currentDealer = _AppContext.DealerApp.GetDealerByDealerId(dealerId);

                ViewBag.ProvinceList = new List<SelectListItem> { new SelectListItem { Text = currentDealer.Province, Value = currentDealer.Province } };
                ViewBag.CityList = new List<SelectListItem> { new SelectListItem { Text = currentDealer.City, Value = currentDealer.City } };
                ViewBag.DealerList = new List<SelectListItem> { new SelectListItem { Text = currentDealer.Name, Value = currentDealer.DealerId } };
                ViewBag.DealerName = currentDealer.Name;
            }
            else
            {


                var provinceList =
                    _AppContext.DealerApp.GetProvinceList().Select(x => new SelectListItem { Text = x, Value = x }).ToList();
                var listItem = new SelectListItem() { Text = "请选择", Value = "-1" };
                provinceList.Insert(0, listItem);

                //var province = provinceList.First().Value;
                //var cityList = _AppContext.DealerApp.GetCityListByProvince(province).
                //    Select(x => new SelectListItem { Text = x, Value = x });

                //var city = cityList.First().Value;
                //var dealerList = _AppContext.DealerApp.GetDealerList(province, city).
                //    Select(x => new SelectListItem { Text = x.Name, Value = x.DealerId });;

                ViewBag.ProvinceList = provinceList;
                ViewBag.CityList = new List<SelectListItem> { new SelectListItem { Text = "请选择", Value = "-1" } };

                ViewBag.DealerList = new List<SelectListItem> { new SelectListItem { Text = "请选择", Value = "-1" } };
            }
        }

        /// <summary>
        /// 新增消费记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(ConsumeEntity entity)
        {
            entity.PaperOrder = Vcyber.BLMS.Common.FilterStr.FormatHTML(entity.PaperOrder);
            this.Init();
            entity.ConsumeType = (int)entity.EConsumeType;
            if (string.IsNullOrEmpty(entity.UserId))
            {
                return this.View(entity);
            }
            entity.IdentityNumber = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.UserId).Result.IdentityNumber;
            if (isDealer && entity.DealerId != currentDealer.DealerId)
            {
                entity.ErrorMsg = "经销商验证失败！";
                return this.View(this.Reset(entity));
            }

            if (ModelState.IsValid)
            {
                if (true)//entity.ConsumePoints == 0)
                //|| _AppContext.UserSecurityApp.ValidateMobileVerifyCode(entity.Phone, entity.VerifyCode).IsSuccess
                {
                    int oldUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);

                    //消耗积分
                    if ((entity.ConsumePoints ?? 0) > 0)
                    {
                        if (
                            !_AppContext.TradePort.TradeService(
                                entity.UserId,
                                entity.ConsumePoints ?? 0,
                                (EOrderMode)entity.EConsumeType))
                        {
                            entity.ErrorMsg = "消费积分失败";
                            return this.View(Reset(entity));
                        }
                    }

                    //增加消费记录
                    int id = _AppContext.ConsumeApp.Add(
                      entity,
                      HttpContext.User.Identity.GetUserId(),
                      HttpContext.User.Identity.GetUserName());

                    //直接发放积分
                    if (_AppContext.DealerMembershipApp.IsPersonalUser(entity.IdentityNumber))
                    {
                        if (entity.ConsumeType != 2)//不是购车
                        {
                            _AppContext.ConsumeApp.AddAndProcess(entity.UserId, id);
                        }
                    }
                    int newUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);

                    if ((entity.ConsumePoints ?? 0) > 0)
                    {
                        var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);
                        Vcyber.BLMS.Entity.UserMessageRecord userMessageRecord = new Vcyber.BLMS.Entity.UserMessageRecord();
                        userMessageRecord.UserId = entity.UserId;
                        userMessageRecord.MsgType = MessageType.IntegralConsum;
                        userMessageRecord.MsgContent = string.Format("您好，您于{0}使用{1}积分抵维保消费{2}元，感谢您的支持，祝您生活愉快。",
                            DateTime.Now, entity.ConsumePoints, entity.ConsumePoints * 0.1);
                        _AppContext.UserMessageRecordApp.Insert(userMessageRecord);
                        _AppContext.SMSApp.SendSMS(ESmsType.消费管理_积分消耗成功, entity.Phone,
                            new string[]
                            {
                                DateTime.Now.Year.ToString(),
                                DateTime.Now.Month.ToString(),
                                DateTime.Now.Day.ToString(),
                                entity.DealerId + "-" + entity.DealerName,
                                Common.EnumExtension.GetDiscribe<EConsumeType>(entity.EConsumeType),
                                oldUserIntegral.ToString(),
                                (entity.ConsumePoints ?? 0).ToString(),
                                ((int) (entity.ConsumePoints ?? 0)*0.1).ToString(),
                                entity.TotalCost.ToString(),
                                (Math.Round(entity.TotalCost*Convert.ToDecimal (discount))).ToString(),
                                newUserIntegral.ToString()
                            });
                    }
                }
                return RedirectToAction("QueryOrders");
            }
            entity.ErrorMsg = "数据验证失败";
            return this.View(Reset(entity));
        }

       
        

        private ConsumeEntity Reset(ConsumeEntity entity)
        {
            entity.PartCost = 0;
            entity.MaterialCost = 0;
            return entity;
        }

        [HttpGet]
        public ActionResult Add()
        {
            this.Init();
            return this.View();
        }
        
        //public ActionResult QueryOrders()
        //{
        //    return this.View();
        //}
        //[HttpPost]
        /// <summary>
        /// 拿到消费记录列表
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="vin"></param>
        /// <param name="pointApproveStatus"></param>
        /// <param name="pointStatus"></param>
        /// <param name="minTotalCost"></param>
        /// <param name="maxTotalCost"></param>
        /// <param name="hasAttachment"></param>
        /// <param name="EConsumeType"></param>
        /// <param name="DealerId"></param>
        /// <param name="gstart"></param>
        /// <param name="gend"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public ActionResult QueryOrders(string phone, string vin, int? pointApproveStatus, int? pointStatus, int? minTotalCost, int? maxTotalCost, int? hasAttachment,
            string EConsumeType/*消费类型*/, string DealerId/*经销商*/, int? MLevel
            , string gstart, string gend,
            int? Minpoints,//最小消耗使用积分
            int? Maxpoints,//最大消耗使用积分
             long page = 1, long itemsPerPage = 10
            )
        {
            this.Init();
            if (currentDealer == null)
            {
                DealerId = DealerId ?? null;
            }
            else
            {
                DealerId = currentDealer.DealerId;
            }
            ViewBag.gstart = gstart;
            ViewBag.gend = gend;
            ConsumeQueryParamEntity entity = new ConsumeQueryParamEntity()
            {
                Phone = phone,
                PointApproveStatus = (EPointApproveStatus)(pointApproveStatus ?? ((int)EPointApproveStatus.All)),
                DealerId = DealerId,//经销商ID
                VIN = vin,
                PointStatus = (EPointStatus)(pointStatus ?? ((int)EPointStatus.All)),//积分发放状态
                MinTotalCost = minTotalCost ?? 0,
                MaxTotalCost = maxTotalCost ?? 0,
                HasAttachment = (EAttachmentStatus)(hasAttachment ?? ((int)EAttachmentStatus.All)),
                EConsumeType = (EConsumeType)Enum.Parse(typeof(EConsumeType), EConsumeType ?? "-1"),//消费类型
                Start = gstart,
                End = gend,
                Minpoints=Minpoints,
                Maxpoints=Maxpoints,
                MLevel = (EMLevelType)(MLevel ?? ((int)EMLevelType.ALL))
            };
            Page<CSConsume> list = _AppContext.ConsumeApp.QueryOrders(entity, page, itemsPerPage);
            list=(list == null ? new Page<CSConsume>() : _AppContext.ConsumeApp.QueryOrders(entity, page, itemsPerPage));
            ViewData.Add("data", list);
            return this.View(entity);
        }
        /// <summary>
        /// 导出 消费记录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="vin"></param>
        /// <param name="pointApproveStatus"></param>
        /// <param name="pointStatus"></param>
        /// <param name="minTotalCost"></param>
        /// <param name="maxTotalCost"></param>
        /// <param name="hasAttachment"></param>
        /// <param name="EConsumeType"></param>
        /// <param name="DealerId"></param>
        /// <param name="gstart"></param>
        /// <param name="gend"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [HttpGet]

        public ActionResult Export(string phone, string vin, int? pointApproveStatus, int? pointStatus, int? minTotalCost, int? maxTotalCost, int? hasAttachment, string EConsumeType/*消费类型*/, string DealerId/*经销商*/, int? MLevel
            , string gstart, string gend,
            int? Minpoints,//最小消耗使用积分
            int? Maxpoints,//最大消耗使用积分 
            long page = 1, long itemsPerPage = 100000)
        {
            this.Init();
            if (currentDealer == null)
            {
                DealerId = DealerId ?? null;
            }
            else
            {
                DealerId = currentDealer.DealerId;
            }
            ConsumeQueryParamEntity entity = new ConsumeQueryParamEntity()
            {
                Phone = phone,
                PointApproveStatus = (EPointApproveStatus)(pointApproveStatus ?? ((int)EPointApproveStatus.All)),
                DealerId = DealerId,//经销商ID
                VIN = vin,
                PointStatus = (EPointStatus)(pointStatus ?? ((int)EPointStatus.All)),//积分发放状态
                MinTotalCost = minTotalCost ?? 0,
                MaxTotalCost = maxTotalCost ?? 0,
                HasAttachment = (EAttachmentStatus)(hasAttachment ?? ((int)EAttachmentStatus.All)),
                EConsumeType = (EConsumeType)Enum.Parse(typeof(EConsumeType), EConsumeType ?? "-1"),//消费类型
                Start = gstart,
                End = gend,
                MLevel = (EMLevelType)(MLevel ?? ((int)EMLevelType.ALL)),
                Minpoints = Minpoints,
                Maxpoints = Maxpoints

            };
            Page<CSConsume> list = _AppContext.ConsumeApp.QueryOrders(entity, page, itemsPerPage);

            //IEnumerable<CSConsume> lists = list.Items.Select(
            //    x => new CSConsume()
            //             {
            //                 DealerId = x.DealerId,
            //                 ConsumeDate = x.ConsumeDate,
            //                 Phone = x.Phone,
            //                 IdentityNumber = x.IdentityNumber,
            //                 VIN = x.VIN,
            //                 ConsumeType = x.ConsumeType,
            //                 PartCost = x.PartCost,
            //                 MaterialCost = x.MaterialCost,
            //                 LaborCost = x.LaborCost,
            //                 PurchaseCost = x.PurchaseCost,
            //                 PointCost = x.PointCost,
            //                 TotalCost = x.TotalCost,
            //                 ConsumePoints = x.ConsumePoints,
            //                 RewardPoints = x.RewardPoints,
            //                 CreateTime = x.CreateTime
            //             });

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("店代码");
            row1.CreateCell(1).SetCellValue("消费时间");
            row1.CreateCell(2).SetCellValue("电话");
            row1.CreateCell(3).SetCellValue("证件号");
            row1.CreateCell(4).SetCellValue("VIN");
            row1.CreateCell(5).SetCellValue("消费类型");
            row1.CreateCell(6).SetCellValue("总费用");
            row1.CreateCell(7).SetCellValue("积分抵扣");
            row1.CreateCell(8).SetCellValue("实际支付费用");
            row1.CreateCell(9).SetCellValue("消耗积分");
            row1.CreateCell(10).SetCellValue("产生积分");
            row1.CreateCell(11).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("会员等级");

            int count = 0;

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Items.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                CSConsume item = list.Items[i];
                rowtemp.CreateCell(0).SetCellValue(item.DealerId);
                rowtemp.CreateCell(1).SetCellValue(item.ConsumeDate.ToString());
                rowtemp.CreateCell(2).SetCellValue(item.Phone);
                rowtemp.CreateCell(3).SetCellValue(item.IdentityNumber);
                rowtemp.CreateCell(4).SetCellValue(item.VIN);
                rowtemp.CreateCell(5).SetCellValue(item.ConsumeTypeString);
                rowtemp.CreateCell(6).SetCellValue((item.PartCost + item.MaterialCost + (item.LaborCost == null ? 0 : item.LaborCost) + (item.PurchaseCost == null ? 0 : item.PurchaseCost)).ToString());
                rowtemp.CreateCell(7).SetCellValue(item.PointCost.ToString());
                rowtemp.CreateCell(8).SetCellValue(item.TotalCost.ToString());
                rowtemp.CreateCell(9).SetCellValue(item.ConsumePoints.ToString());
                rowtemp.CreateCell(10).SetCellValue(item.RewardPoints.ToString());
                rowtemp.CreateCell(11).SetCellValue("注册用户");

                if (item.MLevel == 10)
                {
                    rowtemp.CreateCell(11).SetCellValue("普卡");
                }
                if (item.MLevel == 11)
                {
                    rowtemp.CreateCell(11).SetCellValue("银卡");
                }
                if (item.MLevel == 12)
                {
                    rowtemp.CreateCell(11).SetCellValue("金卡");
                }
               
                count++;
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "消费明细.xls");
        }

        //审核
        [HttpPost]
        private JsonResult UpdateApproveStatus(int id, EPointApproveStatus status)
        {
            if (_AppContext.ConsumeApp.UpdateStatus(
                id,
                status,
                HttpContext.User.Identity.GetUserId(),
                HttpContext.User.Identity.Name) > 0) return Json(new { code = 200, status = status.DisplayName(), statusValue = status });
            return Json(new { code = 500, status = "审核失败" });
        }

        public JsonResult BatchUpdateApproveStats(string ids, EPointApproveStatus status)
        {
            return Json(_AppContext.ConsumeApp.BatchUpdateStatus(
                ids,
                status,
                HttpContext.User.Identity.GetUserId(),
                HttpContext.User.Identity.GetUserName()));
        }


        //public ActionResult Update(int id, string phone, string orderNo, string dealerId, string pointApproveStatus, string pointStatus, int minTotalCost, int maxTotalCost, bool hasAttachment, long page = 1, long itemsPerPage = 2)
        //{
        //    //if (string.IsNullOrEmpty(pointApproveStatus)) pointApproveStatus = EPointApproveStatus.NoBegin.ToString();
        //    //EPointApproveStatus eApproveStatus;
        //    //if (!Enum.TryParse(pointApproveStatus, out eApproveStatus)) eApproveStatus = EPointApproveStatus.NoBegin;

        //    //if (string.IsNullOrEmpty(pointStatus)) pointApproveStatus = EPointStatus.ToDo.ToString();
        //    //EPointStatus oPointStatus;
        //    //if (!Enum.TryParse(pointStatus, out oPointStatus)) oPointStatus = EPointStatus.ToDo;

        //    _AppContext.ScheduleMaintApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { phone, orderNo, dealerId, pointApproveStatus, pointStatus, minTotalCost, maxTotalCost, hasAttachment, page, itemsPerPage });
        //}

        /// <summary>
        /// 供应商下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceCity()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return Json(_provinces);
        }

        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public JsonResult Citys(string provinceValue)
        {
            IList<string> _result = new List<string>();
            IEnumerable<string> _citys = _AppContext.DealerApp.GetCityListByProvince(provinceValue);
            if (_citys != null && _citys.Any())
            {
                _result = _citys.ToList();
            }
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="cityValue"></param>
        /// <returns></returns>
        public JsonResult Dealers(string cityValue, string provinceValue)
        {
            IList<CSCarDealerShip> result = new List<CSCarDealerShip>();
            IEnumerable<CSCarDealerShip> dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);

            if (dealers != null && dealers.Any())
            {
                result = dealers.ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据手机号查询到资料
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public JsonResult GetUserInfo(string phone)
        {
            FrontUserStore<FrontIdentityUser> frontUserStore = new FrontUserStore<FrontIdentityUser>();
            FrontIdentityUser user = frontUserStore.FindByNameAsync(phone).Result;
            if (user == null)
            {
                return Json(new ReturnResult() { IsSuccess = false, Message = "未找到用户" }, JsonRequestBehavior.AllowGet);
            }
            // 10 11 12 都是车主 
            //if (user.MLevel != 10 || user.MLevel != 11 || user.MLevel != 12 || string.IsNullOrEmpty(user.No))
            //{
            //    return Json(new ReturnResult() { IsSuccess = false, Message = "非车主用户不能使用此服务" }, JsonRequestBehavior.AllowGet);
            //}


            string identityNo = user.IdentityNumber;
            if (string.IsNullOrEmpty(identityNo))
                return Json(new ReturnResult() { IsSuccess = false, Message = "未找到用户身份证号" }, JsonRequestBehavior.AllowGet);
            IFCustomer customer = _AppContext.CarServiceUserApp.GetCustomer(identityNo);

            //从BM中查询不到去Crm中查询一遍
            if (customer==null)
            {
                customer=_DbSession.CarServiceUserStorager.OldFindCustomer(new CustomerCondition { IdentityNumber = identityNo }).ElementAtOrDefault(0);
            }

            if (customer == null)
                return Json(new ReturnResult() { IsSuccess = false, Message = "未找到车主资料" }, JsonRequestBehavior.AllowGet);
            var Vins = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNo).Select(x => new[] { x.VIN, x.VIN + " - " + x.CarCategory });

            if (Vins.Count()==0 )
            {
                Vins = _AppContext.CarServiceUserApp.SelectCarListByIdentityOld(customer.IdentityNumber).Select(x => new[] { x.VIN, x.VIN + " - " + x.CarCategory });


            }

            var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(user.Id);
            bool flag = _DbSession.DealerMembershipStorager.IsPersonalUser(user.IdentityNumber);
            dynamic u = new
            {
                userId = user.Id,
                userName = customer.CustName,
                identityNo = customer.IdentityNumber,
                vins = Vins,
                mlevel=user.MLevel,
                discount,
                userPoints = _AppContext.UserIntegralApp.GetTotalIntegral(user.Id),
                isperson = (flag?"1":"2")//1是个人用户 2非个人用户
            };
            return Json(new ReturnResult() { Data = u, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendVerifyCode(string phone, int points)
        {
            ReturnResult result = _AppContext.UserSecurityApp.SendVerifyCodeWithMessage(phone, 4, "您有一笔消费将消耗" + points + "积分");
            // string.Format("亲爱的bluemembers会员，您于{0}在{1}办理了{2}服务，成功使用{3}积分抵扣{4}元现金。截止目前，您的账户积分余额为{5}。【bluemembers官网】（网址短链）",
            //DateTime.Today.ToString("yyyy年M月d日"))
            //ReturnResult result = new ReturnResult { IsSuccess = true };
            if (result.IsSuccess)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyCode(string phone, string code)
        {
            ReturnResult result = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, code);
            return Json(result.IsSuccess, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult ReleasePoints(int id)
        //{
        //    CSConsume consume = _AppContext.ConsumeApp.GetById(id);
        //    consume.RewardPoints = (int)((consume.PartCost + consume.MaterialCost - consume.PointCost ?? 0) / 100);
        //    consume.PointStatus = (int)EPointStatus.Done;
        //    consume.UpdateId = HttpContext.User.Identity.GetUserId();
        //    consume.UpdateName = HttpContext.User.Identity.Name;
        //    consume.UpdateTime = DateTime.Now;
        //    _AppContext.ConsumeApp.UpdateConsume(consume);
        //    _AppContext.UserIntegralApp.Add(new UserIntegral()
        //                                        {
        //                                            CreateTime = DateTime.Now,
        //                                            datastate = (int)EDataStatus.NoDelete,
        //                                            integralSource = "手工发放",
        //                                            UpdateTime = DateTime.Now,
        //                                            userId = consume.UserId,
        //                                            usevalue = 0,
        //                                            value = consume.RewardPoints ?? 0
        //                                        });
        //    return Json(new { rewardPoints = consume.RewardPoints, pointStatus = consume.PointStatus });
        //}



    }
}
