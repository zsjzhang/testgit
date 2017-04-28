/*手机自适应设置*/
window.onload = window.onresize = window.onscroll = function () {
    fontSize();
    pageShow();
};

function pageShow() {
    var oBox = document.getElementsByTagName('body')[0];
    oBox.style.visibility = 'visible';
}

function fontSize() {
    document.documentElement.style.fontSize = 100 * (document.documentElement.clientWidth / 640) + 'px';
}

//input输入框光标点入，默认文字消失
function inpTextF(id, text) {
    $(id).focus(function () {
        $(this).attr('placeholder', '');
    });
    $(id).blur(function () {
        if ($(this).attr('placeholder') == '') {
            $(this).attr('placeholder', text);
        }
    });
}
//成功消息弹框样式
function AlertSuccess(msg, callBack) {
    layer.open({
        type: 1,
        skin: 'bmPop',
        title: 0,
        shift: 2,
        closeBtn: 0,
        shadeClose: true,
        area: ['5.4rem', '3.23rem'],
        btn: ['确定'],
        content: '<img src="../img/boy1.png" /><span>' + msg + '</span>',
        end: function () {
            if (callBack) {
                callBack();
            }
        }
    });
}
//失败消息弹框样式
function AlertFalse(msg, callBack) {
    layer.open({
        type: 1,
        skin: 'bmPopErr',
        title: 0,
        shift: 2,
        closeBtn: 0,
        shadeClose: true,
        area: ['5.4rem', '3.23rem'],
        btn: ['确定'],
        content: '<img src="../img/boy_err.png" /><span>' + msg + '</span>',
        end: function () {
            if (callBack) {
                callBack();
            }
        }
    });
}