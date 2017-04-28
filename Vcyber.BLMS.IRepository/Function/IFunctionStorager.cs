using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 功能操作
    /// </summary>
    public interface IFunctionStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 根据功能Id，获取信息
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        Function SelectOne(int funcId);

        /// <summary>
        /// 根据Url，获取单个功能信息
        /// </summary>
        /// <param name="funcUrl">功能Url</param>
        /// <returns></returns>
        Function SelectOne(string funcUrl);

        Function SelectOne(string action, string controller);
        /// <summary>
        /// 添加功能信息
        /// </summary>
        /// <param name="data">功能信息</param>
        /// <param name="type">功能类型</param>
        /// <param name="keyId">输出Id</param>
        /// <returns>true：成功</returns>
        bool Add(Function data, EFunctionType type,out int keyId);

        /// <summary>
        /// 删除某个功能
        /// </summary>
        /// <param name="keyId">功能KeyId</param>
        /// <returns>true:成功</returns>
        bool Delete(int keyId);

        /// <summary>
        /// 获取顶级模块
        /// </summary>
        /// <returns></returns>
        IEnumerable<Function> RootFunc();


        /// <summary>
        /// 获取用户顶级功能列表
        /// </summary>
        /// <param name="manageId"></param>
        /// <returns></returns>
        IEnumerable<Function> RootFunc(string manageId);

        /// <summary>
        /// 获取用户孩子功能列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        IEnumerable<Function> ChildFunc(int parentId, string managerId);

        /// <summary>
        /// 获取孩子功能
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns></returns>
        IEnumerable<Function> ChildFunc(int parentId);

        Function ParnetFun(int childId);

        IEnumerable<Function> AllParnetFun();

        IEnumerable<Function> AllFunc(string manageId);

        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="keyId">功能KeyId</param>
        /// <param name="name">功能名称</param>
        /// <param name="defaultUrl">默认访问Url</param>
        /// <param name="describe">描述</param>
        /// <param name="routeSelection"></param>
        /// <returns>true:成功</returns>
        bool Update(int keyId, string name, string describe, string routeSelection);

        /// <summary>
        /// 验证某个模块中是否存在某个功能名称
        /// </summary>
        /// <param name="name">功能名称</param>
        /// <param name="parentId">功能父Id</param>
        /// <returns></returns>
        bool IsFunName(string name, int parentId);

        IEnumerable<Function> GetRoleFuncs(string roleId);
        IEnumerable<AuthorityDescriptor> GeFuncsByRoleName(List<string> roleNames);
        bool AddFunc(int funId, string roleId);

        /// <summary>
        /// 删除某个角色所有的绑定
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        void DeleteFunc(string roleId);

        /// <summary>
        /// 删除某个功能的所有绑定
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
         bool DelRoleFunc(int functionId);

        #endregion

         IEnumerable<Function> GetTopMenus(List<string> roles);
    }
}
