using System;

namespace Vcyber.BLMS.Entity
{
    public class MembershipBrand
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string BrandName { get; set; }

        public string ServiceCode { get; set; }

        public string IsMember { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime JoinTime { get; set; }
    }
}
