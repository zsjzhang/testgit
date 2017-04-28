using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Application;
using System.IO;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System.Configuration;

    /// <summary>
    /// 商品管理
    /// </summary>
    [MvcAuthorize]
    public class ImageUploadController : Controller
    {
        /// <summary>
        /// 上传纸质工单或发票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePaperOrder(int id)
        {
            HttpContextBase context = this.HttpContext;

            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = context.Request.Files[0];

                string result = this.SaveImage(file, "PaperOrder");
                if (!string.IsNullOrEmpty(result))
                {
                    _AppContext.ConsumeApp.UpdatePaperOrder(id, result);
                    return Content(result);
                }
            }

            return Content("");
        }

        /// <summary>
        /// 更新纸质工单或发票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadPaperOrder()
        {
            HttpContextBase context = this.HttpContext;

            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = context.Request.Files[0];


                return Content(this.SaveImage(file, "PaperOrder"));
            }

            return Content("");
        }

        private string SaveImage(HttpPostedFileBase file, string folder)
        {
            string extendsName = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(extendsName) || !imgExtends.Contains(extendsName.ToLower()) || file.ContentLength > 10485760)
            {
                return string.Empty;
            }
            //todo: save in folder by date
            string newFileName = DateTime.Now.ToString("HHmmss") + "_" + Guid.NewGuid().ToString("N") + extendsName;
            string filePath = ConfigurationManager.AppSettings[folder] + DateTime.Today.ToString("yyyyMMdd") + "/";
            string diskPath = HttpContext.Server.MapPath(filePath);
            if (!Directory.Exists(diskPath)) Directory.CreateDirectory(diskPath);
            file.SaveAs(Path.Combine(diskPath, newFileName));
            return filePath + newFileName;
        }

        private const string imgExtends = ".bmp.png.jpeg.jpg.gif";
    }
}