addReady(function () {
    fontSize();
});

//Ready加载函数
function addReady(fn) {
    if (document.addEventListener) {
        document.addEventListener('DOMContentLoaded', fn, false);
    } else {
        document.attachEvent('onreadystatechange', function () {
            if (document.readyState == 'complete') {
                fn();
            }
        });
    }
}

function addEvent(obj, sEv, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(sEv, fn, false);
    } else {
        obj.attachEvent('on' + sEv, fn);
    }
}

function fontSize() {
    document.documentElement.style.fontSize = 100 * (document.documentElement.clientWidth / 1200) + 'px';
}

window.onload = window.onresize = window.onscroll = function () {
    fontSize();
};


/*滚动号码*/
function moveUp(boxId, p1Id, p2Id) {

    var oBox = getid(boxId), oP1 = getid(p1Id), oP2 = getid(p2Id), timer = null;
    oP2.innerHTML = oP1.innerHTML;
    timer = setInterval(move, 50);

    oBox.onmouseover = function () {
        clearInterval(timer);
    };
    oBox.onmouseout = function () {
        timer = setInterval(move, 50);
    };

    function getid(id) {
        return document.getElementById(id);
    }

    function move() {
        if (oP2.offsetTop <= oBox.scrollTop) {
            oBox.scrollTop -= oP1.offsetHeight;
        } else {
            oBox.scrollTop++;
        }
    }
}

//通过className获取元素
function getClass(oParent, sClass) {
    if (oParent.getElementsByClassName) {
        return oParent.getElementsByClassName(sClass);
    }
    else {
        // 获取所有子级
        var aTmp = oParent.getElementsByTagName('*');
        var aRes = [];

        for (var i = 0; i < aTmp.length; i++) {
            var arr = aTmp[i].className.split(' ');

            for (var j = 0; j < arr.length; j++) {
                if (arr[j] == sClass) //
                {
                    aRes.push(aTmp[i]);
                }
            }
        }

        return aRes;
    }
}



function navTab() {
    var oBody = document.getElementsByTagName('body')[0];
    var aBox = getClass(oBody, 'boxName');
    //var aBtn = getClass(oBody,'nav')[0].getElementsByTagName('a');
    var oBox = document.getElementById('nav');
    var aBtn = oBox.getElementsByTagName('a');
    var tabBox = getClass(oBody, 'swiBox')[0];

    for (var i = 0; i < aBox.length; i++) {

        (function (index) {
            aBtn[i].onclick = function () {
                for (var i = 0; i < aBox.length; i++) {
                    aBox[i].style.display = 'none';
                    aBtn[i].className = 'opt' + (i + 1);
                    tabBox.style.display = 'none';
                }
                $(".testdrive-container,.recommend-container,.tabPos").hide();
                this.className = 'opt' + (index + 1) + 'on';
                aBox[index].style.display = 'block';
            };
        })(i);
        //tabBox.style.display='block';
    }
}

//轮播图
function tab_box(id) {
    var oBox = document.getElementById(id);
    var oPrev = document.getElementById('prev');
    var oNext = document.getElementById('next');
    var oUl = oBox.getElementsByTagName('ul')[0];
    var aLi = oUl.children;
    var oOl = oBox.getElementsByTagName('ol')[0];
    var aBtn = oOl.children;
    var timer = null;
    var aImg = oUl.getElementsByTagName('img');
    var iNow = 0;
	var oBody=document.getElementsByTagName('body')[0];
	var oLayer=getClass(oBody,'layerD')[0];

	if(window.navigator.userAgent.indexOf('MSIE 6.0')!=-1 || window.navigator.userAgent.indexOf('MSIE 8.0')!=-1 || window.navigator.userAgent.indexOf('MSIE 7.0')!=-1){
		var oLiWidth= 615;
		var oLiHeight=380;
		
	}else{
		var oLiWidth= oLayer.offsetWidth;
		var oLiHeight=oLayer.offsetHeight;
	}


	//var oLiWidth= oLayer.offsetWidth;
	//var oLiHeight=oLayer.offsetHeight;


    //var oLiWidth = parseInt(getStyle(aLi[0], 'width'));
    //var oLiHeight = parseInt(getStyle(aLi[0], 'height'));

    //复制一份内容
    oUl.innerHTML += oUl.innerHTML;
    var aLi = oUl.children;
    oUl.style.width = aLi.length * oLiWidth + 'px';
    //oUl.style.height = oLiHeight+'px';

    var W = aLi.length * oLiWidth;
    var left = 0;

    function aLiW() {
        for (var i = 0; i < aLi.length; i++) {
            aLi[i].style.width = oLiWidth + 'px';
            //aLi[i].style.height = oLiHeight+'px';
        }
        oUl.style.width = oLiWidth * aLi.length + 'px';
        W = oLiWidth * aLi.length;
    }
    aLiW();


    //选项卡
    for (var i = 0; i < aBtn.length; i++) {
        (function (index) {
            addEvent(aBtn[i], 'click', function () {

                for (var i = 0; i < aSpan.length; i++) {//所有弹窗隐藏
                    aPop[i].style.display = 'none';
                }
                iNow = Math.floor(iNow / aBtn.length) * aBtn.length + index;
                tab();
            })
        })(i);
    }

    var aPop = oBox.getElementsByTagName('b');
    var aSpan = oBox.getElementsByTagName('span');
    var aEm = oBox.getElementsByTagName('em');

    for (var i = 0; i < aEm.length; i++) {
        (function (index) {

            addEvent(aEm[i], 'click', function () {

                for (var i = 0; i < aSpan.length; i++) {
                    aPop[i].style.display = 'none';
                }
                aPop[index].style.display = 'block';
            });
        })(i);
    }


    function tab() {
        for (var i = 0; i < aBtn.length; i++) {
            aBtn[i].className = '';
        }
        if (iNow < 0) {
            aBtn[(iNow % aBtn.length + aBtn.length) % aBtn.length].className = 'active';
        } else {
            aBtn[iNow % aBtn.length].className = 'active';
        }

        move(oUl,-iNow*oLiWidth);
    }
    if (document.addEventListener) {
        window.addEventListener('resize', aLiW, false);
        window.addEventListener('resize', tab, false);
    } else {
        window.attachEvent('onresize', aLiW);
        window.attachEvent('onresize', tab);
    }
    //next
    oNext.onclick = function () {
        iNow++;
        if (iNow == aBtn.length) iNow = aBtn.length - 1;
        tab();
        for (var i = 0; i < aPop.length; i++) {
            aPop[i].style.display = 'none';
        }
    };
    oPrev.onclick = function () {
        iNow--;
        if (iNow < 0) iNow = 0;
        tab();
        for (var i = 0; i < aPop.length; i++) {
            aPop[i].style.display = 'none';
        }
    };

    function next() {
        iNow++;
        if (iNow == aBtn.length) iNow = aBtn.length - 1;
        tab(iNow);

    }

    function move(obj, iTarget) {

        var count = Math.round(600 / 30);
        var start = left;
        var dis = iTarget - start;
        var n = 0;
        clearInterval(obj.timer);

        obj.timer = setInterval(function () {
            n++;

            var a = 1 - n / count;
            left = start + dis * (1 - Math.pow(a, 3));

            if (left < 0) {
                obj.style.left = left % W + 'px';
            } else {
                obj.style.left = (left % W - W) % W + 'px';
            }

            if (n == count) {
                clearInterval(obj.timer);
            }
        }, 30);
    }
}

/*---------------------------160408新增-----------------------------------------------*/
//按钮闪动
function bling() {
    var oBody = document.getElementsByTagName('body')[0];
    var aBtncy = getClass(oBody, 'btnImg');

    var nowCy = 0
    var oPa = [
		{ opacity: 1 },
		{ opacity: .4 }
    ];

    next();

    function next() {
        for (var i = 0; i < aBtncy.length; i++) {
            move(aBtncy[i], oPa[nowCy % oPa.length], {
                complete: function () {
                    nowCy++;
                    next();

                }
            }, 500);
        }
    }


}

/**
*	Set Order By strive.
*	copyright by other.
**/
//t  当前时间
//b  初始值
//c  总距离
//d  总时间
//var cur=fx(t,b,c,d)
var Tween = { Linear: function (t, b, c, d) { return c * t / d + b }, Quad: { easeIn: function (t, b, c, d) { return c * (t /= d) * t + b }, easeOut: function (t, b, c, d) { return -c * (t /= d) * (t - 2) + b }, easeInOut: function (t, b, c, d) { if ((t /= d / 2) < 1) { return c / 2 * t * t + b } return -c / 2 * ((--t) * (t - 2) - 1) + b } }, Cubic: { easeIn: function (t, b, c, d) { return c * (t /= d) * t * t + b }, easeOut: function (t, b, c, d) { return c * ((t = t / d - 1) * t * t + 1) + b }, easeInOut: function (t, b, c, d) { if ((t /= d / 2) < 1) { return c / 2 * t * t * t + b } return c / 2 * ((t -= 2) * t * t + 2) + b } }, Quart: { easeIn: function (t, b, c, d) { return c * (t /= d) * t * t * t + b }, easeOut: function (t, b, c, d) { return -c * ((t = t / d - 1) * t * t * t - 1) + b }, easeInOut: function (t, b, c, d) { if ((t /= d / 2) < 1) { return c / 2 * t * t * t * t + b } return -c / 2 * ((t -= 2) * t * t * t - 2) + b } }, Quint: { easeIn: function (t, b, c, d) { return c * (t /= d) * t * t * t * t + b }, easeOut: function (t, b, c, d) { return c * ((t = t / d - 1) * t * t * t * t + 1) + b }, easeInOut: function (t, b, c, d) { if ((t /= d / 2) < 1) { return c / 2 * t * t * t * t * t + b } return c / 2 * ((t -= 2) * t * t * t * t + 2) + b } }, Sine: { easeIn: function (t, b, c, d) { return -c * Math.cos(t / d * (Math.PI / 2)) + c + b }, easeOut: function (t, b, c, d) { return c * Math.sin(t / d * (Math.PI / 2)) + b }, easeInOut: function (t, b, c, d) { return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b } }, Expo: { easeIn: function (t, b, c, d) { return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b }, easeOut: function (t, b, c, d) { return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b }, easeInOut: function (t, b, c, d) { if (t == 0) { return b } if (t == d) { return b + c } if ((t /= d / 2) < 1) { return c / 2 * Math.pow(2, 10 * (t - 1)) + b } return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b } }, Circ: { easeIn: function (t, b, c, d) { return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b }, easeOut: function (t, b, c, d) { return c * Math.sqrt(1 - (t = t / d - 1) * t) + b }, easeInOut: function (t, b, c, d) { if ((t /= d / 2) < 1) { return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b } return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b } }, Elastic: { easeIn: function (t, b, c, d, a, p) { if (t == 0) { return b } if ((t /= d) == 1) { return b + c } if (!p) { p = d * 0.3 } if (!a || a < Math.abs(c)) { a = c; var s = p / 4 } else { var s = p / (2 * Math.PI) * Math.asin(c / a) } return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b }, easeOut: function (t, b, c, d, a, p) { if (t == 0) { return b } if ((t /= d) == 1) { return b + c } if (!p) { p = d * 0.3 } if (!a || a < Math.abs(c)) { a = c; var s = p / 4 } else { var s = p / (2 * Math.PI) * Math.asin(c / a) } return (a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b) }, easeInOut: function (t, b, c, d, a, p) { if (t == 0) { return b } if ((t /= d / 2) == 2) { return b + c } if (!p) { p = d * (0.3 * 1.5) } if (!a || a < Math.abs(c)) { a = c; var s = p / 4 } else { var s = p / (2 * Math.PI) * Math.asin(c / a) } if (t < 1) { return -0.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b } return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * 0.5 + c + b } }, Back: { easeIn: function (t, b, c, d, s) { if (s == undefined) { s = 1.70158 } return c * (t /= d) * t * ((s + 1) * t - s) + b }, easeOut: function (t, b, c, d, s) { if (s == undefined) { s = 1.70158 } return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b }, easeInOut: function (t, b, c, d, s) { if (s == undefined) { s = 1.70158 } if ((t /= d / 2) < 1) { return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b } return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b } }, Bounce: { easeIn: function (t, b, c, d) { return c - Tween.Bounce.easeOut(d - t, 0, c, d) + b }, easeOut: function (t, b, c, d) { if ((t /= d) < (1 / 2.75)) { return c * (7.5625 * t * t) + b } else { if (t < (2 / 2.75)) { return c * (7.5625 * (t -= (1.5 / 2.75)) * t + 0.75) + b } else { if (t < (2.5 / 2.75)) { return c * (7.5625 * (t -= (2.25 / 2.75)) * t + 0.9375) + b } else { return c * (7.5625 * (t -= (2.625 / 2.75)) * t + 0.984375) + b } } } }, easeInOut: function (t, b, c, d) { if (t < d / 2) { return Tween.Bounce.easeIn(t * 2, 0, c, d) * 0.5 + b } else { return Tween.Bounce.easeOut(t * 2 - d, 0, c, d) * 0.5 + c * 0.5 + b } } } };

function move(obj, json, options) {
    options = options || {};
    options.duration = options.duration || 500;
    options.easing = options.easing || Tween.Linear;
    var start = {};
    var dis = {};
    var count = Math.floor(options.duration / 30);
    var n = 0;

    for (var name in json) {
        start[name] = parseFloat(getStyle(obj, name));
        dis[name] = json[name] - start[name];
    }

    clearInterval(obj.timer);
    obj.timer = setInterval(function () {
        n++;
        for (var name in json) {
            //var cur=fx(t,b,c,d)
            var cur = options.easing(n / count * options.duration, start[name], dis[name], options.duration);
            //t  当前时间			  b  初始值 	  c  总距离  d  总时间

            if (name == 'opacity') {
                obj.style[name] = cur;
                obj.style.filter = 'alpha(opacity:' + cur * 100 + ')';
            } else {
                obj.style[name] = cur + 'px';
            }
        }

        if (n == count) {
            clearInterval(obj.timer);
            options.complete && options.complete();
        }

    }, 30);


}
function getStyle(obj, sName) {
    return (obj.currentStyle || getComputedStyle(obj, false))[sName];
}


//自定义滚轮
function scroJs() {
    var oSpan = document.getElementById('span');
    var oCont = document.getElementById('context');
    var oUp = document.getElementById('up');
    var oDown = document.getElementById('down');
    var oSrc = document.getElementById('scr');
    var oSrc_r = document.getElementById('r_scr');
    var oTxt = document.getElementById('txt');
    var timer = null;
    var oldTop = oSpan.offsetTop;
    var oScale = oTxt.offsetHeight / oCont.offsetHeight;
    var h = oSrc.offsetHeight / oScale;
    if (h < 30) {
        h = 30;
    } else if (h > oSrc.offsetHeight) {
        h = oSrc.offsetHeight;
    } else {
        oSpan.style.height = h + 'px';
    }

    //IE8上计算出来的srcMaxH为0,导致无法滚动。
    var srcMaxH = oSrc.offsetHeight - oSpan.offsetHeight == 0 ?
        135 : oSrc.offsetHeight - oSpan.offsetHeight;

    oSrc_r.onmousedown = function () {
        return false;
    };
    oUp.onmousedown = function () {
        var top = oSpan.offsetTop;
        clearInterval(timer);
        timer = setInterval(function () {
            top -= 10;
            setTop(top)
        }, 30);
        return false;
    };

    oDown.onmousedown = function () {
        var top = oSpan.offsetTop;
        clearInterval(timer);
        timer = setInterval(function () {
            top += 10;
            setTop(top);
        }, 30)
        return false;
    };
    oDown.onmouseup = oUp.onmouseup = function () {
        clearInterval(timer);
    };

    oSpan.onmousedown = function (ev) {
        var oEvent = ev || event;
        var disY = oEvent.clientY - oSpan.offsetTop;


        document.onmousemove = function (ev) {
            var oEvent = ev || event;
            var top = oEvent.clientY - disY;

            setTop(top);
        };

        document.onmouseup = function () {
            document.onmousemove = null;
            document.onmouseup = null;
            oSpan.releaseCapture && oSpan.releaseCapture();
        };

        oSpan.setCapture && oSpan.setCapture();
        return false;

    };

    function setTop(top) {
        if (top < 0) {
            top = 0;
        } else if (top > srcMaxH) {
            top = srcMaxH;
        }

        var scale;
        if (srcMaxH != 0) {
            scale = top / srcMaxH;
        } else { scale = 0;}
        oSpan.style.top = top + 'px';

        oTxt.style.top = -oTxt.offsetHeight * scale + 'px';


    }

    addSrc(oCont, function (down) {
        var top = oSpan.offsetTop;
        if (down) {
            top += 10;
            setTop(top);
        } else {
            top -= 10;
            setTop(top);
        }
    })
}

function addSrc(obj, fn) {
    if (window.navigator.userAgent.toLowerCase().indexOf('firefox') != -1) {
        obj.addEventListener('DOMMouseScroll', _wheel, false);
    } else {
        obj.onmousewheel = _wheel
    }
    function _wheel(ev) {
        var oEvent = ev || event;
        var down = false;
        if (oEvent.wheelDelta) {
            down = oEvent.wheelDelta > 0 ? false : true;
        } else {
            down = oEvent.detail > 0 ? true : false;
        }
        fn && fn(down);
        oEvent.preventDefault && oEvent.preventDefault();
        return false;
    }
}

//获取非行间样式
function getStyle(obj, sName) {
    return (obj.currentStyle || getComputedStyle(obj, false))[sName];
}

//"立即参与"按钮调出轮播图
function pushBtn() {
    var oBody = document.getElementsByTagName('body')[0];
    var oBtn = getClass(oBody, 'btnCyHomg')[0];
    var oSiwBox = getClass(oBody, 'swiBox')[0];
    var aConBox = getClass(oBody, 'boxName');
    var oBtnJs = getClass(oBody, 'btn_cy')[0];

    oBtnJs.onclick = oBtn.onclick = function () {
        for (var i = 0; i < aConBox.length; i++) {
            aConBox[i].style.display = 'none';
        }
        oSiwBox.style.display = 'block';
    };

}

function submit_testDrive() {
    var model = function () { };
    model.CarSeries = $("#CarSeries").val();
    if (isnull(model.CarSeries)) {
        popWindownBlue( "请选择车型");
        return;
    }
    model.DealerId = $("#DealerId").val();
    if (isnull(model.DealerId)) {
        popWindownBlue( "请选择经销商");
        return;
    }
    model.ScheduleDate = $("#ScheduleDate").val();
    if (isnull(model.ScheduleDate)) {
        popWindownBlue( "请输入试驾时间");
        return;
    }

    var ds = (new Date() - new Date(model.ScheduleDate)) / 1000 / 60 / 60 / 24;
    if (ds > 1) {
        popWindownBlue( "请输入正确的到店时间");
        return;
    }

    model.UserName = $("#UserName").val();
    if (isnull(model.UserName)) {
        popWindownBlue( "请输入姓名");
        return;
    }
    model.Phone = $("#Phone1").val();
    if (isnull(model.Phone)) {
        popWindownBlue( "请输入手机号");
        return;
    }
    if (!checktel(model.Phone)) {
        popWindownBlue( "请输入正确的手机号");
        return;
    }
    model.ValiateCode = $("#validateCode1").val();
    if (isnull(model.ValiateCode)) {
        popWindownBlue( "请输入短信验证码");
        return;
    }

    model.DealerCity = $("#DealerCity").val();
    model.DealerProvince = $("#DealerProvince").val();
    model.DealerName = $("#DealerName").val();
    model.UserSex = $("#sex1").val() == "1" ? 1 : 0;

    $.post("/lingdong/TestDrive4PC",
        {
            carSeries: model.CarSeries,
            openId: model.OpenId,
            dealerId: model.DealerId,
            scheduleDate: model.ScheduleDate,
            userName: model.UserName,
            phone: model.Phone,
            dealerCity: model.DealerCity,
            dealerProvince: model.DealerProvince,
            dealerName: model.DealerName,
            userSex: model.UserSex,
            valiateCode: model.ValiateCode
        },
        function (data) {
            if (data.ret == 0 || data.ret == 1) { popWindownBlue( data.msg); }
            if (data.retcode == 1 && data.AwardObj != null && data.AwardObj != undefined) {
                $("#form-testdrive").hide();
                $("#test_award").show();
                $("#test_award h4").text("恭喜您中奖了");
                $("#test_award h5").text("获得" + data.AwardObj.Name + "礼品");
            } else if (data.retcode == 1 && data.AwardObj == null) {
                $("#form-testdrive").hide();
                $("#test_award").show();
                $("#test_award h4").text("很遗憾，您没有中奖");
                $("#test-award-back").show();
                $("#test_award #test_award_address").hide();
            }
        });
}

function postAddress() {
    var model = function () { };
    model.openId = $("#Phone1").val();
    model.userName = $("#UserName").val();
    model.phoneNumber = $("#Phone1").val();
    model.address = $("#Province").val()
        + $("#City").val()
        + $("#Area").val()
        + $("#addPart4").val()
        + $("#addPart5").val();

    if ($("#Province").val() === "") { popWindownBlue( "请选择省份"); }
    if ($("#City").val() === "") { popWindownBlue( "请选择城市"); }
    if ($("#Area").val() === "") { popWindownBlue( "请选择地区"); }

    if (isnull(model.address)) {
        popWindownBlue( "请填写邮寄地址");
        return;
    }
    var url = "/lingdong/PostAddress";
    $.post(url
        , {
            name: model.userName,
            openId: model.openId,
            phoneNumber: model.phoneNumber,
            address: model.address
        }
        , function (data) {
            popWindownBlue( "提交成功！");
            $("#test_award").hide();
            $("#address-complete").show();
        });
}

function gotoRecommend(ele) {
    hideAll();
    $(".recommend-container").show();
}

function carOwnerCheck() {
    var name = $("#carowner-check-name").val();
    var phone = $("#carowner-check-phone").val();
    var validateCode = $("#carowner-check-valide").val();
    var idcard = $("#carowner-check-card").val();
    var vin = $("#carowner-check-vin").val();
    if (isnull(name)) {
        popWindownBlue( "姓名不能为空");
        return;
    }
    if (isnull(phone)) {
        popWindownBlue( "手机号不能为空");
        return;
    }
    if (isnull(validateCode)) {
        popWindownBlue( "验证码不能为空");
        return;
    }
    if (isnull(idcard)) {
        popWindownBlue( "身份证不能为空");
        return;
    }
    $.post("/lingdong/Bind", { openId: phone, phone: phone, validateCode: validateCode, idcard: idcard }, function (data) {
        if (data.ret == -1) {
            popWindownBlue( "请输入正确的验证码");
            return;
        } else if (data.ret == 0) {
            popWindownBlue( "验证车主失败，请检查身份证是否正确");
            return;
        } else if (data.ret == 1) {
            $("#recommend-carowner-check").hide();
            $("#form-recommend").show();
        }
    })
}

function submitRecommend() {
    var myphone = $("#carowner-check-name").val();

    var name1 = $("#rec-name-0").val();
    var phone1 = $("#rec-phone-0").val();

    var name2 = $("#rec-name-1").val();
    var phone2 = $("#rec-phone-1").val();

    var name3 = $("#rec-name-2").val();
    var phone3 = $("#rec-phone-2").val();

    if (isnull(name1)) {
        popWindownBlue( "姓名不能为空");
        return;
    }
    if (isnull(phone1)) {
        popWindownBlue( "手机不能为空");
        return;
    }
    if (!checktel(phone1)) {
        popWindownBlue( "请输入正确的手机号");
        return;
    }

    $.post("/lingdong/Recommend", { "OpenId": myphone, Name1: name1, PhoneNumber1: phone1, Name2: name2, PhoneNumber2: phone2, Name3: name3, PhoneNumber3: phone3 }, function (data) {

        if (data == null || data == "" || isnull(data.Name)) {
            //没有中奖
            $("#rec-complete h4").text("很遗憾，您没有中奖");
            $("#form-recommend").hide();
            $("#rec-complete").show();
            $("#rec-complete ul").show();
            return;
        } else {
            //中奖
            $("#rec-complete h4").text("恭喜您中奖了");
            $("#test_award h5").text("获得" + data.AwardObj.Name + "礼品");
            $("#form-recommend").hide();
            $("#rec-complete").show();
            $("#rec-complete ul").show();
            return;
        }
    });

}

//$("#DealerId").change(function () {
//    $("#DealerName").val($(this).find("option:selected").text());
//});

function provinceChange(province) {
    $("#DealerCity option[value!='']").remove();
    $("#DealerId option[value!='']").remove();
    $.get("/lingdong/GetCityListByProvince", { province: province.value }, function (result) {
        $("#DealerCity").append("<option value='0'>请选择</option>");
        $.each(result, function (index, item) {
            $("#DealerCity").append("<option value=" + item + ">" + item + "</option>");
        });
    });
}

function cityChange(city) {
    $("#DealerId option[value!='']").remove();
    var province = $('#DealerProvince').val();
    $.get("/lingdong/GetDealerList", { province: province, city: city.value }, function (result) {
        $("#DealerId").append("<option value='0'>请选择</option>");
        if (result) {
            $.each(result, function (index, item) {
                $("#DealerId").append("<option value=" + item.DealerId + ">" + item.Name + "</option>");
            });
        }
    });
}

function gotoTestDrive() {
    hideAll();
    $(".testdrive-container").show();
}

function getAward() {
    $.get("/lingdong/GetAward", { phone: $("#award-phone").val() }, function (data) {
        $("#ul1 p#p1 a").remove();
        $("#ul1 p#p1 br").remove();
        $("#ul2 p#p3 a").remove();
        $("#ul2 p#p3 br").remove();
        $.each(data.drvAwardList, function (index, item) {
            $("#ul1 p").append("<a href='javascript:;'>" + item.AwardName + "</a><br>");
        });
        $.each(data.rmdAwardList, function (index, item) {
            $("#ul1 p").append("<a href='javascript:;'>" + item.AwardName + "</a><br>");
        });
    });
}

function testDrive() {
    hideAll();
    $(".testdrive-container").show();
}
function recommend() {
    hideAll();
    $(".recommend-container").show();
}

function hideAll() {
    $("#readme-container").hide();
    $(".jpsz").hide();
    $(".zjmd").hide();
    $(".recommend-container").hide();
    $(".testdrive-container").hide();
    $(".tabPos").hide();
    $(".tabPos").css("z-index", "-1");
}

recommendCounter = 1;
function addRecItem() {
    var nameTemp = '<li><label>姓名</label><input type="text" id="rec-name-' + recommendCounter + '" /></li>';
    var phoneTemp = '<li><label>手机号码</label><input type="text" id="rec-phone-' + recommendCounter + '" /></li>';

    $("#form-recommend ul#form-recommend-item").append(nameTemp).append(phoneTemp);
    recommendCounter++;
}

function sendValidate(phone, ele) {
    //$(errorPanel).display = "none";
    if (!checktel(phone)) {
        //$(errorPanel).display = "";
        popWindownBlue( "请输入正确手机号");
        return;
    }
    var url = "/lingdong/SendValidateCode";
    $.post(url, { tel: phone }, function () { });
    time(ele);
    return;
}

function goto() {
    hideAll();
    $(".tabPos").css("z-index", "1001").show();
}

function gotoReadme() {
    $(".nav a.opt2").click();
    $(".tabPos").css("z-index", "1001").show();
}

function isnull(txt) {
    if (typeof (txt) == 'undefined' || txt == undefined || txt == '') {
        return true;
    }
};

//手机号校验
function checktel(tel) {
    //var reg = /^1[3|4|5|8][0-9]\d{4,8}$/;
    var reg = /^1\d{10}$/;
    if (!reg.test(tel)) {
        return false;
    }
    return true;
}

//时间
var wait = 60;
function time(o) {
    if (wait == 0) {
        o.removeAttribute("disabled");
        o.textContent = "发送验证码";
        wait = 60;
    } else { // 
        o.setAttribute("disabled", true);
        o.textContent = "重新发送(" + wait + ")";
        wait--;
        setTimeout(function () {
            time(o);
        },
            1000);
    }
}
