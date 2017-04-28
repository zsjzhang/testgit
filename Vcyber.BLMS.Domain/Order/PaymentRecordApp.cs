using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class PaymentRecordApp:IPaymentRecordApp
    {
        /// <summary>
        /// 创建一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        public void Add(PaymentRecord obj)
        {
            _DbSession.PaymentRecordStorager.Add(obj);
        }
        /// <summary>
        /// 修改一条付款记录
        /// </summary>
        /// <param name="obj">付款记录信息</param>
        public void Update(PaymentRecord obj)
        {
            _DbSession.PaymentRecordStorager.Update(obj);
        }
        /// <summary>
        /// 修改付款单状态
        /// </summary>
        public void Update(string outTradeNo, string state)
        {
            _DbSession.PaymentRecordStorager.Update(outTradeNo, state);
        }
        /// <summary>
        /// 保存付款单信息
        /// </summary>
        /// <param name="obj"></param>
        public void Save(PaymentRecord obj)
        {
            var m = _DbSession.PaymentRecordStorager.Find(obj.OutTradeNo);
            if (m != null)
            {
                m.TransactionId = obj.TransactionId;
                m.IsSubscribe = obj.IsSubscribe;
                m.BankType = obj.BankType;
                m.SettlemenTotalFee = obj.SettlemenTotalFee;
                m.CashFee = obj.CashFee;
                m.TimeEnd = obj.TimeEnd;
                m.ErrCodeDes = obj.ErrCodeDes;
                m.ErrCode = obj.ErrCode;
                m.ResultCode = obj.ResultCode;
                m.ReturnMsg = obj.ReturnMsg;
                m.ReturnCode = obj.ReturnCode;
                m.State = obj.State;
                _DbSession.PaymentRecordStorager.Update(m);
            }
            else
            {
                _DbSession.PaymentRecordStorager.Add(obj);
            }
        }
        /// <summary>
        /// 查询一条付款记录
        /// </summary>
        /// <param name="outTradeNo">商家单号</param>
        /// <returns>付款记录</returns>
        public PaymentRecord Find(string outTradeNo)
        {
            return _DbSession.PaymentRecordStorager.Find(outTradeNo);
        }

    }
}
