		$(function(){
			
			//手机正则
			var n_zz_mobile = /^1[3|5|7|8]{1}[0-9]{9}$/;
			
			$('.from_btn').on('click',function(){
				//共14项不为空判断
			    if ($('#section_name').val() == "" ) {
						layer.open({
						type: 1,
						title: false,
						className: 'PopWindown', //样式类名
						closeBtn: 0, //不显示关闭按钮
						shift: 2,
						shadeClose: true, //开启遮罩关闭
						content: '提交失败！<br>姓名不能为空，谢谢!',
						btn: ['确定']
                       
						});	
				}else if($('#section_tel').val()=="" || !n_zz_mobile.test($('#section_tel').val()) ){
					layer.open({
						type: 1,
						title: false,
						className: 'PopWindown', //样式类名
						closeBtn: 0, //不显示关闭按钮
						shift: 2,
						shadeClose: true, //开启遮罩关闭
						content: '提交失败！<br>请正确填写11位手机号码',
						btn:['确定']
					});	
				}else if($('#section_car').val()==""){
					layer.open({
						type: 1,
						title: false,
						className: 'PopWindown', //样式类名
						closeBtn: 0, //不显示关闭按钮
						shift: 2,
						shadeClose: true, //开启遮罩关闭
						content: '提交失败！<br>车辆信息不能为空，谢谢!',
						btn:['确定']
					});	
				}else if($('#section_color').val()==""){
					layer.open({
						type: 1,
						title: false,
						className: 'PopWindown', //样式类名
						closeBtn: 0, //不显示关闭按钮
						shift: 2,
						shadeClose: true, //开启遮罩关闭
						content: '提交失败！<br>车辆颜色不能为空，谢谢!',
						btn:['确定']
					});	
				}else if($("input:radio[name='question0']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第一个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question1']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第二个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question2']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第三个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question3']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第四个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question4']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第五个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question5']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第六个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question6']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第七个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question7']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第八个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question8']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第九个问题，谢谢!',
							btn:['确定']
						});	
				}else if($("input:radio[name='question9']:checked").val()==null){
						layer.open({
							type: 1,
							title: false,
							className: 'PopWindown', //样式类名
							closeBtn: 0, //不显示关闭按钮
							shift: 2,
							shadeClose: true, //开启遮罩关闭
							content: '提交失败！<br>请回答第十个问题，谢谢!',
							btn:['确定']
						});	
				} else {
				    var section_name = $("#section_name").val();
				    var section_tel = $("#section_tel").val();
				    var section_car = $("#section_car").val();
				    var section_color = $("#section_color").val(); // 
				    var arrays = [];
				    for (var i = 0; i < 10; i++) {
				        var item = "question" + i;
				        arrays.push($("input[name='" + item + "']:checked").val());
				    }
				    var obj = {
				        "section_name": section_name, "section_tel": section_tel, "section_car": section_car,
				        "section_color": section_color, "arrays": arrays.toString()
				};
				    $.ajax({
				        //提交数据的类型 POST GET
				        type: "POST",
				        //提交的网址
				        url: "/SSIQuestionnaire/PostSSIQuestionnaire",
				        //提交的数据
				        data: obj,
				        //返回数据的格式
				        datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
				        //在请求之前调用的函数
				        //成功返回之后调用的函数             
				        success: function (data) {
				            if (data.Code == "200") {
				                layer.open({
				                    type: 1,
				                    title: false,
				                    className: 'PopWindown', //样式类名
				                    closeBtn: 0, //不显示关闭按钮
				                    shift: 2,
				                    shadeClose: true, //开启遮罩关闭
				                    content: '提交成功,感谢您的支持!',
				                    time: 5,
				                    btn: ['确定'],
				                    yes: function() {
				                        window.location.reload();
				                    }
				                });
				            } else {
				                layer.open({
				                    type: 1,
				                    title: false,
				                    className: 'PopWindown', //样式类名
				                    closeBtn: 0, //不显示关闭按钮
				                    shift: 2,
				                    shadeClose: true, //开启遮罩关闭
				                    content: '网络错误,请重新填写!',
				                    time: 5,
				                    btn: ['确定'],
				                    yes: function () {
				                        window.location.reload();
				                    }
				                });
				            }

				        },
				        //调用出错执行的函数
				        error: function () {
				            //请求出错处理
				        }
				    });
					//提交成功
					
				}

				

			});

		});	
				
			
			//提交失败时调用
			/*$('.from_btn').on('click',function(){
				layer.open({
					type: 1,
					title: false,
					className: 'PopWindown', //样式类名
					closeBtn: 0, //不显示关闭按钮
					shift: 2,
					shadeClose: true, //开启遮罩关闭
					content: '提交失败！<br>请联系客服400-800-1100',
					btn:['确定']
				});			
			});*/
			

			//提交成功时调用
			
			/*$('.nav-2').on('click',function(){
				layer.open({
					type: 1,
					title: false,
					className: 'PopWindown', //样式类名
					closeBtn: 0, //不显示关闭按钮
					shift: 2,
					shadeClose: true, //开启遮罩关闭
					content: '提交成功',
					time:2
				});			
			});*/