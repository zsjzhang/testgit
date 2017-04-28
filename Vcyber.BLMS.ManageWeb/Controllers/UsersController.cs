using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;
using WebGrease.Css.Extensions;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class UsersController : Controller
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

        //
        // GET: /User/
        public ActionResult User()
        {
            return View();
        }

        public JsonResult UserList(int? start, int? count, string userName, string userRole)
        {
            var totalCount = 0;
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            var acccountList = userStore.GetUsers(start ?? 0, count ?? 0, userName, userRole, out totalCount).Result;
            var userModelList = new List<UserModel>();
            if (acccountList != null)
            {
                foreach (var item in acccountList)
                {
                    var manager = new UserModel();
                    manager.Id = item.Id;
                    manager.UserName = item.UserName;
                    manager.Email = item.Email;
                    manager.Phone = item.PhoneNumber;
                    manager.CreateTime = item.CreateTime;
                    //manager.LastLoginTime = item.LastLoginTime.ToShortDateString();
                    manager.RoleName = item.RoleName;
                    manager.Department = item.Department;
                    manager.Status = item.Status;
                    manager.StatusName = ((EManagerStatus)item.Status).GetDiscribe();


                    userModelList.Add(manager);
                }
            }

            var result = new { data = userModelList, pos = start ?? 0, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult AddUser()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult AddUser(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                ModelState.Values.Where(c => c.Errors.Count() > 0).Select(r => r.Errors).ForEach((e) =>
                {
                    if (e.FirstOrDefault() != null)
                        msg += e.FirstOrDefault().ErrorMessage + "\n";
                });
                return Json(new { success = false, msg = msg });
            }

            var user = new ApplicationUser();
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Department = model.Department;
            user.Status = model.Status;
            user.DealerId = model.DealerId;//4s店id
            user.DealerName = model.DealerName;//4s店名称
            var result = UserManager.Create(user, model.Password);
            if (!result.Succeeded)
            {
                var message = "";
                foreach (var error in result.Errors)
                {
                    message += error;
                }
                return Json(new { success = false, msg = message });
            }

            //添加默认权限
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            userStore.AddToRoleAsync(user, "4sTest");

            return Json(new { success = true, msg = "操作成功，新账号已添加！" });
        }

        //public ActionResult EditUser()
        //{
        //    return View();
        //}

        public JsonResult UserJsonResult(string id)
        {
            var user = UserManager.FindById(id);
            var managerModel = new UserModel();
            if (user != null)
            {
                managerModel.Id = id;
                managerModel.UserName = user.UserName;
                managerModel.Email = user.Email;
                managerModel.RoleName = user.RoleName;
                managerModel.Phone = user.PhoneNumber;
                managerModel.Department = user.Department;
                managerModel.Status = user.Status;
            }

            return Json(managerModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditUser(UserModel model)
        {
            var user = UserManager.FindById(model.Id);
            if (user == null)
            {
                return Json(
                   new
                   {
                       id = -4,
                       message = "不存在当前用户！"
                   }
             );
            }
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Department = model.Department;
            user.Status = model.Status;
            var result = UserManager.Update(user);


            //when the oldPwd is null or empty
            //if (user.UserName != model.UserName)
            //{
            //    var isExist = UserManager.FindByName(model.UserName);
            //    if (isExist!=null)
            //    {
            //        return Json(
            //            new
            //            {
            //                id = -1,
            //                message = "您输入的账户名已存在，请重新输入！"
            //            });
            //    }
            //}
            //var manager = new ApplicationUser();
            //manager.UserName = user.UserName;//用户名不能修改!
            //manager.Email = model.Email;
            //manager.Id = user.Id;
            //manager.PasswordHash = user.PasswordHash;
            //manager.SecurityStamp = user.SecurityStamp;
            //manager.EmailConfirmed = user.EmailConfirmed;
            //manager.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            //manager.AccessFailedCount = user.AccessFailedCount;
            //manager.LockoutEnabled = user.LockoutEnabled;
            //manager.LockoutEndDateUtc = user.LockoutEndDateUtc;
            //manager.TwoFactorEnabled = user.TwoFactorEnabled;
            //manager.PhoneNumber = model.Phone;
            //manager.Department = model.Department;
            //manager.Status = model.Status;
            //var result = UserManager.Update(manager);
            if (!result.Succeeded)
            {
                return Json(result.Errors.ToString());
            }

            return Json("修改成功");
        }

        [HttpPost]
        public bool DelUser(string Id)
        {
            var user = UserManager.FindById(Id);
            var result = UserManager.Delete(user);
            return result.Succeeded;
        }

        [HttpPost]
        public ActionResult ResetPw(string userId, ResetPWViewModel pwModel)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    if (value.Errors != null && value.Errors.Count > 0)
                    {
                        foreach (var error in value.Errors)
                        {

                            message += error.ErrorMessage + ".";
                        }
                    }
                }
                return Json(message);
            }
            var user = UserManager.FindById(userId);
            if (user != null)
            {
                var result = UserManager.ChangePassword(userId, user.PasswordHash, pwModel.Password);
                if (result.Succeeded)
                {
                    return Json(message = "重置密码成功！");
                }

                foreach (var error in result.Errors)
                {
                    message += error;
                }
            }
            return Json(message);
        }

    }
}