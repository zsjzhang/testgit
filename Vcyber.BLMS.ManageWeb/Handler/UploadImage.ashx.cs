using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.IO;

namespace Iovps.Services.Manage.Handler
{
    /// <summary>
    /// UploadImage 的摘要说明
    /// </summary>
    public class UploadImage : IHttpHandler
    {
        #region ==== 私有方法 ====

        private string imgExtends = ".bmp.png.jpeg.jpg.gif";

        #endregion

        public void ProcessRequest(HttpContext context)
        {
          
            if (context.Request.Files!=null&&context.Request.Files.Count>0)
            {
                HttpPostedFile file = context.Request.Files[0];
                string extendsName = Path.GetExtension(file.FileName);

                if (!this.imgExtends.Contains(extendsName.ToLower())||file.ContentLength>10485760)
                {
                    return;
                }

                string newFileName = Guid.NewGuid().ToString("N")+extendsName;
                file.SaveAs(Path.Combine(context.Server.MapPath("/UploadImg/"), newFileName));
                context.Response.Write("/UploadImg/"+newFileName);
            }
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