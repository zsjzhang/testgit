var praise = function (typeValue, idValue) {
    this._type = typeValue;
    this._id = idValue;
    //是否登录可以点赞
    this._isTrue = false;
    //点赞数量
    this._praiseCount = 0;
    //判断是否重复点击
    this._isDobble = true;
    //当前用户状态
    this._status = -1;
    //当前用户本月点击次数
    this._count = -1;
    //当前用户点赞获得的蓝豆奖励
    this._praiseBlueBean = 15;
    //当前用户的级别
    this._mlevel = 3;

    this.isPraise = function () {
        var that = this;
        $.ajax({
            url: "/Account/IsParaise",
            type: "get",
            data: { type: that._type, id: that._id, t: (new Date()).getTime() },
            dataType: "json",
            success: function (resultData) {
                if (!resultData || !resultData.code || resultData.curCount < -1) {
                    that._status = "400";
                    return false;
                }
                else if (resultData.code == "200") {
                    that._status = resultData.code;
                    that._count = resultData.curCount;

                    that._mlevel = Number(resultData.level);
                    if (that._mlevel <= 1) {
                        that._praiseBlueBean = 0;
                    } else if (that._mlevel == 2) {
                        that._praiseBlueBean = 10;
                    }
                    return false;
                } else {
                    that._status = resultData.code;
                    that._count = resultData.curCount;
                    return false;
                }
            },
            complete: function (xhr) {
                that._isTrue = true;
                xhr = null;
            }
        });
    };

    this.praiseCount = function () {
        var that = this;
        $.ajax({
            url: "/Account/PraiseCount",
            type: "get",
            data: { type: that._type, id: that._id, t: (new Date()).getTime() },
            dataType: "json",
            success: function (resultCount) {
                that._praiseCount = resultCount.content;
            },
            error: function (xhrcount) {
                xhrcount = null;
                return false;
            }
        });
    };

    this.animatePraise = function () {
        var that = this;
        if (!that._isDobble) {
            return false;
        }
        if (window.addEventListener) {
            //第一：获得点赞数直接赋值给将要拉伸的div
            var _donghua = $("#donghua-dianzan-position-auto");

            var _content = parseInt(that._praiseCount) + 1;
            //将相加的结果重新赋值给拉伸的div
            $(_donghua).animate({ width: '75px' }, 500, function () {
                $(_donghua).html(_content);
            });

            //如果当前用户级别低于1星则不执行弹出蓝豆奖励动画            
            if (that._mlevel > 1) {
                //$(_donghua).animate({ width: '75px' }, 500);
                //$(_donghua).animate({ width: '31px' }, 100, function () {
                //    $(_donghua).css({ "background-image": "" });
                //    $(_donghua).html("<div id='donghua-content' style='text-align: center; margin:0; font-size:14px; color:#fff;margin-top:10px;  line-height: 18px; '>已获得" + that._praiseBlueBean + "蓝豆</div>");
                //});
                //将恢复原始大小的div放大，弹出蓝豆奖励动画
                //$(_donghua).animate({ width: '60px', height: '60px', "border-radius": "30px", top: "-80px", left: "-10px", "line-height": "60px" }, 200);
                //$(_donghua).animate({ width: '60px', height: '60px', "border-radius": "30px", top: "-80px", left: "-10px", "line-height": "60px" }, 200, function () {
                //    var _count = 0;
                //    var s = setInterval(function () {
                //        $("#donghua-content").animate({ "font-size": "12px", "color": "#f00" }, 50);
                //        $("#donghua-content").animate({ "font-size": "14px", "color": "#fff" }, 20);
                //        _count++;
                //        if (_count == 2) {
                //            clearInterval(s);
                //            $(_donghua).animate({ "top": "0", "left": "0", "width": "75px", "height": "36px", "line-height": "36px", "color": "#fff", "border-radius": "18px", "float": "left", "position": "relative", "padding-right": "5px" }, 500, function () {
                //                $(_donghua).css({ background: "#f46428 url(/img/praise.png) no-repeat 8px 8px" });
                //                $(_donghua).html(_content);
                //            });
                //        }
                //    }, 70);
                //});
                return false;
            }
        }
        else if (navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE8.0") {
            console.info("ie8");
        }
    };

    //鼠标放在上面
    this.mouseoverAnimate = function () {
        var that = this;
        var _donghua = $("#donghua-dianzan-position-auto");
        $(_donghua).html(that._praiseCount);
        //第二：div拉伸
        $(_donghua).animate({ width: '75px' }, 200);
    };

    //鼠标移走动画
    this.mouseoutAnimate = function () {
        var _donghua = $("#donghua-dianzan-position-auto");
        $(_donghua).animate({ width: '31px' }, 200, function () {
            $(_donghua).html("");
        });
    };

    //点赞请求后台
    this.doPraise = function () {
        var that = this;
        if (!that._isDobble) {
            return false;
        }
        that._isDobble = false;
        $.ajax({
            url: "/Account/DoPraise",
            type: "get",
            data: { type: that._type, id: that._id, t: (new Date()).getTime() },
            dataType: "json",
            success: function (resultDo) {
                that._praiseCount++;
                that._count++;
                that._isDobble = true;
            },
            complete: function (xhr) {
                xhr = null;
                that._isDobble = true;
            }
        });
    };

    //点赞
    this.do = function () {
        if (this._isTrue) {
            if (!this._isDobble) {
                return false;
            }
            if (this._status == "400") {
                var _index = layer.tips("参数错误", '#donghua-dianzan-position-auto', {
                    tips: [1, '#3595CC'],
                    time: 4000
                });
                setTimeout(function () { layer.close(_index); }, 3000);
                return false;
            } else if (this._status == "401") {
                var _index = layer.tips("登录后才能点赞哦", '#donghua-dianzan-position-auto', {
                    tips: [1, '#3595CC'],
                    time: 4000
                });
                setTimeout(function () { layer.close(_index); }, 3000);
                return false;
            }
            else if (this._status == "201" || this._count >= 3) {//
                var _index = layer.tips('您点赞的次数已经超过规定的3次', '#donghua-dianzan-position-auto', {
                    tips: [1, '#3595CC'],
                    time: 4000
                });
                setTimeout(function () { layer.close(_index); }, 3000);
                return false;
            }
            this.animatePraise();
            this.doPraise();
        }
    };

    //初始化点赞数量以及是否可以点赞
    this.isPraise();
    this.praiseCount();
};