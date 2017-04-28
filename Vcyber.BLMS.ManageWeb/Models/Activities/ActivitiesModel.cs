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
    public class ActivitiesModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string MajorImageUrl { get; set; }

        private string _content;

        [Required]
        [Display(Name = "内容")]
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value.Replace(ConfigurationManager.AppSettings["ImgPath"], "");
            }
        }
        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl { get; set; }

        [Required]
        public int IsDisplay { get; set; }

        [Required]
        [Range(0, 1)]
        public int SignUp { get; set; }
        public string SignUpStatus
        {
            get
            {
                switch (this.SignUp)
                {
                    case 1: return "是";
                    default: return "否";
                }
            }
        }

        [Required]
        [DataType(DataType.Time)]
        public string BeginTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string EndTime { get; set; }

        /// <summary>
        /// 0:未开始，1：已开始，未结束，2：已结束
        /// </summary>
        public int Status
        {
            get
            {
                if (DateTime.Now > DateTime.Parse(EndTime))
                {
                    return 2;
                }
                if (DateTime.Now < DateTime.Parse(BeginTime))
                {
                    return 0;
                }
                return 1;
            }
        }

        public string StatusValue
        {
            get
            {
                if (DateTime.Now > DateTime.Parse(EndTime))
                {
                    return EActivitiescsStatus.Finished.GetDiscribe();
                }
                if (DateTime.Now < DateTime.Parse(BeginTime))
                {
                    return EActivitiescsStatus.NoBegin.GetDiscribe();
                }
                return EActivitiescsStatus.InProcess.GetDiscribe(); ;
            }

        }

        [Required]
        [Range(0, 1)]
        public int IsDeleted { get; set; }

        public int IsApproved { get; set; }
        public string ApproveStatusDescribe
        {
            get
            {
                return ((EApproveStatus)this.IsApproved).GetDiscribe();

            }
        }

        [Required]
        [StringLength(200)]
        public string Summary { get; set; }

        /// <summary>
        /// 是否支持URL跳转
        /// </summary>
        [Required]
        public int IsUrl { get; set; }
        public string IsUrlStatus
        {
            get
            {
                switch (this.IsUrl)
                {
                    case 1: return "是";
                    default: return "否";
                }
            }
        }

        /// <summary>
        /// 跳转URL
        /// </summary>
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public int SupportWay
        {
            get
            {
                if (this.SignUp == 1)
                {
                    return 0;
                }
                if (this.IsUrl == 1)
                {
                    return 1;
                }

                return 2;
            }

        }

        /// <summary>
        /// 是否只有车主可以参与
        /// </summary>
        [Required]
        public int IsCarOwner { get; set; }
        public string CarOwnerStatus
        {
            get
            {
                switch (this.IsCarOwner)
                {
                    case 1: return "是";
                    default: return "否";
                }
            }
        }

        public int Priority { get; set; }

        public string Dealer { get; set; }

        /// <summary>
        /// 是否热点活动
        /// </summary>
        [Required]
        public int IsHot { get; set; }
        public string IsHotStatus
        {
            get
            {
                switch (this.IsHot)
                {
                    case 1: return "是";
                    default: return "否";
                }
            }
        }

        public string DealerName { get; set; }
    }
}