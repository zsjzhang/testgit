using System.Web;
using System.Web.Mvc;
using log4net;
using System;
namespace Vcyber.BLMS.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionLogAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
   
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var name = filterContext.Exception.GetType().Name;
            ComomLog.Log(name, filterContext.Exception);
            base.OnException(filterContext);
        }

      

    }
     //[assembly: log4net.Config.XmlConfigurator(Watch = true)]
    public  class ComomLog
    {
         public static void Log(string name, Exception ex)
        {
            ILog log = log4net.LogManager.GetLogger(name);
            log.Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// 异常捕获筛选器
    /// </summary>
    public class WebApiExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常方法
        /// </summary>
        /// <param name="actionExecutedContext">action exception</param>
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            string message = string.Empty;
            log4net.LogManager.GetLogger("bm-web-api-controller").Error(message,actionExecutedContext.Exception);
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            base.OnException(actionExecutedContext);
        }
    }
}
