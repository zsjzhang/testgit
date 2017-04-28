using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.ManageWeb.Models;
using Vcyber.BLMS.Application;
using System.Configuration;
using System.Web.Security;

using Vcyber.BLMS.Common;
using System.Security.Cryptography;
using System.Text;


namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class AppManageController : Controller
    {
        public ActionResult Index()
        {
            string id = HttpContext.User.Identity.GetUserId();
            string salerid = HttpContext.User.Identity.Name;
            string key = salerid + ConfigurationManager.AppSettings["Appkey"].ToString();

            MD5 md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            string md5b = BitConverter.ToString(result).ToLower();
            md5b=md5b.Replace("-","");
            string _url = ConfigurationManager.AppSettings["Appurl"].ToString() + "?salerid=" + salerid + "&vc=" + md5b;
            ViewBag.url = _url;
            return View();
        }


     

           


    }
}