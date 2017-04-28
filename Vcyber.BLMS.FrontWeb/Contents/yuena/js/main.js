$(function () {
    $(window).load(function(){
        var bodyHeight = $(window).height();
        var bodyWidth = $(window).width();
        if( bodyHeight<762||bodyWidth<1542){
            $('.index').addClass('small');
            $('.index2').addClass('small');
            $('.indexVerna').addClass('small');

        }else{
            $('.index').removeClass('small');
            $('.index2').removeClass('small');
            $('.indexVerna').removeClass('small');
        }
    });
    if (screen.width >= 1920) { 
        //$(".swiper-slide > img").css({ "margin-top": "-30px", "margin-left": "5%" })
        $(".swiper-slide > img").css({"width":"90%","margin-top":"0px","margin-left":"5%"})
    };
    if (screen.width <= 1366) {
        $(".swiper-slide > img").css({ "width": "90%", "margin-top": "-30px", "margin-left": "5%" })
    }
    //if (screen.width <= 1366) {
    //    $(".indexVerna").css({ "width": "90%", "margin-top": "-230px", "margin-left": "5%" })
    //}

    $(window).resize(function(){
        $(window).trigger('load');
    });
    //$('.gua_mask').click(function(){
    //    $(this).attr('src', '/Contents/yuena/images/ggk.gif');
    //});
    
	$('.close').click(function(){
		$(this).parent().fadeOut(200);
		$('.cover').fadeOut(200);
	});
	$('.close2').click(function(){
		$('.czrz_prize').fadeOut(200);
		$('.cover').fadeOut(200);
	});
	$('.close3').click(function(){
		$(this).parent().fadeOut(200);
	});
	$('.formtips').focus(function(){
		var $parent = $(this).parent();
		if($(this).val() == this.defaultValue){
			$(this).val("");
		}
	}).blur(function(){
		if($(this).val()==''){
			$(this).val(this.defaultValue);
		}
	}); 
	showTime();
	function showTime(){
		//var curTime = new Date();
		//var endTime = new Date("October,12,2016");
		//var leftTime = Math.ceil((endTime.getTime() - curTime.getTime()) / (24 * 60 * 60 * 1000));
	    var leftTime = parseInt($("#curr_date").val())
	    if (leftTime >= 0 && leftTime<=8) {
	        $('.count').find('>img').hide().eq(leftTime).show();
	    }
        else{
            $('.count').find('>img').hide().eq(0).show();
	    }
		if(leftTime>6){
			$(".slideDot").find('li:lt(1)').addClass('cur');
		}else if(leftTime>4){
			$(".slideDot").find('li:lt(2)').addClass('cur');
		}else if(leftTime>2){
			$(".slideDot").find('li:lt(3)').addClass('cur');
		}else{
			$(".slideDot").find('li').addClass('cur');
		}
	}
	setTimeout(showTime,500);
	$('.btn_sell1').click(function(){
	    if($(this).hasClass('cur')){
	        $('.selldot').eq(0).show();
	    }
	});
	$('.btn_sell2').click(function(){
	    if($(this).hasClass('cur')){
	        $('.selldot').eq(1).show();
	    } 
	});
	$('.btn_sell3').click(function(){
	    if($(this).hasClass('cur')){
	        $('.selldot').eq(2).show();
	    } 
	});
	$('.btn_sell4').click(function(){
	    if($(this).hasClass('cur')){
	        $('.selldot').eq(3).show();
	    } 
	});
	$("#btn_addFri").click(function () {
	    if ($(".friendMsg").length > 4) {
	        popWindownBlue("最多推荐5位好友。");
	        return false;
	    }
	    $('.friends_msg').append('<div class="friendMsg"><dl><dt>好友姓名：</dt><dd><input name="friendname" type="text"></dd></dl><dl><dt>好友电话：</dt><dd><input name="friendnumber" type="text"></dd></dl></div>');

	});
});
