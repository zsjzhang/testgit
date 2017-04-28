using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ActivityInfo
    {
        public ActivityInfo() { }
        /// <summary>
        /// 活动编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 活动简介
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 中奖率
        /// </summary>
        public int Probability { get; set; }
    }
}
