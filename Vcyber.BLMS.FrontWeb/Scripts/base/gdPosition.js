
var gdposition = {
    lng: 0,
    lat: 0,
    pos: ""
};

function gotoposition() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(locationSuccess, locationError, {
            // 指示浏览器获取高精度的位置，默认为false
            enableHighAcuracy: true,
            // 指定获取地理位置的超时时间，默认不限时，单位为毫秒
            timeout: 5000,
            // 最长有效期，在重复获取地理位置时，此参数指定多久再次获取位置。
            maximumAge: 3000
        });
    } else {
        popWindownBlue( "Your browser does not support Geolocation!");
    }
};

function locationError(error) {
    switch (error.code) {
        case error.TIMEOUT:
            console.info("A timeout occured! Please try again!");
            break;
        case error.POSITION_UNAVAILABLE:
            console.info('We can\'t detect your location. Sorry!');
            break;
        case error.PERMISSION_DENIED:
            console.info('Please allow geolocation access for this to work.');
            break;
        case error.UNKNOWN_ERROR:
            console.info('An unknown error occured!');
            break;
    }
}

function locationSuccess(position) {
    var coords = position.coords;
    var result = transformFromWGSToGCJ(coords.longitude, coords.latitude);
    //result[0] = 116.416218;
    //result[1] = 39.91036;
    gdposition.lng = result[0];
    gdposition.lat = result[1];

    var reposition = function () {
        var lnglatXY = new AMap.LngLat(gdposition.lng, gdposition.lat);

        //1.定位
        var map = new AMap.Map('mapContainer', {
            //传入2D视图，设置中心点和缩放级别
            view: new AMap.View2D({
                center: new AMap.LngLat(gdposition.lng, gdposition.lat),
                zoom: 15
            })
        });

        //2.加标注点
        var markerContent = document.createElement("div");
        markerContent.className = "markerContentStyle";

        var markerSpan = document.createElement("div");
        markerSpan.className = "markerSpanStyle";

        var markerButtonSpan = document.createElement("div");
        markerButtonSpan.className = "markerSpanButtonStyle";
        markerButtonSpan.ontouchstart = markerButtonSpanTouch;
        var markerButtonSpanLink = document.createElement("a");
        markerButtonSpanLink.innerHTML = "发送请求";
        markerButtonSpanLink.className = "markerSpanButtonStyleLink";
        markerButtonSpan.appendChild(markerButtonSpanLink);
        markerSpan.appendChild(markerButtonSpan);

        var markerTitleSpan = document.createElement("span");
        markerTitleSpan.className = "markerSpanTitleStyle";
        markerTitleSpan.innerHTML = "目前位置：";
        markerSpan.appendChild(markerTitleSpan);

        var markerAddressSpan = document.createElement("span");
        markerAddressSpan.id = "markerSpanStyleAddress";
        markerAddressSpan.innerHTML = "";
        markerSpan.appendChild(markerAddressSpan);


        markerContent.appendChild(markerSpan);
        //点标记中的图标
        var markerImg = document.createElement("img");
        markerImg.className = "markerlnglat";
        markerImg.src = "/img/gddingweilogo.png";
        markerContent.appendChild(markerImg);
        var marker = new AMap.Marker({
            map: map,
            position: lnglatXY,
            content: markerContent
        });
        marker.setMap(map);
        //map.setFitView();

        //3.设置地理位置
        setTimeout(function () {
            MapGeocoder(new AMap.LngLat(gdposition.lng, gdposition.lat));
        }, 1000);
        AMap.event.addListener(map, 'dragging', function (e) {
            marker.setPosition(map.getCenter());
        });
        AMap.event.addListener(map, 'dragend', function (e) {
            MapGeocoder(map.getCenter());
        });
    };
    var MapGeocoder = function (lnglat) {
        var MGeocoder;
        //加载地理编码插件
        AMap.service(["AMap.Geocoder"], function () {
            MGeocoder = new AMap.Geocoder({
                radius: 1000,
                extensions: "all"
            });
            //逆地理编码
            MGeocoder.getAddress(lnglat, function (status, result) {
                if (status === 'complete' && result.info === 'OK') {
                    address = result.regeocode.formattedAddress;
                    document.getElementById("markerSpanStyleAddress").innerHTML = address;
                    gdposition.pos = address;
                    gdposition.lng = lnglat.lng;
                    gdposition.lat = lnglat.lat;
                }
            });
        });
    }
    setTimeout(reposition, 1000);
}

//发送过位置消息
function sendMyPosition() {
    var locationX = gdposition.lng;
    var locationY = gdposition.lat;
    var address = gdposition.pos;
    if (!locationX || !locationY || !address) {
        popWindownBlue( "定位不成功，请重新定位");
        //$("#showResult").html("定位不成功，请重新定位");
        //$("#showResult").show();
        return false;
    }
    var regMobile = /^1\d{10}$/gi;
    var _usermobile = $("#userMobile").val();
    if (!_usermobile || !regMobile.test(_usermobile)) {
        popWindownBlue( "请正确输入手机号");
        //$("#showResult").html("");
        //$("#showResult").show();
        return false;
    }
    var reqdata = {
        UserMobile: _usermobile,
        Position: locationX + "," + locationY,
        Address: address
    };
    $.ajax({
        url: "/Rescue/freeRoadRescue",
        type: "post",
        data: reqdata,
        dataType: "json",
        success: function (resultData) {
            var result = JSON.parse(resultData);
            if (result && result.IsSuccess) {
                popWindownBlue( "发送成功，请耐心等候，客服人员将尽快与您联系");
                //$("#showResult").html("");
                //$("#showResult").show();
                return false;
            } else {
                popWindownBlue( "发送失败，请重新发送");
                //$("#showResult").html("");
                //$("#showResult").show();
                return false;
            }
        },
        error: function (err) {
            //popWindownBlue( "err=" + JSON.stringify(err));
            popWindownBlue( "对不起，系统异常");
            //$("#showResult").html("");
            //$("#showResult").show();
            return false;
        }
    });
    return false;
}