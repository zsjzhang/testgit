﻿var Dealer = {

};
Dealer.city = function (province) {
    //根据省份获取城市
    $.ajax({
        url: "/DealerShip/Citys",
        type: "get",
        data: { provinceValue: $(province).val() },
        dataType: "json",
        success: function (result) {
            $("#sltCity").html("");
            $("#sltDealer").html("<option value='-1'>请选择</option>");
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                var cityName = $("#dsCity").val();
                var select = cityName == obj ? "selected='selected'" : "";
                _html += "<option "+select+" >" + obj + "</option>";
            });
            //将获取的城市列表填充
            $("#sltCity").append(_html);
        },
        error: function (err) {
            alert("error...");
        }
    });
};

Dealer.list = function (city, province, IsWeibao, Istestserver, IsDingChe) {
    //根据城市获取供应商列表
    $.ajax({
        url: "/DealerShip/Dealers",
        type: "get",
        data: { cityValue: $(city).val(), provinceValue: $(province).val(), IsWeibao: IsWeibao, Istestserver: Istestserver, IsDingChe: IsDingChe },
        dataType: "json",
        success: function (result) {
            $("#sltDealer").html("");
            //将获取的供应商列表填充
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                
                _html += "<option value='" + obj.DealerId + "'" + obj.Name + "</option>";
            });
            //将获取的城市列表填充
            $("#sltDealer").append(_html);
        },
        error: function (err) {
            alert("error");
        }
    });
};
Dealer.cityForReserve = function (province, city_html_id, dealer_html_id) {
    //根据省份获取城市
    $.ajax({
        url: "/DealerShip/Citys",
        type: "get",
        data: { provinceValue: $(province).val() },
        dataType: "json",
        success: function (result) {
            $("#" + city_html_id).html("");
            $("#" + dealer_html_id).html("<option value='-1'>请选择</option>");
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                _html += "<option>" + obj + "</option>";
            });
            //将获取的城市列表填充
            $("#" + city_html_id).append(_html);
        },
        error: function (err) {
            alert("error...");
        }
    });
};
Dealer.listForReserve = function (city, province, dealer_html_id) {
    //根据城市获取供应商列表
    $.ajax({
        url: "/DealerShip/Dealers",
        type: "get",
        data: { cityValue: $(city).val(), provinceValue: $(province).val() },
        dataType: "json",
        success: function (result) {
            $("#" + dealer_html_id).html("");
            //将获取的供应商列表填充
            var _html = "<option value='-1'>请选择</option>";
            $(result).each(function (i, obj) {
                _html += "<option value='" + obj.DealerId + "'>" + obj.Name + "</option>";
            });
            //将获取的城市列表填充
            $("#" + dealer_html_id).append(_html);
        },
        error: function (err) {
            alert("error");
        }
    });
};

Dealer.changeCity = function (city, province) {
    //根据城市获取供应商列表
    $.ajax({
        url: "/DealerShip/Dealers",
        type: "get",
        data: { cityValue: $(city).val(), provinceValue: $(province).val() },
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
        },
        error: function (err) {
            alert("error");
        }
    });
};