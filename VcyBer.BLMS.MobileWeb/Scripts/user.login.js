function log(format, message) {
    return;//skipp the function for product
    $.ajax({url:common.api("User/Log"),
        method:"GET",
        data: { format: format, message: message },
        async: false
    });
}
var UserLoginScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {
                LoginID: {
                    required: true
                },
                LoginPwd: {
                    required: true,
                    minlength: 6
                },
                VerCode: {
                    required: true,
                    length: 4
                }
            },
            messages: {
                LoginID: {
                    required: "请输入手机/VIN码/车牌号/身份证号"
                },
                LoginPwd: {
                    required: "请输入密码",
                    minlength: "密码至少包含6个字符"
                },
                VerCode: {
                    required: "请输入验证码",
                    length: "验证码只能输入4个字符"
                }
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

    $(function () {
        initValidator();
        $(":text").keyup(function () {
            var isShow = true;
            $(":text").each(function (index, element) {
                if ($(element).val().length == 0)
                {
                    isShow = false;
                }
            });
            if (isShow) {
                $(".global-login-btn").removeClass("global-login-btn-bcolor").addClass("global-login-btn-bcolor");
            }

        });
    });

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");        
        common.post(common.api("User/DoLogin"), requestData, function (result) {
            mobileAlert("登录成功", function () {
                var returnUrl = $.query.get("return");
                console.log(returnUrl);
                log("debug", "returnUrl: "+returnUrl);
                if (returnUrl) {
                    if (returnUrl.indexOf("https://") != -1) {
                        window.location.href = returnUrl;
                    }
                    else {
                        console.log("api_url: " + common.api(returnUrl));
                        log("debug", "api_url: " + common.api(returnUrl));
                        window.location.href = common.api(returnUrl);
                    }
                } else {
                    log("debug", "3: " + common.api(returnUrl));
                    window.location.href = common.api("UserCenter/Index");
                }
            }, {
                buttons: [
                    {
                        id: "btnOk",
                        value: "确定",
                        callback: function () {
                            var returnUrl = $.query.get("return");
                            console.log(returnUrl);
                            log("debug", "window.location: " + window.location);
                            log("debug", "returnUrl: " + returnUrl);
                            
                            if (returnUrl) {
                                if (returnUrl.indexOf("https://") != -1) {
                                    window.location.href = returnUrl;
                                }
                                else {
                                    console.log("api_url: " + common.api(returnUrl));
                                    log("debug", "original url: " + returnUrl);
                                    log("debug", "api_url: " + common.api(returnUrl));
                                    window.location.href = common.api(returnUrl);
                                }
                            } else {
                                window.location.href = common.api("UserCenter/Index");
                            }
                        }
                    }
                ]
            });
        }, function (error, errorcode) {
            if (errorcode == "modifypw") {
                $("#errorContainer").html("登录失败");
            }
            else {
                $("#errorContainer").html(error);
            }
          
        });
    }
    
    
    
}();