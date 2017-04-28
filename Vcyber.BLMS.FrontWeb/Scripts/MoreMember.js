var levelArray = ['秀才', '秀才', '秀才', '举人', '进士', '探花', '榜眼', '状元'];
var wordsArray = [
    '秀才公子好，蓝豆已入账，祝来日能金榜题名。',
    '秀才公子好，蓝豆已入账，祝来日能金榜题名。',
    '秀才公子好，蓝豆已入账，祝来日能金榜题名。',
    '恭喜举人大人！仕途由此打开，祝君好运！车载USB充电器已入库房。',
    '哇！亲中进士了呢，小蓝为您备了份贺礼（卡片式移动电源），请笑纳！',
    '听说探花都是颜值界的翘首，拜见颜王。厨具四件套到手。',
    '离状元只差1步，榜眼好像也不错。速来领取卓棒Jawbone UP24智能手环。',
    '哇哦！亲的手气好到爆啊，竟然中了状元！恭喜您获得三星Gear2Neo智能手表。',
];
var prizeArray = ['BM网站10蓝豆', 'BM网站50蓝豆', 'BM网站100蓝豆', '车载usb充电器',
    '卡片式移动电源', '厨具四件套', '卓棒Jawbone UP24智能手环', '三星Gear 2 Neo智能手表'];
var prizeLevel = 0;
//每种奖项的组合点数
var pointArray = new Array();
pointArray.push([[1, 2], [1, 3], [2, 1], [2, 3], [3, 1], [3, 2]]);
pointArray.push([[1, 2], [1, 3], [2, 1], [2, 3], [3, 1], [3, 2]]);
pointArray.push([[1, 2], [1, 3], [2, 1], [2, 3], [3, 1], [3, 2]]);
pointArray.push([[5, 5], [6, 6]]);
pointArray.push([[4, 1], [4, 2], [4, 3]]);
pointArray.push([[4, 5], [4, 6]]);
pointArray.push([[1, 1], [2, 2], [3, 3], [5, 5], [6, 6]]);
pointArray.push([[4, 4]]);
//骰子图片
var imgArray = ['/Img/MoreMember/point1.png',
    '/Img/MoreMember/point2.png',
    '/Img/MoreMember/point3.png',
    '/Img/MoreMember/point4.png',
    '/Img/MoreMember/point5.png',
    '/Img/MoreMember/point6.png'
];
//汽车位置坐标
var px = 0;
var py = 0;
//骰子切换动画


var imgGame;//奖品图片
var imgCar; //汽车图片
var canvas; //H5 Canvas
var ctx;

var gmInterval;
var bmInterval;
function startPoint() {
    bmInterval = setInterval(function () {
        var randNumber = parseInt(Math.random() * 6);
        $('#imgPointA').attr('src', imgArray[randNumber]);
        randNumber = parseInt(Math.random() * 6);
        $('#imgPointB').attr('src', imgArray[randNumber]);
    }, 50);
}

//筛子停止动画
function stopPoint() {
    clearInterval(bmInterval);
}

//游戏停止，变换到到相应的点数
function stopGame(level) {
    setTimeout(function () {
        stopPoint();
        var childArray = pointArray[level - 1];
        var levelLength = childArray.length;
        var randNumber = parseInt(Math.random() * levelLength);
        var diceArray = childArray[randNumber];
        var numA = diceArray[0];
        var numB = diceArray[1];
        $('#imgPointA').attr('src', imgArray[numA - 1]);
        $('#imgPointB').attr('src', imgArray[numB - 1]);
    }, 600);
    setTimeout(function () {
        startCar(level);
    }, 500);

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