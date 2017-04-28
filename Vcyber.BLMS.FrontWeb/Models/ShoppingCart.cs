using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class ShoppingCart
    {
        public int totalQuantity { get; set; }
        public float totalPrice { get; set; }
        public float totalProductPrice { get; set; }
        public int totalBlueBeanQuantity { get; set; }
        public float totalBlueBean { get; set; }
        public int totalIntegralQuantity { get; set; }
        public float totalIntegral { get; set; }
        public IList<CartItem> productList { get; set; }
    }

    public class CartItem
    {
        public string productId { get; set; }
        public string skuId { get; set; }
        public string productName { get; set; }
        public float price { get; set; }
        public int quantity { get; set; }
        public string imgUrl { get; set; }
        public float blueBean { get; set; }
        public float Integral { get; set; }
        public string payType { get; set; }
        public string producttype { get; set; }
        public string productcolor { get; set; }
    }
}