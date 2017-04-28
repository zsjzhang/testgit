using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;


namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public class OrderStorager : IOrderStorager
    {
        #region ==== 构造函数 ====

        public OrderStorager() { }

        #endregion

        #region ==== 前台逻辑 ====

        #region ==== Order ====

        /// <summary>
        /// 获取用户订单
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetUserOrderList(string userID, PageData pageData, out int total)
        {
            // this.ClearOrder(userID);

            StringBuilder sql = new StringBuilder();
            sql.Append("select COUNT(1) from orders where orders.Datastate=@Datastate and orders.UserID=@UserID");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), UserID = userID });

            sql.Clear();



            sql.AppendFormat(" select top {0} * from orders", pageData.Size);
            sql.Append(" where orders.Datastate=@Datastate and orders.UserID=@UserID and orders.ID not in(");
            sql.AppendFormat(" select top {0} orders.ID from orders where orders.Datastate=@Datastate and orders.UserID=@UserID  order by orders.PayState desc,orders.CreateTime", pageData.Index);
            sql.Append(" )");
            sql.Append(" order by orders.id desc");


            //sql.Append(" order by orders.PayState desc,orders.CreateTime");
            return DbHelp.Query<Order>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), UserID = userID });
        }

        public IEnumerable<Order> GetUserOrderPageList(int orderPageType, string userID, int pageIndex, int pageSize, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select COUNT(1) from orders  o where o.Datastate=@Datastate and o.UserID=@UserID");
            if (orderPageType == 2)
            {
                sql.Append(" and  o.PayState=11 and  o.TradeState!=5     ");
            }

            if (orderPageType == 1)
            {
                sql.Append(" and o.PayState =2 and  o.TradeState =6  ");
            }
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), UserID = userID });
            sql.Clear();

            sql.AppendFormat(@" select   * from    (  select o .* ,  ROW_NUMBER ()  over ( order by o.CreateTime  desc )   as  rows  
from  orders o  with(nolock)    where o.Datastate=@Datastate and o.UserID=@UserID  ");

            if (orderPageType == 2)
            {
                sql.Append("  and o.PayState=11 and  o.TradeState!=5  ");
            }

            if (orderPageType == 1)
            {
                sql.Append(" and o.PayState=2 and   o.TradeState=6 ");
            }

            sql.AppendFormat(@")  page
             
            where    page.rows  >= {0}  and  page .rows <=  {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            return DbHelp.Query<Order>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), UserID = userID });
        }

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="instance">订单信息</param>
        /// <returns></returns>
        public void AddOrder(Order instance)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("insert into orders(OrderID,UserID,Type,Mode,Total,Integral,BlueBean,PayState,TradeState,Datastate,Createtime,Updatetime,DataSource) ");
            sql.Append(" values(@OrderID,@UserID,@Type,@Mode,@Total,@Integral,@BlueBean,@PayState,@TradeState,@Datastate,@Createtime,@Updatetime,@DataSource)");
            DbHelp.Execute(sql.ToString(), instance);
        }

        /// <summary>
        /// 获取用户订单信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Order> SelectList(string userId, PageData pageData, out int totalCount)
        {
            // this.ClearOrder(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append("select COUNT(1) from orders where orders.UserID=@UserID");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserID = userId });

            sql.Clear();

            sql.AppendFormat(" select top {0} * from orders where orders.UserID=@UserID and orders.ID not in(", pageData.Size);
            sql.AppendFormat(" select top {0} orders.ID from orders where orders.UserID=@UserID ", pageData.Index);
            sql.AppendFormat(" order by orders.PayState desc,orders.Createtime desc)");
            sql.AppendFormat(" order by orders.PayState desc,orders.Createtime desc");
            return DbHelp.Query<Order>(sql.ToString(), new { UserID = userId });
        }

        #endregion

        #region ==== OrderProduct =====

        /// <summary>
        /// 添加订单商品
        /// </summary>
        /// <param name="instance">商品信息</param>
        /// <returns></returns>
        public void AddOrderProduct(OrderProduct instance)
        {
            var commandText = "insert into orderproduct(OrderID,ProductID,Qty,Price,Integral,BlueBean,ProductColor,ProductType) values(@OrderID,@ProductID,@Qty,@Price,@Integral,@BlueBean,@ProductColor,@ProductType)";
            DbHelp.Execute(commandText, instance);
        }

        /// <summary>
        /// 获取用户 对某个商品的购买次数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="productID">商品ID</param>
        /// <returns></returns>
        public int GetUserOderCount(int userID, int productID)
        {
            var commandText = "SELECT COUNT(*) from orders a INNER JOIN orderproduct b on a.OrderID=b.OrderID WHERE UserID=@UserID and ProductID=@ProductID AND a.TradeState!=@TradeState";
            int count = DbHelp.ExecuteScalar<int>(commandText, new { UserID = userID, ProductID = productID, TradeState = ETradeState.JYQX.ToInt32() });
            return count;
        }

        /// <summary>
        /// 获取用户最新积分订单 数据更新次数
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RecentlyOrder(DateTime date, int userId)
        {
            var commandText = " select count(*)  from orders where UserID=@UserID and Updatetime> @Updatetime";
            return DbHelp.ExecuteScalar<int>(commandText, new { UserID = userId, Updatetime = date });
        }

        /// <summary>
        /// 获取购买商品信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<OrderProduct> SelectUserProduct(string userId, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select COUNT(1) from orders join orderproduct ");
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate and  orders.Paystate=2");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserID = userId, Datastate = EDataStatus.NoDelete.ToInt32() });
             
            sql.Clear();

            sql.AppendFormat(" select top {0} orderproduct.*,product.Name,orders.Createtime from orders join orderproduct", pageData.Size);
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate  and  orders.Paystate=2 and orderproduct.ID not in(");
            sql.AppendFormat(" select top {0} orderproduct.ID from orders join orderproduct ", pageData.Index);
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate  and  orders.Paystate=2 order by orders.Createtime desc");
            sql.Append(" ) order by orders.Createtime desc");
            return DbHelp.Query<OrderProduct>(sql.ToString(), new { UserID = userId, Datastate = EDataStatus.NoDelete.ToInt32() });
        }

        #endregion

        #region ==== OrderAddress ====

        /// <summary>
        /// 添加订单地址
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public void AddOrderAddress(OrderAddress instance)
        {
            var commandText = "insert into orderaddress(OrderID ,ReceiveName ,Phone ,Province ,City ,County ,PCC ,ZipCode,Detail) values(@OrderID ,@ReceiveName ,@Phone ,@Province ,@City ,@County ,@PCC ,@ZipCode,@Detail)";
            DbHelp.Execute(commandText, instance);
        }

        /// <summary>
        /// 修改订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool UpdateOrderAddress(string orderID, OrderAddress instance)
        {
            instance.OrderID = orderID;
            var commandText = "update orderaddress set ReceiveName=@ReceiveName,Phone=@Phone,Province=@Province,City=@City,County=@p4,PCC=@County,ZipCode=@ZipCode,Detail=@Detail where OrderID=@OrderID";
            return DbHelp.Execute(commandText, instance) > 0;
        }

        /// <summary>
        /// 获取订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderAddress GetOrderAddress(string orderID)
        {
            var commandText = "select * from orderaddress where OrderID=@OrderID";
            return DbHelp.QueryOne<OrderAddress>(commandText, new { OrderID = orderID });
        }

        #endregion

        #region ==== OrderShipping ====

        /// <summary>
        /// 设置收货时间
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public bool SetReceiveTime(string orderId)
        {
            string sql = "update ordershipping set ReceiveTime=@Time where OrderID=@OrderID";
            return DbHelp.Execute(sql, new { Time = DateTime.Now, OrderID = orderId }) > 0;
        }


        public IEnumerable<OrderProduct> GetOrdersAndShipping(string userId, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"  select  count(*)   from   orders  o  with(nolock)  
 left join  orderproduct  op  with(nolock)    on  op.OrderID =o.OrderID 
 left join ordershipping  osp   with(nolock)   on  osp .OrderID =o.OrderID
 left join product  p with(nolock)  on p.ID =op .ProductID ");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();
            sql.Append(@" select * from (  select op.*,p.Name,o.Createtime  , osp .Name as ShippingName , osp .Number  ,   ROW_NUMBER ()  over ( order by o.CreateTime  desc )  as rows  from   orders  o  with(nolock)  
 left join  orderproduct  op  with(nolock)    on  op.OrderID =o.OrderID 
 left join ordershipping  osp   with(nolock)   on  osp .OrderID =o.OrderID
 left join product  p with(nolock)  on p.ID =op .ProductID   where   o.UserID=@UserID  and o.Datastate=@Datastate) page 
 where  page.rows  >= (@pagesize  -1 )  * @pageindex +1  and  page .rows <= @pageindex *@pagesize
          
");
            return DbHelp.Query<OrderProduct>(sql.ToString(), new { UserID = userId, pageindex = pageData.Index, pagesize = pageData.Size, Datastate = EDataStatus.NoDelete.ToInt32() });
        }


        #endregion

        #endregion

        #region ==== 后台逻辑 ====

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(string orderId)
        {
            var commandText = "select * from Orders where OrderID=@OrderID";
            return DbHelp.QueryOne<Order>(commandText, new { OrderID = orderId });
        }

        /// <summary>
        /// 修改订单支付状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool EditPayState(string orderId, EPayState state)
        {
            var commandText = @"update Orders set Orders.PayState=@PayState,Updatetime=@Updatetime where OrderID=@OrderID";
            int count = DbHelp.Execute(commandText, new { PayState = state.ToInt32(), Updatetime = DateTime.Now, OrderID = orderId });
            return count > 0;
        }

        /// <summary>
        /// 修改订单交易状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="traderState"></param>
        /// <returns></returns>
        public bool EditTradeState(string orderId, ETradeState traderState)
        {
            string sql = "update Orders set Orders.TradeState=@TradeState,Updatetime=@Updatetime where OrderID=@OrderID";
            return DbHelp.Execute(sql, new { TradeState = traderState.ToInt32(), Updatetime = DateTime.Now, OrderID = orderId }) > 0;
        }

        /// <summary>
        /// 获取订单商品信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IEnumerable<OrderProduct> GetOrderProduct(string orderId)
        {
            List<OrderProduct> list = new List<OrderProduct>();

            StringBuilder sql = new StringBuilder();
            sql.Append(" select orderproduct.*,product.Name,product.Image as Img from orderproduct join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orderproduct.OrderID=@OrderID");

            return DbHelp.Query<OrderProduct>(sql.ToString(), new { OrderID = orderId });
        }

        /// <summary>
        /// 分页获取订单信息
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetPageOrder(OrderCondition conditon, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select COUNT(1) from orders where orders.Datastate=@Datastate and orders.Mode=@Mode and {0}", conditon.ToWhere());
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), Mode = EOrderMode.YJSW.ToInt32() });

            sql.Clear();

            sql.AppendFormat(" select top {0} * from orders", pageData.Size);
            sql.AppendFormat(" where orders.Datastate=@Datastate and orders.Mode=@Mode and {0} and orders.ID not in(", conditon.ToWhere());
            sql.AppendFormat(" select top {0} orders.ID from orders where orders.Datastate=@Datastate and orders.Mode=@Mode and {1} order by orders.CreateTime desc", pageData.Index, conditon.ToWhere());
            sql.Append(" )");
            sql.Append(" order by orders.CreateTime desc");
            return DbHelp.Query<Order>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), Mode = EOrderMode.YJSW.ToInt32() });
        }

        /// <summary>
        /// 订单邮寄信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public OrderShipping GetOrderShipping(string oid)
        {
            var commandText = @"select a.* from OrderShipping a where a.OrderID=@OrderID";
            return DbHelp.QueryOne<OrderShipping>(commandText, new { OrderID = oid });

        }

        /// <summary>
        /// 添加订单邮寄信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddOrderShipping(OrderShipping entity)
        {
            var commandText = @"insert into OrderShipping(OrderID,Type,Number,DeliveryTime,ReceiveTime,Name) 
                                values(@OrderID,@Type,@Number,@DeliveryTime,@ReceiveTime,@Name); select @@identity;";
            int id = DbHelp.ExecuteScalar<int>(commandText, entity);
            return id;
        }

        /// <summary>
        /// 修改订单邮寄信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditOrderShipping(OrderShipping entity)
        {
            var commandText = @"update OrderShipping set Type=@Type,Number=@Number,DeliveryTime=@DeliveryTime where OrderID=@OrderID";
            int count = DbHelp.Execute(commandText, entity);
            return count > 0;
        }

        /// <summary>
        /// 添加订单轨迹
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回轨迹Id</returns>
        public int AddOrderTrack(OrderTrack entity)
        {
            var commandText = @"insert into OrderTrack(OrderID,TypeID,Content,OperateUser,OperateTime) 
                                values(@OrderID,@TypeID,@Content,@OperateUser,@OperateTime); select @@identity;";
            int id = DbHelp.Execute(commandText, entity);
            return id;
        }

        /// <summary>
        /// 分页获取订单轨迹信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<OrderTrack> GetPageOrderTrack(string orderID, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select COUNT(1) from ordertrack where ordertrack.OrderID=@OrderID");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { OrderID = orderID });

            sql.Clear();

            sql.AppendFormat("  select top {0} * from ordertrack where ordertrack.OrderID=@OrderID and ", pageData.Size);
            sql.AppendFormat("  ordertrack.ID not in(select top {0} ordertrack.Id from ordertrack ", pageData.Index);
            sql.Append("  where ordertrack.OrderID=@OrderID order by ordertrack.OperateTime desc)");
            sql.Append("  order by ordertrack.OperateTime desc");
            return DbHelp.Query<OrderTrack>(sql.ToString(), new { OrderID = orderID });
        }


        /// <summary>
        /// 获取物流公司信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Shipping GetShipping(int shippingType)
        {
            var commandText = @"select * from Shipping where Value=@Value";
            return DbHelp.QueryOne<Shipping>(commandText, new { Value = shippingType });
        }

        /// <summary>
        /// 获取全部物流公司信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shipping> GetAllShipping()
        {
            List<Shipping> list = new List<Shipping>();
            var commandText = "select * from Shipping";
            return DbHelp.Query<Shipping>(commandText);
        }


        /// <summary>
        /// 取消订单操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="integral"></param>
        /// <returns></returns>
        public bool CancelOrder(Order entity)
        {
            StringBuilder sql = new StringBuilder();
            //修改订单状态
            sql.AppendFormat("UPDATE ORDERS SET TRADESTATE = 5,PayState=11 WHERE ORDERID = @orderId");
            DbHelp.Execute(sql.ToString(), new { @orderId = entity.OrderId });
            sql.Clear();
            //返还积分
            sql.AppendFormat("update userintegral set usevalue=usevalue-{0} where ID=(select top(1)ID from userintegral where userid = @userid)", FilterStr.FilterSql(entity.Integral.ToString()));
            return DbHelp.Execute(sql.ToString(), new { @userid = entity.UserID }) > 0;
        }

        /// <summary>
        /// 取消订单操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="integral"></param>
        /// <returns></returns>
        public bool BMCancelOrder(Order entity,IEnumerable<OrderProduct> op)
        {
            StringBuilder sql = new StringBuilder();
            //增加库存

            if (op!=null && op.Count()>0)
            {
                foreach (OrderProduct item in op)
                {
                    sql.AppendFormat("UPDATE dbo.product SET Qty=Qty+{0} WHERE id={1};", item.Qty, item.ProductID);
                }
               
            }

            //修改订单状态
            sql.AppendFormat("UPDATE ORDERS SET TRADESTATE = 5,PayState=11 WHERE ORDERID = '{0}'", FilterStr.FilterSql(entity.OrderId));
            DbHelp.Execute(sql.ToString());
            sql.Clear();
            //返还积分
            sql.AppendFormat("update userintegral set usevalue=usevalue-{0} where ID=(select top(1)ID from userintegral where userid = '{1}')", FilterStr.FilterSql(entity.Integral.ToString()), FilterStr.FilterSql(entity.UserID));
            return DbHelp.Execute(sql.ToString()) > 0;
        }

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 清理用户订单
        /// </summary>
        /// <param name="userId"></param>
        private void ClearOrder(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  update orders set TradeState=@TradeState where UserID=@UserID and PayState=@PayState and ");
            sql.Append("  datediff(hh,Createtime,@Time)>24");
            DbHelp.Execute(sql.ToString(), new { TradeState = ETradeState.JYQX.ToInt32(), UserID = userId, PayState = EPayState.ZFZ.ToInt32(), Time = DateTime.Now });

            sql.Clear();

            sql.Append("  update product set Qty=isnull(Qty,0)+(");
            sql.Append("  select orderproduct.Qty from orders join orderproduct on orders.OrderID=orderproduct.OrderID");
            sql.Append("  where orderproduct.ProductID=product.ID),");
            sql.Append("  LQty=isnull(LQty,0)-(select orderproduct.Qty from orders join orderproduct on orders.OrderID=orderproduct.OrderID");
            sql.Append("  where orderproduct.ProductID=product.ID),");
            sql.Append("  Sales=isnull(Sales,0)-(");
            sql.Append("  select orderproduct.Qty from orders join orderproduct on orders.OrderID=orderproduct.OrderID");
            sql.Append("  where orderproduct.ProductID=product.ID");
            sql.Append("  ) where product.ID in(");
            sql.Append("  select orderproduct.ProductID from orders join orderproduct on orders.OrderID=orderproduct.OrderID");
            sql.Append("  where  orders.UserID=@UserID and orders.PayState=@PayState and ");
            sql.Append("  datediff(hh,orders.Createtime,@Time)>24)");
            DbHelp.Execute(sql.ToString(), new { UserID = userId, PayState = EPayState.ZFZ.ToInt32(), Time = DateTime.Now });

        }

        #endregion

        /// <summary>
        /// 订单导出
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetAllOrderList(OrderCondition condition, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select COUNT(1) from orders where orders.Datastate=@Datastate and orders.Mode=@Mode and {0}", condition.ToWhere());
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), Mode = EOrderMode.YJSW.ToInt32() });

            sql.Clear();

            //sql.Append(" select * from orders ");
            //sql.AppendFormat(" where orders.Datastate=@Datastate and orders.Mode=@Mode and {0} ", condition.ToWhere());
            //sql.Append(" order by orders.CreateTime desc");
            sql.Append(" select t.Type as ShippingType,t.DeliveryTime,OrderAddress.ReceiveName,OrderAddress.Phone as ReceivePhone, OrderAddress.PCC ,OrderAddress.Detail,m.UserName,orders.ID,orders.OrderId,orders.UserID,orders.Type,orders.Mode,orders.Total,orders.Integral,orders.BlueBean,orders.PayState,orders.TradeState,orders.Createtime,orders.Updatetime,orders.Datastate,orders.DataSource from orders ");
            sql.Append(" left join  membership m on orders.UserID=m.Id ");
            sql.Append(" left join  orderaddress OrderAddress on OrderAddress.OrderID=orders.OrderID ");
            sql.Append(" left join  OrderShipping t on orders.OrderID=t.OrderID ");
            //sql.Append(" left join  orderproduct p  on orders.OrderID=p.OrderID ");
            sql.AppendFormat(" where orders.Datastate=@Datastate and orders.Mode=@Mode and {0} ", condition.ToWhere());
            sql.Append(" order by orders.CreateTime desc");
            IEnumerable<Order> orderEList = DbHelp.Query<Order>(sql.ToString(), new { Datastate = EDataStatus.NoDelete.ToInt32(), Mode = EOrderMode.YJSW.ToInt32() });

            //获取订单商品信息
            //string sp_sql = " select orderproduct.*,product.Name,product.Image as Img from orderproduct join product on orderproduct.ProductID=product.ID";
            StringBuilder sp_sql = new StringBuilder();
            sp_sql.Append("select op.ID, op.OrderID,op.ProductID,op.Qty,op.Price,op.Integral,op.BlueBean,op.CardCode,");
            sp_sql.Append("case op.ProductColor when '1' then '红' when '2' then '黑'when '3' then '银' when '4' then '红'  when '5' then '黑'  when '6' then '银' else op.ProductColor end as ProductColor,");
            sp_sql.Append("case op.ProductType when '1' then '领动' when '2' then '索九' when '3' then '悦动' when '4' then '朗动' when '5' then '名图'  when '6' then 'IX25' when '7' then '索八' when '8' then '全新途胜' when '9' then 'IX35' when '10' then '途胜' when '11' then '伊兰特' when '12' then '全新胜达' when '13' then '通用' when '14' then '北京' when '15' then '河南' when '16' then '广东' when '17' then '浙江' else op.ProductType  end as ProductType,");
            sp_sql.Append(" product.Name,product.Image as Img from orderproduct op join product on op.ProductID=product.ID");

            List<OrderProduct> list = DbHelp.Query<OrderProduct>(sp_sql.ToString()).ToList();
            if (list != null)
            {
                foreach (var item in orderEList)
                {
                    item.OrderProduct = list.Where(s => s.OrderID == item.OrderId).ToList();
                }
            }
            return orderEList;
        }



        public bool EditOrderCardCode(string orderId, string cardCode,int productId)
        {
            string sql = "  update orderproduct  set   CardCode =@CardCode  where OrderID=@OrderID and ProductID=@ProductID ;";
            return DbHelp.Execute(sql, new { CardCode = cardCode, OrderID = orderId, ProductID =productId}) > 0;
        }





        public string GetOrderProductCategoryCardCode(int productId)
        {
            string sql = @"  select c.CardType from  product as p  left  join   productcategory  as pc  on p.ID = pc.ProductID
                        inner join category as c  on pc.CategoryID = c.ID and c.CardType is  not null  
                        where p.ID=@ID";
            return DbHelp.ExecuteScalar<string>(sql, new { ID = productId });

        }


        public string GetOrderProductCardCode(int productId, string orderId)
        {
            string sql = @"  select CardCode  from orderproduct  where  OrderID=@OrderID and ProductID=@ProductID ";
            return DbHelp.ExecuteScalar<string>(sql, new {OrderID = orderId, ProductID = productId});
        }



        /// <summary>
        /// 兑换记录分页列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<OrderProduct> GetUserOrderProductList(string userId, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select COUNT(1) from orders join orderproduct ");
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate and  orders.Paystate=2");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserID = userId, Datastate = EDataStatus.NoDelete.ToInt32() });

            sql.Clear();

            sql.AppendFormat(" select top {0} orderproduct.*,product.Name,product.Image as Img,orders.TradeState,orders.Createtime from orders join orderproduct", pageData.Size);
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate  and  orders.Paystate=2 and orderproduct.ID not in(");
            sql.AppendFormat(" select top {0} orderproduct.ID from orders join orderproduct ", pageData.Index);
            sql.Append(" on orders.OrderID=orderproduct.OrderID join product on orderproduct.ProductID=product.ID");
            sql.Append(" where orders.UserID=@UserID and orders.Datastate=@Datastate  and  orders.Paystate=2 order by orders.Createtime desc");
            sql.Append(" ) order by orders.Createtime desc");
            return DbHelp.Query<OrderProduct>(sql.ToString(), new { UserID = userId, Datastate = EDataStatus.NoDelete.ToInt32() });
        }
    }
}
