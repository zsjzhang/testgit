using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Vcyber.BLMS.WebApi
{
    using Vcyber.BLMS.Common;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Vcyber.BLMS.Common.LogService.Instance.LoadConfig();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            string error = string.Empty;
            string errortime = string.Empty;
            string erroraddr = string.Empty;
            string errorinfo = string.Empty;
            string errorsource = string.Empty;
            string errortrace = string.Empty;

            error += string.Format("发生时间:{0}", DateTime.Now.ToString());
            errortime = string.Format("发生时间:{0}", DateTime.Now.ToString());

            error += string.Format("发生异常页: {0}", Request.Url.ToString());
            erroraddr = string.Format("发生异常页: {0}", Request.Url.ToString());

            error += string.Format("异常信息: {0}", objErr.Message);
            errorinfo = string.Format("异常信息: {0}", objErr.Message);

            errorsource = string.Format("错误源:{0}", objErr.Source);
            errortrace = string.Format("堆栈信息:{0}", objErr.StackTrace);
            error += "--------------------------------------";
            Server.ClearError();
            Application["error"] = error;
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
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
