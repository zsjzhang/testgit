using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ShareResources
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 资源副名称
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 资源路径
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 资源简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApprovedTime { get; set; }
        /// <summary>
        /// 是否显示 1:显示 0（默认）不显示
        /// </summary>
        public int IsDisplay { get; set; }
        /// <summary>
        /// 是否删除 1：删除 0（默认）未删除
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 是否审批 1：已审批 0（默认）未审批
        /// </summary>
        public int IsApproved { get; set; }
        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadTimes { get; set; }

        public int FileType { get; set; }
        public string Category { get; set; }

        public string ListImageUrl { get; set; }

        public string PlayImageUrl { get; set; }
    }
}
