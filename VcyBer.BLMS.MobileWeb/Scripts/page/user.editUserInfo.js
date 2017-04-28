var UserEditUserInfoScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {
                NickName: {
                    required: true,
                    nickname: true,
                    rangelength: [2, 12]
                },
                IdentityNumber: {
                    IDCardCN: true
                }
              
            },
            messages: {
                NickName: {
                    required: "请输入昵称",
                    nickname: "用户名不能为纯数字",
                    rangelength: "请输入2-12个字符"
                },
                IdentityNumber: {
                    IDCardCN: "身份证填写错误"
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
    });

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("User/APIEditUserInfo"), requestData, function (result) {
            mobileAlert("修改成功", function () {
                window.location.href = common.resolveUrl("/UserCenter/Index");
            });
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();