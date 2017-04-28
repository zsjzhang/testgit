using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Vcyber.BLMS.WebApi.Filter
{
    /// <summary>
    /// 自定义权限验证
    /// </summary>
    public class IOVAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string scopeClaimType = "urn:oauth:scope";

        private string[] scopes;

        /// <summary>
        /// 权限范围值
        /// </summary>
        public string[] Scopes
        {
            get { return scopes; }
        }

        public IOVAuthorizeAttribute(params string[] scopes)
        {
            if (scopes == null || scopes.Count() == 0)
                this.scopes = new string[] { "common" ,"basic"};
            else
                this.scopes = scopes;
        }

        /// <summary>
        /// 覆盖权限验证方法
        /// </summary>
        /// <param name="actionContext">请求上下文</param>
        /// <returns>是否验证通过</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IEnumerable<string> sourceKeys = null;
            string sourceKey = string.Empty;
            bool isAuthorized = false;
            if (actionContext.Request.Headers.TryGetValues("appkey", out sourceKeys))
            {
                sourceKey = sourceKeys.First();
            }
            //如果来自bluemember微信端，就不验证
            if (sourceKey.Contains("blms_wechat"))
            {
                isAuthorized = true;
            }
            else
            {
                //如果来自其他客户端需要验证
                isAuthorized = base.IsAuthorized(actionContext);
                if (isAuthorized)
                {
                    //账号是否冻结
                    var userid = actionContext.RequestContext.Principal.Identity.Name;
                    var manager = new UserManager<FrontIdentityUser>(new FrontUserStore<FrontIdentityUser>());
                    var user = manager.FindById(userid);
                    if (user != null && user.Status == 2)
                    {
                        actionContext.ModelState.AddModelError("Freezon", "账户被冻结");
                        return false;
                    }
                    else if (user != null && user.IsNeedModifyPw == 1)
                    {
                        actionContext.ModelState.AddModelError("NeedModifyPassword", "需要修改初始密码");
                        return false;
                    }
                    //当前具有的Scope值
                    var grantedScopes = ClaimsPrincipal.Current.FindAll(scopeClaimType).Select(c => c.Value).ToList();
                    foreach (string s in scopes)
                    {
                        if (grantedScopes.Contains(s))
                        {
                            return isAuthorized;
                        }
                    }
                    isAuthorized = false;
                }            
            }
            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            if (actionContext.ModelState.Where(e => e.Key == "Freezon").Any())
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
            else if (actionContext.ModelState.Where(e => e.Key == "NeedModifyPassword").Any())
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable);
            }
        }
    }
}