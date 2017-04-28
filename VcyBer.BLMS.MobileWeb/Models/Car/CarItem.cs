using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class CarItem
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public string SeriesName { get; set; }
        public List<CarPrice> CarPriceList { get; set; }
        public List<CarPrice> CarPrices { get; set; }
    }
    public class CarPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}