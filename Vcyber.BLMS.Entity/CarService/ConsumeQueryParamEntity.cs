using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;

    using Vcyber.BLMS.Entity.Enum;

    public class ConsumeQueryParamEntity
    {

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string Phone { get; set; }
        /// <summary>
        /// 经销商
        /// </summary>
        [Display(Name = "店代码")]
        public string DealerId { get; set; }

        /// <summary>
        /// 费用查询范围（从）
        /// </summary>
        [Display(Name = "费用范围")]
        public int MinTotalCost { get; set; }

        /// <summary>
        /// 费用查询范围（到）
        /// </summary>
        [Display(Name = "费用范围")]
        public int MaxTotalCost { get; set; }


        /// <summary>
        /// 工单号
        /// </summary>
        [Display(Name = "工单号")]
        public string OrderNo { get; set; }


        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public EPointApproveStatus PointApproveStatus { get; set; }

        /// <summary>
        /// 积分发放状态
        /// </summary>
        [Display(Name = "积分状态")]
        public EPointStatus PointStatus { get; set; }

        /// <summary>
        /// 是否上传附件
        /// </summary>
        [Display(Name = "已上传附件")]
        public EAttachmentStatus HasAttachment { get; set; }

        [Display(Name = "消费类型")]
        public EConsumeType EConsumeType { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        /// <summary>
        ///车架号
        /// </summary>
        [Display(Name = "VIN")]
        public string VIN { get; set; }

        //最小消耗使用积分
        public int? Minpoints { get; set; }

        //最大消耗使用积分
        public int? Maxpoints { get; set; }

        //会员等级
         [Display(Name = "会员等级")]
        public EMLevelType MLevel { get; set; }


    }
}
