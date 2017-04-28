using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Filter
{
    public class ExceptionHandleErrorAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            LogService.Instance.Error(filterContext.Exception.Message + "\r\n" + filterContext.Exception.StackTrace);
            LogService.Instance.Info(string.Format("Controller Name:{0}, Action Name: {1}", controllerName, actionName));
            //base.OnException(filterContext);
        }
    }
}