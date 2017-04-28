function pjyz() {

    var pjyzcode = $("#peijiancode").val();
    if (pjyzcode == null || pjyzcode == "") {
        popWindownBlue( "请输入配件号");
        return false;
    }
    var phoneReg = new RegExp("^(13|14|15|17|18)[0-9]{9}$");
    if (!phoneReg.test(pjyzcode)) {
        popWindownBlue( "请正确输入防伪号");
        $("#testDriveUserTel").val("");
        return false;
    }

    $.ajax({
        url: "/Car/DoFittingValidate",
        type: "post",
        dataType: "json",
        data: { code: pjyzcode, address: "", Longitude: 0, Latitude: 0, Altitude: 0, ctype: 'bmsite' },
        success: function (result) {
            if (result !== null && result.code == 200) {
                if (result.data == "1") {
                    $(".czpj_res").css("display", "none");
                    $("#firstresult").css("display", "block");
                }
                if (result.data == "2") {
                    $(".czpj_res").css("display", "none");
                    $("#secondresult").css("display", "block");
                }
                if (result.data == "3") {
                    $(".czpj_res").css("display", "none");
                    $("#threeresult").css("display", "block");
                }


            } else {
                popWindownBlue( result.msg);
                return false;
            }
        },
        error: function (err) {
        }
    });



}