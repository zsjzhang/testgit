var ShoppingAddDeliverAddressScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {
                ReceiveName: {
                    required: true
                },
                ProvinceID: {
                    required: true
                },
                CityID: {
                    required: true
                },
                CountyID: {
                    required: true
                },
                Detail: {
                    required: true
                },
                ZipCode: {
                    required: true
                },
                Phone: {
                    required: true, mobileCN: true
                }
            },
            messages: {
                ReceiveName: {
                    required: "请输入收货人姓名"
                },
                ProvinceID: {
                    required: "请选择省份"
                },
                CityID: {
                    required: "请选择城市"
                },
                CountyID: {
                    required: "请选择县"
                },
                Detail: {
                    required: "请输入详细地址"
                },
                ZipCode: {
                    required: "请输入邮政编码"
                },
                Phone: {
                    required: "请输入联系电话", mobileCN: "请输入正确手机号码"
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
        common.post(common.api("Shopping/APIProvince"), null, function (result) {
            $.each(result, function (index, item) {
                $("#ddlProvinceID").append(common.format("<option value=\"{0}\" code=\"{1}\">{2}</option>", item.ID, item.Code, item.Name));
            });
        });
        $("#ddlProvinceID").change(function () {
            $("#ddlCityID option[value!='']").remove();
            $("#ddlCountyID option[value!='']").remove();
            bindCity($("#ddlProvinceID option:selected").attr("code"));
        });
        $("#ddlCityID").change(function () {
            $("#ddlCountyID option[value!='']").remove();
            bindArea($("#ddlCityID option:selected").attr("code"));
        });
        $("#ddlCountyID").change(function () {
            var provinceName = $("#ddlProvinceID option:selected").text();
            var cityName = $("#ddlCityID option:selected").text();
            var areaName = $("#ddlCountyID option:selected").text();
            $("#PCC").val(common.format("{0},{1},{2}", provinceName, cityName, areaName))
        });
    });

    function bindCity(provinceCode) {
        common.post(common.api("Shopping/APICity"), { provinceCode: provinceCode }, function (result) {
            $.each(result, function (index, item) {
                $("#ddlCityID").append(common.format("<option value=\"{0}\" code=\"{1}\">{2}</option>", item.ID, item.Code, item.Name));
            });
        });
    }
    function bindArea(cityCode) {
        common.post(common.api("Shopping/APIArea"), { cityCode: cityCode }, function (result) {
            $.each(result, function (index, item) {
                $("#ddlCountyID").append(common.format("<option value=\"{0}\" code=\"{1}\">{2}</option>", item.ID, item.Code, item.Name));
            });
        });
    }

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("Shopping/APIAddDeliveryAddress"), requestData, function (result) {
            mobileAlert("添加成功", function () {
                window.location.href = common.view("/Shopping/DeliveryAddress", { productId: $.query.get("productId") });
            });
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();