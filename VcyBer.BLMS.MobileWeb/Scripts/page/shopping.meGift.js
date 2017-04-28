var ShoppingMeGiftScripts = function () {
    $(function () {
        pagingProduct();
        $("#divMore").click(pagingProduct);
    });
    var pageIndex = 1;
    function pagingProduct() {
        var productItemHtmlTemp = $("#productItemHtmlTemp").html();        
        common.post(common.resolveUrl("/Shopping/APIMeGift"), { pageIndex: pageIndex }, function (result) {
            console.log(result);
            var data = result.Items;
            if (data != null) {
                $.each(data, function (index, item) {
                    var productItemHtml = productItemHtmlTemp;
                    productItemHtml = productItemHtml.replace(/{ProductImage}/g, $("#img_root").val() + item.ProductImg);
                    productItemHtml = productItemHtml.replace(/{OrderId}/g, item.OrderId);
                    productItemHtml = productItemHtml.replace(/{ProductName}/g, item.ProductName);
                    productItemHtml = productItemHtml.replace(/{Integral}/g, item.Integral);
                    var is_show_class = item.OrderState == "6" ? "btn_active" : "";
                    productItemHtml = productItemHtml.replace(/{IsShowConfirm}/g, is_show_class);
                    $("#divProductList").append(productItemHtml);
                });
            }
            if (result.TotalPages == 0 || pageIndex >= result.TotalPages) {
                //alert("已到最后一页");
                $("#divMore").hide();
            }
            pageIndex++;
        });
    }
}();
function to_detail(obj) {
    var order_id = $(obj).data("orderid");
    var url = common.resolveUrl("/Shopping/OrderDetails/" + order_id);
    window.location.href = url;
}
function confirm_order(obj) {    
    mobileAlert("确认收货？", null, {
        buttons: [
            {
                id: "popBtn1",
                value: "确定",
                callback: function () {
                    var order_id = $(obj).data("orderid");
                    var url = common.resolveUrl("/Shopping/ReceiptConfirm/" + order_id);
                    $.get(url, function (rsp) {
                        window.location.reload();
                    });
                }
            }, {
                id: "popBtn2",
                value: "取消",
                callback: function () {
                    popOverlayClose();
                }
            }
        ]
    });
    return false;
}