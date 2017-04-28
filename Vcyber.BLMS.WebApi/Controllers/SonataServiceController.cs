using System.Web.Http;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Http.Description;

    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;

    using PetaPoco;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.WebApi.Filter;
    using Vcyber.BLMS.WebApi.Common;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Repository;

    /// <summary>
    /// 索九服务
    /// </summary>
    public class SonataServiceController : ApiController
    {

        /// <summary>
        /// 预约维修保养请求
        /// </summary>
        /// <returns></returns>
        [Route("api/SonataService/DoReserveMaintenance")]
        [HttpPost]
        [ResponseType(typeof(int))]
        public IHttpActionResult DoReserveMaintenance(SonataServiceEntity csmaintenanceEntity)
        {
            //string userId = string.Empty;
            //string userName = string.Empty;
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    userId = User.Identity.GetUserId();
            //}

            //csmaintenanceEntity.UserId = userId;
            if (string.IsNullOrEmpty(csmaintenanceEntity.DataSource))
            {
                csmaintenanceEntity.DataSource = "blms";
            }
            csmaintenanceEntity.OrderType = EBMServiceType.CommonMaintain;
            int _code = 200;
            int _result = _AppContext.SonataServiceApp.Add(csmaintenanceEntity);
            if (_result <= 0)
            {
                _code = 400;
            }

            //if (User.Identity.IsAuthenticated)
            //{
            //    int outValue;
            //    var account =
            //        new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId()).Result;
            //    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.预约保养, account.Id, (MemshipLevel)account.MLevel,
            //    //    out outValue);
            //    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.预约保养, account.Id, out outValue);

            //}
            return Json(new { code = _code });
        }




        /// <summary>
        /// 新增索九服务订单（CRM-百日关怀服务，三年九次免检服务，上门取送车服务，专属预约维保服务（一对一）， 普通预约维保服务）
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        /// <remarks>请注意标注服务类型</remarks>
        [Route("api/SonataService/add")]
        [HttpPost]
        [ResponseType(typeof(int))]
        public IHttpActionResult Add1(SonataServiceEntity entity)
        {
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.UserId).Result;
            //if (account == null || string.IsNullOrEmpty(account.IdentityNumber) ||
            //    (string.IsNullOrEmpty(account.No) && entity.OrderType != EBMServiceType.CommonMaintain))
            //{
            //    return this.Ok(new ReturnObject("user-513"));
            //}

            if ((account == null && entity.OrderType != EBMServiceType.CommonMaintain) || (account != null && string.IsNullOrEmpty(account.No) && entity.OrderType != EBMServiceType.CommonMaintain))
            {
                return this.Ok(new ReturnObject("user-513"));
            }

            if (ModelState.IsValid)
            {
                //获取请求渠道
                IEnumerable<string> appkeys = null;
                string appkey = string.Empty;

                if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                {
                    appkey = appkeys.First();
                }

                entity.DataSource = appkey == string.Empty ? "blms" : appkey;
                //entity.DataSource = this.GetSysCode();

                if (account != null)
                {
                    LogService.Instance.Info(string.Format("{0};{1}", account.IdentityNumber, entity.OrderType));
                }


                if (entity.OrderType == EBMServiceType.LongDistanceTravel)
                {
                    //通过身份证查找剩余服务次数
                    var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(account.IdentityNumber, EDMSServiceType.LongDistanceTravel);
                    LogService.Instance.Info(string.Format("{0};{1};{2}", account.IdentityNumber, count, entity.OrderType));
                    if (count >= 2)
                    {
                        return this.Ok(new ReturnObject("412", "服务次数已用完", ""));
                    }
                }

                int retval = _AppContext.SonataServiceApp.Add(entity);
                if (retval > 0)
                {
                    EBRuleType ruleType = entity.ServiceType == EServiceType.Maintenance ? EBRuleType.预约保养 : EBRuleType.预约维修;
                    EEmpiricRule empircRult = entity.ServiceType == EServiceType.Maintenance ? EEmpiricRule.预约保养 : EEmpiricRule.预约维修;

                    //if (account != null)
                    //{
                    //    int outValue;
                    //    _AppContext.BreadApp.BlueBeanBread(
                    //        ruleType,
                    //        entity.UserId,
                    //        (MemshipLevel)account.MLevel,
                    //        out outValue);
                    //    _AppContext.BreadApp.EmpiricBread(empircRult, entity.UserId, out outValue);
                    //}
                    return this.Ok(new ReturnObject((object)retval));
                }
            }
            else
            {
                return this.Ok(new ReturnObject("412", ModelState.GetErrors()));
            }
            return this.Ok(new ReturnObject(201));
        }



        /// <summary>
        /// 新增索九服务订单（CRM-百日关怀服务，三年九次免检服务，上门取送车服务，专属预约维保服务（一对一）， 普通预约维保服务）
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        /// <remarks>请注意标注服务类型</remarks>
        [Route("api/SonataService")]
        [HttpPost]
        [ResponseType(typeof(int))]
        public IHttpActionResult Add(SonataServiceEntity entity)
        {
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.UserId).Result;
            //if (account == null || string.IsNullOrEmpty(account.IdentityNumber) ||
            //    (string.IsNullOrEmpty(account.No) && entity.OrderType != EBMServiceType.CommonMaintain))
            //{
            //    return this.Ok(new ReturnObject("user-513"));
            //}

            if ((account == null && entity.OrderType != EBMServiceType.CommonMaintain) || (account != null && string.IsNullOrEmpty(account.No) && entity.OrderType != EBMServiceType.CommonMaintain))
            {
                return this.Ok(new ReturnObject("user-513"));
            }

            if (ModelState.IsValid)
            {
                //获取请求渠道
                IEnumerable<string> appkeys = null;
                string appkey = string.Empty;

                if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                {
                    appkey = appkeys.First();
                }

                entity.DataSource = appkey == string.Empty ? "blms" : appkey;
                //entity.DataSource = this.GetSysCode();

                if (account != null)
                {
                    LogService.Instance.Info(string.Format("{0};{1}", account.IdentityNumber, entity.OrderType));
                }


                if (entity.OrderType == EBMServiceType.LongDistanceTravel)
                {
                    //通过身份证查找剩余服务次数
                    var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(account.IdentityNumber, EDMSServiceType.LongDistanceTravel);
                    LogService.Instance.Info(string.Format("{0};{1};{2}", account.IdentityNumber, count, entity.OrderType));
                    if (count >= 2)
                    {
                        return this.Ok(new ReturnObject("412", "服务次数已用完", ""));
                    }
                }
                if (entity.OrderType == EBMServiceType.Home2Home)
                {
                    //通过用户Id查找剩余服务次数
                    var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(account.Id, EDMSServiceType.Home2Home);
                    LogService.Instance.Info(string.Format("{0};{1};{2}", account.Id, count, entity.OrderType));
                    if (count <= 0)
                    {
                        return this.Ok(new ReturnObject("412", "服务次数已用完", ""));
                    }
                }
                //如果是维保，是询是否预约过
                if(!string.IsNullOrEmpty(entity.LicensePlate))
                { 
                    QueryParamEntity par = new QueryParamEntity();
                    par.ScheduleFromDate = entity.ScheduleDate;
                    par.ScheduleToDate = entity.ScheduleDate;
                    par.LicensePlate = entity.LicensePlate;
                    var list = _AppContext.SonataServiceApp.QueryOrders(par, 1, 100);
                    if (list.Items.Count > 0)
                    {
                        return this.Ok(new ReturnObject("412", "您已经预约过了！", ""));
                    }
                }
                int retval = _AppContext.SonataServiceApp.Add(entity);
                if (retval > 0)
                {
                    EBRuleType ruleType = entity.ServiceType == EServiceType.Maintenance ? EBRuleType.预约保养 : EBRuleType.预约维修;                    
                    EEmpiricRule empircRult = entity.ServiceType == EServiceType.Maintenance ? EEmpiricRule.预约保养 : EEmpiricRule.预约维修;
                    //预约成功后发送短信
                    _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, entity.Phone, new string[] { "维保", entity.DealerName });                
                    //if (account != null)
                    //{
                    //    int outValue;
                    //    _AppContext.BreadApp.BlueBeanBread(
                    //        ruleType,
                    //        entity.UserId,
                    //        (MemshipLevel)account.MLevel,
                    //        out outValue);
                    //    _AppContext.BreadApp.EmpiricBread(empircRult, entity.UserId, out outValue);
                    //}
                    return this.Ok(new ReturnObject((object)retval));
                }
            }
            else
            {
                return this.Ok(new ReturnObject("412", ModelState.GetErrors()));
            }
            return this.Ok(new ReturnObject(201));
        }

        /// <summary>
        /// 根据用户Id查询索九服务订单
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="type">服务类型</param>
        /// <param name="page">当前页数</param>
        /// <param name="itemsPerPage">每页数据条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/SonataService/User/")]
        [ResponseType(typeof(Page<CSSonataService>))]
        //[IOVAuthorize]
        public IHttpActionResult GetByUserId(string userId, EBMServiceType type, long page = 1, long itemsPerPage = 20)
        {
            //string userId = HttpContext.Current.User.Identity.Name;
            Page<CSSonataService> list = _AppContext.SonataServiceApp.QueryOrdersByUserId(userId, type, page, itemsPerPage);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据索九服务订单ID，查询详情
        /// </summary>
        /// <param name="id">索九服务订单ID</param>
        /// <returns>订单详情</returns>
        [Route("api/SonataService/User/{id}")]
        [HttpGet]
        [ResponseType(typeof(CSSonataService))]
        //[IOVAuthorize]
        public IHttpActionResult GetByUserId(int id)
        {
            var entity = _AppContext.SonataServiceApp.GetEntityById(id);
            return this.Ok(new ReturnObject(entity));
        }

        /// <summary>
        /// 根据订单Id（不是单号）获取订单详情
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns>订单详情</returns>
        [HttpGet]
        [Route("api/SonataService/{id}")]
        [ResponseType(typeof(CSSonataService))]
        // [IOVAuthorize]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(new ReturnObject(_AppContext.SonataServiceApp.GetEntityById(id)));
        }

        /// <summary>
        /// 判断用户是否是索九车主
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <returns>判断结果</returns>
        [HttpGet]
        [Route("api/SonataService/SonataUser/{identity}")]
        [ResponseType(typeof(bool))]
        [IOVAuthorize]
        public IHttpActionResult IsSonataUser(string identity)
        {
            return this.Ok(new ReturnObject(_AppContext.SonataServiceApp.IsSonataUser(identity)));
        }

        /// <summary>
        /// 判断用户是否是Tlc车主
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <returns>判断结果</returns>
        [HttpGet]
        [Route("api/SonataService/TlcUser/{identity}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsTlcUser(string identity)
        {
            return this.Ok(new ReturnObject(_AppContext.SonataServiceApp.IsTlcUser(identity)));
        }

        /// <summary>
        /// 查询剩余服务的次数
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <param name="type">服务类型</param>
        /// <returns>剩余数量</returns>
        [HttpGet]
        [Route("api/SonataService/Count/{identity}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetById(string identity, EDMSServiceType type)
        {
            return this.Ok(new ReturnObject(_AppContext.SonataServiceApp.GetServiceCount(identity, type)));
        }

        /// <summary>
        /// 查询所有服务的次数
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <returns>各项服务剩余数量及加总, 以字典的格返回, 结果中的key参考 [/Help/ResourceModel?modelName=EDMSServiceType] 其中 -1 表示 All</returns>
        [HttpGet]
        [Route("api/SonataService/AllCount/{identity}")]
        [ResponseType(typeof(Dictionary<int, int>))]
        [IOVAuthorize]
        public IHttpActionResult GetById(string identity)
        {
            return this.Ok(new ReturnObject(_AppContext.SonataServiceApp.GetServiceCount(identity)));
        }

        [HttpPost]
        [Route("api/SonataService/FreeRoadRescue")]
        public IHttpActionResult FreeRoadRescue(CS_FreeRoadRescue entity)
        {
            var result = new ReturnResult { IsSuccess = true };

            if (entity == null)
            {
                result.IsSuccess = false;
                return Ok(result);
            }

            var isSuccess = _AppContext.FreeRoadRescueApp.AddFreeRoadRescue(entity);

            if (!isSuccess)
            {
                result.IsSuccess = false;
                return Ok(result);
            }

            return Ok(result);
        }




        /// <summary>
        /// 根据用户Id查询预约维保
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="type">服务类型</param>
        /// <param name="page">当前页数</param>
        /// <param name="itemsPerPage">每页数据条数</param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("GetMaintenanceyById")]
        //[ResponseType(typeof(Page<CSMaintenance>))]
        ////[IOVAuthorize]
        //public IHttpActionResult GetMaintenanceyById(string userId, EBMServiceType type, long page = 1, long itemsPerPage = 20)
        //{
        //    //string userId = HttpContext.Current.User.Identity.Name;
        //    Page<CSSonataService> list = _AppContext.SonataServiceApp.QueryOrdersByUserId(userId, type, page, itemsPerPage);
        //    return this.Ok(new ReturnObject(list));
        //}
    }
}
