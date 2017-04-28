using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class TradeInfoV
    {
        #region ==== 构造函数 ====

        public TradeInfoV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户Id
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 支付类型（2：积分；4：蓝豆）
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 商品收货地址
        /// </summary>
        public TradeAddressV Address { get; set; }

        /// <summary>
        /// 购买商品集合
        /// </summary>
        public List<TradeProductV> products { get; set; }

      
        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 验证交易数据
        /// </summary>
        /// <returns></returns>
        public bool ValidateData()
        {
            if (this.Address == null || this.products == null || this.products.Count == 0)
            {
                return false;
            }

            return true;
        }

        public List<OrderProduct> ConvertProductData()
        {
            List<OrderProduct> dataResult = new List<OrderProduct>(3);

            foreach (var item in this.products)
            {
                dataResult.Add(new OrderProduct() { ProductID = item.id, Qty = item.quantity,ProductColor = item.ProductColor,ProductType = item.ProductType,Name = item.ProductName});
            }

            return dataResult;
        }


        public OrderAddress ConvertOrderAddress()
        {
            return new OrderAddress()
            {
                ReceiveName = this.Address.receiveName,
                Phone = this.Address.phone,
                PCC = this.Address.pCC,
                ZipCode = this.Address.zipCode,
                Detail = this.Address.detail,
                Province = this.Address.provincesId,
                City = this.Address.cityId,
                County = this.Address.areaId
            };
        }

        #endregion
    }


}