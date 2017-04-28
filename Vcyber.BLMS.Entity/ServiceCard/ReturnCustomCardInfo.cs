using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{

    public class RecommendCustomer
    {
        public string OpenId { get; set; }

        public string ActivityType { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Created { get; set; }

        public string DataSource { get; set; }

        public string UserId { get; set; }

        public string UserName { get;set;}

      
    }

    /// <summary>
    ///用户领取兑奖码信息
    /// </summary>
    public class ReturnCustomCardInfo
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
    }


    public class ReturnUserCustomCardInfo : ReturnCustomCardInfo
    {
        /// <summary>
        /// 用户卡券兑奖码
        /// </summary>
        public string CardCode { set; get; }
    }

}
