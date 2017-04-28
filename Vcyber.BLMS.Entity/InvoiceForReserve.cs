using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    public class InvoiceForReserve
    {
        public int Id { get; set; }
        public string MembershipId { get; set; }
        public string ImageUrl { get; set; }
        public string ServiceType { get; set; }
        public DateTime CreateTime { get; set; }

        /*
         UserProofRecord 表新加字段  20160608 
         */
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string PaperWork { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IsDelete { get; set; }
        public int MLevel { get; set; }
        public string MLevelDisc
        {
            get
            {
                int level = 0;
                if (int.TryParse(this.MLevel.ToString(), out level))
                {
                    //return System.Enum.GetName(typeof(MemshipLevel), this.MLevel);
                    return ((MemshipLevel)this.MLevel).GetDiscribe();
                }
                else
                {
                    return this.ApproveStatus.ToString();
                }
            }
            
        }

        public string ImageProofFront { get; set; }
        public string ImageProofVerso { get; set; }
        public string ImageProofByHand { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int ApproveStatus { get; set; }

        public string ApproveStatusDiscribe
        {
            get
            {
                int status = 0;
                if(int.TryParse(this.ApproveStatus.ToString(),out status))
                {
                    //return System.Enum.GetName(typeof(EApproveStatus), this.ApproveStatus).geten;
                    return ((EApproveStatus)this.ApproveStatus).GetDiscribe();
                }
                else{
                    return this.ApproveStatus.ToString();
                }
            }
        }
    }
}
