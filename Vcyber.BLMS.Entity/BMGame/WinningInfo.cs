using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class WinningInfo
    {
        public WinningInfo() { }


        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动编号
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string UserTel { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 车辆Vin码
        /// </summary>
        public string Vin { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgAddress { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceNO { get; set; }
        /// <summary>
        /// 状态：0-信息不全，1-待审核，2-审核不通过，3-审核通过
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 奖品编号
        /// </summary>
        public int PrizesId { get; set; }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        public string PhoneNumber { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 奖品等级
        /// </summary>
        public int PrizeLevel { get; set; }
        /// <summary>
        /// 中奖
        /// </summary>
        /// <param name="user"></param>
        /// <param name="activityId"></param>
        /// <param name="prizesId"></param>
        public void WinningPrize(Membership user, int activityId, int prizesId)
        {
            this.UserId = user.Id;
            this.UserName = user.RealName ?? user.UserName;
            this.UserTel = user.PhoneNumber;
            this.Vin = user.VIN;
            this.ActivityId = activityId;
            this.PrizesId = prizesId;
            this.UpdateTime = DateTime.Now;
            this.CreateTime = DateTime.Now;
            this.State = 0;
        }
    }
}
