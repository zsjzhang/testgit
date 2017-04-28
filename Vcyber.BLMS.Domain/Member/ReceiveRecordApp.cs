using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ReceiveRecordApp:IReceiveRecordApp        
    {
        /// <summary>
        /// 添加一条领取记录
        /// </summary>
        /// <param name="obj">领取记录实体</param>
        /// <returns>成功是失败否</returns>
        public bool Add(Entity.Member.ReceiveRecord obj)
        {
            return _DbSession.ReceiveRecordRepository.Add(obj);
        }
        /// <summary>
        /// 领取记录是否存在
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="brandName">业务名称</param>
        /// <returns>存在是，不存在否</returns>
        public bool IsExist(string userId, string brandName)
        {
            return _DbSession.ReceiveRecordRepository.IsExist(userId, brandName);
        }
    }
}
