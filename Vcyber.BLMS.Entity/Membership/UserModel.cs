using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class UserModel 
    {
        public string Isadmin { get; set; }

        /// <summary>
        /// User Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        public string UserName { get; set; }


        public string NickName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        public virtual string Password { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int IsDel { get; set; }

        /// <summary>
        /// 1.正常 2：冻结
        /// </summary>
        public virtual int Status { get; set; }

        public virtual string StatusName { get; set; }

        public virtual string CreateTime { get; set; }

        public virtual string UpdateTime { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual int IdentityConfirmed { get; set; }

        /// <summary>
        /// 身份证编号
        /// </summary>
        public virtual string IdentityNumber { get; set; }

        public virtual int Age { get; set; }

        public virtual string Birthday { get; set; }

        /// <summary>
        /// 车主级别
        /// </summary>
        public virtual int MLevel { get; set; }
        /// <summary>
        /// 车主类型(用户选择)。1:注册会员 10:普通车主 11:银卡会员，12：金卡会员
        /// </summary>
        public virtual int MType { get; set; }

        public virtual string MTypeName { get; set; }

        /// <summary>
        /// 车主类型(系统匹配)。1:非车主 2:车主 3:索9会员
        /// </summary>
        public virtual int SystemMType { get; set; }

        public virtual string SystemMTypeName { get; set; }

        public virtual string MLevelName { get; set; }

        public virtual string VIN { get; set; }

        public virtual int ApprovalStatus { get; set; }

        public virtual string Gender { get; set; }

        public virtual string GenderName { get; set; }

        public virtual string Provency { get; set; }

        public virtual string City { get; set; }

        public virtual string Area { get; set; }

        public virtual string Address { get; set; }

        public virtual string RealName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatedPerson { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public virtual string FaceImage { get; set; }

        /// <summary>
        /// 付费支付状态 0:未支付 1:已支付 2:用户已提交支付
        /// </summary>
        public virtual int IsPay { get; set; }

        public virtual string PayStatus { get; set; }
        public virtual int IsPayName { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public virtual string No { get; set; }

        /// <summary>
        /// 激活方式 1:前台提交申请  2:后台提交申请
        /// </summary>
        public virtual int ActiveWay { get; set; }

        /// <summary>
        /// 是否强制需要修改密码 0:不需要  1:需要
        /// </summary>
        public virtual int IsNeedModifyPw { get; set; }

        /// <summary>
        /// 天猫会费支付编号
        /// </summary>
        public virtual string PayNumber { get; set; }


        /// <summary>
        /// 爱好
        /// </summary>
        public virtual string Interest { get; set; }

        /// <summary>
        /// 是否会员
        /// </summary>
        public virtual string IsMembership
        {
            get
            {
                return string.IsNullOrEmpty(No) ? "否" : "是";
            }

        }


        public virtual string CarCategory { get; set; }





        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string MembershipId
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 家庭电话
        /// </summary>
        public virtual string TelePhone
        {
            get;
            set;
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public virtual string PaperWork
        {
            get;
            set;
        }

        /// <summary>
        /// 学历
        /// </summary>
        public virtual string Educational
        {
            get;
            set;
        }
        /// <summary>
        /// 职业
        /// </summary>
        public virtual string Job
        {
            get;
            set;
        }


        /// <summary>
        /// 职位
        /// </summary>
        public virtual string Office
        {
            get;
            set;
        }

        /// <summary>
        /// 所属行业
        /// </summary>
        public virtual string Industry
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public virtual string IsMarriage
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻纪念日
        /// </summary>
        public virtual string MarriageDay
        {
            get;
            set;
        }
        /// <summary>
        /// 主要联系方式
        /// </summary>
        public virtual string MainContact
        {
            get;
            set;
        }
        /// <summary>
        /// 主要联系电话
        /// </summary>
        public virtual string MainTelePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public virtual string OrganizationCode
        {
            get;
            set;
        }
        /// <summary>
        /// 是否发送短信
        /// </summary>
        public virtual int SendSms
        {
            get;
            set;
        }
        /// <summary>
        /// 是否打电话
        /// </summary>
        public virtual int MakePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 是否发送信件
        /// </summary>
        public virtual int SendLetter
        {
            get;
            set;
        }
        public virtual int SendEmail
        {
            get;
            set;
        }

        public int jf { get; set; }

        public int value { get; set; }
        /// <summary>
        /// 客户成交时间
        /// </summary>
        public virtual string TransactionTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual string NewLoginTime
        {
            get;
            set;
        }

        public virtual string UserType
        {
            get;
            set;
        }

        /// <summary>
        /// 会员生效时间
        /// </summary>
        public virtual DateTime MLevelBeginDate
        {
            get;
            set;
        }

        public virtual string strMLevelBeginDate
        {
            get;
            set;
        }
        /// <summary>
        /// 会员失效时间
        /// </summary>
        public virtual DateTime MLevelInvalidDate
        {
            get;
            set;
        }

        public virtual string strMlevelInvalidDate { get; set; }
        /// <summary>
        /// 会费
        /// </summary>
        public virtual decimal Amount
        {
            get;
            set;
        }

        public virtual string Mid
        {
            get;
            set;
        }
        /// <summary>
        /// 车主认证车辆时间
        /// </summary>
        public virtual DateTime AuthenticationTime { get; set; }
        /// <summary>
        /// 车主认证来源
        /// </summary>
        public virtual string AuthenticationSource { get; set; }
        public virtual string AccntType { get; set; }
        public virtual int RankID { get; set; }
        /// <summary>
        /// 积分状态
        /// </summary>
        public virtual int datastate { get; set; }
    }
}
