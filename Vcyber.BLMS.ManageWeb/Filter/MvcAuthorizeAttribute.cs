using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Vcyber.BLMS.ManageWeb
{

    /// <summary>
    /// 不启用压缩特性
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    //public class NoCompress : Attribute
    //{
    //    public NoCompress()
    //    {
    //    }
    //}

    /// <summary>
    /// 权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        #region ==== 私有字段 ====

        private static readonly string loginUrl = "~/Account/Login";

        #endregion

        #region ==== 公共方法 ====

      
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            base.AuthorizeCore(httpContext);
            HttpRequestBase request = httpContext.Request;
            HttpResponseBase response = httpContext.Response;

            if (httpContext.User.Identity.IsAuthenticated && httpContext.Response.StatusCode != (int)HttpStatusCode.Unauthorized)
            {
                return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.ActionDescriptor.ActionName.ToLower() == "index" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() == "home")
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(loginUrl);
            }
            else
            { 
                filterContext.RequestContext.HttpContext.Response.Write("<script>alert('操作受限！请联系管理员！');history.back();</Script>");
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //bool isActionAllowAnonymous = false;
            //var attrs = filterContext.ActionDescriptor.GetCustomAttributes(true);
            //attrs.All((p) =>
            //{
            //    if (p is System.Web.Mvc.AllowAnonymousAttribute)
            //        isActionAllowAnonymous = true;
            //    return true;
            //});
            //if (isActionAllowAnonymous)
            //    return;
            HttpResponseBase response = filterContext.HttpContext.Response;
            HttpRequestBase request = filterContext.RequestContext.HttpContext.Request;

            ClaimsIdentity claim = filterContext.HttpContext.User.Identity as ClaimsIdentity; 
            IList<Claim> roles = claim.Claims.Where(c => c.Type == ClaimTypes.Role).ToList<Claim>();

            ValidateRole validateRole = new ValidateRole(filterContext.ActionDescriptor.ActionName,filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            bool result = validateRole.IsAuthorize(roles);

            if (!result)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }

            base.OnAuthorization(filterContext);

            //switch ((HttpStatusCode)response.StatusCode)
            //{
            //    case HttpStatusCode.Redirect: response.Redirect(loginUrl); break;
            //    case HttpStatusCode.Unauthorized:
            //        {
            //            string method = request.HttpMethod;

            //            if (method.Equals("GET"))
            //            {
            //                response.Write("<script>alert('操作受限！');history.back();</Script>");
            //            }
            //            else
            //            {
            //                if (method.Equals("POST"))
            //                {
            //                    response.Write(HttpStatusCode.Unauthorized.ToString());
            //                }
            //            }

            //            break;
            //        }
            //    default: break;
            //}
        }

        #endregion
    }
}