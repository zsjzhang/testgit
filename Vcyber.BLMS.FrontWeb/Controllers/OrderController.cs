using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Script.Serialization;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Domain.BLMSMoney;
using Vcyber.BLMS.Entity.Enum;
using Webdiyer.WebControls;
using Webdiyer.WebControls.Mvc;
namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class OrderController : Controller
    {
        private int PAGESIZE = 10;
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        #endregion

        #region ==== 公共属性 ====

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        #region ==== 构造函数 ====

        public OrderController()
        {
        }

        public OrderController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion


        /// <summary>
        /// 前台--订单确认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                //判断用户是否登录，登录成功查看订单，否则跳转到登录页
                string userId = string.Empty;
                if (this.User.Identity.IsAuthenticated)
                {
                    userId = this.User.Identity.GetUserId();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("LogonPage", "Account", new { returnUrl = "/Order/Index" });
                }
                ViewBag.userId = userId;


                //将购物车的数据添加到数据库
                //第一：从客户端获取购物车数据
                HttpCookie _myCart = HttpContext.Request.Cookies.Get("mycart");

                //第二：将购物车数据转换成购物车对象
                if (_myCart == null || string.IsNullOrEmpty(_myCart.Value))
                {
                    //购物车没数据
                    return RedirectToAction("Default", "Home");
                }
                string _myCartJson = HttpContext.Server.UrlDecode(_myCart.Value);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ShoppingCart _result = jss.Deserialize<ShoppingCart>(_myCartJson);
                if (_result == null || _result.productList == null || !_result.productList.Any())
                {
                    //购物车没数据
                    return RedirectToAction("Default", "Home");
                }
                //第一步：将购物车之前的用户数据删除（逻辑）
                _AppContext.ShoppingApp.Clear(userId);
                //第二步：后台将购物车数据添加到数据库中
                var isCard = 0;//是否包含了实物商品，如果没有包含就不需要进行收货地址验证  1包含  0不包含
                foreach (var item in _result.productList)
                {
                    Product _orderProduct = _AppContext.ProductApp.GetProductById(int.Parse(item.productId));
                    var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(int.Parse(item.productId));
                    if (string.IsNullOrEmpty(cardType))
                    {
                        isCard = 1;
                    }
                    Shopping _myCartEntity = new Shopping();
                    _myCartEntity.UserID = userId;
                    _myCartEntity.ProductID = int.Parse(item.productId);
                    _myCartEntity.Qty = item.quantity;
                    _myCartEntity.CreateTime = DateTime.Now;
                    _myCartEntity.UpdateTime = DateTime.Now;
                    _myCartEntity.Name = item.productName;
                    _myCartEntity.ProductColor = item.productcolor;
                    _myCartEntity.ProductType = item.producttype;
                    // if ("blueBean".Trim().ToLower() == item.payType.Trim().ToLower())
                    //{
                    //  _myCartEntity.BlueBean = _orderProduct.BlueBean;
                    // }
                    //   else if ("Integral".Trim().ToLower() == item.payType.Trim().ToLower())
                    // {
                    _myCartEntity.Integral = (int)item.price;
                    //  }
                    _AppContext.ShoppingApp.Add(_myCartEntity);
                }

                int _total = 1;
                IEnumerable<Shopping> _shoppingCartItem = _AppContext.ShoppingApp.GetList(userId, new PageData() { Index = 0, Size = int.MaxValue }, out _total);
                ViewBag.isCard = isCard;
                return View(_shoppingCartItem);
            }
            catch (Exception ex)
            {
                //数据异常
                return RedirectToAction("Default", "Home");
            }

        }

        /// <summary>
        /// 确认订单项
        /// </summary>
        /// <returns></returns>
        public ActionResult ComfirmOrder(IEnumerable<Shopping> productList)
        {
            return View(productList);
        }

        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string orderId)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/MyCenter/Index" });
            }
            Order _orderEntity = _AppContext.OrderApp.GetOrder(orderId);
            ViewData["orderShipping"] = _AppContext.OrderApp.GetOrderShipping(orderId);
            ViewData["orderAddress"] = _AppContext.OrderApp.GetOrderAddress(orderId);
            return View(_orderEntity);
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PayOrder(string orderId)
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return Json(new { code = "401", msg = "帐号登陆异常" });
                }
                bool _paySuccess = _AppContext.TradePort.OrderPay(new OrderPayInfo() { userId = this.User.Identity.GetUserId(), orderId = orderId });
                if (!_paySuccess)
                {
                    return Json(new { code = "201", msg = "你的积分不足,付款失败" });
                }

                //处理电子卡券
                var orderProductList = _AppContext.OrderApp.GetOrderProduct(orderId);
                string userId = this.User.Identity.GetUserId();
                var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
                foreach (var p in orderProductList)
                {
                    var cardCodes = "";
                    CustomCardInfo customCardInfo = new CustomCardInfo();
                    var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(p.ProductID);
                    if (!string.IsNullOrEmpty(cardType))
                    {
                        for (int i = 0; i < p.Qty; i++)
                        {
                            var res = AddUserCustomCard(cardType);
                            if (res.IsSuccess)
                            {
                                customCardInfo = res.Data as CustomCardInfo;
                                cardCodes += res.Message;
                                if (p.Qty - i > 1)
                                {
                                    cardCodes += "，";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(cardCodes))
                        {
                            bool isCode = _AppContext.OrderApp.EditOrderCardCode(p.OrderID, cardCodes, p.ProductID);
                            if (isCode)
                            {
                                if (!string.IsNullOrEmpty(customCardInfo.SmsContent))
                                {
                                    //发送卡券短信信息；
                                    _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = cardCodes },
                                        member.UserName);
                                }
                                else
                                {
                                    if (p.Name == "电子观影券")
                                    {
                                        _AppContext.SMSApp.SendSMS(ESmsType.电子影票, member.UserName,
                                            new string[] { cardCodes }, false);
                                    }
                                    else
                                    {
                                        _AppContext.SMSApp.SendSMS(ESmsType.电子卡券, member.UserName,
                                            new string[] { customCardInfo.ActivityType, cardCodes }, false);
                                    }
                                }
                            }
                        }
                    }
                }
                var orderList = _AppContext.OrderApp.GetOrderProduct(orderId);
                var isCard = 0;//是否包含了实物商品，如果没有包含就不需要进行收货地址验证  1包含  0不包含
                foreach (var item in orderList)
                {

                    var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(item.ProductID);
                    if (string.IsNullOrEmpty(cardType))
                    {
                        isCard = 1;
                    }
                }
                if (isCard == 1)
                {
                    return Json(new { code = "200", msg = "支付成功" });
                }
                else
                {
                    return Json(new { code = "200", msg = "您已成功兑换【" + orderList.First().Name + "】，电子券将以短信形式下发到您的手机上，请注意查收！" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = "500", msg = ex.Message });
            }

        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelOrder(string orderId)
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return Json(new { code = "401", msg = "帐号登陆异常" });
                }
                bool _isCancel = _AppContext.OrderApp.Cancel(orderId);
                if (!_isCancel)
                {
                    return Json(new { code = "201", msg = "取消失败" });
                }
                return Json(new { code = "200", msg = "取消成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = "500", msg = ex.Message });
            }
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConfirmOrder(string orderId)
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return Json(new { code = "401", msg = "帐号登陆异常" });
                }
                bool _isConfirm = _AppContext.OrderApp.ConfirmOrder(orderId);
                if (!_isConfirm)
                {
                    return Json(new { code = "201", msg = "确认失败" });
                }
                return Json(new { code = "200", msg = "确认成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = "500", msg = ex.Message });
            }
        }

        /// <summary>
        /// 我的订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyOrders(int pageOrderType = 3, int pageIndex = 1)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/MyCenter/Index" });
            }
            int _totalCount = 0;
            OrderCondition _orderConditionEntity = new OrderCondition();
            IEnumerable<Order> _result = _AppContext.OrderApp.GetUserOrderPageList(pageOrderType, this.User.Identity.GetUserId(), pageIndex, PAGESIZE, out _totalCount);
            PagedList<Order> _pageresult = new PagedList<Order>(_result, pageIndex, PAGESIZE, _totalCount);
            var model = new CompositeOrderList();
            model.AlllistCount = _result.Count();
            model.AllList = _pageresult;
            //model.WaitList = _waitpageresult;
            //model.WaitReceiveList = _waitReceivepageresult;

            //IEnumerable<Order> _waitOrder = _result.Where(c => c.PayState == (int)EPayState.ZFZ && c.TradeState != (int)ETradeState.JYQX).ToList();
            //PagedList<Order> _waitpageresult = new PagedList<Order>(_waitOrder, pageIndex, PAGESIZE);
            //IEnumerable<Order> _waitReceiveOrder = _result.Where(c => c.PayState == (int)EPayState.ZFWC && c.TradeState == (int)ETradeState.YFH).ToList();
            //PagedList<Order> _waitReceivepageresult = new PagedList<Order>(_waitReceiveOrder, pageIndex, PAGESIZE);

            return View(model);
        }


        /// <summary>
        /// 个人中心我的订单--兑换的礼品
        /// </summary>
        /// <returns></returns>
        public ActionResult MyLastOrder(int? pageIndex = 1)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/MyCenter/Index" });
            }
            int _totalCount = 0;
            OrderCondition _orderConditionEntity = new OrderCondition();

            PageData _pageDataEntity = new PageData();
            _pageDataEntity.Size = 3;
            _pageDataEntity.Index = 0;
            IEnumerable<OrderProduct> _result = _AppContext.OrderApp.GetUserProduct(this.User.Identity.GetUserId(), _pageDataEntity, out _totalCount);
            return View(_result);
        }

        [HttpPost]
        public ActionResult AddOrder(int addressId, string shopids)
        {
            try
            {
                //校验用户是否登录
                if (!this.User.Identity.IsAuthenticated)
                {
                    return Json(new { code = "402", msg = "帐号登录异常" });
                }
                //校验参数
                if (addressId < 0 || string.IsNullOrEmpty(shopids))
                {
                    return Json(new { code = "401", msg = "参数错误" });
                }
                List<int> _shopids = shopids.Split(',').Select<string, int>((d) => { int shopid = 0; int.TryParse(d, out shopid); return shopid; }).ToList();

                string _userid = this.User.Identity.GetUserId();
                string _message = string.Empty;
                bool _paySuccess = false;
                //提交订单
                Order _order = _AppContext.TradePort.CreateOrder(new OrderCreateInfo() { addressId = addressId, shoppingIds = _shopids, userId = _userid, DataSource = "blms" }, out _message);
                if (_order == null)
                {
                    string _returnMessage = "订单创建失败";
                    switch (_message)
                    {
                        case "03":
                            _returnMessage = "商品不存在";
                            break;
                        case "04":
                            _returnMessage = "商品已经下架";
                            break;
                        case "07":
                            _returnMessage = "商品库存不足";
                            break;
                        case "06":
                            _returnMessage = "超出兑换次数";
                            break;
                        case "904":
                            _returnMessage = "您的积分不足， 订单付款失败";
                            break;
                        // return Json(new {code = "904", msg = _returnMessage});
                    }
                    return Json(new { code = "403", msg = _returnMessage });
                }
                string _orderId = _order.OrderId;
                //订单支付
                _paySuccess = _AppContext.TradePort.OrderPay(new OrderPayInfo() { userId = _userid, orderId = _orderId });
                if (_paySuccess)
                {
                    string productNames = string.Empty;
                    if (_order.OrderProduct != null)
                    {
                        productNames = string.Join(",", _order.OrderProduct.Select(p => p.Name));
                    }
                    
                    //在通知记录中插入一条信息
                    _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
                    {
                        MsgContent = string.Format("您好，您于{0}在积分商城使用{1}积分兑换了{2}礼品，我们会尽快安排邮寄，请保持手机通讯畅通。物流信息将更新至“个人中心”-“兑换的礼品”中，请您及时关注。", DateTime.Now, _order.Integral, productNames),
                        MsgType = MessageType.System,
                        UserId = _userid
                    });
                    //订单支付成功后在 UserIntegralRecord表中加入一条记录
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral()
                    {
                        userId = _userid,
                        integralSource = "40",
                        value = _order.Integral * (-1),
                        datastate = _order.Datastate,
                        remark = "在线商城消费积分",
                        CreateTime = DateTime.Now,
                        ProductName = productNames
                    });
                    //处理电子卡券
                    string userId = this.User.Identity.GetUserId();
                    var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
                    foreach (var p in _order.OrderProduct)
                    {
                        var cardCodes = "";
                        CustomCardInfo customCardInfo = new CustomCardInfo();
                        var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(p.ProductID);
                        if (!string.IsNullOrEmpty(cardType))
                        {
                            for (int i = 0; i < p.Qty; i++)
                            {
                                var res = AddUserCustomCard(cardType);
                                if (res.IsSuccess)
                                {
                                    customCardInfo = res.Data as CustomCardInfo;
                                    cardCodes += res.Message;
                                    if (p.Qty - i > 1)
                                    {
                                        cardCodes += "，";
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(cardCodes))
                            {
                                bool isCode = _AppContext.OrderApp.EditOrderCardCode(p.OrderID, cardCodes, p.ProductID);
                                if (isCode)
                                {
                                    if (!string.IsNullOrEmpty(customCardInfo.SmsContent))
                                    {
                                        //发送卡券短信信息；
                                        _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = cardCodes },
                                            member.UserName);
                                    }
                                    else
                                    {
                                        if (p.Name == "电子观影券")
                                        {
                                            _AppContext.SMSApp.SendSMS(ESmsType.电子影票, member.UserName,
                                                new string[] { cardCodes }, false);
                                        }
                                        else
                                        {
                                            _AppContext.SMSApp.SendSMS(ESmsType.电子卡券, member.UserName,
                                                new string[] { customCardInfo.ActivityType, cardCodes }, false);
                                        }
                                    }

                                }
                            }
                        }
                    }


                    // DelShopping(_userid, _order.OrderProduct);
                    //成功支付
                    return Json(new { code = "200", msg = "订单支付成功" });
                }
                //支付失败
                return Json(new { code = "904", msg = " 您的积分不足， 订单付款失败" });
            }
            catch (Exception ex)
            {
                return Json(new { code = "400", msg = "系统异常" });
            }

        }


        private ReturnResult AddUserCustomCard(string cardName)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            string userId = this.User.Identity.GetUserId();
            var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            if (member != null)
            {
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardName);
                if (customCardInfo == null)
                {
                    res.IsSuccess = false;
                    res.Message = "卡券信息不正确";
                    return res;
                }
                var cardCode = "";
                var rescardCode = "";
                if (customCardInfo.CardSource == 2)
                {
                    var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(
                        customCardInfo.CardType);
                    if (merchant != null)
                    {
                        cardCode = merchant.CardCode;
                        rescardCode = string.Format("{0}[{1}]", customCardInfo.CardType, merchant.CardCode);
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = "商户券码库存不足了";
                        return res;
                    }
                }
                var customcard = new CustomCard()
                {
                    CardType = customCardInfo.CardType,
                    CardCode = rescardCode,
                    CardId = customCardInfo.Id,
                    CreateTime = DateTime.Now,
                    IsSave = true,
                    IsCancel = false,
                    UserId = userId,
                    IsReissue = false,
                    Tel = member.UserName,
                    IsSend = true,
                    OpenId = "",
                    Source = "blms"
                };
                res = _AppContext.CustomCardApp.AddCustomCard(customcard);
                if (res.IsSuccess)
                {
                    res.Message = cardCode;
                    res.Data = customCardInfo;
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
                }
            }
            return res;
        }

        [HttpPost]
        public ActionResult postAddUserCustomCard(string cardName)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
            string userId = this.User.Identity.GetUserId();
            var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            if (member != null)
            {
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardName);
                if (customCardInfo == null)
                {
                    return Json(new { code = 301, msg = "fail_1" });
                }
                if (DateTime.Now < customCardInfo.CardBeginDate || DateTime.Now > customCardInfo.CardEndDate)
                {
                    return Json(new { code = 301, msg = "fail_2" });
                }

                #region  判断卡券库存；

                var rescardCode = "";
                var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                if (customCardInfo.Quantity - usedCount > 0)
                {
                     rescardCode = Vcyber.BLMS.Common.RandomNumberHelper.GetUserCustomCardCode();
                }
                else
                {
                    return Json(new { code = 301, msg = "很抱歉，卡券库存不足请联系管理员" });
                }
                #endregion

                var customcard = new CustomCard()
                {
                    CardType = customCardInfo.CardType,
                    CardCode = rescardCode,
                    CardId = customCardInfo.Id,
                    CreateTime = DateTime.Now,
                    IsSave = true,
                    IsCancel = false,
                    UserId = userId,
                    IsReissue = false,
                    Tel = member.UserName,
                    IsSend = true,
                    OpenId = "",
                    Source = "blms"
                };
                res = _AppContext.CustomCardApp.AddCustomCard(customcard);
                if (res.IsSuccess)
                {
                    //更新卡券库存；
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customCardInfo.CardType);
                    
                    //用户领取卡券后发送短信提醒；
                    _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo,
                        new CustomCardSms() {CardCode = rescardCode}, member.PhoneNumber);
                }
                else
                {
                    return Json(new { code = 301, msg = "fail_3" });
                }
            }
            return Json(new { code = 302, msg = "success" });
        }

        /// <summary>
        /// 删除购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="products">订单商品信息</param>
        private void DelShopping(string userID, List<OrderProduct> products)
        {
            var productIDs = products.Select<OrderProduct, int>((d) => { return d.ProductID; });
            _AppContext.ShoppingApp.DeleteList(userID, productIDs.ToList<int>());

        }


    }

    public class CompositeOrderList : PagerModel
    {
        public PagedList<Order> AllList { get; set; }
        public PagedList<Order> WaitList { get; set; }

        public int AlllistCount { get; set; }
        public int WaitListCount { get; set; }
        public int WaitReciveCount { get; set; }
        public PagedList<Order> WaitReceiveList { get; set; }
    }



    [Serializable]
    public class PagerModel
    {
        public PagerModel()
        {

        }

        public PagerModel(int pageIndex, int pageSize, int totalCount)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }

        /// <summary>
        ///当前页码 
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            //get
            //{
            //    int pages = this.TotalCount / this.PageSize;
            //    if (this.TotalCount % this.PageSize > 0)
            //    {
            //        pages++;
            //    }
            //    return pages;
            //}
            get
            {
                int i = (int)Math.Ceiling(1.0 * this.TotalCount / this.PageSize);
                return i < 0 ? 0 : i;
            }
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }

        /// <summary>
        /// 是否显示分页信息
        /// </summary>
        public bool ShowPager
        {
            get;
            set;
        }
    }
}