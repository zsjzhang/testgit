$(function () {
    $('.homeUp .sdNav img').on('click', function () {
        $('.upBg').css('display', 'block');
        $('.upBg li').eq($(this).index()).addClass('active');
    });
    $('.colseBtn').on('click', function () {
        $('.upBg li').removeClass('active');
        $('.upBg').css('display', 'none');
        $('.downBg li').removeClass('active');
        $('.downBg').css('display', 'none');
    });
    $('.homeDown .sdNav img').on('click', function () {
        $('.downBg').css('display', 'block');
        $('.downBg li').eq($(this).index()).addClass('active');
    });
});
var OrderChange = {};
OrderChange.IsCanInviter = function (userId, flag) {
    $.ajax({
        type: "POST",
        url: common.resolveUrl("/OrderChange/IsCanInviter"),
        data: { userId: userId },
        dataType: "json",
        success: function (data) {
            if (data.code=="001") {
                window.location.href = common.resolveUrl("/OrderChange/Login?returnUrl=OrderChange/index?source=" + $("#hidSource").val());
            }
            else if (data.code == "002") {
                window.location.href = common.resolveUrl("/OrderChange/BindCar?source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val() + "&flag=" + flag);
            }
            else {
                window.location.href = common.resolveUrl("/OrderChange/Inviter?source=" + $("#hidSource").val() + "&userId=" + $("#hidUserId").val() + "&flag=" + flag);
            }
        }
    });
};
new Swiper('.swiper-container', {
    direction: 'vertical',
    effect: 'fade',
    lazyLoading: true,
    paginationClickable: true,
    onTouchStart: function () {

        $('.homeUp .sdNav a').on('click', function () {
            $('.downMaskDiv').css('display', 'block');
            $('.upBg').css('display', 'block');
            $('.upBg li').eq($(this).index()).addClass('active');
        });
        $('.colseBtn').on('click', function () {
            $('.upMaskDiv').css('display', 'none');
            $('.downMaskDiv').css('display', 'none');
            $('.upBg li').removeClass('active');
            $('.upBg').css('display', 'none');
            $('.downBg li').removeClass('active');
            $('.downBg').css('display', 'none');
        });
        $('.homeDown .sdNav a').on('click', function () {
            $('.upMaskDiv').css('display', 'block');
            $('.downBg').css('display', 'block');
            $('.downBg li').eq($(this).index()).addClass('active');
        });

    }
});