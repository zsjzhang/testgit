$(function () {
    $("img").unbind("click");
    $("button").unbind("click");
    $("div").unbind("click");
    $("input").unbind("click");
    $("a").unbind("click");
    $("form").attr("id", "");
    $("img").attr("onclick", "");
    $("a").attr("onclick", "");
    $("input").attr("onclick", "");
    $("button").attr("onclick", "");
    popWindownBlue( "活动已结束，请关注其他活动，卡券有效期截止到7月31日，请及时核销 ");
});