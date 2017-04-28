using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IServiceCardBatchConsumeStorager
    {
        IEnumerable<ServiceCardBatchConsume> SelectServiceCardBatchConsumeList();

        IEnumerable<ServiceCardBatchConsume> SelectServiceCardBatchConsumeList(string serviceBatchId);

        bool AddServiceCardBatchConsume(ServiceCardBatchConsume item);

        bool DeleteServiceCardBatchConsume(string id);

        bool DeleteServiceCardBatchConsumeByBatchId(string batchId);
    }
}
