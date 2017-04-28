$(document).ready(function () {
    $('.plan ol li').bind('mouseover', function () {
        var index = $(this).index();
        $('.plan ol li').eq(index).addClass('on').siblings().removeClass('on');
        $('.plan>ul>li').eq(index).addClass('curTags').siblings().removeClass('curTags');
    });
    $('.new_1 ul li').bind('click', function () {
        var index = $(this).index();
        $('.new_1 ul li').eq(index).addClass('on').siblings().removeClass('on');
    });
    $('.exercise li').bind('click', function () {
        var index = $(this).index();
        $('.exercise li').eq(index).addClass('on').siblings().removeClass('on');
        $('.exercise_2>div').eq(index).addClass('curTags').siblings().removeClass('curTags');
    });
    $(".treat li").hover(function () {
        $(this).find(".b").slideToggle("slow");
    });




    //  µ¯´°

    $('.true_pay').click(function () {
        $('.pay_r_bg').css("display", "block");
        $('.pay_result').css("display", "block");

    })

    $('.apply_member').click(function () {
        $('.zhuce_r_bg').css("display", "block");
        $('.zhuce_result').css("display", "block");

    })

    $('.fapiao_upload').click(function () {
        $('.upload_bg').css("display", "block");
        $('.upload').css("display", "block");

    })

    $('.true_upload').click(function () {
        $('.upload_bg').css("display", "block");
        $('.upload_scs').css("display", "block");

    })


    $('.fix_ul>li').hover(
        function () {
            var index = $(".fix_ul>li").index(this);
            if (index <= 0) {
                return false;
            }
            $(".hide_ul>li").eq(index).show().siblings('li').hide();
            $(".hide_ul>li").mouseover(function () {
                $(".hide_ul>li").eq(index).show().siblings('li').hide();
            });
            $(".hide_ul>li").mouseout(function () {
                $(".hide_ul>li").hide();
            });
        },
        function () {
            $(".hide_ul>li").hide();
        });

})





















