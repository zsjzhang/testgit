using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class WinningModel
    {

        private string username = string.Empty;
        private string userphone = string.Empty;
        /// <summary>
        /// 活动编号
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(username) || username.Length < 2)
                {
                    return "";
                }
                string str = "*";
                for (int i = 1, len = username.Length; i < len; i++)
                {
                    str += "*";
                }
                return username.Replace(username.Substring(1), str);
            }
            set { username = value; }
        }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string UserPhone
        {
            get
            {
                if (string.IsNullOrEmpty(userphone))
                {
                    return "";
                }
                else if (userphone.Length == 11)
                {
                    return userphone.Replace(userphone.Substring(2, 6), "****");
                }
                else
                {
                    return userphone;
                }

            }
            set { userphone = value; }
        }
        /// <summary>
        /// 奖品编号
        /// </summary>
        public int PrizeID { get; set; }
        /// <summary>
        /// 奖品价格
        /// </summary>
        public decimal PrizePrice { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeTitle { get; set; }
        /// <summary>
        /// 奖品图片地址
        /// </summary>
        public string PrizeImg { get; set; }
    }
}
