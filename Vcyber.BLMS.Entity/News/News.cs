using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class News
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 新闻缩略图
        /// </summary>
        public string MajorImageUrl { get; set; }

        /// <summary>
        /// 绝对新闻缩略图
        /// </summary>
        public string TureMajorImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgPath"] + MajorImageUrl;
            }
        }

        /// <summary>
        /// 新闻内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 新闻内容（图片绝对地址）
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
        /// 新闻创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 新闻最近更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 新闻创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 新闻最近更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 新闻审批状态（0：未审批；1：审批通过；2：审批未通过 ）
        /// </summary>
        public int IsApproved { get; set; }

        /// <summary>
        /// 审批最近更新时间 
        /// </summary>
        public DateTime ApprovedTime { get; set; }

        /// <summary>
        /// 审批最新更新人 
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 新闻是否在页面显示（0：不显示；1：显示）
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 是否为热点新闻（0：不是热点新闻；1：是热点新闻）
        /// </summary>
        public int IsHot { get; set; }

        /// <summary>
        /// 新闻展示顺序（从小到大）
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 新闻是否已被删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        ///简介 
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string Dealer { get; set; }
    }
}
