using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class MembershipQueryEntity
    {
        public string Identity { get; set; }

        public string MLevel { get; set; }

        public int Skip { get; set; }

        public int Count { get; set; }
    }
}
