using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class CS_FreeRoadRescue
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Position { get; set; }

        public string IsFinish { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
