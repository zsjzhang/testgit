var WeChat = function () { };
(function (obj) {
    $(function () {
        var currUrl = window.location.href;
        //common.post(common.api("/WeChat/Common/JSSDK_Signature"), { url: currUrl }, function (data) {
        //    WeChat_init(data);
        //});
        common.post("", { url: currUrl }, function (data) {
            WeChat_init(data);
        });
    });

    function WeChat_init(data) {
        //data.debug = true;
        data.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo', 'getLocation'];
        wx.config(data);
        wx.ready(function () {
            if (WeChat && WeChat.readyCallback) { WeChat.readyCallback(obj,wx); }
            var title = "北京现代";
            var desc = "这是描述";
            var link = common.resolveUrl("/Wechat/OAuth/Base", true);
            var link2 = common.resolveUrl("/", true);
            var imgUrl = common.resolveUrl("/images/login_logo.png", true)
            //分享到朋友圈
            wx.onMenuShareTimeline({
                title: title, // 分享标题
                link: link, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享给朋友
            wx.onMenuShareAppMessage({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: link, // 分享链接
                imgUrl: imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享到QQ
            wx.onMenuShareQQ({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: link2, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            //分享到腾讯微博
            wx.onMenuShareWeibo({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: link2, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        });
        //wx.error(function (res) {
        //    common.alertJson(res);
        //});
    }

    obj.getLocation = function (callback) {
        //alert("开始获取，请稍后。");
        wx.getLocation({
            success: function (res) {
                var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
                var speed = res.speed; // 速度，以米/每秒计
                var accuracy = res.accuracy; // 位置精度
                if (callback) { callback(res); }
                //alert(common.format("经度：{0}，纬度：{1}，速度：{2}，位置经度：{3}", latitude, longitude, speed, accuracy));
            }, fail: function () {
                alert("获取位置信息失败，请检查是否已开启GPS定位且允许微信使用定位服务。");
            }
        });
    }
})(WeChat);