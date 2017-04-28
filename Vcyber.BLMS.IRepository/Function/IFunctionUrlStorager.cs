using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 功能访问Url操作
    /// </summary>
    public interface IFunctionUrlStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加访问Url信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keyId">输出Id</param>
        /// <returns>true:成功</returns>
        bool Add(FunctionUrl data, out int keyId);

        /// <summary>
        /// 删除Url
        /// </summary>
        /// <param name="keyId">Url KeyId</param>
        /// <returns></returns>
        bool Delete(int keyId);

        /// <summary>
        /// 修改Url访问
        /// </summary>
        /// <param name="keyId">Url KeyId</param>
        /// <param name="action">访问地址</param>
        /// <param name="controller">访问地址</param>
        /// <param name="describe">描述</param>
        /// <returns>ture:成功</returns>
        bool Update(int keyId, string action, string controller, string describe, string routeSelection);

        /// <summary>
        /// 获取某个功能可以访问的Url
        /// </summary>
        /// <param name="funId"></param>
        /// <returns></returns>
        IEnumerable<FunctionUrl> SelectUrl(int funId);

        #endregion
    }
}
