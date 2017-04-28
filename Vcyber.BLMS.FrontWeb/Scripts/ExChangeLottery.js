﻿var ExChangeLottery = {};
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
    var timeOut = function () {  //超时函数
            angle: 0,
                alert('网络超时')
            }
        });
    };
    var rotateFunc = function (awards, angle, text) {  //awards:奖项，angle:奖项对应的角度
            angle: 0,
                ExChangeLottery.PopWindownBlue(text);
            }
        });
    };
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
                  var pcposition = lotteryposition["pc"];
              }
          }
    });

});