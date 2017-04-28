using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.IRepository
{
    public interface IProductStorager
    {
        #region ==== 前台操作 ====

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        IEnumerable<Product> GetPageProduct(PageData pageData, out int totalCount);

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="categoryID">商品类型Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<Product> GetPageProduct(int categoryID, PageData pageData, out int totalCount);

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductById(int id);

        /// <summary>
        /// 获取商品更新数量
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        int RecentlyProduct(DateTime dateTime);

        /// <summary>
        /// 获取商品形式
        /// </summary>
        /// <param name="productID">商品ID</param>
        /// <returns></returns>
        int GetMode(int productID);

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
        /// 商品赞美
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        bool AddPraiseCount(int id);

        /// <summary>
        /// 获取商品赞美个数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int FindPraiseCount(int id);

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

        // <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(int id, EProductState productState, EDataStatus dataStatus);

        #region ==== 商品库存操作 ====

        /// <summary>
        /// 减去商品库存（积分）
        /// </summary>
        /// <param name="productID">商品ID</param>
        /// <param name="qty">减去数量</param>
        /// <returns>false:库存不足</returns>
        bool SubtractionQty(int productID, int xqty);

        /// <summary>
        /// 减去锁定库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        bool SubLockQty(int productId, int qty);

        /// <summary>
        /// 释放锁定库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        bool UnLQty(int productId, int qty);

        /// <summary>
        /// 获取用户购买某个商品的次数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        int TraderCount(string userId, int productId);

        #endregion

        #endregion

        #region ==== 后台操作 ====

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回商品Id</returns>
        int AddProduct(Product entity);

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditProduct(Product entity);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteProduct(int id);

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool PutOnProduct(int id);

        /// <summary>
        /// 商品下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool PutOffProduct(int id);

        /// <summary>
        /// 编辑商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        bool EditQty(int id, int qty);

        /// <summary>
        /// 添加商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        bool AddQty(int id, int qty);

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
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(int id);

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

        ProductImage GetImgOne(int imgId);

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

        /// 修改商品图片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateImg(ProductImage data);

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        IEnumerable<ProductImage> GetImage(int pid);

        /// <summary>
        /// 得到所有的产品类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductType> GetAllProductTypes();

        /// <summary>
        /// 通过产品Id查询产品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<ProductType> GetProductTypesByIds(string ids);

         IList<ProductColor> GetProductColorsByIds(string ids);

        /// <summary>
        /// 得到所有的产品颜色
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductColor> GetAllProductColors();

        /// <summary>
        /// 添加商品详细
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
        /// 获取商品类型
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        ProductCategory GetProductCategory(int pid);

        /// <summary>
        /// 添加商品类型关系信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddProductCategory(ProductCategory entity);

        /// <summary>
        /// 修改商品类型关系信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditProductCategory(ProductCategory entity);

        #endregion
    }
}
