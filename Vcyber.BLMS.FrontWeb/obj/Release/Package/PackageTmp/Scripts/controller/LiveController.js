var Live = {};

Live.FreeReduce = function () {
    var quantity = $("#SonataLiveReserveFreeFrequency").val();
    if (quantity === null || !parseInt(quantity) || quantity <= 0) {
        quantity = 1;
    }
    quantity = parseInt(quantity) - 1;
    $("#SonataLiveReserveFreeFrequency").val(quantity);
    Live.Statistics();
};

Live.FreePlus = function () {
    var quantity = $("#SonataLiveReserveFreeFrequency").val();
    if (quantity === null || !parseInt(quantity) || quantity < 0) {
        quantity = 0;
    }
    quantity = parseInt(quantity) + 1;

    var surplusFreeFrequency = $("#surplusFreeFrequency").html();
    if (surplusFreeFrequency === null || parseInt(surplusFreeFrequency) < 0) {
        surplusFreeFrequency = 0;
    }
    if (quantity > parseInt(surplusFreeFrequency)) {
        quantity = parseInt(surplusFreeFrequency);
    }

    $("#SonataLiveReserveFreeFrequency").val(quantity);
    Live.Statistics();
};

Live.FreeChange = function () {
    var quantity = $("#SonataLiveReserveFreeFrequency").val();
    if (quantity === null || !parseInt(quantity) || parseInt(quantity) < 0) {
        quantity = 0;
    }

    var surplusFreeFrequency = $("#surplusFreeFrequency").html();
    if (surplusFreeFrequency === null || parseInt(surplusFreeFrequency) < 0) {
        surplusFreeFrequency = 0;
    }
    if (quantity > parseInt(surplusFreeFrequency)) {
        quantity = parseInt(surplusFreeFrequency);
    }

    $("#SonataLiveReserveFreeFrequency").val(quantity);
    Live.Statistics();
};

Live.ScoreReduce = function () {
    var quantity = $("#SonataLiveReserveScoreFrequency").val();
    if (quantity === null || !parseInt(quantity) || quantity <= 0) {
        quantity = 1;
    }
    quantity = parseInt(quantity) - 1;
    $("#SonataLiveReserveScoreFrequency").val(quantity);
    Live.Statistics();

};

Live.ScorePlus = function () {
    var quantity = $("#SonataLiveReserveScoreFrequency").val();
    if (quantity === null || !parseInt(quantity) || quantity < 0) {
        quantity = 0;
    }
    quantity = parseInt(quantity) + 1;

    var surplusScoreFrequency = $("#surplusScoreFrequency").html();
    if (surplusScoreFrequency === null || parseInt(surplusScoreFrequency) < 0) {
        surplusScoreFrequency = 0;
    }
    if (quantity > parseInt(surplusScoreFrequency)) {
        quantity = parseInt(surplusScoreFrequency);
    }

    $("#SonataLiveReserveScoreFrequency").val(quantity);
    Live.Statistics();
};

Live.ScoreChange = function () {
    var quantity = $("#SonataLiveReserveScoreFrequency").val();
    if (quantity === null || !parseInt(quantity) || parseInt(quantity) < 0) {
        quantity = 0;
    }

    var surplusScoreFrequency = $("#surplusScoreFrequency").html();
    if (surplusScoreFrequency === null || parseInt(surplusScoreFrequency) < 0) {
        surplusScoreFrequency = 0;
    }
    if (quantity > parseInt(surplusScoreFrequency)) {
        quantity = parseInt(surplusScoreFrequency);
    }

    $("#SonataLiveReserveScoreFrequency").val(quantity);
    Live.Statistics();
};

Live.Statistics = function () {
    var _freeQuantity = $("#SonataLiveReserveFreeFrequency").val();
    if (_freeQuantity === null || !parseFloat(_freeQuantity) || parseFloat(_freeQuantity) < 0) {
        _freeQuantity = 0;
    }
    var _scoreQuantity = $("#SonataLiveReserveScoreFrequency").val();
    if (_scoreQuantity === null || !parseFloat(_scoreQuantity) || parseFloat(_scoreQuantity) < 0) {
        _freeQuant_scoreQuantityity = 0;
    }
    var _totalScore = 1800;
    $("#SonataLiveReserveTotalFreeQuantity").html(_freeQuantity);
    $("#SonataLiveReserveTotalScore").html(parseFloat(_scoreQuantity) * parseInt(_totalScore));
    $("#SonataLiveReserveTotalQuantity").html(parseInt(_scoreQuantity) + parseInt(_freeQuantity));
};

//省份改变获取相关的机场
Live.ProvinceChange = function (obj) {
    $.ajax({
        url: "/Live/GetAirportsByProvince",
        type: "get",
        data: { province: $(obj).val(), t: (new Date()).getTime() },
        success: function (result) {
            $("#SonataLiveReserveAirportId").html(result);
        },
        error: function (err) {

        }
    });
};

//机场改变获取相关的机场候机室
Live.AirportChange = function () {
    var airportName = $("#SonataLiveReserveAirportId  option:selected").text();
    if (airportName !== "") {
        $.ajax({
            url: "/Live/GetAirportRoomsByAirportName",
            type: "get",
            data: { airportName: airportName, t: (new Date()).getTime() },
            success: function (result) {
                $("#SonataLiveReserveAirportRoomId").html(result);
            },
            error: function (err) {

            }
        });
    }
};

Live.SonataLiveReserveSave = function () {
    var _userId = $("#SonataLiveReserveUserId").val();
    var _phoneNumber = $("#SonataLiveReserveMobile").val();
    var _freeCount = $("#SonataLiveReserveFreeFrequency").val();
    var _scoreCount = $("#SonataLiveReserveScoreFrequency").val();
    var _airportId = $("#SonataLiveReserveAirportRoomId").val();

    var _isCheckConfirm = $("#IsCheckConfirm").is(':checked');

    var _source = $("#source").val();
    if (_source == "" || _source == undefined) {
        _source = "blms";
    }

    if (!_isCheckConfirm) {
        popWindownBlue( "请先同意预约要求");
        return false;
    }

    if (_userId === null || _userId === "") {
        popWindownBlue("请先登录后再预约", function () {
            window.location = "/Account/LogonPage?returnUrl='/Live/Index'";
           
        });
        return false;
    }
    if (_freeCount === null || parseInt(_freeCount) < 0) {
        popWindownBlue( "请正确输入免费预约次数");
        return false;
    }
    if (_scoreCount === null || parseInt(_scoreCount) < 0) {
        popWindownBlue( "请正确输入积分预约次数");
        return false;
    }
    if (parseInt(_scoreCount) <= 0 && parseInt(_freeCount) <= 0) {
        popWindownBlue( "请正确填写预约的次数");
        return false;
    }
    if (_airportId === null || _airportId === "" || parseInt(_airportId) < 0) {
        popWindownBlue( "请选择机场候机室");
        return false;
    }
    if (_phoneNumber === null || _phoneNumber === "") {
        popWindownBlue( "请输入验证码接收手机号");
        return false;
    }
    $.ajax({
        url: "/Live/LiveReserve",
        type: "post",
        dataType: "json",
        data: { userId: _userId, phoneNumber: _phoneNumber, freeCount: _freeCount, scoreCount: _scoreCount, airportId: _airportId, source : _source },
        success: function (result) {
            if (result.IsSuccess) {
                var sncodes = [];
                if (result.Data !== null) {
                    result.Data.forEach(function (obj) {
                        sncodes.push(obj.SNCode);
                    });
                }
                var _cookieHelper = new Cookie();
                _cookieHelper.setCookie("sncodes", JSON.stringify(result), 1);
                window.location = "/Live/ReserveSuccess";
                return false;
            } else {
                window.location = "/Live/ReserveFailed";
                return false;
            }
        },
        error: function () {

        }
    });

};