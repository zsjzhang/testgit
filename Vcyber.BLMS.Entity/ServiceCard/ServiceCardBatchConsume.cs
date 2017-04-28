using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ServiceCardBatchConsume
    {
        public int Id { get; set; }

        public int ServiceBatchId { get; set; }

        public int ConsumeScopeId { get; set; }
    }
}
