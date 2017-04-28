var ShoppingIndexScripts = function () {
    $(function () {
        pagingProduct();
        $("#divMore").click(pagingProduct);
    });

    var pageIndex = 1;
    function pagingProduct() {
        var productItemHtmlTemp = "\
        <div class=\"shop-cp-row\">\
            <div class=\"col\">\
                <a href=\"{productDetailUrl}\"><img src=\"{ProductImage}\" /></a>\
            </div>\
            <div class=\"col\">\
                <strong><a href=\"{productDetailUrl}\">{ProductName}</a></strong>\
                <p>积分：<em>{Integral}</em><br />蓝豆：<em>{BlueBean}</em> </p>\
            </div>\
            <div class=\"col\">\
                <a href=\"{ExchangeUrl}\" class=\"change-btn\">礼品兑换</a>\
            </div>\
        </div>";
        common.post(common.resolveUrl("/Shopping/APIProductList"), { categoryId: $.query.get("categoryId"), pageIndex: pageIndex }, function (result) {
            var data = result.Items;
            if (data != null) {
                $.each(data, function (index, item) {
                    var productItemHtml = productItemHtmlTemp;
                    productItemHtml = productItemHtml.replace(/{ProductImage}/g, item.ImgUrl);
                    productItemHtml = productItemHtml.replace(/{ProductName}/g, item.Name);
                    productItemHtml = productItemHtml.replace(/{Integral}/g, item.Credit);
                    productItemHtml = productItemHtml.replace(/{ExchangeUrl}/g, common.view("/Shopping/DeliveryAddress", { productId: item.Id }));
                    productItemHtml = productItemHtml.replace(/{BlueBean}/g, item.BlueBean);
                    productItemHtml = productItemHtml.replace(/{productDetailUrl}/g, common.view("/Shopping/ProductDetail", { id: item.Id }));
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