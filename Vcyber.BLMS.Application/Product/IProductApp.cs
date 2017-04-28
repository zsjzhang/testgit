using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 商品业务
    /// </summary>
    public interface IProductApp
    {
        #region ==== 前台管理 ====

        /// <summary>
        /// 分页获取商品信息
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetPageProduct(PageData pageData, out int Total);

        /// <summary>
        /// 分页获取商品信息
        /// </summary>
        /// <param name="categoryId">商品类型Id</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetPageProduct(int categoryId, PageData pageData, out int total);

        Product GetProductById(int id);

        int RecentlyProduct(DateTime dateTime);

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="type">商品类型</param>
        /// <param name="mode">商品形式</param>
        /// <returns></returns>
        IEnumerable<Product> GetProduct(EProductType type, EProductMode mode);

        /// <summary>
        /// 根据推荐商品类型获取商品信息
        /// </summary>
        /// <param name="recommend">推荐类型</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetProduct(EProductRecommend recommend, PageData pageData, out int total);

        /// <summary>
        /// 获取销售排行商品
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetSalesProduct(PageData pageData, out int total);

        /// <summary>
        /// 用户是否可以赞美图片或者商品
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="memberId">会员id</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        bool IsParaise(EPraiseType type, string memberId, int value,out int empiricValue);

        /// <summary>
        /// 商品和图片赞美次数
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        int PraiseCount(EPraiseType type, int value);

        /// <summary>
        /// 赞美商品或者图片
        /// </summary>
        /// <param name="type">赞美类型</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="level">会员级别</param>
        /// <param name="value">赞美对象Id</param>
        /// <returns></returns>
        bool Praise(EPraiseType type, string memberId, MemshipLevel level, int value);

        /// <summary>
        /// 赞美商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        bool Praise(int id);

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetProduct(ProductSearchCondition condition, PageData pageData, out int total);

        /// <summary>
        /// 获取商品列表(生日特权专用)
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetProductByBirthday(ProductSearchCondition condition, PageData pageData, out int total);


        /// <summary>
        /// 判断某个用户在当年是否购买过某个类型的产品
        /// </summary>
        /// <param name="categoryId">类型ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns>购买的条数</returns>
         int checkProduct(int categoryId, string userID);

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(int id, EProductState productState, EDataStatus dataStatus);

        /// <summary>
        /// 发放卡券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="activityType">活动类型</param>
        /// <param name="cardName">卡券名称</param>
        /// <param name="count">发放数量</param>
        /// <param name="source">来源</param>
        void SendCard(string userId, string activityType, string cardName, int count, string source = "blms_wechat");

        #endregion

        #region ==== 后台管理 ====

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddProduct(Product entity);

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditProduct(Product entity);

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        bool DeleteProduct(int id);

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool PutOnProduct(int id);

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool PutOffProduct(int id);

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(int id);

        /// <summary>
        /// 编辑商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty">商品库存量</param>
        /// <returns></returns>
        bool EditQty(int id, int qty);

        /// <summary>
        /// 设置商品权重
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="weight">权重</param>
        /// <returns></returns>
        bool SetWeight(int productId, int weight);

        /// <summary>
        /// 设置商品推荐
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        bool SetRecommend(int productId, EProductRecommend recommend);

        /// <summary>
        /// 搜索商品信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetPageProduct(ProductCondition condition, PageData pageData, out int total);

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <returns></returns>
        ProductStatistics Statistics();

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddImage(ProductImage entity);

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteImage(int id);

        /// <summary>
        /// 设置商品默认图片
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="imgid"></param>
        /// <returns></returns>
        bool SetDefaultImage(int pid, int imgid);

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid">商品Id</param>
        /// <returns></returns>
        IEnumerable<ProductImage> GetImage(int pid);

        /// <summary>
        /// 添加商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddDetail(ProductDetail entity);

        /// <summary>
        /// 编辑商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditDetail(ProductDetail entity);

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        ProductDetail GetDetail(int pid);

        /// <summary>
        /// 获取某个商品的商品类型
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        ProductCategory GetProductCategory(int pid);

        /// <summary>
        /// 添加商品与商品类型关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddProductCategory(ProductCategory entity);

        /// <summary>
        /// 编辑商品与商品类型关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditProductCategory(ProductCategory entity);


        /// <summary>
        /// 得到所有的产品类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductType> GetAllProductTypes();

        /// <summary>
        /// 得到所有的产品颜色
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductColor> GetAllProductColors();

        #endregion
    }
}
