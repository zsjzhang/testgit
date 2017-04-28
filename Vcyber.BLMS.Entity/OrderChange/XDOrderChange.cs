using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDOrderChange
    {
        /// <summary>
        /// 预约置换id
        /// </summary>
        public int OrderChangeId { set; get; }
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { set; get; }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarSeriers { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { set; get; }
        /// <summary>
        /// 活动邀请码
        /// </summary>
        public string InviteCode { set; get; }
        /// <summary>
        /// 4S店省
        /// </summary>
        public string ShopProvince { set; get; }
        /// <summary>
        /// 4S店市
        /// </summary>
        public string ShopCity { set; get; }
        /// <summary>
        /// 4S店区
        /// </summary>
        public string ShopDistrict { set; get; }
        /// <summary>
        /// 4S店代码
        /// </summary>
        public string ShopCode { set; get; }
        /// <summary>
        /// 经销商名
        /// </summary>
        public string DealName { set; get; }
        /// <summary>
        /// 邮寄省
        /// </summary>
        public string SendProvince { set; get; }
        /// <summary>
        /// 邮寄市
        /// </summary>
        public string SendCity { set; get; }
        /// <summary>
        /// 邮寄区
        /// </summary>
        public string SendDistrinct { set; get; }
        /// <summary>
        /// 邮寄街道
        /// </summary>
        public string SendAddress { set; get; }
        /// <summary>
        /// 原有车辆品牌
        /// </summary>
        public string OldCarBrand { set; get; }
        /// <summary>
        /// 原有车辆车型
        /// </summary>
        public string OldCarSeriers { set; get; }
        /// <summary>
        /// 原有车辆首次上牌年
        /// </summary>
        public string OldCarLicenseYear { set; get; }
        /// <summary>
        /// 原有车辆首次上牌月份
        /// </summary>
        public string OldCarLicenseMonth { set; get; }
        /// <summary>
        /// 原有车辆行驶公里数
        /// </summary>
        public double OldCarDriver { set; get; }
        /// <summary>
        /// 活动标识（1,2）【1，预约试驾；2，预约置换；】
        /// </summary>
        public int OrderChangeType { set; get; }
        /// <summary>
        /// 活动来源(app dx pc wx)
        /// </summary>
        public string OrderChangeSource { set; get; }
        /// <summary>
        /// 预约置换时间
        /// </summary>
        public DateTime OrderChangeTime { set; get; }
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
        public string UpdaterName { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdaterTime { set; get; }
    }
}
