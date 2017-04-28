using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 北现TLc上市活动
    /// 吹气游戏记录表
    /// </summary>
    public class BlowCarRecord
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// 持续时间（多少秒或者每秒多少下）
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        /// 行驶距离（米）
        /// </summary>
        public decimal Distance { get; set; }

        /// <summary>
        /// 设备类型：0-未知，1-移动端，2-PC
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
