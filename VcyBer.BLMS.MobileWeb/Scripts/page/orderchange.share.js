var WeChat = function () { };
var _json = {
    title: "“惠”聚蓝缤，畅享2万元置换礼包！",
    desc: "北京现代bluemembers【置换专区】惊喜来袭！",
    link: common.resolveUrl("/zhihuan?s=share"),
    imgUrl: common.resolveUrl("/images/orderchange_share.jpg")
};
(function (obj) {
    $(function () {
        var currUrl = window.location.href;
        common.post(common.api("/WeChat/Common/JSSDK_Signature"), { url: currUrl }, function (data) {
            WeChat_init(data);
        });
    });

    function WeChat_init(data) {
        //data.debug = true;
        data.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo', 'getLocation'];
        //data.debug = true;
        wx.config(data);
        wx.ready(function () {
            if (WeChat && WeChat.readyCallback) { WeChat.readyCallback(obj, wx); }
            var title = _json.title;
            var desc = _json.desc;
            var link = _json.link;
            var link2 = _json.link;
            var imgUrl = _json.imgUrl;
            //分享到朋友圈
            wx.onMenuShareTimeline({
                title: title, // 分享标题
                desc: desc, // 分享描述
                link: link, // 分享链接
                imgUrl: imgUrl, // 分享图标
                trigger: function (res) {
                    //alert('用户点击分享到朋友圈');
                },
                success: function (res) {
                   // alert('已分享');
                },
                cancel: function (res) {
                    //alert('已取消');
                },
                fail: function (res) {
                   // alert('wx.onMenuShareTimeline:fail: ' + JSON.stringify(res));
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
        wx.error(function (res) {
            //alert('wx.error: ' + JSON.stringify(res));
        });
    }
})(WeChat);

