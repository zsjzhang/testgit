function mapObject() { }

mapObject.Init = function (divId) {
    mapObject.Object = new AMap.Map(divId, {
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
}

mapObject.AddToken = function (y, x, titleName, addressName, content) {
    if (x == "" || y == "") {
        return;
    }

    var marker = new AMap.Marker({
        icon: "https://webapi.amap.com/images/0.png",
        position: new AMap.LngLat(y, x)
    });

    marker.setMap(mapObject.Object);  //在地图上添加点
    AMap.event.addListener(marker, 'click', function () {
        mapObject.AddressPoint(x, y, addressName);
        mapObject.OpenWidow(titleName, content).open(mapObject.Object, marker.getPosition());
    });

    //mapObject.AddressPoint(x, y, addressName);
    //mapObject.OpenWidow(titleName, content).open(mapObject.Object, marker.getPosition());
}

mapObject.OpenWidow = function (title, content) {
    var info = [];
    info.push("<div><div></div> ");
    info.push("<div style=\"padding:0px 0px 0px 4px;\"><b>" + title + "</b>");
    info.push(content);
    info.push("</div></div>");

    var infoWindow = new AMap.InfoWindow({
        content: info.join("<br/>"),
        offset: new AMap.Pixel(16, -45)//-113, -140
    });

    return infoWindow;
}

mapObject.Search = function (cityName, searchKey) {
    var MSearch;
    AMap.service(["AMap.PlaceSearch"], function () {
        MSearch = new AMap.PlaceSearch({ //构造地点查询类
            pageSize: 10,
            pageIndex: 1,
            city: cityName //城市
        });
        //关键字查询
        MSearch.search(searchKey, function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
                mapObject.keywordSearch_CallBack(result);
            }
        });
    });
}

mapObject.addmarker = function (i, d) {
    var lngX = d.location.getLng();
    var latY = d.location.getLat();
    var markerOption = {
        map: mapObject.Object,
        icon: "https://webapi.amap.com/images/" + (i + 1) + ".png",
        position: new AMap.LngLat(lngX, latY),
        topWhenMouseOver: true

    };
    var mar = new AMap.Marker(markerOption);

    var infoWindow = new AMap.InfoWindow({
        content: "<h3><font color=\"#00a6ac\">  " + (i + 1) + ". " + d.name + "</font></h3>" + mapObject.TipContents(d.type, d.address, d.tel),
        size: new AMap.Size(300, 0),
        autoMove: true,
        offset: new AMap.Pixel(0, -20)
    });

    var aa = function (e) {
        infoWindow.open(mapObject.Object, mar.getPosition());
        mapObject.AddressPoint(mar.getPosition().getLat(), mar.getPosition().getLng(), d.address
            );
    };
    AMap.event.addListener(mar, "click", aa);
}
//回调函数
mapObject.keywordSearch_CallBack = function (data) {
    var resultStr = "";
    var poiArr = data.poiList.pois;
    var resultCount = poiArr.length;
    for (var i = 0; i < resultCount; i++) {
        resultStr += "<div id='divid" + (i + 1) + "' onmouseover='openMarkerTipById1(" + i + ",this)' onmouseout='onmouseout_MarkerStyle(" + (i + 1) + ",this)' style=\"font-size: 12px;cursor:pointer;padding:0px 0 4px 2px; border-bottom:1px solid #C1FFC1;\"><table><tr><td><img src=\"https://webapi.amap.com/images/" + (i + 1) + ".png\"></td>" + "<td><h3><font color=\"#00a6ac\">名称: " + poiArr[i].name + "</font></h3>";
        resultStr += mapObject.TipContents(poiArr[i].type, poiArr[i].address, poiArr[i].tel) + "</td></tr></table></div>";
        mapObject.addmarker(i, poiArr[i]);
    }

    mapObject.Object.setFitView();
}

mapObject.TipContents = function (type, address, tel) {  //窗体内容
    if (type == "" || type == "undefined" || type == null || type == " undefined" || typeof type == "undefined") {
        type = "暂无";
    }
    if (address == "" || address == "undefined" || address == null || address == " undefined" || typeof address == "undefined") {
        address = "暂无";
    }
    if (tel == "" || tel == "undefined" || tel == null || tel == " undefined" || typeof address == "tel") {
        tel = "暂无";
    }
    var str = "  地址：" + address + "<br />  电话：" + tel + " <br />  类型：" + type;
    return str;
}

mapObject.ClearMap = function () {
    mapObject.Object.clearMap();
    mapObject.Object.clearInfoWindow();
}

mapObject.SetCity = function (cityName) {
    mapObject.Object.setCity(cityName);
}

//经纬度距离
mapObject.Distance = function (slng, slat, elng, elat) {
    var startLngLat = AMap.LngLat(slng, slat);
    var endLngLat = AMap.LngLat(elng, elat);
    return startLngLat.distance(endLngLat);
}

//外部回调方法
mapObject.AddressPoint = function (x, y, addressName) {

}

