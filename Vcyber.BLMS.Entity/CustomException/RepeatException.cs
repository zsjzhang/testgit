using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CustomException
{
    /// <summary>
    /// 重复操作
    /// </summary>
    public class RepeatException:Exception
    {
        #region ==== 构造函数 ====

        public RepeatException(string message)
            : base(message)
        {

        }

        #endregion
    }
}
