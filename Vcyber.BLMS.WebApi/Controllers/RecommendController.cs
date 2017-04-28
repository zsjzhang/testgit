using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Repository;
using Vcyber.BLMS.WebApi.Models;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 推荐接口
    /// </summary>
    public class RecommendController : ApiController
    {
        [ResponseType(typeof(ReturnResult))]
        [Route("api/Recommend/FormPost")]
        public IHttpActionResult FormPost(List<Recommend> model)
        {
            try
            {
                foreach (Recommend m in model)
                {
                    RecommendStorager storager = new RecommendStorager();
                    storager.Add(m);
                }
                return Ok(new ReturnResult { IsSuccess = true, Message = "提交成功" });
            }
            catch (Exception ex)
            {
                return Ok(new ReturnResult { IsSuccess = false, Message = "提交失败" });
            }   
        }
    }
}
