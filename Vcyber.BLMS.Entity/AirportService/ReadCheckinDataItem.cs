using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReadCheckinDataItem
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 已核销的总记录数
        /// </summary>
        public int CountNo { get; set; }

        /// <summary>
        /// 新核销的记录数
        /// </summary>
        public int RecordNo { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public List<ReadCheckinDataItemDetail> data { get; set; }
    }
}
