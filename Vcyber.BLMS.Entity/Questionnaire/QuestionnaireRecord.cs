using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 在线调研记录
    /// </summary>
    public class QuestionnaireRecord
    {
        public QuestionnaireRecord() { }

        public int Id { get; set; }

        public string MemberId { get; set; }

        public int QuestionnaireId { get; set; }

        public int State { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 联系信息id
        /// </summary>
        public int ContactId { get; set; }
    }
}
