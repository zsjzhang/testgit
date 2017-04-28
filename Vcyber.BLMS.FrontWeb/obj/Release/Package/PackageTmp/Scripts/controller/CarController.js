var Car = {
    alert: function () {
        popWindownBlue( 111);
    }
};

//预约试驾
Car.reserveDrive = function (carType, province, city, dealerid, dealerName, driveTime, userName, gender, mobile, planBuyTime) {

    $.ajax({
        url: "/Car/DoReserveDrive",
        type: "post",
        dataType: "json",
        data: { CarSeries: carType, DealerProvince: province, DealerCity: city, DealerId: dealerid, DealerName: dealerName, ScheduleDate: driveTime, UserName: userName, UserSex: gender, Phone: mobile, PurchaseTimeFrame: planBuyTime },
        success: function (result) {
            if (result !== null && result.code == 200) {
               // popWindownBlue("恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。", function () {
                 //   window.location = "/BlueVIP/Index";
                    
               // });

                layer.open({
                    type: 1,
                    skin: 'bluePopupCar', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    area: ['431px', '200px'],
                    shadeClose: false, //开启遮罩关闭
                    content: "恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。",
                    end: function () {
                        //window.location = "/BlueVIP/Index"
                    },
                    btn: ['确定']
                });
                $("input").val("");
                return false
            } else {
                popWindownBlue( result.msg);
                return false;
            }
        },
        error: function (err) {
        }
    });
};

//在线预订
Car.reserveBuy = function (carType, province, city, dealerId, dealerName, userName, gender, email, mobile) {
    $.ajax({
        url: "/Car/DoReserveBuy",
        type: "post",
        dataType: "json",
        data: { CarSeries: carType, DealerProvince: province, DealerCity: city, DealerId: dealerId, DealerName: dealerName, UserName: userName, UserSex: gender, Email: email, Phone: mobile },
        success: function (result) {
            if (result !== null && result.code == 200) {

                layer.open({
                    type: 1,
                    skin: 'bluePopupCar', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    area: ['431px', '200px'],
                    shadeClose: false, //开启遮罩关闭
                    content: "恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。",
                    end: function () {
                        //window.location = "/BlueVIP/Index"
                    },
                    btn: ['确定']
                });
                $("input").val("");
            } else {
                popWindownBlue( "预约失败");
                return false;
            }
        },
        error: function (err) {
        }
    });
};

//预约保养
Car.reserveMaintenance = function (carType, province, city, dealerId, dealerName, carNumber, frameNumber, ridesNumber, buyYears, maintenance, endTime, userName, gender, mobile) {
    $.ajax({
        url: "/Car/DoReserveMaintenance",
        type: "post",
        dataType: "json",
        data: { CarSeries: carType, DealerProvince: province, DealerCity: city, DealerId: dealerId, DealerName: dealerName, LicensePlate: carNumber, VIN: frameNumber, MileAge: ridesNumber, PurchaseYear: buyYears, ServiceType: maintenance, ScheduleDate: endTime, UserName: userName, UserSex: gender, Phone: mobile },
        success: function (result) {
            if (result !== null && result.code == 200) {
                layer.open({
                    type: 1,
                    skin: 'bluePopupCar', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    area: ['431px', '200px'],
                    shadeClose: false, //开启遮罩关闭
                    content: "恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。",
                    end: function () {
                        //window.location = "/BlueVIP/Index"
                    },
                    btn: ['确定']
                });
                $("input").val("");
                return false
            } else {
                popWindownBlue("您已经预约过，请耐心等待，经销商会主动与您取得联系");
                return false;
            }
        },
        error: function (err) {
        }
    });
};


