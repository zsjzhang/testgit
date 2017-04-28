using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDActivity
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
        public string ActivityName { set; get; }
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
        public string ActivityImage { set; get; }
        /// <summary>
        /// 活动链接
        /// </summary>
        public string ActivityUrl { set; get; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActivityStartTime { set; get; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActivityEndTime { set; get; }
        /// <summary>
        /// 活动状态
        /// </summary>
        public int ActivityStatus { set; get; }
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int LotteryTotalCount { set; get; }
        /// <summary>
        /// 奖品剩余数量
        /// </summary>
        public int LotteryBalanceCount { set; get; }
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

        public string TrueContent
        {
            get
            {
                var result = "";
                if (!string.IsNullOrEmpty(ActivityContent))
                {
                    result = System.Web.HttpUtility.UrlDecode(ActivityContent);
                    result = result.Replace("/upload/image", ConfigurationManager.AppSettings["ImgPath"] + "/upload/image");
                }
                return result;
            }
        }

    }
}
