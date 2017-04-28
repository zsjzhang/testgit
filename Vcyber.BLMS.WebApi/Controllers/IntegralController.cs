using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Application;

using System.Web.Http.Description;
using System.Web.Script.Serialization;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models;
using Vcyber.BLMS.WebApi.Models.RequestData;
using Vcyber.BLMS.WebApi.Models.ResponseData;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Filter;

    /// <summary>
    /// 用户积分
    /// </summary>
    //[IOVAuthorize]
    public class IntegralController : ApiController
    {
        #region ==== 构造函数 ====

        public IntegralController()
        {
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取会员总积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/integral/totalvalue")]
        [ResponseType(typeof(int))]
        public IHttpActionResult TotalValue(string userId)
        {
            IntegralValueV data = new IntegralValueV();

            if (!string.IsNullOrEmpty(userId))
            {
                data.TotalValue = _AppContext.UserIntegralApp.GetTotalIntegral(userId);
                return this.Ok(new ReturnObject("200", data.TotalValue));
            }
            return this.Ok(new ReturnObject(412)); //参数错误
            //return this.DataResult<IntegralValueV>(data, HttpStatusCode.OK);
        }

        /// <summary>
        /// 获取用户经验记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页个数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/integral/datalist")]
        [ResponseType(typeof(Page<UserIntegralV>))]
        public IHttpActionResult DataList(string userId, int index = 1, int size = 10)
        {
            int total = 0;
            UserIntegralCollectionV dataResult = new UserIntegralCollectionV() { Datas = new List<UserIntegralV>(10) };
            var datas = _AppContext.UserIntegralApp.GetAll(userId, new PageData() { Index = index, Size = size },
                out total);

            if (datas != null && datas.Count() > 0)
            {
                foreach (var item in datas)
                {
                    dataResult.Datas.Add(new UserIntegralV()
                    {
                        CreateTime = item.CreateTime.ToString(),
                        datastate = item.datastate,
                        integralSource = item.integralSource,
                        remark = item.remark,
                        userId = item.userId,
                        value = item.value
                    });
                }
            }

            Page<UserIntegralV> returnList = new Page<UserIntegralV>();
            returnList.CurrentPage = index;
            returnList.ItemsPerPage = 10;
            //var enumerable = list as IList<News> ?? list.ToList();
            returnList.Items = dataResult.Datas;
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        #endregion


        #region  DMS-BM 蓝缤会员积分明细



        private DateTime GetstringToDateTime(string tempTime)
        {
            if (string.IsNullOrEmpty(tempTime))
            {
                return DateTime.MaxValue;
            }
            IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
            string TarStr = "yyyyMMddHHmmss";  //注意这里用到HH
            DateTime MyDate = DateTime.ParseExact(tempTime, TarStr, format);
            return MyDate;
            
        }
        /// <summary>
        /// 蓝缤会员积分明细
        /// </summary>
        /// <param name="model">蓝缤会员积分明细传参实体</param>
        /// <returns>返回蓝缤会员积分明细</returns>
        [HttpPost]
        [Route("api/integral/GetAddORConsumeintegralList")]
        [ResponseType(typeof(ResUserintegral))]
        [IOVAuthorize]
        public IHttpActionResult GetAddORConsumeintegralList(RequestUserintegral model)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(model);
            BLMS.Common.LogService.Instance.Info("调用GetAddORConsumeintegralList接口" + string.Format("蓝缤会员积分明细，方法：GetAddORConsumeintegralList 传入参数：{0}||时间：{1}", jsonData,  DateTime.Now.ToString()));

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new List<ResUserintegral>() };
            try
            {
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "证件号码不能为空";
                    return Ok(result);
                }
                #region 2016-12-08 update by wangchunrong
                //if (string.IsNullOrEmpty(model.BeginDate))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "开始日期不能为空 ";
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.EndDate))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "结束日期不能为空 ";
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.DealerId))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "经销商ID不能为空 ";
                //    return Ok(result);
                //}
                #endregion
                if (string.IsNullOrEmpty(model.BlueMembership_No))
                {
                    result.IsSuccess = false;
                    result.Message = "会员卡号不能为空 ";
                    return Ok(result);
                }

                //获取会员信息；
                string userId = "";
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的会员信息 ";
                    return Ok(result);
                }
                //如果有传经销商ID这个参数则执行以下代码
                if (!string.IsNullOrEmpty(model.DealerId))
                {
                    var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                    if (dealer == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到经销商信息 ";
                        return Ok(result);
                    }
                }
                if (cusObj.Result.No != model.BlueMembership_No)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该会员卡号下的会员信息";
                    return Ok(result);
                }
                userId = cusObj.Result.Id;

                #region  旧逻辑
                ////获取新增积分列表
                //var tempList = _AppContext.UserIntegralApp.GetUserIntegralList(userId, GetstringToDateTime(model.BeginDate),
                //     GetstringToDateTime(model.EndDate));
                ////获取消费积分列表
                //var consumeList = _AppContext.ConsumeApp.GetUserConsumeList(userId, GetstringToDateTime(model.BeginDate),
                //  GetstringToDateTime(model.EndDate));
                ////构建会员积分新增，消费列表信息；
                //List<ResUserintegral> list = new List<ResUserintegral>();
                //foreach (var m in tempList)
                //{
                //    list.Add(new ResUserintegral()
                //    {
                //        DealerId = "",
                //        Operator = "",
                //        Point = m.value.ToString(),
                //        Remark = m.remark,
                //        Type = "1",
                //        UpdateDate = m.CreateTime.ToString("yyyyMMddHHmmss")

                //    });
                //}
                //foreach (var c in consumeList)
                //{
                //    list.Add(new ResUserintegral()
                //    {
                //        DealerId = c.DealerId,
                //        Operator = c.CreateName,
                //        Point = c.RewardPoints.ToString(),
                //        Remark = string.IsNullOrEmpty(c.Comment) ? "" : c.Comment,
                //        Type = "2",
                //        UpdateDate = c.CreateTime.ToString("yyyyMMddHHmmss")
                //    });
                //}
                #endregion

                //查询积分明细记录
                List<ResUserintegral> list = new List<ResUserintegral>();
                var detail = _AppContext.ConsumeApp.GetUserIntegraldetail(userId, GetstringToDateTime(model.BeginDate),
                  GetstringToDateTime(model.EndDate));
                foreach (var item in detail)
                {
                    string dealerid = string.Empty;
                    if (item.Remark == "一键入会返积分" || item.Remark == "经销商入会返积分")
                    {
                        // 查找一键入会返积分/经销商入会返积分的经销商ID
                        dealerid = _AppContext.ConsumeApp.GetDeaByUId(userId);
                    }
                    else if (item.Remark == "缴费返积分")
                    {
                        //查找缴费返积分的经销商ID
                        dealerid = _AppContext.ConsumeApp.GetDIdByUId(userId);
                    }
                    else
                    {
                        dealerid = _AppContext.ConsumeApp.GetDealerIdByname(item.DealerId);
                    }
                    list.Add(new ResUserintegral()
                    {
                        DealerId = dealerid,
                        Operator = "",
                        Point = item.Point,
                        Type = item.Type,
                        Remark = item.Remark,
                        UpdateDate =Convert.ToDateTime(item.UpdateDate).ToString("yyyyMMddHHmmss")
                    });
            }
               
                result.Data = list;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region APP签到送积分
        /// <summary>
        /// APP签到送积分
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="values">积分值</param>
        /// <param name="activityname">活动名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/integral/SignSendIntegral")]
        //[IOVAuthorize]
        public IHttpActionResult SignSendIntegral(string userid,int values,string activityname)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(userid);
            BLMS.Common.LogService.Instance.Info("调用SignSendIntegral接口" + string.Format("APP签到送积分，方法：SignSendIntegral 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    result.IsSuccess = false;
                    result.Message = "用户ID不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(Convert.ToString(values)))
                {
                    result.IsSuccess = false;
                    result.Message = "积分值不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(activityname))
                {
                    result.IsSuccess = false;
                    result.Message = "活动名称不能为空";
                    return Ok(result);
                }

                _AppContext.UserIntegralApp.Add(new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0, //这里逻辑默认0
                    UpdateTime = DateTime.Now,
                    userId = userid,
                    value = values, //
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    integralSource = "80",
                    remark = "APP签到送积分"
                });

                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = userid,
                    integralSource = "80",
                    value =  values,
                    datastate = 0,
                    ProductName = activityname,
                    remark = "APP签到送积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });

                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 车主认证活动下发1000积分
        /// <summary>
        /// 车主认证活动下发1000积分
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="values">积分值</param>
        /// <param name="activityname">活动名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/integral/AuthOwnerSendIntegral")]
        //[IOVAuthorize]
        public IHttpActionResult AuthOwnerSendIntegral(string userid)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(userid);
            BLMS.Common.LogService.Instance.Info("调用AuthOwnerSendIntegral接口" + string.Format("车主认证活动下发1000积分，方法：AuthOwnerSendIntegral 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    result.IsSuccess = false;
                    result.Message = "用户ID不能为空";
                    return Ok(result);
                }
             
                _AppContext.UserIntegralApp.Add(new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0, //这里逻辑默认0
                    UpdateTime = DateTime.Now,
                    userId = userid,
                    value = 1000, //
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    integralSource = "28",
                    remark = "车主认证活动下发积分"
                });

                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = userid,
                    integralSource = "28",
                    value = 1000,
                    datastate = 0,
                    ProductName = "车主认证活动下发积分",
                    remark = "车主认证活动下发积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
                UserMessageRecord userMessageRecord = new UserMessageRecord();
                userMessageRecord.UserId = userid;
                userMessageRecord.MsgType = MessageType.IntegralConsum;
                userMessageRecord.MsgContent = string.Format("恭喜您，在{0}，在车主认证活动中认证成功，获得1000积分。积分可用于到店维保抵扣现金或在线商城购买商品。",
                    DateTime.Now, 1000);
                _AppContext.UserMessageRecordApp.Insert(userMessageRecord);
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 灵狐新增积分
        /// <summary>
        /// 灵狐活动新增积分
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="values">积分值</param>
        /// <param name="activityname">活动名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/integral/AddIntegralForLH")]
        //[IOVAuthorize]
        public IHttpActionResult AddIntegralForLH(string userid, int values, string activityname)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(userid);
            BLMS.Common.LogService.Instance.Info("调用AddIntegralForLH接口" + string.Format("灵狐新增积分，方法：AddIntegralForLH 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    result.IsSuccess = false;
                    result.Message = "用户ID不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(Convert.ToString(values)))
                {
                    result.IsSuccess = false;
                    result.Message = "积分值不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(activityname))
                {
                    result.IsSuccess = false;
                    result.Message = "活动名称不能为空";
                    return Ok(result);
                }

                _AppContext.UserIntegralApp.Add(new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0, //这里逻辑默认0
                    UpdateTime = DateTime.Now,
                    userId = userid,
                    value = values, //
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    integralSource = "130",
                    remark = "灵狐活动新增积分"
                });

                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = userid,
                    integralSource = "130",
                    value = values,
                    datastate = 0,
                    ProductName = activityname,
                    remark = "灵狐活动新增积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
                
                //个人中心推送积分变动消息
                UserMessageRecord userMessageRecord = new UserMessageRecord();
                userMessageRecord.UserId = userid;
                userMessageRecord.MsgType = MessageType.IntegralConsum;
                userMessageRecord.MsgContent = string.Format("恭喜您，您于{0}，在保养服务中获得{1}积分。积分可用于到店维保抵扣现金或在线商城购买商品。",
                    DateTime.Now, values);
                _AppContext.UserMessageRecordApp.Insert(userMessageRecord);
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion


        #region 灵狐活动消费积分
        /// <summary>
        /// 灵狐活动消费积分
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="values">积分值</param>
        /// <param name="activityname">活动名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/integral/ConsumeIntegralForLH")]
        //[IOVAuthorize]
        public IHttpActionResult ConsumeIntegralForLH(string userid, int values, string activityname)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(userid);
            BLMS.Common.LogService.Instance.Info("调用ConsumeIntegralForLH接口" + string.Format("灵狐活动消费积分，方法：ConsumeIntegralForLH 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    result.IsSuccess = false;
                    result.Message = "用户ID不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(Convert.ToString(values)))
                {
                    result.IsSuccess = false;
                    result.Message = "积分值不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(activityname))
                {
                    result.IsSuccess = false;
                    result.Message = "活动名称不能为空";
                    return Ok(result);
                }
                //获取用户当前的总积分
                var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(userid);

                if (_totalScore < values)
                {
                    result.IsSuccess = false;
                    result.Message = "您的账户中的积分不足！！！ ";
                    return Ok(result);
                }
       
                var integralDatas = _AppContext.UserIntegralApp.GetList(userid);
                if (integralDatas != null && integralDatas.Count() > 0)
                {
                    int overIntegral = Convert.ToInt32((-values));//消费使用积分

                    foreach (var integralItem in integralDatas)
                    {
                        int realerIntegral = integralItem.value - integralItem.usevalue;
                        int subIntegral = overIntegral > realerIntegral ? realerIntegral : overIntegral;

                        if (overIntegral < realerIntegral)
                        {

                            _AppContext.UserIntegralApp.SubIntegral(integralItem.Id, userid, Math.Abs(subIntegral));
                            break;
                        }

                    }

                }

                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = userid,
                    integralSource = "140",
                    value = (-values),
                    datastate = 0,
                    ProductName = activityname,
                    remark = "灵狐活动消费积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
                
                //个人中心推送积分变动消息
                UserMessageRecord userMessageRecord = new UserMessageRecord();
                userMessageRecord.UserId = userid;
                userMessageRecord.MsgType = MessageType.IntegralConsum;
                userMessageRecord.MsgContent = string.Format("恭喜您，您于{0}，在保养服务中使用{1}积分。积分可用于到店维保抵扣现金或在线商城购买商品。",
                    DateTime.Now, values);
                _AppContext.UserMessageRecordApp.Insert(userMessageRecord);
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion
    }
}
