var DealerSearchScripts = function () {
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
                        $("#DealerProvince").attr("value", province);
                        bindCity(province);

                        bindDealer(province, city);
                    }
                });
            });
        });
    }


    $(function () {

        $("#DealerProvince").change(function () {
            $("#DealerCity option[value!='']").remove();
            $("#DealerId option[value!='']").remove();
            $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", "选择城市"));

            bindCity(this.value);
        });
        $("#DealerCity").change(function () {

            bindDealer($("#DealerProvince").val(), this.value);
        });

       // $("#DealerProvince")[0].selectedIndex = 1;


    });

    function bindCity(province) {
        common.post(common.api("PageApi/City"), { province: province }, function (result) {
            $.each(result, function (index, item) {
                $("#DealerCity").append(common.format("<option value=\"{0}\">{0}</option>", item.value));
            });
            $("#DealerCity").attr("value", city);
            bindDealer($("#DealerProvince").val(), $("#DealerCity").val());

        });
    }

    function bindDealer(province, city) {
        common.post(common.api("PageApi/Dealer"), { province: province, city: city }, function (result) {
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
                    html += "<br />";//"010-88992391<br />";
                    //if (item.Phone !== undefined) {
                    //    html += "Email:"+item.Email;
                    //}
                    html += "</p>";
                    html += "</div>";
                    html += "<div class=\"btn-bar\">";
                    if (item.Phone == undefined)
                    {
                        html += "<a class=\"agency-tel-btn\" href=\"javascript:alert('很抱歉，暂未收录该经销商电话。')\">电话</a>";
                    }
                    else {
                        html += "<a class=\"agency-tel-btn\" href=\"tel:" + item.Phone.split(" ")[0] + "\">电话</a>";
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


}();