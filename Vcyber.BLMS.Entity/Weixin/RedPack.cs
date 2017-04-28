using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Weixin
{
    /// <summary>
    /// 红包表
    /// </summary>
    public class RedPack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ActivtyName { get; set; }
        public int Type { get; set; }
        public string SceneId { get; set; }
        public int TotalAmount { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public int TotalPersons { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 红包领取记录表
    /// </summary>
    public class RedPackRecord 
    {
        public int Id { get; set; }
        public int RedPackId { get; set; }
        public string CardName { get; set; }
        public string UserId { get; set; }
        public string OpenId { get; set; }
        public decimal Amount { get; set; }
        public string TradeNo { get; set; }
        public string PaymentNo { get; set; }
        public string ResultCode { get; set; }
        public string ErrorCode { get; set; }
        public string Source { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
