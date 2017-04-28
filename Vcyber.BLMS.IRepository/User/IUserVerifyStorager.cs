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
    /// 用户审核信息操作
    /// </summary>
    public interface IUserVerifyStorager
    {
        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="data"></param>
        void Add(UserVerify data);

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id">审核信息Id</param>
        /// <param name="sate"></param>
        /// <returns></returns>
        bool UpdateState(int id, EVerifyState sate);

        /// <summary>
        /// 修改审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(UserVerify data);

        /// <summary>
        /// 获取用户审核信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        UserVerify SelectOne(int userId);
    }
}
