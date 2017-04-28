var News = {
    alert: function () {
        popWindownBlue( "news");
    }
};

//提交纸质杂志申请
News.magazineApply = function (userName, mobile, postcode, carType, carTypeName) {
    $.ajax({
        url: "/News/MagazineApply",
        type: "post",
        dataType: "json",
        data: { ReceiveName: userName, Phone: mobile, ZipCode: postcode, Detail: carTypeName },
        success: function (result) {
            if (result !== null && result.code == 200) {
                popWindownBlue( "恭喜您，您的申请已成功提交。");
                $("#homeReserveDriveUserName").val("");
                $("#homeReserveDriveUserMobile").val("");
                $("#homeReserveDriveUserPostCode").val("");
                $("#carType").children().first().before("<option value='-1'>请选择</option>");
                $("#carType").val("-1");
                return false;
            } else {
                popWindownBlue( result.msg);
                return false;
            }
        },
        error: function (err) {
        }
    });
};