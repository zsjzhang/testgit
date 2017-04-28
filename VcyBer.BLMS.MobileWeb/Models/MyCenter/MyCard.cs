using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class MyCard
    {
        public UserCustomCardModel BmCardList { get; set; }
        public UserCustomCardModel PartnerCardList { get; set; }
        public List<SNCard> FlightNoUseCardList { get; set; }
        public List<SNCard> FlightCardUseList { get; set; }
        public List<SNCard> FlightCardOverUseList { get; set; }
    }
}