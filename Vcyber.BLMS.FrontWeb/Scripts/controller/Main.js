


function popWindownBlue(msg, callBack) {

    layer.open({
        type: 1,
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
       // area:['330px','200px'],
        shadeClose: false, //开启遮罩关闭
        content: msg,
        end: function () {
            if (callBack) {
                callBack();
            }
        },
        btn: ['确定']
    }); 
}

//========================================================生日特权页面=============================================
function birthdayCollect(id, name, img) {
    $.ajax({
        url: "/member/checkpost",
        dataType: "json",
        type: "post",
        success: function (data) {
            if (!data || data.code == "301") {
                popWindownBlue(data.msg);
                return false;
            } else {
                //第一步：清空购物车
                ShoppingCart.RemoveAll();
                //第二步：向购物车添加数据
                var _skuId = "";
                var _productDetailSkuItemVlaue = 0;
                var _productDetailSkuPayType = "";
                var _isSelectPayType = false;

                var item = {
                    skuId: _skuId,
                    productId: id,
                    productName: name,
                    price: _productDetailSkuItemVlaue,
                    quantity: 1,
                    imgUrl: img,
                    payType: _productDetailSkuPayType,
                    producttype: "",
                    productcolor: ""
                };
                ShoppingCart.AddItem(item);
                //第三步：去结算
                window.location = "/Order/Index";
                return false;
            }
        }
    });
}

var sendCaptcha = function (mobile, obj) {
    if (!mobile) {
        popWindownBlue("请正确输入手机号");
        return false;
    }
    $.ajax({
        url: "/Captcha/SendCaptcha",
        dataType: "json",
        //获取mvc的内容
        //url: "/Captcha/GetContent",
        //直接获取mvc的内容
        //url: "/Captcha/Index",
        type: "post",
        data: { mobile: mobile },
        success: function (result) {
            if (!result || result.code == "400") {
                popWindownBlue("验证码发送失败,请重新发送");
                return false;
            }
            if (result.code == "200") {
                //更新剩余时长
                countDown(obj, 60);
                popWindownBlue("短信验证码已发送");
                return false;
            }
        },
        error: function (err) {
            popWindownBlue("err");
        }
    });
};

var sendCaptchaAndCheckImageCode = function (mobile, imageCode, obj) {
    if (!mobile) {
        popWindownBlue("请正确输入手机号");
        return false;
    }
    if (!imageCode) {
        popWindownBlue("请正确输入图形验证码");
        return false;
    }
    $.ajax({
        url: "/Captcha/sendCaptchaAndCheckImageCode",
        dataType: "json",
        //获取mvc的内容
        //url: "/Captcha/GetContent",
        //直接获取mvc的内容
        //url: "/Captcha/Index",
        type: "post",
        data: { mobile: mobile, imageCode: imageCode },
        success: function (result) {
            if (!result || result.code == "400") {
                //  popWindownBlue( "验证码发送失败,请重新发送");
                popWindownBlue(result.msg);
                return false;
            }
            else if (!result || result.code == "401") {
                popWindownBlue("图形验证码输入错误");
                return false;
            }
            else if (result.code == "200") {
                //更新剩余时长
                countDown(obj, 60);
                popWindownBlue("短信验证码已发送");
                return false;
            }
        },
        error: function (err) {
            popWindownBlue("err");
        }
    });
};
var sendCaptchaByRegister = function (mobile, imageCode, obj) {
    if (!mobile) {
        popWindownBlue("请正确输入手机号");
        return false;
    }
    if (!imageCode) {
        popWindownBlue("请正确输入图形验证码");
        return false;
    }
    $.ajax({
        url: "/Captcha/sendCaptchaByRegister",
        dataType: "json",
        //获取mvc的内容
        //url: "/Captcha/GetContent",
        //直接获取mvc的内容
        //url: "/Captcha/Index",
        type: "post",
        data: { mobile: mobile, imageCode: imageCode },
        success: function (result) {
            if (!result || result.code == "400") {
                //  popWindownBlue( "验证码发送失败,请重新发送");
                popWindownBlue(result.msg);
                return false;
            }
            else if (!result || result.code == "401") {
                popWindownBlue("图形验证码输入错误");
                return false;
            }
            else if (result.code == "200") {
                //更新剩余时长
                countDown(obj, 60);
                popWindownBlue("短信验证码已发送");
                return false;
            }
        },
        error: function (err) {
            popWindownBlue("err");
        }
    });
};
var sendCaptchaFindPassWord = function (mobile, imageCode, obj) {
    if (!mobile) {
        popWindownBlue("请正确输入手机号");
        return false;
    }
    if (!imageCode) {
        popWindownBlue("请正确输入图形验证码");
        return false;
    }
    $.ajax({
        url: "/Captcha/sendCaptchaByFindPassWord",
        dataType: "json",
        //获取mvc的内容
        //url: "/Captcha/GetContent",
        //直接获取mvc的内容
        //url: "/Captcha/Index",
        type: "post",
        data: { mobile: mobile, imageCode: imageCode },
        success: function (result) {
            if (!result || result.code == "400") {
                //  popWindownBlue( "验证码发送失败,请重新发送");
                popWindownBlue(result.msg);
                return false;
            }
            else if (!result || result.code == "401") {
                popWindownBlue("图形验证码输入错误");
                return false;
            }
            else if (result.code == "200") {
                //更新剩余时长
                countDown(obj, 60);
                popWindownBlue("短信验证码已发送");
                return false;
            }
        },
        error: function (err) {
            popWindownBlue("err");
        }
    });
};

var sendCaptchaChangePassword = function (mobile, imageCode, obj) {
    if (!mobile) {
        popWindownBlue("请正确输入手机号");
        return false;
    }
    if (!imageCode) {
        popWindownBlue("请正确输入图形验证码");
        return false;
    }
    $.ajax({
        url: "/Captcha/sendCaptchaByChangePassword",
        dataType: "json",
        //获取mvc的内容
        //url: "/Captcha/GetContent",
        //直接获取mvc的内容
        //url: "/Captcha/Index",
        type: "post",
        data: { mobile: mobile, imageCode: imageCode },
        success: function (result) {
            if (!result || result.code == "400") {
                //  popWindownBlue( "验证码发送失败,请重新发送");
                popWindownBlue(result.msg);
                return false;
            }
            else if (!result || result.code == "401") {
                popWindownBlue("图形验证码输入错误");
                return false;
            }
            else if (result.code == "200") {
                //更新剩余时长
                countDown(obj, 60);
                popWindownBlue("短信验证码已发送");
                return false;
            }
        },
        error: function (err) {
            popWindownBlue("err");
        }
    });
};
//单击重新改变验证码
function clickRemoveChangeCode() {
    var code = $("#imgCode").attr("src");
    $("#imgCode").attr("src", code + "1");
}

function clickRemoveChangeCodeByPhone() {
    var code = $("#imgCodephone").attr("src");
    $("#imgCodephone").attr("src", code + "1");
}
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

var test = function () {
    popWindownBlue(123);
};




//获取链接地址的参数  
function getQueryString(property) {
    var reg = new RegExp("(^|&)" + property + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
/** 
 * 倒计时函数
 * 需要在按钮上绑定单击事件
 * 如: <input value="发送短信" data-cke-editable="1" data-cke-pa-onclick="setInterval('countDown(this,30)',1000);" contenteditable="false" type="button">
 * 30代表秒数，需要倒计时多少秒可以自行更改
 */
var countDown = function (obj, second) {
    // 如果秒数还是大于0，则表示倒计时还没结束
    if (second >= 0) {
        $(obj).addClass('reSend1')
        // 获取默认按钮上的文字
        if (typeof buttonDefaultValue === 'undefined') {
            buttonDefaultValue = $(obj).html();
        }
        // 按钮置为不可点击状态
        $(obj).attr("disabled", "true");
        // 按钮里的内容呈现倒计时状态
        $(obj).html(second + "S后重新获取");
        // 时间减一
        second--;
        // 一秒后重复执行
        setTimeout(function () { countDown(obj, second); }, 1000);
        // 否则，按钮重置为初始状态
    } else {
        // 按钮置未可点击状态
        $(obj).removeAttr("disabled");
        $(obj).removeClass('reSend1')

        // 按钮里的内容恢复初始状态
        $(obj).html(buttonDefaultValue);
    }
};

$(function () {

    //====================注册规则新开始==============


    $("#AccountLoginSendCaptcha").click(function () {
        var _mobile = $("#homeAccountPhone").val();
        var _imgcode = $("#LoginCaptcha").val();
        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }
        //首先校验手机号
        if (!_mobile) {
            //请输入手机号
            popWindownBlue("请输入手机号");
            return false;
        }
        if (_imgcode == null || _imgcode == "") {

            popWindownBlue("请输入图片验证码");
            return false;
        }

        //验证码发送
        //sendCaptcha(_mobile, this);
        sendCaptchaAndCheckImageCode(_mobile, _imgcode, this);
    });

    $("#btnInputPayNumberToRegisterVip").click(function () {
        var sltDealer = $("#sltDealer").val();
        if (sltDealer == "-1") {
            popWindownBlue("请选择特约店");
            return false;
        }
        $("#btnApplyMemberToDealer_AccountRegister").css({ "background-color": "#075090", "border-color": "#075090" });
        $.ajax({
            url: "/PayFee/AccountPayNumberActive",
            type: "get",
            success: function (resultHtml) {
                $(".zzDivBox").css('display', 'none');
                layer.open({
                    type: 1,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    // skin: 'layui-layer-rim', //加上边框
                    area: ['413px', '217px'], //宽高

                    content: resultHtml
                });
                //var layerIndex = $.layer({
                //    type: 1,   //0-4的选择,
                //    title: false,
                //    border: [0],
                //    closeBtn: [0],
                //    shadeClose: false,
                //    area: ['413x', '217x'],
                //    page: {
                //        html: resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
                //    }
                //});

                // $("#AccountSonataActiveCancelLayerIndex").val(layerIndex);
            }
        });
    });

    $("#btnInputPayNumberToLoginVip").click(function () {
        var sltDealer = $("#resltDealer").val();
        if (sltDealer == "-1") {
            popWindownBlue("请选择特约店");
            return false;
        }
        $("#btnApplyMemberToDealer_AccountRegister").css({ "background-color": "#075090", "border-color": "#075090" });
        $.ajax({
            url: "/PayFee/LoginPayNumberActive",
            type: "get",
            success: function (resultHtml) {
                $(".zzDivBox").css('display', 'none');
                layer.open({
                    type: 1,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    // skin: 'layui-layer-rim', //加上边框
                    area: ['413px', '217px'], //宽高

                    content: resultHtml
                });
                //var layerIndex = $.layer({
                //    type: 1,   //0-4的选择,
                //    title: false,
                //    border: [0],
                //    closeBtn: [0],
                //    shadeClose: false,
                //    area: ['413x', '217x'],
                //    page: {
                //        html: resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
                //    }
                //});

                // $("#AccountSonataActiveCancelLayerIndex").val(layerIndex);
            }
        });
    });
    $("#btnRegisterAccountRegVip").click(function () {

        //popWindownBlue( "test");
        if ($(this).hasClass("isenableclick")) {
            return false;
        }
        var mobile = $("#registerMobile").val();
        var captcha = $("#registerCaptcha").val();
        var passwd = $("#registerPasswd").val();
        var repasswd = $("#registerComfirmPasswd").val();
        var registerImgCaptcha = $("#registerImageCode").val();
        var nickname = "vip" + mobile.substring(5, 11);//暂时生成呢称
        //var nickname = "";
        var paperwork = $("#accountregisterUserPaperWork").val();

        var identityno = $("#registerIdentityNo").val();
        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var accountPayNumber = $("#accountPayNumber").val();
        var _payType = $("#accountActivityType").val();
        //var registerVIN = $("#registerVIN").val();
        var registerVIN = "";
        var returnUrl = $("#returnUrl").val();
        var source = $("#source").val();
        if (mobile == null || mobile == "") {
            popWindownBlue("请输入手机号");
            return false; 
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入短信验证码");
            return false;
        }

        if (registerImgCaptcha == null || registerImgCaptcha == "") {
            popWindownBlue("请输入图片验证码");
            return false;

        }

        var passReg = /^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{8,100}$///匹配必须包含数字大小写字母 长度为8-100
        if (passwd != "" && passwd.length < 8 || !passReg.test(passwd)) {
            popWindownBlue("密码必须8位以上数字和大小写字母组合");
            return false;
        }
        if (passwd == null || passwd == "" || repasswd == null || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        if ($("#btnAccountRegisterAgreeRegister").is(":checked") == false)
            //if (!$("#AccountRegisterAgreeRegister").is(":checked"))
        {
            popWindownBlue("请阅读会员章程及免责条款");
            return false;
        }

        //if (nickname == null || nickname == "") {
        //    popWindownBlue( "请输入昵称");
        //    return false;
        //}
        //customerType默认为-1，身份证类型为1，组织机构代码为2
        var customerType = 1;
        var identityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        var isCarOwner = 1;
        if ($("#registerIdentityNo_account_radio_new").is(":checked")) {
            isCarOwner = 2;
            //if ($("#registerIdentityNo_account_radio").is(":checked")) {
            if (!paperwork || paperwork < 0) {
                popWindownBlue("请选择证件类型");
                return false;
            }

            //}
            if (identityno == null || identityno == "") {
                popWindownBlue("请输入证件号");
                return false;
            }
            if ($("#registerIdentityNo_account_radio_new").is(":checked")) {
                if (1 == paperwork) {
                    identityReg = /(^\d{15}$)|(^\d{17}([0-9]|X)$)/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (2 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{3,21}$|^(P\d{7})|(G\d{8})$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (3 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{7,21}$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
            }
          
            //if ($("#registerIdentityNo_account_radio").is(":checked") && !identityReg.test(identityno)) {
            //    popWindownBlue( "请正确输入证件号");
            //    return false;
            //}
            //取消个人与企业用户
            //if ($("#registerIdentityNo_account_radio").is(":checked")) {
            //    customerType = 1;
            //}
            //if ($("#zuzhijigouNo_account_radio").is(":checked")) {
            //    customerType = 2;
            //    if (!registerVIN) {
            //        popWindownBlue( "请输入VIN");
            //        return false;
            //    }
            //}

            //if ($("#AccountRegisterIsSonataUser").is(":checked")) {
            //    isCarOwner = 3;
            //    if (sltDealer == -1) {
            //        popWindownBlue( "请选择4s店");
            //        return false;
            //    }
            //    if (_payType != null && _payType != "undefined") {
            //        if (_payType == 1) {
            //            if (accountPayNumber == null || typeof accountPayNumber == "undefined") {
            //                popWindownBlue( "请输入支付码");
            //                return false;
            //            }
            //        }
            //    }
            //}
        }
        else {
            identityno = "";
            sltDealer = "";
            paperwork = "";
        }

        //
        User.Register(mobile, captcha, passwd, repasswd, nickname, identityno, sltDealer, isCarOwner, _payType, accountPayNumber, registerVIN, customerType, paperwork, returnUrl, source, registerImgCaptcha);
    });
    //缴费获取积分  向特约店提交申请
    $("#btnApplyToDealer_AccountRegisterVip").click(function () {

        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var identityno = $("#registerIdentityNo").val();
        if (sltDealer == -1) {
            popWindownBlue("请选择特约店");
            return false;

        }
        $.ajax({
            url: "/Account/CreateMembershipRequest",
            type: "post",
            dataType: "json",
            data: { dealerId: sltDealer, identtiyNumber: identityno },
            success: function (result) {
                if (result !== null) {
                    if (result.code == 200) {
                        $(".zzDivBox").css('display', 'none');
                        popWindownBlue("您向特约店申请交费成功", function () {
                            window.location = "/MyCenter/Index?pageName=MyCenter";
                        });

                        return false;
                    }
                    if (result.code == "401") {
                        popWindownBlue("尊敬的客户，您的账号登录异常，请重新登陆！", function () {
                            window.location = "/Account/LogonPage?returnUrl=/Sonata/SonataActive";
                        });
                        return false;
                    
                    }

                    popWindownBlue(result.msg);

                    }
               
            },
            error: function (err) {

            }
        });

        //$.ajax({
        //    url: "CreateMembershipRequest",
        //    type: "post",
        //    dataType: "json",
        //    data: { userId: userid, dealerId: sltDealer, identtiyNumber: identityno },
        //    success: function (result) {
        //        if (result !== null && result.code == 200) {

        //            $(this).css({ "background-color": "#E3E3E3", "border-color": "#aaa" });
        //            var _index = layer.tips('您已选择向特约店申请成为会员', this, {
        //                tips: [1, '#3595CC'],
        //                time: 4000
        //            });
        //            setTimeout(function () { layer.close(_index); }, 3000);
        //            return false;

        //        } else {
        //            popWindownBlue( result.msg);
        //            return false;
        //        }
        //    },
        //    error: function (err) {
        //    }
        //});

        // $("#accountActivityType").val(2); //向特约店申请为2，向天猫购买支付码为1

    });

    $("#btnApplyToDealer_AccountLoginVip").click(function () {

        var sltProvince = $("#resltProvince").val();
        var sltCity = $("#resltCity").val();
        var sltDealer = $("#resltDealer").val();
        var identityno = $("#loginIdentityNo").val();
        if (sltDealer == -1) {
            popWindownBlue("请选择特约店");
            return false;

        }
        $.ajax({
            url: "/Account/CreateMembershipRequest",
            type: "post",
            dataType: "json",
            data: { dealerId: sltDealer, identtiyNumber: identityno },
            success: function (result) {
                if (result !== null) {
                    if (result.code == 200) {
                        $(".zzDivBox").css('display', 'none');
                        popWindownBlue("您向特约店申请交费成功", function () {
                            window.location = "/MyCenter/Index?pageName=MyCenter";
                        });

                        return false;
                    }
                    if (result.code == "401") {
                        popWindownBlue("尊敬的客户，您的账号登录异常，请重新登陆！", function () {
                            window.location = "/Account/LogonPage?returnUrl=/Sonata/SonataActive";
                        });
                        return false;

                    }

                    popWindownBlue(result.msg);

                }

            },
            error: function (err) {

            }
        });

        //$.ajax({
        //    url: "CreateMembershipRequest",
        //    type: "post",
        //    dataType: "json",
        //    data: { userId: userid, dealerId: sltDealer, identtiyNumber: identityno },
        //    success: function (result) {
        //        if (result !== null && result.code == 200) {

        //            $(this).css({ "background-color": "#E3E3E3", "border-color": "#aaa" });
        //            var _index = layer.tips('您已选择向特约店申请成为会员', this, {
        //                tips: [1, '#3595CC'],
        //                time: 4000
        //            });
        //            setTimeout(function () { layer.close(_index); }, 3000);
        //            return false;

        //        } else {
        //            popWindownBlue( result.msg);
        //            return false;
        //        }
        //    },
        //    error: function (err) {
        //    }
        //});

        // $("#accountActivityType").val(2); //向特约店申请为2，向天猫购买支付码为1

    });

    $("#btnApplyToTmall_AccountRegisterVip").click(function () {


        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var userid = $("#userid").val();
        var identityno = $("#registerIdentityNo").val();
        if (sltDealer == -1) {
            popWindownBlue("请选择特约店");
            return false;
        }
        $("#btnApplyMemberToDealer_AccountRegister").css({ "background-color": "#075090", "border-color": "#075090" });
        $.ajax({
            url: "/PayFee/AccountPayNumberActive",
            type: "get",
            success: function (resultHtml) {
                layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['413x', '217x'],
                    page: {
                        html: resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
                    }
                });
                $(".zzDivBox").css('display', 'none');
                $("#AccountSonataActiveCancelLayerIndex").val(layerIndex);
            }
        });

        // $("#accountActivityType").val(2); //向特约店申请为2，向天猫购买支付码为1

    });


    $("#btnHomePhoneLogon").click(function () {
        //popWindownBlue( "test");
        var account = $('#homeAccountPhone').val();

        var captcha = $("#LoginCaptcha").val();
        var SMScaptcha = $("#LoginSMSCaptcha").val();

        if (account === null || account === "") {
            popWindownBlue("请输入手机号码");
            return false;
        } else {
            var PhoneReg = /^(1[3|4|5|7|8|][0-9]{9})$/;
            if (!PhoneReg.test(account)) {
                popWindownBlue("请正确输入手机号码");
                return false;
            }
        }
        if (captcha === null || captcha === "") {
            popWindownBlue("请输入图片验证码");
            return false;
        }

        if (SMScaptcha === null || SMScaptcha === "") {
            popWindownBlue('请输入短信验证码');
            return false;
        }
        //sendCaptchaAndCheckImageCode(account, captcha, this)
        User.LogonByPhone(account, captcha, SMScaptcha, "BLMS");
    });

    $("#btnCenterPhoneLogon").click(function () {
        //popWindownBlue( "test");
        var account = $('#homeAccountPhone').val();

        var captcha = $("#LoginCaptcha").val();
        var SMScaptcha = $("#LoginSMSCaptcha").val();

        if (account === null || account === "") {
            popWindownBlue('请输入手机号码');
            return false;
        } else {
            var PhoneReg = /^(1[3|4|5|7|8|][0-9]{9})$/;
            if (!PhoneReg.test(account)) {
                popWindownBlue("请正确输入手机号码");
                return false;
            }
        }
        if (captcha === null || captcha === "") {
            popWindownBlue("请输入图片验证码");
            return false;
        }

        if (SMScaptcha === null || SMScaptcha === "") {
            popWindownBlue('请输入短信验证码');
            return false;
        }
        //sendCaptchaAndCheckImageCode(account, captcha, this)
        User.CenterLogonByPhone(account, captcha, SMScaptcha, "BLMS");
    });
    //====================注册规则新结束======================
    //========================================登录页面======================================================
    $("#btnLogonPageLogon").click(function () {
        var _account = $("#LogonPageAccount").val();
        var _passwd = $("#LogonPagePasswd").val();
        User.Logon(_account, _passwd, "", "HOME");
    });



    //========================================首页===================================================================
    //$("#homeSendCaptcha").click(function () {
    //    //首先校验手机号
    //    if ($("#homeAccount").val() === null || $("#homeAccount").val() === "") {
    //        //请输入手机号
    //        popWindownBlue( '请输入手机号');
    //        return false;
    //    } else {
    //        //$("#captchaErrorMessage").hide();
    //    }
    //    //校验是否重复点击
    //    if ($(this).attr("disabled")) {
    //        return false;
    //    }
    //    //更新剩余时长
    //    countDown(this, 5);
    //    //验证码发送
    //    sendCaptcha($("#homeAccount").val());
    //});

    //首页登录
    $("#btnHomeLogon").click(function () {
        var account = $('#homeAccount').val();
        var passwd = $('#homePasswd').val();
        var captcha = $("#homeCaptcha").val();
        if (account === null || account === "") {
            popWindownBlue('请输入手机/用户名/邮箱/会员卡号');
            return false;
        }
        if (passwd === null || passwd === "") {
            popWindownBlue("请输入密码");
            return false;
        }
        if (captcha === null || captcha === "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        User.Logon(account, passwd, captcha, "HOME");
    });

    //首页登录
    $("#btnCenterLogon").click(function () {
        var returnUrl = $("#returnUrl").val();
        var account = $('#homeAccount').val();
        var passwd = $('#homePasswd').val();
        var captcha = $("#homeCaptcha").val();
        if (account === null || account === "") {
            popWindownBlue('请输入手机号');
            return false;
        }
        if (passwd === null || passwd === "") {
            popWindownBlue("请输入密码");
            return false;
        }
        if (captcha === null || captcha === "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        var tempUrl = "HOME";
        if (returnUrl.indexOf("OrderChange") != -1) {
            tempUrl = returnUrl;
        }

        User.CenterLogon(account, passwd, captcha, returnUrl);
    });
    //首页Phone登陆



    //申请邮寄纸质杂志
    $("#btnHomeReserveApply").click(function () {
        var userName = $("#homeReserveDriveUserName").val();
        var mobile = $("#homeReserveDriveUserMobile").val();
        var postcode = $("#homeReserveDriveUserPostCode").val();
        var carType = $("#carType").val();
        var carTypeName = $("#carType").find("option:selected").text();

        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (mobile === null || mobile === "" || !phoneReg.test(mobile)) {
            popWindownBlue("请正确填写您的手机号");
            return false;
        }
        var emailReg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
        if (postcode === null || postcode === "" || !emailReg.test(postcode)) {
            popWindownBlue("请正确填写您的电子邮箱");
            return false;
        }
        News.magazineApply(userName, mobile, postcode, carType, carTypeName);
    });


    //首页在线订车
    $("#btnHomeCarReserveBuy").click(function () {
        var carType = $("#carType").find("option:selected").text();
        var province = $("#sltProvince_home_ordercar").val();
        var city = $("#sltCity_home_ordercar").val();
        var dealerId = $("#sltDealer_home_ordercar").val();
        var dealerName = $("#sltDealer_home_ordercar").find("option:selected").text();
        var userName = $("#homeCarReserveBuyUserName").val();
        var gender = $("input[name='homeCarReserveBuyGender']:checked").val();
        var email = $("#homeCarReserveBuyUserEmail").val();
        var mobile = $("#homeCarReserveBuyUserMobile").val();
        if (carType === null) {
            popWindownBlue("请填写订购车型");
            return false;
        }
        if (dealerId === null || dealerId == -1) {
            popWindownBlue("请选择经销商");
            return false;
        }
        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (mobile === null || mobile === "" || !phoneReg.test(mobile)) {
            popWindownBlue("请正确填写您的手机号码");
            return false;
        }
        Car.reserveBuy(carType, province, city, dealerId, dealerName, userName, gender, email, mobile);
    });
    //在线订车
    $("#btnReserveCarReserveBuy").click(function () {
        var carType = $("#carType").find("option:selected").text();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealerId = $("#sltDealer").val();
        var dealerName = $("#sltDealer").find("option:selected").text();
        var userName = $("#homeCarReserveBuyUserName").val();
        var gender = $("input[name='homeCarReserveBuyGender']:checked").val();
        var email = $("#homeCarReserveBuyUserEmail").val();
        var mobile = $("#homeCarReserveBuyUserMobile").val();
        if (carType === null) {
            popWindownBlue("请填写订购车型");
            return false;
        }
        if (dealerId === null || dealerId == -1) {
            popWindownBlue("请选择经销商");
            return false;
        }
        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }
        //var emailReg = new RegExp("/^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/");
        //if (email === null || email === "" || !emailReg.test(email)) {
        //    popWindownBlue("请正确填写您的邮箱");
        //    return false;
        //}
        var phoneReg = new RegExp("^1[0-9]{10}$"); 
        if (mobile === null || mobile === "" || !phoneReg.test(mobile)) {
            popWindownBlue("请正确填写您的手机号码");
            return false;
        }
       


        Car.reserveBuy(carType, province, city, dealerId, dealerName, userName, gender, email, mobile);
    });


    //预约维修保养
    $("#btnReserveMaintenance").click(function () {
        var carType = $("#carType").find("option:selected").text();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealerId = $("#sltDealer").val();
        var dealerName = $("#sltDealer").find("option:selected").text();
        var carNumber = $("#carReserveMaintenanceCarNumber").val();
        var frameNumber = $("#carReserveMaintenanceCarFrameNumber").val();
        var ridesNumber = $("#carReserveMaintenanceRidesNumber").val();
        var buyYears = $("#carReserveMaintenanceBuyYears").val();
        var r = $("input[name='carReserveMaintenanceMaintenance']:checked");
        var endTime = $("#carReserveMaintenanceEndTime").val();
        var userName = $("#carReserveMaintenanceUserName").val();
        var gender = $("input[name='carReserveMaintenanceGender']:checked").val();
        //var email = $("#userEmail").val();
        var mobile = $("#carReserveMaintenanceUserMobile").val();

        if (r.length === 0) {
            popWindownBlue("请选择服务项目");
            return false;
        }
        if (r.length === 2) {
            maintenance = 2;
        }
        if (r.length === 1) {
            maintenance = r.val();
        }

        if (dealerId === null || dealerId === "" || dealerId == -1) {
            popWindownBlue("请选择特约店");
            return false;
        }
        if (carType === null || carType === "") {
            popWindownBlue("请填写您的车型");
            return false;
        }
        //if (frameNumber == null || frameNumber == "") {
        //    popWindownBlue( "请正确填写您的车架号");
        //    return false;
        //}
        if (carNumber === null || carNumber === "") {
            popWindownBlue("请填写您的车牌号");
            return false;
        }
        //if (maintenance == null || maintenance == "") {
        //    popWindownBlue( "请选择服务项目");
        //    return false;
        //}
        if (endTime === null || endTime === "") {
            popWindownBlue("请正确选择您的预计到店时间");
            return false;
        }
        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (mobile === null || mobile === "" || !phoneReg.test(mobile)) {
            popWindownBlue("请正确填写您的手机号码");
            return false;
        }
        Car.reserveMaintenance(carType, province, city, dealerId, dealerName, carNumber, frameNumber, ridesNumber, buyYears, maintenance, endTime, userName, gender, mobile);
    });

    //Home 页面预约维保
    $("#btnReserveMaintenance_home").click(function () {
        var carType = $("#carType_reverse").find("option:selected").text();
        var province = $("#sltProvince_home").val();
        var city = $("#sltCity_home").val();
        var dealerId = $("#sltDealer_home").val();
        var dealerName = $("#sltDealer_home").find("option:selected").text();
        var carNumber = $("#carReserveMaintenanceCarNumber_home").val();
        var frameNumber = $("#carReserveMaintenanceCarFrameNumber_home").val();
        var ridesNumber = $("#carReserveMaintenanceRidesNumber_home").val();
        var buyYears = $("#carReserveMaintenanceBuyYears_home").val();
        var r = $("input[name='carReserveMaintenanceMaintenance_home']:checked");
        var endTime = $("#carReserveMaintenanceEndTime_home").val();
        var userName = $("#carReserveMaintenanceUserName_home").val();
        var gender = $("input[name='carReserveMaintenanceGender_home']:checked").val();
        //var email = $("#userEmail").val();
        var mobile = $("#carReserveMaintenanceUserMobile_home").val();


        if (r.length === 0) {
            popWindownBlue("请选择服务项目");
            return false;
        }
        if (r.length === 2) {
            maintenance = 2;
        }
        if (r.length === 1) {
            maintenance = r.val();
        }

        if (dealerId === null || dealerId === "" || dealerId == -1) {
            popWindownBlue("请选择特约店");
            return false;
        }
        if (carType === null || carType === "") {
            popWindownBlue("请填写您的车型");
            return false;
        }
        //if (frameNumber == null || frameNumber == "") {
        //    popWindownBlue( "请正确填写您的车架号");
        //    return false;
        //}
        if (carNumber === null || carNumber === "") {
            popWindownBlue("请填写您的车牌号");
            return false;
        }
        //if (maintenance == null || maintenance == "") {
        //    popWindownBlue( "请选择服务项目");
        //    return false;
        //}
        if (endTime === null || endTime === "") {
            popWindownBlue("请正确选择您的预计到店时间");
            return false;
        }
        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (mobile === null || mobile === "" || !phoneReg.test(mobile)) {
            popWindownBlue("请正确填写您的手机号码");
            return false;
        }
        Car.reserveMaintenance(carType, province, city, dealerId, dealerName, carNumber, frameNumber, ridesNumber, buyYears, maintenance, endTime, userName, gender, mobile);
    });

    //预约试驾
    $("#btnReserveDrive").click(function () {
        var carType = $("#carType").find("option:selected").text();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealerId = $("#sltDealer").val();
        var dealerName = $("#sltDealer").find("option:selected").text();
        var driveTime = $("#reserveDriveDriveTime").val();
        var userName = $("#reserveDriveUserName").val();
        var gender = $("input[name='reserveDriveGender']:checked").val();
        var mobile = $("#reserveDriveMobile").val();
        var planBuyTime = $("#reserveDrivePlanBuyTime").val();
        if (dealerId === null || dealerId === "" || dealerId == -1) {
            popWindownBlue("请选择特约经销商");
            return false;
        }
        if (driveTime === null || driveTime === "") {
            popWindownBlue("请正确输入试驾日期");
            return false;
        }
        if (userName === null || userName === "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }

        var phoneReg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !phoneReg.test(mobile)) {
            popWindownBlue("请输入正确的手机号格式。");
            return;
        }
        if (planBuyTime === null || planBuyTime === "") {
            popWindownBlue("请正确输入计划购车日期");
            return false;
        }
        Car.reserveDrive(carType, province, city, dealerId, dealerName, driveTime, userName, gender, mobile, planBuyTime);
    });

    //预约试驾
    $("#btnHomeReserveDrive").click(function () {
        var carType = $("#carType").find("option:selected").text();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealerId = $("#sltDealer").val();
        var dealerName = $("#sltDealer").find("option:selected").text();
        var driveTime = $("#homeReserveDriveDriveTime").val();
        var userName = $("#homeReserveDriveUserName").val().trim();
        var gender = $("input[name='homeReserveDriveGender']:checked").val()
        var mobile = $("#homeReserveDriveUserMobile").val().trim();
        var planBuyTime = $("#homeReserveDriveBuyTime").val();

        if (dealerId == null || dealerId == "" || dealerId == -1) {
            popWindownBlue("请选择特约经销商");
            return false;
        }
        if (driveTime == null || driveTime == "") {
            popWindownBlue("请正确选择您的预计到店时间");
            return false;
        }
        if (userName == null || userName == "") {
            popWindownBlue("请填写您的姓名");
            return false;
        }

        var phoneReg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !phoneReg.test(mobile)) {
            popWindownBlue("请输入正确的手机号格式。");
            return;
        }
        Car.reserveDrive(carType, province, city, dealerId, dealerName, driveTime, userName, gender, mobile, planBuyTime);
    });




    //=================================================注册页面============================================================
    $("#AccountRegisterAgreeRegister").click(function () {
        if ($(this).is(":checked")) {
            $("#btnRegisterAccountReg").removeClass("isenableclick");
        } else {
            $("#btnRegisterAccountReg").addClass("isenableclick");
        }
    });

    $("#IsAccountRegisterCarOwner").click(function () {
        //$("#registerSonataCarownerInfo").show();
        $("#accountregisterisownerchecked").show();
    });

    $("#NoAccountRegisterCarOwner").click(function () {
        //$("#registerSonataCarownerInfo").hide();
        $("#accountregisterisownerchecked").hide();
    });

    $("#AccountRegisterIsSonataUser").click(function () {
        if ($(this).is(":checked")) {
            $("#account_AccountRegisterIsSonataUser").show();
        } else {
            $("#account_AccountRegisterIsSonataUser").hide();
        }
    });

    //$("#zuzhijigouNo_account_radio").click(function () {
    //    $("#registerIdentityNo").val("");
    //    $("#registerVIN_Span").show();
    //    $("#registerPaperWork_Span").hide();
    //});
    //$("#registerIdentityNo_account_radio").click(function () {
    //    $("#registerIdentityNo").val("");
    //    $("#registerVIN_Span").hide();
    //    $("#registerPaperWork_Span").show();
    //});

    $("#btnInputPayNumberToActive_AccountRegister").click(function () {

        var mobile = $("#registerMobile").val();
        var captcha = $("#registerCaptcha").val();
        var passwd = $("#registerPasswd").val();
        var repasswd = $("#registerComfirmPasswd").val();
        var nickname = $("#registerNickname").val();
        var identityno = $("#registerIdentityNo").val();
        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var accountPayNumber = $("#accountPayNumber").val();
        var _payType = $("#accountActivityType").val();
        var registerVIN = $("#registerVIN").val();
        var returnUrl = $("#returnUrl").val();
        if (mobile == null || mobile == "") {
            popWindownBlue("请输入手机号");
            return false;
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        if (passwd == null || passwd == "" || repasswd == null || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        if (nickname == null || nickname == "") {
            popWindownBlue("请输入昵称");
            return false;
        }

        var identityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        var isCarOwner = 1;
        if ($("#IsAccountRegisterCarOwner").is(":checked")) {

            if (identityno == null || identityno == "") {
                popWindownBlue("请输入证件号");
                return false;
            }
            //if ($("#registerIdentityNo_account_radio").is(":checked") && !identityReg.test(identityno)) {
            //    popWindownBlue( "请正确输入证件号");
            //    return false;
            //}
            isCarOwner = 2;
            if ($("#AccountRegisterIsSonataUser").is(":checked")) {
                isCarOwner = 3;
                if (sltDealer == -1) {
                    popWindownBlue("请选择4s店");
                    return false;
                }
                if (_payType != null && _payType != "undefined") {
                    if (_payType == 1) {
                        if (accountPayNumber == null || typeof accountPayNumber == "undefined") {
                            popWindownBlue("请输入支付码");
                            return false;
                        }
                    }
                }
            }
        } else {
            identityno = "";
            sltDealer = "";
        }

        $("#btnApplyMemberToDealer_AccountRegister").css({ "background-color": "#075090", "border-color": "#075090" });
        $.ajax({
            url: "/PayFee/AccountPayNumberActive",
            type: "get",
            success: function (resultHtml) {
                layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['413x', '217x'],
                    content: resultHtml
                });

                //$("#AccountSonataActiveCancelLayerIndex").val(layerIndex);
            }
        });
    });

    $("#btnRegisterAccountReg").click(function () {
        if ($(this).hasClass("isenableclick")) {
            return false;
        }
        var mobile = $("#registerMobile").val();
        var captcha = $("#registerCaptcha").val();
        var passwd = $("#registerPasswd").val();
        var repasswd = $("#registerComfirmPasswd").val();
        var nickname = $("#registerNickname").val();
        var paperwork = $("#accountregisterUserPaperWork").val();
        var identityno = $("#registerIdentityNo").val();
        //var sltProvince = $("#sltProvince").val();
        //var sltCity = $("#sltCity").val();
        //var sltDealer = $("#sltDealer").val();
        //var accountPayNumber = $("#accountPayNumber").val();
        //var _payType = $("#accountActivityType").val();
        //var registerVIN = $("#registerVIN").val();
        var registerVIN = "";
        var returnUrl = $("#returnUrl").val();
        var source = $("#source").val();
        if (mobile == null || mobile == "") {
            popWindownBlue("请输入手机号");
            return false;
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        if (passwd == null || passwd == "" || repasswd == null || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        if (nickname == null || nickname == "") {
            popWindownBlue("请输入昵称");
            return false;
        }
        //customerType默认为-1，身份证类型为1，组织机构代码为2
        var customerType = 1;
        var identityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        var isCarOwner = 1;
        if ($("#IsAccountRegisterCarOwner").is(":checked")) {
            //if ($("#registerIdentityNo_account_radio").is(":checked")) {
            if (!paperwork || paperwork < 0) {
                popWindownBlue("请选择证件类型");
                return false;
            }
            //}
            if (identityno == null || identityno == "") {
                popWindownBlue("请输入证件号");
                return false;
            }
            if ($("#registerIdentityNo_account_radio").is(":checked")) {
                if (1 == paperwork) {
                    identityReg = /(^\d{15}$)|(^\d{17}([0-9]|X)$)/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (2 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{3,21}$|^(P\d{7})|(G\d{8})$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (3 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{7,21}$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
            }
            //if ($("#registerIdentityNo_account_radio").is(":checked") && !identityReg.test(identityno)) {
            //    popWindownBlue( "请正确输入证件号");
            //    return false;
            //}
            //取消个人与企业用户
            //if ($("#registerIdentityNo_account_radio").is(":checked")) {
            //    customerType = 1;
            //}
            //if ($("#zuzhijigouNo_account_radio").is(":checked")) {
            //    customerType = 2;
            //    if (!registerVIN) {
            //        popWindownBlue( "请输入VIN");
            //        return false;
            //    }
            //}
            isCarOwner = 2;
            //if ($("#AccountRegisterIsSonataUser").is(":checked")) {
            //    isCarOwner = 3;
            //    if (sltDealer == -1) {
            //        popWindownBlue( "请选择4s店");
            //        return false;
            //    }
            //    if (_payType != null && _payType != "undefined") {
            //        if (_payType == 1) {
            //            if (accountPayNumber == null || typeof accountPayNumber == "undefined") {
            //                popWindownBlue( "请输入支付码");
            //                return false;
            //            }
            //        }
            //    }
            //}
        }
        else {
            identityno = "";
            sltDealer = "";
        }
        //
        User.Register(mobile, captcha, passwd, repasswd, nickname, identityno, sltDealer, isCarOwner, _payType, accountPayNumber, registerVIN, customerType, paperwork, returnUrl, source);
    });

    $("#btnApplyMemberToDealer_AccountRegister").click(function () {
        var mobile = $("#registerMobile").val();
        var captcha = $("#registerCaptcha").val();
        var passwd = $("#registerPasswd").val();
        var repasswd = $("#registerComfirmPasswd").val();
        var nickname = $("#registerNickname").val();
        var identityno = $("#registerIdentityNo").val();
        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var accountPayNumber = $("#accountPayNumber").val();
        var _payType = $("#accountActivityType").val();
        var registerVIN = $("#registerVIN").val();
        var returnUrl = $("#returnUrl").val();
        if (mobile == null || mobile == "") {
            popWindownBlue("请输入手机号");
            return false;
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        if (passwd == null || passwd == "" || repasswd == null || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        if (nickname == null || nickname == "") {
            popWindownBlue("请输入昵称");
            return false;
        }

        var identityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        var isCarOwner = 1;
        if ($("#IsAccountRegisterCarOwner").is(":checked")) {
            if (identityno == null || identityno == "") {
                popWindownBlue("请输入证件号");
                return false;
            }
            if ($("#registerIdentityNo_account_radio").is(":checked") && !identityReg.test(identityno)) {
                popWindownBlue("请正确输入证件号");
                return false;
            }
            isCarOwner = 2;
            if ($("#AccountRegisterIsSonataUser").is(":checked")) {
                isCarOwner = 3;
                if (sltDealer == -1) {
                    popWindownBlue("请选择4s店");
                    return false;
                }
                if (_payType != null && _payType != "undefined") {
                    if (_payType == 1) {
                        if (accountPayNumber == null || typeof accountPayNumber == "undefined") {
                            popWindownBlue("请输入支付码");
                            return false;
                        }
                    }
                }
            }
        } else {
            identityno = "";
            sltDealer = "";
        }

        $("#accountActivityType").val(2); //向特约店申请为2，向天猫购买支付码为1
        $(this).css({ "background-color": "#E3E3E3", "border-color": "#aaa" });
        var _index = layer.tips('您已选择向特约店申请成为会员', this, {
            tips: [1, '#3595CC'],
            time: 4000
        });
        setTimeout(function () { layer.close(_index); }, 3000);
        return false;
    });







    $("#AccountRegisterSendCaptcha").click(function () {
        $("#registerMobileerrorred").hide();
        $("#registerImageCodeerrored").hide();

        var _mobile = $("#registerMobile").val();
        var _imageCode = $("#registerImageCode").val();
        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }
        //首先校验手机号
        if (!_mobile) {
            //请输入手机号
            $("#registerMobileerrorred").show();
            return false;
        }
        if (!_imageCode) {
            $("#registerImageCodeerrored").show();
            return false;
        }
        //验证码发送
        sendCaptchaByRegister(_mobile, _imageCode, this);
    });

    $("#IsAccountRegisterCarOwner").click(function () {
        if ($(this).is(":checked")) {
            $("#accountregisterisownerchecked").show();
        } else {
            $("#accountregisterisownerchecked").hide();
        }
    });
    $("#registerMobile").blur(function () {
        $("#registerMobileerrorred").hide();
        var reg = /^1[3|4|5|8|7|][0-9]\d{4,8}$/;
        var userName = $("#registerMobile").val();
        if (!userName || userName.length != 11 || !reg.test(userName)) {
            $("#registerMobileerrorred").html("请输入正确的手机号");
            $("#registerMobileerrorred").show();
           // $(this).focus();
            return false;
        }
        User.CheckUserName(userName);
    });

    $("#registerPasswd").blur(function () {
        accountRegisterPasswdCheck(this);
    });

    $("#homeAccountPhone").blur(function () {
        $("#homeAccountPhoneError").hide();
        var reg = /^1[3|4|5|8|7|][0-9]\d{4,8}$/;
        var Phone = $("#homeAccountPhone").val();
        if (!Phone == "") {
            if (!Phone || Phone.length != 11 || !reg.test(Phone)) {
                $("#homeAccountPhoneError").html("请输入正确的手机号");
                $("#homeAccountPhoneError").show();
                $(this).focus();
                return false;
            }
        }
        // User.CheckUserName(userName);
    });
    $("#registerIdentityNo").blur(function () {
        var registerIdentityNo = $("#registerIdentityNo").val();
        if (registerIdentityNo == "" || registerIdentityNo == null || registerIdentityNo == undefined) {
            return;
        }
        User.CheckIdentityNumber(registerIdentityNo);
    });

    //===============================================会员激活流程=================================================================
    //向特约店申请成为会员
    $("#btnApplyMemberToDealer").click(function () {
        Sonata.applyMemberToDealerSave();
        return false;
    });

    //输入激活码，成为会员
    $("#btnInputPayNumberToActive").click(function () {
        $.ajax({
            url: "/PayFee/PayNumberActive",
            type: "get",
            success: function (resultHtml) {
                layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['413x', '217x'],
                    content: resultHtml
                });

                // $("#SonataActiveCancelLayerIndex").val(layerIndex);
            }
        });
    });

    //选择支付宝支付
    $("#btnPayWayAliPay").click(function () {
        $("#PayWaySelectPayWay").val("alipay");
    });

    //选择微信支付
    $("#btnPayWayQQPay").click(function () {
        $("#PayWaySelectPayWay").val("qqpay");
    });

    //确认支付
    $("#btnPayWaySelectSave").click(function () {
        var _payWay = $("#PayWaySelectPayWay").val();
        if (_payWay == null || _payWay == "") {
            popWindownBlue("请选择支付方式");
            return false;
        }
        //1.打开支付页面
        //2.弹出层遮罩层
        var pagei= layer.open({
            type: 1,   //0-4的选择,
            title: false,
            border: [0],
            closeBtn: [0],
            shadeClose: false,
            area: ['560px', '280px'],

            content: '<\div style="width:460px; height:280px; background-color:#81BA25; color:#fff;"><\div style="padding:20px;">支付结果<\/div><\div><span><a id="PayWayPayComplete" >支付完成</a></span><span><a>选择其他支付方式</a></span><\/div><\/div>' //此处放了防止html被解析，用了\转义，实际使用时可去掉

        });
        $("#PayWayLayIndex").val(pagei);
    });

    $("#PayWayPayComplete").click(function () {
        var _payStatus = $("#PayWayPayStatus").val();
        if (_payStatus == "200") {
            window.location = "/PayFee/PaySuccess";
            return false;
        } else {
            window.location = "/PayFee/PayFailed";
            return false;
        }
    });

    $("#btnGoToSonataActiveStepTwo").click(function () {
        var _dealerId = $("#sltDealer").val();
        if (_dealerId == null || _dealerId == "" || parseInt(_dealerId) <= 0) {
            popWindownBlue("请选择经销商");
            return false;
        }
        var _cookieHelper = new Cookie();
        _cookieHelper.setCookie("activememberdealerId", _dealerId)
        window.location.href = "/Sonata/SonataActiveStepTwo";
    });

  
    //==========================================重置密码==============================================================
    $("#btnresetpasswddatasave").click(function () {
        var souce = $("#resetPasswdSource").val();
        var mobile = $("#resetPasswdMobile").val();
        var captcha = $("#resetPasswdCaptcha").val();
        var passwd = $("#resetPasswdPasswd").val();
        var repasswd = $("#reResetPasswdPasswd").val();


        if (mobile == null || mobile == "") {
            popWindownBlue("手机号码输入有误");
            return false;
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入验证码");
            return false;
        }

       
        var passReg = /^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{8,100}$/;
            // var passReg=/^[a-z0-9]+$/
            if (passwd.length < 8 || !passReg.test(passwd)) {
                popWindownBlue('密码必须8位以上数字和大小写字母组合');
                return false;
            }
        

        if (passwd != "" && passwd.length < 8) {
            popWindownBlue("请输入8位以上密码");
            return false;
        }


        if (passwd == "" || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码输入不一致，请重新输入");
            return false;
        }
        User.setPasswd(mobile, passwd, repasswd, captcha, souce);
    });

    $("#resetpasswdSendCaptcha").click(function () {
        $("#resetpasswdmobileerrormsg").hide();
        $("#resetpasswriamgecodeerror").hide();

        var _mobile = $("#resetPasswdMobile").val();
        var _imageCode = $("#resetPasswdImageCode").val();
        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }
        //首先校验手机号
        if (!_mobile) {
            //请输入手机号
            $("#resetpasswdmobileerrormsg").show();
            return false;
        }
        if (!_imageCode) {
            //请输入手机号
            $("#resetpasswriamgecodeerror").show();
            return false;
        }
        //验证码发送
        sendCaptchaFindPassWord(_mobile, _imageCode, this);
    });


    $("#resetPasswdMobile").blur(function () {
        var reg = /^1[3|4|5|8][0-9]\d{4,8}$/;
        var _userName = $(this).val();
        if (_userName != "") {
            if (!_userName || _userName.length != 11 || !reg.test(_userName)) {
                popWindownBlue("请正确输入手机号");
               // $(this).focus();
                return false;
            }
        }
    });


    //===========================================生活服务=========================================================
    //机场服务
    $("#btnSonataLiveReserve").click(function () {
        var source = $("#source").val();
        if (source == "" || source == undefined) {
            source = "blms";
        }

        $.ajax({
            url: "/Live/ReserveLayer?t=" + (new Date()).getTime() + "&source=" + source,
            type: "get",
            success: function (result) {
                if (result == null || result == "") {
                    popWindownBlue("您是注册用户或未登录，暂时无法预约该服务", function () {
                      //  window.location = "/Account/LogonPage?returnUrl=/Live/Index?source=" + source;
                    });
                    
                }
                else {
                  var pagei=  layer.open({
                        type: 1,   //0-4的选择,
                        title: false,
                        border: [0],
                        closeBtn: [0],
                        shadeClose: false,
                        area: ['560px', '520px'],
                        content: result
                    });
                    $("#SonataLiveReserveCloseCurPageIndex").val(pagei);
                }
            },
            error: function () {

            }
        });

    });

    //=================================一对一服务页面     =========================================================


    //==========================================活动页面===========================================================
    $("#btnMovementDetailSave").click(function () {
        var _movementId = $("#movementdetailmovementId").val();
        if (_movementId == null || _movementId == "") {
            popWindownBlue("活动无效");
            return false;
        }
        var _userid = $("#movementdetailuserid").val();
        if (_userid == null || _userid == "") {
            window.location = "/Account/LogonPage?returnUrl=/Mavement/Detail/" + _movementId;
            //登录
            return false;
        }
        $.ajax({
            url: "/Mavement/ApplyMovement",
            type: "post",
            data: { UserId: _userid, ActivitiesId: _movementId },
            dataType: "json",
            success: function (result) {
                if (result != null && result.code == "200") {
                    popWindownBlue("报名成功");
                    location.reload();
                    return false;
                }
            },
            error: function () {
                popWindownBlue("报名失败");
                return false;
            }
        });
    });







    //=============================================Sonata服务==============================================================

    //途胜专享
    $("#travelFormSave").click(function () {
        var carType = $("#carType").val();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealer = $("#sltDealer").val();
        var carNumber = $("#travelFormCarNumber").val();
        var vinNumber = $("#travelFormVinNumber").val();
        //形式里程
        var ridesNumber = $("#travelFormidesNumber").val();
        var buyYears = $("#travelFormBuyYears").val();
        var arriveTime = $("#travelFormArriveTime").val();
        var realName = $("#travelFormRealName").val();
        var gender = $("input[name='gender']").val();
        var mobile = $("#travelFormUserMobile").val();
        var description = $("#travelFormDescription").val();

        var source = $("#source").val();
        if (source == "" || source == undefined) {
            source = "blms";
        }

        if (carType == -1) {
            popWindownBlue("请选择订购车型");
            return false;
        }

        if (dealer == -1) {
            popWindownBlue("请选择经销商");
            return false;
        }
        if (carNumber === null || carNumber === "") {
            popWindownBlue("请输入车牌号码");
            return false;
        }
        if (ridesNumber == null || ridesNumber == "") {
            popWindownBlue("请填写行驶里程");
            return false;
        }
        if (arriveTime == null || arriveTime == "") {
            popWindownBlue("请选择预计到店日期");
            return false;
        }

        if (realName == null || realName == "") {
            popWindownBlue("请填写姓名");
            return false;
        }

        if (mobile == null || mobile == "") {
            popWindownBlue("请填写手机号");
            return false;
        }
        var reg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !reg.test(mobile)) {
            popWindownBlue("手机号格式不正确。");
            return false;
        }
        Sonata.travelFormSave(carType, province, city, dealer, carNumber, vinNumber, ridesNumber, buyYears, arriveTime, realName, gender, mobile, description, source);
    });


    //3年九次免费年检
    $("#freeMotFormSave").click(function () {
        var carType = $("#carType").val();
        var province = $("#sltProvince").val();
        var city = $("#sltCity").val();
        var dealer = $("#sltDealer").val();
        var carNumber = $("#freeMotFormCarNumber").val();
        var vinNumber = $("#freeMotFormVinNumber").val();
        //形式里程
        var ridesNumber = $("#freeMotFormidesNumber").val();
        var buyYears = $("#freeMotFormBuyYears").val();
        var arriveTime = $("#freeMotFormArriveTime").val();
        var realName = $("#freeMotFormRealName").val();
        var gender = $("input[name='gender']").val();
        var mobile = $("#freeMotFormUserMobile").val();
        var description = $("#freeMotFormDescription").val();

        var source = $("#source").val();
        if (source == "" || source == undefined) {
            source = "blms";
        }

        if (carType == -1) {
            popWindownBlue("请选择订购车型");
            return false;
        }

        if (dealer == -1) {
            popWindownBlue("请选择经销商");
            return false;
        }
        if (dealer == -1) {
            popWindownBlue("请选择订购车型");
            return false;
        }
        if (arriveTime == null || arriveTime == "") {
            popWindownBlue("请选择预计到店日期");
            return false;
        }
        if (ridesNumber == null || ridesNumber == "") {
            popWindownBlue("请填写里程数");
            return false;
        }

        if (carNumber == null || carNumber == "") {
            popWindownBlue("请填写车牌号");
            return false;
        }

        //if (vinNumber == null || vinNumber == "") {
        //    popWindownBlue( "请填写车架号");
        //    return false;
        //}
        if (realName == null || realName == "") {
            popWindownBlue("请填写姓名");
            return false;
        }

        if (mobile == null || mobile == "") {
            popWindownBlue("请填写手机号");
            return false;
        }
        var reg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !reg.test(mobile)) {
            popWindownBlue("手机号格式不正确。");
            return false;
        }


        Sonata.freeMotFormSave(carType, province, city, dealer, carNumber, vinNumber, ridesNumber, buyYears, arriveTime, realName, gender, mobile, description, source);
    });

    //hometohome服务
    $("#serviceToHomeFormSave").click(function () {

        var getPosition = $("#getPostion").val();

        if (getPosition == null || getPosition == "") {
            popWindownBlue("请选择取车地点");
            return false;
        }
        var getArray = getPosition.split(",");
        var returnPostion = $("#returnPostion").val();
        if (returnPostion == null || returnPostion == "") {
            popWindownBlue("请选择送车地点");
            return false;
        }
        var returnArray = returnPostion.split(",");
        var dealerId = $("#dealerId").val();
        var dealerName = $("#dealerName").val();
        var dealerAddress = $("#dealerAddress").val();
        var takeAddress = $("#getAddress").val();
        var takeLong = getArray[0];
        var takeLat = getArray[1];
        var returnAddress = $("#returnAddress").val();
        var returnLong = returnArray[0];
        var returnLat = returnArray[1];

        var carType = $("#carType").val();
        var carNumber = $("#serviceToHomeCarNumber").val();
        var vinNumber = $("#serviceToHomeVin").val();
        var takeTime = $("#serviceToHomeTakeTime").val();
        var sendTime = $("#serviceToHomeSendTime").val();
        var realName = $("#serviceToHomeRealName").val();
        var gender = $("input[name='gender']").val();
        var mobile = $("#serviceToHomeMbile").val();
        var description = $("#serviceToHomeDescription").val();
        var source = $("#source").val();
        if (source == "" || source == undefined) {
            source = "blms_pc_web";
        }

        if (dealerId == null || dealerId == "") {
            popWindownBlue("请选择经销商");
            return false;
        }
        if (carType == -1 || carType == "" || carType==null) {
            popWindownBlue("请选择车型");
            return false;
        }
        if (carNumber == null || carNumber == "") {
            popWindownBlue("请填写车牌号");
            return false;
        }
        //if (vinNumber == null || vinNumber == "") {
        //    popWindownBlue( "请填写车架号");
        //    return false;
        //}
        if (takeTime == null || takeTime == "") {
            popWindownBlue("请填写取车时间");
            return false;
        }
        if (sendTime == null || sendTime == "") {
            popWindownBlue("请填写送车时间");
            return false;
        }

        if (realName == null || realName == "") {
            popWindownBlue("请填写姓名");
            return false;
        }

        if (mobile == null || mobile == "") {
            popWindownBlue("请填写手机号");
            return false;
        }
        var reg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !reg.test(mobile)) {
            popWindownBlue("手机号格式不正确。");
            return false;
        }
        Sonata.serviceToHomeFormSave(dealerId, takeAddress, takeLong,
            takeLat, returnAddress, returnLong, returnLat, carType, carNumber, vinNumber, takeTime, sendTime, realName, gender, mobile, description, source);
    });

    //100日上门服务
    $("#goHomeFormSave").click(function () {
        var carType = $("#goHomeFormCarType").val();
        var carNumber = $("#goHomeFormNumber").val();
        var vinNumber = $("#goHomeFormVin").val();
        var takeTime = $("#goHomeFormTakeTime").val();
        var sendTime = $("#goHomeFormSendTime").val();
        var realName = $("#goHomeFormRealName").val();
        var gender = $("input[name='gender']").val();
        var mobile = $("#goHomeFormMbile").val();
        var description = $("#goHomeFormDescription").val();

        if (carNumber == null || carNumber == "") {
            popWindownBlue("请填写车牌号");
            return false;
        }
        //if (vinNumber == null || vinNumber == "") {
        //    popWindownBlue( "请填写车架号");
        //    return false;
        //}

        if (sendTime == null || sendTime == "") {
            popWindownBlue("请填写预计上门时间");
            return false;
        }
        if (realName == null || realName == "") {
            popWindownBlue("请填写姓名");
            return false;
        }

        if (mobile == null || mobile == "") {
            popWindownBlue("请填写手机号");
            return false;
        }
        var reg = new RegExp("^1[0-9]{10}$");

        if (mobile.length != 11 || !reg.test(mobile)) {
            popWindownBlue("手机号格式不正确。");
            return false;
        }

        Sonata.goHomeFormSave(carType, carNumber, vinNumber, takeTime, sendTime, realName, gender, mobile, description);
    });


    //===============================================商品详情页面=========================================================
    //减少数量
    $("#productDetailQutityReduce").click(function () {
        var quntity = $("#productDetailBuyProductQutity").val();
        if (quntity == null || quntity == "") {
            quntity = 1;
        }
        quntity = quntity - 1;
        if (quntity <= 0) {
            quntity = 1;
        }
        $("#productDetailBuyProductQutity").val(quntity);
    });
    //增加数量
    $("#productDetailQutityPlus").click(function () {
        var quntity = $("#productDetailBuyProductQutity").val();
       
        if ((quntity == null || quntity == "" || !parseInt(quntity)) && quntity!="0") {
            quntity = 1;
        }
        
        quntity = parseInt(quntity) + 1;
        var _productStock = $("#productStock").html();
        if (parseInt(quntity) > parseInt(_productStock)) {
            quntity = _productStock;
        }

        $("#productDetailBuyProductQutity").val(quntity);
    });

    //更改数量
    $("#productDetailBuyProductQutity").change(function () {
        //1.校验
        var _quantity = $(this).val();
        if ($(this).val() == null || $(this).val() == "" || !parseInt($(this).val()) || parseInt($(this).val()) < 0) {
            _quantity = 0;
        }
        var _productStock = $("#productStock").html();
        if (parseInt(_quantity) > parseInt(_productStock)) {
            _quantity = _productStock;
        }
        $(this).val(_quantity);
    });

    //更改对换方式
    //$("span[name='skuId']").click(function () {
    //    if (!$(this).hasClass("tracetype")) {
    //        $(this).siblings().removeClass("tracetype");
    //        $(this).addClass("tracetype");
    //    }
    //});

    //添加到购物车
    $("#btnAddProductToCart").click(function () {
        var _skuId = "";
        var _productDetailSkuItemVlaue = 0;
        var _productDetailSkuPayType = "";
        var _isSelectPayType = false;
        $("span[name='skuId']").each(function (i, obj) {
            if ($(obj).hasClass("tracetype")) {
                _isSelectPayType = true;
                _skuId = $(obj).attr("id");
                _productDetailSkuItemVlaue = $(obj).find(".productDetailSkuItemVlaue").html();
                _productDetailSkuPayType = $(obj).attr("paytype");
            }
        });
        if (!_isSelectPayType) {
            popWindownBlue('请先登录后购买');
            return false;
        }

        if (isSetColor) {
            if (_productcolor == '') {
                popWindownBlue('请选择商品颜色');
                return false;
            }
        }
        if (isSetType) {
            if (_producttype == '') {
                popWindownBlue('请选择商品类型');
                return false;
            }
        }

        if ($("#productDetailBuyProductQutity").val() == "0") {
            popWindownBlue("请设置商品数量");
            return false;
        }
        if (parseInt($("#productStock").text()) == 0) {
            popWindownBlue("库存不足， 请选择其他商品");
            return false;
        }
        var item = {
            skuId: _skuId,
            productId: $("#productDetailProductId").val(),
            productName: $("#productDetailProductName").val(),
            price: _productDetailSkuItemVlaue,
            quantity: $("#productDetailBuyProductQutity").val(),
            imgUrl: $("#productDetailBuyImgUrl").val(),
            payType: _productDetailSkuPayType,
            producttype: _producttype,
            productcolor: _productcolor

        };
        var addStatus = ShoppingCart.AddItem(item);
        console.info(addStatus);
        if (typeof addStatus != "undefined" && addStatus != null && addStatus == false) {
            return false;
        }
        //  $("#productdetail_addcart_success_layer").css("top", Math.round((document.documentElement.clientHeight - 200) / 2) + "px");
        // $("#productdetail_addcart_success_layer").css("left", Math.round((document.documentElement.clientWidth - 400) / 2) + "px");
        $("#productdetail_addcart_success_layer").show();
    });
    //立即兑换
    $("#btnOnceExchange").click(function () {
        //第一步：清空购物车
        ShoppingCart.RemoveAll();
        //第二步：向购物车添加数据
        var _skuId = "";
        var _productDetailSkuItemVlaue = 0;
        var _productDetailSkuPayType = "";
        var _isSelectPayType = false;
        $("span[name='skuId']").each(function (i, obj) {
            if ($(obj).hasClass("tracetype")) {
                _isSelectPayType = true;
                _skuId = $(obj).attr("id");
                _productDetailSkuItemVlaue = $(obj).find(".productDetailSkuItemVlaue").html();
                _productDetailSkuPayType = $(obj).attr("paytype");
            }
        });
        if (!_isSelectPayType) {
            popWindownBlue('请先登录后购买');
            return false;
        }
        if (isSetColor) {
            if (_productcolor == '') {
                popWindownBlue('请选择商品颜色');
                return false;
            }
        }
        if (isSetType) {
            if (_producttype == '') {
                popWindownBlue('请选择商品类型');
                return false;
            }
        }
        if ($("#productDetailBuyProductQutity").val() == "0") {
            popWindownBlue("请设置商品数量");
            return false;
        }
        if (parseInt($("#productStock").text())==0) {
            popWindownBlue("库存不足， 请选择其他商品");
            return false;
        }
        var item = {
            skuId: _skuId,
            productId: $("#productDetailProductId").val(),
            productName: $("#productDetailProductName").val(),
            price: _productDetailSkuItemVlaue,
            quantity: $("#productDetailBuyProductQutity").val(),
            imgUrl: $("#productDetailBuyImgUrl").val(),
            payType: _productDetailSkuPayType,
            producttype: _producttype,
            productcolor: _productcolor
        };
        ShoppingCart.AddItem(item);
        //第三步：去结算
        window.location = "/Order/Index";
        return false;
    });

    //========================================================购物车页面===============================================
    //增加数量
    $("a[id^='mycartItemProductQuantityPlus']").click(function () {
        var _mycartItemProductQuantity = $(this).parent().parent().find("input[id^='mycartItemProductQuantity']").val();
        if (_mycartItemProductQuantity == null || _mycartItemProductQuantity == "" || !parseInt(_mycartItemProductQuantity)) {
            _mycartItemProductQuantity = 0;
        }
        _mycartItemProductQuantity = parseInt(_mycartItemProductQuantity) + 1;
        $(this).parent().parent().find("input[id^='mycartItemProductQuantity']").val(_mycartItemProductQuantity);
        var singlePrice = $(this).parent().parent().parent().find("label[id^='mycartItemProductPrice']").html();

        $(this).parent().parent().parent().find("label[id^='mycartItemProductTotalPrice']").html(parseFloat(_mycartItemProductQuantity) * parseFloat(singlePrice));

        ShoppingCart.CartStatistics();
    });
    //减少数量
    $("a[id^='mycartItemProductQuantityReduce']").click(function () {
        var _mycartItemProductQuantity = $(this).parent().parent().find("input[id^='mycartItemProductQuantity']").val();
        if (_mycartItemProductQuantity == null || _mycartItemProductQuantity == "" || !parseInt(_mycartItemProductQuantity)) {
            _mycartItemProductQuantity = 1;
        }
        _mycartItemProductQuantity = parseInt(_mycartItemProductQuantity) - 1;
        if (_mycartItemProductQuantity < 0) {
            return false;
        }
        $(this).parent().parent().find("input[id^='mycartItemProductQuantity']").val(_mycartItemProductQuantity);

        var singlePrice = $(this).parent().parent().parent().find("label[id^='mycartItemProductPrice']").html();

        $(this).parent().parent().parent().find("label[id^='mycartItemProductTotalPrice']").html(parseFloat(_mycartItemProductQuantity) * parseFloat(singlePrice));
        ShoppingCart.CartStatistics();
    });

    //购物车数量改变
    $("input[id^='mycartItemProductQuantity']").change(function () {

        //1.校验
        if ($(this).val() == null || $(this).val() == "" || !parseInt($(this).val()) || parseInt($(this).val()) < 0) {
            $(this).val(0);
        }


        //2.更改购物车统计
        ShoppingCart.CartStatistics();

        //3.更改商品项总价
        var curItemProductQuantity = $(this).parent().parent().parent().find("input[id^='mycartItemProductQuantity']").val();
        var curItemProductPrice = $(this).parent().parent().parent().find("label[id^='mycartItemProductPrice']").html();
        $(this).parent().parent().parent().find("label[id^='mycartItemProductTotalPrice']").html(parseFloat(curItemProductQuantity) * parseFloat(curItemProductPrice));

    });

    //购物车全选按钮
    $("#mycartProductCheckAll").click(function () {
        var _allChecked = document.getElementById('mycartProductCheckAll').checked;
        var itemCheckList = document.getElementsByName('mycartProductCheckItem');
        for (var i = 0; i < itemCheckList.length; i++) {
            itemCheckList[i].checked = _allChecked;
        }
        document.getElementById('mycartBottomProductCheckAll').checked = _allChecked;

        ShoppingCart.CartStatistics();
    });
    //购物车底部全选按钮
    $("#mycartBottomProductCheckAll").click(function () {
        var _allChecked = document.getElementById('mycartBottomProductCheckAll').checked;
        var itemCheckList = document.getElementsByName('mycartProductCheckItem');
        for (var i = 0; i < itemCheckList.length; i++) {
            itemCheckList[i].checked = _allChecked;
        }
        document.getElementById('mycartProductCheckAll').checked = _allChecked;

        ShoppingCart.CartStatistics();
    });

    //选择框更改
    $("input[name='mycartProductCheckItem']").click(function () {
        var isHaveItemChecked = false;
        var itemCheckList = document.getElementsByName('mycartProductCheckItem');
        for (var i = 0; i < itemCheckList.length; i++) {
            if (itemCheckList[i].checked == true) {
                isHaveItemChecked = true;
            }
        }
        document.getElementById('mycartProductCheckAll').checked = isHaveItemChecked;
        document.getElementById('mycartBottomProductCheckAll').checked = isHaveItemChecked;

        ShoppingCart.CartStatistics();
    });


    //清空购物车
    $("#mycartRemoveAll").click(function () {
        ShoppingCart.RemoveAll();
        $(".mycartProductItem").remove();

        ShoppingCart.CartStatistics();
    });

    //删除购物车中的项
    $("#mycartRemoveItem").click(function () {
        $("input[name='mycartProductCheckItem']").each(function (i, obj) {
            if ($(obj).is(":checked")) {
                var item = {
                    skuId: $(obj).parent().parent().parent().find("input[id^='mycartItemProductSkuId']").val()
                };
                ShoppingCart.RemoveItem(item);
                $(obj).parent().parent().parent().remove();
            }
        });
        ShoppingCart.CartStatistics();
    });


    $("a[id^='mycartItemProductDeleteItem']").click(function () {
        var item = {
            skuId: $(this).parent().parent().find("input[id^='mycartItemProductSkuId']").val()
        }
        ShoppingCart.RemoveItem(item);
        $(this).parent().parent().remove();
        ShoppingCart.CartStatistics();
    });

    //去结算
    $("#mycartProductGoOrder").click(function () {
        var total = $("#surplusScore").val() * 1;

        //1.清空购物车
        ShoppingCart.RemoveAll();


        //2.购物车统计
        var _totalQuantity = 0;
        var _totalBlueBeanQuantity = 0;
        var _totalBlueBeanPrice = 0;
        var _totalIntegralQuantity = 0;
        var _totalIntegralPrice = 0;

        $(".mycartProductItem").each(function (i, obj) {
            var _productId = $(obj).find("input[id^='mycartItemCheckProductId']").val();
            var _skuId = $(obj).find("input[id^='mycartItemProductSkuId']").val();
            var _productName = $(obj).find("label[id^='mycartItemProductSkuName']").html();
            var _totalprice = $(obj).find("label[id^='mycartItemProductTotalPrice']").html();
            var _price = $(obj).find("label[id^='mycartItemProductPrice']").html();
            var _quantity = $(obj).find("input[id^='mycartItemProductQuantity']").val();
            var _imgUrl = $(obj).find(".car_pro>img").attr("src");
            var _payType = $(obj).find("label[id^='mycartItemProductType']").attr("title");
            //添加颜色类型
            var _color = $(obj).find("label[id^='mycartItemProductColor']").html();

            //   popWindownBlue( _color);
            //  popWindownBlue( $(obj).find("label[id^='mycartItemProductColor']"));
            //   popWindownBlue( $(obj).find("label[id^='mycartItemProductColor']")[0]);
            var _type = $(obj).find("label[id^='mycartItemProductCarType']").html();
            //添加颜色类型
            _totalQuantity = parseInt(_totalQuantity) + parseInt(_quantity);
            if (_payType == "Integral") {
                _totalIntegralPrice = parseFloat(_totalIntegralPrice) + parseFloat(_price) * parseFloat(_quantity);
                _totalIntegralQuantity = parseInt(_totalIntegralQuantity) + parseInt(_quantity);
            } else {
                _totalBlueBeanPrice = parseFloat(_totalBlueBeanPrice) + parseFloat(_price) * parseFloat(_quantity);
                _totalBlueBeanQuantity = parseInt(_totalBlueBeanQuantity) + parseInt(_quantity);
            }
            var productItem = {
                productId: _productId,
                skuId: _skuId,
                productName: _productName,
                price: _price,//单个商品的价格
                quantity: _quantity,//单个sku的数量
                imgUrl: _imgUrl,
                blueBean: 0,
                Integral: _price,
                payType: _payType,
                producttype: _type,
                productcolor: _color
            };
            ShoppingCart.cart.productList.push(productItem);
        });

        //3.绑定购物车（第二和第三步可以直接在第二步中将ShoppingCart.cart.productList.push(productItem);替换成ShoppingCart.AddItem(productItem)）
        ShoppingCart.cart.totalQuantity = _totalQuantity;
        ShoppingCart.cart.totalPrice = 0;
        ShoppingCart.cart.totalProductPrice = 0;
        ShoppingCart.cart.totalBlueBeanQuantity = _totalBlueBeanQuantity;
        ShoppingCart.cart.totalBlueBean = _totalBlueBeanPrice;
        ShoppingCart.cart.totalIntegralQuantity = _totalIntegralQuantity;
        ShoppingCart.cart.totalIntegral = _totalIntegralPrice;

        //4.将购物车保存到cookie
        ShoppingCart.SaveCart();

        //5.校验购物车中是否存在数据，如果不存在数据则直接返回并提示
        var myCurCart = ShoppingCart.MyCart();
        if (myCurCart == null || myCurCart.totalQuantity <= 0) {
            popWindownBlue("购物车中没有商品，请再逛逛哦~");
            return false;
        }
        //   if (total < _totalIntegralPrice) {
        //   popWindownBlue( "您的积分不足");
        // return false;
        //}

        //6.判断用户是否登录，如果用户未登录则先登录，登录成功则进入订单页面
        if (mycartUserLogonStatus) {
            //6.跳转到订单确认页面
            window.location = "/Order/Index";
            return false;
        } else {
            //如果未登录，则到登录页面，登录成功进入订单页面
            window.location = "/Account/LogonPage?returnUrl='/Order/Index'";
            return false;
        }

    });


    //======================================订单确认页面==========================================================
    //添加地址
    $("#btnOrderAddressCreateNewAddress").click(function () {
        if ($("#OrderAddressCreateNewAddress").is(":hidden")) {
            clearNewAddressTable();
            $("#OrderAddressCreateNewAddress").show();
            return false;
        } else {
            if ($("#myOrderAddressReceiveName").val() != "") {
                clearNewAddressTable();
            } else {
                $("#OrderAddressCreateNewAddress").hide();
            }
            return false;
        }

    });

    //重置邮寄地址表单
    function clearNewAddressTable() {
        $("#myAddress_ID").val("");
        $("#myOrderAddressReceiveName").val("");
        $("#myOrderAddressProvince").val("");
        $("#myOrderAddressCity").val("");
        $("#myOrderAddressCounty").val("");
        $("#myOrderAddressDetail").val("");
        $("#myOrderAddressZipCode").val("");
        $("#myOrderAddressPhone").val("");
        $("#myOrderAddressIsDefault").attr("checked", false);
    }

    //编辑地址
    $("a[name='btnEditAddress']").click(btnEditAddress_click);
    function btnEditAddress_click() {
        //锚点到焦点区域
        $("html,body").animate({ scrollTop: $("#btnOrderAddressCreateNewAddress").offset().top - 100 }, 1000)

        //重置所有连接状态
        $("a[name='btnEditAddress']").each(function () {
            $(this).show();
        });
        $("a[name='btnCancelAddress']").each(function () {
            $(this).hide();
        });

        //转换“编辑”“取消”的显示隐藏
        $(this).hide();
        $(this).parent().find("a[name='btnCancelAddress']").first().show();

        //显示编辑区域
        $("#OrderAddressCreateNewAddress").show();

        //取得当前地址信息
        var address_ID = $(this).parent().find("input[name='address_ID']").first().val();
        var address_ReceiveName = $(this).parent().find("input[name='address_ReceiveName']").first().val();
        var address_Province = $(this).parent().find("input[name='address_Province']").first().val();
        var address_City = $(this).parent().find("input[name='address_City']").first().val();
        var address_County = $(this).parent().find("input[name='address_County']").first().val();
        var address_Detail = $(this).parent().find("input[name='address_Detail']").first().val();
        var address_ZipCode = $(this).parent().find("input[name='address_ZipCode']").first().val();
        var address_Phone = $(this).parent().find("input[name='address_Phone']").first().val();
        var address_IsDefault = $(this).parent().find("input[name='myAddressAddress']").attr("checked");

        $("#myAddress_ID").val(address_ID);
        $("#myOrderAddressReceiveName").val(address_ReceiveName);
        $("#myOrderAddressProvince option[title=" + address_Province + "]").attr("selected", true).trigger('change');
        window.setTimeout(function () {
            $("#myOrderAddressCity option[title=" + address_City + "]").attr("selected", true).change();
            window.setTimeout(function () {
                $("#myOrderAddressCounty option[title=" + address_County + "]").attr("selected", true);
            }, 100);
        }, 100);
        $("#myOrderAddressDetail").val(address_Detail);
        $("#myOrderAddressZipCode").val(address_ZipCode);
        $("#myOrderAddressPhone").val(address_Phone);
        if (address_IsDefault == "checked")
            $("#myOrderAddressIsDefault").attr("checked", true);
        else
            $("#myOrderAddressIsDefault").attr("checked", false);
        return false;
    }


    //取消编辑
    $("a[name='btnCancelAddress']").click(btnCancelAddress_click);
    function btnCancelAddress_click() {
        //转换“编辑”“取消”的显示隐藏
        $(this).hide();
        $(this).parent().find("a[name='btnEditAddress']").first().show();
        //隐藏编辑区域
        $("#OrderAddressCreateNewAddress").hide(); return false;
    }

    //保存用户收货地址
    $("#btnOrderAddressSaveNewAddress").click(function () {
        var _ID = $("#myAddress_ID").val();
        var _userId = $("#myOrderAddressUserId").val();
        var _receiveName = $("#myOrderAddressReceiveName").val();
        var _phone = $("#myOrderAddressPhone").val();
        var _ProvinceText = $("#myOrderAddressProvince").find("option:selected").text();
        var _ProvinceValue = $("#myOrderAddressProvince").val();
        var _Province = $("#myOrderAddressProvince").find("option:selected").attr("title");
        var _CityText = $("#myOrderAddressCity").find("option:selected").text();
        var _CityValue = $("#myOrderAddressCity").val();
        var _City = $("#myOrderAddressCity").find("option:selected").attr("title");
        var _CountyText = $("#myOrderAddressCounty").find("option:selected").text();
        var _CountyValue = $("#myOrderAddressCounty").val();
        var _County = $("#myOrderAddressCounty").find("option:selected").attr("title");
        var _PCC = _ProvinceText + _CityText + _CountyText;
        var _ZipCode = $("#myOrderAddressZipCode").val();
        var _Detail = $("#myOrderAddressDetail").val();
        var _IsDefault = true;
        if (!$("#myOrderAddressIsDefault").is(":checked")) {
            _IsDefault: false;
        }

        if ($.trim(_receiveName) == "") {
            popWindownBlue("请填写收货人信息");
            return false;
        }
        if (_Province == "请选择" || _CityText == "请选择") {
            popWindownBlue("请选择所在地区");
            return false;
        }
        if ($.trim(_Detail) == "") {
            popWindownBlue("请填写收货人详细地址");
            return false;
        }
        if ($.trim(_phone) == "") {
            popWindownBlue("请填写收货人联系电话");
            return false;
        }
        var reg = /^1[3|4|5|8|7|][0-9]\d{4,8}$/;
       
        if (_phone.length != 11 || !reg.test(_phone)) {
            popWindownBlue("请正确填写收货人联系电话");
                return false;
            }
        

        $.ajax({
            url: "/User/AddAddress",
            type: "post",
            dataType: "json",
            data: { ID: _ID, UserID: _userId, ReceiveName: _receiveName, Phone: _phone, Province: _Province, City: _City, County: _County, PCC: _PCC, ZipCode: _ZipCode, Detail: _Detail, IsDefault: _IsDefault },
            success: function () {
                $("#OrderAddressCreateNewAddress").hide();
                $.ajax({
                    url: "/User/AddressList?timestamp=" + (new Date()).valueOf(),
                    type: "get",
                    data: { userId: $("#myOrderAddressUserId").val() },
                    success: function (result) {
                        $("#myOrderAddressList").html("");
                        $("#myOrderAddressList").html(result);
                        $("a[name='btnEditAddress']").click(btnEditAddress_click);
                        $("a[name='btnCancelAddress']").click(btnCancelAddress_click);
                    }
                });
            },
            error: function (result) {
                popWindownBlue("error");
            }
        });
    });
    // 提交订单
    $("#btnConfirmOrderAddOrder").click(function () {
        var isCard = $("#isProductCardType").val();
        var receiveAddressId = 0;
        var cartItemIds = [];
        if (isCard == 1) {
            if (typeof ($("#myOrderAddressList input[name='myAddressAddress']:checked").val()) == "undefined") {
                popWindownBlue("请先添加收货地址")
                return false;
            }
        }
            $("input[id^='myshoppingcartitemid']").each(function (i, obj) {
                cartItemIds.push($(obj).val());
            });
            if (cartItemIds.length <= 0) {
                popWindownBlue("请您请先填写收货地址");
                return false;
            }
            if (isCard == 1) {
                receiveAddressId = $("input[name='myAddressAddress']:checked").val();
            }
       
        $.ajax({
            url: "/Order/AddOrder",
            type: "post",
            data: { addressId: receiveAddressId, shopids: cartItemIds.join(',') },
            dataType: "json",
            success: function (result) {
                //订单提交清除购物车
                ShoppingCart.RemoveAll();
                if (result == null || result.code == "" || result.code == "400") {
                    popWindownBlue("系统异常");
                    return false;
                }
                if (result.code == "401") {
                    popWindownBlue("数据异常");
                    return false;
                }
                if (result.code == "402") {
                    popWindownBlue("帐号登录异常", function () {
                        window.location = "/Account/LogonPage?returnUrl=/Order/Index";
                       
                    });
                    return false;
                }
                if (result.code == "403") {
                    popWindownBlue(result.msg);
                    return false;
                }
                if (result.code == "904") {
                    popWindownBlue(result.msg, function () {
                        window.location = "/Order/MyOrders";
                    });
                    return false;
                }
                if (result.code == "404") {
                    popWindownBlue(reuslt.msg, function () {
                        window.location = "/Order/MyOrders";
                        
                    });
                    return false;
                }
                if (result.code == "200") {
                    if (isCard == 1) {
                        popWindownBlue("订单支付成功", function () {
                            window.location = "/Order/MyOrders";

                        });
                    } else {
                        popWindownBlue("您已成功兑换【" + $(".mar_t_10").html() + "】，电子券将以短信形式下发到您的手机上，请注意查收！", function () {
                            window.location = "/Order/MyOrders";

                        });
                    }
                    return false;
                }
            },
            error: function (err) { }
        });
    });

    //==========================================个人中心页面==============================================
    $("#mycenter_mycar_tocheck_carowner").click(function () {
        //  popWindownBlue( "hello");
        $("#ToCheckCarowner").css("display", "block");
        $(".zzDivBox").css('display', 'block');
        //$.ajax({
        //    url: "/MyCenter/ToCheckCarowner",
        //    type: "get",
        //    success: function (resultHtml) {
        //        var layerIndex = $.layer({
        //            type: 1,   //0-4的选择,
        //            title: false,
        //            border: [0],
        //            closeBtn: [0],
        //            shadeClose: false,
        //            area: ['460px', '280px'],
        //            page: {
        //                html: resultHtml //此处放了防止html被解析，用了\转义，实际使用时可去掉
        //            }
        //        });
        //        $("#mycenter_mycar_tocheckcarowner_layer_layerIndex").val(layerIndex);
        //    }
        //  });

    });

    //个人中心激活方式
    $("#mycenter_progress_toapplymember").click(function () {
        $.ajax({
            url: "/MyCenter/CarownerActive",
            type: "get",
            success: function (resultHtml) {
                var  layerIndex=  layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['460px', '280px'],
                    content: resultHtml
                });
                $("#mycenter_carowneractive_layer_layIndex").val(layerIndex);
            }
        });
    });

    //向天猫买券申请成为会员
    $("#btnMycenterInputPayNumberToActive").click(function () {
        $.ajax({
            url: "/PayFee/PayNumberActive",
            type: "get",
            success: function (resultHtml) {
             var layerIndex=   layer.open({
                    type: 1,   //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0],
                    shadeClose: false,
                    area: ['460px', '280px'],
                    content: resultHtml
                });

               $("#SonataActiveCancelLayerIndex").val(layerIndex);
            }
        });
    });

    //个人中心-账户设置-发送验证码
    $("#mycenterresetpasswdsendcaptcha").click(function () {
        //首先校验手机号
        var _mobile = $("#mycenterresetpasswdmobile").val();
        var _imageCode = $("#mycenterresetpasswdImageCode").val();
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (!_mobile || !phoneReg.test(_mobile)) {
            popWindownBlue('请输入手机号');
            return false;
        }
        if (!_imageCode) {
            popWindownBlue('请输入图形验证码');
            return false;
        }
        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }
        sendCaptchaChangePassword(_mobile, _imageCode, this);
    });

    //个人中心-账户设置-修改密码
    $("#mycenterresetpasswdsave").click(function () {
        var _captcha = $("#mycenterresetpasswdcaptcha").val();
        var _phonenumber = $("#mycenterresetpasswdmobile").val();
        var _passwd = $("#mycenterresetpasswdpasswd").val();
        var _confirmpasswd = $("#mycenterresetpasswdconfirmpasswd").val();
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (_phonenumber == null || _phonenumber == "" || !phoneReg.test(_phonenumber)) {
            popWindownBlue("请正确输入手机号");
            return false;
        }
        if (_passwd != '' && _passwd.length < 8) {
            popWindownBlue('请输入8位以上密码');
            return false;
        }

        var passReg = /^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{8,100}$/;
        // var passReg=/^[a-z0-9]+$/
        if (_passwd.length < 8 || !passReg.test(_passwd)) {
            popWindownBlue('密码必须8位以上数字和大小写字母组合');
            return false;
        }
        if (_passwd == null || _passwd == "" || _confirmpasswd == null || _confirmpasswd == "" || _passwd != _confirmpasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        $.ajax({
            url: "/MyCenter/MyCenterResetPasswdSave",

            type: "post",
            data: { captcha: _captcha, PhoneNumber: _phonenumber, Password: _passwd },
            dataType: "json",
            success: function (result) {
                if (result == null || result.code == "" || result.code == "400") {
                    popWindownBlue("密码修改失败");
                    return false;
                }
                if (result.code == "200") {
                    popWindownBlue("密码修改成功", function () {
                        window.location = "/MyCenter/Index?pageName=MyCenter";
                       
                    });
                    return false;
                  
                }
                if (result.code == "402") {
                    popWindownBlue("验证码失败");
                    return false;
                }
                if (result.code == "401") {
                    popWindownBlue("用户账号登陆异常");
                    return false;
                }
            },
            error: function () {

            }
        });
    });

    //验证用户
    $("#setbaseinfoUserNickname").blur(function () {
        var _nickname = $("#setbaseinfoUserNickname").val();
        if (_nickname == "") {
            popWindownBlue("用户名不能为空");
            return false;
        }
        var _nicknamebit = _nickname.replace(/[^\x00-\xff]/g, "**");
        if (_nickname != "" && _nicknamebit.replace(/[^\x00-\xff]/g, "**").length > 12) {
            popWindownBlue("用户名不能超过6个汉字")
            return false;
        }
        var passreg = /^[0-9]*$/
        if (passreg.test(_nickname.trim()) && _nickname != "") {
            popWindownBlue("用户名不能纯数字");
            return false;
        }
        
        return true;
    });
    //保存个人信息
    $("#mycentersetbaseinfosave").click(function () {
        var _faceimage = $("#faceimage").val();
        var _nickname = $("#setbaseinfoUserNickname").val();

        var _nicknamebit = _nickname.replace(/[^\x00-\xff]/g, "**");
        if (_nickname != "" && _nicknamebit.replace(/[^\x00-\xff]/g, "**").length > 12) {
            popWindownBlue("用户名不能超过6个汉字")
            return;
        }
        var passreg = /^[0-9]*$/
        if (passreg.test(_nickname))
        {
            popWindownBlue("用户名不能纯数字");
            return false;
        }
    ///var passReg = /^(([a-z]+[0-9]+)|([0-9]+[a-z]+))[a-z0-9]*$/i;


        var _realname = $("#setbaseinfoUserRealName").val();
        var _paperwork = $("#setbaseinfoUserPaperWork").val();
        var _identitynumber = $("#setbaseinfoUserIdentityNumber").val();

        if (!_paperwork == "" || !_identitynumber == "") {
            if (1 == _paperwork) {
                identityReg = /(^\d{15}$)|(^\d{17}([0-9]|X)$)/;
                if (!identityReg.test(_identitynumber)) {
                    popWindownBlue("请正确输入证件号码");
                    return false;
                }
            }
            if (2 == _paperwork) {
                identityReg = /^[a-zA-Z0-9]{3,21}$|^(P\d{7})|(G\d{8})$/;
                if (!identityReg.test(_identitynumber)) {
                    popWindownBlue("请正确输入证件号码");
                    return false;
                }
            }
            if (3 == _paperwork) {
                identityReg = /^[a-zA-Z0-9]{7,21}$/;
                if (!identityReg.test(_identitynumber)) {
                    popWindownBlue("请正确输入证件号码");
                    return false;
                }
            }
        }

        var _birthday = $("#setbaseinfoUserBirthday").val();
        var _email = $("#setbaseinfoEmail").val();
        if (_email != "" && _email.length > 50) {
            popWindownBlue("邮箱50字以内");
            return false;
        }
        var _gender = 1;
        var _genderName = "男";
        if ($("#setbaseinfoUserGenderWomen").is(":checked")) {
            _gender = 0;
            var _genderName = "女";
        }
        var _provency = $("#myOrderAddressProvince option:selected").text();
        var _city = $("#myOrderAddressCity option:selected").text();
        var _area = $("#myOrderAddressCounty option:selected").text();
        var _address = $("#setbaseinfoAddress").val();

        if (_email != "" && _email.length > 100) {
            popWindownBlue("地址100字以内");
            return false;
        }
        var _mainContact = "1";
        if ($("#setbaseinfoUserMainContactEmail").is(":checked")) {
            _mainContact = "2";
        }
        var _mainTelePhone = $("#setbaseinfoUserMainTelePhone").val();
        var _telephone = $("#setbaseinfoUserTelePhone").val();
        var _transactionTime = $("#setbaseinfoUserTransactionTime").val();
        var _industry = $("#setbaseinfoUserIndustry").val();
        var _job = $("#setbaseinfoUserJob").val();
        var _isMarriage = 1;
        if ($("#setbaseinfoUserIsMarriageNo").is(":checked")) {
            _isMarriage = 0;
        }
        var _marriageday = $("#setbaseinfoUserMarriageDay").val();
        var _educational = $("#setbaseinfoUserEducational").val();
        var _remark = $("#setbaseinfoUserRemark").val();
        $.ajax({
            url: "/MyCenter/SetBaseInfoSave",
            type: "post",
            data: {
                FaceImage: _faceimage,
                NickName: _nickname,
                RealName: _realname,
                PaperWork: _paperwork,
                IdentityNumber: _identitynumber,
                Birthday: _birthday,
                Email: _email,
                Gender: _gender,
                GenderName: _genderName,
                Provency: _provency,
                City: _city,
                Area: _area,
                Address: _address,
                MainContact: _mainContact,
                MainTelePhone: _mainTelePhone,
                TelePhone: _telephone,
                TransactionTime: _transactionTime,
                Industry: _industry,
                Job: _job,
                IsMarriage: _isMarriage,
                MarriageDay: _marriageday,
                Educational: _educational,
                Remark: _remark
            },
            dataType: "json",
            success: function (result) {
                if (result == null || result.code == "" || result.code == "400") {
                    popWindownBlue("保存失败");
                    return false;
                }
                if (result.code == "200") {
                    popWindownBlue("保存成功", function () {
                        window.location = "/MyCenter/Index";
                       
                    });
                    return false;
                }
                if (result.code == "401") {
                    popWindownBlue(result.msg);
                }
            },
            error: function () { }
        });
    });

    //个人中心-账户设置-发送验证码
    $("#mycenterresetpasswdsendsms_code").click(function () {
        //首先校验手机号
        var phoneReg = new RegExp("^1[0-9]{10}$");
        var _phonenumber = $("#new_mobile").val();
        var _imageCode = $("#mycentersetaccountImageCode").val();
        if (_phonenumber == null || _phonenumber == "" || !phoneReg.test(_phonenumber)) {
            popWindownBlue("请正确输入手机号");
            return false;
        }

        var Resultcode = "";
        $.ajax({
            url: "/MyCenter/IsExist_Mobile",
            type: "post",
            data: { mobile: _phonenumber },
            dataType: "json",
            async: false,
            success: function (result) {
                Resultcode = result.code;
            },
            error: function () { }
        });

        if (Resultcode == null || Resultcode == "") {
            popWindownBlue("验证手机号失败");
            return false;
        }
        if (Resultcode != "200") {
            popWindownBlue("账号已存在");
            return false;
        }
        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }
        if (!_imageCode) {
            popWindownBlue("请输入图形验证码");
            return false;
        }
        sendCaptchaAndCheckImageCode(_phonenumber, _imageCode, this);

    });
    //保存账户设置
    $("#mycentermobilesave").click(function () {
        var _phonenumber = $("#new_mobile").val();
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (_phonenumber == null || _phonenumber == "" || !phoneReg.test(_phonenumber)) {
            popWindownBlue("请正确输入手机号");
            return false;
        }
        $.ajax({
            url: "/MyCenter/SetAccountSave",
            type: "post",
            data: { mobile: _phonenumber, sms_code: $("#sms_code").val() },
            dataType: "json",
            success: function (result) {
                if (result == null || result.code == "") {
                    popWindownBlue("保存失败");
                    return false;
                }
                if (result.code == "400") {
                    popWindownBlue("账号登陆异常");
                    return false;
                }
                if (result.code == "401") {
                    popWindownBlue("手机号已存在");
                    return false;
                }
                if (result.code == "402") {
                    popWindownBlue("验证码失败");
                    return false;
                }
                if (result.code == "200") {
                    popWindownBlue("保存成功", function () {
                        window.location = "/MyCenter/Index";
                        
                    });
                    return false;
                }
            },
            error: function () { }
        });
    });
    //=================================================================================================

    //==========================================问卷调查页面==============================================
    $(".Questionnaire #btnQuestionnaireSubmit").unbind("click").click(function () {
        //获取必答题    
        var requiredQ = $(".Questionnaire p[data-qid][data-type][data-required='True']");
        //用户编号
        var id = $(".Questionnaire #inputUserID").val();
        //昵称
        var nickName = $(".Questionnaire #inputUserNickName").val();
        //问卷编号
        var qid = $(".Questionnaire #input_hidden_QuestionnaireId").val();
        var userEmail = $(".Questionnaire #inputUserEmail").val();
        var linkSource = $(".Questionnaire #inputLinkSource").val();
        var linkFrom = $(".Questionnaire #inputLinkFrom").val();
        var result = true;

        //验证是否完成了必答题
        requiredQ.each(function () {
            var qid = $(this).attr("data-qid").toString();
            var type = $(this).attr("data-type").toString();
            //判断单选
            switch (type) {
                case "0"://单选
                case "2"://判断
                    {
                        var radios = $(".Questionnaire input[type='radio'][name='" + qid + "']:checked");
                        if (radios.length == 0) {
                            result = false;
                            break;
                        }
                        var errorCount = 0;
                        $.each(radios, function () {
                            var _id = $(this).attr("id");
                            var _qid = $(this).attr("name");
                            if ($(this).attr("data-type") == 4) {
                                var _qitaVal = $(".Questionnaire input[type='text'][data-qid='" + _qid + "'][data-for='" + _id + "'][data-qtype='after_fill']").val();
                                if (_qitaVal == null || _qitaVal == "")
                                { errorCount++; }
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "1"://复选
                    {
                        var checkboxs = $(".Questionnaire input[type='checkbox'][name='" + qid + "']:checked");
                        if (checkboxs.length == 0) {
                            result = false;
                            break;
                        }
                        var errorCount = 0;
                        $.each(checkboxs, function () {
                            var _id = $(this).attr("id");
                            var _qid = $(this).attr("name");
                            if ($(this).attr("data-type") == 4) {
                                var _qitaVal = $(".Questionnaire input[type='text'][data-qid='" + _qid + "'][data-for='" + _id + "'][data-qtype='after_fill']").val();
                                if (_qitaVal == null || _qitaVal == "")
                                { errorCount++; }
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "3"://反馈意见
                    {
                        var text = $(".Questionnaire textarea[name='userSuggest'][placeholder][data-qid='" + qid + "']");
                        if (text.val() == null || text.val() == "") {
                            result = false;
                        }
                        break;
                    }
                case "4"://矩阵单选
                    {
                        var childCount = $(".Questionnaire label[data-qid='" + qid + "']").length;
                        var firstChildQ = $(".Questionnaire label[data-qid='" + qid + "']:first");
                        var tempCount = firstChildQ.parentsUntil("table").find("input[type='radio']:checked").length;
                        if (tempCount < childCount) {
                            result = false;
                        }
                        break;
                    }
                case "5"://矩阵多选
                    {
                        var errorCount = 0;
                        $(".Questionnaire label[data-qid='" + qid + "']").each(function () {
                            var childQId = $(this).attr("data-tag");
                            var checkboxs = $(".Questionnaire input[type='checkbox'][name='" + childQId + "']:checked");
                            if (checkboxs.length == 0) {
                                {
                                    errorCount++;
                                }
                            }
                        });
                        if (errorCount > 0)
                            result = false;
                        break;
                    }
                case "7"://满意度调查
                    {
                        var myddcVal = $(".Questionnaire input[type='hidden'][name='myddcVal'][data-qid='" + qid + "']").val();
                        if (myddcVal == null || myddcVal == "") {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "8"://多项填空
                    {
                        var firstRowInput = $(".Questionnaire input[data-qid='" + qid + "'][data-first='true'][type='text']");
                        var errorCount = 0;
                        firstRowInput.each(function () {
                            //必填题目验证
                            if ($(this).val() == null || $(this).val() == "") {
                                errorCount++;
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        var _curSelects = $(".Questionnaire select[data-qid='" + qid + "']");
                        var _curOptions = $(".Questionnaire select[data-qid='" + qid + "'] option[value!='-1']:checked");
                        if (_curOptions.length != _curSelects.length) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "9"://排序题
                    {
                        var allInputNumber = $(".Questionnaire input[data-qid='" + qid + "'][type='number']");
                        var errorCount = 0;
                        var num = new Array();
                        var indexTag = 1;
                        allInputNumber.each(function () {
                            if ($(this).val() == null || $(this).val() == "") {
                                errorCount++;
                            }
                            else if ($(this).val() <= 0) {
                                errorCount++;
                            }
                            else {
                                num.push($(this).val());
                                indexTag++;
                            }
                        });
                        var maxVal = Math.max.apply(Math, num);
                        var minVal = Math.min.apply(Math, num);
                        var numNew = $.unique(num);
                        if (errorCount > 0) {
                            result = false;
                        }
                        if (maxVal != allInputNumber.length || minVal != 1 || numNew.length != allInputNumber.length) {
                            result = false;
                        }
                        break;
                    }
                case "10": //单项填空题
                    {
                        var singleInputVal = $(".Questionnaire input[data-qid='" + qid + "'][type='text']");
                        if (singleInputVal.val() == null || singleInputVal.val() == "") {
                            result = false;
                        }
                        break;
                    }
            }
        });
        //数据验证（多项填空和培训答案验证）
        var allInputText = $(".Questionnaire input[type='text'][data-qtype='fill']");
        allInputText.each(function () {
            var errorCount = 0;
            //输入内容格式验证
            if ($(this).val() != null && $(this).val() != "") {
                if ($(this).attr("data-valuetype") == 1)//整数
                {
                    var ex = /^\d+$/;
                    if (!ex.test($(this).val())) {
                        // 则为整数
                        errorCount++;
                    }
                }
                if ($(this).attr("data-valuetype") == 2)//浮点数
                {
                    if (!$.isNumeric($(this).val())) {
                        errorCount++;
                    }
                }
            }
            if (errorCount > 0) {
                result = false;
            }
        });
        if (!result) { popWindownBlue("请正确填写所有必填项再提交！题号：" + qid); return false; }
        //获取答案
        var inputs = $(".Questionnaire .divContent input:checked");
        var resultvalue = "";
        $.each(inputs, function () {
            if ($(this).attr("data-type") == 4) {
                //var _curQid = $(this).attr("name");
                //var _curOid = $(this).val();
                //var inputVal = $("input[type='text'][data-for='" + _curOid + "'][data-qid='" + _curQid + "']");
                //resultvalue += $(this).attr("name") + "{" + inputVal + "}";
            } else {
                resultvalue += $(this).attr("name") + "{" + $(this).val() + "}";
            }
        });
        var textarea = $(".Questionnaire .divContent textarea[name='userSuggest'][placeholder]");
        $.each(textarea, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //满意度调查
        var inputHidden = $(".Questionnaire .divContent input[name='myddcVal'][type='hidden'],.Questionnaire .divContent input[type='text'][data-type='reason']");
        $.each(inputHidden, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //多项填空
        var inputTexts = $(".Questionnaire .divContent input[type='text'][data-qtype='fill']");
        $.each(inputTexts, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).attr("data-order") + ":" + $(this).val() + "}";
        });
        var inputSelects = $(".Questionnaire .divContent select option[value!='-1']:checked");
        $.each(inputSelects, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).attr("data-order") + ":" + $(this).val() + "}";
        });
        //排序
        var inputOrder = $(".Questionnaire .divContent input[type='number'][data-qtype='order']");
        $.each(inputOrder, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + ":" + $(this).attr("data-tag") + "}";
        });
        //单项填空
        var inputFillTexts = $(".Questionnaire .divContent input[type='text'][data-qtype='single_fill']");
        $.each(inputFillTexts, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //选项补充信息
        var inputAfterRadio = $(".Questionnaire .divContent input[type='text'][data-qtype='after_fill']");
        $.each(inputAfterRadio, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //验证信息并添加访客表和答案表
        if (linkFrom == "1") {
            //CS端数据提交
            Questionnaire.QuestionnaireSubmitByCS(id, qid, resultvalue, userEmail);
            return false;
        }
        else {
            //BM数据提交
            var passerbyName = $(".Questionnaire #inputPasserbyName").val();
            var passerbySex = $(".Questionnaire input[name='radioPasserbySex']:checked").val();
            var passerbyPhone = $(".Questionnaire #inputPasserbyPhone").val();

            var passerbyAge = $(".Questionnaire #inputPasserbyAge option:selected").text();

            var passerbyCarType = "";
            if ($(".Questionnaire #carType").attr("disabled") != "disabled")
                passerbyCarType = $(".Questionnaire #carType option:selected").text();
            else
                passerbyCarType = $(".Questionnaire input[type='checkbox'][name='inputPasserbyCar']:checked").val();

            var passerbyEducation = $(".Questionnaire #inputPasserbyEducation option:selected").text();

            var addressProvince = $(".Questionnaire #selectPasserbyAddressProvince option:selected").text();
            var addressCity = $(".Questionnaire #myOrderAddressCity option:selected").text();
            var addressCounty = $(".Questionnaire #myOrderAddressCounty option:selected").text();
            var passerbyAddress = $(".Questionnaire #passerbyinfoAddress").val();
            var _curBlueBeanCount = $(".Questionnaire #questionnaireBlueBeanCount").val();
            //陌生人信息验证
            if (passerbyName == null || passerbyName == "" || passerbyName.length < 2) {
                popWindownBlue("您未正确填写姓名信息，请修正后继续！");
                $(".Questionnaire #inputPasserbyName").val("");
                return false;
            }
            //年龄验证
            if (passerbyAge == null || passerbyAge == "请选择") {
                passerbyAge = "";
            }
            //学历验证
            if (passerbyEducation == null || passerbyEducation == "请选择") {
                passerbyEducation = "";
            }
            //电话号码验证
            if (passerbyPhone == null || passerbyPhone == "") {
                popWindownBlue("您未填写联系电话，请补充完整后继续！");
                return false;
            }
            //var phoneReg = new RegExp("^1(3\d|5[012356789]|4[57]|7\d|8[03456789])\d{8}$");
            var phoneReg = new RegExp("^(13|14|15|17|18)[0-9]{9}$");
            if (!phoneReg.test(passerbyPhone)) {
                popWindownBlue("请输入正确的手机号码.");
                $(".Questionnaire #inputPasserbyPhone").val("");
                return false;
            }
            //地址信息不完整
            if (addressProvince == null || addressProvince == "" || addressProvince == "请选择" || addressCity == null || addressCity == "" || addressCity == "请选择" || addressCounty == null || addressCounty == "" || addressCounty == "请选择" || passerbyAddress == null || passerbyAddress == "") {
                popWindownBlue("您的地址信息未填写或不完整，请补充完整后继续！");
                return false;
            }
            //提交并保存数据
            Questionnaire.QuestionnaireSubmit(passerbyName, passerbySex, passerbyPhone, addressProvince, addressCity, addressCounty, passerbyAddress, passerbyAge, passerbyEducation, passerbyCarType, userEmail, qid, resultvalue, id, _curBlueBeanCount, linkSource, linkFrom);
            return false;
        }
    });

    $(".Questionnaire #inputIndexAccountLogin").click(function () {
        var _curFrom = getQueryString("from");
        var _curSource = getQueryString("source");
        var returnUrl = "/Questionnaire/Index";
        if (_curFrom != null && _curFrom != "") {
            returnUrl += ("?from=" + _curFrom);
        }
        if (_curSource != null && _curSource != "") {
            if (returnUrl.indexOf("?") > 0)
                returnUrl += ("&source=" + _curSource);
            else
                returnUrl += ("?source=" + _curSource);
        }
        location.href = "/Account/LogonPage?returnUrl=" + encodeURIComponent(returnUrl);
    });

    $(".Questionnaire #inputIndexAccountRegister").click(function () {
        var _curFrom = getQueryString("from");
        var _curSource = getQueryString("source");
        var returnUrl = "/Questionnaire/Index";
        if (_curFrom != null && _curFrom != "") {
            returnUrl += ("?from=" + _curFrom);
        }
        if (_curSource != null && _curSource != "") {
            if (returnUrl.indexOf("?") > 0)
                returnUrl += ("&source=" + _curSource);
            else
                returnUrl += ("?source=" + _curSource);
        }
        if (_curFrom == "CS") {
            location.href = "/Account/AccountRegister?returnUrl=" + encodeURIComponent(returnUrl) + "&source=cs_questionnaire";
        } else {
            location.href = "/Account/AccountRegister?returnUrl=" + encodeURIComponent(returnUrl) + "&source=blms_questionnaire";
        }
    });

    //========================================移动端问卷调查页面============================================

    $(".WapQuestionnaire #btnQuestionnaireSubmit").unbind("click").click(function () {
        //获取必答题    
        var requiredQ = $(".WapQuestionnaire p[data-qid][data-type][data-required='True']");
        //用户编号
        var id = $(".WapQuestionnaire #inputUserID").val();
        //昵称
        var nickName = $(".WapQuestionnaire #inputUserNickName").val();
        //问卷编号
        var qid = $(".WapQuestionnaire #input_hidden_QuestionnaireId").val();
        var userEmail = $(".WapQuestionnaire #inputUserEmail").val();
        var linkSource = $(".WapQuestionnaire #inputLinkSource").val();
        var linkFrom = $(".WapQuestionnaire #inputLinkFrom").val();
        var result = true;

        //验证是否完成了必答题
        requiredQ.each(function () {
            var qid = $(this).attr("data-qid").toString();
            var type = $(this).attr("data-type").toString();
            //判断单选
            switch (type) {
                case "0"://单选
                case "2"://判断
                    {
                        var radios = $(".WapQuestionnaire input[type='radio'][name='" + qid + "']:checked");
                        if (radios.length == 0) {
                            result = false;
                            break;
                        }
                        var errorCount = 0;
                        $.each(radios, function () {
                            var _id = $(this).attr("id");
                            var _qid = $(this).attr("name");
                            if ($(this).attr("data-type") == 4) {
                                var _qitaVal = $(".WapQuestionnaire input[type='text'][data-qid='" + _qid + "'][data-for='" + _id + "'][data-qtype='after_fill']").val();
                                if (_qitaVal == null || _qitaVal == "") {
                                    errorCount++;
                                }
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "1"://复选
                    {
                        var checkboxs = $(".WapQuestionnaire input[type='checkbox'][name='" + qid + "']:checked");
                        if (checkboxs.length == 0) {
                            result = false;
                            break;
                        }
                        var errorCount = 0;
                        $.each(checkboxs, function () {
                            var _id = $(this).attr("id");
                            var _qid = $(this).attr("name");
                            if ($(this).attr("data-type") == 4) {
                                var _qitaVal = $(".WapQuestionnaire input[type='text'][data-qid='" + _qid + "'][data-for='" + _id + "'][data-qtype='after_fill']").val();
                                if (_qitaVal == null || _qitaVal == "") {
                                    errorCount++;
                                }
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "3"://填空
                    {
                        var text = $(".WapQuestionnaire textarea[name='userSuggest'][placeholder][data-qid='" + qid + "']");
                        if (text.val() == null || text.val() == "") {
                            result = false;
                        }
                        break;
                    }
                case "4"://矩阵单选
                    {
                        var errcount = 0;
                        $(".WapQuestionnaire label[data-qid='" + qid + "']").each(function () {
                            var childQId = $(this).attr("data-tag");
                            var checkboxs = $(".WapQuestionnaire input[type='radio'][name='" + childQId + "']:checked");
                            if (checkboxs.length == 0) {
                                {
                                    errcount++;
                                }
                            }
                        });
                        if (errcount > 0)
                            result = false;
                        break;
                    }
                case "5"://矩阵多选
                    {
                        var errcount = 0;
                        $(".WapQuestionnaire label[data-qid='" + qid + "']").each(function () {
                            var childQId = $(this).attr("data-tag");
                            var checkboxs = $(".WapQuestionnaire input[type='checkbox'][name='" + childQId + "']:checked");
                            if (checkboxs.length == 0) {
                                {
                                    errcount++;
                                }
                            }
                        });
                        if (errcount > 0)
                            result = false;
                        break;
                    }
                case "7"://满意度调查
                    {
                        var myddcVal = $(".WapQuestionnaire input[type='hidden'][name='myddcVal'][data-qid='" + qid + "']").val();
                        if (myddcVal == null || myddcVal == "") {
                            result = false;
                        }
                        break;
                    }
                case "8"://多项填空
                    {
                        var firstRowInput = $(".WapQuestionnaire input[data-qid='" + qid + "'][data-first='true'][type='text']");
                        var errorCount = 0;
                        firstRowInput.each(function () {
                            if ($(this).val() == null || $(this).val() == "") {
                                errorCount++;
                            }
                        });
                        if (errorCount > 0) {
                            result = false;
                            break;
                        }
                        var _curSelects = $(".WapQuestionnaire select[data-qid='" + qid + "']");
                        var _curOptions = $(".WapQuestionnaire select[data-qid='" + qid + "'] option[value!='-1']:checked");
                        if (_curOptions.length != _curSelects.length) {
                            result = false;
                            break;
                        }
                        break;
                    }
                case "9"://排序题
                    {
                        var allInputNumber = $(".WapQuestionnaire input[data-qid='" + qid + "'][type='number']");
                        var errorCount = 0;
                        var num = new Array();
                        var indexTag = 1;
                        allInputNumber.each(function () {
                            if ($(this).val() == null || $(this).val() == "") {
                                errorCount++;
                            } else if ($(this).val() <= 0) {
                                errorCount++;
                            }
                            else {
                                num.push($(this).val());
                                indexTag++;
                            }
                        });
                        var maxVal = Math.max.apply(Math, num);
                        var minVal = Math.min.apply(Math, num);
                        var numNew = $.unique(num);
                        if (errorCount > 0 || maxVal != allInputNumber.length || minVal != 1 || numNew.length != allInputNumber.length) {
                            result = false;
                        }
                        break;
                    }
                case "10": //单项填空题
                    {
                        var singleInputVal = $(".WapQuestionnaire input[data-qid='" + qid + "'][type='text']");
                        if (singleInputVal.val() == null || singleInputVal.val() == "") {
                            result = false;
                        }
                        break;
                    }
            }
        });
        //数据验证（多项填空和培训答案验证）
        var allInputText = $(".WapQuestionnaire input[type='text'][data-qtype='fill']");
        allInputText.each(function () {
            var errorCount = 0;
            //输入内容格式验证
            if ($(this).val() != null && $(this).val() != "") {
                if ($(this).attr("data-valuetype") == 1)//整数
                {
                    var ex = /^\d+$/;
                    if (!ex.test($(this).val())) {
                        // 则为整数
                        errorCount++;
                    }
                }
                if ($(this).attr("data-valuetype") == 2)//浮点数
                {
                    if (!$.isNumeric($(this).val())) {
                        errorCount++;
                    }
                }
            }
            if (errorCount > 0) {
                result = false;
            }
        });
        //问卷所有带 * 号项为必填，请完整填写后再提交
        if (!result) { popWindownBlue("请正确填写所有必填项再提交!"); return false; }
        //获取答案
        var inputs = $(".WapQuestionnaire .divContent input:checked");
        var resultvalue = "";
        $.each(inputs, function () {
            if ($(this).attr("data-type") == 4) {
                //var _curQid = $(this).attr("name");
                //var _curOid = $(this).val();
                //var inputVal = $("input[type='text'][data-for='" + _curOid + "'][data-qid='" + _curQid + "']").val().replace(",", "&#44;");
                //resultvalue += $(this).attr("name") + "{" + inputVal + "}";
            } else {
                resultvalue += $(this).attr("name") + "{" + $(this).val() + "}";
            }
        });
        var textarea = $(".WapQuestionnaire .divContent textarea[name='userSuggest'][placeholder]");
        $.each(textarea, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        var inputHidden = $(".WapQuestionnaire .divContent input[name='myddcVal'][type='hidden'],.WapQuestionnaire .divContent input[type='text'][data-type='reason']");
        $.each(inputHidden, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //多项填空
        var inputTexts = $(".WapQuestionnaire .divContent input[type='text'][data-qtype='fill']");
        $.each(inputTexts, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).attr("data-order") + ":" + $(this).val() + "}";
        });
        var inputSelects = $(".WapQuestionnaire .divContent select option[value!='-1']:checked");
        $.each(inputSelects, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).attr("data-order") + ":" + $(this).val() + "}";
        });
        //排序
        var inputOrder = $(".WapQuestionnaire .divContent input[type='number'][data-qtype='order']");
        $.each(inputOrder, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + ":" + $(this).attr("data-tag") + "}";
        });
        //单项填空
        var inputFillTexts = $(".WapQuestionnaire .divContent input[type='text'][data-qtype='single_fill']");
        $.each(inputFillTexts, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //选项补充信息
        var inputAfterRadio = $(".WapQuestionnaire .divContent input[type='text'][data-qtype='after_fill']");
        $.each(inputAfterRadio, function () {
            resultvalue += $(this).attr("data-qid") + "{" + $(this).val() + "}";
        });
        //验证信息并添加访客表和答案表
        if (linkFrom == "1") {
            //CS端数据提交
            WapQuestionnaire.QuestionnaireSubmitByCS(id, qid, resultvalue, userEmail);
            return false;
        }
        else //验证信息并添加访客表和答案表
        {
            var passerbyName = $(".WapQuestionnaire #inputPasserbyName").val();
            var passerbySex = $(".WapQuestionnaire #inputPasserbySex option:selected").text();
            var passerbyPhone = $(".WapQuestionnaire #inputPasserbyPhone").val();

            var passerbyAge = $(".WapQuestionnaire #inputPasserbyAge option:selected").text();


            var passerbyCarType = "";
            if ($(".WapQuestionnaire #carType").attr("disabled") != "disabled")
                passerbyCarType = $(".WapQuestionnaire #carType option:selected").text();
            else
                passerbyCarType = $(".WapQuestionnaire input[type='checkbox'][name='inputPasserbyCar']:checked").val();

            var passerbyEducation = $(".WapQuestionnaire #inputPasserbyEducation option:selected").text();

            var addressProvince = $(".WapQuestionnaire #selectPasserbyAddressProvince option:selected").text();
            var addressCity = $(".WapQuestionnaire #myOrderAddressCity option:selected").text();
            var addressCounty = $(".WapQuestionnaire #myOrderAddressCounty option:selected").text();
            var passerbyAddress = $(".WapQuestionnaire #passerbyinfoAddress").val();
            var _curBlueBeanCount = $(".WapQuestionnaire #questionnaireBlueBeanCount").val();
            //陌生人信息验证
            if (passerbyName == null || passerbyName == "" || passerbyName.length < 2) {
                popWindownBlue("您未正确填写姓名信息，请修正后继续！");
                $(".WapQuestionnaire #inputPasserbyName").val("");
                return false;
            }
            //性别验证
            if (passerbySex == null || passerbySex == "请选择性别") {
                passerbySex = "";
            }
            //年龄验证
            if (passerbyAge == null || passerbyAge == "请选择年龄段") {
                passerbyAge = "";
            }
            //学历验证
            if (passerbyEducation == null || passerbyEducation == "请选择学历") {
                passerbyEducation = "";
            }
            //电话号码验证
            if (passerbyPhone == null || passerbyPhone == "") {
                popWindownBlue("您未填写联系电话，请补充完整后继续！");
                return false;
            }
            var phoneReg = new RegExp("^(13|14|15|17|18)[0-9]{9}$");
            if (!phoneReg.test(passerbyPhone)) {
                popWindownBlue("请输入正确的手机号码.");
                $(".WapQuestionnaire #inputPasserbyPhone").val("");
                return false;
            }
            //地址信息不完整
            if (addressProvince == null || addressProvince == "" || addressProvince == "请选择" || addressProvince == "省" || addressCity == null || addressCity == "" || addressCity == "请选择" || addressCity == "市" || addressCounty == null || addressCounty == "" || addressCounty == "区" || addressCounty == "请选择" || passerbyAddress == null || passerbyAddress == "") {
                popWindownBlue("您的地址信息未填写或不完整，请补充完整后继续！");
                return false;
            }
            //统一保存方式，后台处理判断
            WapQuestionnaire.QuestionnaireSubmit(passerbyName, passerbySex, passerbyPhone, addressProvince, addressCity, addressCounty, passerbyAddress, passerbyAge, passerbyEducation, passerbyCarType, userEmail, qid, resultvalue, id, _curBlueBeanCount, linkSource, linkFrom);
            return false;
        }
    });

    $(".WapQuestionnaire #inputIndexAccountLogin").click(function () {
        var _curFrom = getQueryString("from");
        var _curSource = getQueryString("source");
        var returnUrl = "/WapQuestionnaire/Index";
        if (_curFrom != null && _curFrom != "") {
            returnUrl += ("?from=" + _curFrom);
        }
        if (_curSource != null && _curSource != "") {
            if (returnUrl.indexOf("?") > 0)
                returnUrl += ("&source=" + _curSource);
            else
                returnUrl += ("?source=" + _curSource);
        }
        location.href = "/WapQuestionnaire/WapLogin?returnUrl=" + encodeURIComponent(returnUrl);
    });

    $(".WapQuestionnaire #inputIndexAccountRegister").click(function () {
        var _curFrom = getQueryString("from");
        var _curSource = getQueryString("source");
        var returnUrl = "/WapQuestionnaire/Index";
        if (_curFrom != null && _curFrom != "") {
            returnUrl += ("?from=" + _curFrom);
        }
        if (_curSource != null && _curSource != "") {
            if (returnUrl.indexOf("?") > 0)
                returnUrl += ("&source=" + _curSource);
            else
                returnUrl += ("?source=" + _curSource);
        }
        if (_curFrom == "CS") {
            location.href = "/WapQuestionnaire/WapRegister?returnUrl=" + encodeURIComponent(returnUrl) + "&source=cs_questionnaire";
        } else {
            location.href = "/WapQuestionnaire/WapRegister?returnUrl=" + encodeURIComponent(returnUrl) + "&source=blms_questionnaire";
        }
    });

    //========================================移动端登录页面============================================
    $(".divwaplogin #btnWapLoginSubmit").unbind("click").click(function () {
        //验证数据完整性
        var _username = $(".divwaplogin #LogonPageAccount").val();
        var _password = $(".divwaplogin #LogonPagePasswd").val();
        var _imgcode = $(".divwaplogin #LogonCaptcha").val();
        var _returnurl = $(".divwaplogin #returnUrl").val();
        if (_username == null || _username == "" || _password == null || _password == "") {
            popWindownBlue("用户名或密码不能为空！");
            return false;
        }
        if (_imgcode == null || _imgcode == "") {
            popWindownBlue("请输入验证码！");
            return false;
        }
    });

    $(".divwaplogin #btnWapRegisterSubmit").unbind("click").click(function () {
        //登录跳转注册
        var _curReturnUrl = getQueryString("returnUrl");
        if (_curReturnUrl != null && _curReturnUrl != "") {
            if (_curReturnUrl.indexOf("from=CS", 0) >= 0) {
                location.href = "/WapQuestionnaire/WapRegister?returnUrl=" + encodeURIComponent(_curReturnUrl) + "&source=cs_questionnaire";
            } else {
                location.href = "/WapQuestionnaire/WapRegister?returnUrl=" + encodeURIComponent(_curReturnUrl) + "&source=blms_questionnaire";
            }
        } else {
            location.href = "/WapQuestionnaire/WapRegister";
        }
    });

    //========================================移动端注册页面============================================
    $("#btnInputPayNumberToActive_AccountRegister_Wap").click(function () {
        if ($(this).is(":checked")) {
            $("#btnInputText_PayNumber_Wap").show();
        } else {
            $("#btnInputText_PayNumber_Wap").hide();
        }
    });

    $("#btnApplyMemberToDealer_AccountRegister_Wap").click(function () {
        if ($(this).is(":checked")) {
            $("#btnInputText_PayNumber_Wap").hide();
        } else {
            $("#btnInputText_PayNumber_Wap").show();
        }
    });

    $("#AccountRegisterAgreeRegister_wap").click(function () {
        if ($(this).is(":checked")) {
            $("#btnRegisterAccountReg_Wap").addClass("enableclick")
        } else {
            $("#btnRegisterAccountReg_Wap").removeClass("enableclick");
        }
    });

    $("#AccountRegisterSendCaptcha_Wap").click(function () {

        var _mobile = $("#registerMobile").val();
        var _mobileimgcode = $("#registerImageCode_Wap").val();

        //校验是否重复点击
        if ($(this).attr("disabled")) {
            return false;
        }

        //校验图形验证码是否正确
        //if (!_mobileimgcode) {
        //    popWindownBlue( "请输入正确的图形验证码");
        //    return false;
        //}

        //首先校验手机号
        //if (_mobile == null || _mobile == "") {
        //    //请输入手机号
        //    popWindownBlue( "请输入手机号");
        //    return false;
        //}
        //更新剩余时长
        //countDown(this, 60);
        //验证码发送
        sendCaptchaAndCheckImageCode(_mobile, _mobileimgcode, this);

        //sendCaptcha($("#registerMobile").val());
    });

    $("#btnRegisterAccountReg_Wap").click(function () {
        //判断注册按钮是否可用
        if (!$(this).hasClass("enableclick")) {
            return false;
        }
        var mobile = $("#registerMobile").val();
        var captcha = $("#registerCaptcha").val();
        var passwd = $("#registerPasswd").val();
        var repasswd = $("#registerComfirmPasswd").val();
        var nickname = $("#registerNickname").val();
        var paperwork = $("#accountregisterUserPaperWork").val();
        if (paperwork == "-1")
        {
            paperwork = "";
        }
        var identityno = $("#registerIdentityNo").val();
        var sltProvince = $("#sltProvince").val();
        var sltCity = $("#sltCity").val();
        var sltDealer = $("#sltDealer").val();
        var accountPayNumber = $("#btnInputText_PayNumber_Wap").val();
        var _payType = $("input[name='ActivityType']:checked").val();
        var registerVIN = $("#registerVIN").val();
        var returnUrl = $("#returnUrl").val();
        var source = $("#source").val();
        if (mobile == null || mobile == "") {
            popWindownBlue("请输入手机号");
            return false;
        }
        if (captcha == null || captcha == "") {
            popWindownBlue("请输入验证码");
            return false;
        }
        if (passwd == null || passwd == "" || repasswd == null || repasswd == "" || passwd != repasswd) {
            popWindownBlue("两次密码不一致");
            return false;
        }
        if (nickname == null || nickname == "") {
            popWindownBlue("请输入昵称");
            return false;
        }
        //customerType默认为-1，身份证类型为1，组织机构代码为2
        var customerType = -1;
        var identityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        var isCarOwner = 1;
        if ($("#IsAccountRegisterCarOwner").is(":checked")) {
            if ($("#registerIdentityNo_account_radio").is(":checked")) {
                if (!paperwork || paperwork < 0) {
                    popWindownBlue("请选择证件类型");
                    return false;
                }
            }
            if (identityno == null || identityno == "") {
                popWindownBlue("请输入证件号");
                return false;
            }
            if ($("#registerIdentityNo_account_radio").is(":checked")) {
                if (1 == paperwork) {
                    identityReg = /(^\d{15}$)|(^\d{17}([0-9]|X)$)/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (2 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{3,21}$|^(P\d{7})|(G\d{8})$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
                if (3 == paperwork) {
                    identityReg = /^[a-zA-Z0-9]{7,21}$/;
                    if (!identityReg.test(identityno)) {
                        popWindownBlue("请正确输入证件号码");
                        return false;
                    }
                }
            }
            //if ($("#registerIdentityNo_account_radio").is(":checked") && !identityReg.test(identityno)) {
            //    popWindownBlue( "请正确输入证件号");
            //    return false;
            //}

            if ($("#registerIdentityNo_account_radio").is(":checked")) {
                customerType = 1;
            }
            if ($("#zuzhijigouNo_account_radio").is(":checked")) {
                customerType = 2;
                if (!registerVIN) {
                    popWindownBlue("请输入VIN");
                    return false;
                }
            }
            isCarOwner = 2;
            if ($("#AccountRegisterIsSonataUser").is(":checked")) {
                isCarOwner = 3;
                if (sltDealer == -1) {
                    popWindownBlue("请选择4s店");
                    return false;
                }
                if (_payType != null && _payType != "undefined") {
                    if (_payType == 1) {
                        if (accountPayNumber == null || typeof accountPayNumber == "undefined") {
                            popWindownBlue("请输入支付码");
                            return false;
                        }
                    }
                }
            }
        } else {
            identityno = "";
            sltDealer = "";
        }
        //
        User.Register(mobile, captcha, passwd, repasswd, nickname, identityno, sltDealer, isCarOwner, _payType, accountPayNumber, registerVIN, customerType, paperwork, returnUrl, source);
    });

    $("#VernaaId").click(function () {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();
        var start = year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second;
        var end = '2016-11-10 00:00:00';
        var d1 = start.replace(/\-/g, "\/");
        var d2 = end.replace(/\-/g, "\/");
        if (new Date(d1) > new Date(d2)) {
            popWindownBlue("活动已结束，请关注bluemembers其他活动");
            return;
        } else {
            location.href = "/Yuena/LogOn";
        }
    });
});