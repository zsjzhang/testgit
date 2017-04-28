var ShoppingIndexScripts = function () {
    $(function () {
        $("#categoryId").val($("#category_id").val());
        pagingProduct();        
        $("#categoryId").change(function () {
            $(".shopSect").html("");
            pageIndex = 1;
            pagingProduct();
            $("#divMore").show();
        });
        $("#divMore").click(pagingProduct);
        //如果是注册用户或游客，不能点击兑换        
        $(document).on("click", ".exchangeBtn a", function () {
            var user_level = parseInt($("#user_level").val());
            var user_integral = parseInt($("#user_integral").val());
            var product_identity = parseInt($(this).data("identity"));
            if (user_level == -1) {
                mobileAlert("请先登录后购买!", function () {
                    var categoryId = $("#categoryId").val();
                    window.location.href = "/user/login?return=/shopping?cid=" + categoryId;
                });
                return false;
            }
            else if (user_level == 1) {
                return false;
            }
            else if (user_level == 10) {
                //普卡用户
                if (product_identity != 10 && product_identity != 0) {
                    return false;
                }
                else {
                    var productIntegral = $(this).parent().parent().find(".normal").data("normalintegral");
                    if (parseInt(productIntegral) > user_integral)
                    {
                        mobileAlert("您的积分不足!");
                        return false;
                    }
                }
            }
            else if (user_level == 11) {
                //银卡用户
                if (product_identity != 11 && product_identity != 13 && product_identity != 0) {
                    return false;
                }
                else {
                    var productIntegral = $(this).parent().parent().find(".silver").data("silverintegral");
                    if (parseInt(productIntegral) > user_integral) {
                        mobileAlert("您的积分不足!");
                        return false;
                    }
                }
            }
            else if (user_level == 12) {
                //金卡用户
                if (product_identity != 12 && product_identity != 13 && product_identity != 0) {
                    return false;
                }
                else {
                    var productIntegral = $(this).parent().parent().find(".gold").data("goldintegral");
                    if (parseInt(productIntegral) > user_integral) {
                        mobileAlert("您的积分不足!");
                        return false;
                    }
                }
            }
        });
    });

    var pageIndex = 1;
    function pagingProduct() {
        var productItemHtmlTemp = "\
      <dl><dt><a href=\"{productDetailUrl}\"><img src=\"{ProductImage}\" /></a>\
         </dt>\
                <dd class=\"shopTit\">\{ProductName}\</dd>\
                <dd class=\"shopCarLevel\">\
                    {GoldMemberIntegralHtml}\
                    {SilverMemberIntegralHtml}\
                    {NormalMemberIntegralHtml}\
                </dd>\
                <dd class=\"exchangeBtn\" {ExchangeBtnCss}>\
                    <a href=\"{ExchangeUrl}\" data-identity=\"{ProductIdentity}\" >礼品兑换</a>\
                </dd>\
            </dl>   ";
        common.post(common.resolveUrl("/Shopping/APIProductList"), { categoryId: $("#categoryId").val(), pageIndex: pageIndex }, function (result) {
            var data = result.Items;


            if (data != null) {
                $.each(data, function (index, item) {
                    var productItemHtml = productItemHtmlTemp;
                    productItemHtml = productItemHtml.replace(/{ProductImage}/g, item.ImgUrl);
                    productItemHtml = productItemHtml.replace(/{ProductName}/g, item.Name);

                    //按用户级别显示，如果没有登录则显示全部--begin
                    var is_normal_show = false;
                    var is_gold_show = false;
                    var is_silver_show = false;
                    var GoldMemberIntegralHtml = "<span class='gold' data-goldIntegral='" + item.GoldMemberIntegral + "'>金卡：" + item.GoldMemberIntegral + "</span>";
                    var SilverMemberIntegralHtml = "<span class='silver' data-silverIntegral='" + item.SilverMemberIntegral + "'>银卡：" + item.SilverMemberIntegral + "</span>";
                    var NormalMemberIntegralHtml = "<span class='normal' data-normalIntegral='" + item.Credit + "'>普卡：" + item.Credit + "</span>";
                    switch (item.IsIdentity) {
                        case 10:
                            is_normal_show = true;
                            break;
                        case 11:
                            is_silver_show = true;
                            break;
                        case 12:
                            is_gold_show = true;
                            break;
                        case 13:
                            is_gold_show = true;
                            is_silver_show = true;
                            break;
                        default:
                            is_gold_show = true;
                            is_silver_show = true;
                            is_normal_show = true;
                    }
                    //--end
                    productItemHtml = productItemHtml.replace(/{NormalMemberIntegralHtml}/g, is_normal_show ? NormalMemberIntegralHtml : "");
                    productItemHtml = productItemHtml.replace(/{GoldMemberIntegralHtml}/g, is_gold_show ? GoldMemberIntegralHtml : "");
                    productItemHtml = productItemHtml.replace(/{SilverMemberIntegralHtml}/g, is_silver_show ? SilverMemberIntegralHtml : "");

                    productItemHtml = productItemHtml.replace(/{ExchangeUrl}/g, common.view("/Shopping/DeliveryAddress", { productId: item.Id }));
                    productItemHtml = productItemHtml.replace(/{productDetailUrl}/g, common.view("/Shopping/ProductDetail", { id: item.Id }));
                    productItemHtml = productItemHtml.replace(/{ProductIdentity}/g, item.IsIdentity);
                    //根据用户等级设置兑换按钮css-begin        
                    var user_level = parseInt($("#user_level").val());
                    var product_identity = item.IsIdentity;
                    var exchangeBtnCss = "";
                    if (user_level == 1 ||
                        (user_level == 10 && product_identity != 10 && product_identity != 0) ||
                        (user_level == 11 && product_identity != 11 && product_identity != 13 && product_identity != 0) ||
                        (user_level == 12 && product_identity != 12 && product_identity != 13 && product_identity != 0)
                        ) {
                        exchangeBtnCss = "style='background-color:#a6a6a6'";
                    }
                    productItemHtml = productItemHtml.replace(/{ExchangeBtnCss}/g, exchangeBtnCss);
                    //--end

                    $(".shopSect").append(productItemHtml);
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