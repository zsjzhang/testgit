$(function () {
    initValidator();
    $("#DealerProvince").change(function () {
        $("#DealerCity option[value!='']").remove();
        $("#DealerId option[value!='']").remove();
        bindCity(this.value);
    });
    $("#DealerCity").change(function () {
        $("#DealerId option[value!='']").remove();
        bindDealer($("#DealerProvince").val(), this.value);
    });
    $("#DealerId").change(function () {
        $("#DealerName").val($(this).find("option:selected").text());
    });
});
function initValidator() {
    $.validator.setDefaults({
        submitHandler: submitForm
    });
    // 手机号码验证
    $.validator.addMethod("isMobile", function (value, element) {
        var tel = /^1[34578][0-9]{9}$/;
        return this.optional(element) || (tel.test(value));
    }, "请正确填写您的手机号码！");
    $("#mainForm").validate(
        {
            rules: {
                CarSeries: { required: true },
                DealerProvince: { required: true },
                DealerCity: { required: true },
                DealerId: { required: true },
                ScheduleDate: { required: true },
                PurchaseTimeFrame: { required: true },
                UserSex: { required: true },
                UserName: { required: true },
                Phone: { required: true, isMobile: true }
            },
            messages: {
                CarSeries: { required: "请选择车系" },
                DealerProvince: { required: "请选择省" },
                DealerCity: { required: "请选择市" },
                DealerId: { required: "请选择经销商" },
                ScheduleDate: { required: "请选择试驾时间" },
                PurchaseTimeFrame: { required: "请选择计划购车时间" },
                UserSex: { required: "请选择性别" },
                UserName: { required: "请输入姓名" },
                Phone: { required: "请输入联系方式", isMobile: "请输入正确手机号码" }
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
        }
        );
}

function submitForm() {
    var requestData = $("#mainForm").serialize();
    $.post("/common/DoReserveDrive", requestData, function (result) {
        if (result!=null&&result.code == 200) {
            AlertSuccess("预约成功！", function () {
                window.location.href = "/car/index";
            })
        }
        
    });
}
function bindCity(province) {
    $.post("/common/citys", { provinceValue: province }, function (result) {
        $.each(result, function (index, item) {
            $("#DealerCity").append($.format("<option value=\"{0}\">{0}</option>", item));
        });
    });
}

function bindDealer(province, city) {
    $.post("/common/Dealers", { provinceValue: province, cityValue: city }, function (result) {
        if (result) {
            $.each(result, function (index, item) {
                $("#DealerId").append($.format("<option value=\"{0}\">{1}</option>", item.DealerId, item.Name));
            });
        }
    });
}


