function Lingdong() {};


Lingdong.oid = "";
Lingdong.tel = "";
Lingdong.isover = 0;

Lingdong.isBind = 0; //是否绑定认证车主
Lingdong.isSubDrive = 0; //是否预约试驾
Lingdong.isRecommand = 0; //是否推荐
Lingdong.isSubDriveAward = 0; //是否预约试驾获奖
Lingdong.isRecommandAward = 0; //是否推荐获奖
Lingdong.awardTypeOfSubDriv = 0;//0:虚拟卡券；1：机场候机码；2：实物邮寄；
Lingdong.awardTypeOfRecommand = 0;//0:虚拟卡券；1：机场候机码；2：实物邮寄；
Lingdong.awardNameOfSubDriv = "";
Lingdong.awardNameOfRecommand = "";


//webapi LingdongController
//预约接口 div1 
Lingdong.subDrive = function () {

    $("#sharePageContent").val(1); 
    
    if (Lingdong.isSubDrive==0) {
        Lingdong.gotoDiv(11);
        return;
    }
    
    if (Lingdong.isSubDriveAward == 0) {
        Lingdong.gotoDiv(14);
        return;
    }

    if (Lingdong.isSubDriveAward == 1 && Lingdong.isAddAddress != 1) {
        Lingdong.gotoDiv(12);
        return;
    }
    
    if (Lingdong.isSubDriveAward == 1 && Lingdong.isAddAddress==1) {
        Lingdong.gotoDiv(13);
        return;
    }
};

//推荐div2
Lingdong.recommand = function () {

    $("#sharePageContent").val(2);
    $("#rcmdGetAwardStatus").innerHTML = "很遗憾！您没中奖";
    $("#rcmdGetAwardName").innerHTML = "";
    $("#rcmdGetAwardNameDes").innerHTML = "参与试驾预约，抽取定制手环等精美礼品，更有机会得千元加油卡。";

    if (Lingdong.isBind == 0) {
        Lingdong.gotoDiv(21);
        return;
    }

    if (Lingdong.isRecommand == 0) {
        Lingdong.gotoDiv(22);
        return;
    }
    
    Lingdong.gotoDiv(23);

    if (Lingdong.isRecommandAward==1) {
        $("#rcmdGetAwardStatus").text("恭喜您中奖了") ;
        $("#rcmdGetAwardName").text(Lingdong.awardNameOfRecommand);
        if (Lingdong.awardTypeOfRecommand == 0) {
            $("#rcmdGetAwardNameDes").text("");//实体邮寄
        }
        if (Lingdong.awardTypeOfRecommand == 1) {
            $("#rcmdGetAwardNameDes").text("卡券存储在微信【优惠券】中，请于2016年7月31日前到特约店使用，过期作废。");
        }
        if (Lingdong.awardTypeOfRecommand == 2) {
            $("#rcmdGetAwardNameDes").text("卡券存储在【会员中心】【个人中心】【电子卡券】中。电话预约：拨打北京现代24小时客服热线400-800-1100进行候机服务预约");
        }
    }

};

//重置窗口
Lingdong.gotoDiv = function (divp) {
    for (var i = 0; i < 30; i++) {
        $("#div" + i).hide();
    }
    $("#div" + divp).show();

    //如果是首页，不显示关闭按钮。
    if (divp == 0) { 
        $(".v0_bt8").css('display', 'none');
    } else {
        $(".v0_bt8").css('display', 'block');
    }
    
    //$("#div00").css('display', 'block');
    
    //错误信息

    $("#rzError1").hide();
    $("#rzError2").hide();
    $("#recomdErr1").hide();
    $("#recomdErr2").hide();
};


Lingdong.v0_bt1 = function () {
    //redirect to 试驾有礼 
    Lingdong.subDrive();
};
Lingdong.v0_bt2 = function () {
    //redirect to 推荐有礼
    Lingdong.recommand();
};
Lingdong.v0_bt3 = function () {
    //redirect to 活动介绍
    Lingdong.gotoDiv(3);
};
Lingdong.v0_bt4 = function () {
    //redirect to 参与流程
    Lingdong.gotoDiv(4);
};
Lingdong.v0_bt5 = function () {
    Lingdong.gotoDiv(5);
};
Lingdong.v0_bt6 = function () {
    Lingdong.gotoDiv(6);
};

Lingdong.v0_bt7 = function () {
    var p = $("#sharePageContent").val();
    var url = common.api("Lingdong/ShareGd?p=" + p);
    window.location = url;
};

Lingdong.goShow = function () { 
    var url = common.api("Lingdong/show?openId=" + Lingdong.oid);
    window.location = url;
};

Lingdong.v0_bt8 = function () {
    Lingdong.gotoDiv(0);
};


Lingdong.v0_bt12 = function () {
    Lingdong.submitDriveForm(function (data) { 
        if (data.ret == 0) {
            popWindownBlue( "验证码错误");
            return;
        }
        if (data.ret == 1) {
            popWindownBlue( "您已参加此活动，不能重复参加。");
            return;
        }
        Lingdong.isSubDrive = 1;
        if (data.AwardObj == null || data.AwardObj == "") {;
            Lingdong.isSubDriveAward = 0;
        } else {
            Lingdong.isSubDriveAward = 1;
            Lingdong.awardNameOfSubDriv = data.AwardObj.Name;
            $("#subDrivAwadName").text(Lingdong.awardNameOfSubDriv);
        }
        Lingdong.subDrive(); 
    });
    
};



 
 

Lingdong.init = function () { 
    Lingdong.gotoDiv(0);

    Lingdong.oid = $("#oid").val();
    Lingdong.tel = $("#tel").val();
    Lingdong.isover = $("#isover").val();
    Lingdong.isBind = $("#isBind").val();
    Lingdong.isSubDrive = $("#isSubDrive").val();
    Lingdong.isAddAddress = Lingdong.isSubDrive;
    Lingdong.isRecommand = $("#isRecommand").val();
    Lingdong.isSubDriveAward = $("#isSubDriveAward").val();
    Lingdong.isRecommandAward = $("#isRecommandAward").val();
    Lingdong.awardTypeOfSubDriv = $("#awardTypeOfSubDriv").val();
    Lingdong.awardTypeOfRecommand = $("#awardTypeOfRecommand").val();
    Lingdong.awardNameOfRecommand = $("#awardNameOfRecommand").val();
    Lingdong.awardNameOfSubDriv = $("#awardNameOfSubDriv").val();

    $(".v0_bt1").on("click", Lingdong.v0_bt1);
    $(".v0_bt2").on("click", Lingdong.v0_bt2);
    $(".v0_bt3").on("click", Lingdong.v0_bt3);
    $(".v0_bt4").on("click", Lingdong.v0_bt4);
    $(".v0_bt5").on("click", Lingdong.v0_bt5);
    $(".v0_bt6").on("click", Lingdong.v0_bt6);
    $(".v0_bt7").on("click", Lingdong.v0_bt7);
    $(".v0_bt8").on("click", Lingdong.v0_bt8);

    $(".v0_bt12").on("click", Lingdong.v0_bt12);
    $(".v0_bt13").on("click", Lingdong.submitAddress);
    $(".v0_bt22").on("click", Lingdong.BindChezhu);
    $(".v0_bt23").on("click", Lingdong.submitRcmdForm);
    
    $("#queryAwardBtn").on("click", Lingdong.queryAward);
    
    if (Lingdong.isSubDrive == 1 || Lingdong.isRecommand==1) {
        $("#goShowBtn").show();
    } else {
        $("#goShowBtn").hide();
    }
    $("#goShowBtn").on("click", Lingdong.goShow);


    //发送验证码
    Lingdong.SendValidateCodeInit("#validateCodeBtn1", "#Phone1", "#phoneError");
    Lingdong.SendValidateCodeInit("#validateCodeBtn2", "#phone2", "#phoneError");

    Lingdong.initDriveForm(); 
};

Lingdong.SendValidateCodeInit = function (sendBtn, telInput, errorPanel) {
    $(sendBtn).on("click", function () { 
        var tel = $(telInput).val();
        //$(errorPanel).display = "none";
        if (!checktel(tel)) {
            //$(errorPanel).display = "";
            popWindownBlue( "请输入正确手机号");
            return;
        }
        var url = common.api("Lingdong/SendValidateCode");
        $.post(url, { tel: tel }, function () { });
        time(this);
        return;
    });
};


$(function () {
    Lingdong.init();
    //显示 
    $("#div00").css('display', 'block');
    $("#div100").css('display', 'block');
});


function isnull(txt) {
    if (typeof (txt) == 'undefined' || txt == undefined || txt=='') {
        return true;
    }
};

//时间
var wait = 60;
function time(o) {
    if (wait == 0) {
        o.removeAttribute("disabled");
        o.textContent = "发送验证码";
        wait = 60;
    } else { // 
        o.setAttribute("disabled", true);
        o.textContent = "重新发送(" + wait + ")";
        wait--;
        setTimeout(function() {
            time(o);
        },
            1000);
    }
}

//手机号校验
function checktel(tel) {
    //var reg = /^1[3|4|5|8][0-9]\d{4,8}$/;
    var reg = /^1\d{10}$/;
    if (!reg.test(tel)) {
        return false;
    }
    return true;
}

//身份证校验
function checkIdCard(idcard) {
    var reg = /^[1-9]{1}[0-9]{14}$|^[1-9]{1}[0-9]{16}([0-9]|[xX])$/;
    if (!reg.test(idcard)) {
        return false;
    }
    return true;
}

 
