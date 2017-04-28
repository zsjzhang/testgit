$(function () {
    //转盘定时器开启
    var i = 0;
    var s = 0;
    var t = 0;
    var timer = null;
    var bRotate = false;
    clearInterval(timer);
    function AwardScroll() {
        if (i >= 360) {
            i = 0
        } else {
            i += 0.05
        }
        $("#rotate").rotate({
            angle: i,
        });
    }
    timer = setInterval(AwardScroll, 10);

    //当转盘超过6圈时，网络超时
    var rotateTimeOut = function () {
        $('#rotate').rotate({
            angle: 0,
            animateTo: 2160,
            duration: 8000,
            callback: function () {
                alert('网络超时，请检查您的网络设置！');
            }
        });
    };

    var bRotate = false;

    //设置转盘转动的角度，时间
    var timer1 = null;
    var rotateFn = function (awards, angles, txt) {
        bRotate = !bRotate;
        $('#rotate').stopRotate();
        $('#rotate').rotate({
            angle: 0,
            animateTo: angles + 1800,
            duration: 5000,
            callback: function () {
                /*console.log(awards)*/
                clearTimeout(timer1)
                var noaward1 = '<div class="result_bg"></div><div class="noaward"><div class="awardb"><p></p><a href="/BlueActivity/address?s=' + s + '&t=' + t + '" id="btn" target="_self">请填写邮寄地址</a></div></div>';
                var noaward2 = '<div class="result_bg"></div><div class="noaward"><div class="awardb"><p></p><a href="javascript:;" id="btn">继续参与</a></div></div>';

                if (parseInt(awards) % 2 > 0) {
                    $("body").append(noaward1);
                    $(".noaward .awardb").addClass("luck1")
                }
                else {
                    $("body").append(noaward2);
                    $(".noaward .awardb").addClass("luck2")
                }

                $(".awardb p").html(txt);
                bRotate = !bRotate;
                if ($('#btn').text() != '请填写邮寄地址') {
                    timer1 = setTimeout(function () {
                        $(".result_bg").remove();
                        $(".noaward").remove();
                        timer = setInterval(AwardScroll, 10);
                    }, 3000)
                }
            }
        })
    };
    //点击箭头开始转动
    var rotateSelect = function () {
        t = 0;
        s = 0;
        clearTimeout(timer1)
        clearInterval(timer);

        //转盘转动时，以及提示框出现时，点击转动无效
        if (bRotate) return;
        $(".result_bg").remove();
        $(".noaward").remove();

        if (bRotate) return;
        //如果time=0网络延迟
        //var time = [0, 1];
        //time = time[Math.floor(Math.random() * time.length)];
        //if (time == 0) {
        //    rotateTimeOut(); //网络延时
        //};
        //if (bRotate) return;
        $.ajax({
            type: 'POST',
            url: "/BlueActivity/DrawLuck",
            data: {},
            success: function (data) {
                if (data.Success) {
                    var item = data.Number;
                    t = item;
                    s = data.WinId;
                    switch (item) {
                        case 0:
                            rotateFn(0, 0, '不好意思，离奖品只差一步哦！');
                            break;
                        case 1:
                            rotateFn(1, 90, '恭喜您，抽中usb充电器一个，奖品稍后（活动结束后5个工作日内）会统一安排邮寄，请保持手机畅通！');
                            break;
                        case 2:
                            rotateFn(2, 180, '不好意思，离奖品只差一步哦！');
                            break;
                        case 3:
                            rotateFn(3, 270, '恭喜您，抽中卡片移动电源一个，奖品稍后（活动结束后5个工作日内）会统一安排邮寄，请保持手机畅通！');
                            break;
                    }
                    $('#blue').text(parseInt($(".numb").find("i").text()) - 100);
                } else {
                    layer.open({
                        type: 1,
                        skin: 'bluePopup', //样式类名
                        closeBtn: 1, //不显示关闭按钮
                        shift: 2,
                        shadeClose: true, //开启遮罩关闭
                        content: data.Msg,
                        btn: ['确定']
                    });
                }
            },
            error: function () {
                alert("未中奖");
            },
            dataType: "json"
        });
    }


    $(document).on('click', '#btn', function () {
        if (parseInt($(".numb").find("i").text()) < 100) {
            bRotate = true;
            layer.open({
                type: 1,
                skin: 'bluePopup', //样式类名
                closeBtn: 1, //不显示关闭按钮
                shift: 2,
                shadeClose: true, //开启遮罩关闭
                content: '您好，您的蓝豆不足，无法兑换抽奖机会哦。',
                btn: ['确定']
            });
            $(".result_bg").remove();
            $(".noaward").remove();
            setInterval(AwardScroll, 10);
        } else {
            //判断是否填写邮寄地址
            if ($(this).text() == '请填写邮寄地址') {
                return;
            } else {
                //layer.msg('消耗100蓝豆', { skin: 'bluePopupMesg' });
                rotateSelect()
            }
        }
    });
    $('#Pointer').click(function () {

        if (parseInt($(".numb").find("i").text()) < 100) {
            bRotate = true;
            layer.open({
                type: 1,
                skin: 'bluePopup', //样式类名
                closeBtn: 1, //不显示关闭按钮
                shift: 2,
                shadeClose: true, //开启遮罩关闭
                content: '您的蓝豆不足，无法抽奖',
                btn: ['确定']
            });

            $(".result_bg").remove();
            $(".noaward").remove();
            setInterval(AwardScroll, 10);
        } else {

            layer.confirm('您确定要抽奖吗?<br>每次抽奖消耗100蓝豆', {
                skin: 'bluePopup',
                btn: ['确定', '取消'] //按钮

            }, function (i) {
                layer.close(i);
                rotateSelect();
            }, function () {
                AwardScroll();
            });

        }
    })

});
function rnd(n, m) {
    return Math.floor(Math.random() * (m - n + 1) + n)
}
