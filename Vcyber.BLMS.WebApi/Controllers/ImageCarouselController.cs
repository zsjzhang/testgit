using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI;
using System.Net;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 轮播图
    /// </summary>
    public class ImageCarouselController : ApiController
    {

        /// <summary>
        /// 获取所有轮播图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ImageCarousel")]
        [ResponseType(typeof(IEnumerable<ImageCarousel>))]
        public IHttpActionResult GetAllImageCarousel()
        {
            var total = 0;
            var list = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, null, null, int.MaxValue, out total);
            return this.Ok(new ReturnObject(list));
        }
    }
}