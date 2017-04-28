using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iovps.Services.Manage.Handler
{
    /// <summary>
    /// AddPrudoctImage 的摘要说明
    /// </summary>
    public class AddPrudoctImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //var result = UploadImageHandler.Upload(context.Request.Files[0]);
            //if (result != null)
            //{
            //    context.Response.Write(result.Url);
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}