using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Application;

using System.Web.Http.Description;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Filter;
    using AspNet.Identity.SQL;

    /// <summary>
    /// 蓝豆
    /// </summary>
    public class BlueBeanController : ApiController
    {
        #region ==== 构造函数 ====

        public BlueBeanController() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取用户蓝豆信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="index">分页索引</param>
        /// <param name="page">分页个数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/bluebean/all")]
        [ResponseType(typeof(Page<BlueBeanV>))]
        [IOVAuthorize]
        public IHttpActionResult All(string userId, int index = 1, int page = 10)
        {
            string status = "00";
            BlueBeanCollectionV dataResult = new BlueBeanCollectionV() { };
            int total;
            PageData pageData = new PageData() { Index = index, Size = page };
            var blueBeans = _AppContext.UserBlueBeanApp.GetAll(userId, pageData, out total);

            if (blueBeans != null && blueBeans.Count() > 0)
            {
                dataResult.totalrecord = total;
                dataResult.totalpage = total % pageData.Size == 0 ? total / pageData.Size : total / pageData.Size + 1;
                dataResult.record = blueBeans.Count();
                dataResult.page = pageData.Index;

                foreach (var item in blueBeans)
                {
                    dataResult.datas.Add(new BlueBeanV() { CreateTime = item.CreateTime.ToString(), remark = item.remark, value = item.value });
                }

                status = "99";
            }
            Page<BlueBeanV> pageD = new Page<BlueBeanV>();
            pageD.Items = dataResult.datas;
            pageD.CurrentPage = index;
            pageD.ItemsPerPage = page;
            pageD.TotalItems = total;

            return this.Ok(new ReturnObject(pageD));

            //return this.DataResult<BlueBeanCollectionV>(dataResult, status);
        }

        [HttpPost]
        [Route("api/bluebean/add")]
        [ResponseType(typeof(bool))]
        [IOVAuthorize]
        public IHttpActionResult Add(string userId, int blueBean)
        {
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;

            if (account == null)
            {
                return Ok(new ReturnResult { IsSuccess = false, Message = "未找到用户，请确认用户编号" });
            }

            _AppContext.UserBlueBeanApp.Add(new UserblueBean
            {
                userId = userId,
                integralSource = ((int)EBRuleType.定期活动).ToString(),
                value = blueBean,
                datastate = 0,
                remark = EBRuleType.定期活动.ToString(),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });

            return Ok(new ReturnResult { IsSuccess = true });
        }

        /// <summary>
        /// 取用户所有的蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bluebean/GetAll")]
        [ResponseType(typeof(int))]
        [IOVAuthorize]
        public IHttpActionResult GetAll(string userId)
        {
            var result = _AppContext.UserBlueBeanApp.GetTotalBlueBean(userId);
            return Ok(result);
        }
        #endregion
    }
}
