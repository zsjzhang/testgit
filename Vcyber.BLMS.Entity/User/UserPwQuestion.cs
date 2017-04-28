using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class UserPwQuestion
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 问题ID
        /// </summary>
        public int PwId { get; set; }

        /// <summary>
        /// 设定答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 附加字段－问题
        /// </summary>
        public string Question { get; set; }
    }
}
