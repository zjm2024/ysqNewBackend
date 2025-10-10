$(document).ready(function () {
    Init();    
    initDatePicker();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtUrl: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtUrl: {
                required: "请输入网址！"
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

        var NoticeID = parseInt($("#" + hidPrizeID).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddFarmgamePrize?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (NoticeID < 1) {
                        NoticeID = data.Result;
                        $("#" + hidPrizeID).val(data.Result);
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
                                    window.location.href = "FarmgamePrizeBrowse.aspx";
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
        window.location.href = "FarmgamePrizeBrowse.aspx";
        return false;
    });
          
    
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
function GetSystemMessageVO() {
    var FarmgamePrizeVO = new Object();

    var objName = $("input[id*='txtName']");
    var objImgUrl = $("img[id*='ImgUrl']");
    var objProductUrl = $("input[id*='txtProductUrl']");
    var objPrice = $("input[id*='txtPrice']");
    var objOrderNO = $("input[id*='OrderNO']");

    FarmgamePrizeVO.PrizeID = parseInt($("#" + hidPrizeID).val());
    FarmgamePrizeVO.Name = objName.val();
    FarmgamePrizeVO.ImgUrl = objImgUrl.attr("src");
    FarmgamePrizeVO.ProductUrl = objProductUrl.val();
    FarmgamePrizeVO.Price = objPrice.val();
    FarmgamePrizeVO.OrderNo = objOrderNO.val();

    return FarmgamePrizeVO;
}

function SetSystemMessage(FarmgamePrizeVO) {
    var objName = $("input[id*='txtName']");
    var objImgUrl = $("img[id*='ImgUrl']");
    var objProductUrl = $("input[id*='txtProductUrl']");
    var objPrice = $("input[id*='txtPrice']");
    var objOrderNO = $("input[id*='OrderNO']");
	
    objName.val(FarmgamePrizeVO.Name);
    objImgUrl.attr("src", FarmgamePrizeVO.ImgUrl);

    objProductUrl.val(FarmgamePrizeVO.ProductUrl);
    objPrice.val(FarmgamePrizeVO.Price);
    objOrderNO.val(FarmgamePrizeVO.OrderNo);
}
function Init() {
    var PrizeID = parseInt($("#" + hidPrizeID).val());
    console.log(PrizeID);
    if (PrizeID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetFarmgamePrize?PrizeID=" + PrizeID + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var FarmgamePrizeVO = data.Result;
                    SetSystemMessage(FarmgamePrizeVO);
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
