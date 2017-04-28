using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDInviter
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public int InviterId { set; get; }
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { set; get; }
        /// <summary>
        /// 推荐人姓名
        /// </summary>
        public string InviterName { set; get; }
        /// <summary>
        /// 推荐人手机号
        /// </summary>
        public string InviterMobile { set; get; }
        /// <summary>
        /// 推荐人会员编号
        /// </summary>
        public string InviterUserId { set; get; }
        /// <summary>
        /// 被推荐人姓名
        /// </summary>
        public string InviteredName { set; get; }
        /// <summary>
        /// 被推荐人手机号
        /// </summary>
        public string InviteredMobile { set; get; }
        /// <summary>
        /// 被推荐车型
        /// </summary>
        public string InviteredCar { set; get; }
        /// <summary>
        /// 推荐时间
        /// </summary>
        public DateTime InviterTime { set; get; }
        /// <summary>
        /// 推荐类型【1，置换月推荐有礼】
        /// </summary>
        public int InviterType { set; get; }
        /// <summary>
        /// 推荐来源【app dx pc wx】
        /// </summary>
        public string InviterSource { set; get; }
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
    }
}
