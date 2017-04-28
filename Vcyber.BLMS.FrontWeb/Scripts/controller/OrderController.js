var Order = {};
Order.gotoOrder = function () {
    var orderItem = ShoppingCart.MyCart();
};

//订单取消
Order.CancelOrder = function (orderid) {
    $.ajax({
        url: "/Order/CancelOrder",
        type: "post",
        data: { orderId: orderid },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "500") {
                var msg = result.msg || '系统异常';
                popWindownBlue( msg);
                return false;
            }
            else if (result.code == "401") {
                var msg = result.msg || '帐号登陆异常';
                popWindownBlue( msg);
                return false;

            } else if (result.code == "201") {
                var msg = result.msg || '取消失败';
                popWindownBlue( msg);
                return false;

            }
            else if (result.code == "200") {
                var msg = result.msg || '取消成功';
                popWindownBlue(msg, function () {
                    window.location = window.location;
                  
                });
                return false;
            }
        },
        error: function (err) {

        }
    });
    return false;
};

//订单确认
Order.ConfirmOrder = function (orderid) {
    $.ajax({
        url: "/Order/ConfirmOrder",
        type: "post",
        data: { orderId: orderid },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "500") {
                var msg = result.msg || '系统异常';
                popWindownBlue( msg);
                return false;
            }
            else if (result.code == "401") {
                var msg = result.msg || '帐号登陆异常';
                popWindownBlue( msg);
                return false;

            } else if (result.code == "201") {
                var msg = result.msg || '确认失败';
                popWindownBlue( msg);
                return false;

            }
            else if (result.code == "200") {
                var msg = result.msg || '确认成功';
                popWindownBlue(msg, function () {
                    window.location = window.location;
                   
                });
                return false;
            }
        },
        error: function (err) {

        }
    });
    return false;
};

//订单支付
Order.PayOrder = function (orderid) {
    $.ajax({
        url: "/Order/PayOrder",
        type: "post",
        data: { orderId: orderid },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "500") {
                var msg = result.msg || '系统异常';
                popWindownBlue( msg);
                return false;
            }
            else if (result.code == "401") {
                var msg = result.msg || '帐号登陆异常';
                popWindownBlue( msg);
                return false;

            } else if (result.code == "201") {
                var msg = result.msg || '支付失败';
                popWindownBlue( msg);
                return false;

            }
            else if (result.code == "200") {
                var msg = result.msg || '支付成功';
                popWindownBlue(msg, function () {
                    window.location = window.location;
                  
                });
                return false;
            }
        },
        error: function (err) {

        }
    });
    return false;
};

