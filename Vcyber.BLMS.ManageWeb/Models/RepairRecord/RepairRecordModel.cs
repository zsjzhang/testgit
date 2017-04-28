using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class RepairRecordModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairReportId { get; set; }

        /// <summary>
        /// 服务请求类型
        /// </summary>
        public string ServiceType { get; set; }

        public string ServiceTypeValue
        {
            get
            {
               foreach (int item in Enum.GetValues(typeof(EDMSServiceType)))
               {

                   if (((EDMSServiceType)item).DisplayName() == this.ServiceType)
                   {
                       return ((EDMSServiceType) item).GetDiscribe();
                   }
               }

                return "";
            }
            
        }

        /// <summary>
        /// 预约类型
        /// </summary>
        public string ReserveType
        {
            get
            {
                if(string.IsNullOrEmpty(BluememberBookID))
                {
                    return "直接到店";
                   
                }
                return "在线预约";
            }

        }

        /// <summary>
        /// 4S店ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 人工费
        /// </summary>
        public float LaborCost { get; set; }

        /// <summary>
        /// 配件费
        /// </summary>
        public float FittingsCost { get; set; }

        /// <summary>
        /// 外包费用
        /// </summary>
        public float DelegateCost { get; set; }

        /// <summary>
        /// VIN码
        /// </summary>
        public string VINCode { get; set; }

        /// <summary>
        /// 客户身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public string RepairTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public string FinishTime { get; set; }

        /// <summary>
        /// 状态（维修中/完成等）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// BLMS预定号
        /// </summary>
        public string BluememberBookID { get; set; }

        public string PhoneNumber { get; set; }
    }
}