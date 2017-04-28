using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Report
{
    public class DealerStoryDto
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public List<string> ImgeUrls { get; set; }
    }
}
