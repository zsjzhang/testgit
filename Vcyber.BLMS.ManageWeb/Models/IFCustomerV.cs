using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Application;
using System.Reflection;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class IFCustomerV
    {
        #region ==== 构造函数 ====

        public IFCustomerV()
        {
            this.CustId = this.CreateCustId();
        }

        #endregion

        #region ==== 公共属性 ====

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
        public string VIN { get; set; }

        /// <summary>
        /// 是否是特殊车辆
        /// </summary>
        public bool TS { get; set; }

        public string CarCategory { get; set; }

        public string Buytime { get; set; }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        public bool AddInfo(out string message)
        {
            message = string.Empty;
            IFCustomer data = this.Copy<IFCustomerV, IFCustomer>();

            if (_AppContext.CarServiceUserApp.OldIsExistIdentity(data.IdentityNumber))
            {
                message = "身份证号已经存在。";
                return false;
            }
            IFCustomer c=null;
            if (_AppContext.CarServiceUserApp.OldGetCarInfoByVIN(this.VIN,out c) == null && !data.TS)
            {
                message = "根据VIN,没有查询到客户车辆。";
                return false;
            }
            if (data.TS && _AppContext.CarServiceUserApp.OldGetCarInfoByVIN(this.VIN,out c) != null)
            {
                message = "特殊车辆VIN存在。";
                return false;
            }

            _AppContext.CarServiceUserApp.AddCustomer(data);
            if (data.TS)
            {
                return _AppContext.CarServiceUserApp.InsertCarInfoByVIN(this.VIN, this.CustId,this.CarCategory,this.Buytime);
            }
            else
            { 
                return _AppContext.CarServiceUserApp.UpdateCarInfoByVIN(this.VIN, this.CustId);
            }
        }

        /// <summary>
        /// 验证属性信息
        /// </summary>
        /// <returns></returns>
        public bool ValidateProperties(out string messsage)
        {

            messsage = string.Empty;
            PropertyInfo[] pros = this.GetType().GetProperties();

            foreach (PropertyInfo item in pros)
            {
                object value=item.GetValue(this);

                if (!item.Name.Equals("Buytime") && !item.Name.Equals("CarCategory") && !item.Name.Equals("Email") && (value == null || string.IsNullOrEmpty(value.ToString())))
                {
                    messsage = "必须填写完整的客户信息。";
                    return false;
                }
            }

            if (!this.Gender.Trim().Equals("男")&&!this.Gender.Trim().Equals("女"))
            {
                messsage = "性别只能填写：男或女。";
                return false;
            }

            if (this.CustMobile.Length!=11||!CommonUtilitys.ValidatePhone(this.CustMobile))
            {
                messsage = "手机号格式不正确。";
                return false;
            }

            if (!string.IsNullOrEmpty(this.Email)&&!CommonUtilitys.ValidateEmail(this.Email))
            {
                messsage = "邮箱格式不正确。";
                return false;
            }


            return true;
        }

        #endregion

        #region ==== 私有方法 ====

        private string CreateCustId()
        {
            return string.Format("BM{0}",Math.Abs(Guid.NewGuid().GetHashCode()));
        }

        #endregion
    }
}