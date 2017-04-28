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
    /// 用户操作记录
    /// </summary>
    public class UserRecordApp : IUserRecordApp
    {
        #region ==== 构造函数 ====

        public UserRecordApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="data"></param>
        public void SaveLogin(UserLoginRecord data)
        {
            _DbSession.UserRecordStorager.AddLogin(data);
        }

        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="data"></param>
        public void SaveOperate(UserOperRecord data)
        {
            _DbSession.UserRecordStorager.AddOperate(data);
        }

        #endregion
    }
}
