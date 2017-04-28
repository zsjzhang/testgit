var MagazineSubscriptionScripts = function () {
    function initValidator() {
        $("#mainForm").validate({
            submitHandler: function () {
                //var typeId = $("#ddlType").val();
                var typeId = 1;
                typeId=$("#ddlType").find("option:selected").val();
                $("input:hidden[name='TypeID']").val(typeId);
                if (typeId == 1) {
                    $("#mainForm").hide();
                    $("#form_Electronic").show();
                } else if (typeId == 2) {
                    $("#mainForm").hide();
                    $("#form_Paper").show();
                } else if (typeId==3) {
                    window.location.href = "http://app.focussend.com/focussend/cust/bluemembers/201509/editionB/bluemembers.html";
                }
                $("#mainForm div[name='errorContainer']").html("");
            },
            rules: { ddlType: { required: true } },
            messages: { ddlType: { required: "请选择杂志类型" } },
            onfocusout: false,
            onkeyup: false,
            onclick: false,
            showErrors: function (errorMap, errorList) {
                var errMsg = "";
                $.each(errorList, function (i, v) {
                    errMsg += (v.message + "<br />");
                });
                if (errMsg != "") {
                    $("#mainForm div[name='errorContainer']").html(errMsg);
                }
            }
        });
        //电子杂志
        $("#form_Electronic").validate({
            submitHandler: function () {
                submitForm("form_Electronic");
            },
            rules: {
                Name: { required: true },
                Email: { required: true, email: true }
            },
            messages: {
                Name: { required: "请输入姓名" },
                Email: { required: "请输入电子邮件", email: "电子邮件格式错误" }
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
                    $("#form_Electronic div[name='errorContainer']").html(errMsg);
                }
            }
        });
        //纸质杂志
        $("#form_Paper").validate({
            submitHandler: function () {
                submitForm("form_Paper");
            },
            rules: {
                Name: { required: true },
                Mobile: { required: true, mobileCN: true },
                ZipCode: { required: true },
                Province: { required: true },
                City: { required: true },
                Area: { required: true },
                Address: { required: true }
            },
            messages: {
                Name: { required: "请输入姓名" },
                Mobile: { required: "请输入手机号" },
                ZipCode: { required: "邮编" },
                Province: { required: "请选择省份" },
                City: { required: "请选择城市" },
                Area: { required: "请选择区/县" },
                Address: { required: "请输入详细地址" }
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
                    $("#form_Paper div[name='errorContainer']").html(errMsg);
                }
            }
        });
    }

    $(function () {
        initValidator();
        $("input:button[name='btnPrev']").click(function () {
            $("#mainForm").show();
            $("#form_Electronic").hide();
            $("#form_Paper").hide();
        });
    });

    function submitForm(formId) {
        var requestData = common.serializeJsonByFrom(formId);
        common.post(common.api("Magazine/APISubscription"), requestData, function (openId) {
            //mobileAlert("订阅成功");
            $("#" + formId).find("div[name='errorContainer']").html("");
            //TODO:跳转到个人中心页面
            mobileAlert("订阅成功", function () {
                window.location.href = common.api("/usercenter/index");
            });
            //windows.location.href = "/usercenter/index";
        }, function (error) {
            $("#" + formId).find("div[name='errorContainer']").html(error);
        });
    }
}();