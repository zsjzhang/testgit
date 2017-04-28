var SonataAddOneOnOneScripts = function () {
    var mapObj;
    var city;
    var province;
    var ConsultantList;
    WeChat.readyCallback = function (obj, wx) {
        obj.getLocation(function (res) {
            var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
            var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
            var speed = res.speed; // 速度，以米/每秒计
            var accuracy = res.accuracy; // 位置精度
            //alert(common.format("经度：{0}，纬度：{1}，速度：{2}，位置经度：{3}", latitude, longitude, speed, accuracy));

            AMap.service(["AMap.Geocoder"], function () { //加载地理编码
                geocoder = new AMap.Geocoder({
                    radius: 3000,
                    extensions: "base"
                });
                //步骤三：通过服务对应的方法回调服务返回结果，本例中通过逆地理编码方法getAddress回调结果
                geocoder.getAddress(new AMap.LngLat(longitude, latitude), function (status, result) {
                    //根据服务请求状态处理返回结果
                    if (status == 'error') {
                        mobileAlert("服务请求出错啦！ ");
                    }
                    if (status == 'no_data') {
                        mobileAlert("无数据返回，请换个关键字试试～～");
                    }
                    else {
                        //alert(JSON.stringify());
                        province = result.regeocode.addressComponent.province;
                        if (result.regeocode.addressComponent.city == "") {
                            city = result.regeocode.addressComponent.province;
                        }
                        else {
                            city = result.regeocode.addressComponent.city;
                        }
                        $("#DealerProvince").attr("value", province);
                        bindCity(province);

                        bindDealer(province, city);
                    }
                });
            });
        });
    }


    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitForm
        });
        $("#mainForm").validate({
            rules: {
                CarSeries: { required: true },
                DealerProvince: { required: true },
                DealerCity: { required: true },
                DealerId: { required: true },
                //ConsultantId: { required: true },
                LicensePlate: { required: true },
                ScheduleDate: { required: true },
                UserName: { required: true },
                UserSex: { required: true },
                Phone: { required: true, mobileCN: true }
                //Comment: { required: true }
            },
            messages: {
                CarSeries: { required: "请选择车型" },
                DealerProvince: { required: "请选择经销商所在省" },
                DealerCity: { required: "请选择经销商所在市" },
                DealerId: { required: "请选择经销商" },
                //ConsultantId: { required: "请选择服务顾问" },
                LicensePlate: { required: "请选择车牌号码" },
                ScheduleDate: { required: "请输入预计到店时间" },
                UserName: { required: "请输入姓名" },
                UserSex: { required: "请选择性别" },
                Phone: { required: "请输入手机号", mobileCN: "请输入正确手机号码" }
                //Comment: { required: "请输入补充内容" }
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
        $("#DealerProvince").change(function () {
            $("#DealerCity option[value!='']").remove();
            $("#DealerId option[value!='']").remove();
            $("#ConsultantId option[value!='']").remove();
            bindCity(this.value);
        });
        $("#DealerCity").change(function () {
            $("#DealerId option[value!='']").remove();
            $("#DealerName").val();
            $("#ConsultantId option[value!='']").remove();
            bindDealer($("#DealerProvince").val(), this.value);
        });
        $("#DealerId").change(function () {
            $("#ConsultantId option[value!='']").remove();
            $("#ConsultantName").val();
            $("#DealerName").val($(this).find("option:selected").text());
            bindConsultant(this.value);
        });
        $("#ConsultantId").change(function () {
            $("#ConsultantName").val($(this).find("option:selected").text());
        });
        $("#ConsultantInfo").click(function () {
            ConsultantInfo();
        });
    });
    //城市
    function bindCity(province) {
        common.post(common.api("PageApi/City"), { province: province }, function (result) {
            $.each(result, function (index, item) {
                $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", item.value));
            });
            $("#DealerCity").attr("value", city);
        });
    }
    //经销商
    function bindDealer(province, city) {
        common.post(common.api("PageApi/Dealer"), { province: province, city: city }, function (result) {
            if (result) {
                $.each(result, function (index, item) {
                    $("#DealerId").append(common.format("<option value=\"{0}\">{1}</option>", item.DealerId, item.Name));
                });
            }
        });
    }
    //顾问
    function bindConsultant(dealerId) {
        common.post(common.api("PageApi/Consultant"), { dealerId: dealerId }, function (result) {
            ConsultantList = result;
            if (result) {
                $.each(result, function (index, item) {
                    $("#ConsultantId").append(common.format("<option value=\"{0}\">{1}</option>", item.Id, item.Name));
                });
            }
        });
    }
    var consultantHtmlTemp = "<div style=\"width:95%;\"><div style=\"width:60%;float: left;\"><img src=\"{img}\"></div><div style=\"width:40%;float: right;text-align:left\"><p>职位：{title}</p><p>性别：{sex}</p><p>姓名：{name}</p>年龄：{age}<p>介绍：{comment}</p></div></div>";

    function ConsultantInfo() {
        var consultantId = $("#ConsultantId").val();
        if (consultantId) {

            if (ConsultantList) {
                $.each(ConsultantList, function (index, item) {

                    if (item.Id == consultantId) {

                        var HtmlTemp = consultantHtmlTemp;
                        HtmlTemp = HtmlTemp.replace(/{img}/g, item.Photo);
                        HtmlTemp = HtmlTemp.replace(/{title}/g, item.Title == null ? "" : item.Title);
                        HtmlTemp = HtmlTemp.replace(/{name}/g, item.Name);
                        HtmlTemp = HtmlTemp.replace(/{sex}/g, item.Sex == 0 ? "女" : "男");
                        HtmlTemp = HtmlTemp.replace(/{age}/g, item.Age == null ? "" : item.Age);
                        HtmlTemp = HtmlTemp.replace(/{comment}/g, item.Comment);
                        mobileAlert(HtmlTemp);
                    }
                });
            }

        }
        else {
            mobileAlert("您还未选择服务顾问");
        }
    }

    function submitForm() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("Sonata/APIAddOneOnOne"), requestData, function (result) {
            mobileAlert("预约成功", function () {
                window.location.href = common.resolveUrl("/UserCenter/Index");
            });
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
}();