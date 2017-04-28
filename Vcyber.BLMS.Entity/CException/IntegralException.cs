using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 积分不足 异常
    /// </summary>
    public class IntegralException : Exception
    {
        #region ==== private field ====

        private string message;

        #endregion

        #region ==== public constructor ====

        public IntegralException(string message)
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
