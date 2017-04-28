using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReservationExcel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarSeries { get; set; }

        /// <summary>
        /// 服务顾问
        /// </summary>
        public string ConsultantName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }


        string userSex;
        /// <summary>
        /// 性别
        /// </summary>
        public string UserSex
        {
            get { return this.userSex == "0" ? "女" : "男"; }
            set { this.userSex = value; }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string DealerName { get; set; }

        ///<summary>
        ///预约时间
        ///</summary>
        public string ScheduleDate { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 上门地点
        /// </summary>
        public string TakeAddress { get; set; }

        /// <summary>
        /// 购车年份
        /// </summary>
        public string PurchaseYear { get; set; }

        string maintainType;
        /// <summary>
        /// 服务项目
        /// </summary>
        public string MaintainType
        {
            get
            {
                if (this.maintainType == "0")
                    return "维修";
                else if (this.maintainType == "1")
                    return "保养";
                else
                    return "维保";
            }
            set
            {
                this.maintainType = value;
            }
        }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public string MileAge { get; set; }

        /// <summary>
        /// 送车地点
        /// </summary>
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 送车时间
        /// </summary>
        public string ReturnDate { get; set; }

        /// <summary>
        /// 计划购车日期
        /// </summary>
        public string PurchaseTimeFrame { get; set; }
        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId
        {
            get;
            set;
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTime
        {
        get;
        set;
        }

        /// <summary>
        /// 来源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 受理状态
        /// </summary>
        public string StatusName { get; set; }
    }
}
