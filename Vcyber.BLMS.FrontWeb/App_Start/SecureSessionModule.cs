using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Vcyber.BLMS.FrontWeb
{
    public class SecureSessionModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = this.GetCookie(HttpContext.Current.Response.Cookies);

            if (cookie != null)
            {
                cookie.Secure = true;
                string mac = this.GenerateMac(cookie.Value, HttpContext.Current.Request);
                cookie.Value += mac;
            }         
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpCookie cookie = this.GetCookie(request.Cookies);

            if (cookie != null)
            {
                string value = cookie.Value;
                if (!String.IsNullOrEmpty(value))
                {
                    string sessionID = value.Substring(0, 24);
                    string clientMac = value.Substring(24);

                    string serverMac = this.GenerateMac(sessionID, request);

                    if (!clientMac.Equals(serverMac, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new SecurityException(string.Format("非法访问!v={3},sid={0},s={1},c={2}"
                            ,sessionID,serverMac
                            ,clientMac
                            ,value));
                    }

                    cookie.Value = sessionID;
                }
            }
        }

        private HttpCookie GetCookie(HttpCookieCollection cookies)
        {
            for (int i = 0; i < cookies.Count; i++)
            {
                HttpCookie cookie = cookies[i];
                if (cookie.Name.Equals("ASP.NET_SessionId", StringComparison.OrdinalIgnoreCase))
                {
                    return cookie;
                }
            }
            return null;
        }

        private string GenerateMac(string sessionID, HttpRequest request)
        {
            string content = String.Format(CultureInfo.InvariantCulture, "{0}|{1}", request.UserHostAddress, request.UserAgent);

            byte[] key = Encoding.UTF8.GetBytes("Any server side secret...");

            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
                return Convert.ToBase64String(hash);
            }
        }


    }
}