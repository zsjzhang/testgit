function WxShare () {};


(function (obj) {
    var json1 = {
        title: "【领新而动】蓝缤多重礼遇领不停",
        desc: "北京现代领动，预约试驾，免费得运动手环，购车再送千元加油卡免费",
        link: common.resolveUrl("/Lingdong/share", true),
        imgUrl: common.api("Contents/images/lingdong/usershared.jpg")
    };

    var json2 = {
        title: "【领新而动】蓝缤多重礼遇领不停",
        desc: "我已经推荐你参与，领动新车试驾免费得运动手环活动，快来领取吧！",
        link: common.resolveUrl("/Lingdong/share", true),
        imgUrl: common.api("Contents/images/lingdong/usershared.jpg")
    };
    
    $(function () {
        var currUrl = window.location.href;
        obj.page = $("#sharePageContent").val();
        common.post(common.api("/WeChat/Common/JSSDK_Signature"), { url: currUrl }, function (data) {
            wxShareInit(data);
        });
    });

    function wxShareInit(data) {
        //data.debug = true;
        data.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo'];
        wx.config(data);
        wx.ready(function () {
            if (WxShare && WxShare.readyCallback) { WxShare.readyCallback(obj, wx); }

            var _json = json1;
            if (obj.page==2) {
                _json = json2;
            }  
            
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
    }

})(new WxShare());