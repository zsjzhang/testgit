var UserRegisterScripts = function () {
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
                ValideCode: {
                    required: true,
                    length: 4
                },
                PassWord: {
                    required: true,
                    pwd: true
                },
                LoginPwd2: {
                    required: true,
                    minlength: 6,
                    equalTo: "input:password[name='PassWord']"
                }
            },
            messages: {
                PhoneNumber: {
                    required: "请输入手机号"
                },
                ValideCode: {
                    required: "请输入验证码",
                    length: "验证码只能输入4个字符"
                },
                PassWord: {
                    required: "请输入密码",
                    pwd: "密码需包含大小写字母、数字，且长度在6-12位"
                },
                LoginPwd2: {
                    required: "请输入确认密码",
                    minlength: "确认密码至少包含6个字符",
                    equalTo: "两次密码输入不一致"
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
            if (!mobile) { mobileAlert("请输入手机号"); return false; }
            if (!common.validate.isMobile(mobile)) { mobileAlert("输入的手机号格式错误。"); return false; }
            common.post(common.api("SMS/APISendValidate"), { mobile: mobile }, function (result) {
                mobileAlert("发送成功，请注意查收，3分钟内使用有效。");
            });
        });
        $("#MType").change(function () {
            if ($(this).attr("checked")=="checked") {
                $("#divIdentityNumber").show();
            } else {
                $("#divIdentityNumber").hide();
            }
        });
    });

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("User/DoFirstPw"), requestData, function (result) {
            mobileAlert("修改密码成功,请重新登陆", function () {
                window.location.href = common.resolveUrl("/User/Login");
            });
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();