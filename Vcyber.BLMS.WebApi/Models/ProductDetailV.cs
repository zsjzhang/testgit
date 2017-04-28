using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 商品详情
    /// </summary>
    public class ProductDetailV
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
        /// 身份限制 0无限制；1=索久会员；2=普通会员
        /// </summary>
        public int identity { get; set; }

        /// <summary>
        /// 每人每次最多可兑换（0，没限制）
        /// </summary>
        public int maxPer { get; set; }

        /// <summary>
        /// 每人最多可兑换次数（0，没限制）
        /// </summary>
        public int maxUser { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 商品图片集合
        /// </summary>
        public List<ProductImgV> imgurls { get; set; }

        /// <summary>
        /// 颜色id
        /// </summary>
        public String ProductColorIds { get; set; }
        /// <summary>
        /// 商品颜色
        /// </summary>
        public IList<ProductColor> ProductColorList { get; set; }

        /// <summary>
        /// 商品类型Id
        /// </summary>
        public string ProductTypeIds { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public IList<ProductType> ProductTypeList { get; set; }

        public int GoldMemberIntegral { get; set; }

        public int SilverMemberIntegral { get; set; }

        public int IsIdentity { get; set; }
        public string IsIdentityText { get; set; }

        /// <summary>
        /// 商品状态1.上架 0.下架
        /// </summary>
        public int State { get; set; }
    }

  


    public class ProductImgV
    {
        /// <summary>
        /// 商品图片url地址
        /// </summary>
        public string imgurl { get; set; }
    }
}