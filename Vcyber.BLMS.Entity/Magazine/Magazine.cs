using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Magazine
    {
        /// <summary>
        /// 报刊ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 报刊标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 报刊缩略图地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 绝对报刊缩略图地址
        /// </summary>
        public string TrueImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgPath"] + ImageUrl;
            }
        }

        /// <summary>
        /// 报刊资源下载链接 
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 绝对报刊资源下载链接 
        /// </summary>
        public string TrueLinkUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgPath"] + LinkUrl;
            }
        }

        /// <summary>
        /// 报刊简介 
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 解码后的报刊简介内容 后台报刊列表简介列内容
        /// </summary>
        public string TrueSummary
        {
            get
            {
                var result = "";
                if (!string.IsNullOrEmpty(Summary))
                {
                    result = System.Web.HttpUtility.UrlDecode(Summary);
                    //result = result.Replace("/upload/image", ConfigurationManager.AppSettings["ImgPath"] + "/upload/image");
                }
                return result;
            }
        }
        /// <summary>
        /// 报刊所属年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 报刊所属月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 报刊创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 报刊创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 报刊最新更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 报刊最新更新操作人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 报刊是否已被删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// <summary>
        /// 审批状态（0：未审批；1：审批通过；2：审批未通过 ）
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
        /// 是否显示
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 调查问卷URL
        /// </summary>
        public string QuestionUrl { get; set; }

        /// <summary>
        /// 获奖名单URL
        /// </summary>
        public string ResultUrl { get; set; }
        /// <summary>
        /// 阅读链接
        /// </summary>
        public string ReadLink { get; set; }
    }

}
