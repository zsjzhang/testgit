
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ImageCarousel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgPath"] + ImageUrl;
            }
        }

        /// <summary>
        /// 关联url
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 展示顺序，按照从低到高排序
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否打开新的页面（0：不打开；1：打开）
        /// </summary>
        public int NewPage { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 是否已经删除（0：没有；1：已经删除）
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 活动审批状态（0：未审批；1：审批通过；2：审批未通过 ）
        /// </summary>
        public int IsApproved { get; set; }

        /// <summary>
        /// 审批最近更新时间 
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 审批最新更新人 
        /// </summary>
        public DateTime ApprovedTime { get; set; }

        /// <summary>
        /// 轮播图类型（0：首页；1：新闻等）
        /// </summary>
        public int Type { get; set; }
    }
}
