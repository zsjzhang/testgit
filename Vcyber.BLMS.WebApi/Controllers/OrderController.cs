using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models.ResponseData;

namespace Vcyber.BLMS.WebApi.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// 更改订单状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="orderId">订单号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateOrderStatus")]
        [IOVAuthorize]
        public IHttpActionResult UpdateOrderStatus(string userId, string orderId, string status)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                #region  验证传入参数值
                if (string.IsNullOrEmpty(userId))
                {
                    result.Message = "userID为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(orderId))
                {
                    result.Message = "订单号为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(status))
                {
                    result.Message = "状态值为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //订单是否存在
                var order = _AppContext.OrderApp.GetOrder(orderId);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                // 是否已经完成支付
                if (order.PayState != (int)EPayState.ZFWC)
                {
                    result.Message = "订单未完成支付";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //会员是否存在
                var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
                if (member == null)
                {
                    result.Message = "该用户ID不存在";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                #endregion
                //交易完成，更新订单状态；
                //bool orderResult = _AppContext.OrderApp.EditTradeState(orderId, (ETradeState)Convert.ToInt32(status));
                bool orderResult = _AppContext.OrderApp.EditTradeState(orderId, ETradeState.JYWC);
                if (orderResult)
                {
                    _AppContext.OrderApp.SetReceiveTime(orderId);
                }
                else
                {
                    result.Message = "更新数据失败";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                result.Message = "SUCCESS";
                result.IsSuccess = true;
                return Ok(result);
            }
            catch (Exception)
            {
                result.Message = "ERROR";
                result.IsSuccess = false;
                return Ok(result);
            }
        }



        /// <summary>
        /// 兑换商品详情
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns>兑换商品详情</returns>
        [HttpPost]
        [Route("GetOrderProductDetail")]
        [ResponseType(typeof(ResOrderProductDetail))]
        [IOVAuthorize]
        public IHttpActionResult GetOrderProductDetail(string orderId)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    result.Message = "订单号为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //订单是否存在
                var order = _AppContext.OrderApp.GetOrder(orderId);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    result.IsSuccess = false;
                    return Ok(result);
                }

                //构建兑换商品详情
                var resOrderProductDetail = new ResOrderProductDetail()
                {

                    OrderId = order.OrderId,
                    OrderTime = order.Createtime.ToString(),
                    TradeState = order.TradeState.ToString(),
                    PayState = order.PayState.ToString(),
                    Integral = order.Integral.ToString(),
                    PayTime = order.Updatetime

                };
                //收货信息
                var address = _AppContext.OrderApp.GetOrderAddress(orderId);
                if (address != null)
                {
                    resOrderProductDetail.OrderAddress = string.Format("{0}{1}", address.PCC, address.Detail);
                    resOrderProductDetail.ReceiveName = address.ReceiveName;
                    resOrderProductDetail.ReceivePhone = address.Phone;
                }

                //订单商品信息
                var product = _AppContext.OrderApp.GetOrderProduct(orderId);
                resOrderProductDetail.Product = new List<Products>();
                if (product != null)
                {
                    foreach (var p in product)
                    {
                        resOrderProductDetail.Product.Add(new Products()
                        {
                            ProductQty = p.Qty.ToString(),
                            ProductImg = p.Img,
                            ProductName = p.Name,
                            ProductTotal = Convert.ToString(p.Integral * p.Qty)
                        });
                    }
                }

                //物流单信息
                var shipping = _AppContext.OrderApp.GetOrderShipping(orderId);
                if (shipping != null)
                {
                    resOrderProductDetail.ShippingNumber = shipping.Number;
                    resOrderProductDetail.ShippingName = shipping.Name;
                    resOrderProductDetail.ReceiveTime = shipping.ReceiveTime;
                }

                result.Data = resOrderProductDetail;
                result.Message = "SUCCESS";
                result.IsSuccess = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Message = "ERROR";
                result.IsSuccess = false;
                return Ok(result);
            }
        }


        /// <summary>
        /// 兑换记录分页列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="index">当前索引</param>
        /// <param name="page">分页个数</param>
        /// <returns>兑换记录分页列表</returns>
        [HttpPost]
        [Route("GetUserAllOrderProductList")]
        [ResponseType(typeof(Page<ResUserOrderProducts>))]
        [IOVAuthorize]
        public IHttpActionResult GetUserAllOrderProductList(string userId, string index, string page)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    result.Message = "userID为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(index))
                {
                    result.Message = "当前索引为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(page))
                {
                    result.Message = "分页个数为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //会员是否存在
                var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
                if (member == null)
                {
                    result.Message = "该用户ID不存在";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                int total = 0;
                PageData pageData = new PageData() { Index = Convert.ToInt32(index), Size = Convert.ToInt32(page) };
                var userOrderProducts = _AppContext.OrderApp.GetUserOrderProductList(userId, pageData, out total);
                List<ResUserOrderProducts> list = new List<ResUserOrderProducts>();
                if (userOrderProducts != null && userOrderProducts.Count() > 0)
                {
                    foreach (var item in userOrderProducts)
                    {
                        list.Add(new ResUserOrderProducts()
                        {
                            OrderId = item.OrderID,
                            Integral = item.Integral.ToString(),
                            OrderState = item.TradeState.ToString(),
                            OrderDate = item.Createtime.ToString(),
                            ProductName = item.Name,
                            ProductImg = item.Img,
                            ProductQty = item.Qty.ToString()
                        });
                    }
                }
                Page<ResUserOrderProducts> pageD = new Page<ResUserOrderProducts>();
                pageD.Items = list;
                pageD.CurrentPage = Convert.ToInt32(index);
                pageD.ItemsPerPage = Convert.ToInt32(page);
                pageD.TotalItems = total;
                pageD.TotalPages = total % pageData.Size == 0 ? total / pageData.Size : total / pageData.Size + 1; ;

                result.Data = pageD;
                result.Message = "SUCCESS";
                result.IsSuccess = true;
                return Ok(result);
            }
            catch (Exception)
            {
                result.Message = "ERROR";
                result.IsSuccess = false;
                return Ok(result);
            }
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConfirmOrder")]
        public IHttpActionResult ConfirmOrder(string orderId)
        {
            var result = new ReturnObject("200", "success", "");
            var flag = _AppContext.OrderApp.ConfirmOrder(orderId);
            if (!flag)
            {
                result.Code = "400";
                result.Message = "确认收货操作失败";
            }
            return this.Ok(result);
        }        
    }
}