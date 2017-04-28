using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDSendInfo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int SendId { set; get; }
        public int ActivityId { set; get; }
        public int LotteryId { set; get; }
        public string SendSource { set; get; }
        /// <summary>
        /// SendProvince
        /// </summary>
        public string SendProvince { set; get; }
        /// <summary>
        /// SendCity
        /// </summary>
        public string SendCity { set; get; }
        /// <summary>
        /// SendDistrinct
        /// </summary>
        public string SendDistrinct { set; get; }
        /// <summary>
        /// SendAddress
        /// </summary>
        public string SendAddress { set; get; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserMobile { set; get; }
        /// <summary>
        /// 是否有效（0：无效，1：有效）
        /// </summary>
        public int IsValid { set; get; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreaterId { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreaterName { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreaterTime { set; get; }
        /// <summary>
        /// 修改人id
        /// </summary>
        public string UpdaterId { set; get; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdaterName { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdaterTime { set; get; }
        /// <summary>
        /// 中奖记录ID
        /// </summary>
        public int LotteryRecordId { set; get; }
    }
}
