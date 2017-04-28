using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Helper
{
    using System.Web.Mvc;

    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    public class UserHelper:Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public string GetDealerId()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user.DealerId;
        }
        public bool IsDealer(ApplicationUserManager a)
        {
            var user = a.FindById(User.Identity.GetUserId());
            return user.DealerId!=null;
        }
    }
}