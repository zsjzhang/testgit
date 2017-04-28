using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户操作记录
    /// </summary>
    public class UserRecordStorager : IUserRecordStorager
    {
        #region ==== 构造函数 ====

        public UserRecordStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="data"></param>
        public void AddLogin(UserLoginRecord data)
        {
            string sql = "INSERT INTO userloginrecord(UserId,UserName,LoginMode,CreateTime) VALUES(@UserId,@UserName,@LoginMode,@CreateTime);";
            DbHelp.Execute(sql, data);
        }

        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="data"></param>
        public void AddOperate(UserOperRecord data)
        {
            string sql = "INSERT INTO useroperrecord(UserId,UserName,OperType,Content,CreateTime) VALUES(@UserId,@UserName,@OperType,@Content,@CreateTime);";
            DbHelp.Execute(sql, data);
        }

        #endregion
    }
}
