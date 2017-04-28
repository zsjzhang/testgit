(function ($) {
    $.fn.slider = function (options) {
        options = $.extend({
            nextBtn: null,
            prevBtn: null,
            nums: false
        },
		options || {});
        this.each(function () {//循环取dom节点
            var c = $(this),
				ul = $("ul", c),
				li = $("li", ul),
				cur = 0,
				stime;

            function slider() {
                if (options.times == true) {
                    stime = setInterval(function () {
                        cur += 1;
                        if (cur >= li.length) {
                            cur = 0;
                        }
                        $("#slider>ol>li").eq(cur).addClass("add").siblings().removeClass('add')
                        ul.css({
                            "left": -cur * li.width()
                        })
                    }, options.timer)
                }
            }
            slider();
            li.hover(function () {
                clearInterval(stime)
            }, function () {
                slider();
            })

            //添加小按钮start
            if (options.nums == true) {

                //var len = this.children().children().length;
                var str = '';
                //console.log(li.length)
                for (var i = 0; i < li.length; i++) {
                    s = i + 1
                    //str += '<li>'+s+'</li>';//带字
                    if (i == 0) {
                        str = '<li class="add"></li>';//不带字	
                    } else {
                        str += '<li></li>';//不带字
                    }

                };

                c.append('<ol>' + str + '</ol>');

                var ol = $('ol', c),
					oli = $('li', ol);

                oli.on('click', function () {
                    var index = $(this).index();
                    cur = index;
                    $(this).addClass("add").siblings().removeClass("add")
                    clearInterval(stime)
                    ul.css({
                        "left": -cur * li.width()
                    })
                })


            }//添加小按钮end

            ul.css('width', li.length * li.width());
            //li.css('width',$(window).width());
            if (options.nextBtn) $(options.nextBtn).on('click', function () {
                clearInterval(stime)
                cur += 1;
                if (cur >= li.length) {
                    cur = 0;
                }
                ul.css({
                    "left": -cur * li.width()
                })
            })

            if (options.prevBtn) $(options.prevBtn).on('click', function () {
                clearInterval(stime)
                cur -= 1;
                if (cur < 0) {
                    cur = 4;
                }
                ul.css({
                    "left": -cur * li.width()
                })
            })

        })

    }
})(jQuery);
