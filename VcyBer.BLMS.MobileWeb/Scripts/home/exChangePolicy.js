$(function () {
    listMove();//中奖名单滚动
    $("#ShopProvince").change(function () {
        $("#ShopCity option[value!='']").remove();
        $("#DealName option[value!='']").remove();
        bindCity(this, this.value);
    });
    $("#SendProvince").change(function () {
        $("#SendCity option[value!='']").remove();
        bindcity(this, this.value)      
    });
    $("#ShopCity").change(function () {
        $("#DealName option[value!='']").remove();
        bindDealer($("#ShopProvince option:selected").text(),this.value)
    });
    $("#btnAddOrderChange").click(function (e) {
        //var CarSeriers = $("#carType").val();
        var CarSeriers = $("#CarSeriers option:selected").val();
        var ShopProvince = $("#ShopProvince option:selected").val();
        var ShopCity = $("#ShopCity").val();
        var DealName = $("#DealName").val();
        var Name = $("#Name").val();
        var Mobile = $("#Mobile").val();
        var SendProvince = $("#Province").val();
        var SendCity = $("#City").val();
        var SendArea = $("#Area").val();
        var SendAddress = $("#SendAddress").val();
        var OldCarBrand = $("#OldCarBrand").val();
        var OldCarSeriers = $("#OldCarSeriers").val();
        var OldCarLicenseTime = $("#OldCarLicenseYear").val() +"-"+ $("#OldCarLicenseMonth").val();
        var OldCarDriver = $("#OldCarDriver").val();
        
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (CarSeriers == null || CarSeriers == "") {
            AlertFalse("车型不能为空！");
            return false;
        }
        if (ShopProvince == null || ShopProvince == "") {
            AlertFalse("请选择省！");
            return false;
        }
        if (ShopCity == null || ShopCity == "") {
            AlertFalse("请选择市！");
            return false;
        }
        if (DealName == null || DealName == "") {
            AlertFalse("请选择经销商！");
            return false;
        }
        if (Name == null || Name == "") {
            AlertFalse("姓名不能为空！");
            return false;
        }
        if (Mobile == null || Mobile == "" || !phoneReg.test(Mobile)) {
            AlertFalse("联系方式不正确！");
            return false;
        }
        if (SendProvince == null || SendProvince == "") {
            AlertFalse("邮寄的省不能为空！");
            return false;
        }
        if (SendCity == null || SendCity == "") {
            AlertFalse("邮寄的市不能为空！");
            return false;
        }
        if (SendArea == null || SendArea == "") {
            AlertFalse("邮寄的 市/区/县 不能为空！");
            return false;
        }
        if (SendAddress == null || SendAddress == "") {
            AlertFalse("邮寄的街道不能为空！");
            return false;
        }

        if (isNaN(OldCarDriver)) {
            AlertFalse("请输入正确公里数！");
            return false;
        }
        $.post("/home/AddOrderChange", $("#exChangeForm").serializeArray(), function (result) {
            if (result.code == 0) {
                AlertFalse(result.msg, function () {
                });
            }
            else {
                if (result.code == 1) {
                    window.location.href = "/Account/login?url=/home/exChangePolicy";
                } else {
                    AlertSuccess(result.msg, function () {
                        $("#exChangeForm")[0].reset();
                    });
                }
            }
        });
    });
});
//中奖名单滚动
function listMove() {
    var ulMove = $('.Listover ul');
    var ulHeight = $('.Listover ul li').height() * 100;
    var liLength = $('.Listover ul li').length;
    if (liLength > 7) {
        $('.Listover ul li').clone().prependTo('.Listover ul');
        ulMove.addClass('move');
        ulMove.css('height', ulHeight);
    }
}
//绑定市
function bindCity(ele, province) {
    $.post("/home/GetCityListByProvince", { province: $(ele).val() }, function (result) {
        
        if (result.success) {
            $.each(result.data, function (index, item) {
                $(ele).next().append("<option value=\"" + item + "\">" + item + "</option>");
            });
        }
    })
}
//绑定经销商
function bindDealer(province, city) {
    
    $.post("/home/GetDealerShipList", { province: province, city: city }, function (result) {
        if (result.success) {
            $.each(result.data, function (index, item) {
                $("#DealName").append("<option value=\"" + item.Name + "\">" + item.Name + "</option>");
            });
        }
    })
}
//ajax-form callback
function rsp_success(data, status, xhr) {
    if (data.IsSuccess) {
        //清空数据
        $(".replace_order input[type='text'],.replace_order textarea").val('');
        $(".replace_order select").each(function () {
            $(this).children().first().prop("selected", true);
        });
        mobileAlert("恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。");
    }
    else {
        mobileAlert(data.Message);
    }
}