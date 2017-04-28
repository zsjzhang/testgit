using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb
{
    /// <summary
    /// 授权验证
    /// </summary>
    public class ValidateRole
    {
        //private string accessUrl;
       
        ///// <summary>
        ///// 当前访问Url
        ///// </summary>
        //public string AccessUrl
        //{
        //    get { return this.accessUrl; }
        //}

        private string action;
        public string Action
        {
            get { return action; }
        }

        private string controller;
        public string Controller
        {
            get { return controller; }
        }

        public ValidateRole(string action, string controller)
        {
            this.action = action;
            this.controller = controller;
        }

        /// <summary>
        /// 是否有权限访问页面
        /// </summary>
        /// <returns>true:授权通过</returns>
        public bool IsAuthorize(IList<Claim> roles)
        {
            if(roles == null || roles.Count == 0)
            {
                return false;
            }

            var roleFunc= _AppContext.FunctionApp.GeRoleFuncsByRoleName(roles.Select(c => c.Value).ToList<string>()).Where(p => p.Action == this.action && p.Controller == this.controller);
            if (roleFunc.Count() > 0)
                return true;
            else
                return false;

            //List<string> roleList = new List<string>();
            //foreach (var role in roles)
            //{
            //    roleList.Add(role.Value);
            //}
            //IEnumerable<string> functionList = _AppContext.FunctionApp.GeRoleFuncsByRoleName(roleList);

            //string accesssUrl = this.accessUrl;
            //if (string.IsNullOrEmpty(accesssUrl))
            //{
            //    return false;
            //}

            //accesssUrl = accesssUrl.ToLower();

            //foreach (var function in functionList)
            //{
            //    if (string.IsNullOrEmpty(function))
            //    {
            //        continue;
            //    }
            //    if (function.ToLower().Equals(accesssUrl))
            //    {
            //        return true;
            //    }
            //}
            //return false;

        }        
    }
}