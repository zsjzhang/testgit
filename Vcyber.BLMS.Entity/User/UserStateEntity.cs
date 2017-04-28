using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.User
{
    public class UserStateEntity
    {
        public UserStateEntity()
        {
            Groups = new List<string>();
        }
        /// <summary>
        ///车主用户ID
        /// </summary>
        public int UserId { get; set; }
       
        /// <summary>
        /// 客户端访问的appKey
        /// </summary>
        public string ClientAppKey { get; set; }

        /// <summary>
        /// Ticket after login
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录电话
        /// </summary>
        public string LoginMobile { get; set; }

        /// <summary>
        /// User groups
        /// </summary>
        public List<string> Groups { get; set; }


    }
}
