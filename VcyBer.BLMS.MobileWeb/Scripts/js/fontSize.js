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

/*手机自适应设置*/
window.onload=window.onresize=window.onscroll=function(){	
	fontSize();
	pageShow();
};

function pageShow(){
	var oBox = document.getElementsByTagName('body')[0];
	/*var oWap = getClass(oBox,'wrapper')[0];*/
	oBox.style.visibility = 'visible';
}

function fontSize(){
	document.documentElement.style.fontSize = 100*(document.documentElement.clientWidth/640)+'px';
}
/*手机自适应设置end*/

/*会员计划菜单收发*/
function bmMp(){
	$('.mpBox .mpBoxh2').on('click',function(){
		
		if($(this).next().css('display')=='none'){
			$('.h2Pcont').hide();
			$(this).next().stop().slideDown();
			$(this).find('i').removeClass('up');
			$(this).find('i').addClass('down');
		}else{
			$(this).next().stop().slideUp();
			$(this).find('i').removeClass('down');
			$(this).find('i').addClass('up');
		}
	});
	
}













