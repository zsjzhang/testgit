using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models;
using System.Security.Principal;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.WebApi
{
    public partial class Startup
    {
        private ClientConfig _clientConfig;

        private ClientConfig ClientConfig
        {
            get
            {
                if (_clientConfig == null)
                    _clientConfig = (ClientConfig)XmlFileHelper.LoadXml(HttpRuntime.AppDomainAppPath +
                        "App_Data\\ClientConfig.xml", typeof(ClientConfig));
                return _clientConfig;
            }
        }

        private int accessTokenExpireTimeSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["accessTokenExpireTimeSpan"]);
        private bool allowInsecureHttp = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["allowInsecureHttp"]);

        //-------------------------------------------
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(new Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationOptions());

            #region
            // Enable Application Sign In Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Application",
                AuthenticationMode = AuthenticationMode.Passive,
                LoginPath = new PathString(Paths.LoginPath),
                LogoutPath = new PathString(Paths.LogoutPath),
            });

            // Enable External Sign In Cookie
            app.SetDefaultSignInAsAuthenticationType("External");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "External",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "External",
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            });

            // Enable google authentication
            //app.UseGoogleAuthentication();
            #endregion

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                TokenEndpointPath = new PathString(Paths.TokenPath),
                AccessTokenExpireTimeSpan = new TimeSpan(0, 0, accessTokenExpireTimeSpan), //AccessToken 过期时间，目前暂定24小时（86400秒）
                ApplicationCanDisplayErrors = true,
                //#if DEBUG
                //是否启用ssl
                AllowInsecureHttp = allowInsecureHttp,
                //#endif
                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientRedirectUri = ValidateClientRedirectUri,
                    OnValidateClientAuthentication = ValidateClientAuthentication,
                    OnGrantResourceOwnerCredentials = GrantResourceOwnerCredentials,
                    OnGrantClientCredentials = GrantClientCredetails,
                    OnGrantRefreshToken = GrantRefreshToken
                },

                // Authorization code provider which creates and receives authorization code
                AuthorizationCodeProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateAuthenticationCode,
                    OnReceive = ReceiveAuthenticationCode,
                },

                // Refresh token provider which creates and receives referesh token
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateRefreshToken,
                    OnReceive = ReceiveRefreshToken,
                }
            });
        }
        /// <summary>
        /// 验证config的redirecturl是否相同
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            foreach (Client c in ClientConfig.Clients)
            {
                if (context.ClientId == c.Id)
                {
                    context.Validated(c.RedirectUrl);
                    break;
                }
            }
            return Task.FromResult(0);
        }
        /// <summary>
        /// 验证客户端
        /// </summary>
        private Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            bool isValidated = false;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                foreach (Client c in ClientConfig.Clients)
                {
                    if (clientId == c.Id && clientSecret == c.Secret)
                    {
                        context.Validated();
                        isValidated = true;
                        break;
                    }
                }
            }
            if (!isValidated)
                context.SetError("客户端验证错误");
            return Task.FromResult(0);
        }
        /// <summary>
        /// 用户名和密码获取令牌
        /// </summary>
        private Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //登录名:"ceshi";手机号码:"m:13812345678";电子邮箱:"e:ceshi@vcyber.cn"
            string userName = context.UserName;
            string passWord = context.Password;
            if (string.IsNullOrEmpty(userName))
            {
                context.SetError("用户名为空");                
                return Task.FromResult(0);
            }
            if (string.IsNullOrEmpty(passWord))
            {
                context.SetError("密码为空");                
                return Task.FromResult(0);
            }
            var account = GetAccountByUserName(userName);
            if (account == null)
            {
                context.SetError("不存在此账户");                
                return Task.FromResult(0);
            }
            else if (account.Status == (int)MembershipStatus.Freezon)
            {
                context.SetError("此账户已冻结");                
                return Task.FromResult(0);
            }
            else
            {
                if (account.IsNeedModifyPw == 1)
                {
                    context.SetError("406");//提示修改初始密码                    
                    return Task.FromResult(0);
                }

                var userManager = new UserManager<FrontIdentityUser>(new FrontUserStore<FrontIdentityUser>());

                if (account.IsNeedModifyPw == 0 && !userManager.CheckPasswordAsync(account, passWord).Result && _AppContext.UserWxBindApp.GetUserNameByOpenId(passWord)!=userName)
                {
                    context.SetError("密码错误");                    
                    return Task.FromResult(0);
                }
                else
                {
                    if (!ScopeValidation(context.Scope, new List<string>() { "common" })) //目前用户的Scope值均为"common" 
                    {
                        context.SetError("客户端请求的Scope超出定义。");                        
                        return Task.FromResult(0);
                    }
                    else
                    {
                        try
                        {                            
                            int outValue;
                            //_AppContext.BreadApp.BlueBeanBread(
                            //    EBRuleType.登陆,
                            //    account.Id,
                            //    (MemshipLevel)account.MLevel,
                            //    out outValue);

                            //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, account.Id, out outValue);
                            _AppContext.LoginMemRecordApp.Add(account.Id, account.NickName, this.GetSysCode(context));

                            //绑定微信和APP送蓝豆和经验值
                            if (this.GetSysCode(context) != EDataSource.blms)
                            {
                                //if (this.GetSysCode(context) == EDataSource.blms_web)
                                //{
                                //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.客户端软件, account.Id, (MemshipLevel)account.MLevel, out outValue);
                                //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.下载BM客户端软件, account.Id, out outValue);
                                //}
                                //else
                                //{
                                //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.微信绑定账号, account.Id, (MemshipLevel)account.MLevel, out outValue);
                                //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.微信绑定账号, account.Id, out outValue);
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            LogService.Instance.Error("增加蓝豆失败: " + account.Id);
                            LogService.Instance.Error(ex.Message, ex);
                        }

                        var identity = new ClaimsIdentity(new GenericIdentity(account.Id, OAuthDefaults.AuthenticationType),
                            context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
                        context.Validated(identity);
                    }
                }
            }
            return Task.FromResult(0);
        }
        /// <summary>
        /// 通过id发放令牌
        /// </summary>
        private Task GrantClientCredetails(OAuthGrantClientCredentialsContext context)
        {
            var client = this.ClientConfig.Clients.FirstOrDefault(i => i.Id == context.ClientId);
            if (client == null)
            {
                context.SetError("客户端不存在。");
                return Task.FromResult(0);
            }
            if (!ScopeValidation(context.Scope, client.ClientScope))
            {
                context.SetError("客户端请求的Scope超出定义。");
                return Task.FromResult(0);
            }
            else
            {
                var identity = new ClaimsIdentity(new GenericIdentity(context.ClientId, "Client"),//原值：OAuthDefaults.AuthenticationType 
                    context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
                context.Validated(identity);
                return Task.FromResult(0);
            }
        }


        private readonly ConcurrentDictionary<string, string> _authenticationCodes =
            new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// 生成Token
        /// </summary>        
        private void CreateAuthenticationCode(AuthenticationTokenCreateContext context)
        {
            context.SetToken(Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"));
            _authenticationCodes[context.Token] = context.SerializeTicket();
        }

        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="context"></param>
        private void ReceiveAuthenticationCode(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_authenticationCodes.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="context"></param>
        private void CreateRefreshToken(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        /// <summary>
        /// 移除刷新Token
        /// </summary>
        /// <param name="context"></param>
        private void ReceiveRefreshToken(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
            //修改过期时间和发布时间
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddSeconds(accessTokenExpireTimeSpan));
            context.Ticket.Properties.IssuedUtc = new DateTimeOffset(DateTime.UtcNow);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        private FrontIdentityUser GetAccountByUserName(string userName)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = userStore.FindByNameAsync(userName).Result;
            if (user == null)
                user = userStore.FindByEmailAsync(userName).Result;
            if (user == null)
                user = userStore.FindByNickNameAsync(userName).Result;

            return user;
            //登录名:"ceshi";手机号码:"m:13812345678";电子邮箱:"e:ceshi@vcyber.cn"
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        private FrontIdentityUser GetAccountById(string userId)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            return userStore.FindByIdAsync(userId).Result;
        }

        private bool ScopeValidation(IEnumerable<string> requestScopes, IEnumerable<string> scopes)
        {
            bool isContains = true;
            foreach (var s in requestScopes)
            {
                if (!scopes.Contains(s))
                {
                    isContains = false;
                    break;
                }
            }
            return isContains;
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        private Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            if (context.Ticket.Identity.AuthenticationType == OAuthDefaults.AuthenticationType)
            {
                string accountId = context.Ticket.Identity.GetUserId();
                var account = this.GetAccountById(accountId);
                if (account == null)
                {
                    context.SetError("用户不存在。");
                    return Task.FromResult(0);
                }
                if (account.Status == (int)MembershipStatus.Freezon)
                {
                    context.SetError("用户状态异常。");
                    return Task.FromResult(0);
                }

            }
            context.Validated();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 获取请求设备的key
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private EDataSource GetSysCode(OAuthGrantResourceOwnerCredentialsContext instance)
        {
            string value = string.Empty;

            try
            {
                var values = instance.Request.Headers.GetValues("appkey");

                if (values != null)
                {
                    foreach (var item in values)
                    {
                        value = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                value = "blms";
            }

            if (EDataSource.blms.ToString().Equals(value))
            {
                return EDataSource.blms;
            }

            if (EDataSource.blms_web.ToString().Equals(value))
            {
                return EDataSource.blms_web;
            }

            if (EDataSource.blms_wechat.ToString().Equals(value))
            {
                return EDataSource.blms_wechat;
            }

            return EDataSource.blms;
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /*
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }*/
    }

    public static class Paths
    {
        #region
        /// <summary>
        /// AuthorizationServer project should run on this URL
        /// </summary>
        //public const string AuthorizationServerBaseAddress = "http://localhost:6023";

        /// <summary>
        /// ResourceServer project should run on this URL
        /// </summary>
        //public const string ResourceServerBaseAddress = "http://localhost:38385";

        /// <summary>
        /// ImplicitGrant project should be running on this specific port '38515'
        /// </summary>
        //public const string ImplicitGrantCallBackPath = "http://localhost:38515/Home/SignIn";

        /// <summary>
        /// AuthorizationCodeGrant project should be running on this URL.
        /// </summary>
        //public const string AuthorizeCodeCallBackPath = "http://localhost:38500/";
        #endregion

        public const string AuthorizePath = "/OAuth/Authorize";
        public const string TokenPath = "/OAuth/Token";
        public const string LoginPath = "/Account/Login";
        public const string LogoutPath = "/Account/Logout";
    }
}
