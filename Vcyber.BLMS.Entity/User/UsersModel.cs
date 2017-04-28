using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class UsersModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 经销商ID
        /// </summary>
        public string DealerId { set; get; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { set; get; }


        /// <summary>
        /// 账号状态（1：启用；0：未启用；2：锁定）
        /// </summary>
        public string Status { set; get; }

    }
}
