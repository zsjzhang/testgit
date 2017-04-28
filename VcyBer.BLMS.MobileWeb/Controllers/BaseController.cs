using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 记录异常
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {        
            string errortime = string.Empty;
            string erroraddr = string.Empty;
            string errorinfo = string.Empty;
            string errorsource = string.Empty;
            string errortrace = string.Empty;

            errortime = string.Format("发生时间:{0}", DateTime.Now.ToString());
            erroraddr = string.Format("发生异常页: {0}", Request.Url.ToString());
            errorinfo = string.Format("异常信息: {0}", filterContext.Exception.Message);
            errorsource = string.Format("错误源:{0}", filterContext.Exception.Source);
            errortrace = string.Format("堆栈信息:{0}", filterContext.Exception.StackTrace);
            filterContext.ExceptionHandled = true;       
            //独占方式，因为文件只能由一个进程写入. 
            System.IO.StreamWriter writer = null;
            try
            {
                lock (this)
                {
                    //写入日志 
                    string year = DateTime.Now.ToString("yyyy");
                    string month = DateTime.Now.ToString("MM");
                    string path = string.Empty;
                    string filename = string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                    path = string.Format("{0}{1}{2}", Server.MapPath("~/log/"), year, month);
                    //如果目录不存在则创建 
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    System.IO.FileInfo file = new System.IO.FileInfo(string.Format("{0}/{1}", path, filename));
                    writer = new System.IO.StreamWriter(file.FullName, true);//文件不存在就创建,true表示追加 
                    writer.WriteLine(string.Format("用户IP:{0}", Request.UserHostAddress));
                    writer.WriteLine(errortime);
                    writer.WriteLine(erroraddr);
                    writer.WriteLine(errorinfo);
                    writer.WriteLine(errorsource);
                    writer.WriteLine(errortrace);
                    writer.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                }
                if (filterContext.Exception.GetType() == typeof(System.Net.WebException))
                {
                    var controllerName = (string)filterContext.RouteData.Values["controller"];
                    filterContext.Result = new RedirectResult(Request.RawUrl);
                }
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult { Data = "<h3 class='red'>错误提示：" + filterContext.Exception.Message + "</h3>", JsonRequestBehavior = JsonRequestBehavior.AllowGet };                
                }           
            }
            finally
            {
                base.OnException(filterContext);
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
