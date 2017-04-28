using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    /// <summary>
    /// 会员积分查询实体信息
    /// </summary>
    public class ResMemberUserintegralInfo : BaseMembership
    {

        /// <summary>
        /// 会员生效日期
        /// </summary>
        public string MLevelBeginDate { get; set; }


        /// <summary>
        /// 会员失效时间
        /// </summary>
        public string MLevelInvalidDate
        {
            get;
            set;
        }
        /// <summary>
        /// 会员总积分 
        /// </summary>
        //decimal
        public string TotalPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 已使用积分
        /// </summary>
        /// //decimal
        public string UserValue
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余积分
        /// </summary>
        //decimal
        public string Point
        {
            get;
            set;
        }

        /// <summary>
        /// 可使用的卡券数量
        /// </summary>
        //int
        public string ReceivedCardCount
        {
            get;
            set;
        }

        /// <summary>
        /// 消费时最多可使用的积分
        /// </summary>
        //int
        public string MaxConsumInte
        {
            get;
            set;
        }
        /// <summary>
        /// 蓝宾会员与否 "Y" :YES / "N" : NO
        /// </summary>
        public string BlueMembership_YN
        {
            get;
            set;
        }

        #region add by 会员信息查询和会员积分查询接口合并
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 性别 1:男 2:女
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        #endregion

        /// <summary>
        /// 积分兑换比例 
        /// 1：10代表(10积分=1元)
        /// 1：100代表(100积分=1元)
        /// 以此类推
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 等级认证时间
        /// </summary>
        public string AuthenticationTime { get; set; }
    }
}