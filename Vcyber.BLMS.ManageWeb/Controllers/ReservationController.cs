using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity;
using WebGrease;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System.Collections;
    using System.Web.UI.WebControls;

    using Omu.ValueInjecter;

    using Vcyber.BLMS.Common;

    [MvcAuthorize]
    public class ReservationController : Controller
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
        // GET: /ReservationEntity/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(DateTime? createFromDate, DateTime? createToDate, string orderType, EOrderState? state, EExportSatus? isExported, long page = 1, long itemsPerPage = 10)
        {
            try
            {
                EOrderType oType;
                if (!Enum.TryParse(orderType, out oType)) oType = EOrderType.Care;

                //ReservationEntity entity = new ReservationEntity() { ReservationType = oType, State = state ?? EOrderState.ToBeProcess,IsExported = isExported};

                QueryParamEntity queryParam = new QueryParamEntity() { State = state ?? EOrderState.All, IsExported = isExported ?? EExportSatus.All, CreateFromDate = createFromDate, CreateToDate = createToDate };
                queryParam.DealerId = UserManager.FindById(User.Identity.GetUserId()).DealerId;
                queryParam.OrderType = oType;


                ViewData.Add("data", this.QueryData(oType, queryParam, page, itemsPerPage));
                return View(queryParam);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public ActionResult ExportDataOld(string reservationType, EOrderState? eState, int isExported)
        {
            EOrderType oType;
            if (!Enum.TryParse(reservationType, out oType)) oType = EOrderType.Care;

            //ReservationEntity entity = new ReservationEntity() { ReservationType = oType, State = state ?? EOrderState.ToBeProcess };

            QueryParamEntity queryParam = new QueryParamEntity() { State = eState ?? EOrderState.ToBeProcess };
            queryParam.DealerId = UserManager.FindById(User.Identity.GetUserId()).DealerId;

            Page<ReservationEntity> pages = this.QueryData(oType, queryParam, 1, 50000);
            IEnumerable<ReservationExcelEntity> list = pages.Items.Select(
                x => new ReservationExcelEntity()
                         {
                             Phone = x.Phone,
                             ReservationType = x.ReservationType.DisplayName(),
                             ScheduleDate =
                                 (x.ScheduleDate == null)
                                     ? string.Empty
                                     : ((DateTime)x.ScheduleDate).ToString(
                                         "yyyy/MM/dd"),
                             State = ((EOrderState)(x.State)).DisplayName(),
                             UserName = x.UserName
                         });

            string fileName = string.Format("预约待受理{0}", DateTime.Now.ToString("yyyyMMdd")) + ".xls";
            List<string> propertyName = new List<string> { "ReservationType", "ScheduleDate", "Phone", "UserName", "State" };
            List<string> columName = new List<string> { "预约类型", "预约时间", "电话", "姓名", "受理状态" };
            NPOIHelper<ReservationExcelEntity>.ListToExcel(list.ToList(), fileName, propertyName, columName);

            return null;
        }

        public ActionResult ExportData(DateTime? createFromDate, DateTime? createToDate, string reservationType, EOrderState? state, EExportSatus isExported)
        {
            //设置查询条件
            QueryParamEntity queryParam = new QueryParamEntity() { State = state ?? EOrderState.ToBeProcess, IsExported = isExported, CreateFromDate = createFromDate, CreateToDate = createToDate };
            queryParam.DealerId = UserManager.FindById(User.Identity.GetUserId()).DealerId;

            //总数据集合
            List<IList<ReservationExcel>> list = new List<IList<ReservationExcel>>();
            List<List<string>> propertyName = new List<List<string>>();
            List<List<string>> columName = new List<List<string>>();
            List<string> sheetName = new List<string>();

            //在线订车
            Page<CSOrderCar> listOrderCar = _AppContext.OrderCarApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName1 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "Email", "CreateTime", "DataSource", "OrderNo" ,"StatusName" };
            List<string> columName1 = new List<string> { "姓名", "性别", "手机号", "订购车型", "店代码", "经销商", "电子邮箱", "提交时间", "来源", "预约单号", "受理状态" };
            IList<ReservationExcel> list1 = listOrderCar.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                CarSeries = x.CarSeries,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                Email = x.Email,
                DealerName = x.DealerName,
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list1);
            propertyName.Add(propertyName1);
            columName.Add(columName1);
            sheetName.Add("线上订车");

            //预约试驾
            Page<CSTestDrive> listTestDrive = _AppContext.TestDriveApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName2 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "ScheduleDate", "CreateTime", "DataSource", "", "OrderNo", "StatusName" };
            List<string> columName2 = new List<string> { "姓名", "性别", "手机号", "试驾车型", "店代码", "经销商", "试驾时间", "提交时间", "来源", "计划购车时间", "预约单号", "受理状态" };

            IList<ReservationExcel> list2 = listTestDrive.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                CarSeries = x.CarSeries,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                PurchaseTimeFrame = x.PurchaseTimeFrame,
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list2);
            propertyName.Add(propertyName2);
            columName.Add(columName2);
            sheetName.Add("预约试驾");

            //预约维保
            queryParam.OrderType = EOrderType.CommonMaintain;
            Page<CSSonataService> listSonataService = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName3 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "LicensePlate", "VIN", "MaintainType", "ScheduleDate", "CreateTime", "DataSource", "Comment", "PurchaseYear", "MileAge", "OrderNo", "Email", "StatusName" };
            List<string> columName3 = new List<string> { "姓名", "性别", "手机号", "车型", "店代码", "选择的经销商", "车牌号", "车架号", "服务项目", "预计到店时间", "提交时间", "来源", "补充说明", "购车年份", "行驶里程", "预约单号", "电子邮箱", "受理状态" };

            IList<ReservationExcel> list3 = listSonataService.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                VIN = x.VIN,
                CarSeries = x.CarSeries,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                Email = x.Email,
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                PurchaseYear = x.PurchaseYear,
                MaintainType = x.MaintainType.ToString(),
                MileAge = x.MileAge.ToString(),
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list3);
            propertyName.Add(propertyName3);
            columName.Add(columName3);
            sheetName.Add("普通预约维保服务");


            //上门取送车
            queryParam.OrderType = EOrderType.Home2Home;
            Page<CSSonataService> listHome2Home = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName4 = new List<string> { "UserName", "UserSex", "Phone", "TakeAddress", "ReturnAddress", "ScheduleDate", "ReturnDate", "CarSeries", "VIN", "LicensePlate", "DealerId", "DealerName", "Comment", "CreateTime", "DataSource", "OrderNo", "StatusName" };
            List<string> columName4 = new List<string> { "姓名", "性别", "手机号", "取车地点", "送车地点", "取车时间", "送车时间", "车型", "车架号", "车牌号", "店代码", "选择的4S店", "补充说明", "提交时间", "来源", "预约单号", "受理状态" };


            IList<ReservationExcel> list4 = listHome2Home.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                VIN = x.VIN,
                CarSeries = x.CarSeries,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                TakeAddress = x.TakeAddress,
                ReturnAddress = x.ReturnAddress,
                ReturnDate = x.ReturnDate == null ? "" : ((DateTime)x.ReturnDate).ToString("yyyy-MM-dd"),
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list4);
            propertyName.Add(propertyName4);
            columName.Add(columName4);
            sheetName.Add("上门取送车服务");

            //上门关怀
            queryParam.OrderType = EOrderType.Care;
            Page<CSSonataService> listCare = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName5 = new List<string> { "UserName", "UserSex", "Phone", "TakeAddress", "CarSeries", "LicensePlate", "VIN", "DealerId", "DealerName", "ScheduleDate", "Comment", "CreateTime", "DataSource", "OrderNo", "StatusName" };
            List<string> columName5 = new List<string> { "姓名", "性别", "手机号", "上门地点", "车型", "车牌号", "车架号", "店代码", "选择的4S店", "预计上门时间", "补充说明", "提交时间", "来源", "预约单号", "受理状态" };


            IList<ReservationExcel> list5 = listCare.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                VIN = x.VIN,
                CarSeries = x.CarSeries,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                TakeAddress = x.TakeAddress,
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list5);
            propertyName.Add(propertyName5);
            columName.Add(columName5);
            sheetName.Add("上门关怀服务");

            //1对1
            queryParam.OrderType = EOrderType.SpecialMaintain;
            Page<CSSonataService> listSpecialMaintain = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);

            List<string> propertyName6 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "ConsultantName", "LicensePlate", "ScheduleDate", "Comment", "CreateTime", "DataSource", "OrderNo", "StatusName" };
            List<string> columName6 = new List<string> { "姓名", "性别", "手机号", "车型", "店代码", "选择的4S店", "服务顾问", "车牌号", "预计到店时间", "补充说明", "提交时间", "来源", "预约单号", "受理状态" };

            IList<ReservationExcel> list6 = listSpecialMaintain.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                CarSeries = x.CarSeries,
                ConsultantName = x.ConsultantName,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list6);
            propertyName.Add(propertyName6);
            columName.Add(columName6);
            sheetName.Add("一对一专属服务");

            //3年9次
            queryParam.OrderType = EOrderType.FreeCheck;
            Page<CSSonataService> listFreeCheck = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName7 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "LicensePlate", "VIN", "ScheduleDate", "Comment", "PurchaseYear", "MileAge", "CreateTime", "DataSource", "OrderNo", "Email", "StatusName" };
            List<string> columName7 = new List<string> { "姓名", "性别", "手机号", "车型", "店代码", "选择的经销商", "车牌号", "车架号", "预计到店时间", "补充说明", "购车年份", "行驶里程", "提交时间", "来源", "预约单号", "电子邮箱", "受理状态" };
            IList<ReservationExcel> list7 = listFreeCheck.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                VIN = x.VIN,
                CarSeries = x.CarSeries,
                ConsultantName = x.ConsultantName,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                Email = x.Email,
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                PurchaseYear = x.PurchaseYear,
                MileAge = x.MileAge.ToString(),
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list7);
            propertyName.Add(propertyName7);
            columName.Add(columName7);
            sheetName.Add("三年九次免检服务");


            //长途旅行关怀服务
            queryParam.OrderType = EOrderType.LongDistanceTravel;
            Page<CSSonataService> listLongDistanceTravel = _AppContext.SonataServiceApp.QueryOrders(queryParam, 1, 50000);
            List<string> propertyName8 = new List<string> { "UserName", "UserSex", "Phone", "CarSeries", "DealerId", "DealerName", "LicensePlate", "VIN", "ScheduleDate", "Comment", "PurchaseYear", "MileAge", "CreateTime", "DataSource", "OrderNo", "Email", "StatusName" };
            List<string> columName8 = new List<string> { "姓名", "性别", "手机号", "车型", "店代码", "选择的经销商", "车牌号", "车架号", "预计到店时间", "补充说明", "购车年份", "行驶里程", "提交时间", "来源", "预约单号", "电子邮箱", "受理状态" };
            IList<ReservationExcel> list8 = listLongDistanceTravel.Items.Select(x => new ReservationExcel()
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                VIN = x.VIN,
                CarSeries = x.CarSeries,
                ConsultantName = x.ConsultantName,
                Phone = x.Phone,
                UserName = x.UserName,
                UserSex = x.UserSex.ToString(),
                Email = x.Email,
                DealerName = x.DealerName,
                ScheduleDate = x.ScheduleDate == null ? "" : ((DateTime)x.ScheduleDate).ToString("yyyy-MM-dd"),
                LicensePlate = x.LicensePlate,
                Comment = x.Comment,
                PurchaseYear = x.PurchaseYear,
                MileAge = x.MileAge.ToString(),
                DealerId = x.DealerId,
                CreateTime = x.CreateTime == null ? "" : ((DateTime)x.CreateTime).ToString(),
                DataSource = x.DataSource,
                StatusName = ((EOrderState)x.State).DisplayName()
            }).ToList();

            list.Add(list8);
            propertyName.Add(propertyName8);
            columName.Add(columName8);
            sheetName.Add("长途旅行关怀服务");



            //更新状态
            foreach (var item in list1)
            {
                _AppContext.OrderCarApp.UpdateIsExported(item.Id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name);
            }

            foreach (var item in list2)
            {
                _AppContext.TestDriveApp.UpdateIsExported(item.Id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name);
            }

            UpdateIsExported(list3);
            UpdateIsExported(list4);
            UpdateIsExported(list5);
            UpdateIsExported(list6);
            UpdateIsExported(list7);

            string fileName = string.Format("预约待受理{0}", DateTime.Now.ToString("yyyyMMdd")) + ".xls";

            NPOIHelper<ReservationExcel>.ListToExcelEX(list, fileName, propertyName, columName, sheetName);

            return null;
        }

        private void UpdateIsExported(IList<ReservationExcel> list)
        {
            foreach (var item in list)
            {
                _AppContext.SonataServiceApp.UpdateIsExported(item.Id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name);
            }
        }


        private Page<ReservationEntity> QueryData(EOrderType type, QueryParamEntity queryParam, long page, long itemsPerPage)
        {
            Page<ReservationEntity> realList = new Page<ReservationEntity>();
            switch (type)
            {
                case EOrderType.OrderCar:
                    Page<CSOrderCar> listOrderCar = _AppContext.OrderCarApp.QueryOrders(queryParam, page, itemsPerPage);
                    realList = ConvertList<CSOrderCar, ReservationEntity>(listOrderCar);
                    break;
                case EOrderType.TestDrive:
                    Page<CSTestDrive> listTestDrive = _AppContext.TestDriveApp.QueryOrders(queryParam, page, itemsPerPage);
                    realList = ConvertList<CSTestDrive, ReservationEntity>(listTestDrive);
                    break;
                case EOrderType.Home2Home:
                case EOrderType.Care:
                case EOrderType.SpecialMaintain:
                case EOrderType.FreeCheck:
                case EOrderType.CommonMaintain:
                case EOrderType.LongDistanceTravel:

                    queryParam.OrderType = type;
                    Page<CSSonataService> listSonataService = _AppContext.SonataServiceApp.QueryOrders(
                        queryParam,
                        page,
                        itemsPerPage);
                    realList = ConvertList<CSSonataService, ReservationEntity>(listSonataService);
                    break;

            }
            return realList;
        }

        #region detail actions

        public ActionResult OrderCarDetail(int id)
        {
            CSOrderCar entity = _AppContext.OrderCarApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }


        public ActionResult TestDriveDetail(int id)
        {
            CSTestDrive entity = _AppContext.TestDriveApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        public ActionResult ScheduleMaintDetail(int id)
        {
            CSSonataService entity = _AppContext.SonataServiceApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        public ActionResult HomeToHomeDetail(int id)
        {
            CSSonataService entity = _AppContext.SonataServiceApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        public ActionResult DayCareDetail(int id)
        {
            CSSonataService entity = _AppContext.SonataServiceApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        public ActionResult OneOneDetail(int id)
        {
            CSSonataService entity = _AppContext.SonataServiceApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        public ActionResult YearTimeDetail(int id)
        {
            CSSonataService entity = _AppContext.SonataServiceApp.GetEntityById(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }
        #endregion

        private static Page<T2> ConvertList<T1, T2>(Page<T1> p1) where T2 : new()
        {
            List<T2> list2 = p1.Items.Select(
                x =>
                {
                    T2 t2 = new T2();
                    t2.InjectFrom(x);
                    return t2;
                }).ToList();

            return new Page<T2>
                              {
                                  CurrentPage = p1.CurrentPage,
                                  ItemsPerPage = p1.ItemsPerPage,
                                  TotalItems = p1.TotalItems,
                                  TotalPages = p1.TotalPages,
                                  Items = list2
                              };
        }
    }
}