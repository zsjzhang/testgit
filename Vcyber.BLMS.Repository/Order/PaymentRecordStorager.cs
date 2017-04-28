using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Repository.Entity.Generated;
using PetaPoco;

namespace Vcyber.BLMS.Repository
{
    public class PaymentRecordStorager:IPaymentRecordStorager
    {
        /// <summary>
        /// 创建一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        public void Add(PaymentRecord obj) 
        {
            PocoHelper.CurrentDb().Insert("PaymentRecord", "Id", obj);
        }
        /// <summary>
        /// 修改一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        public void Update(PaymentRecord obj)
        {
            PocoHelper.CurrentDb().Update("PaymentRecord", "Id", obj);
        }
        /// <summary>
        /// 修改付款单状态
        /// </summary>
        public void Update(string outTradeNo, string state)
        {
            var sql = Sql.Builder.Append("UPDATE PaymentRecord SET State = @0 WHERE OutTradeNo = @1", state, outTradeNo);
            PocoHelper.CurrentDb().Execute(sql);
        }
        /// <summary>
        /// 查询一条付款记录
        /// </summary>
        /// <param name="outTradeNo">商家单号</param>
        /// <returns>付款记录</returns>
        public PaymentRecord Find(string outTradeNo)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<PaymentRecord>("SELECT * FROM dbo.PaymentRecord AS pr WHERE OutTradeNo = @0", outTradeNo);
        }
    }
}
