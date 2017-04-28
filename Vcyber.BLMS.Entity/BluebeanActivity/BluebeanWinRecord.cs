using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 蓝豆清零中奖记录
    /// </summary>
    public class BluebeanWinRecord
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 微信openID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Prize { get; set; }

        /// <summary>
        /// 获奖时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Contacts { get; set; }
    }
}
