using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class ImageCarouselModel
    {/// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        [Required(ErrorMessage = "图片名称必填")]
        public string Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Required(ErrorMessage = "图片必选")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl { get; set; }

        /// <summary>
        /// 关联url
        /// </summary>
       [Required(ErrorMessage = "关联url必填")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 展示顺序
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否打开新的页面（0：不打开；1：打开）
        /// </summary>
        public int NewPage { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 活动审批状态（0：未审批；1：审批通过；2：审批未通过 ）
        /// </summary>
        public int IsApproved { get; set; }
        public string ApproveStatusDescribe
        {
            get
            {
                return ((EApproveStatus)this.IsApproved).GetDiscribe();
            }
        }
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

        public string TypeValue {
            get { return ((EImageCarouselType) this.Type).GetDiscribe(); }
        }

    }
}