using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class CustomCardMerchantConsumeCode
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 卡券Guid
        /// </summary>
        public string CardType { set; get; }


        /// <summary>
        /// 卡券名称
        /// </summary>
        public string CardName { set; get; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsDel { set; get; }

        /// <summary>
        /// 商户卡券兑奖码
        /// </summary>
        public string CardCode { set; get; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }
        
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityType { set; get; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { set; get; }

        //是否被领取
        public string SendStatus { get; set; }
    }
}
