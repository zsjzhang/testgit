using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum MembershipStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumDescribe("正常")]
        Nomal=1,

        /// <summary>
        /// 冻结
        /// </summary>
        [EnumDescribe("冻结")]
        Freezon=2
    }
    //积分状态
    public enum Userintegraldatastate
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumDescribe("正常")]
        iscan = 0,

        /// <summary>
        /// 冻结
        /// </summary>
        [EnumDescribe("冻结")]
        nocan = 1
    }


    public enum MembershipApprovalStatus
    {
        /// <summary>
        /// 未激活
        /// </summary>
        [EnumDescribe("未激活")]
        NotActive = 1,

        /// <summary>
        /// 激活中
        /// </summary>
        [EnumDescribe("激活中")]
        Activing = 2,

        /// <summary>
        /// 已激活
        /// </summary>
        [EnumDescribe("已激活")]
        Actived = 3,

        /// <summary>
        /// 激活失败
        /// </summary>
        [EnumDescribe("激活失败")]
        ActiveFailure = 4,

        /// <summary>
        /// 待缴费
        /// </summary>
        [EnumDescribe("待缴费")]
        NeedPay = 5
    }

    /// <summary>
    /// 会员会费支付状态
    /// </summary>
    public enum MembershipPayStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [EnumDescribe("未缴费")]
        NotPay=0,

        /// <summary>
        /// 已支付
        /// </summary>
        [EnumDescribe("已缴费")]
        Paid=1,
        /// <summary>
        /// 用户已提交支付
        /// </summary>
        [EnumDescribe("审核中")]
        Paiing=2
    }

    /// <summary>
    /// 申请会员认证状态
    /// </summary>
    public enum MembershipApplyApprovalStatus
    {
        /// <summary>
        /// 已提交
        /// </summary>
        [EnumDescribe("待审批")]
        Submit = 1,

        /// <summary>
        /// 已支付
        /// </summary>
        //[EnumDescribe("审批中")]
        //Approving = 2,

        /// <summary>
        /// 审批通过
        /// </summary>
        [EnumDescribe("已通过")]
        Approved=3,

        /// <summary>
        /// 审批拒绝
        /// </summary>
        [EnumDescribe("未通过")]
        ApprovalReject=4
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum MembershipType
    {
        /// <summary>
        /// 非车主
        /// </summary>
        [EnumDescribe("非车主")]
        WhitoutCar = 1,

        /// <summary>
        /// 车主
        /// </summary>
        [EnumDescribe("车主")]
        WhitCar = 2,

        /// <summary>
        /// 银卡会员
        /// </summary>
        [EnumDescribe("银卡会员")]
        Sonata9 = 3
    }

    /// <summary>
    /// 会员提交激活方式
    /// </summary>
    public enum MembershipActiveWay
    {
        /// <summary>
        /// 前端提交
        /// </summary>
        [EnumDescribe("前端提交")]
        ClientWeb=1,

        /// <summary>
        /// 后台提交
        /// </summary>
        [EnumDescribe("后台提交")]
        ManageWeb=2,

        [EnumDescribe("APP")]
         App = 3,

        [EnumDescribe("WeChat")]
        Wechat = 4,
    }

    /// <summary>
    /// 是否强制会员修改密码
    /// </summary>
    public enum MembershipNeedModifyPw
    {
        /// <summary>
        /// 不需要
        /// </summary>
        [EnumDescribe("不需要")]
        No = 0,

        /// <summary>
        /// 需要
        /// </summary>
        [EnumDescribe("需要")]
        Yes = 1
    }

    /// <summary>
    /// 车型增积分类型
    /// </summary>
    public enum CarTypeReturnIntegral
    {

        /// <summary>
        /// 不提示交费增积分
        /// </summary>
        [EnumDescribe("不符合缴费增积分")]
        NoIntegral= -1,
        
        /// <summary>
        /// D+S
        /// </summary>
        [EnumDescribe("D+S")]
        DS= 0,

        /// <summary>
        /// D+S
        /// </summary>
        [EnumDescribe("D+S增换")]
        DSAdd = 1,
      
     /// <summary>
        /// D+S
        /// </summary>
        [EnumDescribe("NoDS")]
        NoDS= 2,

        /// <summary>
        /// D+S
        /// </summary>
        [EnumDescribe("NoDSAdd增换")]
        NoDSAdd = 3
    }
}
