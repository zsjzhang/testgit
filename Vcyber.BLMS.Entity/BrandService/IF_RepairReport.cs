using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class IF_RequestLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
    }
}
