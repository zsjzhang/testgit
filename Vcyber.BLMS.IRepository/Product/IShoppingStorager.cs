using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 购物车操作
    /// </summary>
    public interface IShoppingStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加购物商品
        /// </summary>
        /// <param name="data"></param>
        void Add(Shopping data);

        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="shoppingId">购物车Id</param>
        /// <returns></returns>
        Shopping ShoppingOne(string userId, int shoppingId);

        /// <summary>
        /// 商品购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物车ID</param>
        /// <returns></returns>
        bool Delete(string userID, int id);

        /// <summary>
        /// 删除用户购物车
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        bool Delete(string userID);

        /// <summary>
        /// 批量删除用户购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="productIds">商品Id集合</param>
        /// <returns></returns>
        bool DeleteList(string userID, List<int> productIds);

        /// <summary>
        /// 修改购物车某个商品的购买数量
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物ID</param>
        /// <param name="qty">购买数量</param>
        /// <returns></returns>
        bool UpdateQty(string userID, int id, int qty);

        /// <summary>
        /// 获取用户某个商品的购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        Shopping SelectOne(string userID, int productID);

        /// <summary>
        /// 分页获取用户购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Shopping> SelectList(string userID, PageData pageData, out int total);

        #endregion
    }
}
