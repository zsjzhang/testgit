using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class ApproveRecordModel
    {
        public int ItemType { get; set; }
        public string ApproveStatusDescribe
        {
            get
            {
                switch (this.IsApproval)
                {
                    case 1: return "审核通过";
                    case 2: return "审核未通过";
                    default: return "未审核";
                }
            }
        }

        public int ItemId { get; set; }

        public string UpdateTime { get; set; }
        public int IsApproval { get; set; }

        public string ApprovalMemo { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; }
    }
}