using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    class MaintainServiceAPP : IMaintainServiceAPP
    {
        /// <summary>
        /// 根据ActivityType来获取卡券，同时增加排序和上下架功能
        /// </summary>
        /// <param name="actType">活动类型ID</param>
        /// <returns></returns>
        public IEnumerable<MaintainService> GetCustomCardInfoByActType(string actType)
        {
            return _DbSession.MaintainServiceRepository.GetCustomCardInfoByActType(actType);
        }
    }
}
