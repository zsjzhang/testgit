$(function () {
    navigator.geolocation.getCurrentPosition(onSuccess, onError, { enableHighAccuracy: true, maximumAge: 1000 });    
});

function round(v, e) {
    var t = 1;
    for (; e > 0; t *= 10, e--);
    for (; e < 0; t /= 10, e++);
    return Math.round(v * t) / t;
}
//成功时
function onSuccess(position) {
    //返回用户位置
    //经度
    var longitude = position.coords.longitude;
    //纬度
    var latitude = position.coords.latitude;
    gps(longitude, latitude);
}
//失败时
function onError(error) {
    switch (error.code) {
        case 1:
            console.log("位置服务被拒绝");
            break;
        case 2:
            console.log("暂时获取不到位置信息");
            break;
        case 3:
            console.log("获取信息超时");
            break;
        case 4:
            alert("未知错误");
            break;
    }
}
function gps(longitude, latitude) {
    AMap.service(["AMap.Geocoder"], function () { //加载地理编码
        geocoder = new AMap.Geocoder({
            radius: 3000,
            extensions: "base"
        });
        //步骤三：通过服务对应的方法回调服务返回结果，本例中通过逆地理编码方法getAddress回调结果
        geocoder.getAddress(new AMap.LngLat(longitude, latitude), function (status, result) {
            //根据服务请求状态处理返回结果
            var province = "";
            var city = "";
            if (status == 'error') {
                alert("服务请求出错啦！");
            }
            if (status == 'no_data') {
                alert("无数据返回，请换个关键字试试～～");
            }
            else {
                if (result.regeocode.addressComponent.city == "") {
                    city = result.regeocode.addressComponent.province;
                }
                else {
                    city = result.regeocode.addressComponent.city;
                }
                common.post(common.api("Sonata/OnePoint"), { province: result.regeocode.addressComponent.province, city: city, long1: longitude, lat1: latitude }, function (result) {
                    if (result) {

                        var html = "";


                        $.each(result, function (index, item) {

                            html += "   <div class=\"agency-row\">";
                            html += " <div class=\"row\">";
                            html += " <strong class=\"name\">" + item.Name + "</strong>";
                            //html += " <div class=\"coordinate\"></div>";
                            html += " </div>";
                            html += "<div class=\"row\">";
                            html += "  <div class=\"col\">";
                            html += " <p>";
                            html += item.Address + "  <br />";
                            html += "距离" + round(item.Distance, 1) + "公里(仅供参考)<br />";//"010-88992391<br />";
                            html += "";//"Email:cacacwang@126.com";
                            html += "</p>";
                            html += "</div>";
                            html += "<div class=\"btn-bar\">";
                            html += ""; //"<input type=\"button\" value=\"电话\" class=\"agency-tel-btn\" />";
                            if (item.Phone == undefined) {
                                html += "<a class=\"agency-tel-btn\" href=\"javascript:alert('很抱歉，暂未收录该经销商电话。')\"/>电话</a>";
                            }
                            else {
                                html += "<a class=\"agency-tel-btn\" href=\"tel:" + item.Phone + "\">电话</a>";
                            }
                            html += "<input type=\"button\" value=\"地图\" class=\"agency-mp-btn agencyMapBtn\" onclick=\"recmap('" + item.Position + "','" + item.Address + "')\" />";
                            html += "</div>";
                            html += "</div>";
                            html += "</div>";
                        });
                        $("#dealerPanl").html(html);
                    }
                });
            }
        });
    });
}
