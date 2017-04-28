var ExChangeLottery = {};
ExChangeLottery.GetOrderChangeLottery = function (params) {
    var lottery = null;
    $.ajax({
        type: "POST",
        url: "/OrderChange/GetOrderChangeLottery",
        data: params,
        dataType: "json",
        async:false,
        success: function (data) {
            lottery = data;
        }
    });
    return lottery;
};
ExChangeLottery.PopWindownBlue = function (msg, callBack) {
    layer.open({
        type: 1,
        skin: 'bluePopup', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: false, //开启遮罩关闭
        content: msg,
        end: function () {
            if (callBack) {
                callBack();
            }
        },
        btn: ['确定']
    });
};

$(function () {
    var timeOut = function () {  //超时函数        $("#lotteryBtn").rotate({
            angle: 0,            duration: 10000,            animateTo: 2160, //这里是设置请求超时后返回的角度，所以应该还是回到最原始的位置，2160是因为我要让它转6圈，就是360*6得来的            callback: function () {
                alert('网络超时')
            }
        });
    };
    var rotateFunc = function (awards, angle, text) {  //awards:奖项，angle:奖项对应的角度        $('#lotteryBtn').stopRotate();        $("#lotteryBtn").rotate({
            angle: 0,            duration: 5000,            animateTo: angle + 1440, //angle是图片上各奖项对应的角度，1440是我要让指针旋转4圈。所以最后的结束的角度就是这样子^^            callback: function () {
                ExChangeLottery.PopWindownBlue(text);
            }
        });
    };    $("#lotteryBtn").rotate({
        bind:
          {
              click: function () {
                  var params = {
                      mobile: "13439970624",
                      activityId: 1,
                      lotteryType: 0,
                      lotterySource:1
                  };
                  var lottery = ExChangeLottery.GetOrderChangeLottery(params);
                  if (lottery==null) {
                      ExChangeLottery.PopWindownBlue("奖品不存在，请设置奖品！");
                      return;
                  }
                  if (lottery.code!="101") {
                      ExChangeLottery.PopWindownBlue(lottery.msg);
                      return;
                  }

                  var lotteryposition = eval("(" + lottery.data.LotteryPosition + ")");
                  var pcposition = lotteryposition["pc"];                  var positionrange = pcposition[Math.floor(Math.random() * pcposition.length)];                  var angle = Math.floor(Math.random() * (positionrange["position"][1] - positionrange["position"][0]) + positionrange["position"][0]);                  rotateFunc(1, angle, lottery.data.LotteryName);
              }
          }
    });

});