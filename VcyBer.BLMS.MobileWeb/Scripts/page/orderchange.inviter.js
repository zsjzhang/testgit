var OrderChange = {};
OrderChange.AddXDInviter = function (params) {
    $.ajax({
        type: "POST",
        url: common.resolveUrl("/OrderChange/AddXDInviter"),
        data: { inviters: params },
        dataType: "json",
        success: function (data) {
            if (data.code=="200") {
                window.location.href = common.resolveUrl("/OrderChange/Lottery?source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val() + "&flag=" + $("#hidFlag").val() + "&pagesource=tj");
            }
            else {
                if (data.code == "201") {
                    layer.open({
                        type: 1,
                        title: false,
                        area: ['300px', '180px'],
                        skin: 'bluePopup', //样式类名
                        closeBtn: 1, //不显示关闭按钮
                        shift: 2,
                        shadeClose: true, //开启遮罩关闭
                        content: '<h2>恭喜您！成功推荐好友购车！</h2>',
                        btn: ['更多好礼', '分享活动'],
                        yes: function (index, layero) {
                            window.location.href = common.resolveUrl("/OrderChange/Activity?source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val());
                        },
                        btn2: function (index, layero) {
                            var url = common.api("/OrderChange/Share");
                            window.location = url;
                        }
                    });
                }
                else {
                    OrderChange.PopWindownBlue(data.msg);
                }
            }
        }
    });
};
OrderChange.PopWindownBlue = function (msg) {
    layer.open({
        type: 1,
        title: '信息',
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: true, //开启遮罩关闭
        content: '<h2>' + msg + '</h2>'
    });
};
$(function () {
    $('.tuijian em i').on('click', function () {
        if ($(this).text() == '+') {
            $(this).text('-');
            $('.colonDiv').css('display', 'block');
        } else {
            $(this).text('+');
            $('.colonDiv').css('display', 'none');
        }
    });

    $("#btnInviter").click(function () {
        var params = [];
        if ($("#username1").val() == null || $("#username1").val() == "") {
            OrderChange.PopWindownBlue("姓名不能为空！");
            return;
        }
        if ($("#phone1").val() == null || $("#phone1").val() == "") {
            OrderChange.PopWindownBlue("电话不能为空！");
            return;
        }
        var phoneReg = new RegExp("^1[0-9]{10}$");
        if (!phoneReg.test($("#phone1").val())) {
            OrderChange.PopWindownBlue("电话不正确！");
            return;
        }
        params.push({
            InviterUserId: $("#hidUserId").val(),
            InviteredName: $("#username1").val(),
            InviteredMobile: $("#phone1").val(),
            InviteredCar: $("#hidFlag").val() == 1 ? "领动" : "全新胜达",
            InviterSource: $("#hidSource").val()
        });
        if ($('.colonDiv').css('display')=="block") {
            if ($("#username2").val() == null || $("#username2").val() == "") {
                OrderChange.PopWindownBlue("姓名不能为空！");
                return;
            }
            if ($("#phone2").val() == null || $("#phone2").val() == "") {
                OrderChange.PopWindownBlue("电话不能为空！");
                return;
            }
            if (!phoneReg.test($("#phone2").val())) {
                OrderChange.PopWindownBlue("电话不正确！");
                return;
            }
            if ($("#phone1").val() == $("#phone2").val()) {
                OrderChange.PopWindownBlue("电话不能重复！");
                return;
            }
            params.push({
                InviterUserId: $("#hidUserId").val(),
                InviteredName: $("#username2").val(),
                InviteredMobile: $("#phone2").val(),
                InviteredCar: $("#hidFlag").val() == 1 ? "领动" : "全新胜达",
                InviterSource: $("#hidSource").val()
            });
        }
        var inviters = new Object();
        inviters.xDInviters = params;
        OrderChange.AddXDInviter(JSON.stringify(inviters));
    });
});