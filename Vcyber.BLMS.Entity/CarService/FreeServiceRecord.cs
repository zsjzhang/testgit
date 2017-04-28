using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class FreeServiceRecord
    {
        #region CS_SonataService实体属性
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int OrderType { get; set; }
        public string UserId { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCity { get; set; }
        public string DealerProvince { get; set; }
        public string PurchaseYear { get; set; }
        public string UserName { get; set; }
        public int UserSex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactTime { get; set; }
        public string Comment { get; set; }
        public int State { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string TakeAddress { get; set; }
        public float TakeLong { get; set; }
        public float TakeLat { get; set; }
        public string ReturnAddress { get; set; }
        public float ReturnLong { get; set; }
        public float ReturnLat { get; set; }
        public DateTime ReturnDate { get; set; }
        public string UpdateId { get; set; }
        public string UpdateName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string VIN { get; set; }
        public string CarSeries { get; set; }
        public string LicensePlate { get; set; }
        public int MileAge { get; set; }
        public int Status { get; set; }
        public string ServiceOrderNo { get; set; }
        public int MaintainType { get; set; }
        public DateTime SyncTime { get; set; }
        public int IsExported { get; set; }
        public string DataSource { get; set; }
        public string OpenId { get; set; }
        #endregion
        public string IdentityNumber { get; set; }
        public DateTime MLevelBeginDate { get; set; }
        public DateTime MLevelInvalidDate { get; set; }
        public DateTime BuyTime { get; set; }
    }
}
