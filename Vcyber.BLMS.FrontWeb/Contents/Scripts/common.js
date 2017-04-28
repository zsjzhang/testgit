/*手机自适应设置*/
window.onload=window.onresize=window.onscroll=function(){	
	fontSize();
	pageShow();
};

function pageShow(){
	var oBox = document.getElementsByTagName('body')[0];
	oBox.style.visibility = 'visible';
}

function fontSize(){
	document.documentElement.style.fontSize = 100*(document.documentElement.clientWidth/640)+'px';
}

//input输入框光标点入，默认文字消失
function inpTextF(id,text){
	$(id).focus(function(){
		$(this).attr('placeholder','');
	});
	$(id).blur(function(){
		if($(this).attr('placeholder')==''){
			$(this).attr('placeholder',text);
	}
});
}