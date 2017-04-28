using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class JoinActivity
    {
        public JoinActivity() { }
        /// <summary>
        /// 活动编号
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
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
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
        /// Email地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 设备类型（设备类型：0-未知，1-移动端，2-PC）
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 成绩1
        /// </summary>
        public string Results1 { get; set; }
        /// <summary>
        /// 成绩2
        /// </summary>
        public string Results2 { get; set; }
        /// <summary>
        /// 成绩3
        /// </summary>
        public string Results3 { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }

        public string Source { get; set; }

        /// <summary>
        /// 参加活动
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        public void Join(string userId, int activityId, string source, string vin)
        {
            this.UserId = userId;
            this.ActivityId = activityId;
            this.Results3 = source;
            this.Results1 = vin;
            this.CreateDate = DateTime.Now;
        }
    }
}
