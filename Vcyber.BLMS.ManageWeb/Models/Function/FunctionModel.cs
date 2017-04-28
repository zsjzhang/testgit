using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class FunctionModel
    { 
        #region ==== 构造函数 ====

        public FunctionModel()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
         [Required(ErrorMessage = "权限名不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 默认访问Url
        /// </summary>
        public string Action { get; set; }
        public string Controller { get; set; }
        public string UrlDescibe { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public int FType { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        public bool IsChecked { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 使用哪种路由方法（1:angularjs route, 2:mvc）
        /// </summary>
        public string RouteSelection { get; set; }

        public List<FunctionModel> data { get; set; }

        public static FunctionModel ConvertFunctionToModel(FunctionTreeView treeView)
        {
            if (treeView == null || treeView.FuncData == null)
            {
                return null;
            }

            FunctionModel model = new FunctionModel();
            Function function = treeView.FuncData;

            List<FunctionTreeView> childFunction = treeView.ChildeFuncs;
            List<FunctionModel> subModelList = new List<FunctionModel>();

            model.id = function.Id;
            model.Describe = function.Describe;
            model.Name = function.Name;
            model.FType = function.FType;
            model.IsChecked = false;
            if (childFunction == null || childFunction.Count == 0)
            {
                return model;
            }

            foreach (var subFun in childFunction)
            {
                FunctionModel subModel = new FunctionModel();
                subModel = ConvertFunctionToModel(subFun);
                subModelList.Add(subModel);
            }

            model.data = subModelList;
            return model;
        }
        public static FunctionModel ConvertFunctionToModel(FunctionTreeView treeView, IEnumerable<Function> roleFunList)
        {
            if (treeView == null || treeView.FuncData == null)
            {
                return null;
            }

            FunctionModel model = new FunctionModel();
            Function function = treeView.FuncData;

            List<FunctionTreeView> childFunction = treeView.ChildeFuncs;
            List<FunctionModel> subModelList = new List<FunctionModel>();

            model.id = function.Id;
            model.Describe = function.Describe;
            model.Name = function.Name;
            model.FType = function.FType;
            if (roleFunList.FirstOrDefault(q => q.Id == model.id) != null)
            {
                model.IsChecked = true;
            }
            if (childFunction == null || childFunction.Count == 0)
            {
                return model;
            }

            foreach (var subFun in childFunction)
            {
                FunctionModel subModel = new FunctionModel();
                subModel = ConvertFunctionToModel(subFun, roleFunList);
                subModelList.Add(subModel);
            }

            model.data = subModelList;
            return model;
        }

        #endregion
    }
}