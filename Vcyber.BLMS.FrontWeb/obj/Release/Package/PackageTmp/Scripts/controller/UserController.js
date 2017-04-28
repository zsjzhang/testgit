var User = {

};
User.Logon = function (account, passwd, captcha, source) {
    if (account === null || account === "") {

        return false;
    }
    if (passwd === null || passwd === "") {
        return false;
    }
    if (captcha === null || captcha === "") {
        return false;
    }
    $.ajax({
        url: "/Account/DoLogin",  //DoAccountLogon
        type: "post",
        dataType: "json",
        data: { UserName: account, Password: passwd, captcha: captcha, t: (new Date()).getTime() },
        success: function (result) {
            if (result === null || result.code === null || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "401") {
                popWindownBlue( '验证码错误');
                return false;
            }
            if (result.code == "402") {
                popWindownBlue( "账号或密码有误");
                return false;
            }
            if (result.code == "902") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "901") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "300") {

                popWindownBlue(result.msg, function () {
                    window.location.href = "/Account/ResetPasswd?source=" + source;
                   
                });
                return false;
            }
            if (result.code == "200") {
                $.ajax({
                    url: "/Account/HomeLogonSuccesstwo?t=" + (new Date().getTime()),
                    type: "get",
                    success: function (resultView) {
                        $("div.logon").html(resultView);
                    },
                    error: function (err) { }
                });

                if (result.isrequest == 1) {
                  //  alert(result.returnIntegralType);
                    //salert(result.identityNumber);
                    if (result.returnIntegralType == 0) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值400元积分");
                      //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();

                    }
                    if (result.returnIntegralType == 1) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值700元积分");
                       // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                    }
                    if (result.returnIntegralType == 2) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值200元积分");
                       // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToLoginVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    if (result.returnIntegralType == 3) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值400元积分");
                      //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    $("#loginIdentityNo").val(result.identityNumber);
                    $("#account_AccountRegisterIsSonataUser").show();
                }
            }
        },
        error: function (err) {

        }
    });
};

User.CenterLogon = function (account, passwd, captcha, source) {
    if (account === null || account === "") {

        return false;
    }
    if (passwd === null || passwd === "") {
        return false;
    }
    if (captcha === null || captcha === "") {
        return false;
    }
    $.ajax({
        url: "/Account/DoLogin",  //DoAccountLogon
        type: "post",
        dataType: "json",
        data: { UserName: account, Password: passwd, captcha: captcha, t: (new Date()).getTime() },
        success: function (result) {
            if (result === null || result.code === null || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            }

            if (result.code == "401") {
                popWindownBlue( '验证码错误');
                return false;
            }
            if (result.code == "402") {
                popWindownBlue( "账号或密码有误");
                return false;
            }
            if (result.code == "902") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "901") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "300") {

                popWindownBlue(result.msg, function () {
                    window.location.href = "/Account/ResetPasswd?source=" + source;
                    
                });
                return false;
            }

            if (result.code == "200") {
                if (source.indexOf("OrderChange") != -1) {
                    if (result.identityNumber != "" && result.systemmtype == 2) {
                        window.location.href = "/OrderChange/TuiJian" + source.substring(source.indexOf("?")).replace("|", "&");
                    }
                    else {
                        window.location.href = "/OrderChange/BangDing" + source.substring(source.indexOf("?")).replace("|", "&");
                    }
                    return;
                }
                if (result.isrequest == 1) {
                    //  alert(result.returnIntegralType);
                    //salert(result.identityNumber);
                    if (result.returnIntegralType == 0) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();

                    }
                    if (result.returnIntegralType == 1) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值700元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                    }
                    if (result.returnIntegralType == 2) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值200元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToLoginVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    if (result.returnIntegralType == 3) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    $("#loginIdentityNo").val(result.identityNumber);
                    $("#account_AccountRegisterIsSonataUser").show();
                } else
                {
                    window.location.href = "/MyCenter/Index?pageName=MyCenter";
                }
            }
        },
        error: function (err) {

        }
    });
};
User.LogonByPhone = function (account, captcha,smscaptcha, source) {
    if (account === null || account === "") {

        return false;
    }
    if (captcha === null || captcha === "") {
        return false;
    }

    if (smscaptcha === null || smscaptcha === "") {
        return false;
    }
    var  password = "Bm"+ account.substring(5, 11);
    $.ajax({
        url: "/Account/DoLoginByPhone",  //DoAccountLogon
        type: "post",
        dataType: "json",
        data: { UserName: account, Password: password, Captcha: captcha, SMSCaptcha: smscaptcha, t: (new Date()).getTime() },
        success: function (result) {
            if (result === null || result.code === null || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            }
            //if (result.code == "401") {
            //    popWindownBlue( '短信验证码错误');
            //    return false;
            //}
            if (result.code == "300") {

                popWindownBlue(result.msg, function () {
                    window.location.href = "/Account/ResetPasswd?source=" + source;
                    
                });
                return false;
            }
            if (result.code == "200") {
                if (result.first == 1)
                {
                    popWindownBlue("请完善您的个人信息，绑定车辆享受更多权益哦", function () {
                        window.location.href = "/MyCenter/SetUserInfo";
                        
                    });
                    return false;
                }
                   $.ajax({
                    url: "/Account/HomeLogonSuccess?t=" + (new Date().getTime()),
                    type: "get",
                    success: function (resultView) {
                        $("div.logon").html(resultView);
                    },
                    error: function (err) { }
                });
                
            }
        },
        error: function (err) {

        }
    });
};
User.LogonByPhone = function (account, captcha,smscaptcha, source) {
    if (account === null || account === "") {

        return false;
    }
    if (captcha === null || captcha === "") {
        return false;
    }

    if (smscaptcha === null || smscaptcha === "") {
        return false;
    }
    var  password = "Bm"+ account.substring(5, 11);
    $.ajax({
        url: "/Account/DoLoginByPhone",  //DoAccountLogon
        type: "post",
        dataType: "json",
        data: { UserName: account, Password: password, Captcha: captcha, SMSCaptcha: smscaptcha, t: (new Date()).getTime() },
        success: function (result) {
            if (result === null || result.code === null || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            }
            //if (result.code == "401") {
            //    popWindownBlue( '短信验证码错误');
            //    return false;
            //}
            if (result.code == "300") {

                popWindownBlue(result.msg, function () {
                    window.location.href = "/Account/ResetPasswd?source=" + source;
                   
                });
                return false;
            }
            if (result.code == "200") {
                if (result.first == 1)
                {
                    popWindownBlue("请完善您的个人信息，绑定车辆享受更多权益哦", function () {
                        window.location.href = "/MyCenter/SetUserInfo";
                       
                    });
                    return false;
                }
                $.ajax({
                    url: "/Account/HomeLogonSuccesstwo?t=" + (new Date().getTime()),
                    type: "get",
                    success: function (resultView) {
                        $("div.logon").html(resultView);
                    },
                    error: function (err) { }
                });


                if (result.isrequest == 1) {
                    //  alert(result.returnIntegralType);
                    //salert(result.identityNumber);
                    if (result.returnIntegralType == 0) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();

                    }
                    if (result.returnIntegralType == 1) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值700元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                    }
                    if (result.returnIntegralType == 2) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值200元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToLoginVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    if (result.returnIntegralType == 3) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    $("#loginIdentityNo").val(result.identityNumber);
                    $("#account_AccountRegisterIsSonataUser").show();
                }
            }
        },
        error: function (err) {

        }
    });
};

User.CenterLogonByPhone = function (account, captcha, smscaptcha, source) {
    if (account === null || account === "") {

        return false;
    }
    if (captcha === null || captcha === "") {
        return false;
    }

    if (smscaptcha === null || smscaptcha === "") {
        return false;
    }
    var password = "Bm" + account.substring(5, 11);
    $.ajax({
        url: "/Account/DoLoginByPhone",  //DoAccountLogon
        type: "post",
        dataType: "json",
        data: { UserName: account, Password: password, Captcha: captcha, SMSCaptcha: smscaptcha, t: (new Date()).getTime() },
        success: function (result) {
            if (result === null || result.code === null || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            }
            //if (result.code == "401") {
            //    popWindownBlue( '短信验证码错误');
            //    return false;
            //}
            if (result.code == "300") {

                popWindownBlue(result.msg, function () {
                    window.location.href = "/Account/ResetPasswd?source=" + source;
                   
                });
                return false;
            }
            if (result.code == "200") {

                if (result.isrequest == 1) {
                    //  alert(result.returnIntegralType);
                    //salert(result.identityNumber);
                    if (result.returnIntegralType == 0) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();

                    }
                    if (result.returnIntegralType == 1) {

                        $("#reIntegralmsg").html("您交费100元即可获得价值700元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                    }
                    if (result.returnIntegralType == 2) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值200元积分");
                        // $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToLoginVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    if (result.returnIntegralType == 3) {

                        $("#reIntegralmsg").html("您交费50元即可获得价值400元积分");
                        //  $("#reIntegralmsg").css("display", "block");
                        $("#account_AccountRegisterIsSonataUser").show();
                        $('#btnInputPayNumberToVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                    }
                    $("#loginIdentityNo").val(result.identityNumber);
                    $("#account_AccountRegisterIsSonataUser").show();
                }else{

                if (result.first == 1) {
                    popWindownBlue("请完善您的个人信息，绑定车辆享受更多权益哦", function () {
                        window.location.href = "/MyCenter/SetUserInfo";
                        
                    });
                    return false;
                }
                else {
                    window.location.href = "/MyCenter/Index?pageName=MyCenter";
                }
                }
            }
        },
        error: function (err) {

        }
    });
};
User.Register = function (mobile, captcha, passwd, repasswd, nickname, identityNo, sltDealer, isCarOwner, payType, accountPayNumber, registerVIN, customerType, paperwork, returnUrl, source, imgCaptcha) {
    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Account/DoRegister_Vip",
        type: "post",
        data: { Mobile: mobile, NickName: nickname, Captcha: captcha, IdentityNumber: identityNo, Password: passwd, ConfirmPassword: repasswd, ActivedealerId: sltDealer, MType: isCarOwner, ActivityType: payType, PayNumber: accountPayNumber, VIN: registerVIN, customerType: customerType, PaperWork: paperwork, returnUrl: returnUrl, Source: source , ImgCaptcha:imgCaptcha},
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue(result.msg);
                return false;
            }
            //注册成功
            if (result.code == "200") {


                if (result.Mtype == 1)
                {
                    popWindownBlue("您好，绑定车辆享受更多会员权益！请在个人中心进行车辆绑定", function () {
                        window.location = "/MyCenter/Index?pageName=MyCenter";
                     
                    });
                    return false;
                }
                if (result.Mtype == 2 && result.HasCar == 0)
                {
                    popWindownBlue("很抱歉，您的身份未绑定成功，请拨打400-800-1100", function () {
                       window.location = "/MyCenter/Index?pageName=MyCenter";
                       return false;
                    });
                   
                    //layer.open({
                    //    type: 1,
                    //    skin: 'bluePopup', //样式类名
                    //    closeBtn: 1, //不显示关闭按钮
                    //    shift: 2,
                    //    shadeClose: false, //开启遮罩关闭
                    //    content: "很抱歉，您的身份未绑定成功，请拨打400-800-1100",
                    //    area: ['420px', '360px'],
                    //    end: function () {
                    //        window.location = "/MyCenter/Index?pageName=MyCenter";
                    //    },
                    //    btn: ['确定']
                    //});
                    return false;
                }
               /// popWindownBlue( "恭喜您已成功注册成为bluemembers普通会员。");
                if (result.ReIntegralType == -1)
                {
                    window.location = "/MyCenter/Index?pageName=MyCenter";

                }
                if (result.ReIntegralType == 0)
                {
                   
                    $("#reIntegralmsg").val("您交费100元即可获得价值400元积分");
                    $("#reIntegralmsg").css("display", "block");
                    $("#account_AccountRegisterIsSonataUser").show();
                   
                }
                if (result.ReIntegralType == 1) {
                  
                    $("#reIntegralmsg").val("您交费100元即可获得价值700元积分");
                    $("#reIntegralmsg").css("display", "block");
                    $("#account_AccountRegisterIsSonataUser").show();
                }
                if (result.ReIntegralType == 2) {
                
                    $("#reIntegralmsg").val("您交费50元即可获得价值200元积分");
                    $("#reIntegralmsg").css("display", "block");
                    $("#account_AccountRegisterIsSonataUser").show();
                    $('#btnInputPayNumberToRegisterVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                }
                if (result.ReIntegralType == 3) {
                  
                    $("#reIntegralmsg").val("您交费50元即可获得价值400元积分");
                    $("#reIntegralmsg").css("display", "block");
                    $("#account_AccountRegisterIsSonataUser").show();
                    $('#btnInputPayNumberToRegisterVip').attr('href', 'https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop');
                }
                //var i   eft = (window.screen.width - 960) / 2;
                //window.open('/DealerNew/AlertDealerInfo', 'newwindow', 'height=600, width=850, top=100, left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no');

                

                return false;
            }
            if (result.code == "301") {
                popWindownBlue( result.msg);
                var iLeft = (window.screen.width - 960) / 2;
                window.open('/DealerNew/AlertDealerInfo', 'newwindow', 'height=600, width=850, top=100, left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no');
                setTimeout(function () {
                    window.location = "/MyCenter/Index?pageName=MyCenter";
                    return false;
                }, 5000);
                //301提示未匹配到您的车辆，请拨打客服电话
                return false;
            }
            if (result.code == "302") {
                //找
                popWindownBlue( "恭喜您已成功注册成为bluemembers普通会员。");
                var iLeft = (window.screen.width - 960) / 2;
                window.open('/DealerNew/AlertDealerInfo', 'newwindow', 'height=600, width=850, top=100, left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no');
                window.location.href = "/Sonata/SonataActive";
                return false;
            }
            if (result.code == "401") {
                popWindownBlue( result.msg);
                return false;
            }
            if (result.code == "402") {
                popWindownBlue( result.msg);
                //$("#registerVIN_Span").show();
                return false;
            }
            if (result.code == "801")
            {
                popWindownBlue( "注册成功" + result.msg);
                if (returnUrl !== null && returnUrl !== "") {
                    window.location = returnUrl;
                    return false;
                }
                setTimeout(function () {
                    window.location = "/MyCenter/Index?pageName=MyCenter";
                    return false;
                }, 5000);
                return false;
            }
           
        },
        error: function (err) {
            popWindownBlue( "系统异常！");
            //网络超时
        }
    });
};

User.setPasswd = function (mobile, passwd, repasswd, captcha, source) {
    $.ajax({
        url: "/Account/DoResetPasswd",
        type: "post",
        data: { userAccount: mobile, passwd: passwd, newpasswd: repasswd, captcha: captcha },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( result.msg);
                return false;
            } else if (result.code == "200") {
                if (source !== null && source == "HOME") {

                }
                popWindownBlue("重置成功", function () {
                    window.location = "/Home/Default";
                    return false;
                });
               
            }
        },
        error: function (err) {

        }
    });
};

User.mySurplusBlueBean = function (mobile, passwd) {
    $.ajax({
        url: "",
        type: "post",
        data: { userAccount: mobile, pwd: passwd },
        dataType: "json",
        success: function (result) {
        },
        error: function (err) {

        }
    });
};

User.mySurplusIntegral = function (mobile, passwd) {
    $.ajax({
        url: "",
        type: "post",
        data: { userAccount: mobile, pwd: passwd },
        dataType: "json",
        success: function (result) {
        },
        error: function (err) {

        }
    });
};

//根据省份获取城市
User.FindCityByProvince = function (province) {
    $.ajax({
        url: "/User/FindCitysByProvince",
        type: "get",
        data: { provinceCode: $(province).val() },
        success: function (result) {
            $("#myOrderAddressCitySpan").html(result);
        }, error: function () {
            popWindownBlue( "城市获取失败");
            return false;
        }

    });
};

//根据城市获取区域
User.FindAreasByCity = function (city) {
    $.ajax({
        url: "/User/FindAreasByCity",
        type: "get",
        data: { cityCode: $(city).val() },
        success: function (result) {
            $("#myOrderAddressAreaSpan").html(result);
        }, error: function () {
            popWindownBlue( "地区获取失败");
            return false;
        }

    });
};

//验证用户名 是否被注册
User.CheckUserName = function (mobile) {
    //$("#registerMobileerrorred").hide();
    $.ajax({
        url: "/Account/DoCheckUserName",
        type: "post",
        dataType: "json",
        data: { UserName: mobile },
        success: function (result) {
            if (result == "1") {
                $("#registerMobileerrorred").html("你输入的【手机号】已被注册，请更换");
                $("#registerMobileerrorred").show();
                $("#registerMobile").focus();
                return false;
            }

        },
        error: function (err) {

        }
    });
};

//验证身份证号 是否被注册
User.CheckIdentityNumber = function (identityNumber) {
    var textV = $("#accountregisterUserPaperWork").find("option:selected").text();
    if (textV == "请选择") {
        popWindownBlue("请先选择证件类型");
        return false;
    }
    $.ajax({
        url: "/Account/DoCheckIdentityNumber",
        type: "post",
        dataType: "json",
        data: { IdentityNumber: identityNumber },
        success: function (result) {
            if (result == "1") {
                popWindownBlue("你输入的【" + textV + "】已被注册");
                $("#registerVIN_Span").show();
                return false;
            } else {
                if ($("#zuzhijigouNo_account_radio").is(":checked")) {
                    $("#registerVIN_Span").show();
                    $("#registerPaperWork_Span").hide();
                    return false;
                } else {
                    $("#registerVIN_Span").hide();
                    $("#registerPaperWork_Span").show();
                    return false;
                }

            }

        },
        error: function (err) {
        }
    });
};

User.InputPayNumberToActiveSave = function (payNumber, dealerId) {
    if (payNumber == "")
    {
        popWindownBlue( "请正确输入支付码");
        return false;
    }
   
    $.ajax({
        url: "/Account/PayByTmallRequest",
        type: "post",
        dataType: "json",
        data: {  payNumber: payNumber, dealerId: dealerId },
        success: function (result) {
            if (result !== null) {
                if (result.code == 200) {
                    $(".zzDivBox").css('display', 'none');
                    layer.close(document.getElementById('AccountSonataActiveCancelLayerIndex').value);
                    popWindownBlue("支付成功", function () {
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
                return false;
            } 
        },
        error: function (err) {
        }
    });

    //$("#accountPayNumber").val(payNumber);
    //$("#accountActivityType").val(1);//向特约店申请为2，向天猫购买支付码为1
};


