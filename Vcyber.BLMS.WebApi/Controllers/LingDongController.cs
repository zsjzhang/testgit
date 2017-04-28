using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Repository;
using Vcyber.BLMS.WebApi.Models;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 领动活动
    /// </summary>
    public class LingDongController : ApiController
    {
        const string ActivtyName = "lingding";
        /// <summary>
        /// 返回值说明：Code=0 调用失败，Code=1调用成功，Code=2已申请过试驾
        /// 如果调用成功且Content有值，则中奖，否则为未中奖
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("api/LingDong/TestDrive4Wx")]
        [HttpPost]
        [ResponseType(typeof(ReturnObject))]
        public IHttpActionResult TestDrive4Wx(SonataServiceEntity entity)
        {
            TestDriveEntity testEntity = new TestDriveEntity();
            testEntity.Phone = entity.Phone;
            testEntity.UserName = entity.UserName;
            testEntity.DealerId = entity.DealerId;
            testEntity.DealerName = entity.DealerName;
            testEntity.DealerCity = entity.DealerCity;
            testEntity.DealerProvince = entity.DealerCity;
            testEntity.CarSeries = entity.CarSeries;
            testEntity.OpenId = entity.OpenId;
            testEntity.ActivityName = ActivtyName;
            testEntity.ScheduleDate = entity.ScheduleDate;
            testEntity.UserSex = entity.UserSex;
            testEntity.DataSource = "blms_wechat";


            //entity.OrderType = EBMServiceType.LingDong;
            //entity.DataSource = "blms_wechat";

            LotteryDrawPoolType type = LotteryDrawPoolType.LingDong_TestDeive;
            if (CheckExistTestDrive(testEntity)) //是否已经提交过申请
            {
                //AwardSendRecordRepository awRepository=new AwardSendRecordRepository();
                //awRepository.GetAward(entity.OpenId, type)
                return Ok(new ReturnObject { Code = "2", Message = "已申请过试驾" });
            }

            int retval = _AppContext.TestDriveApp.Add(testEntity);
            //int retval = _AppContext.SonataServiceApp.Add(entity);
            if (retval > 0)
            {
                //抽奖
                LotteryDrawBL BL = new LotteryDrawBL();
                LotteryDrawPool LDP = BL.Execute(type, 10);
                if (LDP != null && LDP.Award != null)
                {
                    //更新奖品池数量
                    int result = BL.UpdateLotteryDrawPool(LDP.Id, BL.GetVersionNumber(LDP.Id).VersionNumber);
                    if (result <= 0)
                    {
                        return Ok(new ReturnObject { Code = "1", Message = "成功, 没有中奖" });
                    }
                    //中奖信息写入数据库
                    bool visualSendState = false;
                    if (LDP.Award.AwardType == AwardType.visual)
                    {
                        //虚拟奖品需要立即发放
                        visualSendState = BL.SendVisualAward(LDP, entity.OpenId);
                        //TO-DO
                    }
                    //记录奖品发放记录
                    BL.AwardSendRecord(LDP, entity.OpenId, visualSendState);

                    return Ok(new ReturnObject { Code = "1", Message = "成功, 中奖", Content = LDP.Award });
                }
                else
                {
                    //没有中奖
                    return Ok(new ReturnObject { Code = "1", Message = "成功, 没有中奖" });
                }
            }
            return Ok(new ReturnObject { Code = "0", Message = "提交失败" });
        }

        /// <summary>
        /// 是否已经提交过试驾
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [Route("api/LingDong/IsTestDrive")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public bool IsTestDrive(string openid)
        {
            return _IsTestDrive(ActivtyName, openid);
        }

        /// <summary>
        /// 是否已经推荐
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [Route("api/LingDong/IsRecommend")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public bool IsRecommend(string openid)
        {
            return _CheckExistRecommend(ACTIVITY_TYPE, openid);
        }

        private bool CheckExistTestDrive(TestDriveEntity entity)
        {
            return _IsTestDrive(entity.ActivityName, entity.OpenId);
        }

        private bool _IsTestDrive(string activityName, string openid)
        {
            TestDriveRepository repository = new TestDriveRepository();
            var result = repository.GetOne(activityName, openid);
            if (result != null && result.Id > 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 提交邮寄地址
        /// </summary>
        /// <param name="postAd"></param>
        /// <returns></returns>
        [Route("api/LingDong/PostAddress")]
        [HttpPost]
        [ResponseType(typeof(ReturnObject))]
        public IHttpActionResult AddAddress(PostAddress postAd)
        {
            PostAddressRepository repository = new PostAddressRepository();
            if (repository.Add(postAd) > 0)
            {
                return Ok(new ReturnObject { Code = "1", Message = "成功" });
            }
            return Ok(new ReturnObject { Code = "0", Message = "提交失败" });
        }

        /// <summary>
        /// 获取用户的中奖记录
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [Route("api/LingDong/GetAward")]
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, List<Award>>))]
        public IHttpActionResult GetAward(string openId)
        {
            Dictionary<string, List<Award>> re = new Dictionary<string, List<Award>>();
            AwardSendRecordRepository repository = new AwardSendRecordRepository();
            var result1 = repository.GetAward(openId, LotteryDrawPoolType.LingDong_TestDeive).ToList();
            var result2 = repository.GetAward(openId, LotteryDrawPoolType.LingDong_Recommend).ToList();

            //老车主推荐新车主购领动车成功
            //RecommendAward(openId, repository, result2);
            //试驾后购领动车，送千元加油卡（共10名）  事后抽奖，系统不处理

            re.Add("LingDong_TestDeive", result1);
            re.Add("LingDong_Recommend", result2);
            return Ok(re);
        }

        //private void TestDriveAward(string openId, AwardSendRecordRepository repository, List<Award> result1)
        //{
        //    if (!result1.Any(c => c.Alias == "gascard_1000"))
        //    {
        //        if (CheckTestDriveNewDriver(openId))
        //        {
        //        }
        //    }
        //}

        private void RecommendAward(string openId, AwardSendRecordRepository repository, List<Award> result2)
        {
            if (!result2.Any(c => c.Alias == "wecard_jcby"))//基础保养券
            {
                if (CheckRecommendNewDriver(openId))//被推荐人成为领动新车主
                {
                    var awardItem = new AwardRepository().GetByAlias("wecard_jcby");
                    repository.AddRecord(new AwardSendRecord
                    {
                        OpenId = openId,
                        AwardId = awardItem.Id,
                        Award = awardItem
                    });
                    result2.Add(awardItem);
                    //发微信券  TO-DO

                }
            }
        }

        private bool CheckTestDriveNewDriver(string openId)
        {
            RecommendRepository repository = new RecommendRepository();
            return repository.CheckTestDriveNewDriver(openId);
        }

        /// <summary>
        /// 被推荐人是否购买领动车
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private bool CheckRecommendNewDriver(string openId)
        {
            RecommendRepository repository = new RecommendRepository();
            return repository.CheckRecommendNewDriver(openId);
        }

        const string ACTIVITY_TYPE = "LingDong";

        /// <summary>
        /// 返回值说明：Code=0 调用失败，Code=1调用成功，Code=2已经推荐过
        /// 如果调用成功且Content有值，则中奖，否则为未中奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/LingDong/Recommend")]
        [ResponseType(typeof(ReturnObject))]
        public IHttpActionResult Recommend(RecommendViewModel_SB temp)
        {
            try
            {
                List<Vcyber.BLMS.WebApi.Models.RecommendViewModel> model = new List<Vcyber.BLMS.WebApi.Models.RecommendViewModel>();
                model.Add(new Vcyber.BLMS.WebApi.Models.RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name1, PhoneNumber = temp.PhoneNumber1 });
                model.Add(new Vcyber.BLMS.WebApi.Models.RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name2, PhoneNumber = temp.PhoneNumber2 });
                model.Add(new Vcyber.BLMS.WebApi.Models.RecommendViewModel { OpenId = temp.OpenId, Name = temp.Name3, PhoneNumber = temp.PhoneNumber3 });

                if (_CheckExistRecommend(ACTIVITY_TYPE, model.FirstOrDefault().OpenId))//是否已经推荐过
                {
                    return Ok(new ReturnObject { Code = "2", Message = "已经推荐过" });
                }
                RecommendRepository repository = new RecommendRepository();
                Recommend entity;

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (Vcyber.BLMS.WebApi.Models.RecommendViewModel vm in model)
                    {
                        if (!string.IsNullOrEmpty(vm.Name) && !string.IsNullOrEmpty(vm.PhoneNumber))
                        {
                            entity = new Entity.Recommend
                            {
                                OpenId = vm.OpenId,
                                ActivityType = ACTIVITY_TYPE,
                                Name = vm.Name,
                                PhoneNumber = vm.PhoneNumber
                            };
                            repository.Add(entity);
                        }
                    }

                    //抽奖
                    LotteryDrawBL BL = new LotteryDrawBL();
                    LotteryDrawPool LDP = BL.Execute(LotteryDrawPoolType.LingDong_Recommend, 10);
                    if (LDP != null && LDP.Award != null)
                    {
                        //更新奖品池数量
                        int result = BL.UpdateLotteryDrawPool(LDP.Id, BL.GetVersionNumber(LDP.Id).VersionNumber);
                        if (result <= 0)
                        {
                            scope.Complete();
                            return Ok(new ReturnObject { Code = "1", Message = "没有中奖" });
                        }
                        //中奖信息写入数据库
                        bool visualSendState = false;
                        if (LDP.Award.AwardType == AwardType.visual)
                        {
                            //虚拟奖品需要立即发放
                            visualSendState = BL.SendVisualAward(LDP, model.FirstOrDefault().OpenId);
                        }
                        //记录奖品发放记录
                        BL.AwardSendRecord(LDP, model.FirstOrDefault().OpenId, visualSendState);
                    }
                    scope.Complete();
                    if (LDP != null && LDP.Award != null)
                    {
                        return Ok(new ReturnObject { Code = "1", Message = "中奖", Content = LDP.Award });
                    }
                    else
                    {
                        return Ok(new ReturnObject { Code = "1", Message = "没有中奖" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Ok(new ReturnObject { Code = "0", Message = "提交失败" });
            }
        }

        private bool _CheckExistRecommend(string ACTIVITY_TYPE, string openid)
        {
            RecommendRepository repository = new RecommendRepository();
            var result = repository.GetByOpenId(ACTIVITY_TYPE, openid);
            if (result != null && result.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否微信绑定
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private bool IsWxBind(string openId)
        {
            WxBindNewRepository repository = new WxBindNewRepository();
            return repository.IsWxBind_New(openId);
        }

        /// <summary>
        /// 中奖名单查询，phoneNumber为空时，查询所有中奖名单
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/LingDong/GetAllLotteryList")]
        [ResponseType(typeof(Dictionary<string, List<LotteryModel>>))]
        public IHttpActionResult GetAllLotteryList(string phoneNumber)
        {
            AwardSendRecordRepository repository = new AwardSendRecordRepository();
            var result = repository.GetAllLottery(phoneNumber);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/LingDong/GetTest")]
        public IHttpActionResult GetTest(List<LotteryModel> model)
        {
            return Ok();
        }
    }
}
