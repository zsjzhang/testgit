$(function () {
    $("#btnSubscribe").click(function () {
        if ($("#UserId").val() == "") {
            AlertFalse("您尚未登陆！", function () {
                window.location.href = "/Account/login?url=/carservice/freeCar";
            });
        }
        else {
            if ($("#MLevel").val() == 12) {
                if ($("#ServicesRecordCount").val() > 0) {
                    AlertFalse("很抱歉，免费取送车服务次数已用完，无法预约", function () { });
                }
                else {
                    window.location.href = "/carService/AddFreeCar";
                }
            }
            else {
                AlertFalse("您不是金卡用户或未登录，暂不能预约免费取送车服务", function () { });
            }
        }
    })
})