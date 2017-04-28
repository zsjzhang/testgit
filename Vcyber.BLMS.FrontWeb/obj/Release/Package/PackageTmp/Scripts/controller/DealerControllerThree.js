
var Dealer2 = {

};
Dealer2.city = function (province) {
    //根据省份获取城市
    $.ajax({
        url: "/Sonata/Citys",
        type: "get",
        data: { provinceValue: $(province).val() },
        dataType: "json",
        success: function (result) {
            $("#sltCity2").html("");
            $("#sltDealer2").html("<option value='-1'>请选择</option>");
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                _html += "<option>" + obj + "</option>";
            });
            //将获取的城市列表填充
            $("#sltCity2").append(_html);
        },
        error: function (err) {
            popWindownBlue( "error...");
        }
    });
};

Dealer2.list = function (city, province) {
    mapObject.ClearMap();
    mapObject.SetCity($(city).val());
    //根据城市获取供应商列表
    $.ajax({
        url: "/Sonata/Dealers",
        type: "get",
        data: { cityValue: $(city).val(), provinceValue: $(province).val() },
        dataType: "json",
        success: function (data) {
            mapObject.Object.setFitView();
        },
        error: function (err) {
            popWindownBlue( "error");
        }
    });
};