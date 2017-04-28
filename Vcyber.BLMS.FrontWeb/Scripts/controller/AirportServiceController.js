var AirportCtr = {
    isStar: true
};
AirportCtr.togglerShow = function (obj) {
    $(".alert_box").each(function () {
        $(this).hide();
    });
    $(obj).show("slow");
    return false;
}

AirportCtr.showWinningInfo = function () {
    if (!AirportCtr.isStar) {
        //popWindownBlue( AirportCtr.isStar.toString());
        return false;
    }
    //$("#divDoubleWinningInfo").modal("show");
    $.ajax({
        url: "/AirportService/StartActivity",
        data: { activityId: 5, oid: $("#from_oid").val(), source: $("#from_source").val() },
        type: "post",
        datatype: "json",
        beforeSend: function () {
            AirportCtr.isStar = false;
        },
        success: function (result) {
            if (result == null || result.code == null || result.code == 400) {
                AirportCtr.togglerShow('#divNotWinningInfo');
                //$("#divNotWinningInfo").fadeIn(500);
                return false;
            }
            else if (result.code == 401) {
                AirportCtr.togglerShow('#divHasJoinInfo');
                //$("#divHasJoinInfo").fadeIn(500);
                return false;
            }
            else {
                AirportCtr.isStar = false;
                switch (result.prizeid) {
                    case 0: {
                        AirportCtr.togglerShow('#divWinTravelSetInfo');
                        //$("#divWinTravelSetInfo").fadeIn(500);
                        break;
                    }
                    case 1: {
                        AirportCtr.togglerShow('#divWinAirportCodeInfo');
                        //$("#divWinAirportCodeInfo").fadeIn(500);
                        break;
                    }
                    case 2: {
                        AirportCtr.togglerShow('#divDoubleWinningInfo');
                        //$("#divDoubleWinningInfo").fadeIn(500);
                        break;
                    }
                    default:
                        break;
                }
                return false;
            }

        },
        error: function () {
            //popWindownBlue( "系统异常");
        },
        complete: function (xhr) {
            xhr = null;
            //if (xhr == null || xhr.responseJSON == null || xhr.responseJSON.code == null || xhr.responseJSON.code == 400)
            //{ AirportCtr.isStar = true; }
            //AirportCtr.isStar = true;
            $("#playyaoyiyao").attr("autoplay", "");
        }
    });
}

AirportCtr.airportroominfo = function () {
    var _Province = $(obj).attr("data-province");
    var _City = $(obj).attr("data-city");
    var _Airport = $(obj).text();
    $("#divRirportRoomInfo #divDialogTitle").text(_Airport);
    $.ajax({
        url: "/AirportService/AirportRoomInfo",
        data: { province: _Province, city: _City, airport: _Airport },
        datatype: "json",
        type: "post",
        success: function (result) {
            $("#divRirportRoomInfo #divAirportRoomInfoConent").html(result);
            AirportCtr.togglerShow('#divRirportRoomInfo');
            //$("#divRirportRoomInfo").show();
            return false;
        },
        error: function () {
            //popWindownBlue( "系统异常")
        }
    });
}

AirportCtr.isSubmitUserInfo = false;
AirportCtr.completeuserinfo = function () {
    var prizeId = $("#inputPrizeId").val();
    var prizeType = $("#inputPrizeType").val();
    var _name = $("#airportUserName").val();
    var _tel = $("#airportUserTel").val();
    var _provinceId = $("#myOrderAddressProvince").find("option:selected").val();
    var _cityId = $("#myOrderAddressCity").find("option:selected").val();
    var _areaId = $("#myOrderAddressCounty").find("option:selected").val();
    var _province = $("#myOrderAddressProvince").find("option:selected").text();
    var _city = $("#myOrderAddressCity").find("option:selected").text();
    var _area = $("#myOrderAddressCounty").find("option:selected").text();
    var _address = $("#airportDetailAddress").val();
    var _oid = $("#from_oid").val();
    var _source = $("#from_source").val();
    if (prizeType == 1) {
        //虚拟获取信息验证
        if (_name == null || _name == "" || _tel == null || _tel == "") {
            popWindownBlue( "信息不完整");
            return false;
        }
    } else {
        if (_name == null || _name == "" || _tel == null || _tel == "" || _provinceId == null || _provinceId == -1 || _cityId == null || _cityId == -1 || _areaId == null || _areaId == -1 || _address == null || _address == "") {
            popWindownBlue( "信息不完整");
            return false;
        }
    }
    var phoneReg = new RegExp("^(13|14|15|17|18)[0-9]{9}$");
    if (!phoneReg.test(_tel)) {
        popWindownBlue( "请输入正确的手机号码.");
        $("#airportUserTel").val("");
        return false;
    }
    if (AirportCtr.isSubmitUserInfo) {
        return false;
    }
    else {
        AirportCtr.isSubmitUserInfo = true;
    }
    $.ajax({
        url: "/AirportService/CompleteWinningInfo",
        data: { activityId: 5, prizeId: prizeId, _name: _name, _tel: _tel, _province: _province, _city: _city, _area: _area, _address: _address, prizeType: prizeType, oid: _oid, source: _source },
        datatype: "json",
        type: "post",
        success: function (result) {
            if (result) {
                AirportCtr.togglerShow('#divCompleteWinUserInfo');
                //$("#divCompleteWinUserInfo").fadeIn(500);
                return false;
            } else {
                popWindownBlue( "数据保存失败！");
                return false;
            }
        },
        error: function () { }
    });
}

AirportCtr.isSubmitDriveInfo = false;
AirportCtr.completedriveuserinfo = function (obj) {
    var _name = $("#testDrivetUserName").val();
    var _tel = $("#testDriveUserTel").val();
    var _provinceId = $("select#sltProvince").find("option:selected").val();
    var _cityId = $("select#sltCity").find("option:selected").val();
    var _dealerId = $("select#sltDealer").find("option:selected").val();
    var _cartypeId = $("select#carType").find("option:selected").val();
    var _province = $("select#sltProvince").find("option:selected").text();
    var _city = $("select#sltCity").find("option:selected").text();
    var _dealer = $("select#sltDealer").find("option:selected").text();
    var _cartype = $("select#carType").find("option:selected").text();
    var _driveTime = $("#testDriveDateTime").val();
    if (_name == null || _name == "") {
        popWindownBlue( "姓名信息不能为空！");
        return false;
    }
    var phoneReg = new RegExp("^(13|14|15|17|18)[0-9]{9}$");
    if (_tel == null || _tel == "" || !phoneReg.test(_tel)) {
        popWindownBlue( "电话号码为空或格式错误！");
        $("#testDriveUserTel").val("");
        return false;
    }
    if (_driveTime == null || _driveTime == "") {
        popWindownBlue( "预约日期不能为空！");
        $("#testDriveDateTime").val("");
        return false; 
    }
    if (_provinceId == null || _provinceId == "" || _provinceId == -1 || _cityId == null || _cityId == "" || _dealerId == null || _dealerId == "" || _cityId == -1 || _dealerId == -1) {
        popWindownBlue( "特约店信息选择不完整！");
        return false;
    }
    if (AirportCtr.isSubmitDriveInfo) {
        return false;
    } else {
        AirportCtr.isSubmitDriveInfo = true;
    }
    AirportCtr.reserveDrive(_cartype, _province, _city, _dealerId, _dealer, _driveTime, _name, 1, _tel, null);
}

//预约试驾
AirportCtr.reserveDrive = function (carType, province, city, dealerid, dealerName, driveTime, userName, gender, mobile, planBuyTime) {

    $.ajax({
        url: "/AirportService/DoTestDrive",
        type: "post",
        dataType: "json",
        data: { CarSeries: carType, DealerProvince: province, DealerCity: city, DealerId: dealerid, DealerName: dealerName, ScheduleDate: driveTime, UserName: userName, UserSex: gender, Phone: mobile, PurchaseTimeFrame: planBuyTime, DataSource: "blms_wechat" },
        success: function (result) {
            if (result !== null && result.code == 200) {
                //您好，您已成功预约_年_月_日，____（特约店），_____(车型)的店内试驾。到店试驾别忘记给小编发送试驾美图哦！
                var _msg = "您好，您已成功预约" + driveTime + "，" + dealerName + "，" + carType + "的店内试驾。到店试驾后将试驾照片发送到bluemembers官方微信（公众号：北京现代bluemembers）即可哦！分享给好友一起赢好礼吧!";
                $("#testDriveSubmitInfo").text(_msg);
                AirportCtr.togglerShow('#divTestDriveSuccess');
                //$("#divTestDriveSuccess").show(500);
                return false;
            } else {
                popWindownBlue( result.msg);
                return false;
            }
        },
        error: function (err) {
        }
    });
}

//分享相关
var _json = {
    title: "暖心候机礼  服务再升级",
    desc: "暖心候机礼 服务再升级 惊喜摇不停！尊享候机服务码、定制旅行四件套等惊喜大奖等你来抢。活动期间到店试驾即获两次免费候机服务、购车更享超值礼包！",
    link: "https://www.bluemembers.com.cn/airportservice/index",
    imgUrl: "https://www.bluemembers.com.cn/img/AirportService/bg.jpg"
};
var currUrl = window.location.href;
$.ajax({
    url: "https://www.bluemembers.com.cn/weixin/WeChat/Common/JSSDK_Signature",
    data: { url: currUrl },
    type: "get",
    dataType: "json",
    success: function (data) {
        data.data.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo', 'getLocation'];
        wx.config(data.data);
        wx.ready(function (argument) {
            wx.onMenuShareTimeline({
                title: _json.title, // 分享标题
                desc: _json.desc, // 分享描述
                link: _json.link, // 分享链接
                imgUrl: _json.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    //$("#shareit").css("display", "none");
                    //Sharestat("tsina");

                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享给朋友
            wx.onMenuShareAppMessage({
                title: _json.title, // 分享标题
                desc: _json.desc, // 分享描述
                link: _json.link, // 分享链接
                imgUrl: _json.imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                    //$("#shareit").css("display", "none");
                    //Sharestat("pengyou");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享到QQ
            wx.onMenuShareQQ({
                title: _json.title, // 分享标题
                desc: _json.desc, // 分享描述
                link: _json.link, // 分享链接
                imgUrl: _json.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    //$("#shareit").css("display", "none");
                    //Sharestat("QQ");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享到腾讯微博
            wx.onMenuShareWeibo({
                title: _json.title, // 分享标题
                desc: _json.desc, // 分享描述
                link: _json.link, // 分享链接
                imgUrl: _json.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    //$("#shareit").css("display", "none");
                    //Sharestat("Weibo_Tencent");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        });
        wx.error(function (res) {


        });
    }

});

