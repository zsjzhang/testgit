var UserRegisterScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {                
                Mobile: {
                    required: true,
                    mobileCN: true
                },
                VerCode: {
                    required: true
                },
                Captcha: {
                    required: true,
                    length: 4
                },
                
                NickName: {
                    required: true
                },
                IdentityNumber: {
                    required: true,
                    IDCardCN: true
                },
                Password: {
                    required: true,
                    pwd: true
                },
                ConfirmPassword: {
                    required: true,
                    minlength: 8,
                    equalTo: "input:password[name='Password']"
                },
                VIN: {
                    required: true//,
                    //IDCardCN: true
                }
            },
            messages: {
                Mobile: {
                    required: "请输入手机号"
                },
                VerCode: {
                    required: "请输入图形验证码"
                },
                Captcha: {
                    required: "请输入验证码",
                    length: "验证码只能输入4个字符"
                },
                IdentityNumber: {
                    required: "证件号不能为空"
                },
                NickName: {
                    required: "请输入昵称"
                },
                Password: {
                    required: "请输入密码",
                    pwd: "密码需包含大小写字母、数字，且长度在8-12位"
                },
                ConfirmPassword: {
                    required: "请输入确认密码",
                    minlength: "确认密码至少包含8个字符",
                    equalTo: "两次密码输入不一致"
                },
                VIN: {
                    required: "请输入VIN"
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
            var mobile = $("#Mobile").val();
            var code = $("#VerCode").val();
            if (!mobile) { mobileAlert("请输入手机号。"); return false; }
            if (!common.validate.isMobile(mobile)) { mobileAlert("输入的手机号格式错误。"); return false; }
            if (!code) { mobileAlert("请输图形验证码。"); return false; }

            common.post(common.api("SMS/APISendValidateCode"), { mobile: mobile,verCode:code }, function (result) {
                mobileAlert("发送成功，请注意查收，3分钟内使用有效。");
            });
        });
        $("#MType").change(function () {
            if ($(this).attr("checked")=="checked") {
                $("#divIdentityNumber").show();
                $("#divBusinessUser").show();
            } else {
                $("#divIdentityNumber").hide();
                $("#divBusinessUser").hide();
                $("#divVin").hide();
                $("#Businessuser").attr("checked", false);
            }
        });

        $("#Businessuser").change(function () {
            if ($(this).attr("checked") == "checked") {
                $("#divVin").show();
            } else {
                $("#divVin").hide();
            }
        });
        reg_agree_change();
        $("#regAgree").on("change", function () {
            reg_agree_change();
        });
    });
    //根据同意不同意开启或禁用提交按钮
    function reg_agree_change()
    {
        if ($("#regAgree").prop("checked")) {
            $(".global-reg-btn").css("background-color", "#3387d7").removeAttr("disabled");
        }
        else {
            $(".global-reg-btn").css("background-color", "#a6a6a6").attr("disabled","disabled");
        }
    }
    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("User/DoRegisterForNew"), requestData, function (openId) {
            mobileAlert("注册成功", function () {
                window.location.href = common.api("/UserCenter/index", { openId: openId });
            });
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();