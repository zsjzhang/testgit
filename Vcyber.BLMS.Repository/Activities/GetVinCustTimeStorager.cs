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
    public class GetVinCustTimeStorager : IGetVinCustTimeStorager
    {


        /// <summary>
        /// 查询银卡会员的vin，姓名，购车日期
        /// </summary>
        /// <returns>机场列表</returns>
       public IEnumerable<GetVinCustTimeEF> GetVinCustTime(string start , string end)
        {
            string sql = @"select   IF_Car.BuyTime,IF_Car.VIN,IF_Customer.CustName 
from IF_Car with (nolock) inner join IF_Customer with(nolock)  on  IF_Car .CustId =IF_Customer.CustId  
inner join Membership  with(nolock) on Membership .IdentityNumber = IF_Customer .IdentityNumber 
where 
IF_Customer.CustId=IF_Car.CustId and IF_Car.CarCategory in('第九代索纳塔','全新途胜','索纳塔9','索纳塔9(混合动力)','改款全新胜达','悦纳(YC)','索纳塔9混动版','2016款全新胜达','悦纳')
and IF_Car.BuyTime between '2016-01-01' and '2016-12-31'";

            if (!string.IsNullOrEmpty(start))
            {
                sql += string.Format ( " AND if_car.BuyTime  >='{0}'", start);
            }

            if (!string.IsNullOrEmpty(end))
            {
                sql += string .Format ("  AND if_car.BuyTime <'{0}'",end);
            }
            return DbHelp.Query<GetVinCustTimeEF>(sql);
        }

       public IEnumerable<GetVinCustTimeEF> GetVinCustTime()
       {
           string sql = "select IF_Car.BuyTime,IF_Car.VIN,IF_Customer.CustName from IF_Car,IF_Customer where IF_Customer.CustId=IF_Car.CustId and (IF_Car.CarCategory ='第九代索纳塔' or IF_Car.CarCategory ='全新途胜'or IF_Car.CarCategory ='索纳塔9' )  and DateDiff(dd,IF_Car.BuyTime,getdate())=1";
           return DbHelp.Query<GetVinCustTimeEF>(sql);
       }
    }
}
