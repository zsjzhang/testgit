using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.ManageWeb.Models;
using Vcyber.BLMS.Common;
namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class MenuController : Controller
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

        private IEnumerable<Function> allfuns;

        [AllowAnonymous]
        public JsonResult GetMenus()
        {
            LogService.Instance.Info("加载菜单");
            ClaimsIdentity claim = this.User.Identity as ClaimsIdentity;
            IList<Claim> roles = claim.Claims.Where(c => c.Type == ClaimTypes.Role).ToList<Claim>();

            var allFuncList = new List<Function>();

            var funList = _AppContext.FunctionApp.AllFunc(User.Identity.GetUserId());

            allfuns = _AppContext.FunctionApp.AllParnetFun();//

            foreach (var i in funList)
            {
                if (i.ParentId == 0)
                {
                    var xid = allFuncList.FirstOrDefault(q => q.Id == i.Id);
                    if (xid == null)
                    {
                        allFuncList.Add(i);
                    }

                    continue;
                }
                //var rootFunc = GetRootFunByFunId(i);
                var rootFunc = GetRootFunByFunId_M(i);
                if (rootFunc == null)
                {
                    continue;
                }
                var id = allFuncList.FirstOrDefault(q => q.Id == rootFunc.Id);
                if (id == null)
                {
                    allFuncList.Add(rootFunc);
                }
            }

            var menuList = new MenuViewModel();

            var menuData = new List<MenuItemViewModel>();
            foreach (var item in allFuncList)
            {
                var menuModel = new MenuItemViewModel();
                menuModel.id = item.Id;
                menuModel.value = item.Name;
                menuModel.href = string.Format(item.RouteSelection == "0" ? "/#/{0}/{1}" : "/{0}/{1}", item.Controller, item.Action);
                menuData.Add(menuModel);
                menuList.Menus = menuData;
            }
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取一级菜单
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult GetTopMenus()
        {
            ClaimsIdentity claim = this.User.Identity as ClaimsIdentity;
            IList<Claim> roles = claim.Claims.Where(c => c.Type == ClaimTypes.Role).ToList<Claim>();
            var roleListStr = roles.Select(e => e.Value).ToList<string>();
            var topMenus = _AppContext.FunctionApp.GetTopMenus(roleListStr);

            var menuList = new List<MenuItemViewModel>();
            foreach (var item in topMenus)
            {
                var menuModel = new MenuItemViewModel();
                menuModel.id = item.Id;
                menuModel.value = item.Name;
                menuModel.href = string.Format(item.RouteSelection == "0" ? "/#/{0}/{1}" : "/{0}/{1}", item.Controller, item.Action);
                menuList.Add(menuModel);
            }
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult TopMenu()
        {
            string action = "";
            string controller = "";
            if (this.Request.Url != null)
            {
                var url = this.Request.Url.LocalPath;
                var list = url.Split('/');
                action = list.Count() > 2 ? list[2] : list[1];
                controller = list[1];
            }


            var allFuncList = new List<Function>();

            var funList = _AppContext.FunctionApp.AllFunc(User.Identity.GetUserId());

            allfuns = _AppContext.FunctionApp.AllParnetFun();//

            foreach (var i in funList)
            {
                if (i.ParentId == 0)
                {
                    if (allFuncList.Count(g => g.Id == i.Id) > 0)
                    {
                        continue;
                    }
                    allFuncList.Add(i);
                    continue;
                    
                }
                //var rootFunc = GetRootFunByFunId(i);
                var rootFunc = GetRootFunByFunId_M(i);
                if (rootFunc == null)
                {
                    continue;
                }
                var id = allFuncList.FirstOrDefault(q => q.Id == rootFunc.Id);
                if (id == null)
                {
                    allFuncList.Add(rootFunc);
                }
            }

            // var funList = functionApp.RootFun(currentUser.Id);
            var menuList = new MenuViewModel();

            var menuData = new List<MenuItemViewModel>();

            if (allFuncList != null)
            {
                var urlFunList = _AppContext.FunctionApp.GetList(action, controller);
                var rootFun = urlFunList.FirstOrDefault(q => q.ParentId == 0);
                foreach (var item in allFuncList)
                {
                    var menuModel = new MenuItemViewModel();
                    menuModel.id = item.Id;
                    menuModel.value = item.Name;
                    menuModel.href = string.Format(item.RouteSelection == "0" ? "/#/{0}/{1}" : "/{0}/{1}", item.Controller, item.Action);

                    if (rootFun != null && rootFun.Id == item.Id)
                    {
                        menuModel.isDefault = true;
                    }
                    menuData.Add(menuModel);
                    menuList.Menus = menuData;
                }
            }

            return View(menuList);
        }

        [AllowAnonymous]
        public ActionResult GetLeftMenu(int parentId)
        {
            var childFun = _AppContext.FunctionApp.ChildFun(parentId);
            if (childFun != null)
            {
                childFun = childFun.OrderBy(q => q.Rate).ToList();
            }

            var allFunList = _AppContext.FunctionApp.AllFunc(User.Identity.GetUserId());
            var funList = Get2LMenuByParentAndUserId(childFun, User.Identity.GetUserId(), allFunList);

            var menuList = new MenuViewModel();
            var menuData = new List<MenuItemViewModel>();
            if (funList != null)
            {
                foreach (var item in funList)
                {

                    var menuModel = new MenuItemViewModel();
                    menuModel.id = item.Id;
                    menuModel.value = item.Name;
                    menuModel.href = string.Format(item.RouteSelection == "0" ? "/#/{0}/{1}" : "/{0}/{1}", item.Controller, item.Action);

                    menuData.Add(menuModel);
                    menuList.Menus = menuData;

                }
            }

            return Json(menuList, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult LeftMenu(int parentId)
        {
            var childFun = _AppContext.FunctionApp.ChildFun(parentId);
            if (childFun != null)
            {
                childFun = childFun.OrderBy(q => q.Rate).ToList();
            }

            var allFunList = _AppContext.FunctionApp.AllFunc(User.Identity.GetUserId());
            var funList = Get2LMenuByParentAndUserId(childFun, User.Identity.GetUserId(), allFunList);

            var menuList = new MenuViewModel();
            var menuData = new List<MenuItemViewModel>();
            if (funList != null)
            {
                foreach (var item in funList)
                {

                    var menuModel = new MenuItemViewModel();
                    menuModel.id = item.Id;
                    menuModel.value = item.Name;
                    menuModel.href = string.Format(item.RouteSelection == "0" ? "/#/{0}/{1}" : "/{0}/{1}", item.Controller, item.Action);

                    menuData.Add(menuModel);
                    menuList.Menus = menuData;

                }
            }

            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetFunByActionAndController(string location)
        {

            var fun = new Function();
            if (string.IsNullOrEmpty(location))
            {
                return Json(fun, JsonRequestBehavior.AllowGet);

            }
            var loc = location.Split('/');
            if (!string.IsNullOrEmpty(loc[2]) && !string.IsNullOrEmpty(loc[1]))
            {
                fun = _AppContext.FunctionApp.GetFunByActionAndFunction(loc[2], loc[1]);
            }
            fun = GetRootFunByFunId(fun);
            return Json(fun, JsonRequestBehavior.AllowGet);
        }

        private Function GetRootFunByFunId(Function fun)
        {
            if (fun == null)
            {
                return null;
            }
            if (fun.ParentId == 0)
            {
                return fun;
            }

            return GetRootFunByFunId(_AppContext.FunctionApp.ParentFun(fun.Id));

        }

        private Function GetRootFunByFunId_M(Function fun)
        {
            if (fun == null)
            {
                return null;
            }
            if (fun.ParentId == 0)
            {
                return fun;
            }
            var funcs = allfuns.Where(e => e.ChildId == fun.Id.ToString()).ToList();

            return GetRootFunByFunId_M(funcs.FirstOrDefault());

        }

        private IEnumerable<Function> Get2LMenuByParentAndUserId(IEnumerable<Function> allFun, string userId, IEnumerable<Function> userAllFun)
        {
            var resultList = new List<Function>();
            foreach (var item in allFun)
            {
                var obj = userAllFun.FirstOrDefault(q => q.Id == item.Id);
                if (obj != null)
                {
                    resultList.Add(item);
                    continue;
                }
                var isCheck = Is2LChecked(item, userAllFun);
                if (isCheck == true)
                {
                    resultList.Add(item);
                }


            }
            return resultList;
        }

        private bool Is2LChecked(Function function, IEnumerable<Function> userAllFun)
        {
            if (function == null)
            {
                return false;
            }

            var child = _AppContext.FunctionApp.ChildFun(function.Id);
            if (child == null)
            {
                return false;
            }

            foreach (var item in child)
            {
                var obj = userAllFun.FirstOrDefault(q => q.Id == item.Id);
                if (obj != null)
                {
                    return true;
                }

                Is2LChecked(item, userAllFun);
            }
            return false;

        }

    }
}