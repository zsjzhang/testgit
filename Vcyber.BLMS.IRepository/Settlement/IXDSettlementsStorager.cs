using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
   public interface IXDSettlementStorager
    {
        IEnumerable<SettlementList> GetXDSettlementList(string createtime);

        //IEnumerable<SettlementList> UpdateXDSettlementStatus(string settlementstate, string dealerid);
        int UpdateXDSettlementStatus(string settlementstate, string id);

        //存储DMS的积分明细
        bool InsertDMSIntegralList(DMSIntergralListV data);

        //更新BM积分结算的状态
        bool UpdateBMSettlementStatus(DMSIntergralListV data);
    }
  
}
