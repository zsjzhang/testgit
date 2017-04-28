using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum ELogType
    {
        /// <summary>
        /// 新闻
        /// </summary>
         [EnumDescribe("新闻")]
        News = 1,

        /// <summary>
        /// 活动
        /// </summary>
         [EnumDescribe("活动")]
         Activities = 2,

        /// <summary>
        /// 轮播图
        /// </summary>
         [EnumDescribe("轮播图")]
         ImageCarousel = 3,

        /// <summary>
        /// 报刊
        /// </summary>
         [EnumDescribe("报刊")]
         Magazine = 4,

        /// <summary>
        /// 电子手册
        /// </summary>
         [EnumDescribe("电子手册")]
         UserGuide = 5

    }
}
