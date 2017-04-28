using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class OweAggregations
    {
        public int RankID { get; set; }
        public string UserID { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public int Mlevel { get; set; }
        public int MlevelChange { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public int CardCout { get; set; }
        public int TotalIntegral { get; set; }
        public int RepairCout { get; set; }
        public string AccntType { get; set; }
        public int IsChange { get; set; }
    }
}
