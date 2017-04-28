using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class CommonController : BaseController
    {      
        /// <summary>
        /// 预约试驾
        /// </summary>
        /// <returns></returns>
        public ActionResult TestDrive()
        {
            //车型
            IEnumerable<CSBaseCar> CarTypeList = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.TestDrive).ToList();
            ViewBag.CarTypeList = CarTypeList;

            //省会
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            ViewBag.Provinces = _provinces;
            ViewBag.IsWeibao = 0;
            ViewBag.Istestserver = 0;
            ViewBag.IsDingChe = 0;

            return View();
        }

        /// <summary>
        /// 预约维保
        /// </summary>
        /// <returns></returns>
        public ActionResult ReservationService()
        {
            //车型
            IEnumerable<CSBaseCar> CarTypeList = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.Maintenance).ToList();
            ViewBag.CarTypeList = CarTypeList;

            //省会
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            ViewBag.Provinces = _provinces;
            return View();
        }

        /// <summary>
        /// 纯正配件查询
        /// </summary>
        /// <returns></returns>
        public ActionResult PartsQuery()
        {
            return View();
        }

        /// <summary>
        /// 经销商查询
        /// </summary>
        /// <returns></returns>
        public ActionResult DealerQuiry()
        {
            //省会
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            ViewBag.Provinces = _provinces;

            return View();
        }

        /// <summary>
        /// 附近经销商
        /// </summary>
        /// <returns></returns>
        public ActionResult DealerNear()
        {
            return View();
        }

        /// <summary>
        /// 消息中心
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageCenter()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Login", "Account",);
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            NoReadMsgModel model = new NoReadMsgModel();
            //model = _AppContext.UserMessageRecordApp.NoReadList(User.Identity.GetUserId());
            model.SysMsgCount = _AppContext.UserMessageRecordApp.GetNoReadMsg(1, User.Identity.GetUserId()).ToList().Count();
            model.IntegralchangeMsgCount = _AppContext.UserMessageRecordApp.GetNoReadMsg(3, User.Identity.GetUserId()).ToList().Count();
            model.CardcouponMsgCount = _AppContext.UserMessageRecordApp.GetNoReadMsg(4, User.Identity.GetUserId()).ToList().Count();
            model.ActAndServerCount = _AppContext.UserMessageRecordApp.GetNoReadMsg(5, User.Identity.GetUserId()).ToList().Count();
            return View(model);
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
        /// 预约试驾
        /// </summary>
        /// <param name="cstestDriveEntity"></param>
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
            cstestDriveEntity.DataSource = "blms_wap";
            int _code = 200;
            int _result = _AppContext.TestDriveApp.Add(cstestDriveEntity);
            if (_result <= 0)
            {
                _code = 400;
            }
            _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, cstestDriveEntity.Phone, new string[] { });

            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 纯正配件查询结果展示
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult PartsResult(string code)
        {
            code = code ?? string.Empty;
            string content = string.Empty;
            string userid = string.Empty;
            bool IsLogin = true;
            if (!User.Identity.IsAuthenticated)
            {
                //return Json(new { code = 400, msg = "请登陆后验证配件" }, JsonRequestBehavior.AllowGet);
                IsLogin = false;
            }
            else
            {
                userid = this.User.Identity.GetUserId();
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var url = ConfigurationManager.AppSettings["BxWeiXinApiUrl"] ?? "http://10.37.93.116:9001";
                    var urlFitt = String.Format(url + "/api/Car/FittingValidate?code={0}&userId={1}&address=&Longitude=0&Latitude=0&Altitude=0", code, userid);
                    #region 日志
                    System.IO.StreamWriter writer = null;
                    System.IO.FileInfo file = new System.IO.FileInfo(string.Format("{0}/{1}", "D:/Log", "/log.txt"));
                    writer = new System.IO.StreamWriter(file.FullName, true);//文件不存在就创建,true表示追加 
                    writer.WriteLine(string.Format("Time:{0}", DateTime.Now));
                    writer.WriteLine(string.Format("url:{0}", urlFitt));
                    writer.Close();
                    #endregion
                    var json = this.GET_WebRequestHTML("utf-8", urlFitt);
                    if (!String.IsNullOrEmpty(json))
                    {
                        writer = new System.IO.StreamWriter(file.FullName, true);//文件不存在就创建,true表示追加 
                        writer.WriteLine(string.Format("json:{0}", json));
                        var fitting = WebUtils.JsonToObj<Fitting>(json, null);
                        if (fitting != null && fitting.Code == "200")
                        {
                            writer.WriteLine(string.Format("fitting:{0}", fitting));
                            content = fitting.Content;
                            writer.WriteLine(string.Format("content:{0}", content));
                            writer.Close();
                        }
                    }
                    IsLogin = true;
                }
            }
            ViewBag.oid = userid;
            ViewBag.code = code;
            ViewBag.content = content;
            ViewBag.IsLogin = IsLogin;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodingName">编号格式：utf-8</param>
        /// <param name="htmlUrl">请求Url</param>
        /// <returns></returns>
        private string GET_WebRequestHTML(string encodingName, string htmlUrl)
        {
            string html = string.Empty;
            #region 日志
            System.IO.StreamWriter writerhtml = null;
            System.IO.FileInfo file = new System.IO.FileInfo(string.Format("{0}/{1}", "D:/Log", "/log"+DateTime.Now.ToString("yyyyMMddHHmmssffffff")+".txt"));
            writerhtml = new System.IO.StreamWriter(file.FullName, true);//文件不存在就创建,true表示追加 
            writerhtml.WriteLine(string.Format("Time:{0}", DateTime.Now));
            #endregion
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingName);

                WebRequest webRequest = WebRequest.Create(htmlUrl);
                webRequest.Headers.Add("appkey", "blms_wap");
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);

                html = streamReader.ReadToEnd();
                writerhtml.WriteLine(string.Format("html:{0}", html));
                writerhtml.Close();

                httpWebResponse.Close();
                responseStream.Close();
                streamReader.Close();
            }
            catch (WebException ex)
            {
                #region 日志
                writerhtml.WriteLine(string.Format("ExceptionMessage:{0}", ex.Message));
                writerhtml.Close();
                #endregion
                throw new Exception(ex.Message);              
            }

            return html;
        }

        /// <summary>
        /// 预约维保
        /// </summary>
        /// <param name="csmaintenanceEntity"></param>
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

            csmaintenanceEntity.DataSource = "blms_wap";
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

            return Json(new { code = _code }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 纯正配件查询json对象转换
        /// </summary>
        public class Fitting
        {
            public string Code { get; set; }

            public string Message { get; set; }

            public string Content { get; set; }
        }
    }
}