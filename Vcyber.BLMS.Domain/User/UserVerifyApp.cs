using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 用户审核信息业务
    /// </summary>
    public class UserVerifyApp : IUserVerifyApp
    {
        #region ==== 构造函数 ====

        public UserVerifyApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="data"></param>
        public void Save(UserVerify data)
        {
            _DbSession.UserVerifyStorager.Add(data);
        }

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id">审核信息Id</param>
        /// <param name="sate"></param>
        /// <returns></returns>
        public bool UpdateState(int id, EVerifyState state)
        {
            return _DbSession.UserVerifyStorager.UpdateState(id, state);
        }

        /// <summary>
        /// 修改审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateInfo(UserVerify data)
        {
            return _DbSession.UserVerifyStorager.Update(data);
        }

        /// <summary>
        /// 获取用户审核信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public UserVerify GetOne(int userId)
        {
            return _DbSession.UserVerifyStorager.SelectOne(userId);
        }

        #endregion
    }
}
