using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.SQL.FrontWeb.QueryEntity
{
    public class FrontUserExtraQueryEntity
    {
        public string RealName { get; set; }

        public string VIN { get; set; }

        public int Skip { get; set; }

        public int Count { get; set; }
    }
}
