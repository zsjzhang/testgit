using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IAddressStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回地址Id</returns>
        int AddAddress(Address entity);

        /// <summary>
        /// 地址是否重复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true:重复</returns>
        bool IsAddresss(Address entity);

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateAddress(Address entity);

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteAddress(int id);

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SetDefaultAddress(string userId, int id);

        /// <summary>
        /// 获取用户常用地址列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<Address> SelectList(string userID);

          /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
         Address SelectOne(int id);

        #endregion
    }
}
