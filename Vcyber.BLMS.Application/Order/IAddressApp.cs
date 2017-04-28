using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IAddressApp
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户常用地址
        /// </summary>
        /// <param name="data"></param>
        int Add(Address data);

        /// <summary>
        /// 删除地址信息
        /// </summary>
        /// <param name="id">地址ID</param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 修改地址信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Address data);

        /// <summary>
        /// 获取用户地址信息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Address> GetList(string userId);

        /// <summary>
        /// 设置用户默认地址
        /// </summary>
        /// <param name="id">地址ID</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool SetDefault(int id, string userId);

        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        Address GetOne(int id);

        #endregion
    }
}
