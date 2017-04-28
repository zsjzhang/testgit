using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 优惠券实体
    /// </summary>
    public class CustomCardInfo
    {

        /// <summary>
        /// 自定义卡券ID
        /// </summary>
        public int Id
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
        /// 商户logo
        /// </summary>
        public string MerchantLogoUrl
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券来源
        /// </summary>
        public int CardSource
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

        public string ActivityType
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券有效期，1：固定时间；2：领取后生效
        /// </summary>
        public int CardValidityType
        {
            set;
            get;
        }
        /// <summary>
        /// 有效期天
        /// </summary>
        public int CardValidity
        {
            set;
            get;
        }
        /// <summary>
        /// 可用时段，1：全时段可用；2：指定时段可以；
        /// </summary>
        public int CardTimeLimitType
        {
            set;
            get;
        }
        /// <summary>
        /// 时间段：1：MONDAY周一 ；2：TUESDAY周二；3：WEDNESDAY周三 ；4：THURSDAY周四 ；5：FRIDAY周五；6：SATURDAY周六 ； 7：SUNDAY周日 
        /// </summary>
        public string CardTimeLimit
        {
            set;
            get;
        }

        /// <summary>
        /// 已使用卡券库存数量
        /// </summary>
        public int Used
        {
            set;
            get;
        }


        /// <summary>
        /// 卡券库存数量
        /// </summary>
        public int Quantity
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
        /// 领取限制数量
        /// </summary>
        public int GetLimit
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券投放范围 1：全国通用；2：指定特约店；
        /// </summary>
        public int CardPutinType
        {
            set;
            get;
        }

        /// <summary>
        /// 指定特约店 json :如：”111，222，333"
        /// </summary>
        public string CardPutin
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
        /// 卡券状态，1：有效；2：无效
        /// </summary>
        public int status
        {
            set;
            get;
        }
        /// <summary>
        /// 卡券创建人
        /// </summary>
        public string UserId
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
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set;
            get;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate
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
        /// 卡券短信信息
        /// </summary>
        public string SmsContent
        {
            set;
            get;
        }
    }



    public class UserCustomCard : CustomCard
    {

        /// <summary>
        /// 卡券来源
        /// </summary>
        public int CardSource
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

        public string ActivityType
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
    }

    public class ReturnCustomCardCodeInfo
    {

        /// <summary>
        ///     是否核销
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        ///     是否发送
        /// </summary>
        public bool IsSend { get; set; }

        /// <summary>
        ///     是否领取
        /// </summary>
        public bool IsSave { get; set; }
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

        /// <summary>
        /// 卡券类型
        /// </summary>
        public string CardType
        {
            set;
            get;
        }
    }

    public class ReturnCustomCardCodeInfoByDMS
    {

        /// <summary>
        ///     是否核销
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        ///     是否发送
        /// </summary>
        public bool IsSend { get; set; }

        /// <summary>
        ///     是否领取
        /// </summary>
        public bool IsSave { get; set; }
        /// <summary>
        /// 卡券开始有效期；
        /// </summary>
        public string CardBeginDate
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券结束有效期；
        /// </summary>
        public string CardEndDate
        {
            set;
            get;
        }
        /// <summary>
        /// 用户卡券兑奖码
        /// </summary>
        public string CardCode { set; get; }

        /// <summary>
        /// 卡券类型
        /// </summary>
        public string CardType
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券抵扣金额
        /// </summary>
        public string ReduceCost { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Tel { set; get; }

        /// <summary>
        /// 卡券优惠说明
        /// </summary>
        public string CardRemark { set; get; }

        /// <summary>
        /// 卡券类型（1：工时费；2：配件费；3：维修费）
        /// </summary>
        public string CardCategory { set; get; }
    }

    /// <summary>
    /// 卡券短信实体；
    /// </summary>
    public class CustomCardSms
    {
        /// <summary>
        /// 兑奖码
        /// </summary>
        public string CardCode { set; get; } 

    }
}
