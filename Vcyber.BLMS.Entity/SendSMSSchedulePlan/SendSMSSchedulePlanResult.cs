using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class SendSMSSchedulePlanResult
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PlanId { get; set; }

        public string IsSend { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? SendTime { get; set; }

        public string UserName { get; set; }

        public string ServiceTitle { get; set; }

        public string CarCategory { get; set; }
    }
}
