var MyCenter = {};
//个人中心-我的爱车-认证车主-保存
MyCenter.myCarToCheckCarownerSave = function () {
    var mycenterIdentityReg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    var _identityNumber = $("#mycenter_mycar_tocheckcarowner_layer_identitynumber").val();

    var _paperwork = $("#accountCarPaperWork").val();

    if (!_paperwork || _paperwork < 0)
    {
        popWindownBlue( "请选择证件类型");
        return false;
    }

    if (_identityNumber == null || _identityNumber == "" ) {
        popWindownBlue('请正确输入证件号码');
        return false;
    }

    if (1 == _paperwork) {
        identityReg = /(^\d{15}$)|(^\d{17}([0-9]|X)$)/;
        if (!identityReg.test(_identityNumber)) {
            popWindownBlue( "请正确输入证件号码");
            return false;
        }
    }
    if (2 == _paperwork) {
        identityReg = /^[a-zA-Z0-9]{3,21}$|^(P\d{7})|(G\d{8})$/;
        if (!identityReg.test(_identityNumber)) {
            popWindownBlue( "请正确输入证件号码");
            return false;
        }
    }
    if (3 == _paperwork) {
        identityReg = /^[a-zA-Z0-9]{7,21}$/;
        if (!identityReg.test(_identityNumber)) {
            popWindownBlue( "请正确输入证件号码");
            return false;
        }
    }
    //var _mtype = 2;
    //var _issonata9Obj = $("#mycenter_mycar_tocheckcarowner_layer_issuonata9");
    //if (_issonata9Obj.is(":checked")) {
    //    _mtype = 3;
    //}
    $.ajax({
        url: "/MyCenter/ToCheckCarownerSave",
        type: "post",
        data: { identityNumber: _identityNumber, mtype: _paperwork },
        dataType: "json",
        success: function (result) {
            if (result === null || result.code === null || result.code === "") {
                popWindownBlue( "认证信息保存失败");
                return false;
            }
            if (result.code == "301") {
               // $("#ToCheckCarowner").css("display", "none");
               //  $(".zzDivBox").css('display', 'none');
                layer.close(document.getElementById('mycenter_mycar_tocheckcarowner_layer_layerIndex').value);
                //popWindownBlue(result.msg, function () {
                  // window.location = window.location;

                layer.open({
                    type: 1,
                    skin: 'bluePopup', //样式类名
                    closeBtn: 1, //不显示关闭按钮
                    shift: 2,
                    area:["465px","228px"],
                    shadeClose: false, //开启遮罩关闭
                    content: result.msg,
                    end: function () {
                        //window.location = window.location;
                    },
                    btn: ['确定']
                });
               //});
               // popWindownBlue(result.msg);
                return false;
            }
            if (result.code == "304") {
                popWindownBlue(result.msg);
                return false;
            }
            //认证成功
            if (result.code == "302") {

                $("#ToCheckCarowner").css("display", "none");
                $(".zzDivBox").css('display', 'none');
                //window.location.href = "/Sonata/SonataActive";
                popWindownBlue("车辆绑定成功", function () {
                    window.location.href = "/MyCenter/Index";
                });
               
                return false;
            }
            //认证成功
            if (result.code == "200") {
                $("#ToCheckCarowner").css("display", "none");
                $(".zzDivBox").css('display', 'none');
                popWindownBlue("车辆绑定成功", function () {
                    window.location.href = "/MyCenter/Index";
                });
            }
           
        }, error: function () {

        }
    });
};

MyCenter.CarownerToActiveMember = function () {

};