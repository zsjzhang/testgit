using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;

namespace Vcyber.BLMS.WebApi.Controllers
{
    [RoutePrefix("api/OrderChange")]
    public class OrderChangeController : ApiController
    {
        [HttpGet]
        [Route("GetXDActivityByActivityId/{activityId}")]
        [ResponseType(typeof(XDActivity))]
        public IHttpActionResult GetXDActivityByActivityId(int activityId)
        {
            return this.Ok(new ReturnObject(_AppContext.XDActivityApp.GetXDActivityByActivityId(activityId)));
        }
        /// <summary>
        /// 添加预约置换
        /// </summary>
        /// <param name="xDOrderChange"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrderChange")]
        public IHttpActionResult AddOrderChange(XDOrderChange xDOrderChange)
        {
            try
            {                
                if (_AppContext.XDOrderChangeApp.IsCanOrderChange(xDOrderChange.CarSeriers, xDOrderChange.Mobile,xDOrderChange.ShopCode))
                {
                    return Ok(new ReturnObject { Code = "0", Message = "您已经预约过，请耐心等待，经销商会主动与您取得联系" });
                }
                bool result = _AppContext.XDOrderChangeApp.AddOrderChange(xDOrderChange);
                if (!result)
                {
                    return Ok(new ReturnObject { Code = "0", Message = "预约置换失败！" });
                }
                else
                {
                    return Ok(new ReturnObject { Code = "200", Message = "预约置换成功！" });
                }
            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "0", Message = "预约置换异常！" });
            }
        }
        [HttpPost]
        [Route("IsInvited")]
        public IHttpActionResult IsInvited(XDInviter xDInviter)
        {
            try
            {
                if (_AppContext.XDInviterApp.IsInvited(xDInviter.InviteredMobile,xDInviter.ActivityId))
                {
                    return Ok(new ReturnObject { Code = "200", Message = "该手机号：" + xDInviter.InviteredMobile + "已经被邀请过！" });
                }
                else
                {
                    return Ok(new ReturnObject { Code = "002", Message = "未被邀请过！" });
                }
            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "003", Message = "未被邀请过异常！" });
            }
        }
        [HttpPost]
        [Route("AddXDInviter")]
        public IHttpActionResult AddXDInviter(XDInviter xDInviter)
        {
            try
            {
                if (string.IsNullOrEmpty(xDInviter.InviterUserId))
                {
                    return Ok(new ReturnObject { Code = "001", Message = "该用户没有登录,不可以推荐！" });
                }
                if (_AppContext.XDInviterApp.AddXDInviter(xDInviter))
                {
                    bool isExistLottery = _AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserId(xDInviter.InviterUserId, xDInviter.ActivityId, 0);
                    if (isExistLottery)
                    {
                        return Ok(new ReturnObject { Code = "201", Message = "已抽奖！" });
                    }
                    else
                    {
                        return Ok(new ReturnObject { Code = "200", Message = "推荐成功！" });
                    }
                }
                else
                {
                    return Ok(new ReturnObject { Code = "002", Message = "推荐失败！" });
                }
            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "003", Message = "推荐异常！" });
            }
        }

        /// <summary>
        /// 获取预约置换抽奖
        /// </summary>
        /// <param name="xDOrderChange"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderChangeLottery")]
        public IHttpActionResult GetOrderChangeLottery(string mobile, int activityId, int lotteryType, string lotterySource)
        {
            try
            {
                if (!_AppContext.XDOrderChangeApp.IsCanOrderChange(activityId, mobile))
                {
                    return Ok(new ReturnObject { Code = "001", Message = "该手机号未参与过预约置换,不可以抽奖！" });
                }
                else if (_AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserMobile(mobile, activityId, lotteryType))
                {
                    return Ok(new ReturnObject { Code = "002", Message = "该手机号已参与预约置换抽奖！" });
                }
                XDActivity activity = _AppContext.XDActivityApp.GetXDActivityByActivityId(activityId);
                if (activity == null)
                {
                    return Ok(new ReturnObject { Code = "004", Message = "该活动不存在！" });
                }
                XDLottery lottery = _AppContext.XDLotteryApp.GetNextLottery(activityId, lotteryType);
                if (lottery == null)
                {
                    return Ok(new ReturnObject { Code = "100", Message = "奖品已抽完！" });
                }
                else
                {
                    XDLotteryRecord xDLotteryRecord = new XDLotteryRecord();
                    xDLotteryRecord.LotteryId = lottery.LotteryId;
                    xDLotteryRecord.ActivityId = activityId;
                    xDLotteryRecord.ActivityName = activity.ActivityName;
                    xDLotteryRecord.LotteryName = lottery.LotteryName;
                    xDLotteryRecord.UserId = mobile;
                    xDLotteryRecord.UserName = mobile;
                    xDLotteryRecord.LotteryRecordTime = DateTime.Now;
                    xDLotteryRecord.UserIp = "";
                    xDLotteryRecord.IsValid = 1;
                    xDLotteryRecord.LotteryType = lotteryType;
                    xDLotteryRecord.UserMobile = mobile;
                    xDLotteryRecord.LotteryRecordSource = lotterySource;
                    xDLotteryRecord.CreaterId = mobile;
                    xDLotteryRecord.CreaterName = mobile;
                    xDLotteryRecord.CreaterTime = DateTime.Now;
                    xDLotteryRecord.UpdaterId = mobile;
                    xDLotteryRecord.UpdaterName = mobile;
                    xDLotteryRecord.UpdaterTime = DateTime.Now;
                    //添加中奖记录
                    int LotteryRecordId = _AppContext.XDLotteryRecordApp.AddXDLotteryRecord(xDLotteryRecord);
                    lottery.LotteryRecordId = LotteryRecordId;
                    if (lottery.LotteryId > 0)
                    {
                        //减少活动的奖品剩余数量
                        _AppContext.XDActivityApp.UpdateActivityLotteryBalanceCount(activityId);
                        //减少奖品的剩余数量
                        _AppContext.XDLotteryApp.UpdateLotteryBalanceCount(lottery.LotteryId, activityId);

                        XDOrderChange xDOrderChange = _AppContext.XDOrderChangeApp.GetOrderChangeByMobile(activityId, mobile);
                        XDSendInfo xDSendInfo = new XDSendInfo();
                        xDSendInfo.UserId = "";
                        xDSendInfo.ActivityId = activityId;
                        xDSendInfo.CreaterId = "0";
                        xDSendInfo.CreaterName = xDOrderChange.Name;
                        xDSendInfo.CreaterTime = DateTime.Now;
                        xDSendInfo.UpdaterId = "0";
                        xDSendInfo.UpdaterName = xDOrderChange.Name;
                        xDSendInfo.UpdaterTime = DateTime.Now;
                        xDSendInfo.IsValid = 1;
                        xDSendInfo.LotteryId = lottery.LotteryId;
                        xDSendInfo.UserName = xDOrderChange.Name;
                        xDSendInfo.UserMobile = xDOrderChange.Mobile;
                        xDSendInfo.SendProvince = xDOrderChange.SendProvince;
                        xDSendInfo.SendCity = xDOrderChange.SendCity;
                        xDSendInfo.SendDistrinct = xDOrderChange.SendDistrinct;
                        xDSendInfo.SendAddress = xDOrderChange.SendAddress;
                        xDSendInfo.SendSource = lotterySource;
                        xDSendInfo.LotteryRecordId = LotteryRecordId;
                        _AppContext.XDSendInfoApp.AddXDSendInfo(xDSendInfo);
                    }
                    return Ok(new ReturnObject { Code = "200", Message = "可以参与推荐抽奖！", Content = lottery });
                }

            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "0", Message = "是否参与过预约置换抽奖校验异常！" });
            }
        }
        [HttpPost]
        [Route("GetInviterLottery")]
        public IHttpActionResult GetInviterLottery(string userId,int activityId, int lotteryType, string lotterySource)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(new ReturnObject { Code = "001", Message = "该用户没有登录,不可以抽奖！" });
                }
                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
                if (account == null || account.Result == null)
                {
                    return Ok(new ReturnObject { Code = "005", Message = "会员账号不存在！" });
                }

                if (account.Result.SystemMType!=2)//是否绑定身份证
                {
                    return Ok(new ReturnObject { Code = "006", Message = "用户还没有绑定身份证！" });
                }
                if (_AppContext.XDInviterApp.IsExistXDInviter(userId, activityId) <= 0)
                {
                    return Ok(new ReturnObject { Code = "002", Message = "该用户没有推荐,不可以抽奖！" });
                }
                else if (_AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserId(userId, activityId, lotteryType))
                {
                    return Ok(new ReturnObject { Code = "003", Message = "该用户已参与推荐抽奖！" });
                }
                XDActivity activity = _AppContext.XDActivityApp.GetXDActivityByActivityId(activityId);
                if (activity == null)
                {
                    return Ok(new ReturnObject { Code = "004", Message = "该活动不存在！" });
                }
                XDLottery lottery = _AppContext.XDLotteryApp.GetNextLottery(activityId, lotteryType);
                if (lottery == null)
                {
                    return Ok(new ReturnObject { Code = "100", Message = "奖品已抽完！" });
                }
                else
                {
                    var user = account.Result;
                    XDLotteryRecord xDLotteryRecord = new XDLotteryRecord();
                    xDLotteryRecord.LotteryId = lottery.LotteryId;
                    xDLotteryRecord.ActivityId = activityId;
                    xDLotteryRecord.ActivityName = activity.ActivityName;
                    xDLotteryRecord.LotteryName = lottery.LotteryName;
                    xDLotteryRecord.UserId = userId;
                    xDLotteryRecord.UserName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.LotteryRecordTime = DateTime.Now;
                    xDLotteryRecord.UserIp = "";
                    xDLotteryRecord.IsValid = 1;
                    xDLotteryRecord.LotteryType = lotteryType;
                    xDLotteryRecord.UserMobile = user.PhoneNumber;
                    xDLotteryRecord.LotteryRecordSource = lotterySource;
                    xDLotteryRecord.CreaterId = userId;
                    xDLotteryRecord.CreaterName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.CreaterTime = DateTime.Now;
                    xDLotteryRecord.UpdaterId = userId;
                    xDLotteryRecord.UpdaterName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.UpdaterTime = DateTime.Now;
                    //添加中奖记录
                    int LotteryRecordId = _AppContext.XDLotteryRecordApp.AddXDLotteryRecord(xDLotteryRecord);
                    lottery.LotteryRecordId = LotteryRecordId;
                    if (lottery.LotteryId > 0)
                    {
                        //减少活动的奖品剩余数量
                        _AppContext.XDActivityApp.UpdateActivityLotteryBalanceCount(activityId);
                        //减少奖品的剩余数量
                        _AppContext.XDLotteryApp.UpdateLotteryBalanceCount(lottery.LotteryId, activityId);
                    }
                    return Ok(new ReturnObject { Code = "200", Message = "可以参与推荐抽奖！", Content = lottery });
                }
            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "0", Message = "预约置换抽奖异常！" });
            }
        }

        [HttpPost]
        [Route("AddXDSendInfo")]
        public IHttpActionResult AddXDSendInfo(XDSendInfo xDSendInfo)
        {
            try
            {
                if (_AppContext.XDSendInfoApp.AddXDSendInfo(xDSendInfo))
                {
                    return Ok(new ReturnObject { Code = "200", Message = "添加邮寄地址成功！" });
                }
                else
                {
                    return Ok(new ReturnObject { Code = "0", Message = "添加邮寄地址失败！" });
                }
            }
            catch (Exception)
            {
                return Ok(new ReturnObject { Code = "0", Message = "添加邮寄地址异常！" });
            }

        }

        /// <summary>
        /// 返回中奖列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ReturnObject))]
        [Route("LotteryRecords")]
        public IHttpActionResult LotteryRecord(int activityId)
        {
            var result = new ReturnObject("200", null);
            try
            {                
                IEnumerable<XDLotteryRecord> query = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityId, 0, 0);
                result.Content = query;
            }
            catch (Exception ex)
            {
                result.Code = "500";
                result.Message = ex.Message;
            }
            return this.Ok(result);
        }

        /// <summary>
        /// 查询活动列表
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <param name="activityId">活动ID</param>
        /// <returns>活动列表</returns>
        [HttpGet]
        [Route("GetXDActivitys")]
        [ResponseType(typeof(ReturnObject))]
        public IHttpActionResult GetXDActivitys(int activityType, int activityId)
        {
            var result = new ReturnObject("200", null);
            try
            {
                var query = _AppContext.XDActivityApp.GetXDActivityList(activityType, activityId);
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