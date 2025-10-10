$(document).ready(function () {
    initDatePicker();
    SetButton();
    Init();

    /*
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $.validator.addMethod("listrequired", function (value, element, params) {
        if ($(element).children().length < 1) {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $.validator.addMethod("isTel", function (value, element, params) {
        var length = value.length;
        var mobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value)) || length == 0;
    }, "请输入正确格式的电话");

    $.validator.addMethod("Password", function (value, element, params) {
        var arr = value.split(".");
        var reg = /^[a-zA-Z]\w{5,17}$/;
        if (reg.test(value)) {
            return true;
        }
        else
            return false;
    }, "密码必须以字母开头，长度在6-18之间，只能包含字符、数字和下划线");

    $.validator.addMethod("email", function (value, element, params) {
        var ema = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/;
        return this.optional(element) || (ema.test(value));
    }, "请输入正确格式的电子邮箱");

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {  
            ctl00$ContentPlaceHolder_Content$txtCustomerAccount: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel : true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtEmail: {
                email: true
            }
        },
        messages: {
            
            ctl00$ContentPlaceHolder_Content$txtCustomerAccount: {
                required: "请输入账号！"
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: "请输入联系电话！"
            },
            ctl00$ContentPlaceHolder_Content$txtEmail: {
                email: "请输入正确格式的电子邮箱！"
            }
        },
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
        },

        success: function (e) {
            $(e).closest('.form-group').removeClass('has-error');
            $(e).remove();
        }
    });

    */
    $("#btn_save").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var customerId = parseInt($("#" + hidCustomerId).val());
        var customerVO = GetCustomerVO();
        console.log(customerVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateCustomer?token=" + _Token,
            type: "POST",
            data: customerVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (customerId < 1) {
                        $("#" + hidCustomerId).val(data.Result);
                        SetButton();
                    }
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
    });

    

    $("#btn_cancel").click(function () {
        window.location.href = "CustomerBrowse.aspx";
        return false;
    });
          
    $("#btn_Finance").click(function () {
        var customerId = parseInt($("#" + hidCustomerId).val());
        window.location.href = "CustomerFinance.aspx?CustomerId=" + customerId;
        return false;
    });

    $("#btn_PromotionAwards").click(function () {
        var customerId = parseInt($("#" + hidCustomerId).val());
        Recharge(customerId);
        return false;
    });
});

function Recharge(CustomerId) {
    var title = "发放推广奖励";
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{ top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseRecharge.aspx" height="200px" width="100%" frameborder="0"><\/iframe>',

        title: title,
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCost']")
                    var Sum = objCost.val();
                    AddExchangeCodeDialog(Sum, CustomerId);
                }
            },
            "Cancel":
            {
                "label": "取消",
                "className": "btn-sm",
                "callback": function () {
                }
            }
        }
    });
    return false;
}

function AddExchangeCodeDialog(Sum, CustomerId) {
    var message = "确定要发放<font style='color:#ff0000'>" + Sum + "</font>元推广奖励吗？"
    bootbox.dialog({
        message: message,
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    AddExchangeCode(Sum, CustomerId);
                }
            },
            "Cancel":
            {
                "label": "取消",
                "className": "btn-sm",
                "callback": function () {
                }
            }
        }
    });
}
function AddExchangeCode(Sum, CustomerId) {
    console.log(Sum, CustomerId);
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/DistributePromotionAwards?RebateCost=" + Sum + "&CustomerId=" + CustomerId + "&token=" + _Token,
        type: "get",
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                window.location.href = window.location.href;
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

function GetCustomerVO() {
    var customerVO = new Object();

    var objHeaderLogo = $("img[id*='imgHeaderLogo']");
	var objCustomerCode = $("input[id*='txtCustomerCode']");
	var objCustomerAccount = $("input[id*='txtCustomerAccount']");
	var objPassword = $("input[id*='txtPassword']");
	var objPhone = $("input[id*='txtPhone']");
	var objCustomerName = $("input[id*='txtCustomerName']");
	var objBirthday = $("input[id*='txtBirthday']");
	var objEmail = $("input[id*='txtEmail']");
	var objDescription = $("textarea[id*='txtDescription']");
	var objMale = $("input[id*='radSexMale']:checked");
	var objEnable = $("input[id*='radStatusEnable']:checked");
	var objAgent = $("input[id*='radAgentEnable']:checked");
	var objVip = $("select[id*='VipLevel']");
	var objExpirationAt = $("input[id*='ExpirationAt']");
	var objoriginCustomerId = $("input[id*='originCustomerId']");

	customerVO.CustomerId = parseInt($("#" + hidCustomerId).val());
	customerVO.HeaderLogo = objHeaderLogo.attr("src");
	customerVO.CustomerCode = objCustomerCode.val();
	customerVO.CustomerAccount = objCustomerAccount.val();
	customerVO.Phone = objPhone.val();
	customerVO.CustomerName = objCustomerName.val();	
	customerVO.Birthday = objBirthday.val();
	customerVO.Email = objEmail.val();
	customerVO.Description = objDescription.val();
	customerVO.ExpirationAt = objExpirationAt.val();
	customerVO.originCustomerId = objoriginCustomerId.val();
	
	
	if (objMale.length > 0)
	    customerVO.Sex = true;
	else
	    customerVO.Sex = false;

	if (objEnable.length > 0)
	    customerVO.Status = 1;
	else
	    customerVO.Status = 0;

	if (objAgent.length > 0)
	    customerVO.Agent = true;
	else
	    customerVO.Agent = false;


	var VipLevel = objVip.val();
	if (VipLevel > 0)
	{
	    customerVO.isVip = true;
	    customerVO.VipLevel = VipLevel;
	}
	else
	{
	    customerVO.isVip = false;
	    customerVO.VipLevel = VipLevel;
	}
	   

    return customerVO;
}

function SetCustomer(customerVO) {

    var objHeaderLogo = $("img[id*='imgHeaderLogo']");
	var objCustomerCode = $("input[id*='txtCustomerCode']");
	var objCustomerAccount = $("input[id*='txtCustomerAccount']");
	var objPhone = $("input[id*='txtPhone']");
	var objCustomerName = $("input[id*='txtCustomerName']");
	var objBirthday = $("input[id*='txtBirthday']");
	var objEmail = $("input[id*='txtEmail']");
	var objDescription = $("textarea[id*='txtDescription']");
	var objMale = $("input[id*='radSexMale']");
	var objFeMale = $("input[id*='radSexFeMale']");
	var objEnable = $("input[id*='radStatusEnable']");
	var objDisable = $("input[id*='radStatusDisable']");

	var objAgentEnable = $("input[id*='radAgentEnable']");
	var objAgentDisable = $("input[id*='radAgentDisable']");

	var objVipLevel = $("select[id*='VipLevel']");
	var objExpirationAt = $("input[id*='ExpirationAt']");
	var objoriginCustomerId = $("input[id*='originCustomerId']");


	objHeaderLogo.attr("src",customerVO.HeaderLogo);
	objCustomerCode.val(customerVO.CustomerCode);
	objCustomerAccount.val(customerVO.CustomerAccount);
	objPhone.val(customerVO.Phone);
	objCustomerName.val(customerVO.CustomerName);
	//objBirthday.val(new Date(customerVO.Birthday).format("yyyy-MM-dd"));
	objBirthday.datepicker("setDate", new Date(customerVO.Birthday));

	if (customerVO.ExpirationAt != "1900-01-01T00:00:00") {
	    objExpirationAt.datepicker("setDate", new Date(customerVO.ExpirationAt));
	}
	
	objEmail.val(customerVO.Email);
	objDescription.val(customerVO.Description);
	objoriginCustomerId.val(customerVO.originCustomerId);
	

	if (customerVO.Sex == 1)
        objMale.attr("checked", true);
    else
	    objFeMale.attr("checked", true);

	if (customerVO.Status == 1)
	    objEnable.attr("checked", true);
	else
	    objDisable.attr("checked", true);

	if (customerVO.Agent == 1)
	    objAgentEnable.attr("checked", true);
	else
	    objAgentDisable.attr("checked", true);


	if (customerVO.isVip == 1)
	{
	    if (customerVO.VipLevel>0)
	        objVipLevel.val(customerVO.VipLevel);
	    else
	        objVipLevel.val(1);
	}
	   
	else
	    objVipLevel.val(0);
}

function SetButton() {
    
}

function Init() {
    var customerId = parseInt($("#" + hidCustomerId).val());
    if (customerId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + customerId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var customerVO = data.Result;
                    SetCustomer(customerVO);
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
                load_hide();
            },
            error: function (data) {
                alert(data);
                load_hide();
            }
        });
    }
}

function initDatePicker() {
    $('.date-picker').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
    $('.timepicker1').timepicker({
        minuteStep: 1,
        defaultTime: false,
        showSeconds: true,
        showMeridian: false, showWidgetOnAddonClick: false
    }).next().on("click", function () {
        $(this).prev().focus();
    });

    $('.date-picker-yyyy').datepicker({
        minViewMode: 'years',
        format: 'yyyy',
        autoclose: true,
        startViewMode: 'year',
        startDate: '1900',
        endDate: '2100',
        language: 'zh-CN'
    })
    .next().on("click", function () {
        $(this).prev().focus();
    });
}