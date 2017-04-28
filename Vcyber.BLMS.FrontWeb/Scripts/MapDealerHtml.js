
function OODealerMap() { }

OODealerMap.Close = function () {
    $("#dealerDiv").hide();
}

OODealerMap.Open = function () {
    $("#dealerDiv").show();
}

OODealerMap.SelectDealer = function (instance) {

    OODealerMap.Close();
    var shengName = $(instance).attr("shengName");
    var cityName = $(instance).attr("cityName");
    var dealerName = $(instance).attr("dealerName");

    $("#sltProvince").children().each(function () {
        if ($(this).text() == shengName) {
            $(this).attr("selected", "selected");
        }
    });

    $.ajax({
        url: "/Car/Citys",
        type: "get",
        data: { provinceValue: document.getElementById('sltProvince').value },
        dataType: "json",
        success: function (result) {
            $("#sltCity").html("");
            $("#sltDealer").html("<option value='-1'>请选择</option>");
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                _html += "<option>" + obj + "</option>";
            });
            //将获取的城市列表填充
            $("#sltCity").append(_html);

            $("#sltCity").children().each(function () {
                if ($(this).text() == cityName) {
                    $(this).attr("selected", "selected");
                    return false;
                }
            });

            $.ajax({
                url: "/Car/Dealers",
                type: "get",
                data: { cityValue: document.getElementById('sltCity').value, provinceValue: document.getElementById('sltProvince').value },
                dataType: "json",
                success: function (result) {
                    $("#sltDealer").html("");
                    //将获取的供应商列表填充
                    var _html = "<option value='-1'>请选择</option>";
                    $(result).each(function (i, obj) {
                        _html += "<option value='" + obj.DealerId + "'>" + obj.Name + "</option>";
                    });
                    //将获取的城市列表填充
                    $("#sltDealer").append(_html);

                    $("#sltDealer").children().each(function () {
                        if ($(this).text() == dealerName) {
                            $(this).attr("selected", "selected");
                        }
                    });
                },
                error: function (err) {
                    popWindownBlue( "error");
                }
            });
        },
        error: function (err) {
            popWindownBlue( "error...");
        }
    });
}