function userRevevance() { }

userRevevance.ShowDiv = function () {
    $("#RelevanceRegister").css("display", "inherit");
}

userRevevance.HidenDiv = function () {
    $("#RelevanceRegister").css("display", "none");
    $(":input").filter("[type != 'button']").val("");
}

userRevevance.Save = function () {
    var mallCode = $("#mallCode").val();
    var mallUserName = $("#mallUserName").val().trim();
    var password = $("#password").val().trim();
    var checked = document.getElementById("Agree").checked;

    if (mallCode == "" || mallUserName == "" || password == "" || !checked) {
        return;
    }

    $.post("../UserRelevance/Save", { "MallCode": mallCode, "UserName": mallUserName, "Password": password }, function (data) {
        if (data.Status == 1) {
            popWindownBlue( data.Message);
            location.reload();
        } else {
            popWindownBlue( data.Message);
        }
    });
}