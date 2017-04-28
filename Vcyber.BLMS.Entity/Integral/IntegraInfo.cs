using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户积分信息（总，剩余，使用）
    /// </summary>
   public class IntegraInfo 
    {
        /// <summary>
        /// 总积分
        /// </summary>
        public decimal total { get; set; }

        /// <summary>
        /// 使用积分
        /// </summary>
        public decimal usevalue { get; set; }

        /// <summary>
        /// 剩余积分
        /// </summary>
        public decimal Surplus { get; set; }
    }
}
