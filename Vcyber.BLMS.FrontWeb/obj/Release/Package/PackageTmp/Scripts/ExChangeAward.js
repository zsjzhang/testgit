

//通过className获取元素
function getClass(oParent,sClass){
	if (oParent.getElementsByClassName){
		return oParent.getElementsByClassName(sClass);
	}else{
		// 获取所有子级
		var aTmp=oParent.getElementsByTagName('*');
		var aRes=[];
		
		for (var i=0; i<aTmp.length; i++){
			var arr=aTmp[i].className.split(' ');
			
			for (var j=0; j<arr.length; j++){
				if (arr[j] == sClass){
					aRes.push(aTmp[i]);
				}
			}
		}
		
		return aRes;
	}
}

var ExChangeLottery = {};
ExChangeLottery.GetOrderChangeLottery = function (params) {
    var lottery = null;
    $.ajax({
        type: "POST",
        url: "/OrderChange/GetRandomLottery",
        data: params,
        dataType: "json",
        async: false,
        success: function (data) {
            lottery = data;
        }
    });
    return lottery;
};
ExChangeLottery.AddXDSendInfo = function (index) {
    var SendProvince = $("#Province").val();
    var SendCity = $("#City").val();
    var SendDistrinct = $("#Area").val();
    var SendAddress = $("#SendAddress").val();
    var UserName = $("#UserName").val();
    var UserMobile = $("#UserMobile").val();
    var LotteryId = $("#hidLotteryId").val();

    if (UserName == null || UserName == "") {
        ExChangeLottery.PopWindownBlue("姓名不能为空！");
        return false;
    }
    if (UserMobile == null || UserMobile == "") {
        ExChangeLottery.PopWindownBlue("手机号不能为空！");
        return false;
    }
    if (LotteryId <= 0) {
        ExChangeLottery.PopWindownBlue("奖品不正确！");
        return false;
    }
    if (SendProvince == null || SendProvince == "") {
        ExChangeLottery.PopWindownBlue("邮寄的省不能为空！");
        return false;
    }
    if (SendCity == null || SendCity == "") {
        ExChangeLottery.PopWindownBlue("邮寄的市不能为空！");
        return false;
    }
    if (SendDistrinct == null || SendDistrinct == "") {
        ExChangeLottery.PopWindownBlue("邮寄的地区不能为空！");
        return false;
    }
    if (SendAddress == null || SendAddress == "") {
        ExChangeLottery.PopWindownBlue("邮寄的街道不能为空！");
        return false;
    }
    var params = {
        LotteryId: LotteryId,
        LotteryRecordId:$("#hidLotteryRecordId").val(),
        UserName: UserName,
        UserMobile: UserMobile,
        SendProvince: SendProvince,
        SendCity: SendCity,
        SendDistrinct: SendDistrinct,
        SendAddress: SendAddress,
        SendSource: $("#hidSource").val()
    };

    $.ajax({
        type: "POST",
        url: "/OrderChange/AddXDSendInfo",
        data: params,
        dataType: "json",
        async: false,
        success: function (data) {
            layer.close(index);
            ExChangeLottery.SharePop("邮寄地址添加成功！");
        }
    });
};
ExChangeLottery.PopWindownBlue = function (msg, callBack) {
    layer.open({
        type: 1,
        title: '信息',
        skin: 'bluePopup2', //样式类名
        closeBtn: 1, //不显示关闭按钮
        shift: 2,
        shadeClose: true, //开启遮罩关闭
        content: '<h2>' + msg + '</h2>'
    });
};
ExChangeLottery.IsCanInviter = function (flag) {
    $.ajax({
        type: "POST",
        url: "/OrderChange/IsCanInviter",
        data: {},
        dataType: "json",
        success: function (data) {
            if (data.code == "001") {
                window.location.href = "/Account/LogonPage?returnUrl=/OrderChange/index?source=" + $("#hidSource").val();
            }
            else if (data.code == "002") {
                window.location.href = "/OrderChange/BangDing?source=" + $("#hidSource").val() + "&flag=" + flag;
            }
            else {
                window.location.href = "/OrderChange/TuiJian?source=" + $("#hidSource").val() + "&flag=" + flag;
            }
        }
    });
};
ExChangeLottery.SharePop = function (text) {
    if ($("#hidPageSource").val() == "change") {
        layer.open({
            type: 1,
            skin: 'bluePopup bluePopupForm2', //样式类名
            title: false,
            shift: 2,
            shadeClose: true, //开启遮罩关闭
            content: '<div class="noaward"><div class="awardb"><div class="aConBox"><p>' + (text == undefined ? '' : text) + '</p><span class="tishi">活动结束后5个工作日邮寄礼品，请耐心等候！ <br>立即参与推荐好友购车，还有好礼相送！<br></span></div></div></div>',
            btn: ['更多好礼'],
            yes: function (index, layero) {
                window.location.href = "/OrderChange/index?source=" + $("#hidSource").val() + "&type=1";
            }
        });
    }
    else {
        layer.open({
            type: 1,
            title:false,
            skin: 'bluePopup bluePopupForm2', //样式类名
            
            shift: 2,
            shadeClose: true, //开启遮罩关闭
            content: '<div class="noaward"><div class="awardb"><div class="aConBox"><p>' + (text == undefined ? '' : text) + '</p><span class="tishi">活动结束后5个工作日邮寄礼品，请耐心等候！ <br>立即参与推荐好友购车，还有好礼相送！<br></span></div></div></div>',
            btn: ['更多好礼'],
            yes: function (index, layero) {
                window.location.href = "/OrderChange/index?source=" + $("#hidSource").val() + "&type=1";
            }
        });
    }
};
$(function (){
	var rotateFunc = function (awards, angle, text, lotteryId) {  //awards:奖项，angle:奖项对应的角度	    $('#Pointer').stopRotate();	    $("#Pointer").rotate({
	        angle: 0,	        duration: 5000,	        animateTo: angle + 1800, //angle是图片上各奖项对应的角度，1440是我要让指针旋转4圈。所以最后的结束的角度就是这样子^^	        callback: function () {
	            if (lotteryId > 0) {
	                $("#hidLotteryId").val(lotteryId);
	                if ($("#hidPageSource").val() != "change") {
	                    layer.open({
	                        type: 1,
	                        title: false,
	                        skin: 'bluePopup bluePopupForm', //样式类名
	                        closeBtn: 1, //不显示关闭按钮
	                        shift: 2,
	                        shadeClose: false, //开启遮罩关闭
	                        content: '<div class="awardb"><div class="aConBox"><p>恭喜您！<br/>获得'+text+'！</p><em>请填写礼品邮寄地址</em><form><div><input type="text" id="UserName" placeholder="请输入您的姓名" /></div><div><input type="text" id="UserMobile" placeholder="请输入您的手机" /></div><div><select id="Province" name="Province" class="select-city"><option value="">省</option></select><select id="City" name="City" class="select-city"><option value="">市</option></select><select id="Area" name="Area" class="select-city"><option value="">区</option></select></div><div><input type="text" id="SendAddress" placeholder="请输入详细地址" /></div></form><span class="tishi">温馨提示：<br>1、若填写有误，视为主动放弃获奖资格。 <br>2、活动结束后5个工作日邮寄礼品，请耐心等候。<br></span></div></div>',
	                        btn: ['提交'],
	                        yes: function (index, layero) {
	                            //按钮【按钮一】的回调
	                            ExChangeLottery.AddXDSendInfo(index);
	                        },
	                        success: function () {
	                            loadPCA();
	                        }
	                    });
	                }
	                else {
	                    ExChangeLottery.SharePop("恭喜您！<br/>获得" + text + "！");
	                }
	                
	            }
	            else {
	                layer.open({
	                    type: 1,
	                    skin: 'bluePopup bluePopupForm2', //样式类名
	                    area: ['500px', '200px'],
	                    title: false,
	                    shift: 2,
	                    shadeClose: true, //开启遮罩关闭
	                    content: '<div class="noaward"><div class="awardb"><div class="aConBox"><p>很遗憾！<br>没有中奖！</p></div></div></div>',
	                    btn: ['更多好礼'],
	                    yes: function (index, layero) {
	                        window.location.href = "/OrderChange/index?source=" + $("#hidSource").val() + "&type=1";
	                    }
	                });
	            }
	        }
	    });
	};	$("#Pointer").rotate({
	    bind:
          {
              click: function () {
                  var params = {
                      mobileOrUserId: $("#hidPageSource").val() == "change" ? $("#hidMobile").val() : $("#hidUserId").val(),
                      lotteryType: 0,
                      source: $("#hidSource").val(),
                      inviteOrChange: $("#hidPageSource").val()
                  };
                  var lottery = ExChangeLottery.GetOrderChangeLottery(params);
                  if (lottery == null) {
                      ExChangeLottery.PopWindownBlue("奖品不存在，请设置奖品！");
                      return;
                  }
                  if (lottery.code != "200") {
                      ExChangeLottery.PopWindownBlue(lottery.msg);
                      return;
                  }

                  var lotteryposition = eval("(" + lottery.data.LotteryPosition + ")");
                  var pcposition = lotteryposition["pc"];                  var positionrange = pcposition[Math.floor(Math.random() * pcposition.length)];                  var angle = Math.floor(Math.random() * (positionrange["position"][1] - positionrange["position"][0]) + positionrange["position"][0]);                  $("#hidLotteryRecordId").val(lottery.data.LotteryRecordId);                  rotateFunc(1, angle, lottery.data.LotteryName, lottery.data.LotteryId);
              }
          }
	});

});


// 中奖名单滚动
(function($){
	$.fn.myScroll = function(options){
	//默认配置
	var defaults = {
		speed:30,  //滚动速度,值越大速度越慢
		rowHeight:24 //每行的高度
	};
	var opts = $.extend({}, defaults, options),intId = [];
	function marquee(obj, step){
		obj.find("ul").animate({
			marginTop: '-=1'
		},0,function(){
				var s = Math.abs(parseInt($(this).css("margin-top")));
				if(s >= step){
					$(this).find("li").slice(0, 1).appendTo($(this));
					$(this).css("margin-top", 0);
				}
			});
		}
		this.each(function(i){
			var sh = opts["rowHeight"],speed = opts["speed"],_this = $(this);
			intId[i] = setInterval(function(){
				if(_this.find("ul").height()<=_this.height()){
					clearInterval(intId[i]);
				}else{
					marquee(_this, sh);
				}
			}, speed);

			_this.hover(function(){
				clearInterval(intId[i]);
			},function(){
				intId[i] = setInterval(function(){
					if(_this.find("ul").height()<=_this.height()){
						clearInterval(intId[i]);
					}else{
						marquee(_this, sh);
					}
				}, speed);
			});
		});
	}
})(jQuery);














