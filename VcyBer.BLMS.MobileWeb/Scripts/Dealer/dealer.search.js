var DealerSearchScripts = function () {

    navigator.geolocation.getCurrentPosition(onSuccess, onError, { enableHighAccuracy: true, maximumAge: 1000 });

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
                else
                {
                    province = result.regeocode.addressComponent.province;
                    if (result.regeocode.addressComponent.city == "") {
                        city = result.regeocode.addressComponent.province;
                    }
                    else {
                        city = result.regeocode.addressComponent.city;
                    }
                    $("#DealerProvince").attr("value", province);
                    bindCity(province);

                    bindDealer(province, city);               
                }
            });
        });
    }


    $(function () {
        $("#DealerProvince").change(function () {
            $("#DealerCity option[value!='']").remove();
            $("#DealerId option[value!='']").remove();
            $("#DealerCity").append("<option value=\"选择城市\">选择城市</option>");

            bindCity(this.value);
        });
        $("#DealerCity").change(function () {

            bindDealer($("#DealerProvince").val(), this.value);
        });
    });

    function bindCity(province) {        
        $.ajax({
            url: "/Common/Citys",
            type: "get",
            data: { provinceValue: province },
            dataType: "json",
            success: function (result) {
                $("#DealerCity").html("");
                var _html = "<option value='选择城市'>选择城市</option>";
                $(result).each(function (i, obj) {
                    _html += "<option value=" + obj + ">" + obj + "</option>";
                });
                //将获取的城市列表填充
                $("#DealerCity").append(_html);
            },
            error: function (err) {
            }
        });
    }

    function bindDealer(province, city) {
        $.ajax({
            url: "/Common/Dealers",
            type: "get",
            data: { cityValue: city, provinceValue: province },
            dataType: "json",
            success: function (result) {
                $("#dealerPanl").html("");
                //将获取的供应商列表填充
                var _html = "";
                $(result).each(function (i, obj) {
                    _html += "<div class='agency-row'>";
                    _html += "     <div class='row'> ";
                    _html += "          <strong class='name'>" + obj.Name + "</strong> ";
                    _html += "     </div>";
                    _html += "     <div class='row'>  ";
                    _html += "          <div class='col'> ";
                    _html += "               <p>" + obj.Address + " <br><br></p>";
                    _html += "          </div>";
                    _html += "          <div class='btn-bar'>";
                    if (obj.Phone == undefined) {
                        _html += "<a class=\"agency-tel-btn\" href=\"javascript:alert('很抱歉，暂未收录该经销商电话。')\">电话</a>";
                    } else {
                        _html += "               <a class='agency-tel-btn' href='tel:" + obj.Phone + "'>电话</a>";
                    }
                    _html += "               <input type='button' value='地图' class='agency-mp-btn agencyMapBtn' onclick=\"recmap('" + obj.Position + "','" + obj.Address + "')\">";
                    _html += "          </div>";
                    _html += "     </div>";
                    _html += "</div>";
                });
                //将获取的城市列表填充
                $("#dealerPanl").append(_html);
            },
            error: function (err) {
            }
        });
    }
}();