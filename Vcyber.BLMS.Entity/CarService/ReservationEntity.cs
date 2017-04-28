using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.CarService
{
    public class ReservationEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        ///<summary>
        ///预约类型
        ///</summary>
        public EOrderType ReservationType { get; set; }

        ///<summary>
        ///预约时间
        ///</summary>
        public DateTime? ScheduleDate { get; set; }

        /// <summary>
        /// 预约人手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 预约人姓名
        /// </summary>
        public string UserName { get; set; }

  /// <summary>
        /// 受理状态
        /// </summary>
        public int State { get; set; }

        ///<summary>
        ///受理人
        ///</summary>
        public string UpdateName { get; set; }

        /// <summary>
        /// 受理时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        public int? IsExported { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId
        {
        get;set;
        }
    }

    public class ReservationExcelEntity
    {

       

        ///<summary>
        ///预约类型
        ///</summary>
        public string ReservationType { get; set; }

        ///<summary>
        ///预约时间
        ///</summary>
        public string ScheduleDate { get; set; }

        /// <summary>
        /// 预约人手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 预约人姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 受理状态
        /// </summary>
        public string State { get; set; }
    }
}
