using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vcyber.BLMS.FrontWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
           name: "zhihuan",
           url: "zhihuan",
           defaults: new { controller = "OrderChange", action = "Index" }
          );

            routes.MapRoute(
               name: "yuena",
               url: "verna-yuena/{action}/{id}",
               defaults: new { controller = "Yuena", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "Vcyber.BLMS.FrontWeb.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}
