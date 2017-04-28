using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户回答实体
    /// </summary>
    public class Answer
    {
        #region ==== 构造函数 ====

        public Answer() { }
        #endregion

        #region ==== 公用属性 ====

        public int Id { get; set; }

        /// <summary>
        /// 填写该答案的用户id
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 该答案所属问题id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 若问题为选择则该列为A等选项
        /// </summary>
        public string AContent { get; set; }

        /// <summary>
        /// 该答案当前状态：0 删除，1 正常
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 当为选择题时，所属选项id
        /// </summary>
        public int OptionId { get; set; }
        #endregion
    }
}
