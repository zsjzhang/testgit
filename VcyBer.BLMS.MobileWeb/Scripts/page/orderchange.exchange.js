var OrderChangeController = {};

OrderChangeController.AddOrderChange = function (params) {
    $.ajax({
        type: "POST",
        url: common.resolveUrl("/OrderChange/AddOrderChange"),
        data: params,
        dataType: "json",
        success: function (data) {
            if (data.code == "200") {
                window.location.href = common.resolveUrl("/OrderChange/Lottery?orderchangetype =" + params.OrderChangeType + " source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val() + "&flag=" + $("#hidFlag").val() + "&mobile=" + $("#Mobile").val() + "&pagesource=change");
            }
            else {
                if (data.code == "201") {
                    layer.open({
                        type: 1,
                        title: false,
                        skin: 'bluePopup', //样式类名
                        closeBtn: 1, //不显示关闭按钮
                        shift: 2,
                        shadeClose: true, //开启遮罩关闭
                        content: '<h2>您已经成功领取活动礼包</h2><p>快去预约的4S店，使用特权吧！</p>',
                        btn: ['更多好礼'],
                        yes: function (index, layero) {
                            window.location.href = common.resolveUrl("/OrderChange/Activity?source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val());
                        }
                    });
                }
                else {
                    OrderChangeController.PopWindownBlue(data.msg);
                }
            }
        }
    });
};
OrderChangeController.PopWindownBlue = function (msg) {
    layer.open({
        type: 1,
        title: '信息',
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: true, //开启遮罩关闭
        content: '<h2>' + msg + '</h2>'
    });
};
$(function () {
    $("#DealerProvince").change(function () {
        $("#DealerCity option[value!='']").remove();
        $("#DealerId option[value!='']").remove();
        bindCity(this.value);
    });
    $("#DealerCity").change(function () {
        $("#DealerId option[value!='']").remove();
        bindDealer($("#DealerProvince").val(), this.value);
    });

    $(".mainCont .tabTilBox li").on("click", function () {
        $(".mainCont .tabTilBox li").removeClass('on');
        $(this).addClass('on');
        if ($(this).attr("tag") == "zhihuan") {
            $("p[tag='zhihuan']").show();
            if ($("input[name='CarSeriers']:checked").val() == "全新胜达") {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgSd1.jpg") + ")" });
            }
            else {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgLd1.jpg") + ")" });
            }
        }
        else {
            $("p[tag='zhihuan']").hide();
            if ($("input[name='CarSeriers']:checked").val() == "全新胜达") {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgSd0.jpg") + ")" });
            }
            else {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgLd0.jpg") + ")" });
            }
        }


    });
    $(".tabConBox:eq(0) dt span:eq(0)").click(function () {
        if (!$('#mainBox').hasClass('yyBox')) {
            $('#mainBox').removeClass('yyBoxSd');
            $('#mainBox').addClass('yyBox');
        }
        if ($(".tabTilBox>li.on").text() == "预约试驾") {
            $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgLd0.jpg") + ")" });
        }
        else {
            $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgLd1.jpg") + ")" });
        }

    });
    $(".tabConBox:eq(0) dt span:eq(1)").click(function () {
        if (!$('#mainBox').hasClass('yyBoxSd')) {
            $('#mainBox').removeClass('yyBox');
            $('#mainBox').addClass('yyBoxSd');
            if ($(".tabTilBox>li.on").text() == "预约试驾") {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgSd0.jpg") + ")" });
            }
            else {
                $("#mainBox").css({ "background-image": "url(" + common.resolveUrl("/images/bgSd1.jpg") + ")" });
            }
        }
    });
    $("#btnAddOrderChange").click(function () {
        var CarSeriers = $("input[name='CarSeriers']:checked").val();
        var ShopProvince = $("#DealerProvince").val();
        var ShopCity = $("#DealerCity").val();
        var ShopCode = $("#DealerId").val();
        var DealName = $("#DealerId").find("option:selected").text();
        var Name = $("#Name").val();
        var Mobile = $("#Mobile").val();
        var SendProvince = $("#Province").val();
        var SendCity = $("#City").val();
        var SendDistrinct = $("#Area").val();
        var SendAddress = $("#SendAddress").val();
        var InviteCode = $("#InviteCode").val();
        var OldCarBrand = $("#OldCarBrand").val();
        var OldCarSeriers = $("#OldCarSeriers").val();
        var OldCarLicenseYear = $("#OldCarLicenseYear").val();
        var OldCarLicenseMonth = $("#OldCarLicenseMonth").val();
        var OldCarDriver = $("#OldCarDriver").val();


        var type = 1;
        if ($(".tabTilBox>li.on").text() == "预约试驾") {
            type = 2;
        }

        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (CarSeriers == null || CarSeriers == "") {
            OrderChangeController.PopWindownBlue("车型不能为空！");
            return false;
        }
        if (ShopProvince == null || ShopProvince == "") {
            OrderChangeController.PopWindownBlue("请选择试驾店所在省！");
            return false;
        }
        if (ShopCity == null || ShopCity == "") {
            OrderChangeController.PopWindownBlue("请选择试驾店所在市！");
            return false;
        }
        if (ShopCode == null || ShopCode == "") {
            OrderChangeController.PopWindownBlue("请选择试驾店的经销商！");
            return false;
        }
        if (Name == null || Name == "") {
            OrderChangeController.PopWindownBlue("姓名不能为空！");
            return false;
        }
        if (Mobile == null || Mobile == "" || !phoneReg.test(Mobile)) {
            OrderChangeController.PopWindownBlue("联系方式不正确！");
            return false;
        }
        if (SendProvince == null || SendProvince == "") {
            OrderChangeController.PopWindownBlue("邮寄的省不能为空！");
            return false;
        }
        if (SendCity == null || SendCity == "") {
            OrderChangeController.PopWindownBlue("邮寄的市不能为空！");
            return false;
        }
        if (SendDistrinct == null || SendDistrinct == "") {
            OrderChangeController.PopWindownBlue("邮寄的地区不能为空！");
            return false;
        }
        if (SendAddress == null || SendAddress == "") {
            OrderChangeController.PopWindownBlue("邮寄的街道不能为空！");
            return false;
        }
        var OrderChangeType = 1;
        if ($("p[tag='zhihuan']").css("display") == "block") {
            if (OldCarBrand == null || OldCarBrand == "") {
                OrderChangeController.PopWindownBlue("原品牌不能为空！");
                return false;
            }
            if (OldCarSeriers == null || OldCarSeriers == "") {
                OrderChangeController.PopWindownBlue("原车型不能为空！");
                return false;
            }
            if (OldCarLicenseYear == null || OldCarLicenseYear == "") {
                OrderChangeController.PopWindownBlue("首上牌时间的年份不能为空！");
                return false;
            }
            if (OldCarLicenseMonth == null || OldCarLicenseMonth == "") {
                OrderChangeController.PopWindownBlue("首上牌时间的月份不能为空！");
                return false;
            }
            if (OldCarDriver == null || OldCarDriver == "") {
                OrderChangeController.PopWindownBlue("行驶公里数不能为空！");
                return false;
            }
            OrderChangeType = 2;
        }
        var params = {
            ActivityId: $("#hidActivityId").val(),
            CarSeriers: CarSeriers,
            ShopProvince: ShopProvince,
            ShopCity: ShopCity,
            ShopCode: ShopCode,
            DealName: DealName,
            Name: Name,
            Mobile: Mobile,
            SendProvince: SendProvince,
            SendCity: SendCity,
            SendDistrinct: SendDistrinct,
            SendAddress: SendAddress,
            OldCarBrand: OldCarBrand,
            OldCarSeriers: OldCarSeriers,
            OldCarLicenseYear: OldCarLicenseYear,
            OldCarLicenseMonth: OldCarLicenseMonth,
            InviteCode: InviteCode,
            OldCarDriver: OldCarDriver,
            OrderChangeType: OrderChangeType,
            OrderChangeSource: $("#hidOrderChangeSource").val()
        };
        OrderChangeController.AddOrderChange(params);
    });
});

function bindCity(province) {
    common.post(common.api("PageApi/City"), { province: province }, function (result) {
        $.each(result, function (index, item) {
            $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", item.value));
        });
    });
}

function bindDealer(province, city) {
    common.post(common.api("PageApi/Dealer"), { province: province, city: city }, function (result) {
        if (result) {
            $.each(result, function (index, item) {
                $("#DealerId").append(common.format("<option value=\"{0}\">{1}</option>", item.DealerId, item.Name));
            });
        }
    });
}
