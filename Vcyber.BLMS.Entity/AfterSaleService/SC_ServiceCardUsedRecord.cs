using System;

namespace Vcyber.BLMS.Entity
{
    public class Remeal
    {
        public string CardCode { get; set; }
        public string CustName { get; set; }

        public string PhoneNumber { get; set; }

        public string Vin { get; set; }

        public string DearID { get; set; }

        public string CardType { get; set; }
        public string CardType1 { get; set; }

        public string Mileage { get; set; }

        public string CreateTime1 { get; set; }

        public string BuyTime { get; set; }

        public string CarCategory { get; set; }

        public string DearName { get; set; }

        public string Mlevel { get; set; }

        public string CardName { get; set; }

    }

    public class SC_ServiceCardUsedRecord
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string PhoneNumber { get; set; }

        public string CarCategory { get; set; }

        public string CardType { get; set; }

        public string CardTypeName { get; set; }

        public string CardInfo { get; set; }

        public string CardNo { get; set; }

        public string UserId { get; set; }

        public string CustName { get; set; }

        public string VIN { get; set; }

        public int Mileage { get; set; }

        public DateTime ConsumeDate { get; set; }

        public decimal ConsumeMoney { get; set; }

        public string DealerId { get; set; }

        public DateTime CreateTime { get; set; }

        public string ActivityTag { get; set; }

        public string DealerName { get; set; }

        public string RecommendName { get; set; }

        public string RecommendPhone { get; set; }
        public string MLevel { get; set; }
        public string BuyTime { get; set; }
        public string kqCreateTime { get; set; }

        public string Area { get; set; }

        public string Region { get; set; }

        public string DMSOrderNo { get; set; }
    }
}
