using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models.Membership
{
    public class MemberShipProofModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        public string MembershipId { get; set; }

        public string PaperWork { get; set; }
        public string IdentityNumber { get; set; }

        public string CreateTime { get; set; }

        public string MLevelDisc { get; set; }

        public string ImageProofFront { get; set; }
        public string ImageProofVerso { get; set; }
        public string ImageProofByHand { get; set; }

        public int ApproveStatus { get; set; }
        public string ApproveStatusDiscribe { get; set; }
    }
}