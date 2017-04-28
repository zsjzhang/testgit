using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 经销商信息
    /// </summary>
    public class DealerShipV
    {
        #region ==== 构造函数 ====

        public DealerShipV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 数据标识ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 4S店ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 地理坐标
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所属市区
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 销售电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 淘宝账号
        /// </summary>
        public string TBAccount { get; set; }

        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string AlipayAccount { get; set; }

        /// <summary>
        /// 售后电话
        /// </summary>
        public string AfterSalesPhone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 道路救援电话
        /// </summary>
        public string FreeRoadRescuePhone { get; set; }

        /// <summary>
        /// 是否前台显示
        /// </summary>
        public int IsDel { get; set; }


        public int Istestserver { get; set; }

        public int IsDingChe { get; set; }

        public int IsWeibao { get; set; }



        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加经销商信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddData(out string message)
        {
            message = string.Empty;
            this.Id = Common.CommonUtilitys.CreateNumber();

            if (this.ValidateData(out message) == false)
            {
                return false;
            }

            if (_AppContext.DealerApp.findOne(this.DealerId) != null)
            {
                message = "店代码重复。";
                return false;
            }

            _AppContext.DealerApp.add(Common.CommonUtilitys.CopyData<DealerShipV, CSCarDealerShip>(this));
            return true;
        }

        /// <summary>
        /// 编辑经销商信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool EditeData(out string message)
        {
            message = string.Empty;

            if (this.ValidateData(out message) == false)
            {
                return false;
            }

            if (_AppContext.DealerApp.findOne(this.DealerId) == null)
            {
                message = "必须填写店代码。";
                return false;
            }

            _AppContext.DealerApp.update(this.DealerId, Common.CommonUtilitys.CopyData<DealerShipV, CSCarDealerShip>(this));
            return true;
        }

        #endregion

        #region ==== 私有方法 ====

        private bool ValidateData(out string message)
        {
            message = string.Empty;

            PropertyInfo[] pros = this.GetType().GetProperties();

            foreach (PropertyInfo item in pros)
            {
                object value = item.GetValue(this);

                if (!item.Name.Equals("FreeRoadRescuePhone") && !item.Name.Equals("Email") && !item.Name.Equals("TBAccount")
                    && !item.Name.Equals("AlipayAccount") && !item.Name.Equals("Position")
                    && (value == null || string.IsNullOrEmpty(value.ToString())))
                {
                    message = "必须填写完整的经销商信息。";
                    return false;
                }
                if (item.PropertyType == typeof(string))
                {
                    item.SetValue(this, value == null ? value : value.ToString().Trim());
                }
            }

            //if (_AppContext.DealerApp.validateData(this.DealerId, this.Name))
            //{
            //    message = "全称重复。";
            //    return false;
            //}

            //if (_AppContext.DealerApp.validateData(this.DealerId, this.Abbreviation))
            //{
            //    message = "简称重复。";
            //    return false;
            //}

            if (this.Province.Equals("-1") || this.City.Equals("-1"))
            {
                message = "请选择省市。";
                return false;
            }

            //if (!string.IsNullOrEmpty(this.Email) && Common.CommonUtilitys.ValidateEmail(this.Email) == false)
            //{
            //    message = "邮箱格式不合法。";
            //    return false;
            //}

            //if (!string.IsNullOrEmpty(this.Position) && this.Position.Split(',').Length != 2)
            //{
            //    message = "经度与纬度必须都填写。";
            //    return false;
            //}

            return true;
        }

        #endregion
    }
}