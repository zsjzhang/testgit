using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReturnCustomCardTypeModel
    {
        /// <summary>
        /// 卡券卡号
        /// </summary>
        public string CardType { set; get; }
        /// <summary>
        /// 卡券名称
        /// </summary>
        public string CardName { set; get; }
        /// <summary>
        /// 卡券活动名称
        /// </summary>
        public string ActivityName { set; get; }
    }
}
