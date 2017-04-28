using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
   public interface ISettlementApp
    {
        IEnumerable<SettlementList> GetXDSettlementList(string createtime);

        //IEnumerable<SettlementList> UpdateXDSettlementStatus(string settlementstate, string dealerid);
        int UpdateXDSettlementStatus(string settlementstate,string id);

        //存储DMS的积分明细
        bool InsertDMSIntegralList(DMSIntergralListV data);

        //修改BM结算状态
        bool UpdateBMSettlementStatus(DMSIntergralListV data);
    }
}
