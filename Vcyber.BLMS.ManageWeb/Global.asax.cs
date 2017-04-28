using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Vcyber.BLMS.ManageWeb;

namespace Vcyber.BLMS.ManageWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            Vcyber.BLMS.Common.LogService.Instance.LoadConfig();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

      
        public override void Init()
        {
            this.PreSendRequestHeaders += MvcApplication_PreSendRequestHeaders;
            base.Init();
        }

        void MvcApplication_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Set("Server", "microsoft-iis/6.0");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}
