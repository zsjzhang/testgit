var UserForgetPwdScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {
                PhoneNumber: {
                    required: true,
                    mobileCN: true
                },
                NewPassword: {
                    required: true,
                    pwd: true
                },
                ConfirmPassword: {
                    required: true,
                    minlength: 8,
                    equalTo: "input:password[name='NewPassword']"
                },
                VerCode: {
                    required: true,
                    length: 4
                },
                VerifyCode: {
                    required:true
                }
            },
            messages: {
                PhoneNumber: {
                    required: "请输入手机号", mobileCN: "请输入正确手机号码"
                },
                NewPassword: {
                    required: "请输入密码",
                    pwd: "密码需包含大小写字母、数字，且长度在8-12位"
                },
                ConfirmPassword: {
                    required: "请输入确认密码",
                    minlength: "确认密码至少包含8个字符",
                    equalTo: "两次密码输入不一致"
                },
                VerCode: {
                    required: "请输入验证码",
                    length: "验证码只能输入4个字符"
                },
                VerifyCode: {
                    required: "请输入图形验证码"
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
        $("#btnSendVerCode").click(function () {
            var mobile = $("#PhoneNumber").val();
            var code = $("#VerifyCode").val();
            if (!mobile) { mobileAlert("请输入手机号"); return false; }
            if (!common.validate.isMobile(mobile)) { mobileAlert("输入的手机号格式错误。"); return false; }
            if (!code) { mobileAlert("请输图形验证码。"); return false; }

            common.post(common.api("SMS/APISendValidateCode"), { mobile: mobile,verCode:code }, function (result) {
                mobileAlert("发送成功，请注意查收，3分钟内使用有效。");
            });
        });
    });

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("User/DoForgetPwd"), requestData, function (result) {
            //mobileAlert("密码重置成功。", function () { });
                window.location.href = common.api("/User/Login");
            
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();