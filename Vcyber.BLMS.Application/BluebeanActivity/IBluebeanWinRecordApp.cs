using System.Collections.Generic;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IBluebeanWinRecordApp
    {
        string QueryWinRecord(string phone);
        IEnumerable<BluebeanWinRecord> QueryWinRecords(int quantity);

        BluebeanWinResult DrawLuck(string userId);

        BluebeanWinRecord QueryWinRecordByUserId(string userId);

        bool UpdateAddress(BluebeanWinRecord bluebeanWinRecord);
    }
}