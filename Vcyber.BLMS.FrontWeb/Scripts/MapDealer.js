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

mapObject.AddToken = function (y, x,titleName,content) {
    var marker = new AMap.Marker({
        icon: "https://webapi.amap.com/images/0.png",
        position: new AMap.LngLat(y, x)
    });

    marker.setMap(mapObject.Object);  //在地图上添加点m
    AMap.event.addListener(marker, 'click', function () {
        mapObject.OpenWidow(titleName, content).open(mapObject.Object, marker.getPosition());
    });
}

mapObject.OpenWidow = function (title, content) {
    var info = [];
    info.push("<div><div></div> ");
    info.push("<div style=\"padding:0px 0px 0px 4px;\"><b>" + title + "</b>");
    info.push(content);
    info.push("</div></div>");

    var infoWindow = new AMap.InfoWindow({
        content: info.join("<br/>"),
        offset: new AMap.Pixel(0, -20)//-113, -140
    });

    return infoWindow;
}

mapObject.ClearMap = function () {
    mapObject.Object.clearMap();
    mapObject.Object.clearInfoWindow();
}

mapObject.SetCity = function (cityName) {
    mapObject.Object.setCity(cityName);
}