using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Application;

using System.Web.Http.Description;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models;
using System.IO;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using System.Runtime.Remoting;
    using PetaPoco;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.Entity.SelectCondition;
    using Vcyber.BLMS.WebApi.Filter;
    using Vcyber.BLMS.Common;
    using AspNet.Identity.SQL;


    /// <summary>
    /// 商品业务
    /// </summary>
    public class ProductController : ApiController
    {
        #region ==== 构造函数 ====

        public ProductController() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取商品类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/product/categorylist")]
        [ResponseType(typeof(List<CategoryV>))]
        public IHttpActionResult CategoryList()
        {
            CategoryCollection dataResult = new CategoryCollection() { status = "00", datas = new List<CategoryV>() };
            var category = _AppContext.CategoryApp.GetList();

            if (category != null && category.Count() > 0)
            {
                dataResult.status = "99";

                foreach (var item in category)
                {
                    dataResult.datas.Add(new CategoryV() { ID = item.ID, Name = item.Name });
                }
            }
            return this.Ok(new ReturnObject(dataResult.datas));
        }

        /// <summary>
        /// 获取孩子商品类型
        /// </summary>
        /// <param name="categoryId">商品父类型</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/product/categoryChildlist")]
        [ResponseType(typeof(List<CategoryV>))]
        public IHttpActionResult CategoryChildList(int categoryId)
        {
            CategoryCollection dataResult = new CategoryCollection() { status = "00", datas = new List<CategoryV>() };
            var category = _AppContext.CategoryApp.GetList(categoryId);

            if (category != null && category.Count() > 0)
            {
                dataResult.status = "99";

                foreach (var item in category)
                {
                    dataResult.datas.Add(new CategoryV() { ID = item.ID, Name = item.Name });
                }
            }
            return this.Ok(new ReturnObject(dataResult.datas));
            //return this.DataResult<CategoryCollection>(dataResult, dataResult.status);
        }

        /// <summary>
        /// 根据类型名称获取类型ID
        /// </summary>
        /// <param name="categoryName">类型名称</param>
        /// <returns>类型ID</returns>
        [HttpGet]
        [Route("api/product/categorybyname")]
        [ResponseType(typeof(String))]
        public IHttpActionResult CategoryByName(string categoryName)
        {
            var rsp = new ReturnObject("2", "success", "");
            try
            {
                var categoryId = _AppContext.CategoryApp.selectIdByName(categoryName);
                rsp.Content = categoryId;
            }
            catch
            {
                rsp.Code = "0";
                rsp.Message = "该类型不存在！";
            }
            return this.Ok(rsp);
        }

        /// <summary>
        /// 获取商品列表信息
        /// </summary>
        /// <param name="type">商品类型Id</param>
        /// <param name="index">当前索引</param>
        /// <param name="page">分页个数</param>
        /// <param name="payType">支付方式：2：积分；4：蓝豆</param>
        /// <param name="pxModel">排序方式：1：销售；2：积分；3：蓝豆</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/product/productlist")]
        [ResponseType(typeof(Page<ProductV>))]
        public IHttpActionResult ProductList(EPayType? payType = 0, EPaiXuMode? pxModel = 0, int type = 0, int index = 1, int page = 10)
        {
            int total = 0;
            payType = ((int)payType) == 0 ? null : payType;
            pxModel = ((int)pxModel) == 0 ? null : pxModel;
            ProductCollectionV dataResult = new ProductCollectionV() { gifts = new List<ProductV>() };
            PageData pageData = new PageData() { Index = index, Size = page };

            var products = _AppContext.ProductApp.GetProduct(new ProductSearchCondition() { CategoryID = type, PayType = payType, PXModel = pxModel }, pageData, out total);

            if (products != null && products.Count() > 0)
            {
                dataResult.totalrecord = total;
                dataResult.totalpage = total % pageData.Size == 0 ? total / pageData.Size : total / pageData.Size + 1;
                dataResult.record = products.Count();
                dataResult.page = pageData.Index;
                dataResult.type = type;

                foreach (var item in products)
                {
                    dataResult.gifts.Add(new ProductV() { addtime = item.ShelfTime.ToString(), credit = item.Integral, expire = "", id = item.ID, imgurl = item.Image, name = item.Name, BlueBean = item.BlueBean, GoldMemberIntegral = (int)item.FrontGoldMemberIntegral, SilverMemberIntegral = (int)item.FrontSilverMemberIntegral, IsIdentity = item.IsIdentity, IsIdentityText = item.IsIdentityText });
                }
            }


            Page<ProductV> pageD = new Page<ProductV>();
            pageD.Items = dataResult.gifts;
            pageD.CurrentPage = index;
            pageD.ItemsPerPage = page;
            pageD.TotalItems = total;
            pageD.TotalPages = dataResult.totalpage;
            return this.Ok(new ReturnObject("200", pageD));
            //return this.DataResult<ProductCollectionV>(dataResult, status);
        }
        /// <summary>
        /// 获取商品列表信息(生日特权)
        /// </summary>
        /// <param name="type">商品类型Id</param>
        /// <param name="index">当前索引</param>
        /// <param name="page">分页个数</param>
        /// <param name="payType">支付方式：2：积分；4：蓝豆</param>
        /// <param name="pxModel">排序方式：1：销售；2：积分；3：蓝豆</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/product/productforbirthdaylist")]
        [ResponseType(typeof(Page<ProductV>))]
        public IHttpActionResult ProductForBirthdayList(EPayType? payType = 0, EPaiXuMode? pxModel = 0, int type = 0, int index = 1, int page = 10)
        {
            int total = 0;
            payType = ((int)payType) == 0 ? null : payType;
            pxModel = ((int)pxModel) == 0 ? null : pxModel;
            ProductCollectionV dataResult = new ProductCollectionV() { gifts = new List<ProductV>() };
            PageData pageData = new PageData() { Index = index, Size = page };

            var products = _AppContext.ProductApp.GetProductByBirthday(new ProductSearchCondition() { CategoryID = type, PayType = payType, PXModel = pxModel }, pageData, out total);

            if (products != null && products.Count() > 0)
            {
                dataResult.totalrecord = total;
                dataResult.totalpage = total % pageData.Size == 0 ? total / pageData.Size : total / pageData.Size + 1;
                dataResult.record = products.Count();
                dataResult.page = pageData.Index;
                dataResult.type = type;

                foreach (var item in products)
                {
                    dataResult.gifts.Add(new ProductV() { addtime = item.ShelfTime.ToString(), credit = item.Integral, expire = "", id = item.ID, imgurl = item.Image, name = item.Name, BlueBean = item.BlueBean, GoldMemberIntegral = (int)item.FrontGoldMemberIntegral, SilverMemberIntegral = (int)item.FrontSilverMemberIntegral, IsIdentity = item.IsIdentity, IsIdentityText = item.IsIdentityText });
                }
            }


            Page<ProductV> pageD = new Page<ProductV>();
            pageD.Items = dataResult.gifts;
            pageD.CurrentPage = index;
            pageD.ItemsPerPage = page;
            pageD.TotalItems = total;
            pageD.TotalPages = dataResult.totalpage;
            return this.Ok(new ReturnObject("200", pageD));
            //return this.DataResult<ProductCollectionV>(dataResult, status);
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns>00:商品不存在</returns>
        [HttpGet]
        [Route("api/product/detail")]
        [ResponseType(typeof(ProductDetailV))]
        public IHttpActionResult Detail(int id)
        {
            ProductDetailV dataResult = new ProductDetailV() { imgurls = new List<ProductImgV>() };

            var product = _AppContext.ProductApp.GetProduct(id, EProductState.Putaway, EDataStatus.NoDelete);
            if (product == null)
            {
                product = _AppContext.ProductApp.GetProduct(id, EProductState.Putaway, EDataStatus.Delete);
            }
            if (product != null)
            {
                dataResult.id = product.ID;
                dataResult.name = product.Name;
                dataResult.credit = product.Integral;
                dataResult.identity = product.IsIdentity;
                dataResult.maxPer = product.MaxPer;
                dataResult.maxUser = product.MaxUser;
                dataResult.total = product.Qty;
                dataResult.addtime = product.ShelfTime.ToString();
                dataResult.expire = "";
                dataResult.type = product.Category.CategoryName;
                dataResult.BlueBean = product.BlueBean;
                dataResult.ProductColorIds = product.ProductColorIds;
                dataResult.ProductColorList = product.ProductColorList;
                dataResult.ProductTypeIds = product.ProductTypeIds;
                dataResult.ProductTypeList = product.ProductTypeList;
                dataResult.GoldMemberIntegral = (int)product.FrontGoldMemberIntegral;
                dataResult.SilverMemberIntegral = (int)product.FrontSilverMemberIntegral;
                dataResult.IsIdentityText = product.IsIdentityText;
                //add by wangchunrong 20161221
                dataResult.State = product.State;
                foreach (var item in product.Images)
                {
                    dataResult.imgurls.Add(new ProductImgV() { imgurl = item.Image });
                }
            }
            return this.Ok(new ReturnObject(dataResult));
            //return this.DataResult<ProductDetailV>(dataResult, status);
        }

        /// <summary>
        /// 兑换商品
        /// </summary>
        /// <param name="data">交易信息</param>
        /// <returns>支付是否成功(200:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败;08:积分或蓝豆不足)</returns>
        [HttpPost]
        [Route("api/product/trade")]
        [ResponseType(typeof(bool))]
        [IOVAuthorize]
        public IHttpActionResult TradeProduct(TradeInfoV data)
        {
            if (data == null || !data.ValidateData() || (data.payType != EPayType.Integral.ToInt32() && data.payType != EPayType.BlueBean.ToInt32()))
            {
                return BadRequest();
            }

            string status;
            var orderAddress = data.ConvertOrderAddress();
            orderAddress.DataSource = this.GetSysCode();
            string orderId = _AppContext.TradePort.CreateOrder(data.userid, data.ConvertProductData(), orderAddress, (EPayType)data.payType, out status);
            var order = _AppContext.OrderApp.GetOrder(orderId);
            string Name = getProductName(data.ConvertProductData());
            if (status == "99") status = "200";
            if (!string.IsNullOrEmpty(orderId))
            {
                bool result = _AppContext.TradePort.OrderPay(data.userid, orderId, (EPayType)data.payType);
                status = result ? "200" : "08";
                //处理电子卡券
                if (result)
                {
                    //添加统计记录表 
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                    {
                        userId = data.userid,
                        integralSource = "70",
                        value = order.Integral * -1,
                        datastate = 0,
                        remark = "APP在线商城消费积分",
                        CreateTime = DateTime.Now,
                        ProductName = Name
                    });
                    GetOrderProductCardCode(data.userid, orderId);
                }
            }

            return this.Ok(new ReturnObject(status));
        }

        /// <summary>
        /// 获取用户兑换的商品
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="index"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/product/traderecord")]
        [ResponseType(typeof(Page<OrderProductV>))]
        [IOVAuthorize]
        public IHttpActionResult TradeRecord(string userId, int index = 1, int page = 10)
        {
            string status = "00";   
            int total = 0;
            OrderProductCollectionV dataResult = new OrderProductCollectionV() { results = new List<OrderProductV>() };
            PageData pageData = new PageData() { Index = index, Size = page };

            var products = _AppContext.OrderApp.GetUserProduct(userId, pageData, out total);

            if (products != null && products.Count() > 0)
            {
                dataResult.totalrecord = total;
                dataResult.totalpage = total % pageData.Size == 0 ? total / pageData.Size : total / pageData.Size + 1;
                dataResult.record = products.Count();
                dataResult.page = pageData.Index;

                foreach (var item in products)
                {
                    dataResult.results.Add(new OrderProductV() { credit = item.Integral, date = item.Createtime.ToString(), name = item.Name, BlueBean = item.BlueBean });
                }

                status = "99";
            }

            Page<OrderProductV> pageD = new Page<OrderProductV>();
            pageD.Items = dataResult.results;
            pageD.CurrentPage = index;
            pageD.ItemsPerPage = page;
            pageD.TotalItems = total;
            pageD.TotalPages = dataResult.totalpage;
            return this.Ok(new ReturnObject(pageD));
            //return this.DataResult<OrderProductCollectionV>(dataResult, status);
        }

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="index"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpGet]
        [Route("api/product/tradeflow")]
        [ResponseType(typeof(Page<TradeflowV>))]
        public IHttpActionResult TradeFlow(string userId, int index = 1, int page = 10)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return this.BadRequest();
            }

            int total;
            List<TradeflowV> dataResult = new List<TradeflowV>(10);
            IEnumerable<Tradeflow> tempData = _AppContext.TradeFlowApp.GetList(userId, new PageData() { Index = index, Size = page }, out total);

            if (tempData != null && tempData.Count() > 0)
            {
                foreach (var item in tempData)
                {
                    dataResult.Add(new TradeflowV() { BlueBean = item.BlueBean, CreateTime = item.CreateTime.ToString(), FlowCode = item.FlowCode, orderId = item.orderId, remark = item.remark, tradeintegral = item.tradeintegral, TradeType = item.TradeType, UserId = item.UserId });
                }
            }

            Page<TradeflowV> pageD = new Page<TradeflowV>();
            pageD.Items = dataResult;
            pageD.CurrentPage = index;
            pageD.ItemsPerPage = page;
            pageD.TotalItems = total;
            pageD.TotalPages = dataResult != null && dataResult.Count() > 0 ? dataResult.Count() : 0;
            return this.Ok(new ReturnObject(pageD));
        }

        /// <summary>
        /// 在线商城，电子卡券商品领取兑换码接口
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="productName">商品名称</param>
        /// <param name="userId">用户ID</param>
        /// <param name="phone">手机号码</param>
        /// <param name="orderId">订单ID</param>
        /// <returns>卡券GUID</returns>
        [HttpGet]
        [Route("api/product/GetOrderProductCategoryCardType")]
        [ResponseType(typeof(ReturnResult))]
        [IOVAuthorize]
        public IHttpActionResult GetOrderProductCategoryCardType(int productId, string productName, string userId, string phone, string orderId)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true };
            if (productId == 0)
            {
                result.IsSuccess = false;
                result.Message = "商品ID不正确";
                return this.Ok(result);
            }
            if (string.IsNullOrEmpty(productName))
            {
                result.IsSuccess = false;
                result.Message = "商品名称不能为空";
                return this.Ok(result);
            }
            if (string.IsNullOrEmpty(userId))
            {
                result.IsSuccess = false;
                result.Message = "用户ID不能为空";
                return this.Ok(result);
            }
            if (string.IsNullOrEmpty(phone))
            {
                result.IsSuccess = false;
                result.Message = "手机号码不能为空";
                return this.Ok(result);
            }
            if (string.IsNullOrEmpty(orderId))
            {
                result.IsSuccess = false;
                result.Message = "订单ID不能为空";
                return this.Ok(result);
            }
            string cardCode = _AppContext.OrderApp.GetOrderProductCardCode(productId, orderId);
            if (!string.IsNullOrEmpty(cardCode))
            {
                result.IsSuccess = false;
                result.Message = "该商品已领取过兑换码";
                return this.Ok(result);
            }
            var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(productId);
            if (string.IsNullOrEmpty(cardType))
            {
                result.IsSuccess = false;
                result.Message = "未找到卡券ID";
                return this.Ok(result);
            }
            if (!string.IsNullOrEmpty(cardType))
            {
                var res = AddUserCustomCard(cardType, userId);
                if (res.IsSuccess)
                {
                    var customCardInfo = res.Data as CustomCardInfo;
                    bool isCode = _AppContext.OrderApp.EditOrderCardCode(orderId, res.Message, productId);
                    if (isCode)
                    {
                        if (!string.IsNullOrEmpty(customCardInfo.SmsContent))
                        {
                            //发送卡券短信信息；
                            _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = res.Message }, phone);
                        }
                        else
                        {
                            if (productName == "电子观影券")
                            {
                                _AppContext.SMSApp.SendSMS(ESmsType.电子影票, phone, new string[] { res.Message }, false);
                            }
                            else
                            {
                                _AppContext.SMSApp.SendSMS(ESmsType.电子卡券, phone, new string[] { "星巴克", res.Message },
                                    false);
                            }
                        }
                        result.Message = res.Message;
                    }
                }
            }
            return this.Ok(result);
        }


        /// <summary>
        /// 电子商城获取劵码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public string GetOrderProductCardCode(string userId, string orderId)
        {
            //处理电子卡券
            var orderProductList = _AppContext.OrderApp.GetOrderProduct(orderId);
            string cardType = "";
            var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            var cardCodes = "";
            foreach (var p in orderProductList)
            {
                CustomCardInfo customCardInfo = new CustomCardInfo();
                cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(p.ProductID);
                if (!string.IsNullOrEmpty(cardType))
                {
                    for (int i = 0; i < p.Qty; i++)
                    {
                        var res = AddUserCustomCard(cardType, userId);
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
                    _AppContext.OrderApp.EditTradeState(p.OrderID, ETradeState.JYWC);//卡券类商品置为确认收货
                    if (!string.IsNullOrEmpty(cardCodes))
                    {
                        bool isCode = _AppContext.OrderApp.EditOrderCardCode(p.OrderID, cardCodes, p.ProductID);
                        if (isCode)
                        {
                            //_AppContext.OrderApp.EditTradeState(p.OrderID, ETradeState.JYWC);//卡券类商品置为确认收货
                            if (!string.IsNullOrEmpty(customCardInfo.SmsContent))
                            {
                                //发送卡券短信信息；
                                _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = cardCodes }, member.UserName);
                            }
                            else
                            {
                                if (p.Name == "电子观影券")
                                {
                                    _AppContext.SMSApp.SendSMS(ESmsType.电子影票, member.UserName, new string[] { cardCodes },
                                        false);
                                }
                                else
                                {
                                    _AppContext.SMSApp.SendSMS(ESmsType.电子卡券, member.UserName,
                                        new string[] { "星巴克", cardCodes }, false);
                                }
                            }
                        }
                    }
                }
            }
            return cardCodes;
        }

        private ReturnResult AddUserCustomCard(string cardName, string userId)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };
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
                    Source = "blms_web"
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
        /// <summary>
        /// 获取商品详情（微信）
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns>00:商品不存在</returns>
        [HttpGet]
        [Route("api/product/detailforweixin")]
        [ResponseType(typeof(ProductDetailV))]
        public IHttpActionResult DetailForWeixin(int id)
        {
            ProductDetailForWexin dataResult = new ProductDetailForWexin() { imgurls = new List<ProductImgV>() };

            var product = _AppContext.ProductApp.GetProduct(id, EProductState.Putaway, EDataStatus.NoDelete);
            var cardCode = _AppContext.OrderApp.GetOrderProductCategoryCardCode(id);
            if (product == null)
            {
                product = _AppContext.ProductApp.GetProduct(id, EProductState.Putaway, EDataStatus.Delete);
            }
            if (product != null)
            {
                dataResult.id = product.ID;
                dataResult.name = product.Name;
                dataResult.credit = product.Integral;
                dataResult.identity = product.IsIdentity;
                dataResult.maxPer = product.MaxPer;
                dataResult.maxUser = product.MaxUser;
                dataResult.total = product.Qty;
                dataResult.addtime = product.ShelfTime.ToString();
                dataResult.expire = "";
                dataResult.type = product.Category.CategoryName;
                dataResult.CardCode = cardCode;
                dataResult.BlueBean = product.BlueBean;
                dataResult.ProductColorIds = product.ProductColorIds;
                dataResult.ProductColorList = product.ProductColorList;
                dataResult.ProductTypeIds = product.ProductTypeIds;
                dataResult.ProductTypeList = product.ProductTypeList;
                dataResult.GoldMemberIntegral = (int)product.FrontGoldMemberIntegral;
                dataResult.SilverMemberIntegral = (int)product.FrontSilverMemberIntegral;
                dataResult.IsIdentityText = product.IsIdentityText;
                foreach (var item in product.Images)
                {
                    dataResult.imgurls.Add(new ProductImgV() { imgurl = item.Image });
                }
            }
            return this.Ok(new ReturnObject(dataResult));
        }
        /// <summary>
        /// 兑换商品（微信）
        /// </summary>
        /// <param name="data">交易信息</param>
        /// <returns>支付是否成功(200:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败;08:积分或蓝豆不足)</returns>
        [HttpPost]
        [Route("api/product/tradeforweixin")]
        [ResponseType(typeof(bool))]
        [IOVAuthorize]
        public IHttpActionResult TradeProductForWeixin(TradeInfoV data)
        {
            if (data == null || !data.ValidateData() || (data.payType != EPayType.Integral.ToInt32() && data.payType != EPayType.BlueBean.ToInt32()))
            {
                return BadRequest();
            }

            string status;
            var cardCode = string.Empty;
            var orderAddress = data.ConvertOrderAddress();
            orderAddress.DataSource = this.GetSysCode();
            //获取商品名称 
            string Name = getProductName(data.ConvertProductData());
            string orderId = _AppContext.TradePort.CreateOrder(data.userid, data.ConvertProductData(), orderAddress, (EPayType)data.payType, out status);
            var order = _AppContext.OrderApp.GetOrder(orderId);
            if (status == "99") status = "200";
            if (!string.IsNullOrEmpty(orderId))
            {
                bool result = _AppContext.TradePort.OrderPay(data.userid, orderId, (EPayType)data.payType);
                status = result ? "200" : "08";
                //处理电子卡券
                if (result)
                {
                    //添加统计记录表 
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                    {
                        userId = data.userid,
                        integralSource = "50",
                        value = order.Integral * -1,
                        datastate = 0,
                        ProductName = Name,//添加商品名称add by wangchunrong 20161205
                        remark = "微信在线商城消费积分",
                        CreateTime = DateTime.Now
                    });   
                    cardCode = GetOrderProductCardCode(data.userid, orderId);
                }
            }
            var rsp = new ReturnObject(status);
            rsp.Content = cardCode;
            return this.Ok(rsp);
        }

        /// <summary>
        /// 获取商品名称
        /// </summary>
        /// <param name="ProductInfo"></param>
        /// <returns></returns>
        public string getProductName(List<OrderProduct> ProductInfo)
        {
            //在积分记录表里添加商品名称字段
            string name = string.Empty;
            foreach (var item in ProductInfo)
            {
                if (item.Name != "" && item.Name != null)
                {
                  name = item.Name.ToString();
            }
                else
                {
                    name = "";
                }
            }
            return name;
        }

        /// <summary>
        /// 判断是否可以兑换生日特权（微信）
        /// </summary>        
        /// <param name="userId">用户id</param>
        /// <returns>是否可以兑换（0：没有登录，1：没有权限，2：可以兑换）</returns>        
        [Route("api/product/checkbirthgift")]
        [HttpGet]
        [ResponseType(typeof(String))]
        public IHttpActionResult CheckBirthDayGift(string userId)
        {
            //获取生日特权商品类型ID
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");
            var store = new FrontUserStore<FrontIdentityUser>();
            var rsp = new ReturnObject("2", "success", "");
            if (string.IsNullOrEmpty(userId))
            {
                rsp.Code = "0";
                rsp.Message = "请登录后领取";
            }
            else
            {
                var user = store.FindByIdAsync(userId).Result; //根据当前用户id查询用户实体
                if (user == null)
                {
                    rsp.Code = "0";
                    rsp.Message = "登录已失效,请重新登录";
                }
                else if (string.IsNullOrEmpty(user.Birthday))
                {
                    rsp.Code = "1";
                    rsp.Message = "您还没有设置生日信息！";
                }
                else
                {
                    var month = DateTime.Parse(user.Birthday).Month;
                    if (user.MLevel != 12)
                    {
                        rsp.Code = "1";
                        rsp.Message = "该权益只有金卡会员可以享受哦";
                    }
                    else if (month != DateTime.Now.Month)
                    {
                        rsp.Code = "1";
                        rsp.Message = "您好,本活动只针对生日当月的用户！";
                    }
                    else if (_AppContext.ProductApp.checkProduct(categoryId, userId) > 0)
                    {
                        rsp.Code = "1";
                        rsp.Message = "您好,您已领取了生日礼物，不要太贪心哦！";
                    }
                }
            }
            return this.Ok(rsp);
        }
        #endregion
    }
}
