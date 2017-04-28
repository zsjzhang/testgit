$(function () {
    $("#add_road_rescue").on("click", function () {
        var user_id = $("#UserId").val();
        var user_type = $("#UserType").val();
        //var tel_number = $(this).attr("href");
        if (user_id != "") {
            if (user_type == "1") {
                AlertFalse('您还未认证车主', function () {
                    window.location.href = "/carService/roadRescue";
                });
            }
            else if (user_type == "2") {
                AlertFalse('您还不是服务指定车辆车主');
            }
            else if (user_type == "3") {
                window.location.href = 'Tel:400-0365-906';
            }
            else {
                AlertFalse('您还未认证车主或者不是服务指定车辆车主！');
            }
            ////记录点击
            //$.post("/carService/FreeRoadRescue", {}, function (result) {
            //    if (result.success) {
            //        AlertSuccess(result.msg, function () { });
            //    }
            //    else {
            //        AlertFalse(result.msg, function () { });
            //    }
            //});
        }
        else {
            AlertFalse('请您登陆北京现代bluemembers会员账号', function () {
                window.location.href = "/Account/login?url=/carservice/roadRescue";
            });
        }
        return false;
    });
});