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
    /// 用户记录操作
    /// </summary>
    public interface IUserRecordStorager
    {
        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="data"></param>
        void AddLogin(UserLoginRecord data);

        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="data"></param>
        void AddOperate(UserOperRecord data);
    }
}
