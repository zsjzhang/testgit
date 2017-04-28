using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class CardModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 服务编码
        /// </summary>
        public string SNCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName 
        { 
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 下发方式
        /// </summary>
        public int SendType { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UseTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}