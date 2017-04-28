using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 维保服务类型
    /// </summary>
    public enum EServiceType
    {
        /// <summary>
        /// 维修
        /// </summary>
        [Display(Name = "维修")]
        Repair=0,
        /// <summary>
        /// 保养
        /// </summary>
        [Display(Name = "保养")]
        Maintenance=1,
        /// <summary>
        /// 维保
        /// </summary>
        [Display(Name = "维保")]
        RepairAndMaint=2
    }
}
