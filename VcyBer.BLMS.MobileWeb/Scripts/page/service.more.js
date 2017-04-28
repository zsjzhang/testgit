var mapObj;
var city;
var province;
WeChat.readyCallback = function (obj, wx) {
    obj.getLocation(function (res) {
        var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
        var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
        var speed = res.speed; // 速度，以米/每秒计
        var accuracy = res.accuracy; // 位置精度
        //alert(common.format("经度：{0}，纬度：{1}，速度：{2}，位置经度：{3}", latitude, longitude, speed, accuracy));
        
        AMap.service(["AMap.Geocoder"], function () { //加载地理编码
            geocoder = new AMap.Geocoder({
                radius: 3000,
                extensions: "base"
            });
            
            //步骤三：通过服务对应的方法回调服务返回结果，本例中通过逆地理编码方法getAddress回调结果
            geocoder.getAddress(new AMap.LngLat(longitude, latitude), function (status, result) {
                //根据服务请求状态处理返回结果
                if (status == 'error') {
                    mobileAlert("服务请求出错啦！ ");
                }
                if (status == 'no_data') {
                    mobileAlert("无数据返回，请换个关键字试试～～");
                }
                else {
                    //alert(JSON.stringify());
                    province = result.regeocode.addressComponent.province;
                    if (result.regeocode.addressComponent.city == "") {
                        city = result.regeocode.addressComponent.province;
                    }
                    else {
                        city = result.regeocode.addressComponent.city;
                    }
                    
                    AMap.service('AMap.Weather', function () {
                        weather = new AMap.Weather();
                        weather.getLive(city, function (err, data) {

                            $("#weatherPanl").html(common.format("<strong>天气:{0}</strong><p>{1}℃<br /></p>", data.weather, data.temperature));
                            $("#areaPanl").html(common.format("<span>{0}</span><span>{1}</span>", city, currdate));
                        });
                    });
                   
                    if (city == "北京市")
                    {
                        xianxing();
                    }

                    //alert(city);
                }
            });
        });
    });
}

function getXHNumber(tDate, sDate) {
    var nDayNum = tDate.getDay() == 0 ? 7 : tDate.getDay();
    if (nDayNum > 5) return nDayNum;
    var nDiff = (tDate - sDate) / 1000 / 3600 / 24 / 7 / 13;
    nDiff = Math.floor(nDiff) % 5;
    nDayNum = 5 - nDiff + nDayNum;
    if (nDayNum > 5) nDayNum -= 5;
    return nDayNum;
}
function xianxing() {
    //获取今天的日期
    var dd = new Date();
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;//获取当前月份的日期 
    var d = dd.getDate();
    if (m < 10) {
        m = "0" + m;
    }
    if (d < 10) {
        d = "0" + d;
    }
    var sday = y + "-" + m + "-" + d;

    var sStartDate = '2012-10-08';//开始星期，周一的日期
    var x1 = '1和6';
    var x2 = '2和7';
    var x3 = '3和8';
    var x4 = '4和9';
    var x5 = '5和0';
    var x6 = '不限行';
    var x7 = '不限行';
    var arr1 = sStartDate.split("-");
    var vStartDate = new Date(arr1[0], arr1[1] - 1, arr1[2]);
    var arr2 = sday.split("-");
    var vToday = new Date(arr2[0], arr2[1] - 1, arr2[2]);
    var nTodayNum = getXHNumber(vToday, vStartDate);
    vToday.setDate(vToday.getDate() + 1);
    var nTomorrowNum = getXHNumber(vToday, vStartDate);
    /***start week***/
    var arr_week = new Array("星期六", "星期日", "星期一", "星期二", "星期三", "星期四", "星期五");
    var todayweek = vToday.getDay();
    /***end week***/
    //<span>限</span>
    //<p>5和0</p>
    if (nTodayNum == 6 || nTodayNum == 7) {
        document.getElementById("stopPanl").innerHTML = "" + "<p>" + eval('x' + nTodayNum) + "</p>";
    }
    else {
        document.getElementById("stopPanl").innerHTML = "<span>限</span>" + "<p>" + eval('x' + nTodayNum) + "</p>";
    }
}