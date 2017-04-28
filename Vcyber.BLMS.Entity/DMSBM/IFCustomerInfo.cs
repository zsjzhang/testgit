using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class IFCustomerInfo
    {

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustName { get; set; }

        /// <summary>
        /// 客户手机号
        /// </summary>
        public string CustMobile { get; set; }

        /// <summary>
        /// 客户身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 客户性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 所属市区
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Agree { get; set; }

        public string PaperWork { get; set; }
    }
}
