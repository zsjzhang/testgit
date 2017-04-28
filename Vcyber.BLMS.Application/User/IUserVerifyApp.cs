using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 用户审核信息业务
    /// </summary>
    public interface IUserVerifyApp
    {
        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="data"></param>
        void Save(UserVerify data);

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id">审核信息Id</param>
        /// <param name="sate"></param>
        /// <returns></returns>
        bool UpdateState(int id, EVerifyState state);

        /// <summary>
        /// 修改审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateInfo(UserVerify data);

        /// <summary>
        /// 获取用户审核信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        UserVerify GetOne(int userId);
    }
}
