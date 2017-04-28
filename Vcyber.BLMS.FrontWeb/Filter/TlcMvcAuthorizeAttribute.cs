using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.FrontWeb
{
    /// <summary>
    /// 权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TlcMvcAuthorizeAttribute : AuthorizeAttribute
    {
        #region ==== 公共方法 ====


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;
            var userIdentity = userStore.FindByIdAsync(httpContext.User.Identity.GetUserId()).Result;

            if (httpContext.User.Identity.IsAuthenticated && userIdentity.Status == (int)MembershipStatus.Freezon)
            {
                return false;
            }

            if (userIdentity.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.Tlc) && userIdentity.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.TOP))
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.ActionDescriptor.GetParameters().Count() > 0 && filterContext.Controller.ValueProvider.GetValue("source") != null)
            {
                string returnUrl = string.Format("?returnUrl=/{0}/{1}?{2}={3}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.GetParameters()[0].ParameterName, filterContext.Controller.ValueProvider.GetValue("source").AttemptedValue);
                
                filterContext.HttpContext.Response.Redirect("/Sonata/Index" + returnUrl);
            }
            else
            {
                string returnUrl = string.Format("?returnUrl=/{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
                
                filterContext.HttpContext.Response.Redirect("/Sonata/Index" + returnUrl);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        #endregion
    }
}