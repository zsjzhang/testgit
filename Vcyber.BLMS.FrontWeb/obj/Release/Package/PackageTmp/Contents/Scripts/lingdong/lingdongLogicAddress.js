
Lingdong.submitAddress = function (successCallback) {

    var model = function () { };
    model.openId = Lingdong.oid;
    model.name = Lingdong.userName;
    model.phoneNumber = Lingdong.phoneNumber;
    model.address = $("#Province").val()
        + $("#City").val()
        + $("#Area").val()
        + $("#addPart4").val()
        + $("#addPart5").val();

    if ($("#Province").val() === "") { popWindownBlue( "请选择省份"); }
    if ($("#City").val() === "") { popWindownBlue( "请选择城市"); }
    if ($("#Area").val() === "") { popWindownBlue( "请选择地区"); }

    if (isnull(model.address)) {
        popWindownBlue( "请填写邮寄地址");
        return;
    }  
    var url = common.api("Lingdong/PostAddress");
     
    $.post(url

        , {
            name: model.name,
            openId: model.phoneNumber,
            phoneNumber: model.phoneNumber,
            address: model.address 
        }

        , function (data) {
            Lingdong.isAddAddress = 1;
            Lingdong.subDrive();
        }
        , "json");
};



Lingdong.queryAward = function () {

    var phoneNumber = $("#userTel").val();
    if (isnull(phoneNumber)) {
        popWindownBlue( "请填写手机号");
        return;
    }

    if (!checktel(phoneNumber)) {
        popWindownBlue( "请输入正确的手机号");
        return;
    }
    
    var url = common.api("Lingdong/GetAward");
    
    $.get(url, { phone: phoneNumber }, function (data) {
        //var json = $.parseJSON(data); 
            if (isnull(data.DrvAwardName)) {
                data.DrvAwardName = "无中奖信息";
            }

            if (isnull(data.RcmdAwardName)) {
                data.RcmdAwardName = "无中奖信息";
            }
            $("#areaDrivAward").val(data.DrvAwardName);
            $("#areaRcmdAward").val(data.RcmdAwardName);

            $("#areaDrivAward1").hide();
            $("#areaDrivAward").show();
            $("#areaRcmdAward1").hide();
            $("#areaRcmdAward").show();
        
    });

 };