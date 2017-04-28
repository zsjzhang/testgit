using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Generated;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class ExChangePolicy
    {
        public Activities Activity { get; set; }
        public IEnumerable<string> ProvinceList { get; set; }
        public IEnumerable<CSBaseCar> CarTypeList { get; set; }
    }
}