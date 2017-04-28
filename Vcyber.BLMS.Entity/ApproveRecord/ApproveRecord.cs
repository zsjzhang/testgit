using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
   public class ApproveRecord
    {
       public int Id { get; set; }

       public string ItemType { get; set; }

       public int ItemId { get; set; }

       public DateTime CreateTime { get; set; }
       public DateTime UpdateTime { get; set; }
       public int IsApproval { get; set; }

       public string ApprovalMemo { get; set; }

       public string OperatorId { get; set; }

       public string OperatorName { get; set; }

    }
}
