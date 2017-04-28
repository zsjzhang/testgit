using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product
    {
        #region ==== 构造函数 ====

        public Product()
        {
            ProductColorList = new List<ProductColor>();
            ProductTypeList = new List<ProductType>();
        }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        /// <summary>
        /// 长标题
        /// </summary>
        public string Name { get; set; }

        ///<summary>
        ///短标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///类型:1礼品2奖品3定制
        /// </summary>
        public int Type { get; set; }

        ///<summary>
        ///商品形式1.邮寄实物2电话卡3救援服务4积分
        /// </summary>
        public int Mode { get; set; }

        private string image;

        /// <summary>
        /// 商品图片
        /// </summary>
        public string Image { get { return this.image; } set { this.image = value; } }

        /// <summary>
        /// 积分
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "普卡会员价格不能为空")]
        [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "普卡会员价格不能为0")]
        public int Integral { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 蓝豆
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 锁定库存
        /// </summary>
        public int LQty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 销售量
        /// </summary>
        public int Sales { get; set; }

        ///<summary>
        ///库存为0是否显示
        /// </summary>
        public int IsDisplay { get; set; }

        ///<summary>
        ///商品状态1.上架 0.下架
        /// </summary>
        public int State { get; set; }

        ///<summary>
        ///是否允许退货0不允许1允许
        /// </summary>
        public int Returned { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        public short Datastate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime ShelfTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Updatetime { get; set; }

        ///<summary>
        ///供应商
        ///</summary>
        public string Provider { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 每人每次最多可兑换（0，没限制）
        /// </summary>
        public int MaxPer { get; set; }

        /// <summary>
        /// 每人最多可兑换次数（0，没限制）
        /// </summary>
        public int MaxUser { get; set; }

        /// <summary>
        /// 没人最多免费次数（0，不免费）
        /// </summary>
        public int MaxFree { get; set; }

        /// <summary>
        /// 商品推荐（0：普通；1=新品；2=热销）
        /// </summary>
        public int IsRecommend { get; set; }

        /// <summary>
        /// 商品权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 购买者身份限制；0无限制；1=索久会员；2=普通会员
        /// </summary>
        public int IsIdentity { get; set; }

        public string  IsIdentityText { 
            
            get {
                 if(IsIdentity ==10)
                 {
                   return  "仅限普卡会员";
                 }
                 if (IsIdentity ==11)
                 {
                 return  "仅限银卡会员";
                 }
                 if(IsIdentity ==12)
                 {
                 return "仅限金卡会员";
                 }
                 if (IsIdentity == 13)
                 {
                     return "仅限金卡、银卡会员";
                 }
                 return "";
        
        }
          }

        /// <summary>
        /// 商品赞美个数
        /// </summary>
        public int PraiseCount { get; set; }
        /// <summary>
        /// 库存预警项
        /// </summary>
        public int Yjx { get; set; }

     /// <summary>
        /// 会员生日折扣月
        /// </summary>
        public decimal? MemberBirthdayDiscountPrice { get; set; }

        /// <summary>
        /// 是否是会员商品
        /// </summary>
        public Boolean? IsMember { get; set; }

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

        /// <summary>
        /// 商品规格
        /// </summary>
        public int ProductSpec { get; set; }

        #endregion

        #region ==== 附件属性 ====

        public ProductDetail Detail { get; set; }

        public List<ProductImage> Images { get; set; }

        public ProductCategory Category { get; set; }

        public string TextContent { get; set; }


        public Category ParentCategory { get; set; }

        public Category ChildCategory { get; set; }

       [Required(AllowEmptyStrings = false, ErrorMessage = "金卡会员价格不能为空")]
       [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "金卡会员价格不能为0")]
        public decimal? GoldMemberIntegral { get; set; }

        public decimal? FrontGoldMemberIntegral
        {
            get { 
                return  GoldMemberIntegral==null ?  Integral : GoldMemberIntegral;
            }
           
        
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "银卡会员价格不能为空")]
        [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "银卡会员价格不能为0")]
        public decimal? SilverMemberIntegral
        {
            get;
            set;
        }

        public decimal? FrontSilverMemberIntegral
        {
            get
            {
                return SilverMemberIntegral == null ? Integral : SilverMemberIntegral;
            }
        }
        #endregion
    }
}
