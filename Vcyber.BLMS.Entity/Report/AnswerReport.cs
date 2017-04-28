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
    public class AnswerReport
    {



        public int Id { get; set; }

        /// <summary>
        /// 一级题目
        /// </summary>
        public string QContent { get; set; }

        public string MemberId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string MLevel { get; set; }

        public string IdentityNumber { get; set; }

        public string VIN { get; set; }
    }
}
