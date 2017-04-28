using System;
using System.Collections.Generic;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface ISC_ServiceCardUsedRecordApp
    {
        IEnumerable<Remeal> SelectRepairList(SC_ServiceCardUsedRecordSearchParam param);
        ReturnResult ConfirmUseCard(SC_ServiceCardUsedRecord record);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordList(SC_ServiceCardUsedRecordSearchParam param);

        ReturnResult GetServiceCardInfo(string cardId, string code, string activity);

        ReturnResult ServiceCardConsum(string cardId, string code, string activity);

        ReturnResult UpdateRecord(int id, string custName, int Mileage);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVin(string vin, string activityTag);

        IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVinAndCardType(string vin, string CardType);

        RecommendCustomer GetRecommendNameByPhone(string phone);

        CustomCardInfo GetCustomTypeByVin(string Vin, string ActivityType);

        IEnumerable<Remeal> SelectRemealByVin(string Vin, string CardType);
    }
}
