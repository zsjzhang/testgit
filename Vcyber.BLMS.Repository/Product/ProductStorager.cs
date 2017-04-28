using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 商品信息操作
    /// </summary>
    public class ProductStorager : IProductStorager
    {
        #region ==== 构造函数 ====

        public ProductStorager() { }

        #endregion

        #region ==== 前台操作 ====

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(PageData pageData, out int totalCount)
        {
            totalCount = DbHelp.ExecuteScalar<int>("select COUNT(*) from product where Datastate=@Datastate and State=@State ", new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select top {0} * from product", pageData.Size);
            sql.Append(" where Datastate=@Datastate and State=@State ");
            sql.AppendFormat(" and ID not in(select top {0} ID from product where Datastate=@Datastate and State=@State order by product.Weight desc,Createtime desc)", pageData.Index);
            sql.Append(" order by product.Weight desc,Createtime desc");

            var tempData = DbHelp.Query<Product>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="categoryID">商品类型Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(int categoryID, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  select COUNT(1) from product join productcategory ");
            sql.Append("  on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID ");
            sql.Append("  where category.ID=@CategoryID or category.ParentID=@CategoryID and product.Datastate=@Datastate  ");
            sql.Append("  and productcategory.Datastate=@Datastate and product.State=@State ");
            sql.Append("  and category.Datastate=@Datastate  ");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { CategoryID = categoryID, Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            sql.Clear();

            sql.AppendFormat("  select top {0} product.* from product join productcategory ", pageData.Size);
            sql.Append(" on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID");
            sql.Append(" where category.ID=@CategoryID or category.ParentID=@CategoryID and product.Datastate=@Datastate ");
            sql.Append(" and productcategory.Datastate=@Datastate and product.State=@State");
            sql.Append(" and category.Datastate=@Datastate and product.ID not in(");
            sql.AppendFormat(" select top {0} product.ID from product join productcategory ", pageData.Index);
            sql.Append(" on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID");
            sql.Append(" where category.ID=@CategoryID or category.ParentID=@CategoryID and product.Datastate=@Datastate ");
            sql.Append(" and productcategory.Datastate=@Datastate and product.State=@State");
            sql.Append(" and category.Datastate=@Datastate order by product.Weight desc,product.Createtime desc");
            sql.Append(" ) order by product.Weight desc,product.Createtime desc");

            var tempData = DbHelp.Query<Product>(sql.ToString(), new { CategoryID = categoryID, Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(int id)
        {
            var product = DbHelp.QueryOne<Product>("SELECT a.ProductTypeIds,a.ProductColorIds,a.ID,a.Name,c.CategoryID,d.Name as CategoryName,a.Title,a.Image,a.integral,a.price,a.qty,a.unit,a.Provider,a.Createtime,a.Mode,a.MaxPer,a.MaxUser,a.Returned,b.Description,a.BlueBean,a.Sales,a.State , GoldMemberIntegral ,SilverMemberIntegral ,IsIdentity from product a LEFT JOIN productdetail b on a.ID=b.ProductID left join productcategory c on a.ID=c.ProductID left join category d on c.CategoryID=d.ID where a.Datastate=0 and a.ID=@ID", new { ID = id });

            if (product != null && !string.IsNullOrEmpty(product.Image))
            {
                if (!string.IsNullOrEmpty(product.ProductTypeIds))
                {
                    product.ProductTypeList = GetProductTypesByIds(product.ProductTypeIds);
                }
                if (!string.IsNullOrEmpty(product.ProductColorIds))
                {
                    product.ProductColorList = GetProductColorsByIds(product.ProductColorIds);
                }
                product.Image = ConfigurationManager.AppSettings["ImgPath"] + product.Image;

                if (!string.IsNullOrEmpty(product.Description))
                {
                    product.Description = product.Description.Replace("/upload/image", ConfigurationManager.AppSettings["ImgPath"] + "/upload/image");

                }
            }

            return product;
        }

        /// <summary>
        /// 获取销售量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetSalesById(int id)
        {
            return DbHelp.ExecuteScalar<int>("SELECT COUNT(*) from orderproduct a LEFT JOIN orders b on a.OrderID=b.OrderID where a.ProductID=@ProductID and b.Type=2", new { ProductID = id });
        }

        /// <summary>
        /// 商品赞美
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public bool AddPraiseCount(int id)
        {
            string sql = "update product set PraiseCount=ISNULL(PraiseCount,0)+1 where ID=@ID";
            return DbHelp.Execute(sql, new { ID = id }) > 0;
        }

        /// <summary>
        /// 获取商品赞美个数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int FindPraiseCount(int id)
        {
            return DbHelp.ExecuteScalar<int>("select product.PraiseCount from product where product.ID=@ID", new { ID = id });
        }

        /// <summary>
        /// 获取商品更新数量
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int RecentlyProduct(DateTime dateTime)
        {
            return DbHelp.ExecuteScalar<int>("SELECT count(*) FROM product WHERE Datastate=0 and Type=2 and Createtime>@Createtime", new { Createtime = dateTime });
        }

        /// <summary>
        /// 获取商品形式
        /// </summary>
        /// <param name="productID">商品ID</param>
        /// <returns></returns>
        public int GetMode(int productID)
        {
            var commandText = "select Mode from product where ID=@ID";
            return DbHelp.ExecuteScalar<int>(commandText, new { ID = productID });
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="type">商品类型</param>
        /// <param name="mode">商品形式</param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(EProductType type, EProductMode mode)
        {
            var comandText = "select ID,Name,Title from Product where Type=@Type and Mode=@Mode and State=@State and Datastate=0";
            var tempData = DbHelp.Query<Product>(comandText, new { Type = type.ToInt32(), Mode = mode.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 根据推荐商品类型获取商品信息
        /// </summary>
        /// <param name="recommend">推荐类型</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(EProductRecommend recommend, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from product join productcategory on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID where IsRecommend=@IsRecommend and product.Datastate=@Datastate and category.Datastate=@Datastate and State=@State");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { IsRecommend = recommend.ToInt32(), Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });
            sql.Clear();

            sql.AppendFormat(" select top {0} product.* from product join productcategory on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID where IsRecommend=@IsRecommend and product.Datastate=@Datastate and category.Datastate=@Datastate and State=@State", pageData.Size);
            sql.Append(" and product.ID not in(");
            sql.AppendFormat(" select top {0} product.ID from product join productcategory on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID where IsRecommend=@IsRecommend and product.Datastate=@Datastate and category.Datastate=@Datastate and State=@State", pageData.Index);
            sql.Append(" order by product.Createtime desc");
            sql.Append(" )order by product.Createtime desc ");
            var tempData = DbHelp.Query<Product>(sql.ToString(), new { IsRecommend = recommend.ToInt32(), Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 获取销售排行商品
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetSalesProduct(PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from product where  Datastate=@Datastate and State=@State");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });
            sql.Clear();

            sql.AppendFormat(" select top {0} * from product where  Datastate=@Datastate and State=@State ", pageData.Size);
            sql.Append(" and product.ID not in( ");
            sql.AppendFormat(" select top {0} product.ID from product where  Datastate=@Datastate and State=@State", pageData.Index);
            sql.Append(" order by product.Sales desc ");
            sql.Append(" )order by product.Sales desc ");

            var tempData = DbHelp.Query<Product>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProduct(ProductSearchCondition condition, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select count(1) from product {0} ", condition.ToJoin());
            sql.AppendFormat(" where 1=1 {0} {1} and product.Datastate=@Datastate   and State=@State", condition.ToWhereJoin(), condition.ToWherePayType());
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });
            sql.Clear();

            sql.AppendFormat(" select top {0} product.* from product {1}", pageData.Size, condition.ToJoin());
            sql.AppendFormat(" where 1=1 {0} {1} and product.Datastate=@Datastate and product.State=@State", condition.ToWhereJoin(), condition.ToWherePayType());
            sql.Append(" and product.ID not in( ");
            sql.AppendFormat(" select top {0} product.ID from product {1}", pageData.Index, condition.ToJoin());
            sql.AppendFormat(" where 1=1 {0} {1} and product.Datastate=@Datastate and product.State=@State", condition.ToWhereJoin(), condition.ToWherePayType());
            sql.AppendFormat(" order by {0}product.Createtime desc)", condition.ToWherePaiXu());
            sql.AppendFormat(" order by {0}product.Createtime desc", condition.ToWherePaiXu());
            var tempData = DbHelp.Query<Product>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 获取商品列表(生日特权专用)
        /// </summary>
        /// <param name="condition">商品搜索条件</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProductByBirthday(ProductSearchCondition condition, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select count(1) from product {0} ", condition.ToJoin());
            sql.AppendFormat(" where product.Datastate=@Datastate   and State=@State",condition.ToWherePayType());
            if (condition.CategoryID > 0)
            {
             sql.AppendFormat(" and productcategory.Datastate=@Datastate and category.Datastate=@CDatastate and ( category.ID=" + condition.CategoryID + " or category.ParentID=" + condition.CategoryID+" ) ");   
            }
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32(),CDatastate=EDataStatus.Delete.ToInt32() });
            sql.Clear();

            sql.AppendFormat(" select top {0} product.* from product {1}", pageData.Size, condition.ToJoin());
            sql.AppendFormat(" where  product.Datastate=@Datastate and product.State=@State",condition.ToWherePayType());
             if (condition.CategoryID > 0)
            {
             sql.AppendFormat(" and productcategory.Datastate=@Datastate and category.Datastate=@CDatastate and ( category.ID=" + condition.CategoryID + " or category.ParentID=" + condition.CategoryID+" ) ") ;  
            }
            sql.Append(" and product.ID not in( ");
            sql.AppendFormat(" select top {0} product.ID from product {1}", pageData.Index, condition.ToJoin());
            sql.AppendFormat(" where  product.Datastate=@Datastate and product.State=@State",condition.ToWherePayType());
             if (condition.CategoryID > 0)
            {
                sql.AppendFormat(" and productcategory.Datastate=@Datastate and category.Datastate=@CDatastate and ( category.ID=" + condition.CategoryID + " or category.ParentID=" + condition.CategoryID + " ) ");  
            }
            sql.AppendFormat(" order by {0}product.Createtime desc)", condition.ToWherePaiXu());
            sql.AppendFormat(" order by {0}product.Createtime desc", condition.ToWherePaiXu());
            var tempData = DbHelp.Query<Product>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.Putaway.ToInt32(),CDatastate=EDataStatus.Delete.ToInt32() });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }


        /// <summary>
        /// 判断某个用户在当年是否购买过某个类型的产品
        /// </summary>
        /// <param name="categoryId">类型ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns>购买的条数</returns>
        public int checkProduct(int categoryId, string userID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select COUNT(1) from orderproduct op,orders o,productcategory pc");
            sql.AppendFormat(" where op.OrderID=o.OrderID and op.ProductID=pc.ProductID");
            sql.AppendFormat(" and pc.CategoryID={0} and o.UserID='{1}'",categoryId,userID);
            sql.AppendFormat(" and YEAR(o.Createtime)=YEAR(GETDATE())");
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id, EProductState productState, EDataStatus dataStatus)
        {
            var commandText = @"select * from Product where ID=@ID and Datastate=@Datastate and State=@State";
            var tempData = DbHelp.QueryOne<Product>(commandText, new { ID = id, Datastate = dataStatus.ToInt32(), State = productState.ToInt32() });

            if (tempData != null)
            {
                if (!string.IsNullOrEmpty(tempData.ProductTypeIds))
                {
                    tempData.ProductTypeList = GetProductTypesByIds(tempData.ProductTypeIds);
                }
                if (!string.IsNullOrEmpty(tempData.ProductColorIds))
                {
                    tempData.ProductColorList = GetProductColorsByIds(tempData.ProductColorIds);
                }
                tempData.Image = ConfigurationManager.AppSettings["ImgPath"] + tempData.Image;
            }

            return tempData;
        }

        #region ==== 商品库存操作 ====

        /// <summary>
        /// 减去商品库存（积分）
        /// </summary>
        /// <param name="productID">商品ID</param>
        /// <param name="qty">减去数量</param>
        /// <returns>false:库存不足</returns>
        public bool SubtractionQty(int productID, int xqty)
        {
            if (xqty <= 0)
            {
                return false;
            }

            var commandText = "UPDATE product SET Qty=isnull(Qty,0)-@Qty,Sales=isnull(Sales,0)+@Qty,LQty=isnull(LQty,0)+@Qty WHERE ID=@productID and qty>=@Qty";
            return DbHelp.Execute(commandText, new { Qty = xqty, productID = productID }) > 0;
        }

        /// <summary>
        /// 减去锁定库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool SubLockQty(int productId, int qty)
        {
            if (qty <= 0)
            {
                return false;
            }

            string sql = "UPDATE product SET LQty=isnull(LQty,0)-@Qty WHERE ID=@productID and isnull(LQty,0)-@Qty>=0";
            return DbHelp.Execute(sql, new { Qty = qty, productID = productId }) > 0;
        }

        /// <summary>
        /// 释放锁定库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool UnLQty(int productId, int qty)
        {
            if (qty <= 0)
            {
                return false;
            }

            string sql = "UPDATE product SET Qty=isnull(Qty,0)+@Qty,Sales=isnull(Sales,0)-@Qty,LQty=isnull(LQty,0)-@Qty  WHERE ID=@productID and isnull(LQty,0)-@Qty>=0";
            return DbHelp.Execute(sql, new { Qty = qty, productID = productId }) > 0;
        }

        /// <summary>
        /// 获取用户购买某个商品的次数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public int TraderCount(string userId, int productId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select COUNT(1) from orders join orderproduct on orders.OrderID=orderproduct.OrderID");
            sql.Append(" where orders.UserID=@UserID and orderproduct.ProductID=@ProductID and orders.tradestate in(2,6,17)");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserID = userId, ProductID = productId });
        }

        #endregion

        #endregion

        #region ==== 后台操作 ====

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回商品Id</returns>
        public int AddProduct(Product entity)
        {
            entity.Createtime = DateTime.Now;

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into Product(Name,Title,Type,Mode,Image,Integral,Price,Qty,LQty,Unit,MaxPer,MaxUser,IsIdentity,Sales,IsDisplay,State,ShelfTime,");
            sql.Append(" Returned,Provider,Datastate,Createtime,Updatetime,MaxFree,BlueBean,IsRecommend,Weight,ProductTypeIds,ProductColorIds, GoldMemberIntegral,SilverMemberIntegral) ");
            sql.Append(" values(@Name,@Title,@Type,@Mode,@Image,@Integral,@Price,@Qty,@LQty,@Unit,@MaxPer,@MaxUser,@IsIdentity,@Sales,@IsDisplay,@State,@ShelfTime,");
            sql.Append(" @Returned,@Provider,@Datastate,@Createtime,@Updatetime,@MaxFree,@BlueBean,@IsRecommend,@Weight,@ProductTypeIds,@ProductColorIds,@GoldMemberIntegral,@SilverMemberIntegral); select @@identity;");

            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditProduct(Product entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  update Product set Name=@Name,Title=@Title,Type=@Type,Mode=@Mode,Image=@Image,Integral=@Integral,IsIdentity=@IsIdentity,");
            sql.Append("  Price=@Price,Qty=@Qty,Unit=@Unit,MaxPer=@MaxPer,MaxUser=@MaxUser,MaxFree=@MaxFree,");
            sql.Append("  IsDisplay=@IsDisplay,State=@State,ShelfTime=@ShelfTime,Returned=@Returned,Provider=@Provider,Updatetime=@Updatetime");
            sql.Append(" ,BlueBean=@BlueBean,IsRecommend=@IsRecommend,Weight=@Weight,ProductTypeIds=@ProductTypeIds,ProductColorIds=@ProductColorIds ,GoldMemberIntegral=@GoldMemberIntegral, SilverMemberIntegral=@SilverMemberIntegral");
            sql.Append("  where ID=@ID");

            return DbHelp.Execute(sql.ToString(), entity) > 0;
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteProduct(int id)
        {
            var commandText = @"update Product set Datastate=@Datastate,Updatetime=@Updatetime where ID=@ID";
            return DbHelp.Execute(commandText, new { Datastate = EDataStatus.Delete.ToInt32(), Updatetime = DateTime.Now, ID = id }) > 0;
        }

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PutOnProduct(int id)
        {
            var commandText = @"update Product set state=@State,ShelfTime=@ShelfTime,Updatetime=@Updatetime where ID=@ID";
            return DbHelp.Execute(commandText, new { State = EProductState.Putaway.ToInt32(), ShelfTime = DateTime.Now, Updatetime = DateTime.Now, ID = id }) > 0;
        }

        /// <summary>
        /// 商品下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PutOffProduct(int id)
        {
            var commandText = @"update Product set state=@State,Updatetime=@Updatetime where ID=@ID";
            return DbHelp.Execute(commandText, new { State = EProductState.SoldOut.ToInt32(), Updatetime = DateTime.Now, ID = id }) > 0;
        }

        /// <summary>
        /// 编辑商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool EditQty(int id, int qty)
        {
            var commandText = @"update Product set Qty=@Qty where ID=@ID";
            int count = DbHelp.Execute(commandText, new { Qty = qty, ID = id });
            return count > 0;
        }

        /// <summary>
        /// 添加商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool AddQty(int id, int qty)
        {
            var commandText = @"update Product set Qty=Qty+@Qty where ID=@ID";
            int count = DbHelp.Execute(commandText, new { Qty = qty, ID = id });
            return count > 0;
        }

        /// <summary>
        /// 设置商品权重
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="weight">权重</param>
        /// <returns></returns>
        public bool SetWeight(int productId, int weight)
        {
            string sql = "update product set Weight=@Weight where ID=@ID";
            return DbHelp.Execute(sql, new { Weight = weight, ID = productId }) > 0;
        }

        /// <summary>
        /// 设置商品推荐
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recommend"></param>
        /// <returns></returns>
        public bool SetRecommend(int productId, EProductRecommend recommend)
        {
            string sql = "update product set IsRecommend=@IsRecommend where ID=@ID";
            return DbHelp.Execute(sql, new { IsRecommend = recommend.ToInt32(), ID = productId }) > 0;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            var commandText = @"select a.* from Product a where a.ID=@ID ";
            var tempData = DbHelp.QueryOne<Product>(commandText, new { ID = id });

            if (tempData != null)
            {
                if (!string.IsNullOrEmpty(tempData.ProductTypeIds))
                {
                    tempData.ProductTypeList = GetProductTypesByIds(tempData.ProductTypeIds);
                }
                if (!string.IsNullOrEmpty(tempData.ProductColorIds))
                {
                    tempData.ProductColorList = GetProductColorsByIds(tempData.ProductColorIds);
                }
                tempData.Image = ConfigurationManager.AppSettings["ImgPath"] + tempData.Image;
            }

            return tempData;
        }

        /// <summary>
        /// 搜索商品信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetPageProduct(ProductCondition condition, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(tempTable.ID) from(");
            sql.Append(" select product.*,productcategory.CategoryID,ABS(product.Qty)-ABS(product.Sales) as Yjx from product join productcategory on product.ID=productcategory.ProductID");
            sql.Append(" where product.Datastate=0 and productcategory.Datastate=0");
            sql.AppendFormat(" ) as tempTable where {0}", condition.ToWhere());
            total = DbHelp.ExecuteScalar<int>(sql.ToString(),condition );

            sql.Clear();

            sql.AppendFormat("  select top {0} tempTable.* from(", pageData.Size);
            sql.Append(" select product.*,productcategory.CategoryID,ABS(product.Qty)-ABS(product.Sales) as Yjx from product join productcategory on product.ID=productcategory.ProductID");
            sql.Append(" where product.Datastate=0 and productcategory.Datastate=0");
            sql.AppendFormat(" ) as tempTable where {0} and tempTable.ID not in(", condition.ToWhere());
            sql.AppendFormat(" select top {0} tempTable.ID from(", pageData.Index);
            sql.Append(" select product.*,productcategory.CategoryID,ABS(product.Qty)-ABS(product.Sales) as Yjx from product join productcategory on product.ID=productcategory.ProductID");
            sql.Append(" where product.Datastate=0 and productcategory.Datastate=0");
            sql.AppendFormat(" ) as tempTable where {0} order by tempTable.Createtime desc", condition.ToWhere());
            sql.Append(" )");
            sql.Append(" order by tempTable.Createtime desc");

            var tempData = DbHelp.Query<Product>(sql.ToString(),condition);

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<Product, Product>((d) =>
                {
                    if (!string.IsNullOrEmpty(d.ProductTypeIds))
                    {
                        d.ProductTypeList = GetProductTypesByIds(d.ProductTypeIds);
                    }
                    if (!string.IsNullOrEmpty(d.ProductColorIds))
                    {
                        d.ProductColorList = GetProductColorsByIds(d.ProductColorIds);
                    }
                    d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image;
                    return d;
                });
                return tempData2.ToList<Product>();
            }

            return tempData;
        }

        /// <summary>
        /// 商品统计
        /// </summary>
        /// <returns></returns>
        public ProductStatistics Statistics()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select (select COUNT(1) from product where product.Datastate=@Datastate) as ProductCount");
            sql.Append(" (select COUNT(1) from product where product.State=@State) as XJCount");
            sql.Append(" (select COUNT(1) from product where product.IsRecommend=@xp) as XPCount");
            sql.Append(" (select COUNT(1) from product where product.IsRecommend=@rx) as RXCount");
            return DbHelp.QueryOne<ProductStatistics>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), State = EProductState.SoldOut.ToInt32(), xp = EProductRecommend.XP.ToInt32(), rx = EProductRecommend.RX.ToInt32() });
        }

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddImage(ProductImage entity)
        {

            var commandText = @"insert into ProductImage(ProductID,Image,IsDefault,Datastate) values(@ProductID,@Image,@IsDefault,@Datastate); select @@identity;";
            int id = DbHelp.ExecuteScalar<int>(commandText, entity);
            return id;
        }

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteImage(int id)
        {
            var commandText = @"update ProductImage set Datastate=@Datastate where ID=@ID";
            int count = DbHelp.Execute(commandText, new { Datastate = EDataStatus.Delete.ToInt32(), ID = id });
            return count > 0;
        }

        /// <summary>
        /// 设置商品默认图片
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="imgid"></param>
        /// <returns></returns>
        public bool SetDefaultImage(int pid, int imgid)
        {
            //using (TransactionScope tran = new TransactionScope())
            //{
            var commandText = @"
                                update Product set Image=(select Image from ProductImage where ID=@ImgId)
                                where ID=@ProId;
                                update ProductImage set IsDefault=0 where ProductID=@ProId;
                                update ProductImage set IsDefault=1 where ID=@ImgId;";
            int count = DbHelp.Execute(commandText, new { ProId = pid, ImgId = imgid });
            //tran.Complete();
            return count > 0;
            //}
        }

        /// <summary>
        /// 修改商品图片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateImg(ProductImage data)
        {
            if (data != null && !string.IsNullOrEmpty(data.Image))
            {
                data.Image = data.Image.Replace(ConfigurationManager.AppSettings["ImgPath"], "");
            }

            string sql = "update productimage set Image=@Image,ProductID=@ProductID,IsDefault=@IsDefault,Datastate=@Datastate where ID=@ID";
            return DbHelp.Execute(sql, data) > 0;
        }

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IEnumerable<ProductImage> GetImage(int pid)
        {
            List<ProductImage> list = new List<ProductImage>();
            var commandText = "select * from ProductImage where ProductID=@ProductID and Datastate=0";
            var tempData = DbHelp.Query<ProductImage>(commandText, new { ProductID = pid });

            if (tempData != null && tempData.Count() > 0)
            {
                var tempData2 = tempData.Select<ProductImage, ProductImage>((d) => { d.Image = ConfigurationManager.AppSettings["ImgPath"] + d.Image; return d; });
                return tempData2.ToList<ProductImage>();
            }

            return tempData;
        }

        public IEnumerable<ProductType> GetAllProductTypes()
        {
            var sql = @"SELECT 
*   
  FROM [ProductType] where [IsDeleted]=0";
            return DbHelp.Query<ProductType>(sql);
        }

        public IList<ProductType> GetProductTypesByIds(string ids)
        {
            var sql = String.Format(@"SELECT 
*   
  FROM [ProductType] where Id in ({0})", ids);
            return DbHelp.Query<ProductType>(sql).ToList();
        }

        public IList<ProductColor> GetProductColorsByIds(string ids)
        {
            var sql = String.Format(@"SELECT 
*   
  FROM [ProductColor] where Id in ({0})", ids);
            return DbHelp.Query<ProductColor>(sql).ToList();
        }

        public IEnumerable<ProductColor> GetAllProductColors()
        {
            var sql = @"SELECT *  
  FROM [ProductColor] where [IsDeleted]=0";
            return DbHelp.Query<ProductColor>(sql);
        }


        public ProductImage GetImgOne(int imgId)
        {
            string sql = "select * from ProductImage where ID=@ID and Datastate=@Datastate";
            var tempData = DbHelp.QueryOne<ProductImage>(sql, new { ID = imgId, Datastate = EDataStatus.NoDelete.ToInt32() });

            if (tempData != null)
            {
                tempData.Image = ConfigurationManager.AppSettings["ImgPath"] + tempData.Image;
            }

            return tempData;
        }

        /// <summary>
        /// 添加商品详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddDetail(ProductDetail entity)
        {
            if (entity != null && !string.IsNullOrEmpty(entity.Description))
            {
                entity.Description = entity.Description.Replace(ConfigurationManager.AppSettings["ImgPath"] + "/upload/image", "/upload/image");
            }

            var commandText = @"insert into ProductDetail(ProductID,Description) 
                                values(@ProductID,@Description); select @@identity;";
            int id = DbHelp.Execute(commandText, entity);
            return id;
        }

        /// <summary>
        /// 编辑商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditDetail(ProductDetail entity)
        {
            if (entity != null && !string.IsNullOrEmpty(entity.Description))
            {
                entity.Description = entity.Description.Replace(ConfigurationManager.AppSettings["ImgPath"] + "/upload/image", "/upload/image");
            }

            var commandText = @"update ProductDetail set Description=@Description where ProductID=@ProductID";
            int count = DbHelp.Execute(commandText, entity);
            return count > 0;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProductDetail GetDetail(int pid)
        {
            var commandText = @"select * from ProductDetail where ProductID=@ProductID";
            var tempData = DbHelp.QueryOne<ProductDetail>(commandText, new { ProductID = pid });

            if (tempData != null && !string.IsNullOrEmpty(tempData.Description))
            {
                tempData.Description = tempData.Description.Replace("/upload/image", ConfigurationManager.AppSettings["ImgPath"] + "/upload/image");
            }

            return tempData;
        }

        /// <summary>
        /// 获取商品类型
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProductCategory GetProductCategory(int pid)
        {
            var commandText = @"select a.*,b.Name as CategoryName from ProductCategory a join Category b on a.CategoryID=b.ID where a.ProductID=@ProductID";
            return DbHelp.QueryOne<ProductCategory>(commandText, new { ProductID = pid });
        }

        /// <summary>
        /// 添加商品类型关系信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddProductCategory(ProductCategory entity)
        {
            var commandText = @"insert into ProductCategory(ProductID,CategoryID,Datastate) 
                                values(@ProductID,@CategoryID,@Datastate); select @@identity;";
            int id = DbHelp.Execute(commandText, entity);
            return id;
        }

        /// <summary>
        /// 修改商品类型关系信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditProductCategory(ProductCategory entity)
        {
            var commandText = @"update ProductCategory set CategoryID=@CategoryID where ProductID=@ProductID";
            int count = DbHelp.Execute(commandText, entity);
            return count > 0;
        }

        #endregion

       
    }
}
