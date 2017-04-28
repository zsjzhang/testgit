using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class ReportCard
    {
        public string PhoneNumber { get; set; }

        public string CustName { get; set; }

        public string CarCategory { get; set; }

        public string VIN { get; set; }

        public int Mileage { get; set; }

        public string CardType { get; set; }

        public string CardInfo { get; set; }

        public string CreateTime { get; set; }

        public string DealerId { get; set; }

        public string CardNo { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActName { get; set; }

        /// <summary>
        /// 卡券类型ID
        /// </summary>
        public string Type { get; set; }

        //被推荐人姓名
        public string RecommendName { get; set; }

     
        


    }
}