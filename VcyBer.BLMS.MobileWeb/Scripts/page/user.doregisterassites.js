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
                Captcha: {
                    required: true,
                    length: 4
                },

                NickName: {
                    required: true
                },
                IdentityNumber: {
                    required: true//,
                    //IDCardCN: true
                },
                Password: {
                    required: true,
                    pwd: true
                },
                ConfirmPassword: {
                    required: true,
                    minlength: 6,
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
                Captcha: {
                    required: "请输入验证码",
                    length: "验证码只能输入4个字符"
                },
                IdentityNumber: {
                    required: "请输入身份证号"
                },
                NickName: {
                    required: "请输入用户名"
                },
                Password: {
                    required: "请输入密码",
                    pwd: "密码需包含大小写字母、数字，且长度至少6位"
                },
                ConfirmPassword: {
                    required: "请输入确认密码",
                    minlength: "确认密码至少包含6个字符",
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
            if (!mobile) { mobileAlert("请输入手机号。"); return false; }
            if (!common.validate.isMobile(mobile)) { mobileAlert("输入的手机号格式错误。"); return false; }
            common.post(common.api("SMS/APISendValidate"), { mobile: mobile }, function (result) {
                mobileAlert("发送成功，请注意查收，3分钟内使用有效。");
            });
        });



        var owner = $(".owner"), hideNo = $(".hideNo"), suojiu = $("#suojiu"), BusinessuserRow = $(".BusinessuserRow"), divVin = $("#divVin"), payOther = $(".payOther"), PayNumber = $("#divPayNumber");
        owner.on("click", function () {
            var self = $(this);
            if ((self.find("input").val()) === "1") {
                hideNo.show();
            } else {
                hideNo.hide();
            }
        });
        suojiu.change(function () {
            if (($(this).find("input").attr("checked")) == "checked") {
                $("#hideS9").show();
            } else {
                $("#hideS9").hide();
            }
        });
        BusinessuserRow.on("click", function () {
            var self = $(this);
            if ((self.find("input").val()) === "2") {
                divVin.show();
            } else {
                divVin.hide();
            }
        });
        payOther.on("click", function () {
            var self = $(this);
            if ((self.find("input").val()) === "1") {
                PayNumber.show();
            } else {
                PayNumber.hide();
            }
        });


        $("#DealerProvince").change(function () {
            $("#DealerCity option[value!='']").remove();
            $("#DealerId option[value!='']").remove();
            $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", "选择城市"));

            bindCity(this.value);
        });
        $("#DealerCity").change(function () {

            bindDealer($("#DealerProvince").val(), this.value);
        });
    });

    function submitForm() {
        $("#errorContainer").html("");
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("User/DoRegisterForNew"), requestData, function (openId) {
            mobileAlert("注册成功", function () {
                window.location.href = common.api("/User/Login", { openId: openId });
            });
        }, function (error) {
            //alert(error);
            if (error == "证件号已经存在,请输入VIN码")
            {
                $("#divVin").show();
                $("#errorContainer").html(error);
            }
            else if (error == "当前手机号已注册，请直接登录") {
                mobileAlert("当前手机号已注册，请直接登录", function () {
                    window.location.href = common.api("/User/Login");
                });
            }
            else {
                $("#errorContainer").html(error);
            }
        });
    }


    function bindCity(province) {
        common.post(common.api("PageApi/City"), { province: province }, function (result) {
            $.each(result, function (index, item) {
                $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", item.value));
            });
            $("#DealerCity").attr("value", city);
            bindDealer($("#DealerProvince").val(), $("#DealerCity").val());

        });
    }


    function bindDealer(province, city) {
        common.post(common.api("PageApi/Dealer"), { province: province, city: city }, function (result) {
            if (result) {
                $.each(result, function (index, item) {
                    $option = $("<option value=\"" + item.DealerId + "\">" + item.Name + "</option>");
                    $("#DealerId").append($option);
                });
               
            }
        });
    }
}();

