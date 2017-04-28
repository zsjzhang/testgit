using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 抽奖信息
    /// </summary>
    public class BluebeanWinResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success { get; set; }

        public string Msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public int WinId { get; set; }
    }
}
