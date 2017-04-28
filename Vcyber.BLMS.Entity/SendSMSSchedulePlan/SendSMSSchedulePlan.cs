using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class SendSMSSchedulePlan
    {
        public int Id { get; set; }

        public string ServiceTitle { get; set; }

        public int TimeType { get; set; }

        public string TimeTypeName
        {
            get
            {
                if (TimeType == 1)
                    return "购车之日起多少天";
                if (TimeType == 2)
                    return "特定日期";
                else
                    return "即时执行";
            }
        }

        public DateTime? ScheduleTime { get; set; }

        public int ValueTime { get; set; }

        public string SMSContent { get; set; }

        public string IsOpen { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string CarCategory { get; set; }
    }
}
