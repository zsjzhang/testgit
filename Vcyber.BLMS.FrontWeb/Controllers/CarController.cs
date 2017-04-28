using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Entity.CarService;
using PetaPoco;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace Vcyber.BLMS.FrontWeb.Controllers
{
    using AspNet.Identity.SQL;
    using Vcyber.BLMS.FrontWeb.ServiceReference1;
    using Vcyber.BLMS.Entity;
    using Webdiyer.WebControls.Mvc;

    /// <summary>
    /// 索九专区、车辆服务
    /// </summary>
    public class CarController : Controller
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
        /// 车辆服务首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["ServieList"] = null;
            return View();
        }

        /// <summary>
        /// 汽车服务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id = -1)
        {
            ViewData["item"] = null;
            return View();
        }

        /// <summary>
        /// 首页的预约服务（预约试驾、在线订车、预约维修保养）
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeCarService()
        {
            return View();
        }

        /// <summary>
        /// 预约试驾
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeReserveDrive()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DoFittingValidate(string code, string address, float Longitude, float Latitude, float Altitude, string ctype)
        {
            string returnData = string.Empty;
            if (!User.Identity.IsAuthenticated)
            {

                return Json(new { code = 400, msg = "请登陆后验证配件" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var user = UserManager.FindById(this.User.Identity.GetUserId());
                try
                {
                    WebService1SoapClient PeijianClient = new WebService1SoapClient();
                    returnData = PeijianClient.CheckQRForWebNew(code, address, Longitude, Latitude, Altitude, user.Id, ctype);
                    FittingValidate FittingValidateEntity = new FittingValidate();
                    FittingValidateEntity.Code = code;
                    //  FittingValidateEntity.CreateTime =DateTime .Now ;
                    FittingValidateEntity.Ctype = ctype == string.Empty ? FittingValidateEntity.Ctype : ctype;
                    FittingValidateEntity.Latitude = Latitude;
                    FittingValidateEntity.Longitude = Longitude;
                    FittingValidateEntity.UserAddress = address;
                    FittingValidateEntity.Altitude = Altitude;
                    FittingValidateEntity.Result = returnData;
                    FittingValidateEntity.Userid = user.Id;
                    _AppContext.FittingValidateApp.Add(FittingValidateEntity);


                }

                catch (Exception ex)
                {
                    //return Json(new { code = 400, msg = "调用配件接口错误:" + ex.StackTrace.ToString() }, JsonRequestBehavior.AllowGet);
                    return Json(new { code = 400, msg = "查询不到此配件信息" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { code = 200, data = returnData }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult HomeFittingValidate()
        {
            return View();
        }
        /// <summary>
        /// form表单ajax预约试驾请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoReserveDrive(TestDriveEntity cstestDriveEntity)
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

            string userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            cstestDriveEntity.UserId = userId;
            cstestDriveEntity.DataSource = "blms";
            int _code = 200;
            int _result = _AppContext.TestDriveApp.Add(cstestDriveEntity);
            if (_result <= 0)
            {
                _code = 400;
            }
            _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, cstestDriveEntity.Phone, new string[] { });



            //if (User.Identity.IsAuthenticated)
            //{
            //    int outValue;
            //    var account =
            //        new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId()).Result;
            //    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.预约保养, account.Id, (MemshipLevel) account.MLevel,
            //    //    out outValue);
            //    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.预约保养, account.Id, out outValue);
            //}

            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 在线订车
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeCarReserveBuy()
        {
            return View();
        }

        /// <summary>
        /// form表单ajax在线订车请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoReserveBuy(OrderCarEntity csorderCarEntity)
        {
            string userId = string.Empty;
            string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            csorderCarEntity.UserId = userId;
            int _code = 200;
            csorderCarEntity.DataSource = "blms";
            int _result = _AppContext.OrderCarApp.Add(csorderCarEntity);
            if (_result <= 0)
            {
                _code = 400;
            }
            _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, csorderCarEntity.Phone, new string[] { });
            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预约维修保养
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeCarReserveMaintenance()
        {
            return View();
        }

        /// <summary>
        /// form表单ajax预约维修保养请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoReserveMaintenance(SonataServiceEntity csmaintenanceEntity)
        {
            string userId = string.Empty;
            string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }

            csmaintenanceEntity.DataSource = "blms";
            csmaintenanceEntity.UserId = userId;
            csmaintenanceEntity.OrderType = EBMServiceType.CommonMaintain;
            //判断此车型是不预约过
            QueryParamEntity par = new QueryParamEntity();
            par.ScheduleFromDate = csmaintenanceEntity.ScheduleDate;
            par.ScheduleToDate = csmaintenanceEntity.ScheduleDate;
            par.LicensePlate = csmaintenanceEntity.LicensePlate;
            var list = _AppContext.SonataServiceApp.QueryOrders(par, 1, 100);
            if (list.Items.Count > 0)
            {
                return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
            }
            int _code = 200;
            int _result = _AppContext.SonataServiceApp.Add(csmaintenanceEntity);
            if (_result <= 0)
            {
                _code = 400;
            }
            _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, csmaintenanceEntity.Phone, new string[] { });
            //if (User.Identity.IsAuthenticated)
            //{
            //    int outValue;
            //    var account =
            //        new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId()).Result;
            //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.预约保养, account.Id, (MemshipLevel) account.MLevel,
            //        out outValue);
            //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.预约保养, account.Id, out outValue);

            //}
            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首页索九专享服务(Sonata)
        /// </summary>
        /// <returns></returns>
        public ActionResult HotCarService()
        {
            return View();
        }

        /// <summary>
        /// 索九专享服务列表(Sonata)
        /// </summary>
        /// <returns></returns>
        public ActionResult CarServicePages()
        {
            return View();
        }

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult CarTypeView()
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return View(_result);
        }

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult CarTypeViewForType(ECarSeriesType type)
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(type);
            return View(_result);
        }

        public ActionResult CarTypeReverse()
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.Maintenance);
            return View(_result);
        }

        public ActionResult CarTypeViewForService()
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return View(_result);
        }

        public ActionResult AccountPageProvince()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        public ActionResult ReAccountPageProvince()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        /// <summary>
        /// 供应商下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceCity(int? IsWeibao = 0, int? Istestserver = 0, int? IsDingChe = 0)
        {
            ViewBag.IsWeibao = IsWeibao;
            ViewBag.Istestserver = Istestserver;
            ViewBag.IsDingChe = IsDingChe;
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        //public ActionResult GetDealerShipList(int Istestserver, int IsDingChe, int IsWeibao)
        //{
        //    IEnumerable<string> _provinces = _AppContext.DealerApp.GetDealerShipList( Istestserver,  IsDingChe,  IsWeibao);
        //    return View(_provinces);
        //}

        public ActionResult ProvinceCity_ReserveMaintenance()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        public ActionResult ProvinceCity_OrderCar()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
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
            IList<CSCarDealerShip> _result = new List<CSCarDealerShip>();
            IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
            if (_dealers != null && _dealers.Any())
            {
                _result = _dealers.ToList();
            }
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 个人中心-享受的服务
        /// 根据用户帐号获取该帐号所能享受的服务
        /// </summary>
        /// <returns></returns>
        public ActionResult MyService()
        {
            ViewData["myservices"] = null;
            return View();
        }

        /// <summary>
        /// 个人中心-享受的服务-快到期的服务
        /// </summary>
        /// <returns></returns>
        public ActionResult MySoonService()
        {
            Page<CSSonataServiceV> _serviceV = _AppContext.ScheduleServiceApp.QueryUserOrdersByType(this.User.Identity.GetUserId(), 1, 1);
            ViewData["soonService"] = _serviceV;
            return View();
        }




        public ActionResult CsConsumePagelist(int pageIndex = 1)
        {
            int total = 0;
            IEnumerable<CSConsume> userIntegralList = _AppContext.ConsumeApp.GetUserConsume(this.User.Identity.GetUserId(), pageIndex, 10, out total);
            PagedList<CSConsume> _pageresult = new PagedList<CSConsume>(userIntegralList, pageIndex, 10, total);


            return View(_pageresult);

        }
        /// <summary>
        /// 经销商展示页面(查询经销商)
        /// </summary>
        /// <returns></returns>
        public ActionResult DealerList()
        {
            return View();
        }

        #region 对外预约服务接口
        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public JsonpResult CarTypeWS(string callback)
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
                foreach (var item in _result)
                {
                    list.Add(new { Name = item.SeriesName, Value = item.SeriesId });
                }
                return this.Jsonp(list);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取车型异常", e);
                return this.Jsonp(new { code = 500, msg = "获取车型异常" });
            }
        }
        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns></returns>
        public JsonpResult ProvinceCityWS()
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<string> _result = _AppContext.DealerApp.GetProvinceList();
                foreach (var item in _result)
                {
                    list.Add(new { Name = item, Value = string.Empty });
                }
                return this.Jsonp(list);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取省份异常", e);
                return this.Jsonp(new { code = 500, msg = "获取省份异常" });
            }
        }
        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceValue"></param>
        /// <returns></returns>
        public JsonpResult CitysWS(string provinceValue)
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<string> _result = _AppContext.DealerApp.GetCityListByProvince(provinceValue);
                foreach (var item in _result)
                {
                    list.Add(new { Name = item, Value = string.Empty });
                }
                return this.Jsonp(list);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取市异常", e);
                return this.Jsonp(new { code = 500, msg = "获取市异常" });
            }
        }

        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="provinceValue"></param>
        /// <returns></returns>
        public JsonpResult DealersWS(string cityValue, string provinceValue)
        {
            try
            {
                List<object> list = new List<object>();
                IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
                foreach (var item in _dealers)
                {
                    list.Add(new { Name = item.Name, Value = item.DealerId });
                }
                return this.Jsonp(list);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取供应商异常", e);
                return this.Jsonp(new { code = 500, msg = "获取供应商异常" });
            }
        }

        /// <summary>
        /// form表单预约试驾请求------POST
        /// </summary>
        /// <form name="Phone">手机</form>
        /// <form name="UserName">姓名</form>
        /// <form name="CarSeries">车型</form>
        /// <form name="DealerId">经销商Id</form>
        /// <form name="DealerCity">经销商所在城市</form>
        /// <form name="DealerProvince">经销商所在省份</form>
        /// <form name="PurchaseTimeFrame">计划购车时间</form>
        /// <form name="Source">请求来源(如：360、baidu)</form>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoReserveDriveWS(string post)
        {
            string Phone = string.Empty, UserName = string.Empty, CarSeries = string.Empty, DealerId = string.Empty, DealerCity = string.Empty, DealerProvince = string.Empty, PurchaseTimeFrame = string.Empty, Source = string.Empty, callback = string.Empty;
            try
            {
                if (Request.Form == null)
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "错误的请求" });
                }
                var postFormData = Request.Form;

                if (string.IsNullOrEmpty(postFormData["mobile"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "请输入手机号码" });
                }
                else
                {
                    Phone = postFormData["mobile"];
                }
                if (string.IsNullOrEmpty(postFormData["name"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "请输入姓名" });
                }
                else
                {
                    UserName = postFormData["name"];
                }
                if (string.IsNullOrEmpty(postFormData["cartype"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "请输入试驾车型" });
                }
                else
                {
                    CarSeries = postFormData["cartype"];
                }
                if (string.IsNullOrEmpty(postFormData["dealer"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "请选择经销商" });
                }
                else
                {
                    DealerId = postFormData["dealer"];
                }
                if (string.IsNullOrEmpty(postFormData["city"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "经销商所在城市不能为空" });
                }
                else
                {
                    DealerCity = postFormData["city"];
                }
                if (string.IsNullOrEmpty(postFormData["province"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "经销商所在省份不能为空" });
                }
                else
                {
                    DealerProvince = postFormData["province"];
                }
                if (string.IsNullOrEmpty(postFormData["buytime"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "计划购车时间不能为空" });
                }
                else
                {
                    PurchaseTimeFrame = postFormData["buytime"];
                }
                if (string.IsNullOrEmpty(postFormData["source"]))
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "请求来源不能为空" });
                }
                else
                {
                    Source = postFormData["source"];
                }

                var cstestDriveEntity = new TestDriveEntity();
                cstestDriveEntity.Phone = Phone;
                cstestDriveEntity.UserName = UserName;
                cstestDriveEntity.CarSeries = CarSeries;
                cstestDriveEntity.DealerId = DealerId;
                cstestDriveEntity.DealerCity = DealerCity;
                cstestDriveEntity.DealerProvince = DealerProvince;
                cstestDriveEntity.PurchaseTimeFrame = PurchaseTimeFrame;
                cstestDriveEntity.DataSource = "blms_three_" + Source;
                int _result = _AppContext.TestDriveApp.Add(cstestDriveEntity);
                if (_result <= 0)
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 400, msg = "提交失败" });
                }
                else
                {
                    return RedirectToAction("DoReserveDriveWS", new { code = 200, msg = "提交成功" });
                }
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("提交预约试驾异常", e);
                return RedirectToAction("DoReserveDriveWS", new { code = 500, msg = "提交预约试驾异常" });
            }
        }

        [HttpGet]
        public ActionResult DoReserveDriveWS()
        {
            return View();
        }

        /// <summary>
        /// form表单ajax预约试驾请求------Get
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <param name="name">姓名</param>
        /// <param name="cartype">车型</param>
        /// <param name="dealer">经销商Id</param>
        /// <param name="city">经销商所在城市</param>
        /// <param name="province">经销商所在省份</param>
        /// <param name="buytime">计划购车时间</param>
        /// <param name="source">请求来源(如：360、baidu)</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public JsonpResult DoReserveDriveWSGet(string mobile, string name, string cartype, string dealer, string city, string province, string buytime, string source, string callback)
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入手机号码" });
                }
                if (string.IsNullOrEmpty(name))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入姓名" });
                }
                if (string.IsNullOrEmpty(cartype))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入试驾车型" });
                }
                if (string.IsNullOrEmpty(dealer))
                {
                    return this.Jsonp(new { code = 400, msg = "请选择经销商" });
                }
                if (string.IsNullOrEmpty(city))
                {
                    return this.Jsonp(new { code = 400, msg = "经销商所在城市不能为空" });
                }
                if (string.IsNullOrEmpty(province))
                {
                    return this.Jsonp(new { code = 400, msg = "经销商所在省份不能为空" });
                }
                if (string.IsNullOrEmpty(buytime))
                {
                    return this.Jsonp(new { code = 400, msg = "计划购车时间不能为空" });
                }
                if (string.IsNullOrEmpty(source))
                {
                    return this.Jsonp(new { code = 400, msg = "请求来源不能为空" });
                }

                var cstestDriveEntity = new TestDriveEntity();
                cstestDriveEntity.Phone = mobile;
                cstestDriveEntity.UserName = name;
                cstestDriveEntity.CarSeries = cartype;
                cstestDriveEntity.DealerId = dealer;
                cstestDriveEntity.DealerCity = city;
                cstestDriveEntity.DealerProvince = province;
                cstestDriveEntity.PurchaseTimeFrame = buytime;
                cstestDriveEntity.DataSource = "blms_three_" + source;
                int _result = _AppContext.TestDriveApp.Add(cstestDriveEntity);
                if (_result <= 0)
                {
                    return this.Jsonp(new { code = 400, msg = "提交失败" });
                }
                else
                {
                    return this.Jsonp(new { code = 200, msg = "提交成功" });
                }
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("提交预约试驾异常", e);
                return this.Jsonp(new { code = 500, msg = "提交预约试驾异常" });
            }
        }

        #endregion
    }

    #region 对外预约服务接口--辅助类

    public class JsonpResult : JsonResult
    {
        private static readonly string JsonpCallbackName = "callback";
        private static readonly string CallbackApplicationType = "application/json";

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((JsonRequestBehavior == JsonRequestBehavior.DenyGet) &&
                  String.Equals(context.HttpContext.Request.HttpMethod, "GET"))
            {
                throw new InvalidOperationException();
            }
            var response = context.HttpContext.Response;
            if (!String.IsNullOrEmpty(ContentType))
                response.ContentType = ContentType;
            else
                response.ContentType = CallbackApplicationType;

            response.ContentEncoding = System.Text.Encoding.UTF8;

            if (Data != null)
            {
                String buffer;
                var request = context.HttpContext.Request;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (request[JsonpCallbackName] != null)
                    buffer = String.Format("{0}({1})", request[JsonpCallbackName], serializer.Serialize(Data));
                else
                    buffer = serializer.Serialize(Data);
                response.Write(buffer);
            }
        }
    }

    public static class ContollerExtensions
    {
        /// <summary>
        /// 扩展Contoller
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            JsonpResult result = new JsonpResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return result;
        }
    }

    #endregion

}