var DealerNewCtr = {};
DealerNewCtr.GetRightCode = function () {    
    $.ajax({
        url: "/DealerNew/GetTuniuRightCode",
        data: {},
        datatype: "json",
        type: "post",
        success: function (result) {
            if (result.code == null || result.code == 404) {
                $("#divAlertLogin").show("slow");
                return false;
            }
            else if (result.code == 401) {
                popWindownBlue( "尊敬的会员您好，非车主会员暂无法领取途牛五星权益礼包哦~");
                return false;
            }
            else if (result.code == 201) {
                popWindownBlue( result.msg + ":" + result.data.ServiceCode);
                return false;
            }
            else if (result.code == 200) {
                $("#divGetCodeSuccess").find("#alertMessage").text("您已成功领取途牛五星会员权益码:" + result.data.ServiceCode + "，立即激活您的途牛会员权益吧！");
                $("#divGetCodeSuccess").show("slow");
                return false;
            }
            else {
                popWindownBlue( result.msg);
                return false;
            }
        },
        error: function () { }
    });
}

DealerNewCtr.UpgradeFiveStarMembers = function () {
    $.ajax({
        url: "/DealerNew/ToBeFiveStarMember",
        data: {},
        datatype: "json",
        type: "post",
        success: function (result) {
            if (result.code == null || result.code == 400) {
                $("#divAlertLogin").show("slow");
                return false;
            }
            else if (result.code == 201) {
                popWindownBlue( "尊敬的车主会员，您还未领取途牛五星权益礼包哦，请您领取五星权益礼包后进行升级~~");
                return false;
            }
            else if (result.code == 202) {
                //$("#divGetCodeSuccess").find("#alertMessage").text("尊敬的车主会员，您已成为途牛五星会员了~");
                //$("#divGetCodeSuccess").show("slow");
                popWindownBlue( "尊敬的车主会员，您已成为途牛五星会员了~");
                return false;
            }
            else {
                window.open('http://www.tuniu.com/zt/bjxd/', 'newwindow');
                //location.href = "http://www.tuniu.com/zt/bjxd/";
                return false;
            }
        },
        error: function () { }
    });
};

DealerNewCtr.UpgradeDiamondMembership = function () {
    $.ajax({
        url: "/DealerNew/TobeMasonryMember",
        data: {},
        datatype: "json",
        type: "post",
        success: function (result) {
            if (result == null || result.Data == null || result.Data == 404) {
                $("#divAlertLogin").show("slow");
                return false;
            }
            else if (!result.IsSuccess) {
                popWindownBlue( result.Message);
                return false;
            }
            else {
                $("#divSubmitSuccess").show("slow");
                return false;
            }
        },
        error: function () { }
    });
};
//途家会员权益领取
DealerNewCtr.GetTujiaMemberRules = function () {
    $.ajax({
        url: "/DealerNew/GetTujiaMemberRule",
        data: {},
        datatype: "json",
        type: "post",
        success: function (result) {
            //未登录
            if (result == null || result.Data == null || result.Data == 404) {
                $("#divAlertLogin").show("slow");
                return false;
            }
            //信息错误
            else if (!result.IsSuccess) {
                popWindownBlue(result.Message);
                return false;
            }
            //成功领取
            else {
                //$("#divSubmitSuccess").show("slow");
                window.location.href = "https://passport.tujia.com/PortalSite/Register?eid=112068";
                return false;
            }
        },
        error: function () { }
    });
};