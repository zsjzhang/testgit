using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class FunctionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FunctionListJsonResult()
        {

            var functionResult = _AppContext.FunctionApp.LoadAll();
            var modelList = RevertFunctionListToModeList(functionResult);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddRootFunction(FunctionModel model)
        {
            return this.AddFunction(model,0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddChildFunction(FunctionModel model, int parentId)
        {
            return this.AddFunction(model, parentId);
        }


        public ActionResult EditFunction()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EditFunction(FunctionModel model)
        {
            if (ModelState.IsValid)
            {
                var fun = _AppContext.FunctionApp.GetOne(model.id);
                if (fun.Name!=model.Name)
                {
                    if (_AppContext.FunctionApp.IsFunName(model.Name,model.ParentId))
                    {
                        return Json(new { success = true, msg = "父节点中已经存在此功能名" });
                    }
                }

                bool isSuccess = _AppContext.FunctionApp.UpdateFuncAndUrl(model.id, model.Name, model.Describe,
                    model.Action,model.Controller, model.UrlDescibe, model.RouteSelection);

                if (isSuccess)
                {
                    return Json(new {success = true, msg = "编辑功能成功"});
                }
            }
                return Json(new {success = true, msg = "编辑功能失败"});
           
        }

        public JsonResult FunctionJsonResult(int id)
        {
            var function = _AppContext.FunctionApp.GetOne(id);
            return Json(function, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FunctionUrlJsonResult(int id)
        {
            var function = _AppContext.FunctionApp.SelectUrl(id);

            return Json(function, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取功能细节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FunctionAndUrlJsonResult(int id)
        {
            var funtionViewModel = new FunctionModel();

            var funtion = _AppContext.FunctionApp.GetOne(id);
            if (funtion != null)
            {
                funtionViewModel.id = id;
                funtionViewModel.Name = funtion.Name;
                funtionViewModel.Describe = funtion.Describe;
                var functionUrl = _AppContext.FunctionApp.SelectUrl(id).FirstOrDefault();
                if (functionUrl!=null)
                {
                    funtionViewModel.Action = functionUrl.Action;
                    funtionViewModel.Controller = functionUrl.Controller;
                    funtionViewModel.UrlDescibe = functionUrl.Describe;
                }
            }
            return Json(funtionViewModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public bool DelFunction(int id)
        {
            return _AppContext.FunctionApp.DeleteFunc(id);
        }

        #region private method
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

        private JsonResult AddFunction(FunctionModel model, Nullable<int> parentId)
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
                return Json(new { success = false, message});
            }

            if (parentId == null)
            {
                parentId = 0;
            }

            //Check whether the function is dup with others in the same level

            bool isExists = _AppContext.FunctionApp.IsFunName(model.Name, parentId.Value);

            if (isExists)
            {
                return Json(new { success = false, msg = "已存在此功能名称" });
            }

            Function function = new Function();
            function.Describe = model.Describe;
            function.Name = model.Name;
            function.ParentId = parentId.Value;
            function.FType = 1;
            function.IsDel = 0;
            function.RouteSelection = model.RouteSelection;

            FunctionUrl funUrl = new FunctionUrl();
            funUrl.Action = model.Action;
            funUrl.Controller = model.Controller; 
            funUrl.Describe = model.UrlDescibe;
            funUrl.IsDel = 0;

            if (_AppContext.FunctionApp.AddFunc(function,funUrl, EFunctionType.Fun))
            {
                return Json(new { success = true, msg = "添加功能成功" });
            }
            return Json(new { success = false, msg = "添加功能失败" });
        }

        #endregion
    }
}