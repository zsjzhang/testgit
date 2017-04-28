//汽车跑动画
function startCar(level) {
    gmInterval = setInterval(function () {
        switch (level) {
            case 1:
            case 2:
            case 3:
                if (px >= 180) {
                    window.clearInterval(gmInterval);
                }
                px += 10;
                break;
            case 4:
                px += 10;
                if (px >= 460) {
                    py += 10;
                }
                if (py >= 80) {
                    window.clearInterval(gmInterval);
                }
                break;
            case 5:
                if (py <= 80) {
                    px += 10;
                } else if (py >= 80) {
                    px -= 10;
                    if (px == 310) {
                        window.clearInterval(gmInterval);
                    }
                }
                if (px >= 460) {
                    py += 10;
                }
                break;
            case 6:
                if (py <= 80) {
                    px += 10;
                } else if (py >= 80) {
                    px -= 10;
                    if (px <= 60) {
                        py += 10;
                        if (py == 230) {
                            window.clearInterval(gmInterval);
                        }
                    }
                }
                if (px >= 460) {
                    py += 10;
                }
                break;
            case 7:
                draw78(190);
                break;
            case 8:
                draw78(500);
                break;
            default:
        }
        ctx.clearRect(0, 0, 680, 420);
        ctx.beginPath();
        ctx.drawImage(imgGame, 0, 0, 680, 420);
        ctx.drawImage(imgCar, px, py, 140, 80);
    }, 10);
}
//跑到第七八个奖品
function draw78(num) {
    if (px < 460 && py == 0) {
        px += 10;
    }
    else if (px >= 460 && py < 80) {
        px += 10;
        py += 10;
    }
    else if (px > 460 && py >= 80 && py < 160) {
        px -= 10;
        py += 10;
    }
    else if (px > 80 && px <= 460 && py == 160) {
        px -= 10;
    }
    else if (px <= 80 && py < 240) {
        px -= 10;
        py += 10;
    }
    else if (px < 80 && py >= 240) {
        px += 10;
        py += 10;
    }
    else if (px < num && py == 320) {
        px += 10;
    }
    else {
        window.clearInterval(gmInterval);
    }
}

function canvasInit() {
    if ($('#gameCanvas').length <= 0) {
        canvas = document.createElement('canvas');
        canvas.setAttribute('id', 'gameCanvas');
        canvas.setAttribute('width', 680);
        canvas.setAttribute('height', 420);
        canvas.innerHTML = '你的游览器不支持html5的画布元素，请升级到IE9+或使用firefox、chrome这类高级的智能游览器！';
        $('#divCanvas').html(canvas);
        $('#imgGamebg').hide();
    }
    canvas = document.getElementById('gameCanvas');
    if (typeof window.G_vmlCanvasManager != 'undefined') {
        canvas = window.G_vmlCanvasManager.initElement(canvas);
        ctx = canvas.getContext('2d');
    } else {
        ctx = canvas.getContext('2d');
    }
}
//初始化汽车游戏
function drawInit() {
    if (gmInterval != null) {
        clearInterval(gmInterval);
    }
    px = 0;
    py = 0;
    ctx.clearRect(0, 0, 680, 420);
    ctx.beginPath();
    ctx.drawImage(imgGame, 0, 0, 680, 420);
    ctx.drawImage(imgCar, 0, 0, 140, 80);
}

//开始请求后台游戏
function startGame() {
    canvasInit();
    drawInit();
    //return;
    $('#btnStart').removeAttr('onclick', 'start()');
    var source=$('#source').val();
    $.post('/MoreMember/StartActivity', { source: source }, function (reponse) {
        if (reponse.Code == -1) {
            showAgain();
            $('#btnStart').attr('onclick', 'start()');
        } else if (reponse.Code == 0) {
            var plevel = reponse.PrizesInfo.PrizeLevel;
            $('#hBeans').html(levelArray[plevel - 1] + '！');
            $('#pBeans').html(wordsArray[plevel - 1]);
            $('#winningId').val(reponse.WinningInfo.Id);
            startPoint();
            stopGame(plevel);
            setTimeout(function () {
                showBeans();
                $('#btnStart').attr('onclick', 'start()');
            }, 2000);
        }
        else if (reponse.Code == 1) {
            var plevel = reponse.PrizesInfo.PrizeLevel;
            prizeLevel = plevel;
            $('#hGoods').html('恭喜，中' + levelArray[plevel - 1] + '啦！');
            $('#pGoods').html(wordsArray[plevel - 1]);
            $('#winningId').val(reponse.WinningInfo.Id);
            startPoint();
            stopGame(plevel);
            setTimeout(function () {
                showGoods();
                $('#btnStart').attr('onclick', 'start()');
            }, 2000);
        }
        else if (reponse.Code == -2) {
            showJoin();
            $('#btnStart').attr('onclick', 'start()');
        }
    }, 'json');

}
//修改中奖信息
function editInfos() {
    var winId = $('#winningId').val();
    var userName = $('#userName').val().trim();
    var userPhone = $('#userPhone').val().trim();
    var userAddress = $('#userAddress').val().trim();
    var Province = $('#Province').find("option:selected").text();
    var City = $('#City').find("option:selected").text();
    var Area = $('#Area').val();

    if (userName == '') {
        layer.msg('大人怎么称呼！');
        return;
    }
    if (userPhone == '') {
        layer.msg('大人留下手机号码！');
        return;
    }
    var reg = /^(\+\d{2,3}\-)?\d{11}$/;
    if (userPhone.length != 11 || !reg.test(userPhone)) {
        layer.msg("请输入正确的手机号");
        return false;
    }
    if (Area <= 0) {
        layer.msg('大人请选择省市县！');
        return;
    }
    if (userAddress == '') {
        layer.msg('大人现居何处！');
        return;
    }
    Area = $('#Area').find("option:selected").text();
    $.post('/MoreMember/PerfectInformation', {
        'id': winId, 'name': userName, 'phone': userPhone, 'address': userAddress, 'province': Province,
        'city': City, 'area': Area,
    }, function (response) {
        if (response == 'ok') {
            $('#userName').val('');
            $('#userAddress').val('');
            showIknow();
        } else {
            layer.msg('亲，信息有错误哦！');
        }
    })
}



var pageLayer;
//飞车主会员
function showJoin() {
    showLayer('#divJoin', '528px', '165px');
}
//登录注册框
function showLogin() {
    showLayer('#divLogin', '528px', '298px');
}
//蓝豆提示框
function showBeans() {
    showLayer('#divBeans', '528px', '235px');
}
//中奖信息框
function showGoods() {
    showLayer('#divGoods', '528px', '268px');
}
//填写信息框
function showInfos() {
    hideLayer();
    $('#hLevel').html('恭喜您，抽中' + prizeArray[prizeLevel - 1] + '奖品!');
    showLayer('#divInfos', '460px', '368px');
}
//中奖完成框
function showIknow() {
    hideLayer();
    showLayer('#divIknow', '528px', '220px');
}
function showAgain() {
    //hideLayer();
    showLayer('#divAgain', '528px', '235px');
}
function showQRCode() {
   layer.open({
        type: 1,
        shade: [0],
        area: ['280px', '280px'],
        title: false,
        border: [0],
        content: $('#divQRCode') 
    });
}

//弹出层
function showLayer(obj, width, height) {
    pageLayer = layer.open({
        type: 1,
        shade: [0.7, '#000'], //遮罩层
        closeBtn: [0, false], //去掉默认关闭按钮
        area: [width, height],//宽高
        title: false,
        border: [0],
        content:$(obj) 
    });
}
//隐藏弹出层
function hideLayer() {
    layer.close(pageLayer);
    if (ctx != null) {
        drawInit();
    } 
}

$(function () {
    imgGame = document.getElementById('imgGame');
    imgCar = document.getElementById('imgCar');



    //ctx.drawImage(imgCar, 180, 0);
    //ctx.drawImage(imgCar, 540, 80);
    //ctx.drawImage(imgCar, 310, 160);
    //ctx.drawImage(imgCar, 0, 230);
    //ctx.drawImage(imgCar, 190, 320);
    //ctx.drawImage(imgCar, 520, 320);


    //中奖信息滚动
    var scrollInterval = window.setInterval(function () {
        var html = $('#trScroll').find('tr:first').html();
        $('#trScroll').find('tr:first').fadeOut(1000).remove();
        $('#trScroll').append('<tr>' + html + '</tr>');
    }, 1000);


    $('header').height($(window).width() * ($('.bg-header').height() / $('.bg-header').width()));
    $('#pShare').css("padding-top", 400 * $('header').height() / 800);
    $('.bg-body').height($(document).height());
    $(window).bind("resize", resizeWindow);
    function resizeWindow() {
        $('header').height($(window).width() * ($('.bg-header').height() / $('.bg-header').width()));
        $('#pShare').css("padding-top", 400 * $('header').height() / 800);
        if ($('.bg-body').height() < $(document).height()) {
            $('.bg-body').height($(document).height());
        }
        if ($('.bg-body').height() > $(document).height()) {
            $('.bg-body').height($(document).height());
        }
    }
})
