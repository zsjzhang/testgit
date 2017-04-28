using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 购物车业务
    /// </summary>
    public class ShoppingApp : IShoppingApp
    {
        #region ==== 构造函数 ====

        public ShoppingApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加购物商品
        /// </summary>
        /// <param name="data"></param>
        public void Add(Shopping data)
        {
            var shopping = _DbSession.ShoppingStorager.SelectOne(data.UserID, data.ProductID);

            if (data==null||data.Qty<=0)
            {
                return;
            }

            if (shopping != null && data.ProductID == shopping.ProductID && data.ProductType == shopping.ProductType && data.ProductColor == shopping.ProductColor)
            {
                _DbSession.ShoppingStorager.UpdateQty(data.UserID, shopping.ID, shopping.Qty + data.Qty);
            }
            else
            {
                _DbSession.ShoppingStorager.Add(data);
            }
        }

        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="shoppingId">购物车Id</param>
        /// <returns></returns>
        public Shopping ShoppingOne(string userId, int shoppingId)
        {
            return _DbSession.ShoppingStorager.ShoppingOne(userId, shoppingId);
        }

        /// <summary>
        /// 删除购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物车ID</param>
        /// <returns></returns>
        public bool Delete(string userID, int id)
        {
            return _DbSession.ShoppingStorager.Delete(userID, id);
        }

        /// <summary>
        /// 清理用户购物车
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Clear(string userID)
        {
            return _DbSession.ShoppingStorager.Delete(userID);
        }

        /// <summary>
        /// 修改购物车某个商品的购买数量
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物ID</param>
        /// <param name="qty">购买数量</param>
        /// <returns></returns>
        public bool UpdateQty(string userID, int id, int qty)
        {
            return _DbSession.ShoppingStorager.UpdateQty(userID, id, qty);
        }

        /// <summary>
        /// 获取用户某个商品的购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Shopping GetOne(string userID, int productID)
        {
            return _DbSession.ShoppingStorager.SelectOne(userID, productID);
        }

        /// <summary>
        /// 分页获取用户购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Shopping> GetList(string userID, PageData pageData, out int total)
        {
            return _DbSession.ShoppingStorager.SelectList(userID, pageData, out total);
        }

        public bool DeleteList(string userID, List<int> productIds)
        {
            return _DbSession.ShoppingStorager.DeleteList(userID, productIds);
        }
        #endregion
    }
}
