$(document).ready(function () {
    initDatePicker();
    SetButton();
    Init();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtcode: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtcode: {
                required: "请输入消息文本！"
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
        var customerId = parseInt($("#" + hidCarouselID).val());
        var customerVO = GetCustomerVO();
        
        $.ajax({
            url: _RootPath + "SPWebAPI/User/UpdateCarousel?token=" + _Token,
            type: "POST",
            data: customerVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (customerId < 1) {
                        $("#" + hidCarouselID).val(data.Result);
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
                                    window.location.href = "CarouselBrowse.aspx";
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
        window.location.href = "CarouselBrowse.aspx";
        return false;
    });
});


function GetCustomerVO() {
    var customerVO = new Object();

    var objText = $("input[id*='txtText']");

    customerVO.CarouselID = parseInt($("#" + hidCarouselID).val());
    customerVO.Text = objText.val();

    return customerVO;
}

function SetCustomer(customerVO) {
    var objText = $("input[id*='txtText']");

    objText.val(customerVO.Text);
}

function SetButton() {

}

function Init() {
    var CarouselID = parseInt($("#" + hidCarouselID).val());

    if (CarouselID > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/User/GetCarousel?CarouselID=" + CarouselID,
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