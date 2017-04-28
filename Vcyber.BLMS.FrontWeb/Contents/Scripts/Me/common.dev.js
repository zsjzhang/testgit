/*
弹出层

update tiem：2015-03-27 22:16

demo:
以下代码将生成两个按钮
mobileAlert("这是提示内容", null, {
            buttons: [
                {
                    id: "popBtn1",
                    value: "这是按钮一",
                    callback: function () {
                        popWindownBlue( "您点击了按钮一");
                    }
                }, {
                    id: "popBtn2",
                    value: "这是按钮二",
                    callback: function () {
                        popWindownBlue( "您点击了按钮二");
                    }
                }
            ]
        });

以下代码存在关闭回调
mobileAlert("这是提示内容", function () {
            popWindownBlue( "这是关闭的回调函数");
        });

以下代码仅弹出提示
mobileAlert("这是提示内容");
*/

function mobileAlert(msg, fnClose, settings) {
    var settingDefault = {
        buttons: []
    };
    settings = common.formatAPIParam(settingDefault, settings, false);

    var buttonHtmlTemp = "<div style=\"width:{num}%;float:left;\"><input type=\"button\" id=\"{id}\" value=\"{value}\" class=\"btn\"></div>";
    var html = "\
    <div id=\"popOverlay\" class=\"overlay\">\
        <section class=\"modal\" id=\"info\">\
            <div class=\"container\">\
                <div class=\"content\">\
                    <span id=\"closePopOverlay\" class=\"modal-close close\"></span>\
                    <div class=\"modal-bd\">\
                       <strong class=\"tit\">{msg}</strong>\
                        <div class=\"overlaybtndiv\">\
                            {buttons}\
                        </div>\
                    </div>\
                </div>\
            </div>\
        </section>\
    </div>";
    //替换弹出提示
    html = html.replace(/{msg}/g, msg);
    //组装按钮
    var buttonListHtml = "";
    $.each(settings.buttons, function (index, item) {
        var buttonHtml = buttonHtmlTemp;
        buttonHtml = buttonHtml.replace(/{id}/g, item.id);
        buttonHtml = buttonHtml.replace(/{value}/g, item.value);
        buttonHtml = buttonHtml.replace(/{num}/g, 100/settings.buttons.length);
        buttonListHtml += buttonHtml;
    });
    //添加按钮
    html = html.replace(/{buttons}/g, buttonListHtml);
    //将弹出组件添加到body
    $("body").append(html);
    //绑定按钮事件

    //为弹出做准备
    var $overlay = $("#popOverlay");
    $overlay.addClass("active").children().addClass("modal-in");
    $("#closePopOverlay").unbind("click").on("click", function () {
        $overlay.removeClass("active").children().removeClass("modal-in");
        $overlay.remove();
        if (fnClose) { fnClose(); }
    });

    $.each(settings.buttons, function (index, item) {
        $("#popOverlay").find("#" + item.id).bind("click", function () { 
            if (item.callback) {
                item.callback();
            }
        });
    });
}
function popOverlayClose() {
    var $overlay = $("#popOverlay");
    $overlay.removeClass("active").children().removeClass("modal-in");
    $overlay.remove();
}