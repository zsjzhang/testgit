$(function () {

    if ($("#SourceType").val() == "1") {
        Activity();
    }
    $('.homeOneBtn').on('click', function () {
        $('.homeImgBox').css('display', 'none');
        $(this).css('display', 'none');
        $('.homeP2').css('display', 'block');
    });


    $('.homeP2 dd:eq(0) a').on('click', function () {
        var indexI = $(this).index();
        $('.homeImgLiBox li').css('display', 'none');
        $('.fixBox').css('display', 'block');
        $('.homeImgLiBox').css('display', 'block');
        $('.homeImgLiBox li').eq(indexI).css('display', 'block');
    });
    $('.homeP2 dd:eq(1) a').on('click', function () {
        var indexI = $(this).index();
        $('.homeImgLiBox li').css('display', 'none');
        $('.fixBox').css('display', 'block');
        $('.homeImgLiBox').css('display', 'block');
        $('.homeImgLiBox li').eq(indexI + 4).css('display', 'block');
    });


    $('.imgLiBoxClose').on('click', function () {
        $('.fixBox').css('display', 'none');
        $('.homeImgLiBox').css('display', 'none');
    });
});

function Activity() {
    $('.homeImgBox').css('display', 'none');
    $('.homeOneBtn').css('display', 'none');
    $('.homeP2').css('display', 'block');
}

var OrderChange = {};
OrderChange.IsCanInviter = function (flag) {
    $.ajax({
        type: "POST",
        url: "/OrderChange/IsCanInviter",
        data: {},
        dataType: "json",
        success: function (data) {
            if (data.code == "001") {
                window.location.href = "/Account/LogonPage?returnUrl=/OrderChange/index?source=" + $("#hidSource").val() + "|flag=" + flag;
            }
            else if (data.code == "002") {
                window.location.href = "/OrderChange/BangDing?source=" + $("#hidSource").val() + "&flag=" + flag;
            }
            else {
                window.location.href = "/OrderChange/TuiJian?source=" + $("#hidSource").val() + "&flag=" + flag;
            }
        }
    });
};
