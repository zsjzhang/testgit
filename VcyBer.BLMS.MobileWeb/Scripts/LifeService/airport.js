
//机场预约表单验证
function initValidator() {
    $("#mainForm").validate({
        rules: {
            airAgree: { required: true },
            hiduseAirNum: { required: true, range: [1, 100] },
            Terminal: { required: true },
            phoneNumber: { required: true, mobileCN: true }
        },
        messages: {
            airAgree: { required: "请选择在预约后3个月内使用" },
            hiduseAirNum: { required: "请选择预约候机服务次数", range: "预约候机服务次数错误" },
            Terminal: { required: "请选择航站楼" },
            phoneNumber: { required: "请填写手机号码" }
        },
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        showErrors: function (errorMap, errorList) {
            var errMsg = "";
            $.each(errorList, function (i, v) {
                errMsg += (v.message + "<br />");
            });
            if (errMsg != "") {
                $("#errorContainer").html(errMsg);
            }
        }
    });
}
//机场预约相关
$(function () {
    //根据省份加载机场
    $("#AirProvince").change(function () {
        $("#AirPort option[value!='']").remove();
        $("#Terminal option[value!='']").remove();
        $.ajax({
            url: "/LifeService/GetAirportsByProvince",
            type: "get",
            dataType: "json",
            data: { province: $("#AirProvince").find("option:selected").val(), t: (new Date()).getTime() },
            success: function (result) {
                //var data = '(' + result + ')';
                $.each(result, function (index, item) {
                    //debugger;
                    $("#AirPort").append("<option value=\"" + item.Id + "\">" + item.AirportName + "</option>");
                });
            },
            error: function (err) {
            }
        });
    });
    //根据机场加载候机室
    $("#AirPort").change(function () {
        $("#Terminal option[value!='']").remove();
        var airportName = $("#AirPort").find("option:selected").text();
        if (airportName !== "") {
            $.ajax({
                url: "/LifeService/GetAirportRoomsByAirportName",
                type: "get",
                dataType: "json",
                data: { airportName: airportName, t: (new Date()).getTime() },
                success: function (result) {
                    $.each(result, function (index, item) {
                        //debugger;
                        $("#Terminal").append("<option value=\"" + item.Id + "\">" + item.AirportAllName + "</option>");
                    });
                },
                error: function (err) {

                }
            });
        }
    });
    //免费次数预约递增
    $("#addFreeNum").click(function () {
        var hidFreeNum = Number($("#hidFreeNum").val());
        iptFreeNum = Number($("#iptFreeNum").val());
        useAirNum = Number($("#useAirNum").html());
        if (iptFreeNum < hidFreeNum) {
            iptFreeNum = iptFreeNum + 1
            $("#iptFreeNum").val(iptFreeNum);
            $("#useFreeNum").html(iptFreeNum);
            $("#useAirNum").html(useAirNum + 1);
            $("#hiduseAirNum").val(useAirNum + 1);
        }
        else {
            AlertFalse("您最多能预约" + hidFreeNum + "次");
            return false;
        }
    });
    //免费次数预约递减
    $("#lessFreeNum").click(function () {
        iptFreeNum = Number($("#iptFreeNum").val());
        useAirNum = Number($("#useAirNum").html());
        if (iptFreeNum > 0) {
            iptFreeNum = iptFreeNum - 1
            $("#iptFreeNum").val(iptFreeNum);
            $("#useFreeNum").html(iptFreeNum);
            $("#useAirNum").html(useAirNum - 1);
            $("#hiduseAirNum").val(useAirNum - 1);
        }
    });
    //积分兑换次数递增
    $("#addIntegralNum").click(function () {
        //积分免费兑换的总数
        var hidIntegral = Number($("#hidIntegral").val());
        iptIntegralNum = Number($("#iptIntegralNum").val());
        useAirNum = Number($("#useAirNum").html());
        if (iptIntegralNum < hidIntegral) {
            iptIntegralNum = iptIntegralNum + 1
            $("#iptIntegralNum").val(iptIntegralNum);
            $("#useIntegra").html(iptIntegralNum * 1800);
            $("#useAirNum").html(useAirNum + 1);
            $("#hiduseAirNum").val(useAirNum + 1);
        }
        else {
            AlertFalse("您最多能预约" + hidIntegral + "次");
            return false;
        }
    });
    //积分兑换次数递减
    $("#lessIntegralNum").click(function () {
        iptIntegralNum = Number($("#iptIntegralNum").val());
        useAirNum = Number($("#useAirNum").html());
        if (iptIntegralNum > 0) {
            iptIntegralNum = iptIntegralNum - 1
            $("#iptIntegralNum").val(iptIntegralNum);
            $("#useIntegra").html(iptIntegralNum * 1800);
            $("#useAirNum").html(useAirNum - 1);
            $("#hiduseAirNum").val(useAirNum - 1);
        }
    });
});
//预约表单提交
function submitForm() {
    //debugger;
    var _userId = $("#SonataLiveReserveUserId").val();
    var _phoneNumber = $("#phoneNumber").val();
    var _freeCount = $("#iptFreeNum").val();
    var _scoreCount = $("#iptIntegralNum").val();
    var _airportId = $("#Terminal").find("option:selected").val()
    var _isCheckConfirm = $("#airAgree").is(':checked');
    if (_userId == null || _userId == "") {
        AlertFalse("请先登录后再预约");
        window.location.href = "/Account/Login";
        return false;
    }
    if (!_isCheckConfirm) {
        AlertFalse("请选择在预约后3个月内使用");      
        return false;
    }
    if (_freeCount == null || parseInt(_freeCount) < 0) {
        AlertFalse("请正确输入免费预约次数");
        return false;
    }
    if (_scoreCount == null || parseInt(_scoreCount) < 0) {
        AlertFalse("请正确输入积分预约次数");
        return false;
    }
    if (parseInt(_scoreCount) <= 0 && parseInt(_freeCount) <= 0) {
        AlertFalse("请正确填写预约的次数");
        return false;
    }
    if (_airportId == null || _airportId == "" || parseInt(_airportId) < 0) {
        AlertFalse("请选择机场航站楼");
        return false;
    }
    if (_phoneNumber == null || _phoneNumber == "") {
        AlertFalse("请输入验证码接收手机号");
        return false;
    }
    else {
        var reg = /^(1)\d{10}$/;
        if (!reg.test(_phoneNumber)) {
            AlertFalse("请输入正确的手机号码");
            return false;
        }
    }

    $.ajax({
        url: "/LifeService/LiveReserve",
        type: "post",
        dataType: "json",
        data: { userId: _userId, phoneNumber: _phoneNumber, freeCount: _freeCount, scoreCount: _scoreCount, airportId: _airportId, __RequestVerificationToken: document.getElementsByName('__RequestVerificationToken')[0].value },
        success: function (result) {
            if (result.IsSuccess) {
                //debugger;
                var sncodes = [];
                if (result.Data !== null) {
                    result.Data.forEach(function (obj) {
                        sncodes.push(obj.SNCode);
                    });
                }
                setCookie("sncodes", JSON.stringify(result), 1);
                window.location.href = "/LifeService/ReserveSuccess";
            } else {
                AlertFalse(result.Message);
                return false;
                //window.location.href = "/Account/Login?url="+result.Data;
            }
        },
        error: function () {

        }
    });
}
//设置cookie  
//name是cookie中的名，value是对应的值，iTime是多久过期（单位为天）  
function setCookie(name, value, iTime) {
    var oDate = new Date();
    //设置cookie过期时间  
    oDate.setDate(oDate.getDate() + iTime);
    document.cookie = name + '=' + value + ';expires=' + oDate.toGMTString();
}
//获取cookie  
function getCookie(name) {
    //cookie中的数据都是以分号加空格区分开  
    var arr = document.cookie.split("; ");
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].split("=")[0] == name) {
            return arr[i].split("=")[1];
        }
    }
    //未找到对应的cookie则返回空字符串  
    return '';
}

