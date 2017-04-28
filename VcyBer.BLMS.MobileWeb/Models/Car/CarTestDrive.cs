using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity.Generated;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class CarTestDrive
    {
        public string CarType { get; set; }
        public string SeriesName { get; set; }
        public List<CSBaseCar> CarTypeList { get; set; }
        public List<string> Provinces { get; set; }
    }
}