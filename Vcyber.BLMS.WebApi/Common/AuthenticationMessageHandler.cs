namespace Vcyber.BLMS.WebApi.Common
{
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using Vcyber.BLMS.WebApi.Common;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;

    public class AuthenticationMessageHandler : DelegatingHandler
    {
        //readonly IEngineeService engineeService = AppServiceLocator.Instance.GetInstance<IEngineeService>();
        /// <summary>
        /// Sends the async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string clientInfo = request.GetHeader("clientinfo")??string.Empty;
            string appkey = request.GetHeader("appkey");
            log4net.LogManager.GetLogger("Header验证:").Info(clientInfo);
            log4net.LogManager.GetLogger("Header验证:").Info(appkey);
            if (string.IsNullOrEmpty(appkey) || string.IsNullOrEmpty(clientInfo))
            {
                var obj = new ReturnObject("412", "appkey or clientinfo is missing");
                log4net.LogManager.GetLogger("Header验证:").Info("返回400");
                var response = request.CreateResponse(HttpStatusCode.BadRequest, obj);
                return Task.FromResult(response);
            }

            clientInfo = appkey + "|" + clientInfo;
            string ip = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            string loginfo = ip + " | " + request.RequestUri + " | " + request.Method + " | " + clientInfo + " | "
            + request.Content.ReadAsStringAsync().Result;
            LogService.Instance.Info(loginfo);

            var principal = this.CreateIPrincipal(request);

            this.SetPrincipal(principal);

            var result = base.SendAsync(request, cancellationToken);
            return result;
        }

        /// <summary>
        /// Generate IPrincipal
        /// </summary>
        /// <returns>IPrincipal</returns>
        private IPrincipal CreateIPrincipal(HttpRequestMessage request)
        {
            string ticket = request.GetHeader("ticket");
            if (string.IsNullOrEmpty(ticket)) return null;

            UserState stateEntity = _AppContext.UserStateApp.Get(ticket);
            if (stateEntity == null) return null;
            
            UserStateEntity userEntity = stateEntity.Entity;
            if (userEntity == null) return null;
            
            GenericIdentity identity = new GenericIdentity(userEntity.UserId.ToString());
            string[] userRoles = { "common" };
            return new GenericPrincipal(identity, userRoles);
        }


        /// <summary>
        /// Set pricipal 
        /// </summary>
        /// <param name="principal">The principal.</param>
        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}