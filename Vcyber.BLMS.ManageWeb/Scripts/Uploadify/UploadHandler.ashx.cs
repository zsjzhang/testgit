using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Vcyber.ServiceCenter.Manage.Scripts.Uploadify
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            HttpPostedFile file = context.Request.Files["Filedata"];

            string end = Path.GetExtension(file.FileName);
            string uploadPath = context.Server.MapPath("~/Upload\\News\\Image\\");
            string fileName = Guid.NewGuid().ToString() + end;

            file.SaveAs(uploadPath + fileName);
            context.Response.Write(fileName);
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