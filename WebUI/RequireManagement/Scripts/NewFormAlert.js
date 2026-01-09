function Newform_alert(id) { //认证页弹窗
    $("#"+id).fadeIn(100);
}
function Newform_demand_alert(id) { //采购需求立即报价弹窗
	$("input[name='OfferDemandId']").val(id);
	$("input[name='OfferName']").val("");
	$("input[name='OfferCost']").val("");
	$("input[name='OfferPhone']").val("");
	$("textarea[name='OfferDescription']").val("");
    $("#demandoffer").fadeIn(100);
}
function Newform_close(id) {//认证页弹窗
    $("#"+id).fadeOut(300);
}
function wx_share_show(){//微信分享引导图显示
	$("#wx_share").show();
}
function wx_share_hide(){//微信分享引导图隐藏
	$("#wx_share").hide();
}
function Quick_release(team){ //快速发布弹窗
	var height =$(".Quick_release").height();
	var width =$(".Quick_release").width();
	var winheight =$(window).height();
	var winwidth =$(window).width();

	var left=(winwidth-width)/2;
	var top = (winheight - height) / 2;

	$(".Quick_release").css({
		'top':top+"px",
		'left':left+"px"
	});
	$(".Quick_release").fadeIn(100);	
	$('.Quick_release_left ul li').removeClass('on');
	$('#'+team+"_team").addClass('on');
	$('.Quick_release_from').hide();
	$("#"+team).show();
}
function prohibit_alert() {//提示禁止弹窗
    bootbox.dialog({
        message: "该功能暂未开放，敬请期待！",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    
                }
            }
        }
    });
}
function Quick_demand_class() {//快速发布采购需求分类
    var childDrp = $("select[id*='Quick_demand_class']");
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetCategory",
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                var childList = data.Result;
                for (var i = 0; i < childList.length; i++) {
                    childDrp.append("<option value=\"" + childList[i].CategoryId + "\">" + childList[i].CategoryName + "</option>");
                }
            }
        },
        error: function (data) {
            
        }
    });

    //如果登陆状态下则隐藏密码输入框
    if (_CustomerId > 0)
    {
        $(".password_li").hide();
        $("input[name=Quick_Require_code]").hide();
        $("button[name=Quick_Require_codebtn]").hide();
        $("input[name=Quick_demand_code]").hide();
        $("button[name=Quick_demand_codebtn]").hide();

        var Quick_Require_phone = $("input[name*='Quick_Require_phone']");
        var Phone = $("input[name*='Quick_demand_phone']");
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + _CustomerId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                Quick_Require_phone.val(data.Result.Phone);
                Phone.val(data.Result.Phone);
            }
        });

    }else{
        $(".Quick_link").hide();
    }

    //时间选择器
    var objDemandEffectiveEndDate = $("input[name*='DemandEffectiveEndDate']");

    dtTmp = new Date(Date.now());
    
    objDemandEffectiveEndDate.datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
    objDemandEffectiveEndDate.datepicker("setDate", new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + 2, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds()));
}

function saveRequirebtn() { //快速发布任务保存数据
    var Quick_Require_name = $("input[name*='Quick_Require_name']");
    var Quick_Require_cost = $("input[name*='Quick_Require_cost']");
    var Quick_Require_content = $("textarea[name*='Quick_Require_content']");
    var Quick_Require_phone = $("input[name*='Quick_Require_phone']");
    var Quick_Require_code = $("input[name*='Quick_Require_code']");
    var Quick_Require_password = $("input[name*='Quick_Require_password']");

    if (Quick_Require_name.val() == "") {
        bootbox.dialog({
            message: "请输入任务标题",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }

    if (Quick_Require_cost.val() == "") {
        bootbox.dialog({
            message: "请输入任务酬金",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }

    var ival = parseInt(Quick_Require_cost.val());//如果变量val是字符类型的数则转换为int类型 如果不是则ival为NaN
    if (isNaN(ival)) {
        bootbox.dialog({
            message: "酬金只能输入数字，不能有中文符号",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }

    if (_CustomerId == 0) {
        if (Quick_Require_password.val() == "") {
            bootbox.dialog({
                message: "请输入要注册的登陆密码",
                buttons:
                {
                    "Confirm":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {

                        }
                    }
                }
            });
            return false;
        }
    }

    if (Quick_Require_content.val() == "") {
        bootbox.dialog({
            message: "请输入任务详情",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }
	var Quick_Require_content_text=getFormatCode(Quick_Require_content.val());
    var seller_data = {
        "Requirement": {
            "Title": Quick_Require_name.val(),
            "Cost": Quick_Require_cost.val(),
            "Description": Quick_Require_content_text,
            "Phone": Quick_Require_phone.val(),
        },
    }

    $.ajax({
        url: _RootPath + "SPWebAPI/Require/QuickAddRequire?code=" + Quick_Require_code.val() + "&password=" + Quick_Require_password.val() + "&Token=" + _Token,
        type: "POST",
        data: seller_data,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                Quick_release_close();
                bootbox.dialog({
                    message: "您的销售任务已成功提交，为保证有更多的销售接单，请尽快到雇主-我的任务-修改完善任务信息",
                    buttons:
                    {
                        "click":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                }
                            },
                        "Cancel":
                            {
                                "label": "前往",
                                "className": "btn-sm",
                                "callback": function () {
                                    window.location.href = _RootPath + "RequireManagement/RequirementCreateEdit.aspx?RequirementId=" + data.Result;
                                }
                            }
                    }
                });
            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }

        },
        error: function (data) {
            console.log(data);
        }
    });

}

function savedemandbtn() { //采购需求保存数据
    var CategoryId = $("select[name*='Quick_demand_class']");
    var Phone = $("input[name*='Quick_demand_phone']");
    var Description = $("textarea[name*='Quick_demand_textarea']");
    var code = $("input[name*='Quick_demand_code']");
    var password = $("input[name*='Quick_demand_password']");
    var DemandEffectiveEndDate = $("input[name*='DemandEffectiveEndDate']");

    if (Phone.val() == "") {
        bootbox.dialog({
            message: "请输入手机号码",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }

    
    if (_CustomerId == 0) {
        if (password.val() == "") {
            bootbox.dialog({
                message: "请输入要注册的登陆密码",
                buttons:
                {
                    "Confirm":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {

                        }
                    }
                }
            });
            return false;
        }
        if (code.val() == "") {
            bootbox.dialog({
                message: "请输入验证码",
                buttons:
                {
                    "Confirm":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {

                        }
                    }
                }
            });
            return false;
        }
    }
    if (Description.val() == "") {
        bootbox.dialog({
            message: "请输入您的需求",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
        return false;
    }
	var DescriptionText=getFormatCode(Description.val());
    var seller_data = {
        "CategoryId": CategoryId.val(),
        "Phone": Phone.val(),
        "Description": DescriptionText,
        "EffectiveEndDate": DemandEffectiveEndDate.val()
    }

    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateDemand?code=" + code.val() + "&password=" + password.val() + "&Token=" + _Token,
        type: "POST",
        data: seller_data,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                Quick_release_close();
                bootbox.dialog({
                    message: "提交成功，请等待审核通过！",
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }

        },
        error: function (data) {
            console.log(data);
        }
    });

}

function Quick_release_close(){//快速发布弹窗
    $(".Quick_release").fadeOut(300);	
}
    $(document).ready(function () {
        Quick_demand_class();
        //弹窗拖动
        $('.Quick_release_left ul li').click(function(){
            $('.Quick_release_left ul li').removeClass('on');
            $(this).addClass('on');
            $('.Quick_release_from').hide();
            $("#"+$(this).attr("data")).show();
        });
	
        $(".Quick_release_title").mousedown(function(e)//e鼠标事件 
        {  
            var left = $(".Quick_release").position().left;//DIV在页面的位置
            var top = $(".Quick_release").position().top;
            

            var x = e.clientX - left;//获得鼠标指针离DIV元素左边界的距离 
            var y = e.clientY - top;//获得鼠标指针离DIV元素上边界的距离

            $(document).bind("mousemove",function(ev)//绑定鼠标的移动事件，因为光标在DIV元素外面也要有效果，所以要用doucment的事件，而不用DIV元素的事件 
            { 
                $(".Quick_release").stop();//加上这个之后 
			
                var _x = ev.clientX - x;//获得X轴方向移动的值 
                var _y = ev.clientY - y;//获得Y轴方向移动的值 
                $(".Quick_release").animate({left:_x+"px",top:_y+"px"},10); 
            }); 
        }); 

        $(document).mouseup(function() 
        { 
            $(this).unbind("mousemove"); 
        }); 
        //弹窗拖动结束
	
        //顶部搜索下拉
        $('.slDiv #btnSelect').on('click',function(){  
            $(this).siblings('.ulDiv').toggleClass('ulShow');  
        });  
        $('.slDiv .ulDiv ul li').on('click',function(){  
            var selTxt=$(this).text();  
            $('.slDiv #btnSelect').text(selTxt);  
            $('.ulDiv').removeClass('ulShow');  
        });  
        //顶部搜索下拉结束
	
        //列表页筛选模块自动浮动
        $(window).scroll(function(){
            var s=$(window).scrollTop();
            if(s>176)
            {
                $(".lsearchall").addClass("on");
            }
            else
            {
                $(".lsearchall").removeClass("on");
            }
            if (s > 300) {
                $('#backToTop').show();
                $('.right_qq').show();
            } else {
                $('#backToTop').hide();
                $('right_qq').hide();
            }
        });
        //列表页筛选模块自动浮动结束

        //回到顶部
        $("#backToTop").click(function () {
            var _this = $(this);
            $('html,body').animate({ scrollTop: 0 }, 500, function () {
                /*_this.hide();*/
            });
        });
        //回到顶部结束

    });

function getcode(type) {//获取验证码
    var objPhone = $("input[name*='Quick_" + type + "_phone']");
    $.ajax({
        url: _RootPath + "SendPassCodeMsg.aspx?phone=" + objPhone.val(),
        type: "Get",
        data: null,
        success: function (data) {
            data = JSON.parse(data);
            if (data.Flag == 1) {
                invokeSettime($("button[name*='Quick_" + type + "_codebtn']"));
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }

        },
        error: function (data) {
            alert(data);
        }
    });
}
function invokeSettime(obj) {
    var countdown = 60;
    settime(obj);
    function settime(obj) {
        if (countdown == 0) {
            $(obj).attr("disabled", false);
            $(obj).val("点击发送");
            $(obj).html("点击发送");
            countdown = 60;
            return;
        } else {
            $(obj).attr("disabled", true);
            $(obj).val(countdown + "s 重新发送");
            $(obj).html(countdown + "s 重新发送");
            countdown--;
        }
        setTimeout(function () {
            settime(obj)
        }, 1000)
    }
}
function MyMessageList() {//消息和签到弹窗
    if (_CustomerId != 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/getisSignIn?CustomerId=" + _CustomerId + "&Token=" + _Token,
            type: "Get",
            success: function (data) {
                if (data.Flag == 1) {
                    AddSignIn(data.Result);
                } else {
                    getMessageList();
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    }
};
function AddSignIn(count) {//签到弹窗
    var objMyMessageList = $('.index_zxt .MyMessageList');
    var objMyMessageList_text = $('.index_zxt .MyMessageList .MyMessageList_text');
    var objMyMessageList_btn_click = $('.index_zxt .MyMessageList_btn_click');
    var objMyMessageList_btn_Cancel = $('.index_zxt .MyMessageList_btn_Cancel');
    objMyMessageList.show();
    objMyMessageList_text.html("今天还没签到，你已经连续签到<font style=\"color: #fb5001;\">" + count + "</font>天");
    objMyMessageList_btn_Cancel.hide();
    objMyMessageList_btn_click.html("签到");

    objMyMessageList_btn_click.click(function () {
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/AddSignIn?CustomerId=" + _CustomerId + "&Token=" + _Token,
            type: "post",
            success: function (data) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }
        });
        objMyMessageList.hide();
    });
}
function getMessageList() {//投标和项目消息
    var data_filter = {
        "Filter": {
            "groupOp": "and",
            "rules": [{
                "field": "SendTo",
                "op": "eq",
                "data": _CustomerId
            }, {
                "field": "MessageTypeId",
                "op": "ne",
                "data": 1
            }, {
                "field": "MessageTypeId",
                "op": "ne",
                "data": 2
            }, {
                "field": "MessageTypeId",
                "op": "ne",
                "data": 4
            }, {
                "field": "Status",
                "op": "eq",
                "data": 0
            }],
        },
        "PageInfo": {
            "PageIndex": 1,
            "PageCount": 50,
            "SortName": "SendAt",
            "SortType": "desc"
        }
    }
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetMyMessageList?Token=" + _Token,
        type: "POST",
        data: data_filter,
        success: function (data) {
            if (data.Flag == 1) {
                if (data.Result.length > 0) {
                    MyMessageList_read(data.Result[0], data.Result.length, data.Result);
                }
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function MyMessageList_read(OneMessageObj, MessageLength, MessageObj) {
    var objMyMessageList = $('.index_zxt .MyMessageList');
    var objMyMessageList_text = $('.index_zxt .MyMessageList .MyMessageList_text');
    var objMyMessageList_btn_click = $('.index_zxt .MyMessageList_btn_click');
    var objMyMessageList_btn_Cancel = $('.index_zxt .MyMessageList_btn_Cancel');
    objMyMessageList.show();
    objMyMessageList_text.html(OneMessageObj.Message);
    var url_readmsg = _RootPath + "SPWebAPI/Customer/UpdateMessageStatus?messageId=" + OneMessageObj.MessageId + "&status=1" + "&token=" + _Token;

    objMyMessageList_btn_click.click(function () {
        $.ajax({
            url: url_readmsg,
            type: "get",
            success: function (data) {
            }
        });
        objMyMessageList.hide();
        /*
        if (MessageLength == MessageObj.length) {
            MessageObj.splice(0, 1);
        }
        if (MessageObj.length > 0) {
            MyMessageList_read(MessageObj[0], MessageObj.length, MessageObj);
        } else {
            objMyMessageList.hide();
        }*/
    });

    objMyMessageList_btn_Cancel.click(function () {
        $.ajax({
            url: url_readmsg,
            type: "get",
            success: function (data2) {
                if (OneMessageObj.MessageTypeId == 5) {
                    window.location.href = _RootPath + "RequireManagement/RequirementBrowse.aspx";
                } else if (OneMessageObj.MessageTypeId == 3) {
                    var Cts = OneMessageObj.Title;
                    if (Cts.indexOf("项目") >= 0) {
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + _CustomerId + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Result.BusinessStatus == 1 && data.Result.AgencyStatus == 1) {
                                    window.location.href = _RootPath + "ProjectManagement/BusinessProjectBrowse.aspx";
                                } else if (data.Result.BusinessStatus == 0 && data.Result.AgencyStatus == 1) {
                                    window.location.href = _RootPath + "ProjectManagement/AgencyProjectBrowse.aspx";
                                } else {
                                    window.location.href = _RootPath + "ProjectManagement/BusinessProjectBrowse.aspx";
                                }
                            }
                        });

                    } else if (Cts.indexOf("合同") >= 0) {
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + _CustomerId + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Result.BusinessStatus == 1 && data.Result.AgencyStatus == 1) {
                                    window.location.href = _RootPath + "ProjectManagement/BusinessContractBrowse.aspx";
                                } else if (data.Result.BusinessStatus == 0 && data.Result.AgencyStatus == 1) {
                                    window.location.href = _RootPath + "ProjectManagement/AgencyContractBrowse.aspx";
                                } else {
                                    window.location.href = _RootPath + "ProjectManagement/BusinessContractBrowse.aspx";
                                }
                            }
                        });
                    } else if (Cts.indexOf("乐币") >= 0) {
                        window.location.href = _RootPath + "CustomerManagement/zxbRequire.aspx";
                    } else if (Cts.indexOf("任务") >= 0) {
                        window.location.href = _RootPath + "RequireManagement/RequirementBrowse.aspx";
                    } else if (Cts.indexOf("商机") >= 0) {
                        window.location.href = _RootPath + "RequireManagement/DemandBrowse.aspx";
                    }
                }
            }
        });
    });
}

var getFormatCode = function (strValue) {//textarea 换行
    return strValue.replace(/\r\n/g, '<br/>').replace(/\n/g, '<br/>').replace(/\s/g, ' ');
}