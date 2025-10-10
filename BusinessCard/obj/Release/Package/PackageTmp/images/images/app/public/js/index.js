// JavaScript Document
$(function () {
	//展开收缩左侧栏目
	var isList=true
	$('.menu').click(function(e) {
		if(isList){
			isList=false;
			$('.Mainleft').addClass('on');
			$('.Mainright').addClass('on');
		}else{
			isList=true;
			$('.Mainleft').removeClass('on');
			$('.Mainright').removeClass('on');
		}
	});
	
	//展开收缩菜单
	$('.contact_div ul li').click(function(e) {
		var isclickopen=false;
		for(var i=0;i<e.originalEvent.path.length;i++){
			if(e.originalEvent.path[i].className=="open")
				isclickopen=true;
		}
	   	if(!$(this).hasClass('on')){
			$('.contact_div ul li').removeClass('on');
			$(this).addClass('on');
			$('.contact_div ul li .list').not('.contact_div ul li.on .list').animate({height: 'hide'}, 500)
			$('.contact_div ul li.on').find('.list').animate({height: 'show'}, 500)
		}else if(isclickopen){
			$('.contact_div ul li.on').find('.list').animate({height: 'hide'}, 500);
			$('.contact_div ul li.on').removeClass('on');	
		}
	});
});