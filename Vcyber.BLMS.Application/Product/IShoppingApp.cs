using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 购物车业务
    /// </summary>
    public interface IShoppingApp
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
        /// 删除购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物车ID</param>
        /// <returns></returns>
        bool Delete(string userID, int id);

        /// <summary>
        /// 清理用户购物车
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        bool Clear(string userID);

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
        /// <param name="productID">商品Id</param>
        /// <returns></returns>
        Shopping GetOne(string userID, int productID);

        /// <summary>
        /// 分页获取用户购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Shopping> GetList(string userID, PageData pageData, out int total);

        bool DeleteList(string userID, List<int> productIds);

        #endregion
    }
}
