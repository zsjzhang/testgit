using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class UserGuideModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 缩略图URL
        /// </summary>
        public string ImageUrl { get; set; }


        /// <summary>
        /// 绝对图片地址
        /// </summary>
        public string TrueImageUrl { get; set; }

        /// <summary>
        /// 资源URL
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 最新更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最新更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 是否已经删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 审批状态
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
        /// 更新审批状态时间
        /// </summary>
        public string ApprovedTime { get; set; }

        /// <summary>
        /// 更新审批状态人
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadTimes { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsDisplay { get; set; }
    }
}