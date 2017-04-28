$(function () {
    Lingdong.i = 1;
    $("#btn-rec-ad").click(function () {
        var nameTemp = '<div class="commonrow"><span class="commontitle">姓&emsp;&emsp;名：</span><input type="text" class="commoninput" id="rec-name-' + Lingdong.i + '" /></div>';
        var phoneTemp = '<div class="commonrow"><span class="commontitle">手机号码：</span><input type="text" class="commoninput" id="rec-phone-' + Lingdong.i + '" /></div>';

        $("#new-rec-container").append(nameTemp).append(phoneTemp).append("<br/>");
        Lingdong.i++;
    });

});

Lingdong.submitRcmdForm = function (successCallback) {
    //popWindownBlue( $("#new-rec-container div.commonrow").length);
    var url = common.api("Lingdong/Recommend");
    var recCount = $("#new-rec-container div.commonrow").length / 2;
    //var jsonStr="[";
    //for (var j = 0; j < recCount; j++) {
    //    var name = $("#rec-name-" + j).val();
    //    var phone = $("#rec-phone-" + j).val();
    //    jsonStr += '{"OpenId":' + Lingdong.oid + ',"Name":' + name + ',"PhoneNumber":' + phone + '},';
    //}
    //jsonStr = jsonStr.substr(0, jsonStr.length - 1);
    //jsonStr += "]";
    var phone_me = $("#phone_me").val();

    var name1 = $("#rec-name-0").val();
    var phone1 = $("#rec-phone-0").val();

    var name2 = $("#rec-name-1").val();
    var phone2 = $("#rec-phone-1").val();

    var name3 = $("#rec-name-2").val();
    var phone3 = $("#rec-phone-2").val();

    $.post(url, { "OpenId": phone_me, Name1: name1, PhoneNumber1: phone1, Name2: name2, PhoneNumber2: phone2, Name3: name3, PhoneNumber3: phone3 }, function (data) {

        if (data == null || data == "" || isnull(data.Name)) {
            Lingdong.isRecommandAward = 0;
        } else {
            Lingdong.isRecommandAward = 1;
            Lingdong.awardNameOfRecommand = data.Name;
            Lingdong.awardTypeOfRecommand = data.Type;
        }

        Lingdong.isRecommand = 1;
        Lingdong.recommand();
    });
};