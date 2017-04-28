using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "图片")]
        public string MajorImageUrl { get; set; }


        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl { get; set; }

        private string _content;

        [Required]
        [Display(Name = "文章内容")]
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value.Replace(ConfigurationManager.AppSettings["ImgPath"], "");
            }
        }

        public string CreateTime { get; set; }

        public string UpdateTime { get; set; }

        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        [Required]
        [Range(0, 2)]
        public int IsApproved { get; set; }
        public string ApproveStatusDescribe
        {
            get
            {
                return ((EApproveStatus)this.IsApproved).GetDiscribe();
            }
        }
        public string ApprovedTime { get; set; }

        public string ApprovedBy { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsDisplay { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsHot { get; set; }

        public int Priority { get; set; }

        public int IsDeleted { get; set; }

        [Required]
        [StringLength(1000)]
        public string Summary { get; set; }

        public string Dealer { get; set; }
        public string DealerName { get; set; }
    }
}