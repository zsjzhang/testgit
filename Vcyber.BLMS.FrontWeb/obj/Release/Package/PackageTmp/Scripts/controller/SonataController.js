var Sonata = {};
Sonata.freeMotFormSave = function (carType, province, city, dealer, carNumber, vinNumber, ridesNumber, buyYears, arriveTime, realName, gender, mobile, description, source) {
    $.ajax({
        url: "/Sonata/FreeMotFormSave",
        type: "post",
        data: {
            CarSeries: carType,
            DealerProvince: province,
            DealerCity: city,
            DealerId: dealer,
            LicensePlate: carNumber,
            VIN: vinNumber,
            MileAge: ridesNumber,
            PurchaseYear: buyYears,
            ScheduleDate: arriveTime,
            UserName: realName,
            UserSex: gender,
            Phone: mobile,
            Comment: description,
            DataSource : source
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                popWindownBlue(result.message, function () {
                    window.location = "/Sonata/Index";
                   
                });
                return false;
            } else {
                popWindownBlue( result.message);
                return false;
            }
        },
        error: function (err) {
            //go to error page
            popWindownBlue( result.message);
        }
    });
};

Sonata.travelFormSave = function (carType, province, city, dealer, carNumber, vinNumber, ridesNumber, buyYears, arriveTime, realName, gender, mobile, description, source) {
    $.ajax({
        url: "/Sonata/DoTravel",
        type: "post",
        data: {
            CarSeries: carType,
            DealerProvince: province,
            DealerCity: city,
            DealerId: dealer,
            LicensePlate: carNumber,
            VIN: vinNumber,
            MileAge: ridesNumber,
            PurchaseYear: buyYears,
            ScheduleDate: arriveTime,
            UserName: realName,
            UserSex: gender,
            Phone: mobile,
            Comment: description,
            DataSource: source
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                popWindownBlue(result.message, function () {
                    window.location = "/Sonata/Index";
                });
                
                return false;
            } else {
                popWindownBlue( result.message);
                return false;
            }
        },
        error: function (err) {
            //go to error page
            popWindownBlue( result.message);
        }
    });
};

Sonata.serviceToHomeFormSave = function (dealerId, getPosition,
    takeLong, taktLat, returnPostion, returnLong, returnLat, carType, carNumber, vinNumber, takeTime, sendTime, realName, gender, mobile, description, source) {
    $.ajax({
        url: "/Sonata/ServiceToHomeFormSave",
        type: "post",
        data: {
            TakeAddress: getPosition,
            TakeLong: takeLong,
            TakeLat: taktLat,
            ReturnAddress: returnPostion,
            ReturnLong: returnLong,
            ReturnLat: returnLat,
            DealerId: dealerId,
            CarSeries: carType,
            LicensePlate: carNumber,
            VIN: vinNumber,
            ScheduleDate: takeTime,
            ReturnDate: sendTime,
            UserName: realName,
            UserSex: gender,
            Phone: mobile,
            Comment: description,
            DataSource: source
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {

                layer.open({
                    type: 1,
                    skin: 'bluePopupCar_0', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    shadeClose: false, //开启遮罩关闭
                    content: result.message,
                    area: ['431px', '220px'],
                    end: function () {
                        window.location = "/Sonata/Index";
                    },
                    btn: ['确定']
                });
                return false;
            } else {
                layer.open({
                    type: 1,
                    skin: 'bluePopupCar', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    shadeClose: false, //开启遮罩关闭
                    content: result.message,
                    area: ['431px', '200px'],
                    end: function () {
                        //window.location = "/MyCenter/Index?pageName=MyCenter";
                    },
                    btn: ['确定']
                });
                return false;
            }
        },
        error: function (err) {
            window.location.href = "/Sonata/Fail/2";
        }
    });
};

Sonata.goHomeFormSave = function (takeAddress, dealerId, carType, licensePlate, vin, scheduleDate, userName, userSex, phone, comment, city, provinceName, source, takeLong, takeLat) {

    $.ajax({
        url: "/Sonata/GoHomeFormSave",
        type: "post",
        data: {
            TakeAddress: takeAddress,
            DealerId: dealerId,
            CarSeries: carType,
            LicensePlate: licensePlate,
            VIN: vin,
            ScheduleDate: scheduleDate,
            UserName: userName,
            UserSex: userSex,
            Phone: phone,
            Comment: comment,
            DealerCity: city,
            DealerProvince: provinceName,
            TakeLong: takeLong,
            TakeLat: takeLat,
            DataSource : source
        },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                popWindownBlue(result.message, function () {
                    window.location = "/Sonata/Index";
                   
                });
                return false;
            } else {
                popWindownBlue( result.message);
                return false;
            }
        },
        error: function (err) {
            window.location.href = "/Sonata/Fail/0";
        }
    });
};

//向特约店申请
Sonata.applyMemberToDealerSave = function () {
    $.ajax({
        url: "/PayFee/ActivityApply",
        type: "post",
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( "尊敬的客户，非常抱歉，您的身份证号未能匹配到车辆，无法进行激活，如有疑问请电话联系我们的工作人员，谢谢！");
                return false;
            }
            if (result.code == "401") {
                popWindownBlue("尊敬的客户，您的账号登录异常，请重新登陆进行激活！", function () {
                    window.location = "/Account/LogonPage?returnUrl=/Sonata/SonataActive";
                   
                });
                return false;
            }
            if (result !== null && result.code == "200") {
                var _resultHtml = '<div  style="background: url(/img/car_tanchuang2.png) no-repeat; width:413px; height:217px; position: fixed;  left: 62%; margin-left: -206px; "><dl style="width: 294px;margin: 0 auto;margin-top: 34px;"><dt style="float: left;margin-right: 10px;height: 70px;"><img src="/img/zhuce_dianhua.png"></dt><dd><h3>尊敬的车主您好， 恭喜您您的交费申请已提交成功，审核结果会在5个工作日内通知您，请您耐心等待。</h3></dd></dl><a   style="text-align: center; width: 230px; height: 30px; line-height: 30px; color: #FFFFFF; font-size: 14px; background: #075090; border: 1px solid #075090;display: block;margin: 0 auto;margin-top: 28px;" href="javascript:void(0);" onclick="Sonata.ApplyMemberToDealerLayerIndexClose();" >我知道了 <input type="hidden" id="ApplyMemberToDealerLayerIndex"></a></div>';
                var layerIndex= layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['413x', '217x'],
                    contet:_resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
                    
                });

                $("#ApplyMemberToDealerLayerIndex").val(layerIndex);
                window.location.href = "/Mycenter/Index";
            }
        },
        error: function () {
            popWindownBlue( "error");
        }
    });
};

///提交天猫支付码以激活会员
Sonata.InputPayNumberToActiveSave = function (payNo) {
    if (payNo === null || payNo === "" || payNo.length > 50) {
        popWindownBlue( "请正确输入支付码");
        return false;
    }
    $.ajax({
        url: "/PayFee/InputPayNumberToActiveSave",
        type: "post",
        dataType: "json",
        data: { payNumber: payNo },
        success: function (result) {
            if (result !== null && result.code == "200") {
                layer.close(document.getElementById('SonataActiveCancelLayerIndex').value);

                var _resultHtml = '<div  style="background: url(/img/car_tanchuang2.png) no-repeat; width:413px; height:217px; position: fixed;  left: 62%; margin-left: -206px; "><dl style="width: 294px;margin: 0 auto;margin-top: 34px;"><dt style="float: left;margin-right: 10px;height: 70px;"><img src="/img/zhuce_dianhua.png"></dt><dd><h3>尊敬的第九代索纳塔车主您好， 恭喜您已成功注册，您的银卡会员申请已提交，审核结果会在5个工作日内通知您，请您耐心等待。</h3></dd></dl><a   style="text-align: center; width: 230px; height: 30px; line-height: 30px; color: #FFFFFF; font-size: 14px; background: #075090; border: 1px solid #075090;display: block;margin: 0 auto;margin-top: 28px;" href="javascript:void(0);" onclick="Sonata.ApplyMemberToDealerLayerIndexClose();" >我知道了 <input type="hidden" id="ApplyMemberToDealerLayerIndex"></a></div>';
                var   layerIndex=  layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['413x', '217x'],
                    content: _resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
                    
                });

                $("#ApplyMemberToDealerLayerIndex").val(layerIndex);
                window.location.href = "/Mycenter/Index";
            }
        },
        error: function () {
            popWindownBlue( "error");
        }
    });
};

Sonata.ApplyMemberToDealerLayerIndexClose = function () {
    layer.close($("#ApplyMemberToDealerLayerIndex").val());
};