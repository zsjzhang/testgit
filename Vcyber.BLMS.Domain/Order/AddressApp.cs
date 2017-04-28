using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 用户常用地址业务
    /// </summary>
    public class AddressApp : IAddressApp
    {
        #region ==== 构造函数 ====

        public AddressApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户常用地址
        /// </summary>
        /// <param name="data"></param>
        public int Add(Address data)
        {
            bool result = _DbSession.AddressStorager.IsAddresss(data);

            if (result)
            {
                throw new RepeatException("地址重复");
            }

            return _DbSession.AddressStorager.AddAddress(data);
        }

        /// <summary>
        /// 删除地址信息
        /// </summary>
        /// <param name="id">地址ID</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return _DbSession.AddressStorager.DeleteAddress(id);
        }

        /// <summary>
        /// 修改地址信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Address data)
        {
            bool result = _DbSession.AddressStorager.IsAddresss(data);

            if (result)
            {
                throw new RepeatException("地址重复");
            }

            return _DbSession.AddressStorager.UpdateAddress(data);
        }

        /// <summary>
        /// 获取用户地址信息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Address> GetList(string userId)
        {
            return _DbSession.AddressStorager.SelectList(userId);
        }

        /// <summary>
        /// 设置用户默认地址
        /// </summary>
        /// <param name="id">地址ID</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SetDefault(int id, string userId)
        {
            return _DbSession.AddressStorager.SetDefaultAddress(userId, id);
        }

        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        public Address GetOne(int id)
        {
            return _DbSession.AddressStorager.SelectOne(id);
        }

        #endregion
    }
}
