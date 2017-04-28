using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerCondition
    {
        #region ==== 构造函数 ====

        public CustomerCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string IdentityNumber { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustId { get; set; }

        public bool IsValidate { get; private set; }

        public string whatType { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            StringBuilder sql = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(this.Name))
            {
                sql.AppendFormat(" and IF_Customer.CustName='{0}' ", this.Name);
            }

            if (!string.IsNullOrEmpty(this.Phone))
            {
                sql.AppendFormat(" and IF_Customer.CustMobile='{0}' ", this.Phone);
            }

            if (!string.IsNullOrEmpty(this.IdentityNumber))
            {
                sql.AppendFormat(" and IF_Customer.IdentityNumber='{0}' ", this.IdentityNumber);
            }
            if (!string.IsNullOrEmpty(this.CustId))
            {
                sql.AppendFormat(" and IF_Customer.CustId='{0}' ", this.CustId);
            }
            this.Validate();

            return sql.ToString();
        }

        #endregion

        #region ==== 私有方法 ====

        private bool Validate()
        {
            if (string.IsNullOrEmpty(this.Name) && string.IsNullOrEmpty(this.Phone) && string.IsNullOrEmpty(this.IdentityNumber))
            {
                return false;
            }

            this.IsValidate = true;
            return true;
        }

        #endregion
    }
}
