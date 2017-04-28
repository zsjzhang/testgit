using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MagazineModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "主图")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl { get; set; }
        
        
        /// <summary>
        /// 下载链接
        /// </summary>
        //[Required]
        [Display(Name = "下载URL")]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 杂志简介
        /// </summary>
        [Required]
        [Display(Name = "简介")]
        [StringLength(5000, ErrorMessage = "{0} 必须至少包含 {2} 个字符或文字超出长度。", MinimumLength = 1)]
        public string Summary { get; set; }

        [Required]
        [Range(1990,2400)]
        [Display(Name = "所属年份")]
        public int Year { get; set; }

        [Required]
        [Range(1,12)]
        [Display(Name = "所属月份")]
        public int Month { get; set; }
        public string Date
        {
            get
            {
                return this.Year + "年" + this.Month + "月";
            }
        }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateBy { get; set; }
        public int IsDeleted { get; set; }

        public int IsDisplay { get; set; }

        public int IsApproved { get; set; }
        public string ApproveStatusDescribe
        {
            get
            {
                return ((EApproveStatus)this.IsApproved).GetDiscribe();
            }
        }
        public string ApprovedBy { get; set; }

        public string ApprovedTime { get; set; }

        public string QuestionUrl { get; set; }

        public string ResultUrl { get; set; }
        public string ReadLink { get; set; }
    }
}