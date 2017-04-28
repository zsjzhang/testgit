using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class ContentApproveModel
    {
        public int Id { get; set; }
        public int SouceId { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string TypeValue {
            get
            {
                return ((EApproveType) this.Type).GetDiscribe();
                
            }
        }

        public int Status { get; set; }

        public string StatusValue
        {
            get
            {
                return ((EApproveStatus)this.Status).GetDiscribe();
                
            }
        }

        public string UpdateTime { get; set; }
        public string UpdateBy { get; set; }

        public string ApproveTime { get; set; }

        public string ApproveBy { get; set; }
    }
}