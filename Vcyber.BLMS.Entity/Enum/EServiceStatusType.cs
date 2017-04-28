using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
   public enum EServiceStatusType
    {
        /// <summary>
        /// 全部
        /// </summary>
        [EnumDescribe("待受理")]
        All = 0,

        /// <summary>
        /// 未发放
        /// </summary>
        [EnumDescribe("系统已受理")]
        Processed = 1,

        /// <summary>
        /// 未发放
        /// </summary>
        [EnumDescribe("待特约店处理")]
        ToBeProcess = 2,

        /// <summary>
        /// 未发放
        /// </summary>
        [EnumDescribe("服务记录已完成")]
        Finish = 3
        
       
    }
}
