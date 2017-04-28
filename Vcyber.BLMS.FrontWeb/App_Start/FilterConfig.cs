using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.FrontWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
