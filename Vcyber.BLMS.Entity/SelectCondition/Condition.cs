using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 条件
    /// </summary>
    public interface Condition
    {
        string ToWhere();
    }
}
