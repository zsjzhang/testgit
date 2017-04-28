using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.Entity
{
    public class CoreResultPara
    {
        public int State { get; set; }
        public int HttpStatusCode { get; set; }
        public string FlowNo { get; set; }
        public string Msg { get; set; }
    }
}