var Questionnaire = {
};
var _submitFlag = false;

Questionnaire.OtherTypeClick = function (checkbox) {
    if (checkbox.checked == true) {
        $("#hidden_div_info_of_passerby #carType").attr("disabled", "disabled");
        $("#hidden_div_info_of_passerby #inputPasserbyCarOtherType").attr("disabled", "disabled");
    }
    else {
        $("#hidden_div_info_of_passerby #carType").removeAttr("disabled");
        $("#hidden_div_info_of_passerby #inputPasserbyCarOtherType").removeAttr("disabled");
    }

}

Questionnaire.OtherTypeOwnerClick = function (checkbox) {
    if (checkbox.checked == true) {
        $("#hidden_div_info_of_passerby #carType").attr("disabled", "disabled");
        $("#hidden_div_info_of_passerby #inputPasserbyCarOtherNotOwner").attr("disabled", "disabled");
    }
    else {
        $("#hidden_div_info_of_passerby #carType").removeAttr("disabled");
        $("#hidden_div_info_of_passerby #inputPasserbyCarOtherNotOwner").removeAttr("disabled");
    }
}

//检测当前用户是否登录
Questionnaire.CheckCurUserIsLogin = function () {
    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/CheckLoginState",
        type: "post",
        data: {},
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                return false;
            }
            else {
                var cells = $(".Questionnaire #hidden_div_info_of_passerby input,.Questionnaire #hidden_div_info_of_passerby select");
                cells.each(function () { $(this).attr("disabled", "disabled"); });
                return true;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常！");
            //网络超时
        }
    });
};

//检测当前用户是否已经完成过问卷
Questionnaire.CheckIsAnswer = function () {
    var memberId = $(".Questionnaire #inputUserID").val();
    var pid = $(".Questionnaire #input_hidden_QuestionnaireId").val();
    //如果为进行登录，退出检测
    if (memberId === null || memberId === "") {
        return false;
    }
    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/CheckIsAnswer",
        type: "post",
        data: { memberId: memberId, pid: pid },
        dataType: "json",
        success: function (result) {
            var returnUrl = "/MyCenter/Index";
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( result.msg);
                location.href = returnUrl;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常！");
            //网络超时
        }
    });
};

//路人答题后问卷提交---原来逻辑
Questionnaire.PasserbySubmit = function (psName, psSex, psPhone, addressProvince, addressCity, addressCounty, psAddress, psAge, psEducation, psCarType, qid, resultvalue) {

    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/SaveResultByPasserBy",
        type: "post",
        data: { psName: psName, psSex: psSex, psPhone: psPhone, addressProvince: addressProvince, addressCity: addressCity, addressCounty: addressCounty, psAddress: psAddress, psAge: psAge, psEducation: psEducation, psCarType: psCarType, qid: qid, resultvalue: resultvalue },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( "数据提交失败,请重试!");
                return false;
            }
            else {
                popWindownBlue( "感谢您的参与，活动结束后会统一进行抽奖，中奖名单将在本站公布，敬请期待！");
                location.href = "/Home";
                return true;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常，网络超时！");
        }
    });
};
//答题后问卷提交
Questionnaire.QuestionnaireSubmit = function (psName, psSex, psPhone, addressProvince, addressCity, addressCounty, psAddress, psAge, psEducation, psCarType, psEmail, qid, resultvalue, id, _curBlueBeanCount, linkSource, linkFrom) {
    if (!_submitFlag)
        _submitFlag = true;
    else {
        return false;
    }

    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/SaveQuestionnaireResult",
        type: "post",
        data: { psName: psName, psSex: psSex, psPhone: psPhone, addressProvince: addressProvince, addressCity: addressCity, addressCounty: addressCounty, psAddress: psAddress, psAge: psAge, psEducation: psEducation, psCarType: psCarType, psEmail: psEmail, qid: qid, resultvalue: resultvalue, memberId: id, _curBlueBeanCount: _curBlueBeanCount, linkSource: linkSource, linkFrom: linkFrom },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( "数据提交失败,请重试!");
                return false;
            }
            else {
                if (id == "" || linkFrom == 1) {
                    popWindownBlue( "感谢您的参与，活动结束后我们会为您邮寄精美礼品，敬请期待！");
                    //popWindownBlue( "感谢您的参与，活动结束后会统一进行抽奖。\n中奖名单将在本站公布，敬请期待！");
                }
                else {
                    popWindownBlue( "感谢您的参与，活动结束后会统一进行抽奖。中奖名单将在本站公布，敬请期待！");
                }
                location.href = "/Home";
                return true;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常，网络超时！");
        }
    });
};
//会员答题后问卷提交 
Questionnaire.QuestionnaireSubmitByCS = function (memberId, qid, resultValue, userEmail) {
    $.ajax({
        url: "/Questionnaire/SubmitResultByCS",
        type: "post",
        data: { memberId: memberId, qid: qid, resultValue: encodeURIComponent(resultValue), userEmail: userEmail },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( "数据提交失败,请重试!");
                return false;
            }
            else {
                popWindownBlue( "感谢您的参与，活动结束后我们会为您邮寄精美礼品，敬请期待！");
                location.href = "/Home";
                return true;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常，网络超时！");
        }
    });
};

//保存会员答题记录
Questionnaire.VipSubmit = function (memberId, qid) {
    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/SaveResultByVip",
        type: "post",
        data: { memberId: memberId, qid: qid },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === "" || result.code == "400") {
                popWindownBlue( "数据提交失败,请重试!");
                return false;
            }
            else {
                popWindownBlue( "感谢您的参与，活动结束后会统一进行抽奖，中奖名单将在本站公布，敬请期待！");
                location.href = "/MyCenter/Index";
                return true;
            }
        },
        error: function (err) {
            popWindownBlue( "系统异常，网络超时！");
        }
    });
};

Questionnaire.CheckQuestionnaireState = function (qtype) {
    var _curFrom = $(".Questionnaire #inputLinkFrom").val();
    var _curUserEmail = $(".Questionnaire #inputUserEmail").val();
    $.ajax({
        //此处注册调用的是api中的接口
        url: "/Questionnaire/CheckQuestionnaireState",
        type: "post",
        data: { qtype: qtype },
        dataType: "json",
        success: function (result) {
            if (result !== null || result.code !== "") {
                switch (result.code) {
                    case 201:               //201：活动未开始
                        {
                            popWindownBlue( "本期问卷活动已结束，即将公布获奖名单，敬请期待！");
                            location.href = "/Questionnaire/QuestionnaireHistory";
                            break;
                        }
                    case 203:           //203：活动已结束
                        {
                            popWindownBlue( "本期活动已经结束，点击查看获奖名单!");
                            location.href = "/Questionnaire/Result?qid=0";
                            break;
                        }
                    case 400:               //400：用户未登录
                        {
                            var _curLinkFrom = getQueryString("from");
                            var _curLinkSource = getQueryString("source");
                            var returnUrl = "/Questionnaire/Index";
                            if (_curLinkFrom != null && _curLinkFrom != "") {
                                returnUrl += ("?from=" + _curLinkFrom);
                            }
                            if (_curLinkSource != null && _curLinkSource != "") {
                                if (returnUrl.indexOf("?") > 0)
                                    returnUrl += ("&source=" + _curLinkSource);
                                else
                                    returnUrl += ("?source=" + _curLinkSource);
                            }
                            if (_curFrom == 1) {
                                popWindownBlue( "您还未登录，登录后可参与答题，立即登录？");
                                //登录 
                                location.href = "/Account/LogonPage?returnUrl=" + encodeURIComponent(returnUrl);
                            }
                            else {
                                //账户未登录
                                if (confirm("登录后完成问卷调查可获得蓝豆奖励，是否立即登录？")) {
                                    //登录 
                                    location.href = "/Account/LogonPage?returnUrl=" + encodeURIComponent(returnUrl);
                                }
                            }

                            break;
                        }
                    case 300:
                        {
                            var email = "";
                            //if (_curFrom == 1) {
                            //    while (true) {
                            //        if (_curUserEmail != null && _curUserEmail != "") {
                            //            email = prompt("请确定您的Email地址是否正确", _curUserEmail);
                            //        }
                            //        else {
                            //            email = prompt("请输入您的Email地址", _curUserEmail);
                            //        }
                            //        var emailReg = /^\s*\w+(?:\.{0,1}[\w-]+)*@[a-zA-Z0-9]+(?:[-.][a-zA-Z0-9]+)*\.[a-zA-Z]+\s*$/;

                            //        if (email != null && email != "") {
                            //            if (!emailReg.exec(email)) {
                            //                popWindownBlue( "Email地址格式错误，请重试！");
                            //            } else {
                            //                $(".Questionnaire #inputUserEmail").val(email);
                            //                break;
                            //            }
                            //        }
                            //    }

                            //}
                            break;
                        }
                    case 301:               //301：用户已完成过问卷
                        {
                            if (confirm("您已成功提交过该问卷，是否要继续查看问卷？")) {
                                //设置为只读模式
                                var _inputchild = $(".Questionnaire input,.Questionnaire select,.Questionnaire textarea");
                                _inputchild.each(function () {
                                    $(this).attr("disabled", "disabled");
                                });
                            }
                            else {
                                location.href = "/MyCenter/Index";
                            }
                            break;
                        }
                    default:
                        { break; }
                }
            }
            return false;
        },
        error: function (err) {
            popWindownBlue( "系统异常，网络超时！");
            return false;
        }
    });
};

Questionnaire.Question = function Question() {
    this.data = new Array();
    this.put = function (key, value) {
        this.data[key] = value;
    };
    this.get = function (key) {
        return this.data[key];
    };
    this.remove = function (key) {
        this.data[key] = null;
    };
    this.isempty = function () {
        this.data.length = 0;
    };
    this.size = function () {
        return this.data.length;
    };
    this.append = function (key, value) {
        this.data[key] += "," + value;
    };
    this.isexist = function (key) {
        return this.data.hasOwnProperty(key);
    };
};

//根据省份获取城市
Questionnaire.FindCityByProvince = function (province) {
    $.ajax({
        url: "/Questionnaire/FindCitysByProvince",
        type: "get",
        data: { provinceCode: $(province).val() },
        success: function (result) {
            $("#passerbyAddressCitySpan").html(result);
        }, error: function () {
            popWindownBlue( "城市获取失败");
            return false;
        }

    });
};

//根据城市获取区域
Questionnaire.FindAreasByCity = function (city) {
    $.ajax({
        url: "/Questionnaire/FindAreasByCity",
        type: "get",
        data: { cityCode: $(city).val() },
        success: function (result) {
            $("#passerbyAddressAreaSpan").html(result);
        }, error: function () {
            popWindownBlue( "地区获取失败");
            return false;
        }

    });
};

Questionnaire.ResultLoginCallBack = function () {
    var curQid = Questionnaire.getQueryString("qid");
    var returnUrl = "/Questionnaire/Result?qid=" + curQid;
    location.href = "/Account/LogonPage?returnUrl=" + encodeURIComponent(returnUrl);
}

Questionnaire.ResultRegisterCallBack = function () {
    var curQid = Questionnaire.getQueryString("qid");
    var returnUrl = "/Questionnaire/Result?qid=" + curQid;
    location.href = "/Account/AccountRegister?returnUrl=" + encodeURIComponent(returnUrl) + "&source=blms_questionnaire";
}

Questionnaire.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}