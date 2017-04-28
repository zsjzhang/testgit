function user() {

}

user.getInfo = function () {
    $.post("../User/Info", null, function (data) {
        $("#realName").text(data.RealName);
        $("#birthTime").val(data.BirthTime);

        $(":radio").each(function (index, element) {
            if ($(this).val() == data.Sex) {
                $(this).attr("checked", "checked");
            }
        });

        $(":checkbox").each(function (index, element) {
            var value = $(this).val();
            var tempValue = data.Hobby.match(value)

            if (value == tempValue) {
                $(this).attr("checked", "checked");
            }
        });
    });
}

user.saveInfo = function () {
    var hobby = "";
    var sex = 0;
    var birthTime = $("#birthTime").val().trim();

    $(":checked").filter(":checkbox").each(function (index, element) {
        if (hobby == "") {
            hobby = $(this).val();
        } else {
            hobby += ";" + $(this).val();
        }
    });

    $(":checked").filter(":radio").each(function (index, element) {
        sex = $(this).val();
    });

    if (birthTime == null || sex == 0 || hobby == "") {
        return;
    }

    $.post("../User/SaveInfo", { "BirthTime": birthTime, "Sex": sex, "Hobby": hobby }, function (data) {
        if (data.Status == 1) {
            popWindownBlue( data.Message);
        } else {
            popWindownBlue( data.Message);
        }
    });
}