$(function () {
    $("#btnSubmit").click(function () {
        $.ajax({
            url: "/LifeService/BecomeLiyueMember",
            data: {},
            datatype: "json",
            type: "post",
            success: function (result) {
                if (result == null || result.Data == null || result.Data == 404) {
                    AlertFalse(result.Message,function(){
                        window.location.href = "/Account/Login?url=/LifeService/LiyueIndex"});
                }
                else if (!result.IsSuccess) {
                    AlertFalse(result.Message);
                    return false;
                }
                else {
                    $("#div_pop").show("slow");
                    return false;
                }
            },
            error: function () { }
        });
    })
})