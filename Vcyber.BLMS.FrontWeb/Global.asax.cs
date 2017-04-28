using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Vcyber.BLMS.FrontWeb
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

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //遍历Post参数，隐藏域除外
            foreach (string i in this.Request.Form)
            {
                if (i == "__VIEWSTATE") continue;
                this.goErr2(this.Request.Form[i].ToString());
            }
            //遍历Get参数。

            foreach (string i in this.Request.QueryString)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    this.goErr2(this.Request.QueryString[i].ToString());
                }

            }

        }
        public bool SqlFilter(string InText)
        {
            string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join|cmd|;|'|--";//这里加要过滤的SQL字符
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 校验参数是否存在SQL字符
        /// </summary>
        /// <param name="tm"></param>
        private void goErr2(string tm)
        {
            if (SqlFilter(tm))
            {
                Response.Write("<script>window.alert('参数存在不安全字符');" + "</" + "script>");
            }
        }

        private void goErr(string p)
        {
            throw new NotImplementedException();
        }
    }
}
