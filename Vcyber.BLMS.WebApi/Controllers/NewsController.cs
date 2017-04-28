using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using System.Web.UI;

    using PetaPoco;

    /// <summary>
    /// 新闻
    /// </summary>
    public class NewsController : ApiController
    {
        /// <summary>
        /// 按页获取所有新闻记录
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的新闻记录</returns>
        [HttpGet]
        [ResponseType(typeof(Page<News>))]
        public IHttpActionResult GetNews(int pageNumber, int pageSize)
        { 
            int total =0 ;
            if(pageNumber<=0)
            {
                pageNumber = 1;
            }
            if (pageSize<0) 
            {
                pageSize = 0;
            }
            var index = (pageNumber - 1) * pageSize;

            var list = _AppContext.NewsApp.Select(string.Empty,1,(int)EApproveStatus.Approved,string.Empty, index, pageSize, out total);
           
            Page<News> returnList = new Page<News>();
            returnList.CurrentPage = pageNumber;
            returnList.ItemsPerPage = pageSize;
            var enumerable = list as IList<News> ?? list.ToList();
            returnList.Items = enumerable.ToList();
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        /// <summary>
        /// 按页获取所有热点新闻记录
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的热点新闻记录</returns>
        [HttpGet]
        [ResponseType(typeof(Page<News>))]
        [Route("api/HotNews")]
        public IHttpActionResult GetHotNews(int pageNumber, int pageSize)
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

            var list = _AppContext.NewsApp.SelectHotNews(string.Empty, index, pageSize, out total);

            Page<News> returnList = new Page<News>();
            returnList.CurrentPage = pageNumber;
            returnList.ItemsPerPage = pageSize;
            var enumerable = list as IList<News> ?? list.ToList();
            returnList.Items = enumerable.ToList();
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="id">新闻ID</param>
        /// <returns>获取新闻详情</returns>
        [HttpGet]
        [Route("api/News/{id}")]
        [ResponseType(typeof(News))]
        public IHttpActionResult GetNewsById(int id)
        {
            var news = _AppContext.NewsApp.GetNewsById(id);
            return Ok(new ReturnObject(news));
        }

    }
}