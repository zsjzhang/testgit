using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class UserRolesController : Controller
    {
        public UserRolesController()
        {
        }

        public UserRolesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToRole(string userId)
        {
            var userroleModel = new UserRoleModel();
            userroleModel.UserId = userId;

            userroleModel.UserName = UserManager.FindById(userId).UserName;
            userroleModel.Roles = new List<UserRole>();
            var allroles = RoleManager.Roles;
            var userroles = UserManager.GetRoles(userId);
            foreach (var item in allroles)
            {
                var role = new UserRole();
                role.Id = item.Id;
                role.Name = item.Name;
                if (userroles.FirstOrDefault(q => q == role.Name) != null)
                {
                    role.IsChecked = true;
                }
                userroleModel.Roles.Add(role);
            }
            return Json(userroleModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToRole(UserRoleModel userRoles)
        {
            string message = string.Empty;
            bool deleteResult = true;
            bool addResult = true;
            var userroles = UserManager.GetRoles(userRoles.UserId);
            var userAddRoles = userRoles.Roles.Where(r => r.IsChecked == true).Select(c => c.Name).ToArray();
            if (userroles != null && userroles.Count() > 0)
            {
                var result = UserManager.RemoveFromRoles(userRoles.UserId, userroles.ToArray());
                if (!result.Succeeded)
                {
                    deleteResult = false;
                    message += result.Errors.ToArray().ToString();
                }
            }
            if (userAddRoles != null && userAddRoles.Count() > 0)
            {
                var result=UserManager.AddToRoles(userRoles.UserId, userAddRoles);
                if (!result.Succeeded)
                {
                    addResult = false;
                    message += result.Errors.ToString();
                }
            }
            if (deleteResult & addResult)
                return Json(new { success = true });
            else
                return Json(new { success = false, msg = message });
        }
    }
}