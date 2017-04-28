
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb
{
    public static class ControllerExtends
    {
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Entity.UserLoginInfo GetLogin(this Controller instance)
        {
            ClaimsIdentity identityData = instance.HttpContext.User.Identity as ClaimsIdentity;
            string userGuid = identityData.GetUserId();

            var userData = _AppContext.UserInfoApp.GetOne(userGuid);

            if (userData!=null)
            {
                Entity.UserLoginInfo loginData = new Entity.UserLoginInfo();
                loginData.UserName = userData.UserName;
                loginData.UserId = userData.Id;
                loginData.UserGuid = userGuid;
                loginData.PhoneNumber = userData.PhoneNumber;
                loginData.Email = userData.Email;
                loginData.MallCode = ConfigurationManager.AppSettings["CYTCode"];
                return loginData;
            }
            else
            {
#warning ==== 葵花宝典逻辑 ====
            }

            return new Entity.UserLoginInfo() { UserGuid=string.Empty,UserId=0};
        }

    }
}