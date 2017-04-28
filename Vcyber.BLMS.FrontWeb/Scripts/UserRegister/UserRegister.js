function register() {
}

register.getValidateCode = function () {

    var phoneNumber = $("#PhoneNumber").val().trim();

    if (!register.ValidatePhone(phoneNumber)) {

        return;
    }

    $("#SMSCode").attr("disabled", "disabled");

    $.post("../UserRegister/SMSCode", { "phoneNumber": phoneNumber }, function (data) {
        if (data.Status == 1) {
        }
        else {
            $("#SMSCode").removeAttr("disabled");
        }

        popWindownBlue( data.Message);
    });
}

register.ValidatePhone = function (phoneNumber) {
    var reg = /^(\+\d{2,3}\-)?\d{11}$/;

    if (phoneNumber == null || phoneNumber.length != 11 || !reg.test(phoneNumber)) {
        return false;
    }

    return true;
}

register.ValidatePw = function (pwValue) {

    if (pwValue == null || pwValue.length > 11) {
        return false;
    }

    return true;
}

register.ValidateRealName = function (realName) {
    var reg = /^[\u4E00-\u9FA5]{2,5}(?:·[\u4E00-\u9FA5]{2,5})*$/;

    if (realName == null || !reg.test(realName)) {
        return false;
    }

    return true;
}

register.ValidateIdentityNumber = function (identityNumber) {
    var reg = /^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/;

    if (identityNumber == null || !reg.test(identityNumber)) {
        return false;
    }

    return true;
}

register.IsUserName = function (instance) {
    var phoneNumber = $(instance).val().trim();

    if (!register.ValidatePhone(phoneNumber)) {
        return;
    }

    $.post("../UserRegister/IsUserName", { "phoneNumber": phoneNumber }, function (data) {
        if (data.Status == 1) {
            $("#SMSCode").removeAttr("disabled");
        }
        else {
            popWindownBlue( data.Message);
        }
    });
}

register.IsIdentityNumber = function (instance) {
    var identityNumber = $(instance).val().trim();

    if (!register.ValidateIdentityNumber(identityNumber)) {
        return;
    }

    $.post("../UserRegister/IsIdentityNumber", { "identityNumber": identityNumber }, function (data) {
        if (data.Status == 1) {

        } else {
            popWindownBlue( data.Message);
        }
    });
}

register.NextOne = function () {
    var phoneNumber = $("#PhoneNumber").val().trim();
    var smsCode = $("#ValidateCode").val().trim();
    var isAgree = document.getElementById("Agree").checked;

    if (smsCode == null || !isAgree || !register.ValidatePhone(phoneNumber)) {
        return;
    }

    $.post("../UserRegister/ValidateCode", { "phoneNumber": phoneNumber, "smsCode": smsCode }, function (data) {
        if (data.Status == 1) {
            $("#UserName").text(phoneNumber);
            $("#RegisterOne").css("display", "none");
            $("#RegisterTwo").css("display", "inherit");
            popWindownBlue( data.Message);
        }
        else {
            $("#SMSCode").removeAttr("disabled");
            popWindownBlue( data.Message);
        }
    });
}

register.NextTwo = function () {
    var loginPwOne = $("#loginPwOne").val().trim();
    var loginPwTwo = $("#loginPwTwo").val().trim();
    var realName = $("#realName").val().trim();
    var identityNumber = $("#identityNumber").val().trim();
    var userName = $("#UserName").text();
    var request = $("[name=__RequestVerificationToken]").val();

    if (!register.ValidatePw(loginPwOne) || !register.ValidatePw(loginPwTwo) || loginPwOne != loginPwTwo) {
        return;
    }

    if (!register.ValidateRealName(realName) || !register.ValidateIdentityNumber(identityNumber)) {
        return;
    }

    $.post("../UserRegister/Register", { "userName": userName, "password": loginPwOne, "realName": realName, "identityNumber": identityNumber, "__RequestVerificationToken": request }, function (data) {
        if (data.Status == 1) {
            $("#RegisterTwo").css("display", "none");
            $("#RegisterThree").css("display", "inherit");
        } else {
            popWindownBlue( data.Message);
        }
    });
}