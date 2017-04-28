var ShoppingIndexScripts = function () {
    $(function () {
        //商品列表
        product_query();
        //点击加载更多
        $("#pull").click(product_query);
        //验证用户身份，meber_status = 0(没登录),1(没有权限)，2（可以兑换）     
        var member_status = parseInt($("#member_status").val());
        var member_message = $("#member_message").val();
        $(document).on("click", ".exchangeBtn", function () {
            if (member_status == 0 || member_status == 1 || member_status == -1) {
                mobileAlert(member_message);
                return false;                
            }
        });
    });
    //加载商品列表
    var pageIndex = 1;
    function product_query() {
        var product_item_tmpl = '<li><a href="{ProductDetailUrl}" class="bri_shoppic"><img src="{ProductImage}" /></a>'
        product_item_tmpl += '<h4 class="bri_shopname">{ProductName}</h4>'
        product_item_tmpl += '<div class="bri_receive"><a href="{ExchangeUrl}" class="exchangeBtn" >立即领取</a></div></li>';
        common.post(common.resolveUrl("/Member/GetBirthdayGiftList"), { pageIndex: pageIndex }, function (rsp) {
            var data = rsp.Items;
            if (data != null) {
                $.each(data, function (index, item) {
                    var item_html = product_item_tmpl;
                    item_html = item_html.replace(/{ProductDetailUrl}/g, common.view("/Member/BirthdayGiftDetail", { id: item.Id }));
                    item_html = item_html.replace(/{ProductImage}/g, item.ImgUrl);
                    item_html = item_html.replace(/{ProductName}/g, item.Name);
                    item_html = item_html.replace(/{ExchangeUrl}/g, common.view("/Shopping/DeliveryAddress", { productId: item.Id }));                    
                    $(".shopSect").append(item_html);
                });
            }                     
            if ($("#member_status").val() == "1") {
                $(".exchangeBtn").css("background-color", "#a6a6a6");
            }
            if (rsp.TotalPages == 0 || pageIndex >= rsp.TotalPages) {
                //alert("已到最后一页");
                $("#pull").hide();
            }
            pageIndex++;
        });
    }
}();