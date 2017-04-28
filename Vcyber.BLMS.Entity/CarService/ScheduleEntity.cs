using System;

namespace Vcyber.BLMS.Entity.CarService
{
    using Vcyber.BLMS.Entity.Enum;

    public class ScheduleEntity
    {

        public string ScheduleType { get; set; }
        ///<summary>
        ///预约日期
        ///</summary>
        public DateTime? ScheduleDate { get; set; }

        ///<summary>
        ///受理状态
        ///</summary>
        public EOrderState State { get; set; }

        ///<summary>
        ///受理人
        ///</summary>
        public string UpdateName { get; set; }

        ///<summary>
        ///受理时间
        ///</summary>
        public string UpdateTime { get; set; }

    }
}
