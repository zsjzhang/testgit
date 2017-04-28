using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Membership
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }

        /// <summary>
        /// 会员等级1-注册用户 10-普卡  11-银卡 12-金卡
        /// </summary>
        public string MLevel { get; set; }

        public string VIN { get; set; }

        public string IdentityNumber { get; set; }

        public string Gender { get; set; }

        public string No { get; set; }
        public string DealerId { get; set; }
        public string CustName { get; set; }
        public string PayNumber { get; set; }
       
        public string CreateTime { get; set; }

        public string CreatedPerson { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string CarCategory { get; set; }
        public string NickName { get; set; }

      

        /// <summary>
        /// 会员失效时间
        /// </summary>
        public DateTime MLevelInvalidDate { get; set; }

        /// <summary>
        /// 会员生效时间
        /// </summary>
        public DateTime MLevelBeginDate { get; set; }

        public DateTime? Birthday { get; set; }
        //积分总量
        public int TotalIntegral { get; set; }
        //使用积分
        public int Shiyongintegral { get; set; }

        //剩余积分 
        public int Shenyuintegral { get; set; }

        public int SystemMType { get; set; }

        public int MType { get; set; }

        public int IsPay { get; set; }

        public string Address { get; set; }
    }
}
