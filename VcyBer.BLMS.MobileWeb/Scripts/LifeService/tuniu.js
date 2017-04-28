
$(function () {
    //领取权益礼包
    $("#btn_servicecode").click(function () {
        $.ajax({
            url: "/LifeService/GetGiftBag?t=" + (new Date()).getTime(),
            type: "get",
            success: function (result) {
                if (result.code == null || result.code == 404) {
                    AlertFalse(result.msg, function () {
                        window.location.href = "/Account/Login?url=" + result.toUrl
                    });
                }
                else if (result.code == 401) {
                    AlertFalse(result.msg);
                    return false;
                }
                else if (result.code == 201) {
                    AlertFalse(result.msg + ":" + result.data.ServiceCode);
                    return false;
                }
                else if (result.code == 200) {
                    $("#span_serviceCode").val(result.data.ServiceCode );
                    $("#div_pop").show("slow");
                    return false;
                }
                else {
                    AlertFalse(result.msg);
                    return false;
                }
            },
            error: function () {

            }
        });
    })
})