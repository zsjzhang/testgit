using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 获取奖品表
    /// </summary>
    public class QuestionnaireWinning
    {

        public QuestionnaireWinning() { }

        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 问卷id
        /// </summary>
        public int QuestionnaireId { get; set; }

        /// <summary>
        /// 获奖人姓名
        /// </summary>
        public string WName { get; set; }

        /// <summary>
        /// 获奖人手机号
        /// </summary>
        public string WPhoneNumber { get; set; }

        /// <summary>
        /// 奖品
        /// </summary>
        public string Prize { get; set; }
    }
}
