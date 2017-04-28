using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Description;
using System.Web.UI;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 经销商
    /// </summary>
    public class DealerController : ApiController
    {

        /// <summary>
        /// 搜索经销商
        /// </summary>
        /// <param name="keyWord">经销商名字关键字</param>
        /// <param name="page">页索引</param>
        /// <param name="itemsPerPage">页大小</param>
        /// <returns></returns>
        [Route("api/Dealers")]
        [HttpGet]
        [ResponseType(typeof(Page<CSCarDealerShip>))]
        public IHttpActionResult GetDealerListByKeyWord(string keyWord, long page = 1, long itemsPerPage = 20)
        {
            Page<CSCarDealerShip> list = _AppContext.DealerApp.GetDealerListByKeyWord(keyWord, page, itemsPerPage);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据省、市，返回经销商列表
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>

       // [CrossSite]
        [Route("api/Dealers")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CSCarDealerShip>))]
        public IHttpActionResult GetDealerList(string province, string city)
        {
            var list = _AppContext.DealerApp.GetDealerList(province, city).ToList();
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 获取经销商详情
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [Route("api/Dealers/{id}")]
        [HttpGet]
        [ResponseType(typeof(CSCarDealerShip))]
        public IHttpActionResult GetDealerDetailsById(string id)
        {
            var entity = _AppContext.DealerApp.GetDealerDetailsById(id);
            return this.Ok(new ReturnObject(entity));
        }
        /// <summary>
        /// 根据经销商编号获取经销商
        /// </summary>
        /// <param name="dealerId">经销商编号</param>
        /// <returns>经销商信息</returns>
        [HttpGet]
        [Route("api/dealers/getdealerbycode")]
        public IHttpActionResult GetDealerByCode(string dealerId)
        {
            var dealer = _AppContext.DealerApp.GetDealerByDealerId(dealerId);
            return this.Ok(new ReturnObject(dealer));
        }

        /// <summary>
        /// 获取经销商所在的所有省份
        /// </summary>
        /// <returns></returns>
        [CrossSite]
        [Route("api/Provinces")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult GetProvinceList()
        {
            var result = _AppContext.DealerApp.GetProvinceList();
            return this.Ok(new ReturnObject(result));

        }

        /// <summary>
        /// 获取经销商所在的的某一省份下所有城市
        /// </summary>
        /// <param name="province">省份</param>
        /// <returns>城市列表</returns>

        [CrossSite]
        [Route("api/Citys")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult GetCityListByProvince(string province)
        {
            var result = _AppContext.DealerApp.GetCityListByProvince(province);
            return this.Ok(new ReturnObject(result));

        }

        /// <summary>
        /// 获取经销商顾问信息
        /// </summary>
        /// <param name="dealerId">经销商Id(DealerId)</param>
        /// <returns>顾问列表</returns>
        [Route("api/Dealer/Consultant/{dealerId}")]
        [ResponseType(typeof(IEnumerable<Vcyber.BLMS.Entity.Generated.CSConsultant>))]
        public IHttpActionResult GetConsultantByDealerId(string dealerId)
        {
            return this.Ok(new ReturnObject(_AppContext.CSConsultantApp.GetList(dealerId)));
        }

        /// <summary>
        /// 根据两个地点距离查询经销商
        /// </summary>
        /// <param name="province">经销商所在省份</param>
        /// <param name="city">经销商所在城市</param>
        /// <param name="long1">第一个地点经度</param>
        /// <param name="lat1">第一个地点纬度</param>
        /// <param name="long2">第二个地点经</param>
        /// <param name="lat2">第二个地点纬度</param>
        /// <param name="distance">搜索半径(米)</param>
        /// <returns>经销商列表</returns>

        
        [Route("api/Dearler/TwoPoints")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CSCarDealerShip>))]
        public IHttpActionResult GetDealerListByDistance2(
            string province,
            string city,
            double long1,
            double lat1,
            double long2,
            double lat2,
            int distance = 20000)
        {
            IEnumerable<CSCarDealerShip> list = _AppContext.DealerApp.GetDealerListByDistance
            (province,
              city,
              long1,
              lat1,
              long2,
              lat2,
             distance);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据一个地点距离查询经销商
        /// </summary>
        /// <param name="province">经销商所在省份</param>
        /// <param name="city">经销商所在城市</param>
        /// <param name="long">第一个地点经度</param>
        /// <param name="lat">第一个地点纬度</param>
        /// <param name="distance">搜索半径(米)</param>
        /// <returns>经销商列表</returns>
        [Route("api/Dearler/OnePoint")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CSCarDealerShip>))]
        public IHttpActionResult GetDealerListByDistance(
            string province,
            string city,
            double @long,
            double lat,
            int distance = 20000)
        {
            IEnumerable<CSCarDealerShip> list = _AppContext.DealerApp.GetDealerListByDistance
            (province,
              city,
              @long,
              lat,
             distance);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据省、市，返回可以支付的经销商列表
        /// </summary>
        /// <param name="province">省，不需要请输入空值</param>
        /// <param name="city">市，不需要请输入空值</param>
        /// <returns></returns>
        [Route("api/PaidDealers")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CSCarDealerShip>))]
        public IHttpActionResult GetPaiedDealerList(string province, string city)
        {
            var list = _AppContext.DealerApp.GetPaidDealerShip(province, city).ToList();
            return this.Ok(new ReturnObject(list));
        }
        /// <summary>
        /// 申请微信商户
        /// </summary>
        [HttpPost]
        [Route("api/dealer/addweixinmerchant")]
        public IHttpActionResult AddWeixinMerchant(WeixinMerchant obj)
        {
            var message = string.Empty;
            var isExist = _AppContext.WeixinMerchantApp.IsExist(obj.DealerId, obj.OpenId);
            if (!isExist)
            {
                _AppContext.WeixinMerchantApp.Add(obj);
                message = "您的信息已经提交成功，如有问题，客服人员会联系您，请保持手机畅通。";
            }
            else
            {
                message = "您已经提交过申请了，如须修改，请联系客服。";
            }
            return this.Ok(new ReturnObject("200", "success", message));
        }

        /// <summary>
        /// 车主故事--我要发帖
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="userid"></param>
        /// <param name="Content"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/dealer/AddDealerStory")]
        public IHttpActionResult AddDealerStory(string Title, string userid, string Content, string img)
        {
            var msg = string.Empty;
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                var isExist = _AppContext.DealerApp.AddDealerStory(Title, userid, Content, img);
                msg = "发帖成功";
                return Ok(new ReturnObject("200", "success", msg));
            }
            catch (Exception ex)
            {
                msg = "发帖失败";
                return this.Ok(new ReturnObject("500", "success", msg));
            }
        }

        /// <summary>
        /// 车主故事--删除帖子
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dealer/DelDealerStory")]
        public IHttpActionResult DelDealerStory(int id)
        {
            var message = string.Empty;
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "",Message = "", Data = ""};
            try
            {
                var isExist = _AppContext.DealerApp.DelDealerStory(id);
                message = "删除帖子成功";
                return Ok(new ReturnObject("200", "success", message));
            }
            catch
            {
                message = "删除帖子失败";
                return Ok(new ReturnObject("500", "success", message));
            }
        }

        /// <summary>
        /// 车主故事--更新帖子
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <param name="Contents">帖子内容</param>
        /// <param name="Title">标题</param>
        /// <param name="img">图片</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/dealer/UpdateDealerStory")]
        public IHttpActionResult UpdateDealerStory(int id,string Contents,string Title,string img)
        {
            var message = string.Empty;
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                var isExist = _AppContext.DealerApp.UpdateDealerStory(id, Contents,Title,img);
                message = "更新帖子成功";
                return Ok(new ReturnObject("200", "success", message));
            }
            catch
            {
                message = "更新帖子失败";
                return Ok(new ReturnObject("500", "success", message));
            }
        }

        /// <summary>
        /// 车主故事--查找个人全部帖子
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dealer/SelactDealerStory")]
        public IHttpActionResult SelactDealerStory(string Userid)
        {
            var message = string.Empty;
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                DealerStory Detail = new DealerStory();
                var StoryInfo = _AppContext.DealerApp.SelactDealerStory(Userid);
                //if (StoryInfo != null)
                //{
                //    foreach (var item in StoryInfo)
                //    {
                //         Detail.Title = item.Title;
                //         Detail.Content = item.Content;
                //         Detail.Createtime = item.Createtime;
                //         Detail.image = item.image;
                //         Detail.UpdateTime = item.UpdateTime;
                //    }
                    
                //}
                return this.Ok(new ReturnObject(StoryInfo));
            }
            catch
            {
                message = "查找用户帖子失败";
                return Ok(new ReturnObject("500", "success", message));
            }
        }

        /// <summary>
        /// 车主故事--根据ID查找相应的帖子
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dealer/SelactDealerStoryForId")]
        public IHttpActionResult SelactDealerStoryForId(string id)
        {
            var message = string.Empty;
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = "" };
            try
            {
                DealerStory Detail = new DealerStory();
                var StoryInfo = _AppContext.DealerApp.SelactDealerStoryForId(id);
                //if (StoryInfo != null)
                //{
                //    foreach (var item in StoryInfo)
                //    {
                //         Detail.Title = item.Title;
                //         Detail.Content = item.Content;
                //         Detail.Createtime = item.Createtime;
                //         Detail.image = item.image;
                //         Detail.UpdateTime = item.UpdateTime;
                //    }

                //}
                return this.Ok(new ReturnObject(StoryInfo));
            }
            catch
            {
                message = "查找帖子失败";
                return Ok(new ReturnObject("500", "success", message));
            }
        }
    }
}