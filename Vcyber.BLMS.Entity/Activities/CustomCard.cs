using System;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    ///     自定义卡券
    /// </summary>
    public class CustomCard
    {
        public long Id { get; set; }

        /// <summary>
        ///     卡券code
        /// </summary>
        public string CardCode { get; set; }

        /// <summary>
        ///     卡券类型
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

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
        ///     用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     微信openId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 卡券ID
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// 是否补发  
        /// </summary>
        public bool IsReissue { get; set; }


        /// <summary>
        /// 数据来源(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string Source { get; set; }

    }


    public class RrsUserCustomCard : CustomCard
    {
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
        /// 卡券名称
        /// </summary>
        public string CardName
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
    }
}