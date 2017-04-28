using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 重复操作异常
    /// </summary>
    public class RepeatException : Exception
    {
        #region ==== private field ====

        private string message;

        #endregion

        #region ==== public constructor ====

        public RepeatException(string message)
        {
            this.message = message;
        }

        #endregion

        #region ==== public property ====

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        #endregion
    }
}
