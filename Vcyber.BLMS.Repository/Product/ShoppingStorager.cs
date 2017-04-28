using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;
using System.Transactions;
using System.Configuration;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 购物车操作
    /// </summary>
    public class ShoppingStorager : IShoppingStorager
    {
        #region ==== 构造函数 ====

        public ShoppingStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加购物商品
        /// </summary>
        /// <param name="data"></param>
        public void Add(Shopping data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into shopping(UserID,ProductID,Qty,CreateTime,UpdateTime,DateState,BlueBean,Integral,ProductColor,ProductType) ");
            sql.Append(" values(@UserID,@ProductID,@Qty,@CreateTime,@UpdateTime,@DateState,@BlueBean,@Integral,@ProductColor,@ProductType) ");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 商品购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="id">购物车ID</param>
        /// <returns></returns>
        public bool Delete(string userID, int id)
        {
            string sql = "update shopping set DateState=@DataState,UpdateTime=@UpdateTime where UserID=@UserID and ID=@ID";
            return DbHelp.Execute(sql, new { DataState = EDataStatus.Delete.ToInt32(), UpdateTime = DateTime.Now, UserID = userID, ID = id }) > 0;
        }

        /// <summary>
        /// 删除用户购物车
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Delete(string userID)
        {
            string sql = "update shopping set DateState=@DataState,UpdateTime=@UpdateTime where UserID=@UserID";
            return DbHelp.Execute(sql, new { DataState = EDataStatus.Delete.ToInt32(), UpdateTime = DateTime.Now, UserID = userID }) > 0;
        }

        /// <summary>
        /// 批量删除用户购物车信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="productIds">商品Id集合</param>
        /// <returns></returns>
        public bool DeleteList(string userID, List<int> productIds)
        {
            string sql = string.Format("update shopping set DateState=@DataState,UpdateTime=@UpdateTime where UserID=@UserID and ProductID in({0})", string.Join(",", productIds));
            return DbHelp.Execute(sql, new { DataState = EDataStatus.Delete.ToInt32(), UpdateTime = DateTime.Now, UserID = userID }) > 0;
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
            string sql = "update shopping set Qty=@Qty where UserID=@UserID and ID=@ID";
            return DbHelp.Execute(sql, new { Qty = qty, UserID = userID, ID = id }) > 0;
        }

        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="shoppingId">购物车Id</param>
        /// <returns></returns>
        public Shopping ShoppingOne(string userId, int shoppingId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select a.*,b.Name from shopping a join product b on a.ProductID=b.ID where a.UserID=@UserID and a.DateState=@DateState and a.ID=@ID");
            return DbHelp.QueryOne<Shopping>(sql.ToString(), new { UserID = userId, DateState = EDataStatus.NoDelete.ToInt32(), ID=shoppingId });
        }

        /// <summary>
        /// 获取用户某个商品的购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID">商品Id</param>
        /// <returns></returns>
        public Shopping SelectOne(string userID, int productID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select top 1 shopping.*,product.Name,product.Image from shopping join product ");
            sql.Append(" on shopping.ProductID=product.ID where shopping.UserID=@UserID and shopping.DateState=@Datastate ");
            sql.Append(" and product.Datastate=@Datastate and shopping.ProductID=@ProductID ");
            return DbHelp.QueryOne<Shopping>(sql.ToString(), new { UserID = userID, ProductID = productID, Datastate = EDataStatus.NoDelete.ToInt32() });
        }


        /// <summary>
        /// 分页获取用户购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Shopping> SelectList(string userID, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from shopping join product on shopping.ProductID=product.ID ");
            sql.Append(" where shopping.UserID=@UserID and shopping.DateState=@Datastate and product.Datastate=@Datastate ");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserID = userID, Datastate = EDataStatus.NoDelete.ToInt32() });
            sql.Clear();

            sql.AppendFormat(" select top {0} shopping.*,product.Name,product.Image from shopping join product on shopping.ProductID=product.ID ", pageData.Size);
            sql.Append(" where shopping.UserID=@UserID and shopping.DateState=@Datastate and product.Datastate=@Datastate ");
            sql.Append(" and shopping.ID not in( ");
            sql.AppendFormat(" select top {0} shopping.ID from shopping join product on shopping.ProductID=product.ID  ", pageData.Index);
            sql.Append(" where shopping.UserID=@UserID and shopping.DateState=@Datastate and product.Datastate=@Datastate ");
            sql.Append(" order by shopping.CreateTime desc) ");
            sql.Append(" order by shopping.CreateTime desc ");
            var tempDatas = DbHelp.Query<Shopping>(sql.ToString(), new { UserID = userID, Datastate = EDataStatus.NoDelete.ToInt32() });

            if (tempDatas != null && tempDatas.Count() > 0)
            {
                var tempData2 = tempDatas.Select<Shopping, Shopping>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Shopping>();
            }

            return tempDatas;
        }

        #endregion
    }
}
