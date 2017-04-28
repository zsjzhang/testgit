using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class CarFeature
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public string SeriesName { get; set; }
        public List<string> ShopSlideUrlList { get; set; }
        public List<ArticleProduct> ArticleProductList { get; set; }
    }
    public class ArticleProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string  Content { get; set; }
    }
}