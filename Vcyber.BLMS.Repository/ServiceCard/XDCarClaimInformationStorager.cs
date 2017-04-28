using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
   public class XDCarClaimInformationStorager : IXDCarClaimInformationStorager
    {

        /// <summary>
        /// 卡券核销索赔
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public IEnumerable<ResCarClaimInformation> GetCarClaimInformation(string starttime, string endtime)
        {
            string sql = string.Empty;
            //string endtime = Convert.ToString(DateTime.Now);
            //string starttime = Convert.ToString(DateTime.Now.AddDays(-2));
            sql = @"select UsedRecord.CreateTime as CreateTime, CardType.Code as Code, CardType.ActivityCode as ActivityCode,UsedRecord.VIN as VIN 
          from SC_ServiceCardUsedRecord UsedRecord left join SC_ServiceCardType CardType on UsedRecord.CardType=CardType.CardType
         LEFT JOIN IF_CAR R ON UsedRecord.Vin = R.VIN 
         where 1=1 and CardType.activitycode is not null and UsedRecord.CreateTime between @starttime and @endtime";
            return DbHelp.Query<ResCarClaimInformation>(sql, new { @starttime = starttime, @endtime = endtime });


        }
    }
}
