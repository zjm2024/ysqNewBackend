  $(document).ready(function () {
    initDatePicker();
    SetButton();
    Init();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {  
            ctl00$ContentPlaceHolder_Content$txtPurpose: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtPurpose: {
                required: "请输入奖励提示文本！"
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: "请输入奖励金额！"
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

    $("#btn_save").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var customerId = parseInt($("#" + hidZxbConfigID).val());
        var customerVO = GetCustomerVO();
        console.log(customerVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateZxbConfig?token=" + _Token,
            type: "POST",
            data: customerVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (customerId < 1) {
                        $("#" + hidZxbConfigID).val(data.Result);
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
        window.location.href = "ZxbConfigBrowse.aspx";
        return false;
    });
});


function GetCustomerVO() {
    var customerVO = new Object();

    var objPurpose = $("input[id*='txtPurpose']");
    var objCost = $("input[id*='txtCost']");
	var objEnable = $("input[id*='radStatusEnable']:checked");

	customerVO.ZxbConfigID = parseInt($("#" + hidZxbConfigID).val());
	customerVO.Cost = objCost.val();
	customerVO.Purpose = objPurpose.val();
	
	if (objEnable.length > 0)
	    customerVO.Status = 1;
	else
	    customerVO.Status = 0;
    return customerVO;
}

function SetCustomer(customerVO) {
    var objPurpose = $("input[id*='txtPurpose']");
    var objCost = $("input[id*='txtCost']");
    var objcode = $("input[id*='txtcode']");
	var objEnable = $("input[id*='radStatusEnable']");
	var objDisable = $("input[id*='radStatusDisable']");

	objPurpose.val(customerVO.Purpose);
	objCost.val(customerVO.Cost);
	objcode.val(customerVO.code);

	if (customerVO.Status == 1)
	    objEnable.attr("checked", true);
	else
	    objDisable.attr("checked", true);
}

function SetButton() {
    
}

function Init() {
    var ZxbConfigID = parseInt($("#" + hidZxbConfigID).val());
    
    if (ZxbConfigID > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetZxbConfig?ZxbConfigID=" + ZxbConfigID + "&token=" + _Token,
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