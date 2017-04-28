using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
   public  class OnlineServiceRecord
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public string QuestionType { get; set; }
        public string DataSource { get; set; }
        public bool IsDel { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
    }
}
