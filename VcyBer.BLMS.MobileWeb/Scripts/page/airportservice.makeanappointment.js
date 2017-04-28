var AirPortServiceMakeAnAppointment = function () {
    function initValidator() {
        $.validator.setDefaults({

            submitHandler: submitForm,
            ignore: ""
        });
        $("#mainForm").validate({
            rules: {
                airAgree: { required: true },
                hiduseAirNum: { required: true, range: [1, 100] },
                Terminal: { required: true },
                phoneNumber: { required: true, mobileCN: true }
            },
            messages: {
                airAgree: { required: "请选择在预约后3个月内使用" },
                hiduseAirNum: { required: "请选择预约候机服务次数", range: "预约候机服务次数错误" },
                Terminal: { required: "请选择航站楼" },
                phoneNumber: { required: "请填写手机号码" }
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
        $("#AirProvince").change(function () {
            $("#AirPort option[value!='']").remove();
            $("#Terminal option[value!='']").remove();
            bindAirPort(this.value);
        });
        $("#AirPort").change(function () {
            $("#Terminal option[value!='']").remove();
            bindTerminal(this.value);
        });
        $("#Terminal").change(function () {
            $("#TerminalName").val($(this).find("option:selected").val());
        });

        $("#addFreeNum").click(function () {
            iptFreeNum = Number($("#iptFreeNum").val());
            useAirNum = Number($("#useAirNum").html());
            if (freeNum > iptFreeNum) {
                iptFreeNum = iptFreeNum + 1
                $("#iptFreeNum").val(iptFreeNum);
                $("#useFreeNum").html(iptFreeNum);
                $("#useAirNum").html(useAirNum + 1);
                $("#hiduseAirNum").val(useAirNum + 1);
            }
        });
        $("#lessFreeNum").click(function () {
            iptFreeNum = Number($("#iptFreeNum").val());
            useAirNum = Number($("#useAirNum").html());
            if (iptFreeNum > 0) {
                iptFreeNum = iptFreeNum - 1
                $("#iptFreeNum").val(iptFreeNum);
                $("#useFreeNum").html(iptFreeNum);
                $("#useAirNum").html(useAirNum - 1);
                $("#hiduseAirNum").val(useAirNum - 1);
            }
            //iptFreeNum
            //useFreeNum
            //useAirNum
        });
        $("#addIntegralNum").click(function () {
            iptIntegralNum = Number($("#iptIntegralNum").val());
            useAirNum = Number($("#useAirNum").html());
            if (integralNum > iptIntegralNum) {
                iptIntegralNum = iptIntegralNum + 1
                $("#iptIntegralNum").val(iptIntegralNum);
                $("#useIntegra").html(iptIntegralNum * 1800);
                $("#useAirNum").html(useAirNum + 1);
                $("#hiduseAirNum").val(useAirNum + 1);
            }
            //iptIntegralNum
            //useIntegra
            //useAirNum
        });
        $("#lessIntegralNum").click(function () {
            iptIntegralNum = Number($("#iptIntegralNum").val());
            useAirNum = Number($("#useAirNum").html());
            if (iptIntegralNum > 0) {
                iptIntegralNum = iptIntegralNum - 1
                $("#iptIntegralNum").val(iptIntegralNum);
                $("#useIntegra").html(iptIntegralNum * 1800);
                $("#useAirNum").html(useAirNum - 1);
                $("#hiduseAirNum").val(useAirNum - 1);
            }
            //iptIntegralNum
            //useIntegra
            //useAirNum
        });
    });

    function bindAirPort(province) {
        common.post(common.api("PageApi/AirPortByProvince"), { province: province }, function (result) {
            $.each(result, function (index, item) {
                $("#AirPort").append(common.format("<option value=\"{0}\">{0}</option>", item.AirportName));
            });
        });
    }

    function bindTerminal(airPortName) {
        common.post(common.api("PageApi/TerminalByAirName"), { airPortName: airPortName }, function (result) {
            if (result) {
                $.each(result, function (index, item) {
                    $("#Terminal").append(common.format("<option value=\"{0}\">{1}</option>", item.id, item.AirportAllName));
                });
            }
        });
    }

    function submitForm() {
        //var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("AirPortService/AddMake"), { phoneNumber: $("#phoneNumber").val(), freeNum: $("#iptFreeNum").val(), hiduseAirNum: $("#iptIntegralNum").val(), airPortId: $("#TerminalName").val() }, function (data, msg) {

            location.href = common.api("/AirPortService/ServiceVoucherSuccess", { snid: data });

        }, function (error) {
            $("#errorContainer").html(error);
        });
    }
    var $overlay = $("#overlay");
    function msgShow(msg) {
        $overlay.addClass("active").children().addClass("modal-in");
        $("#ajaxmsg").html(msg);
    }

    $(".close").click(function () {
        $overlay.removeClass("active").children().removeClass("modal-in");
    });

}();