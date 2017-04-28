using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 功能操作
    /// </summary>
    public class FunctionApp : IFunctionApp
    {
        #region ==== 构造函数 ====

        public FunctionApp()
        {
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 根据功能Id,获取信息
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        public Function GetOne(int funcId)
        {
            return _DbSession.FunctionStorager.SelectOne(funcId);
        }

        /// <summary>
        /// 根据Url 逆向获取function
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerable<Function> GetList(string url)
        {
            List<Function> dataResult = new List<Function>(3);
            Function funData = _DbSession.FunctionStorager.SelectOne(url);

            if (funData != null)
            {
                dataResult.Add(funData);
                this.LoadFunc(funData.ParentId, dataResult);
            }


            return dataResult;
        }

        /// <summary>
        /// 根据Url 逆向获取function
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerable<Function> GetList(string action, string controller)
        {
            List<Function> dataResult = new List<Function>(3);
            Function funData = _DbSession.FunctionStorager.SelectOne(action,controller);

            if (funData != null)
            {
                dataResult.Add(funData);
                this.LoadFunc(funData.ParentId, dataResult);
            }


            return dataResult;
        }

        /// <summary>
        /// 获取顶级功能
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Function> RootFun()
        {
            return _DbSession.FunctionStorager.RootFunc();
        }

        /// <summary>
        /// 获取用户顶级功能列表
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public IEnumerable<Function> RootFun(string managerId)
        {
            return _DbSession.FunctionStorager.RootFunc(managerId);
        }

        /// <summary>
        /// 获取用户孩子功能列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public IEnumerable<Function> ChildFun(int parentId, string managerId)
        {
            return _DbSession.FunctionStorager.ChildFunc(parentId, managerId);
        }

        /// <summary>
        /// 获取孩子功能
        /// </summary>
        /// <param name="funId">功能Id</param>
        /// <returns></returns>
        public IEnumerable<Function> ChildFun(int funId)
        {
            return _DbSession.FunctionStorager.ChildFunc(funId);
        }

        public Function ParentFun(int funId)
        {
            return _DbSession.FunctionStorager.ParnetFun(funId);
        }

        public IEnumerable<Function> AllParnetFun()
        {
            return _DbSession.FunctionStorager.AllParnetFun();
        }

        public IEnumerable<Function> AllFunc(string manageId)
        {
            return _DbSession.FunctionStorager.AllFunc(manageId);
        }

        /// <summary>
        /// 加载功能树
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionTreeView> LoadAll()
        {
            List<FunctionTreeView> dataResult = new List<FunctionTreeView>();

            var rootFuncs = this.RootFun();
#warning ==== 加载全部功能逻辑待完善（此方法不应该存在） ====
            if (rootFuncs != null && rootFuncs.Count() > 0)
            {
                foreach (var funcItem in rootFuncs)
                {
                    FunctionTreeView treeView = new FunctionTreeView()
                    {
                        FuncData = funcItem,
                        ChildeFuncs = new List<FunctionTreeView>()
                    };
                    dataResult.Add(treeView);
                    this.CreateFuncTreeView(treeView);
                }
            }

            return dataResult;
        }

        /// <summary>
        /// 添加功能信息
        /// </summary>
        /// <param name="data">功能信息</param>
        /// <param name="type">功能类型</param>
        /// <returns>true：成功</returns>
        public bool AddFunc(Function data, EFunctionType type)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var funId = -1;
                var isTrue = _DbSession.FunctionStorager.Add(data, type, out funId);
                if (isTrue)
                {
                    DeleteRoleFunBinding(this.ParentFun(funId));
                }

                scope.Complete();
                return isTrue;
            }
        }


        /// <summary>
        /// 添加功能+功能的url
        /// </summary>
        /// <param name="data"></param>
        /// <param name="funUrl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AddFunc(Function data, FunctionUrl funUrl, EFunctionType type)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
                var funId = -1;
                var isTrue = _DbSession.FunctionStorager.Add(data, type, out funId);
                if (isTrue)
                {
                    var functionUrl = new FunctionUrl();
                    functionUrl.FunId = funId;
                    functionUrl.Controller = funUrl.Controller;
                    functionUrl.Action = funUrl.Action; 
                    functionUrl.Describe = funUrl.Describe;
                    var urlId = -1;
                    isTrue = _DbSession.FunctionUrlStorager.Add(functionUrl, out urlId);
                }
                if (isTrue)
                {
                    DeleteRoleFunBinding(this.ParentFun(funId));
                }

                //scope.Complete();
                return isTrue;
            //}
        }

        /// <summary>
        /// 删除某一功能的祖节点的绑定
        /// </summary>
        /// <param name="fun"></param>
        private void DeleteRoleFunBinding(Function fun)
        {
            if (fun == null)
            {
                return;
            }

            this.DelRoleFunc(fun.Id);
            DeleteRoleFunBinding(_AppContext.FunctionApp.ParentFun(fun.Id));

        }

        ///// <summary>
        ///// 添加功能信息，批量添加Url
        ///// </summary>
        ///// <param name="data">功能信息</param>
        ///// <param name="type">功能类型</param>
        ///// <param name="funcUrls">功能可以访问的Url</param>
        ///// <returns></returns>
        //public bool AddFunc(Function data, EFunctionType type, IEnumerable<FunctionUrl> funcUrls)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        int funId;

        //        if (this.AddFunc(data, type, out funId))
        //        {
        //            if (funcUrls != null && funcUrls.Count() > 0)
        //            {
        //                foreach (var urlItem in funcUrls)
        //                {
        //                    int urlId;
        //                    urlItem.FunId = funId;
        //                    this.AddUrl(urlItem, out urlId);
        //                }
        //            }

        //            scope.Complete();
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="funId">功能Id</param>
        /// <returns></returns>
        public bool DeleteFunc(int funId)
        {
            return _DbSession.FunctionStorager.Delete(funId);
        }

        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="keyId">功能KeyId</param>
        /// <param name="name">功能名称</param>
        /// <param name="describe">描述</param>
        /// <param name="routeSelection"></param>
        /// <returns>true:成功</returns>
        public bool UpdateFunc(int funId, string name, string describe, string routeSelection)
        {
            return string.IsNullOrEmpty(name) ? false : _DbSession.FunctionStorager.Update(funId, name, describe, routeSelection);
        }

        /// <summary>
        /// 验证某个模块中是否存在某个功能名称
        /// </summary>
        /// <param name="funName">功能名称</param>
        /// <param name="parentFunId">功能父Id</param>
        /// <returns>true:存在</returns>
        public bool IsFunName(string funName, int parentFunId)
        {
            return _DbSession.FunctionStorager.IsFunName(funName, parentFunId);
        }

        public bool UpdateFuncAndUrl(int funId, string name, string describe, string action, string controller, string urlDescibe, string routeSelection)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
                var isSuccess = _DbSession.FunctionStorager.Update(funId, name, describe, routeSelection);
                if (isSuccess)
                {
                    isSuccess = _DbSession.FunctionUrlStorager.Update(funId, action, controller, urlDescibe, routeSelection);

                }
                //scope.Complete();
                return isSuccess;
            //}
        }

        public Function GetFunByActionAndFunction(string action, string controller)
        {
            Function funData = _DbSession.FunctionStorager.SelectOne(action, controller);
            return funData;

        }

        #region ==== 功能与Url操作 ====

        /// <summary>
        /// 获取某个功能可以访问的Url
        /// </summary>
        /// <param name="funId"></param>
        /// <returns></returns>
        public IEnumerable<FunctionUrl> SelectUrl(int funId)
        {
            return _DbSession.FunctionUrlStorager.SelectUrl(funId);
        }

        /// <summary>
        /// 添加功能访问Url信息
        /// </summary>
        /// <param name="data">url信息</param>
        /// <param name="urlId"></param>
        /// <returns>true:成功</returns>
        public bool AddUrl(FunctionUrl data, out int urlId)
        {
            urlId = -1;
            return data == null ? false : _DbSession.FunctionUrlStorager.Add(data, out urlId);
        }

        /// <summary>
        /// 修改Url访问
        /// </summary>
        /// <param name="urlId">Url KeyId</param>
        /// <param name="url">访问地址</param>
        /// <param name="controller"></param>
        /// <param name="describe">描述</param>
        /// <param name="action"></param>
        /// <param name="routeSelection"></param>
        /// <returns>ture:成功</returns>
        public bool UpdateUrl(int urlId, string action, string controller, string describe, string routeSelection)
        {
            return string.IsNullOrEmpty(action) || string.IsNullOrEmpty(controller) ? false : _DbSession.FunctionUrlStorager.Update(urlId, action, controller, describe, routeSelection);
        }

        /// <summary>
        /// 删除功能访问Url
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        public bool DeleteUrl(int urlId)
        {
            return _DbSession.FunctionUrlStorager.Delete(urlId);
        }

        #endregion

        #region role and function method

        public IEnumerable<Function> GetRoleFuncs(string roleId)
        {
            return _DbSession.FunctionStorager.GetRoleFuncs(roleId);
        }
        public IEnumerable<AuthorityDescriptor> GeRoleFuncsByRoleName(List<string> roleNames)
        {
            return _DbSession.FunctionStorager.GeFuncsByRoleName(roleNames);
        }
        public void DeleteRoleFunc(string roleId)
        {
            _DbSession.FunctionStorager.DeleteFunc(roleId);
        }
        public bool DelRoleFunc(int functionId)
        {
            return _DbSession.FunctionStorager.DelRoleFunc(functionId);
        }

        /// <summary>
        /// 批量添加角色相对应的功能
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="funcIds">功能Id集合</param>
        public bool AddRoleFunc(string roleId, IEnumerable<int> funcIds)
        {
            bool isSuccess = false;
            //using (TransactionScope scope = new TransactionScope())
            //{
                try
                {
                   this.DeleteRoleFunc(roleId);

                    foreach (var funcId in funcIds)
                    {
                        isSuccess = _DbSession.FunctionStorager.AddFunc(funcId, roleId);
                        
                    }
                    //scope.Complete();
                }
                catch (Exception e)
                {
                    isSuccess = false;
                }
                return isSuccess;
            //}

        }
        #endregion

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 创建功能树
        /// </summary>
        /// <param name="parentTree">功能父节点</param>
        private void CreateFuncTreeView(FunctionTreeView parentTree)
        {
            var childFuncs = this.ChildFun(parentTree.FuncData.Id);

            if (childFuncs != null && childFuncs.Count() > 0)
            {
                foreach (var funcItem in childFuncs)
                {
                    FunctionTreeView treeView = new FunctionTreeView() { FuncData = funcItem, ChildeFuncs = new List<FunctionTreeView>() };
                    parentTree.ChildeFuncs.Add(treeView);
                    CreateFuncTreeView(treeView);
                }
            }
        }

        /// <summary>
        /// 逆向加载Func
        /// </summary>
        /// <param name="funcId"></param>
        /// <param name="funcArray"></param>
        private void LoadFunc(int funcId, List<Function> funcArray)
        {
            var funData = this.GetOne(funcId);

            if (funData != null)
            {
                funcArray.Add(funData);

                if (funData.ParentId > 0)
                {
                    this.LoadFunc(funData.ParentId, funcArray);
                }
            }
        }

        #endregion


        public IEnumerable<Function> GetTopMenus(List<string> roles)
        {
            return _DbSession.FunctionStorager.GetTopMenus(roles);
        }
    }
}
