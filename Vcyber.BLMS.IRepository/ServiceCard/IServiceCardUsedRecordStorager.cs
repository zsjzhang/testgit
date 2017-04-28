using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IServiceCardUsedRecordStorager
    {
        IEnumerable<ServiceCardUsedRecord> SelectServiceCardUsedRecordList();

        bool AddServiceCardUsedRecord(ServiceCardUsedRecord record);
    }
}
