using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public interface IRepairRecordStorager
   {
        IEnumerable<RepairRecord> GetRepirRecordList(string identityNumber, string vin, DateTime? repairStartTime, DateTime? repairEndTime, string serviceType, int? start, int? count, string dealerid,
           out int totalCount);

        Page<IFRepairReport> QueryServiceOrders(
            string identityNumber,
            EDMSServiceType4Q type,
            long page = 1,
            long itemsPerPage = 10000);

   }
}
