using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 杂志
    /// </summary>
    public class MagazineController : ApiController
    {
        /// <summary>
        /// 按页获取所有杂志
        /// </summary>
        /// <param name="year">杂志所属年份</param>
        /// <param name="month">杂志所属月份</param>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的活动杂志</returns>
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(Page<Magazine>))]
        public IHttpActionResult GetMagazine(int? year, int? month, int pageNumber, int pageSize)
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

            var list = _AppContext.MagazineApp.GetMagazineList((int)EApproveStatus.Approved,year, month, string.Empty, index, pageSize, out total);
            Page<Magazine> returnList = new Page<Magazine>();
            returnList.CurrentPage = pageNumber;
            returnList.ItemsPerPage = pageSize;
            var enumerable = list as IList<Magazine> ?? list.ToList();
            returnList.Items = enumerable.ToList();
            returnList.TotalItems = total;
            return Ok(new ReturnObject(returnList));
        }

        /// <summary>
        /// 按页获取所有杂志
        /// </summary>
        /// <param name="year">杂志所属年份</param>
        /// <param name="month">杂志所属月份</param>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>指定页码的活动杂志</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/magazine/all")]
        [ResponseType(typeof(Page<Magazine>))]
        public IHttpActionResult All()
        {            
            var query = _AppContext.MagazineApp.GetMagazineAll();
            return Ok(new ReturnObject(query));
        }

        /// <summary>
        /// 获取杂志详情
        /// </summary>
        /// <param name="id">杂志ID</param>
        /// <returns>获取杂志详情</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Magazine/{id}")]
        [ResponseType(typeof(Magazine))]
        public IHttpActionResult GetMagazineById(int id)
        {
            var magazine = _AppContext.MagazineApp.GetMagazineById(id);
            return Ok(new ReturnObject(magazine));
        }
    }
}