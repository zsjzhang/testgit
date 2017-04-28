using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.WebApi.Filter;
    using Vcyber.BLMS.WebApi.ServiceReference1;

    /// <summary>
    /// 爱车
    /// </summary>

    public class CarController : ApiController
    {
        /// <summary>
        /// 获取用户爱车信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns>用户爱车列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<Car>))]
        [Route("api/Car/{id}")]
        [IOVAuthorize]
        private IHttpActionResult GetCarListByUser(string id)
        {
            var list = _AppContext.CarServiceUserApp.SelectCarListByUserId(id).ToList<Car>();

            return Ok(list);
        }

        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("api/Car/IsExist/{vin}")]
        public IHttpActionResult IsExistByVin(string vin)
        {
            return this.Ok(new ReturnObject(_AppContext.CarServiceUserApp.GetCarInfoByVIN(vin) == null ? false : true));
        }

        [HttpGet]
        [Route("api/Car/CarInfo/{vin}")]
        public IHttpActionResult CarInfo(string vin)
        {
            IFCustomer customer = null;
            var car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out customer);

            return this.Ok(new ReturnObject(new { car = car, customer = customer }));
        }
        /// <summary>
        /// 根据用户id查询预约试驾订单
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        
        [HttpGet]
        [Route("api/Car/GetByTestListUserId")]
        [ResponseType(typeof(CSTestDrive))]
        public IHttpActionResult GetByTestListUserId(string userid)
        {
            var entity = _AppContext.CarServiceUserApp.GetCSTestDrive(userid);
            return this.Ok(new ReturnObject(entity));
        }

        /// <summary>
        /// 根据用户手机号查询预约试驾订单
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Car/GetByTestListPhone")]
        [ResponseType(typeof(CSTestDrive))]
        public IHttpActionResult GetByTestListPhone(string Phone)
        {
            var entity = _AppContext.CarServiceUserApp.GetCSTestDriveByPhone(Phone);
            return this.Ok(new ReturnObject(entity));
        }

        /// <summary>
        /// 根据预约试驾订单查看订单详情
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Car/GetCSTestDriveDetial")]
        [ResponseType(typeof(CSTestDrive))]
        public IHttpActionResult GetCSTestDriveDetial(string OrderNo)
        {
            var entity = _AppContext.CarServiceUserApp.GetCSTestDriveDetial(OrderNo);
            return this.Ok(new ReturnObject(entity));
        }

        /// <summary>
        /// 根据用户id查看预约维保信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Car/GetCSMaintenance")]
        [ResponseType(typeof(CSMaintenance))]
        public IHttpActionResult GetCSMaintenance(string userid)
        {
            var entity = _AppContext.CarServiceUserApp.GetCSMaintenance(userid);
            return this.Ok(new ReturnObject(entity));
        }

        /// <summary>
        /// 根据预约维保id查看详情
        /// </summary>
        /// <param name="userid">预约维保ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Car/GetCSMaintenanceDetial")]
        [ResponseType(typeof(CSMaintenance))]
        public IHttpActionResult GetCSMaintenanceDetial(string OrderNo)
        {
            var entity = _AppContext.CarServiceUserApp.GetCSMaintenanceDetial(OrderNo);
            return this.Ok(new ReturnObject(entity));
        }


        /// <summary>
        /// 修改车牌号
        /// </summary>
        /// <param name="userid">vin</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Car/UpdateCarInfo2")]
        //[ResponseType(typeof(CSMaintenance))]
        public IHttpActionResult UpdateCarInfo2(string vin, string LicencePlate, string Mileage)
        {
            if(string.IsNullOrEmpty(vin))
            {
                return BadRequest("VIN不能为空。");
            }
            var car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin);
            if (car==null)
            {
                return BadRequest("不存在此车辆信息！");
            }
            var entity = _AppContext.CarServiceUserApp.UpdateCarInfo2( vin,  LicencePlate,  Mileage);
            return this.Ok(true);
          
        }
        /// <summary>
        /// 配件验证
        /// </summary>
        /// <param name="code">防伪码</param>
        /// <param name="userId">用户ID</param>
        /// <param name="address">地址</param>
        /// <param name="Longitude">经度</param>
        /// <param name="Latitude">纬度</param>
        /// <param name="Altitude">海拔</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ReturnObject))]
        [Route("api/Car/FittingValidate")]
        public IHttpActionResult FittingValidate(string code, string userId, string address = null, float Longitude = 0, float Latitude = 0, float Altitude = 0)
        {

            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("防伪码不能为空");
            }
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("用户ID不能为空");
            }

                //获取请求渠道
                IEnumerable<string> appkeys = null;
                string appkey = string.Empty;

                if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                {
                appkey = appkeys.First();
                }
                try
                {
                    WebService1SoapClient PeijianClient = new WebService1SoapClient();
                    var returnData = PeijianClient.CheckQRForWebNew(code, address, Longitude, Latitude, Altitude, userId, appkey);
                    FittingValidate FittingValidateEntity = new FittingValidate();
                    FittingValidateEntity.Code = code;
                    FittingValidateEntity.Ctype = appkey == string.Empty ? FittingValidateEntity.Ctype : appkey;
                    FittingValidateEntity.Latitude = Latitude;
                    FittingValidateEntity.Longitude = Longitude;
                    FittingValidateEntity.UserAddress = address;
                    FittingValidateEntity.Altitude = Altitude;
                    FittingValidateEntity.Result = returnData;
                    FittingValidateEntity.Userid = userId;
                    _AppContext.FittingValidateApp.Add(FittingValidateEntity);
                    return this.Ok(new ReturnObject("200",returnData));
                }
                catch  (Exception ex)
                {
                    return this.Ok(new ReturnObject("503", ex.Message, ex.StackTrace.ToString()));
                }

        }
        /// <summary>
        /// 返回车辆类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ReturnObject))]
        [Route("api/Car/CarTypes")]
        public IHttpActionResult CarTypes()
        {
            var result = new ReturnObject("200", null);
            try
            {
                IEnumerable<CSBaseCar> query = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
                result.Content = query;
            }
            catch (Exception ex)
            {
                result.Code = "500";
                result.Message = ex.Message;
            }
            return this.Ok(result);
        }             
    }
}
