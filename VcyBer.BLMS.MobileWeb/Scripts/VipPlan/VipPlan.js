$(function () {
    /*会员计划菜单收发*/
    $('.mpBox .mpBoxh2').on('click', function () {

        if ($(this).next().css('display') == 'none') {
            $('.h2Pcont').hide();
            $(this).next().stop().slideDown();
            $(this).find('i').removeClass('up');
            $(this).find('i').addClass('down');
        } else {
            $(this).next().stop().slideUp();
            $(this).find('i').removeClass('down');
            $(this).find('i').addClass('up');
        }
    });
})