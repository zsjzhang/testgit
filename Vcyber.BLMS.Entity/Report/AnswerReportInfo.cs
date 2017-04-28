using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 结果统计信息
    /// </summary>
    public class AnswerReportInfo
    {

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 一级题目
        /// </summary>
        public string QContent { get; set; }

        /// <summary>
        /// 二级题目
        /// </summary>
        public string CQContent { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string OName { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Age { get; set; }

        public string Education { get; set; }

        public string CarModels { get; set; }

        public int MLevel { get; set; }

        public string VIN { get; set; }

        public string IdentityNumber { get; set; }
    }
}
