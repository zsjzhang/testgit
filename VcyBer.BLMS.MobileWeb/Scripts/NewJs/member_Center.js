//弹出层示例（以下方面脚导航的'.nav-1'，'.nav-2'做为示例）
 		//$(function(){
		//	$('.nav-1').on('click',function(){
		//		layer.open({
		//			type: 1,
		//			title: false,
		//			className: 'PopWindown', //样式类名
		//			closeBtn: 0, //不显示关闭按钮
		//			shift: 2,
		//			shadeClose: true, //开启遮罩关闭
		//			content: '提交失败！<br>请联系客服400-800-1100',
		//			btn:['确定']
		//		});			
		//	});
			
		//	$('.nav-2').on('click',function(){
		//		layer.open({
		//			type: 1,
		//			title: false,
		//			className: 'PopWindown', //样式类名
		//			closeBtn: 0, //不显示关闭按钮
		//			shift: 2,
		//			shadeClose: true, //开启遮罩关闭
		//			content: '提交成功',
		//			time:2
		//		});			
		//	});
		//});
 		//弹窗结束

 		
 		$(document).ready(function() {
 			//我要绑定下拉框显示
 			$('#mybind').click(function() {
 				$('.carbind_from').show();
 			});
 			$('#carbind_cancel').click(function(event) {
 				$('.carbind_from').hide();
 			});
 			//缴费获取积分
 			$('.member_pay').click(function(){
 				if($('.member_pay i').hasClass('pay_top')){
			 		$('.member_payfrom').show();
			 		$('.member_pay i').removeClass('pay_top').addClass('pay_bottom');
			 	}else{ 
			 		$('.member_payfrom').hide();
			 		$('.member_pay i').removeClass('pay_bottom').addClass('pay_top');
			 	}
			});
			//缴费获取积分进度
 			$('.member_payprocess').click(function(){
 				if($('.member_payprocess i').hasClass('pay_top')){
			 		$('.member_paybar').show();
			 		$('.member_payprocess i').removeClass('pay_top').addClass('pay_bottom');
			 	}else{ 
			 		$('.member_paybar').hide();
			 		$('.member_payprocess i').removeClass('pay_bottom').addClass('pay_top');
			 	}
			});
 		});