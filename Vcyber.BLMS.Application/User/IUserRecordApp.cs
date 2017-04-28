using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    public interface IUserRecordApp
    {
        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="data"></param>
        void SaveLogin(UserLoginRecord data);

        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="data"></param>
        void SaveOperate(UserOperRecord data);
    }
}
