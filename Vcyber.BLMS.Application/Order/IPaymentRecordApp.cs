using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;


namespace Vcyber.BLMS.Application
{
    public interface IPaymentRecordApp
    {
        /// <summary>
        /// 创建一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        void Add(PaymentRecord obj);
        /// <summary>
        /// 修改一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        void Update(PaymentRecord obj);
        /// <summary>
        /// 修改付款单状态
        /// </summary>
        void Update(string outTradeNo, string state);
        /// <summary>
        /// 保存付款单信息
        /// </summary>
        /// <param name="obj"></param>
        void Save(PaymentRecord obj);
        /// <summary>
        /// 查询一条付款记录
        /// </summary>
        /// <param name="outTradeNo">商家单号</param>
        /// <returns>付款记录</returns>
        PaymentRecord Find(string outTradeNo);
    }
}
