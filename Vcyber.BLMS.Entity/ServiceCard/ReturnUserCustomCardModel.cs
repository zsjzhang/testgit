using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 返回用户领取兑奖码信息
    /// </summary>
    /// <typeparam >兑奖码信息</typeparam>
    public class ReturnUserCustomCardModel
    {
        /// <summary>
        /// 用户卡券核销编号
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 卡券来源
        /// </summary>
        public int CardSource
        {
            set;
            get;
        }


        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName
        {
            set;
            get;
        }

        /// <summary>
        /// 商户logo
        /// </summary>
        public string MerchantLogoUrl
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券logo
        /// </summary>
        public string CardLogoUrl
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券颜色
        /// </summary>
        public string CardColor
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券名称
        /// </summary>
        public string CardName
        {
            set;
            get;
        }
        /// <summary>
        /// 投放活动名称
        /// </summary>

        public string ActivityName
        {
            set;
            get;
        }



        /// <summary>
        /// 卡券抵扣金额；
        /// </summary>
        public int ReduceCost
        {
            set;
            get;
        }


        /// <summary>
        /// 卡券优惠说明
        /// </summary>
        public string CardRemark
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券使用须知
        /// </summary>
        public string Instructions
        {
            set;
            get;
        }


        /// <summary>
        /// 优惠券GUID
        /// </summary>
        public string CardType
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券开始有效期；
        /// </summary>
        public DateTime CardBeginDate
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券结束有效期；
        /// </summary>
        public DateTime CardEndDate
        {
            set;
            get;
        }

        /// <summary>
        /// 用户卡券兑奖码
        /// </summary>
        public string CardCode { set; get; }
    }


    /// <summary>
    /// 用户自定义优惠券 状态（未使用，已使用，已过期）
    /// </summary>

    public class UserCustomCardModel
    {

        /// <summary>
        /// 用户已经领取优惠券
        /// </summary>
        public List<ReturnUserCustomCardModel> ReceivedCustomCardList { set; get; }

        /// <summary>
        /// 用户已经使用优惠券
        /// </summary>
        public List<ReturnUserCustomCardModel> UsedCustomCardList { set; get; }

        /// <summary>
        /// 用户已经过期优惠券
        /// </summary>
        public List<ReturnUserCustomCardModel> ExpiredCustomCardList { set; get; }

    }
}
