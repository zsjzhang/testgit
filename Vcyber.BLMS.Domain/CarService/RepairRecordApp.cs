using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public class RepairRecordApp : IRepairReportApp
    {
        public IEnumerable<RepairRecord> GetRepirRecordList(string identityNumber, string vin, DateTime? repairStartTime, DateTime? repairEndTime, string serviceType, int? start,
           int? count, string dealerid, out int totalCount)
       {
           return _DbSession.RepairRecordStorager.GetRepirRecordList(identityNumber, vin, repairStartTime, repairEndTime, serviceType, start, count,dealerid,
               out totalCount);
       }

        public Page<IFRepairReport> QueryServiceOrders(
            string identityNumber,
            EDMSServiceType4Q type,
            long page = 1,
            long itemsPerPage = 10000)
        {
            return _DbSession.RepairRecordStorager.QueryServiceOrders(identityNumber, type, page, itemsPerPage);
        }

    }
}
