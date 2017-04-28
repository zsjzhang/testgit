using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单不能退款，订单有退款正在进行退款
    /// </summary>
    public class TKZException:Exception
    {
         #region ==== private field ====

        private string message;

        #endregion

        #region ==== public constructor ====

        public TKZException(string message)
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
