using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReadCheckinDataItemDetail
    {
        /// <summary>
        /// 核销码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 核销权益次数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 核销地点
        /// </summary>
        public string info { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        public string checkintime { get; set; }
    }
}
