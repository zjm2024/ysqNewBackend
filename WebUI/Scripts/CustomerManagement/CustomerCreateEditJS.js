$(document).ready(function () {
    initDatePicker();
    Init();
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
                required: "请输入手机号码！"
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

    $("button[id*='btn_save']").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var customerId = parseInt($("#" + hidCustomerId).val());
        var customerVO = GetCustomerVO();
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

    $("button[id*='btn_ViewBusiness']").click(function () {
        window.location.href = "BusinessCreateEdit.aspx";
        return false;
    });

    $("button[id*='btn_ApplicantBusiness']").click(function () {
        window.location.href = "BusinessCreateEdit.aspx";
        return false;
    });

    $("button[id*='btn_ViewAgency']").click(function () {
        window.location.href = "AgencyCreateEdit.aspx";
        return false;
    });

    $("button[id*='btn_ApplicantAgency']").click(function () {
        window.location.href = "AgencyCreateEdit.aspx";
        return false;
    });
    /*
    $("#btn_cancel").click(function () {
        window.location.href = "CustomerBrowse.aspx";
        return false;
    });*/
       
    $("textarea[id$='txtDescription']").attr("maxlength", "400");
});

function change(uploadId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("img[id*='" + uploadId + "Pic']").attr("src", data.Result.FilePath.replace("~", _APIURL));
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
    });
}

function GetCustomerVO() {
    var customerVO = new Object();

    var objHeaderLogo = $("img[id*='imgHeaderLogoPic']");
	var objCustomerCode = $("input[id*='txtCustomerCode']");
	var objCustomerAccount = $("input[id*='txtCustomerAccount']");
	var objPassword = $("input[id*='txtPassword']");
	var objPhone = $("input[id*='txtPhone']");
	var objCustomerName = $("input[id*='txtCustomerName']");
	var objBirthday = $("input[id*='txtBirthday']");
	var objEmail = $("input[id*='txtEmail']");
	var objDescription = $("textarea[id*='txtDescription']");
	var objMale = $("input[id*='radSexMale']:checked");

	customerVO.CustomerId = parseInt($("#" + hidCustomerId).val());
	customerVO.HeaderLogo = objHeaderLogo.attr("src");
	customerVO.CustomerCode = objCustomerCode.val();
	customerVO.CustomerAccount = objCustomerAccount.val();
	customerVO.Phone = objPhone.val();
	customerVO.CustomerName = objCustomerName.val();	
	customerVO.Birthday = objBirthday.val();
	customerVO.Email = objEmail.val();
	customerVO.Description = objDescription.val();
	
	if (objMale.length > 0)
	    customerVO.Sex = true;
	else
	    customerVO.Sex = false;


    return customerVO;
}

function SetCustomer(customerVO) {

    var objHeaderLogo = $("img[id*='imgHeaderLogoPic']");
	var objCustomerCode = $("input[id*='txtCustomerCode']");
	var objCustomerAccount = $("input[id*='txtCustomerAccount']");
	var objPhone = $("input[id*='txtPhone']");
	var objCustomerName = $("input[id*='txtCustomerName']");
	var objBirthday = $("input[id*='txtBirthday']");
	var objEmail = $("input[id*='txtEmail']");
	var objDescription = $("textarea[id*='txtDescription']");
	var objMale = $("input[id*='radSexMale']");
	var objFeMale = $("input[id*='radSexFeMale']");
	var objBusinessInfo = $("span[id*='lblBusinessInfo']");
	var objAgencyInfo = $("span[id*='lblAgencyInfo']");

	objHeaderLogo.val(customerVO.HeaderLogo);
	$("#imgHeaderLogoPic").attr("src", customerVO.HeaderLogo);
	objCustomerCode.val(customerVO.CustomerCode);
	objCustomerAccount.val(customerVO.CustomerAccount);
	objPhone.val(customerVO.Phone);
	objCustomerName.val(customerVO.CustomerName);
	//objBirthday.val(new Date(customerVO.Birthday).format("yyyy-MM-dd"));
	objBirthday.datepicker("setDate", new Date(customerVO.Birthday));
	objEmail.val(customerVO.Email);
	objDescription.val(customerVO.Description);

	if (customerVO.Sex == 1)
        objMale.attr("checked", true);
    else
	    objFeMale.attr("checked", true);


	var btnViewBusiness = $("#btn_ViewBusiness");
	var btnApplicantBusiness = $("#btn_ApplicantBusiness");
	var btnViewAgency = $("#btn_ViewAgency");
	var btnApplicantAgency = $("#btn_ApplicantAgency");

	if (customerVO.BusinessId > 0) {
	    if (customerVO.BusinessStatus == 0) {
	        objBusinessInfo.html("审核中");
	    } else if (customerVO.BusinessStatus == 1) {
	        objBusinessInfo.html("已认证");
	    } else if (customerVO.BusinessStatus == 2) {
	        objBusinessInfo.html("已拒绝");
	    }
	    btnViewBusiness.show();
	    btnApplicantBusiness.hide();
	} else {
	    objBusinessInfo.html("未认证");
	    btnViewBusiness.hide();
	    btnApplicantBusiness.show();
	}

	if (customerVO.AgencyId > 0) {
	    if (customerVO.AgencyStatus == 0) {
	        objAgencyInfo.html("审核中");
	    } else if (customerVO.AgencyStatus == 1) {
	        objAgencyInfo.html("已认证");
	    } else if (customerVO.AgencyStatus == 2) {
	        objAgencyInfo.html("已拒绝");
	    }
	    btnViewAgency.show();
	    btnApplicantAgency.hide();
	} else {
	    objAgencyInfo.html("未认证");
	    btnViewAgency.hide();
	    btnApplicantAgency.show();
	}

}

function Init() {
    $("input[name='id-input-file']").ace_file_input({
        no_file: '请选择 ...',
        btn_choose: '选择',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false
    });
    $(".ace-file-input").attr("style", "width: 41.6666%;");

    //var customerId = parseInt($("#" + hidCustomerId).val());
    //if (customerId > 0) {
    //    load_show();
    //    $.ajax({
    //        url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + customerId + "&token=" + _Token,
    //        type: "Get",
    //        data: null,
    //        async: false,
    //        success: function (data) {
    //            if (data.Flag == 1) {
    //                var customerVO = data.Result;
    //                SetCustomer(customerVO);

    //            } else {
    //                bootbox.dialog({
    //                    message: data.Message,
    //                    buttons:
    //                    {
    //                        "Confirm":
    //                        {
    //                            "label": "确定",
    //                            "className": "btn-sm btn-primary",
    //                            "callback": function () {

    //                            }
    //                        }
    //                    }
    //                });
    //            }
    //            load_hide();
    //        },
    //        error: function (data) {
    //            alert(data);
    //            load_hide();
    //        }
    //    });
    //}
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