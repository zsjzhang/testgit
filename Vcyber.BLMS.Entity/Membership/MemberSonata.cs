using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class MemberSonata
    {
        public string CarId { get; set; }

        public string CustId { get; set; }

        public string DealerId { get; set; }

        public string CustMobile { get; set; }

        public string CustName { get; set; }

        public string VIN { get; set; }

        public string IdentityNumber { get; set; }

        public string Gender { get; set; }

        public string BuyTime { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string No { get; set; }

        public string MembershipId { get; set; }

        public string IsCanJoin { get; set; }
        public decimal Amount { get; set; }
        public int IsPay { get; set; }
        public string  IsPayState { get; set; }
        public string PaperWork { get; set; }
        public int Age { get; set; }

    }
}
