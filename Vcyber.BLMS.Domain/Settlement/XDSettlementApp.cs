using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;


namespace Vcyber.BLMS.Domain
{
   public class XDSettlementApp :ISettlementApp
    {
        public IEnumerable<Entity.SettlementList> GetXDSettlementList(string createtime)
        {
            return _DbSession.XDSettlementStorager.GetXDSettlementList(createtime);
        }

        //public IEnumerable<Entity.SettlementList> UpdateXDSettlementStatus(string settlementstate, string dealerid)
        //{
        //    return _DbSession.XDSettlementStorager.UpdateXDSettlementStatus(settlementstate, dealerid);
        //}

        public int UpdateXDSettlementStatus(string settlementstate,string id)
        {
            return _DbSession.XDSettlementStorager.UpdateXDSettlementStatus(settlementstate,id);
        }

        //存储DMS的积分明细
        public bool InsertDMSIntegralList(DMSIntergralListV data)
        {
            return _DbSession.XDSettlementStorager.InsertDMSIntegralList(data);
        }

        //更新BM积分结算的状态
        public bool UpdateBMSettlementStatus(DMSIntergralListV data)
        {
           return _DbSession.XDSettlementStorager.UpdateBMSettlementStatus(data);
        }

    }
}
