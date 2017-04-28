using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using Vcyber.BLMS.Entity.Enum;

    public  class CSSonataServiceV
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public EOrderType OrderType { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 经销商所在城市
        /// </summary>
        public string DealerCity { get; set; }

        /// <summary>
        /// 经销商所在省份
        /// </summary>
        public string DealerProvince { get; set; }

        /// <summary>
        /// 购车年份
        /// </summary>
        public string PurchaseYear { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别(0: 女, 1:男)
        /// </summary>
        public int UserSex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 可以联系用户的时间
        /// </summary>
        public string ContactTime { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EOrderState State { get; set; }

        /// <summary>
        /// 预约/服务日期
        /// </summary>
        public string ScheduleDate { get; set; }

        /// <summary>
        /// 取车地址
        /// </summary>
        public string TakeAddress { get; set; }

        /// <summary>
        /// 取车地址经度
        /// </summary>
        public double? TakeLong { get; set; }

        /// <summary>
        /// 取车地址纬度
        /// </summary>
        public double? TakeLat { get; set; }

        /// <summary>
        /// 送车地址
        /// </summary>
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 送车地址经度
        /// </summary>
        public double? ReturnLong { get; set; }

        /// <summary>
        /// 送车地址纬度
        /// </summary>
        public double? ReturnLat { get; set; }

        /// <summary>
        /// 送车时间
        /// </summary>
        public string ReturnDate { get; set; }

        /// <summary>
        /// 操作人员Id
        /// </summary>
        public string UpdateId { get; set; }

        /// <summary>
        /// 操作人员姓名
        /// </summary>
        public string UpdateName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 顾问Id
        /// </summary>
        public int? ConsultantId { get; set; }

        /// <summary>
        /// 顾问名称
        /// </summary>
        public string ConsultantName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarSeries { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public int? MileAge { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// CRM服务单号
        /// </summary>
        public string ServiceOrderNo { get; set; }

        /// <summary>
        /// 服务项目(0:维修， 1:保养， 2:维保)
        /// </summary>
        public int? MaintainType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SyncTime { get; set; }

        /// <summary>
        /// 数据是否导出（0：否，1：是）
        /// </summary>
        public int? IsExported { get; set; }

        /// <summary>
        /// 计划购车时间
        /// </summary>
        public string PurchaseTimeFrame { get; set; }
    }
}
