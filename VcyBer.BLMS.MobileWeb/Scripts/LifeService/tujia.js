$(function () {
    $("#to_tujia").click(function () {
        $.ajax({
            url: "/LifeService/GetTujiaMemberRule",
            data: {},
            datatype: "json",
            type: "post",
            success: function (result) {
                //未登录
                if (result == null || result.Data == 404) {
                    AlertFalse(result.Message, function () {
                        window.location.href = "/Account/Login?url=/LifeService/TujiaIndex"
                    });
                }
                else if (!result.IsSuccess) {
                    //信息错误
                    AlertFalse(result.Message);
                    return false;
                }
                else {
                    //成功领取
                    window.location.href = "https://passport.tujia.com/PortalSite/Register?eid=112068";
                }
            },
            error: function () { }
        });
    });
});
