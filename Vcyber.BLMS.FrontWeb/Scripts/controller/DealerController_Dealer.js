var Dealer4 = {

};
Dealer4.city = function (province) {
    $("#maptip").css("display", "none");
    $("#maptip").html();
    //根据省份获取城市
    $.ajax({
        url: "/Dealer/Citys",
        type: "get",
        data: { provinceValue: $(province).val() },
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
        },
        error: function (err) {
            popWindownBlue( "error...");
        }
    });
};

Dealer4.list = function (city, province) {
    $("#dealer-city").text($(city).val());
    //根据城市获取供应商列表
    $.ajax({
        url: "/Dealer/Dealers",
        type: "get",
        data: { cityValue: $(city).val(), provinceValue: $(province).val(),IsWeibao:0,Istestserver:0,IsDingChe:0 },
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
            Dealer4.fillDealers(result);
            //切换地图
            mapObject.map.setCity($("#dealer-city").text());
        },
        error: function (err) {
            popWindownBlue( "error");
        }
    });
};


Dealer4.fillDealers = function (data) {
    $("#dealerlist").html('');
    mapObject.markers = [];
    $.each(data, function (index, e) {
        var li = $('<li' + (index % 2 === 0 ? ' class="have_mr"' : '') + ' ></li>');
        li.append('<div style="float:left;width:100%;"><h3>' + e.Name + '</h3></div>');
        li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/weizhi.png" />位<b></b>置：</div><div style="float:right; width:205px;">' + e.Address + '</div></div>');
        li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/dianhua.png" />咨询电话：</div><div style="float:right; width:205px;word-wrap:break-word;">' + (e.Phone === null ? "" : e.Phone) + '<div></div>');
        li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/dianhua.png" />售后电话：</div><div style="float:right; width:205px;">' + (e.AfterSalesPhone === null ? "" : e.AfterSalesPhone) + '</div></div>');
        $("#dealerlist").append(li);

        var imgIndex = (index + 1) % 10; //index>8?0:(index+1);
        if (e.Position !== null && e.Position.indexOf(",") >= 0) {
            var marker = new AMap.Marker({
                icon: "https://webapi.amap.com/images/0.png",
                position: new AMap.LngLat(e.X, e.Y),
                title: e.Name,
                extData: li.clone()
            });
            marker.position = new AMap.LngLat(e.X, e.Y);
            mapObject.markers[e.DealerId] = marker;
            marker.setMap(mapObject.map);
            AMap.event.addListener(marker, 'click', function () {
                mapObject.OpenWidow(e.Name, e.Address, (e.Phone === null ? "" : e.Phone), (e.AfterSalesPhone === null ? "" : e.AfterSalesPhone)).open(mapObject.map, marker.getPosition());
            });
        }
    });
    mapObject.map.setFitView();
};

Dealer4.dealerChange = function (obj) {
    var dealerId = $("#sltDealer").val();
    $("#maptip").html("");
    if (mapObject.markers[dealerId] !== null) {       
        mapObject.map.setZoomAndCenter(16, mapObject.markers[dealerId].position);
        //$("#maptip").append(mapObject.markers[dealerId].Rc.extData);
        //mapObject.markers[dealerId].setContent($("#maptip").html());
        $.ajax({
            url: "/Dealer/GetDealer",
            type: "get",
            data: { dealerId: dealerId },
            dataType: "json",
            success: function (result) {
                //将获取的供应商列表填充
                var _html = "<option value='-1'>请选择</option>";
                $(result).each(function (i, obj) {
                    _html += "<option value='" + obj.DealerId + "'>" + obj.Name + "</option>";
                });

                var li = $('<li class="have_mr"></li>');
                li.append('<div style="float:left;width:100%;"><h3>' + result.Name + '</h3></div>');
                li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/weizhi.png" />位<b></b>置：</div><div style="float:right; width:205px;">' + result.Address + '</div></div>');
                li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/dianhua.png" />咨询电话：</div><div style="float:right; width:205px;word-wrap:break-word;">' + (result.Phone === null ? "" : result.Phone) + '<div></div>');
                li.append('<div style="float:left;width:100%; padding:5px 0;"><div style="float:left"><img src="/img/dianhua.png" />售后电话：</div><div style="float:right; width:205px;">' + (result.AfterSalesPhone === null ? "" : result.AfterSalesPhone) + '</div></div>');
                $("#dealerlist").html(li);
            },
            error: function (err) {
                popWindownBlue( "error");
            }
        });
    }
};



function mapObject() {

}
mapObject.DealerToken = new Array();
mapObject.markers = [];
mapObject.Init = function (divId) {
    mapObject.map = new AMap.Map(divId, {
        resizeEnable: true,
        rotateEnable: true,
        dragEnable: true,
        zoomEnable: true,
        //设置可缩放的级别
        zooms: [3, 18],
        //传入2D视图，设置中心点和缩放级别
        view: new AMap.View2D({
            center: new AMap.LngLat(116.397428, 39.90923),
            zoom: 12
        })
    });
};
mapObject.OpenWidow = function (title, addressName, phone1, phone2) {
    var info = [];
    info.push("<div>");
    info.push("<div style=\"padding:0px 0px 0px 0px;\"><div>" + title + "</div>");
    info.push("<div style='margin:0px;padding:0px;'>位置：" + addressName + "</div>");
    info.push("<div style='margin:0px;padding:0px;'>咨询电话：" + phone1 + "</div>");
    info.push("<div style='margin:0px;padding:0px;'>售后电话：" + phone2 + "</div>");
    info.push("</div></div>");
    var infoWindow = new AMap.InfoWindow({
        content: info.join("<br/>"),
        offset: new AMap.Pixel(0, -20)//-113, -140
    });
    return infoWindow;
};

$(function () {
    mapObject.Init("mapDiv");
    mapObject.map.setCity($("#dealer-city").text());
});