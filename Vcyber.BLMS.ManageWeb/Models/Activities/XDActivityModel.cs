using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models { 
    public class XDActivityModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { set; get; }
        /// <summary>
        /// 活动编码
        /// </summary>
        public string ActivityCode { set; get; }
        /// <summary>
        /// 活动名称
        /// </summary>
        [Required]
        [StringLength(100)] 
        public string XDActivityName { set; get; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivityTitle { set; get; }
        /// <summary>
        /// 活动类型（1，预约试驾；2，预约置换）
        /// </summary>
        public int ActivityType { set; get; }
        /// <summary>
        /// 活动投放位置（1，APP；2，BM；3，wap；4，wechat，5，mms）
        /// </summary>
        public int ActivityPutType { set; get; }
        /// <summary>
        /// 活动图片
        /// </summary>
        public string ActivityName { set; get; }
        /// <summary>
        /// 活动链接
        /// </summary>
        public string ActivityUrl { set; get; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public string ActivityStartTime { set; get; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public string ActivityEndTime { set; get; }
       
        
        /// <summary>
        /// 活动状态  
        ///  0 进行中   1已结束   2 未开始
        /// </summary>
        public int ActivityStatus { set; get; }
        //public int Status
        //{
        //    get
        //    {
        //        if (DateTime.Now > DateTime.Parse(ActivityEndTime))
        //        {
        //            return 2;
        //        }
        //        if (DateTime.Now < DateTime.Parse(ActivityStartTime))
        //        {
        //            return 0;
        //        }

                //if (ActivityStatus==1)
                //{
                //    return EPermuteActivitiescsStatus.Finished.GetDiscribe();
                //}
                //if (ActivityStatus==2)
                //{
                //    return EPermuteActivitiescsStatus.NoBegin.GetDiscribe();
                //}
        //        return 1;
        //    }
        //}

        public string StatusValue
        {
            get
            {
                if ((DateTime.Parse(ActivityStartTime) < DateTime.Now) && (DateTime.Now < DateTime.Parse(ActivityEndTime)))
                {
                    return EPermuteActivitiescsStatus.InProcess.GetDiscribe();
                }
                else if (DateTime.Now > DateTime.Parse(ActivityEndTime))
                {
                    return EPermuteActivitiescsStatus.Finished.GetDiscribe();
                }
                else if (DateTime.Now < DateTime.Parse(ActivityStartTime))
                {
                    return EPermuteActivitiescsStatus.NoBegin.GetDiscribe();
                }
                else
                {
                    return EPermuteActivitiescsStatus.UnKnow.GetDiscribe(); 
                }
            }

        }
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int LotteryTotalCount { set; get; }
        /// <summary>
        /// 奖品剩余数量
        /// </summary>
        public int LotteryBalanceCount { set; get; }
        /// <summary>
        /// 是否在前台显示（0：无效 不显示 ，1：显示 ）
        /// </summary>
        public int IsValid { set; get; }
        /// <summary>
        /// 是否删除 0：否 1：是
        /// </summary>
        public int IsDelete { get; set; }
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
        /// 活动图片
        /// </summary>
        public string ActivityImage { get; set; }
        /// <summary>
        /// 活动缩略图
        /// </summary>
        public string ActivitySubImage { set; get; }
        /// <summary>
        /// 推广车型
        /// </summary>
        public string ActivityCarType { set; get; }
        /// <summary>
        /// 活动礼品(奖品)
        /// </summary>
        public string ActivityLotteryName { set; get; }
        /// <summary>
        /// 活动内容
        /// </summary>
        public string ActivityContent { get; set; }
    }
}