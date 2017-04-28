using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 活动
    /// </summary>
    public class ActivitiesController : ApiController
    {
        /// <summary>
        /// 按页获取所有活动记录
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的活动记录</returns>
        [HttpGet]
        [Route("api/Activities")]
        [ResponseType(typeof(Page<Activities>))]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            int total = 0;
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize < 0)
            {
                pageSize = 0;
            }
            var index = (pageNumber - 1) * pageSize;

            var list = _AppContext.ActivitiesApp.Select(string.Empty,null,index, pageSize, out total);

            Page<Activities> returnList = new Page<Activities>();
            returnList.CurrentPage = pageNumber;
            returnList.ItemsPerPage = pageSize;
            var enumerable = list as IList<Activities> ?? list.ToList();
            returnList.Items = enumerable.ToList();
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        /// <summary>
        /// 按页获取所有进行中的活动
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的进行中的活动</returns>
        [HttpGet]
        [ResponseType(typeof(Page<Activities>))]
        public IHttpActionResult GetInProgress(int pageNumber, int pageSize)
        {
            int total = 0;
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize < 0)
            {
                pageSize = 0;
            }
            var index = (pageNumber - 1) * pageSize;

            var list = _AppContext.ActivitiesApp.Select(string.Empty,(int)EActivitiescsStatus.InProcess,index, pageSize, out total);

            Page<Activities> returnList = new Page<Activities>();
            returnList.CurrentPage = pageNumber;
            returnList.ItemsPerPage = pageSize;
            var enumerable = list as IList<Activities> ?? list.ToList();
            returnList.Items = enumerable.ToList();
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="id">活动ID</param>
        /// <returns>获取活动详情</returns>
        [HttpGet]
        [Route("api/Activities/{id}")]
        [ResponseType(typeof(Activities))]
        public IHttpActionResult GetActivitiesById(int id)
        {
            var activities  = _AppContext.ActivitiesApp.GetActivitiesById(id);
            return Ok(new ReturnObject(activities));
        }
    }
}