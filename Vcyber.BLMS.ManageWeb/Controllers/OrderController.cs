using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.ManageWeb.Helper;
using System.Text;
using System.IO;
using System.Data;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    [MvcAuthorize]
    public class OrderController : Controller
    {
        #region ==== 构造函数 ====

        public OrderController() { }

        #endregion

        #region ==== 公共方法 ====

        public ActionResult Index()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }

        public ActionResult PartialPage(string oid, string startTime, string endTime, string phone, int tradeState = -1, int index = 1, int size = 10)
        {
            phone = FilterStr.FilterSql(phone);
            OrderCondition condition = new OrderCondition(oid, phone, startTime, endTime, tradeState);

            if (FilterStr.IsFlag<OrderCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }

            int total = 0;
            var list = _AppContext.OrderApp.GetPageOrder(condition, new PageData() { Index = index, Size = size }, out total);

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var product = _AppContext.OrderApp.GetOrderProduct(item.OrderId);
                    item.OrderProduct = product != null ? product.ToList<OrderProduct>() : new List<OrderProduct>();
                }
            }

            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.Total = total;
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count;

            return PartialView(list);
        }

        public ActionResult Detail(string orderId)
        {

            Order entity = GetOrderSubInfoById(orderId);

            return View(entity);
        }

        /// <summary>
        /// 获取订单的附属信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private static Order GetOrderSubInfoById(string orderId)
        {
            Order entity = _AppContext.OrderApp.GetOrder(orderId);


            if (entity != null)
            {
                var addresss = _AppContext.OrderApp.GetOrderAddress(orderId);
                entity.OrderAddress = addresss != null ? addresss : new OrderAddress();

                int total = 0;
                var tracks = _AppContext.OrderApp.GetPageOrderTrack(orderId, new PageData() { Index = 1, Size = 20 }, out total);
                entity.OrderTrack = tracks != null ? tracks.ToList<OrderTrack>() : new List<OrderTrack>(1);

                var orderProducts = _AppContext.OrderApp.GetOrderProduct(orderId);
                entity.OrderProduct = orderProducts != null ? orderProducts.ToList<OrderProduct>() : new List<OrderProduct>(1);

                var shippings = _AppContext.OrderApp.GetOrderShipping(orderId);
                entity.Shipping = shippings != null ? shippings : new OrderShipping();

                FrontUserTable<FrontIdentityUser> userTable = new FrontUserTable<FrontIdentityUser>();
                entity.UserName = userTable.GetUserName(entity.UserID);
            }
            else
            {
                entity = new Order();
                entity.Shipping = new OrderShipping();
                entity.OrderProduct = new List<OrderProduct>();
                entity.OrderTrack = new List<OrderTrack>();
                entity.OrderAddress = new OrderAddress();
            }
            return entity;
        }

        [HttpPost]
        public ActionResult Shipping(string oid, string shipnumber, string shipname, string content)
        {
            Order entity = _AppContext.OrderApp.GetOrder(oid);

            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                if (entity.TradeState == ETradeState.DFH.ToInt32())
                {
                    _AppContext.OrderApp.AddOrderShipping(new OrderShipping() { DeliveryTime = DateTime.Now, Number = shipnumber, OrderID = oid, Name = shipname, ReceiveTime = DateTime.Now });
                    _AppContext.OrderApp.AddOrderTrack(new OrderTrack() { OperateTime = DateTime.Now, Content = "订单已发货", OrderID = oid, OperateUser = "" });
                    _AppContext.OrderApp.UpdateTradeState(oid, ETradeState.YFH);
                    //scope.Complete();


                    var orderProducts = _AppContext.OrderApp.GetOrderProduct(oid);
                    var orderAddress = _AppContext.OrderApp.GetOrderAddress(oid);
                    var orderShipping = _AppContext.OrderApp.GetOrderShipping(oid);

                    if (orderProducts != null && orderAddress != null && orderShipping != null)
                    {
                        var productNames = orderProducts.Select<OrderProduct, string>((d) => { return d.Name; });
                        string productName = string.Join(",", productNames);
                        string shippingName = orderShipping.Name;
                        string shippingNumber = orderShipping.Number;
                        _AppContext.SMSApp.SendSMS(ESmsType.后台订单发货, orderAddress.Phone, new string[] { productName, shippingName, shippingNumber });
                    }

                    return Content("ok");
                }

                return Content("on");
            }
            catch (Exception ex)
            {
                return Content("on");
            }
            //}
        }


        /// <summary>
        /// 导出订单记录到Excel
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="tradeState"></param>
        /// <returns></returns>
        public ActionResult ExportData(string oid, string phone, string startTime, string endTime, int tradeState = -1)
        {
            try
            {
                OrderCondition condition = new OrderCondition(oid, phone, startTime, endTime, tradeState);
                int total = 0;
                var data = _AppContext.OrderApp.GetAllOrderList(condition, out total);


                var orderList = data.ToList();

                //List<string> propertyName = new List<string> { "OrderId", "TradeState", "UserName", "Createtime", "Integral", "BlueBean", "Type", "Shipping.Type", "Shipping.DeliveryTime" };
                List<string> columName = new List<string> { "订单号", "订单状态", "下单人", "下单日期", "订单积分", "支付方式", "物流类型", "发货时间", "商品信息", "收货人", "收货人电话", "收货人地址", "属性", "颜色", "卡券号码" };

                string fileName = string.Format("订单信息列表{0}", DateTime.Now.ToString("yyyyMMdd")) + ".xls";

                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("订单信息列表");
                var wrap_cs = book.CreateCellStyle();
                wrap_cs.WrapText = true;

                sheet1.SetColumnWidth(0, 100 * 36);
                sheet1.SetColumnWidth(2, 100 * 36);
                sheet1.SetColumnWidth(3, 120 * 36);
                sheet1.SetColumnWidth(7, 120 * 36);
                sheet1.SetColumnWidth(8, 400 * 36);
                sheet1.SetDefaultColumnStyle(9, wrap_cs);
                sheet1.SetColumnWidth(11, 100 * 36);
                sheet1.SetColumnWidth(12, 200 * 36);
                sheet1.SetColumnWidth(13, 200 * 36);
                // sheet1.SetColumnWidth(14, 200 * 36);
                if (orderList != null)
                {

                    //给sheet1添加第一行的头部标题
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    for (int i = 0; i < columName.Count; i++)
                    {
                        row1.CreateCell(i).SetCellValue(columName[i]);
                    }


                    //将数据逐步写入sheet1各个行
                    for (int i = 0; i < orderList.Count(); i++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                        rowtemp.CreateCell(0, NPOI.SS.UserModel.CellType.String).SetCellValue(orderList[i].OrderId.ToString());
                        rowtemp.CreateCell(1).SetCellValue(EnumExtension.GetDiscribe((ETradeState)orderList[i].TradeState));
                        rowtemp.CreateCell(2).SetCellValue(orderList[i].UserName);
                        rowtemp.CreateCell(3).SetCellValue(orderList[i].Createtime.ToString());
                        rowtemp.CreateCell(4).SetCellValue(orderList[i].Integral);
                        //rowtemp.CreateCell(5).SetCellValue(orderList[i].BlueBean);

                        rowtemp.CreateCell(5).SetCellValue(EnumExtension.GetDiscribe((EPayType)orderList[i].Type));
                        rowtemp.CreateCell(6).SetCellValue(StoreHelpr.GetShippingName(orderList[i].ShippingType));
                        rowtemp.CreateCell(7).SetCellValue(orderList[i].DeliveryTime.ToString());

                        //商品信息
                        var str_products = new StringBuilder();

                        var str_Carcode = new StringBuilder();
                        int tempInt = 0;
                        int tempInt1 = 0;
                        /*
                         导出添加属性和颜色
                         */
                        string productColor = string.Empty;//颜色
                        string productType = string.Empty;//属性
                        string productCode = string.Empty;//卡号
                        foreach (var item in orderList[i].OrderProduct)
                        {
                            if (tempInt > 0)
                            {
                                str_products.Append("\r\n");
                                str_Carcode.Append("\r\n");
                            }
                            tempInt++;
                            str_products.Append(item.Name + "（数量：" + item.Qty + "）");
                            str_Carcode.Append(item.CardCode); 
                            if (!string.IsNullOrEmpty(item.ProductColor))
                            {
                                productColor = productColor + ";" + item.ProductColor;
                            }
                            else
                            {
                                productColor = item.ProductColor;
                            }

                            if (!string.IsNullOrEmpty(item.ProductColor))
                            {
                                productType = productType + ";" + item.ProductType;
                            }
                            else
                            {
                                productType = item.ProductType;
                            }
                        }
                        rowtemp.CreateCell(8).SetCellValue(str_products.ToString());

                        //收货信息
                        rowtemp.CreateCell(9).SetCellValue(orderList[i].ReceiveName);
                        rowtemp.CreateCell(10).SetCellValue(orderList[i].ReceivePhone);

                        rowtemp.CreateCell(11).SetCellValue(orderList[i].PCC + orderList[i].Detail);
                        rowtemp.CreateCell(11).SetCellValue(orderList[i].PCC + orderList[i].Detail);
                        rowtemp.CreateCell(11).SetCellValue(orderList[i].PCC + orderList[i].Detail);
                        //属性和颜色
                        rowtemp.CreateCell(12).SetCellValue(string.IsNullOrEmpty(productType) ? productType : productType.TrimStart(';'));
                        rowtemp.CreateCell(13).SetCellValue(string.IsNullOrEmpty(productColor) ? productColor : productColor.TrimStart(';'));

                        //卡券号码
                        rowtemp.CreateCell(14).SetCellValue(str_Carcode.ToString());
                    }
                }

                else
                {
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    row1.CreateCell(0).SetCellValue("导出数据出错");

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return File(ms, "application/ms-excel", fileName);


            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量导入物流单号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportShipping()
        {
            HttpPostedFileBase file = Request.Files["file"];

            if (file != null)
            {
                string filePath = Path.Combine(HttpContext.Server.MapPath("../UploadImg"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));

                if (System.IO.Path.GetExtension(filePath) != ".xls" && System.IO.Path.GetExtension(filePath) != ".xlsx")
                {
                    return RedirectToAction("Index", "Order");
                }

                file.SaveAs(filePath);

                DataTable dt = Common.NPOIHelper<OrderShipping>.ReadExcel(filePath, System.IO.Path.GetExtension(filePath));

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        //string oid = dr["OrderNo"].ToString();
                        //string shipnumber = dr["ShippingNo"].ToString();
                        //string shiptype = dr["ShippingType"].ToString();
                        //string content = dr["Remark"].ToString();

                        string oid = dr["订单编号"].ToString();
                        string shipnumber = dr["快递单号"].ToString();
                        string shiptype = dr["快递公司"].ToString();
                        string content = dr["备注"].ToString();

                        Order entity = _AppContext.OrderApp.GetOrder(oid);

                        if (entity != null && entity.TradeState == ETradeState.DFH.ToInt32())
                        {
                            _AppContext.OrderApp.AddOrderShipping(new OrderShipping() { DeliveryTime = DateTime.Now, Number = shipnumber, OrderID = oid, Name = shiptype.ToString(), ReceiveTime = DateTime.Now });
                            _AppContext.OrderApp.AddOrderTrack(new OrderTrack() { OperateTime = DateTime.Now, Content = string.IsNullOrEmpty(content) ? "订单已发货" : content, OrderID = oid, OperateUser = "" });
                            _AppContext.OrderApp.UpdateTradeState(oid, ETradeState.YFH);

                            var orderProducts = _AppContext.OrderApp.GetOrderProduct(oid);
                            var orderAddress = _AppContext.OrderApp.GetOrderAddress(oid);
                            var orderShipping = _AppContext.OrderApp.GetOrderShipping(oid);

                            if (orderProducts != null && orderAddress != null && orderShipping != null)
                            {
                                var productNames = orderProducts.Select<OrderProduct, string>((d) => { return d.Name; });
                                string productName = string.Join(",", productNames);
                                string shippingName = orderShipping.Name;
                                string shippingNumber = orderShipping.Number;
                                _AppContext.SMSApp.SendSMS(ESmsType.后台订单发货, orderAddress.Phone, new string[] { productName, shippingName, shippingNumber });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Order");
                    }
                }


            }

            return RedirectToAction("Index", "Order");
        }

        /// <summary>
        /// 取消物流订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelShipping(string orderId, string userId, int integral)
        {

          
            Order entity = new Order() { OrderId = orderId, UserID = userId, Integral = integral };
            if (FilterStr.IsFlag<Order>(entity))
            {
                return Redirect("/Content/error.htm");
            }

            var orderProducts = _AppContext.OrderApp.GetOrderProduct(orderId);

            bool isSuccess = _AppContext.OrderApp.BMCancelOrder(entity, orderProducts);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
           
        }
        #endregion
    }
}