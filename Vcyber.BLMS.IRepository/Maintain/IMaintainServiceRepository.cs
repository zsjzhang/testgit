using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IMaintainServiceRepository
    {
        /// <summary>
        /// 根据ActivityType来获取卡券，同时增加排序和上下架功能
        /// </summary>
        /// <param name="actType">活动类型ID</param>
        /// <returns></returns>
        IEnumerable<MaintainService> GetCustomCardInfoByActType(string actType);
    }
}
