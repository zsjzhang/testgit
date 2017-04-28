
//汽车跑动画
function startCar(level) {
    drawInit();
    gmInterval = setInterval(function () {
        ctx.clearRect(0, 0, 320, 210);
        ctx.drawImage(imgGame, 0, 0, 320, 210);
        switch (level) {
            case 1:
            case 2:
            case 3:
                if (px >= 90) {
                    window.clearInterval(gmInterval);
                }
                px += 5;
                break;
            case 4:
                px += 5;
                if (px >= 220) {
                    py += 5;
                }
                if (py >= 40) {
                    window.clearInterval(gmInterval);
                }
                break;
            case 5:
                if (py <= 40) {
                    px += 5;
                } else if (py >= 40) {
                    px -= 5;
                    if (px == 145) {
                        window.clearInterval(gmInterval);
                    }
                }
                if (px >= 220) {
                    py += 5;
                }
                break;
            case 6:
                if (py <= 40) {
                    px += 5;
                } else if (py >= 40) {
                    px -= 5;
                    if (px <= 40) {
                        py += 5;
                        if (py == 120) {
                            window.clearInterval(gmInterval);
                        }
                    }
                }
                if (px >= 220) {
                    py += 5;
                }
                break;
            case 7:
                draw78(100);
                break;
            case 8:
                draw78(250);
                break;
            default:
        }
        ctx.drawImage(imgCar, px, py, 60, 40);
    }, 10);
}
//跑到第七八个奖品
function draw78(num) {
    if (px < 220 && py == 0) {
        px += 5;
    }
    else if (px >= 220 && py < 40) {
        px += 5;
        py += 5;
    }
    else if (px > 220 && py >= 40 && py < 80) {
        px -= 5;
        py += 5;
    }
    else if (px > 40 && px <= 220 && py == 80) {
        px -= 5;
    }
    else if (px <= 40 && py < 120) {
        px -= 5;
        py += 5;
    }
    else if (px < 40 && py >= 120) {
        px += 5;
        py += 5;
    }
    else if (px < num && py == 160) {
        px += 5;
    }
    else {
        window.clearInterval(gmInterval);
    }
}
//初始化汽车游戏
function drawInit() {
    clearInterval(gmInterval);
    px = 0;
    py = 0;
    ctx.clearRect(0, 0, 320, 210);
    ctx.beginPath();
    ctx.drawImage(imgGame, 0, 0, 320, 210);
    ctx.drawImage(imgCar, 0, 0, 60, 40);
}

//开始游戏
function start() {
    $.get('/MoreMember/IsLogin?rand=' + new Date().getMilliseconds(), function (response) {
        if (response == 'ok') {
            startGame();
        } else {
            showLogin();
        }
    });
}
//开始请求后台游戏
function startGame() {
    $('#btnStart').removeAttr('onclick', 'start()');
    var source = $('#source').val();
    $.post('/MoreMember/StartActivity', { source: source }, function (reponse) {
        console.log(reponse);
        if (reponse.Code == -1) {
            showAgain()
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
    showLayer('#divJoin', '300px', '142px');
}
//蓝豆提示框
function showBeans() {
    showLayer('#divBeans', '300px', '140px');
}
//中奖信息框
function showGoods() {
    showLayer('#divGoods', '300px', '180px');
}
//填写信息框
function showInfos() {
    hideLayer();
    $('#hLevel').html('恭喜您，抽中' + prizeArray[prizeLevel - 1] + '奖品!');
    showLayer('#divInfos', '300px', '140px');
}
//中奖完成框
function showIknow() {
    hideLayer();
    showLayer('#divIknow', '300px', '142px');
}
function showAgain() {
    //hideLayer();
    showLayer('#divAgain', '300px', '142px');
}
//弹出层
function showLayer(obj, width, height) {
    pageLayer = layer.open({
        type: 1,
        shade: [0.7, '#000'], //遮罩层
        closeBtn: [0, false], //去掉默认关闭按钮
        area: [width, height],//宽高
        offset: ['20%', ($(window).width() - 300) / 2 + 'px'],
        title: false,
        border: [0],
       conten:$(obj)
    });
}
//隐藏弹出层
function hideLayer() {
    layer.close(pageLayer);
    drawInit();;
}

$(function () {
    imgGame = document.getElementById('imgGame');
    imgCar = document.getElementById('imgCar');

    canvas = document.getElementById('gameCanvas');
    ctx = canvas.getContext('2d');
    drawInit();

    //ctx.drawImage(imgCar, 90, 0, 60, 40);
    //ctx.drawImage(imgCar, 260, 40, 60, 40);
    //ctx.drawImage(imgCar, 145, 80, 60, 40);
    //ctx.drawImage(imgCar, 0, 120, 60, 40);
    //ctx.drawImage(imgCar, 100, 160, 60, 40);
    //ctx.drawImage(imgCar, 250, 160, 60, 40);

    $.get('/MoreMember/IsLogin?rand=' + new Date().getMilliseconds(), function (response) {
        if (response == 'ok') {
            setTimeout(function () {
                start();
            }, 1000);
        } else {
            var returnUrl = "/MoreMemberMobile/Game";
            location.href = "/WapQuestionnaire/WapLogin?returnUrl=" + encodeURIComponent(returnUrl);
        }
    });
})




//根据省份获取城市
var FindCityByProvince = function (province) {
    $.ajax({
        url: "/User/FindCitysByProvince",
        type: "get",
        data: { provinceCode: $(province).val() },
        success: function (result) {
            $("#City").html(result);
        }, error: function () {
            popWindownBlue( "城市获取失败");
            return false;
        }

    });
};

//根据城市获取区域
var FindAreasByCity = function (city) {
    $.ajax({
        url: "/User/FindAreasByCity",
        type: "get",
        data: { cityCode: $(city).val() },
        success: function (result) {
            $("#Area").html(result);
        }, error: function () {
            popWindownBlue( "地区获取失败");
            return false;
        }

    });
};