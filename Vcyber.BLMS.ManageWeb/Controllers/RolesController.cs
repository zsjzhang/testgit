using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using AspNet.Identity.SQL;
using System.Security.Claims;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.ManageWeb.Models;
using Vcyber.BLMS.Repository;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class RolesController : Controller
    {
        public RolesController()
        {
        }

        public RolesController(
            ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        //
        //读取角色创建
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //异步写入角色创建
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create(RolesViewModel rolesViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(rolesViewModel.Name);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public JsonResult RoleList(int? start, int? count)
        {
            int totalCount = 0;

            var roleStore = new RoleStore<IdentityRole>();

            var roleList = roleStore.GetRoles(start ?? 0, count ?? 0, out totalCount);
            var result = new { data = roleList.Result, pos = start ?? 0, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult RoleTypeList()
        //{
        //    IEnumerable<IdentityRole> roleList = RoleManager.Roles;
        //    var roleTypeList = new List<OptionType>();
        //    foreach (var role in roleList)
        //    {
        //        var roleType = new OptionType(role.Id, role.Name);
        //        roleTypeList.Add(roleType);
        //    }
        //    return Json(roleTypeList, JsonRequestBehavior.AllowGet);

        //}

        //public JsonResult BindJsonResult()
        //{
        //    var roleList = roleApp.getAll();
        //    return Json(roleList, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult RoleJsonResult(string id)
        //{
        //    var roleresult = RoleManager.FindById(id);
        //    return Json(roleresult, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult AddRole()
        //{
        //    return View();

        //}

        [HttpPost]
        public ActionResult AddRole(RoleModel roleModel)
        {
            if (!ModelState.IsValid)
            {
                return Json
                 (new
                 {
                     id = -3,
                     message = "输入格式错误"
                 }
                 );
            }
            var isExist = RoleManager.RoleExists(roleModel.Name);
            if (isExist)
            {
                return Json
                (
                    new
                    {
                        id = -1,
                        message = "您输入的角色名已存在，请重新输入！"
                    }
                )
                ;
            }
            else
            {
                var role = new IdentityRole();
                role.Name = roleModel.Name;
                role.Describe = roleModel.Describe;
                var roleresult = RoleManager.Create(role);

                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                }

                return Json
                (
                    new
                    {
                        id = 1,
                        message = " 操作成功，新角色已添加！"
                    }
                );
            }
        }

        public ActionResult EditRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditRole(RoleModel roleModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success=false, message = "输入格式错误" });
            }
            if (roleModel == null)
            {
                return Json(new { success = false, message = " 不存在当前角色！" });
            }
            var role = RoleManager.FindById(roleModel.Id);
            if (role.Name != roleModel.Name)
            {
                var isExist = RoleManager.RoleExists(roleModel.Name);
                if (isExist)
                {
                    return Json(new { success = false, message = "您输入的角色名已存在，请重新输入！" });
                }
            }
            role.Name = roleModel.Name;
            role.Describe = roleModel.Describe;
            var roleresult = RoleManager.Update(role);
            if (!roleresult.Succeeded)
            {
                return Json(new { success = false, message = "更新失败！" });
            }
            return Json(new { success=true, message = "更新成功！" });
        }

        [HttpPost]
        public bool DelRole(string Id)
        {
            var result = RoleManager.Delete(RoleManager.FindById(Id));
            return result.Succeeded;
        }

        public JsonResult BindFunctionJsonResult(string id)
        {
            //get role binding function
            var functionList = _AppContext.FunctionApp.GetRoleFuncs(id);

            //get all function
            var allFunction = _AppContext.FunctionApp.LoadAll();
            var allFunctionModel = RevertFunctionListToModeList(allFunction, functionList);
            //var idList = new List<int>();
            //foreach (var fun in functionList)
            //{
            //    var function = _AppContext.FunctionApp.GetOne(fun.Id);
            //    if (function != null)
            //    {
            //        idList.Add(function.Id);
            //    }
            //}
            //var functionModelList = new FunctionModelList();
            
            //functionModelList.FunctionModels = allFunctionModel;
           // functionModelList.IdList = idList;
            return Json(allFunctionModel, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public bool BindFunction(List<int> things, string roleId)
        {
            if (things != null)
            {
                return _AppContext.FunctionApp.AddRoleFunc(roleId, things);
            }

            return false;

        }

        private List<FunctionModel> RevertFunctionListToModeList(IEnumerable<FunctionTreeView> functionList)
        {
            var modelList = new List<FunctionModel>();

            if (functionList == null)
            {
                return null;
            }

            foreach (var functionTreeView in functionList)
            {
                FunctionModel model = FunctionModel.ConvertFunctionToModel(functionTreeView);

                modelList.Add(model);
            }

            return modelList;
        }

        private List<FunctionModel> RevertFunctionListToModeList(IEnumerable<FunctionTreeView> functionList,IEnumerable<Function> roleFunList)
        {
            var modelList = new List<FunctionModel>();

            if (functionList == null)
            {
                return null;
            }

            foreach (var functionTreeView in functionList)
            {
                FunctionModel model = FunctionModel.ConvertFunctionToModel(functionTreeView, roleFunList);
              
                if (roleFunList.FirstOrDefault(q=>q.Id == model.id) != null)
                {
                    model.IsChecked = true;
                }
                modelList.Add(model);
            }

            return modelList;
        }

        private List<int> GetCheckedIdList(FunctionModel things)
        {
            var idList = new List<int>();
            if (things == null)
            {
                return null;
            }

            if (things.IsChecked)
            {
                idList.Add(things.id);
            }

            if (things.data == null || things.data.Count==0)
            {
                return idList;
            }
            foreach (var subFun in things.data)
            {
                idList.AddRange(GetCheckedIdList(subFun));
            }
            return idList;
        }

    }
}