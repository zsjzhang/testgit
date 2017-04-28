using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.ManageWeb.Filter;

namespace Vcyber.BLMS.ManageWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandleErrorAttribute());
            //filters.Add(new MvcAuthorizeAttribute()); 
        }
    }
}
