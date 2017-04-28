using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface ISC_ServiceCardUsedRecordStorager
    {
        IEnumerable<CustomCardInfo> BuyCustCard(string cardType, string vin);
        IEnumerable<SC_ServiceCardUsedRecord> BuySCServiceCardUsedRecord(string cardType);
        IEnumerable<Remeal> SelectRepairList(SC_ServiceCardUsedRecordSearchParam param);
        bool AddISCServiceCardUsedRecord(SC_ServiceCardUsedRecord record);

        SC_ServiceCardUsedRecord GetSCServiceCardUsedRecord(string cardType, string cardNo);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordList(SC_ServiceCardUsedRecordSearchParam param);

        bool Update(int id, string custName, int Mileage);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVin(string vin, string activityTag);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVinAndCardType(string vin, string CardType);

        IEnumerable<Remeal> SelectRemealByVin(string Vin, string CardType);
    }
}
