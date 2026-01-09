$(document).ready(function () {
    Init();    
    initDatePicker();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtCode: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtCode: {
                required: "请输入优惠码！"
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: "请输入优惠价格！"
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

        var DiscountCodeID = parseInt($("#" + hidDiscountCodeID).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddCardDiscountCode?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (DiscountCodeID < 1) {
                        DiscountCodeID = data.Result;
                        $("#" + hidDiscountCodeID).val(data.Result);
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
                                    window.location.href = "CardDiscountCodeBrowse.aspx";
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
    });
       

    $("#btn_cancel").click(function () {
        window.location.href = "CardDiscountCodeBrowse.aspx";
        return false;
    });
          
    
});

function GetSystemMessageVO() {
    var CardDiscountCodeVO = new Object();

    var objCode = $("input[id*='txtCode']");
    var objCost = $("input[id*='txtCost']");
    var objExpirationAt = $("input[id*='ExpirationAt']");

    CardDiscountCodeVO.DiscountCodeID = parseInt($("#" + hidDiscountCodeID).val());
    CardDiscountCodeVO.Code = objCode.val();    
    CardDiscountCodeVO.Cost = objCost.val();
    CardDiscountCodeVO.ExpirationAt = objExpirationAt.val();

    return CardDiscountCodeVO;
}

function SetSystemMessage(CardDiscountCodeVO) {

    var objCode = $("input[id*='txtCode']");
    var objCost = $("input[id*='txtCost']");
    var objExpirationAt = $("input[id*='ExpirationAt']");
	
    objCode.val(CardDiscountCodeVO.Code);
    objCost.val(CardDiscountCodeVO.Cost);

    if (CardDiscountCodeVO.ExpirationAt != "1900-01-01T00:00:00") {
        objExpirationAt.datepicker("setDate", new Date(CardDiscountCodeVO.ExpirationAt));
    }
}
function Init() {
    var DiscountCodeID = parseInt($("#" + hidDiscountCodeID).val());
    console.log(DiscountCodeID);
    if (DiscountCodeID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetCardDiscountCode?DiscountCodeID=" + DiscountCodeID + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var CardNoticeVO = data.Result;
                    SetSystemMessage(CardNoticeVO);
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
