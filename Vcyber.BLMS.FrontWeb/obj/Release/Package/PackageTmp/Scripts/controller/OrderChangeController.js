var OrderChangeController = {};

OrderChangeController.AddOrderChange = function (params) {
    $.ajax({
        type: "POST",
        url: "/OrderChange/AddOrderChange",
        data: params,
        dataType:"json",
        success: function (data) {
            
            OrderChangeController.PopWindownBlue(data.msg);
            $(':input', '#permuteForm')
            .not(':button, :submit, :reset, :hidden')
            .val('')
            .removeAttr('checked')
            .removeAttr('selected');
        }
    });
};
//OrderChangeController.AddXDInviter = function (params) {
//    $.ajax({
//        type: "POST",
//        url: "/OrderChange/AddXDInviter",
//        data: params,
//        dataType: "json",
//        success: function (data) {
//            OrderChangeController.PopWindownBlue(data.msg);
//        }
//    });
//};

OrderChangeController.PopWindownBlue = function (msg, callBack) {
    layer.open({
        type: 1,
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: false, //开启遮罩关闭
        content: msg,
        end: function () {
            if (callBack) {
                callBack();
            }
        },
        btn: ['确定']
    });
};

$(function () {
    $("#btnAddOrderChange").click(function () {
        //var CarSeriers = $("#carType").val();
        var CarSeriers = $("#carType option:selected").text();        
        var ShopProvince = $("#sltProvince").val();
        var ShopCity = $("#sltCity").val();
        var ShopCode = $("#sltDealer").val();
        var Name = $("#zhName").val();
        var Mobile = $("#zhTel").val();
        var SendProvince = $("#Province").val();
        var SendCity = $("#City").val();
        var SendArea = $("#Area").val();
        var SendAddress = $("#zhiHuanAddText").val();
        var OldCarBrand = $("#OldCarBrand").val();
        var OldCarSeriers = $("#OldCarSeriers").val();
        var OldCarLicenseTime = $("#OldCarLicenseTime").val();
        var OldCarDriver = $("#zhMil").val();

        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (CarSeriers == null || CarSeriers == "") {
            OrderChangeController.PopWindownBlue("车型不能为空！");
            return false;
        }
        if (ShopProvince == null || ShopProvince == "-1") {
            OrderChangeController.PopWindownBlue("请选择省！");
            return false;
        }
        if (ShopCity == null || ShopCity == "-1") {
            OrderChangeController.PopWindownBlue("请选择市！");
            return false;
        }
        if (ShopCode == null || ShopCode == "-1") {
            OrderChangeController.PopWindownBlue("请选择经销商！");
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
        if (SendArea == null || SendArea == "") {
            OrderChangeController.PopWindownBlue("邮寄的 市/区/县 不能为空！");
            return false;
        }
        if (SendAddress == null || SendAddress == "") {
            OrderChangeController.PopWindownBlue("邮寄的街道不能为空！");
            return false;
        }
        //if (OldCarBrand == null || OldCarBrand == "") {
        //    OrderChangeController.PopWindownBlue("原有品牌不能为空！");
        //    return false;
        //}
        //if (OldCarSeriers == null || OldCarSeriers == "") {
        //    OrderChangeController.PopWindownBlue("原车型不能为空！");
        //    return false;
        //}
        //if (OldCarLicenseTime == null || OldCarLicenseTime == "") {
        //    OrderChangeController.PopWindownBlue("首上牌时间不能为空！");
        //    return false;
        //}
        //if (OldCarDriver == null || OldCarDriver == "") {
        //    OrderChangeController.PopWindownBlue("行驶里程数不能为空！");
        //    return false;
        //}

        if (isNaN(OldCarDriver)) {
            OrderChangeController.PopWindownBlue("请输入正确公里数！");
            return false;
        }

        var params = {
            ActivityId: $("#hidActivityId").val(),
            CarSeriers: CarSeriers,
            ShopProvince: ShopProvince,
            ShopCity: ShopCity,
            ShopCode: ShopCode,
            Name: Name,
            Mobile: Mobile,
            SendProvince: SendProvince,
            SendCity: SendCity,
            SendAddress: SendAddress,
            OldCarBrand: OldCarBrand,
            OldCarSeriers: OldCarSeriers,
            OldCarLicenseTime: OldCarLicenseTime,
            OldCarDriver: OldCarDriver,
            OrderChangeSource:"blms_web"
        };
        OrderChangeController.AddOrderChange(params);
    });

    //$("#btnAddXDInviter").click(function () {
    //    var InviteredName = "hhh";//$("#InviteredName").val();
    //    var InviteredSex = "男";//$("#InviteredSex").val();
    //    var InviteredMobile = "13439970624";//$("#InviteredMobile").val();
    //    var InviteredCar = "岭东";//$("#InviteredCar").val();

    //    if (InviteredSex == null || InviteredSex == "") {
    //        OrderChangeController.PopWindownBlue("性别不能为空！");
    //        return false;
    //    }
    //    if (InviteredName == null || InviteredName == "") {
    //        OrderChangeController.PopWindownBlue("姓名不能为空！");
    //        return false;
    //    }
    //    var phoneReg = new RegExp("^1[0-9]{10}$");
    //    if (InviteredMobile == null || InviteredMobile == "" || !phoneReg.test(InviteredMobile)) {
    //        OrderChangeController.PopWindownBlue("手机号不正确！");
    //        return false;
    //    }
    //    var params = {
    //        InviteredSex: InviteredSex,
    //        InviteredName: InviteredName,
    //        InviteredMobile: InviteredMobile,
    //        InviteredCar: InviteredCar,
    //        InviterSource: 2
    //    };

    //    OrderChangeController.AddXDInviter(params);
    //});
});