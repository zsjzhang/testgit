using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class CustomerServiceMessage
    {
        public string openid { get; set; }
        public string opercode { get; set; }
        public string text { get; set; }
        public long time { get; set; }
        public DateTime opertime { get; set; }
        public string worker { get; set; }
        public long timestamp { get; set; }
    }

    public class CustomerServiceMessageQuery 
    {
        public DateTime CurrDate { get; set; }
        public List<CustomerServiceMessage> Items { get; set; }
    }
}
