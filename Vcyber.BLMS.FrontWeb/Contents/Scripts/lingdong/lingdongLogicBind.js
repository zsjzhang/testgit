
Lingdong.BindChezhu = function (successCallback) {
    
    var url = common.api("lingdong/Bind");
    
    var userName = $("#userName2").val();
    var phone = $("#phone2").val();
    var validateCode = $("#validateCode2").val();
    var idcard = $("#idcard").val();
    var vin = $("#vin").val();

    if (isnull(userName)) {
        popWindownBlue( "请输入姓名");
        return;
    }
    if (isnull(phone)) {
        popWindownBlue( "请输入手机号码");
        return;
    }
    if (!checktel(phone)) {
        popWindownBlue( "请输入正确手机号");
        return;
    }

    if (isnull(validateCode)) {
        popWindownBlue( "请输入验证码");
        return;
    }
    if (isnull(idcard)) {
        popWindownBlue( "请输入身份证");
        return;
    }
    if (!checkIdCard(idcard)) {
        popWindownBlue( "请输入正确的身份证");
        return;
    }
    if (isnull(vin)) {
        popWindownBlue( "请输入车架号码");
        return;
    }
     

    $.post(url, { openid: Lingdong.oid, phone: phone, validateCode: validateCode, idcard:idcard },
        function (data) {
            if (data.ret == -1) {
                popWindownBlue( "验证码错误");
                return;
            }

            if (data.ret != 1) {
                $("#rzError1").show();
                $("#rzError2").show();
                return;
            }

            Lingdong.isBind = 1;
            Lingdong.recommand();
        }, "json");
    
    Lingdong.userName = userName;
    Lingdong.phoneNumber = phone;
};