﻿var OrderChange = {};
OrderChange.PopWindownBlue = function (msg) {
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
OrderChange. IdentityCodeValid = function (code) {
    var city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
    var tip = "";
    var pass = true;

    if (!code || !/^[1-9][0-9]{5}(19[0-9]{2}|200[0-9]|2010)(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])[0-9]{3}[0-9xX]$/i.test(code)) {
        tip = "身份证号码位数不正确！";
        pass = false;
    }

    else if (!city[code.substr(0, 2)]) {
        tip = "地址编码错误";
        pass = false;
    }
    else {
        //18位身份证需要验证最后一位校验位
        if (code.length == 18) {
            code = code.split('');
            //∑(ai×Wi)(mod 11)
            //加权因子
            var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
            //校验位
            var parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
            var sum = 0;
            var ai = 0;
            var wi = 0;
            for (var i = 0; i < 17; i++) {
                ai = code[i];
                wi = factor[i];
                sum += ai * wi;
            }
            var last = parity[sum % 11];
            if (parity[sum % 11] != code[17]) {
                tip = '身份证号码位数错误！';//"校验位错误";
                pass = false;
            }
        }
    }
    return pass;
}
OrderChange.BindCar = function () {
    var identityNumber = $("#identityNumber").val();
    if (identityNumber == null || identityNumber=="") {
        OrderChange.PopWindownBlue("请输入您的身份证号!");
        return;
    }
    /*
    if (!OrderChange.IdentityCodeValid(identityNumber)) {
        OrderChange.PopWindownBlue("请输入正确的身份证号!");
        return;
    }*/

    $.ajax({
        type: "POST",
        url: "/OrderChange/BindIdentityNumber",
        data: { userId: $("#hidUserId").val(), identityNumber: identityNumber },
        dataType: "json",
        success: function (data) {
            if (data.code == "200") {
                window.location.href = "/OrderChange/TuiJian?source=" + $("#hidSource").val() + "&flag=" + $("#hidFlag").val();
            }
            else {
                OrderChange.PopWindownBlue(data.msg);
            }
        }
    });
};
$(function () {
    $("#btnBind").click(function () {
        OrderChange.BindCar();
    });
});