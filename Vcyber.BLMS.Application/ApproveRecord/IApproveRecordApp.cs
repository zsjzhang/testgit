using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IApproveRecordApp
    {
        bool UpdateStatus(ApproveRecord record);
        IEnumerable<ApproveRecord> GetApproveRecordList(int itemId, string itemType);
        ApproveRecord GetLatestRecord(int itemId, string itemType);

    }
}
