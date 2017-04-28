using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.SQL.FrontWeb.QueryEntity;
using Omu.ValueInjecter;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MembershipExtraQueryViewModel
    {
        public string RealName { get; set; }

        public string VIN { get; set; }

        public int Skip { get; set; }

        public int Count { get; set; }

        public FrontUserExtraQueryEntity ToEntity()
        {
            var entity = new FrontUserExtraQueryEntity();
            entity.InjectFrom(this);
            return entity;
        }
    }
}