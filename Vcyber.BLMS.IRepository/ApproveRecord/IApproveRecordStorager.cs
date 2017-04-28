﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IApproveRecordStorager
    {
        bool UpdateStatus(ApproveRecord record);
        bool CreateApproveRecord(ApproveRecord record);
        IEnumerable<ApproveRecord> GetApproveRecordList(int itemId, string itemType);
        ApproveRecord GetLatestRecord(int itemId, string itemType);
    }
}
