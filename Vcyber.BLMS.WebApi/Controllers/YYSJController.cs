using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 预约试驾服务接口
    /// </summary>
    public class YYSJController : ApiController
    {
        #region 对外预约服务接口
        private static readonly string CallbackApplicationType = "application/json";

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TestDriveY/CarTypeWS")]
        public HttpResponseMessage CarTypeWS(string callback)
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
                foreach (var item in _result)
                {
                    list.Add(new { Name = item.SeriesName, Value = item.SeriesId.ToString() });
                }

                return this.Jsonp(list, callback);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取车型异常", e);
                return this.Jsonp(new { code = 500, msg = "获取车型异常" }, callback);
            }
        }

        /// <summary>
        /// 获取省
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TestDriveY/ProvinceCityWS")]
        public HttpResponseMessage ProvinceCityWS(string callback)
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<string> _result = _AppContext.DealerApp.GetProvinceList();
                foreach (var item in _result)
                {
                    list.Add(new { Name = item, Value = string.Empty });
                }
                return this.Jsonp(list, callback);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取省份异常", e);
                return this.Jsonp(new { code = 500, msg = "获取省份异常" }, callback);
            }
        }

        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceValue"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TestDriveY/CitysWS")]
        public HttpResponseMessage CitysWS(string provinceValue, string callback)
        {
            try
            {
                IList<object> list = new List<object>();
                IEnumerable<string> _result = _AppContext.DealerApp.GetCityListByProvince(provinceValue);
                foreach (var item in _result)
                {
                    list.Add(new { Name = item, Value = string.Empty });
                }
                return this.Jsonp(list, callback);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取市异常", e);
                return this.Jsonp(new { code = 500, msg = "获取市异常" }, callback);
            }
        }

        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="cityValue"></param>
        /// <param name="provinceValue"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TestDriveY/DealersWS")]
        public HttpResponseMessage DealersWS(string cityValue, string provinceValue, string callback)
        {
            try
            {
                List<object> list = new List<object>();
                IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
                foreach (var item in _dealers)
                {
                    list.Add(new { Name = item.Name, Value = item.DealerId });
                }
                return this.Jsonp(list, callback);
            }
            catch (Exception e)
            {
                Vcyber.BLMS.Common.LogService.Instance.Error("获取供应商异常", e);
                return this.Jsonp(new { code = 500, msg = "获取供应商异常" }, callback);
            }
        }

        /// <summary>
        /// form表单提交预约试驾请求
        /// </summary>
        /// <form name="Phone">手机</form>
        /// <form name="UserName">姓名</form>
        /// <form name="CarSeries">车型</form>
        /// <form name="DealerId">经销商Id</form>
        /// <form name="DealerCity">经销商所在城市</form>
        /// <form name="DealerProvince">经销商所在省份</form>
        /// <form name="PurchaseTimeFrame">计划购车时间</form>
        /// <form name="Source">请求来源(如：360、baidu)</form>
        /// <form name="callback">回调函数</form>
        /// <returns></returns>
        [HttpPost]
        [Route("api/TestDriveY/DoReserveDriveWS")]
        public HttpResponseMessage DoReserveDriveWS()
        {
            string Phone = string.Empty, UserName = string.Empty, CarSeries = string.Empty, DealerId = string.Empty, DealerCity = string.Empty, DealerProvince = string.Empty, PurchaseTimeFrame = string.Empty, Source = string.Empty, callback = string.Empty;
            try
            {
                if(HttpContext.Current==null||HttpContext.Current.Request==null||HttpContext.Current.Request.Form==null)
                {
                    return this.Jsonp(new { code = 400, msg = "错误的请求" });
                }
                var postFormData = HttpContext.Current.Request.Form;

                if (string.IsNullOrEmpty(postFormData["mobile"]))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入手机号码" });
                }
                else
                {
                    Phone = postFormData["mobile"];
                }
                if (string.IsNullOrEmpty(postFormData["name"]))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入姓名" });
                }
                else
                {
                    UserName = postFormData["name"];
                }
                if (string.IsNullOrEmpty(postFormData["cartype"]))
                {
                    return this.Jsonp(new { code = 400, msg = "请输入试驾车型" });
                }
                else
                {
                    CarSeries = postFormData["cartype"];
                }
                if (string.IsNullOrEmpty(postFormData["dealer"]))
                {
                    return this.Jsonp(new { code = 400, msg = "请选择经销商" });
                }
                else
                {
                    DealerId = postFormData["dealer"];
                }
                if (string.IsNullOrEmpty(postFormData["city"]))
                {
                    return this.Jsonp(new { code = 400, msg = "经销商所在城市不能为空" });
                }
                else
                {
                    DealerCity = postFormData["city"];
                }
                if (string.IsNullOrEmpty(postFormData["province"]))
                {
                    return this.Jsonp(new { code = 400, msg = "经销商所在省份不能为空" });
                }
                else
                {
                    DealerProvince = postFormData["province"];
                }
                if (string.IsNullOrEmpty(postFormData["buytime"]))
                {
                    return this.Jsonp(new { code = 400, msg = "计划购车时间不能为空" });
                }
                else
                {
                    PurchaseTimeFrame = postFormData["buytime"];
                }
                if (string.IsNullOrEmpty(postFormData["source"]))
                {
                    return this.Jsonp(new { code = 400, msg = "请求来源不能为空" });
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

        private HttpResponseMessage Jsonp(object data)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return new HttpResponseMessage { Content = new StringContent(String.Format("{0}", serializer.Serialize(data)), System.Text.Encoding.UTF8, CallbackApplicationType) };
        }

        private HttpResponseMessage Jsonp(object data, string callbackFun)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return new HttpResponseMessage { Content = new StringContent(String.Format("{0}({1})", callbackFun, serializer.Serialize(data)), System.Text.Encoding.UTF8, CallbackApplicationType) };
        }

        #endregion
    }

}
