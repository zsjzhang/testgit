using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum EOrderState
    {
        /// <summary>
        /// 待受理
        /// </summary>
        [Display(Name = "待受理")]
        ToBeProcess = 0,

        /// <summary>
        /// 系统已受理
        /// </summary>
        [Display(Name = "已受理")]
        Processed = 1,


        /// <summary>
        /// 待特约店处理
        /// </summary>
        [Display(Name = "待处理")]
        ToDoByDealer = 2,


        /// <summary>
        /// 服务记录已完成
        /// </summary>
        [Display(Name = "不回访关闭")]
        Done = 3,

        /// <summary>
        /// 全部
        /// </summary>
        [Display(Name = "全部")]
        All =-1
    }
}
