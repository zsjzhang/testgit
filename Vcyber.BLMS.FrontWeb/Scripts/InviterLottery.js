var InviterLottery = {};
InviterLottery.GetInviterLottery = function (params) {
    $.ajax({
        type: "POST",
        url: "/OrderChange/GetInviterLottery",
        data: params,
        dataType: "json",
        success: function (data) {
            InviterLottery.PopWindownBlue(data.msg);
        }
    });
};
InviterLottery.PopWindownBlue = function (msg, callBack) {
    layer.open({
        type: 1,
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: false, //开启遮罩关闭
        content: msg,
        end: function () {
            if (callBack) {
                callBack();
            }
        },
        btn: ['确定']
    });
};
$(function () {

});