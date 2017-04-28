using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class SCServiceCardType
    {

        public int Id { get; set; }

        /// <summary>
        ///     卡券code
        /// </summary>
        public string CardTypeName { get; set; }

        /// <summary>
        ///     卡券类型
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     卡券活动名称
        /// </summary>
        public string ActivityType { get; set; }

        //活动代码
        public string code { get; set; }
    }
}
