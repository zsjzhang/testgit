using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReadCheckinData
    {
        /// <summary>
        /// 核销数据
        /// </summary>
        public ReadCheckinDataItem data { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 处理结果。0正常,-1错误
        /// </summary>
        public int result { get; set; }
    }
}
