 
Lingdong.bindCity = function (province) {
    $.post("/Dealer/Citys", { provinceValue: province }, function (result) {
        $.each(result, function (index, item) {
            $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", item));
        });
    });
};

Lingdong.bindDealer = function (province, city) {
    $.post("/Dealer/Dealers", { provinceValue: province, cityValue: city }, function (result) {
        if (result) {
            $.each(result, function (index, item) {
                $("#DealerId").append(common.format("<option value=\"{0}\">{1}</option>", item.DealerId, item.Name));
            });
        }
    });
};


Lingdong.initDriveForm = function () { 
    $("#DealerProvince").change(function () {
        $("#DealerCity option[value!='']").remove();
        $("#DealerId option[value!='']").remove();
        Lingdong.bindCity(this.value);
    });
    $("#DealerCity").change(function () {
        $("#DealerId option[value!='']").remove();
        Lingdong.bindDealer($("#DealerProvince").val(), this.value);
    });
    $("#DealerId").change(function () {
        $("#DealerName").val($(this).find("option:selected").text());
    });
};


Lingdong.submitDriveForm = function (successCallback) {

    var model = function (){};
    model.OpenId = Lingdong.oid;
    model.CarSeries = $("#CarSeries").val();
    if (isnull(model.CarSeries)) {
        popWindownBlue( "请选择车型");
        return;
    }
    model.DealerId = $("#DealerId").val();
    if (isnull(model.DealerId)) {
        popWindownBlue( "请选择经销商");
        return;
    }
    model.ScheduleDate = $("#ScheduleDate").val();
    if (isnull(model.ScheduleDate)) {
        popWindownBlue( "请输入预计到店时间");
        return;
    }
    
    var ds = (new Date() - new Date(model.ScheduleDate)) / 1000 / 60 / 60 / 24;
    if (ds > 1) {
        popWindownBlue( "请输入正确的到店时间");
        return;
    }

    model.UserName = $("#UserName").val();
    if (isnull(model.UserName)) {
        popWindownBlue( "请输入姓名");
        return;
    }
    model.Phone = $("#Phone1").val();
    if (isnull(model.Phone)) {
        popWindownBlue( "请输入手机号");
        return;
    }
    if (!checktel(model.Phone)) {
        popWindownBlue( "请输入正确的手机号");
        return;
    }
    model.ValiateCode = $("#validateCode1").val();
    if (isnull(model.ValiateCode)) {
        popWindownBlue( "请输入短信验证码");
        return;
    }

    model.DealerCity = $("#DealerCity").val();
    model.DealerProvince = $("#DealerProvince").val();
    model.DealerName = $("#DealerName").val();
    model.UserSex = $("#sex1").checked ? 1 : 0; 

    var url = common.api("Lingdong/TestDrive4Wx");

    $.post(url
    
        , {carSeries: model.CarSeries,
        openId: model.OpenId,
        dealerId: model.DealerId,
        scheduleDate: model.ScheduleDate,
        userName: model.UserName,
        phone: model.Phone,
        dealerCity: model.DealerCity,
        dealerProvince: model.DealerProvince,
        dealerName: model.DealerName,
        userSex: model.UserSex,
        valiateCode:model.ValiateCode 
        }
    
        ,function (data) {

            if (successCallback) {
                successCallback(data);
            }
        }
        , "json");
     

    Lingdong.userName = model.userName;
    Lingdong.phoneNumber = model.Phone;
};