using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class ProductV
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 兑换所需积分
        /// </summary>
        public int credit { get; set; }

        /// <summary>
        /// 兑换蓝豆
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public string addtime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public string expire { get; set; }

        /// <summary>
        /// 图片url地址
        /// </summary>
        public string imgurl { get; set; }

        public int GoldMemberIntegral { get; set; }

        public  int SilverMemberIntegral { get;   set;}

        public int IsIdentity { get; set; }
        public string IsIdentityText { get; set; }
        
    }
}