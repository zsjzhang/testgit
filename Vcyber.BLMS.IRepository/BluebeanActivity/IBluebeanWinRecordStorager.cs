using System.Collections.Generic;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IBluebeanWinRecordStorager
    {
        string QueryWinRecord(string phone);
        IEnumerable<BluebeanWinRecord> QueryWinRecords(int quantity);
        int GetCurrentWinPrizeCount();

        IEnumerable<BlueMemberPrize> GetPrizeByType(string prizeType);

        bool IsSelected(string userId);
        void Insert(BluebeanActiveRecord bluebeanActiveRecord);
        int InsertWinRecords(int prizeId,BluebeanWinRecord bluebeanWinRecord);

        BluebeanWinRecord QueryWinRecordByUserId(string userId);

        bool UpdateAddress(BluebeanWinRecord bluebeanWinRecord);
    }
}