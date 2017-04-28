
function setTake(lng, lat, address) {
    //alert(lng);
    $("#TakeLong").val(lng);
    $("#TakeLat").val(lat);
    $("#TakeAddress").val(address);

    if ($("#ReturnAddress").val() != "") {
        setDealer();
    }
}
function setReturn(lng, lat, address) {
    $("#ReturnLong").val(lng);
    $("#ReturnLat").val(lat);
    $("#ReturnAddress").val(address);
    if ($("#TakeAddress").val() != "") {
        setDealer();
    }
}

function setDealer() {
    //alert(province);
    //alert(city);
    $("#DealerProvince").val(province);
    $("#DealerCity").val(city);
    $.post("/carService/TwoPoints", {
        province: province,
        city: city,
        long1: $("#TakeLong").val(),
        lat1: $("#TakeLat").val(),
        long2: $("#ReturnLong").val(),
        lat2: $("#ReturnLat").val()
    }, function (result) {
        if (result) {
            //alert(JSON.stringify(result));
            $("#DealerId option[value!='']").remove();
            $.each(result.Data, function (index, item) {
                //alert(item);
                $("#DealerId").append("<option value=" + item.DealerId + ">" + item.Name + "</option>");
            });
        }
    });
}

function initValidator() {
    $.validator.setDefaults({
        submitHandler: submitForm
    });
    $("#mainForm").validate({
        rules: {
            //TakeAddress: { required: true },
            //ReturnAddress: { required: true },
            DealerId: { required: true },
            CarSeries:{required:true},
            LicensePlate: { required: true },
            //VIN: { required: true },
            ScheduleDate: { required: true },
            ReturnDate: { required: true },
            UserName: { required: true },
            //UserSex: { required: true },
            Phone: { required: true, mobileCN: true }
            //Comment: { required: true }
        },
        messages: {
            //TakeAddress: { required: "请输入取车地点" },
            //ReturnAddress: { required:"请输入送车地点" },
            DealerId: { required: "请选择经销商" },
            CarSeries: { required: "请选择车型" },
            LicensePlate: { required: "请输入车牌号码" },
            //VIN: { required: "请选择车架号" },
            ScheduleDate: { required: "请选择取车时间" },
            ReturnDate: { required: "请选择送车时间" },
            UserName: { required: "请输入姓名" },
            //UserSex: { required: "请选择性别" },
            Phone: { required: "请输入手机号", mobileCN: "请输入正确手机号码" }
            //Comment: { required: "请输入补充内容" }
        },
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        showErrors: function (errorMap, errorList) {
            var errMsg = "";
            if ($("#TakeAddress").val().length == 0)
            {
                errMsg += "请输入取车地点<br />";
            }
            if ($("#ReturnAddress").val().length == 0) {
                errMsg += "请输入送车地点<br />";
            }
            $.each(errorList, function (i, v) {
                errMsg += (v.message + "<br />");
            });
            if (errMsg != "") {
                $("#errorContainer").html(errMsg);
            }
        }
    });
}

$(function () {
    initValidator();
    bindMeCard();
    $("#DealerId").change(function () {
        $("#DealerName").val($(this).find("option:selected").text());
    });
});

//我的车辆，车架号
function bindMeCard() {
    //common.post(common.api("PageApi/MeCarList"), {}, function (result) {
    //    if (result) {
    //        $.each(result, function (index, item) {
    //            $("#VIN").append(common.format("<option value=\"{0}\">{0}</option>", item.VIN));
    //        });
    //    }
    //});
}

function submitForm() {
    var requestData = $("#mainForm").serialize();
    $.post("/carService/SubmitFreeCar", requestData, function (result) {
        //debugger
        if(result != null)
        {
            if (result.IsSuccess) {
                AlertSuccess("预约成功！", function () {
                    window.location.href = "/MyCenter/Index";
                })
            }
            else {
                $("#errorContainer").html(result.Message);
            }
        }
    });
}

var latitude = 0;
var longitude = 0;
var mapObj, marker;
var lnglat;
var centerlnglat;
var province;
var city;
//WeChat.readyCallback = function (obj, wx) {
//    obj.getLocation(function (res) {
//        latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
//        longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
//        var speed = res.speed; // 速度，以米/每秒计
//        var accuracy = res.accuracy; // 位置精度
//        //alert(common.format("经度：{0}，纬度：{1}，速度：{2}，位置经度：{3}", latitude, longitude, speed, accuracy));
//        centerlnglat = new AMap.LngLat(longitude,latitude);

//    });
//}



function mapInit(adreesstype) {
    MapWrap = $("#MapWrap");
    MapWrap.show();

    mapObj = new AMap.Map("MapWrap", { center: centerlnglat, level: 13 });

    var listener = AMap.event.addListener(mapObj, "click", function (e) {
        lnglat = e.lnglat;
        //if (marker) {
        //    marker.hide();
        //}
        //mapobj.clearmap();

        //marker = new amap.marker({
        //    map: mapobj,
        //    position: e.lnglat,
        //    icon: "https://webapi.amap.com/images/0.png",
        //    offset: new amap.pixel(-10, -34),
        //    visible: true,
        //    //content: "<input type='button' value='选择地点' onclick='settake(" + lnglat.getlng() + "," + lnglat.getlat() + ")' />"
        //});
        //amap.event.addlistener(marker, "click", function (e) { settake(lnglat.getlng(), lnglat.getlat()); });


        //mapObj.setCenter(lnglat);
        AMap.service(["AMap.Geocoder"], function () { //加载地理编码
            geocoder = new AMap.Geocoder({
                radius: 3000,
                extensions: "base"
            });
            //步骤三：通过服务对应的方法回调服务返回结果，本例中通过逆地理编码方法getaddress回调结果
            geocoder.getAddress(new AMap.LngLat(lnglat.getLng(), lnglat.getLat()), function (status, result) {
                //根据服务请求状态处理返回结果
                if (status == 'error') {
                    mobileAlert("服务请求出错啦！ ");
                }
                if (status == 'no_data') {
                    mobileAlert("无数据返回，请换个关键字试试～～");
                }
                else {
                    //alert(json.stringify());
                    province = result.regeocode.addressComponent.province;
                    if (result.regeocode.addressComponent.city == "") {
                        city = result.regeocode.addressComponent.province;
                    }
                    else {
                        city = result.regeocode.addressComponent.city;
                    }
                    if (adreesstype == "take") {
                        setTake(e.lnglat.getLng(), e.lnglat.getLat(), result.regeocode.formattedAddress);
                    }
                    else {
                        setReturn(e.lnglat.getLng(), e.lnglat.getLat(), result.regeocode.formattedAddress);
                    }

                }
            });
        });
        MapWrap.html("");
        MapWrap.hide();

    });

    mapObj.setFitView();

}


$("#TakeSelect").click(function () {
    mapInit('take');
});

$("#ReturnSelect").click(function () {
    mapInit("return");
});
