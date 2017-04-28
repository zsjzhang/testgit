using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Activities
    {
        /// <summary>
        /// 活动ID 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动缩略图
        /// </summary>
        public string MajorImageUrl { get; set; }

        /// <summary>
        /// 绝对活动缩略图
        /// </summary>
        public string TrueMajorImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgPath"] + MajorImageUrl;
            }
        }

        /// <summary>
        /// 活动内容 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 活动内容 （图片绝对地址）
        /// </summary>
        public string TrueContent
        {
            get
            {
                var result = "";
                if (!string.IsNullOrEmpty(Content))
                {
                    result = System.Web.HttpUtility.UrlDecode(Content);
                    result = result.Replace("/upload/image", ConfigurationManager.AppSettings["ImgPath"] + "/upload/image");
                }
                return result;
            }
        }

        /// <summary>
        /// 活动创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 活动更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 活动创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 活动最近更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 可以忽略
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 可以忽略
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 是否支持在线报名（0：不支持；1：支持）
        /// </summary>
        public int SignUp { get; set; }
       
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动是否已经删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 活动审批状态（0：未审批；1：审批通过；2：审批未通过 ）
        /// </summary>
        public int IsApproved { get;set; }
       
        /// <summary>
        /// 审批最近更新时间 
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 审批最新更新人 
        /// </summary>
        public DateTime ApprovedTime { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 是否支持URL跳转
        /// </summary>
        public int IsUrl { get; set; }

        /// <summary>
        /// 跳转URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否只有车主可以参与
        /// </summary>
        public int IsCarOwner { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string Dealer { get; set; }

        /// <summary>
        /// 是否为热点活动
        /// </summary>
        public int IsHot { get; set; }
    }
}
