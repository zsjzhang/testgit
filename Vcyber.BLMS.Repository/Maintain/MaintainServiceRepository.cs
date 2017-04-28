using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    class MaintainServiceRepository : IMaintainServiceRepository
    {
        /// <summary>
        /// 根据ActivityType来获取卡券，同时增加排序和上下架功能
        /// </summary>
        /// <param name="actType">活动类型ID</param>
        /// <returns></returns>
        public IEnumerable<MaintainService> GetCustomCardInfoByActType(string actType)
        {
            string sql = @"SELECT c.CardType,ms.CardName,ActivityType,CardLogoUrl,c.Id as CradId ,
                        CASE c.CardValidityType  WHEN 1  THEN c.CardEndDate   WHEN 2 THEN  GETDATE()  +isnull(c.CardValidity,0)    ELSE  c.CardEndDate  end as   CardEndDate,
                        CASE c.CardValidityType  WHEN 1  THEN c.CardBeginDate   WHEN 2 THEN  GETDATE()   ELSE  c.CardBeginDate  end as   CardBeginDate
                         from CustomCardInfo c  inner join  MaintainService ms on   c.CardType=ms.CardType   where ms.State=2 and Type=@Type AND status=1 order by ms.OrderBy desc";
            return DbHelp.Query<MaintainService>(sql, new { Type = actType });
        }
    }
}
