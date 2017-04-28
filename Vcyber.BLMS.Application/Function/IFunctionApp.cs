using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 功能操作
    /// </summary>
    public interface IFunctionApp
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 根据功能Id,获取信息
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        Function GetOne(int funcId);

        /// <summary>
        /// 根据Url 逆向获取function
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerable<Function> GetList(string url);

        IEnumerable<Function> GetList(string action, string controller);
        /// <summary>
        /// 获取顶级功能
        /// </summary>
        /// <returns></returns>
        IEnumerable<Function> RootFun();

        /// <summary>
        /// 获取用户顶级功能列表
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        IEnumerable<Function> RootFun(string managerId);

        IEnumerable<Function> AllFunc(string manageId);
        /// <summary>
        /// 获取用户孩子功能列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        IEnumerable<Function> ChildFun(int parentId, string managerId);
        Function ParentFun(int funId);

        IEnumerable<Function> AllParnetFun();

        /// <summary>
        /// 获取孩子功能
        /// </summary>
        /// <param name="funId">功能Id</param>
        /// <returns></returns>
        IEnumerable<Function> ChildFun(int funId);

        /// <summary>
        /// 加载功能树
        /// </summary>
        /// <returns></returns>
        IEnumerable<FunctionTreeView> LoadAll();

        /// <summary>
        /// 添加功能信息
        /// </summary>
        /// <param name="data">功能信息</param>
        /// <param name="type">功能类型</param>
        /// <param name="funId">输出功能Id</param>
        /// <returns>true：成功</returns>
        bool AddFunc(Function data,EFunctionType type);
        
        bool AddFunc(Function data,FunctionUrl funUrl,EFunctionType type);
        /// <summary>
        /// 添加功能信息，批量添加Url
        /// </summary>
        /// <param name="data">功能信息</param>
        /// <param name="type">功能类型</param>
        /// <param name="funcUrls">功能可以访问的Url</param>
        /// <returns></returns>
       // bool AddFunc(Function data, EFunctionType type, IEnumerable<FunctionUrl> funcUrls);

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="funId">功能Id</param>
        /// <returns></returns>
        bool DeleteFunc(int funId);

        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="funId"></param>
        /// <param name="name">功能名称</param>
        /// <param name="describe">描述</param>
        /// <param name="routeSelection"></param>
        /// <returns>true:成功</returns>
        bool UpdateFunc(int funId, string name, string describe, string routeSelection);
        bool UpdateFuncAndUrl(int funId, string name, string describe, string action,string controller, string urlDescibe, string routeSelection);
        /// <summary>
        /// 验证某个模块中是否存在某个功能名称
        /// </summary>
        /// <param name="funName">功能名称</param>
        /// <param name="parentFunId">功能父Id</param>
        /// <returns>true:存在</returns>
        bool IsFunName(string funName, int parentFunId);

        Function GetFunByActionAndFunction(string action, string controller);
        #region ==== 功能与Url操作 ====

        /// <summary>
        /// 获取某个功能可以访问的Url
        /// </summary>
        /// <param name="funId"></param>
        /// <returns></returns>
        IEnumerable<FunctionUrl> SelectUrl(int funId);

        /// <summary>
        /// 添加功能访问Url信息
        /// </summary>
        /// <param name="data">url信息</param>
        /// <param name="urlId"></param>
        /// <returns>true:成功</returns>
        bool AddUrl(FunctionUrl data, out int urlId);

        /// <summary>
        /// 修改Url访问
        /// </summary>
        /// <param name="urlId">Url KeyId</param>
        /// <param name="action">访问地址</param>
        /// <param name="controller">访问地址</param>
        /// <param name="describe">描述</param>
        /// <param name="routeSelection"></param>
        /// <returns>ture:成功</returns>
        bool UpdateUrl(int urlId, string action, string controller, string describe, string routeSelection);
        /// <summary>
        /// 删除功能访问Url
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        bool DeleteUrl(int urlId);

        #endregion

        #region role and function interface 

        IEnumerable<Function> GetRoleFuncs(string roleId);
        IEnumerable<AuthorityDescriptor> GeRoleFuncsByRoleName(List<string> roleNames);
        bool AddRoleFunc(string roleId, IEnumerable<int> funcIds);
        void DeleteRoleFunc(string roleId);

        bool DelRoleFunc(int functionId);

        IEnumerable<Function> GetTopMenus(List<string> roles);

        #endregion

        #endregion
    }
}
