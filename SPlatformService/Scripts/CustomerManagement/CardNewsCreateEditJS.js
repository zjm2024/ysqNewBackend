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

        var NoticeID = parseInt($("#" + hidNoticeID).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddCardNews?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (NoticeID < 1) {
                        NoticeID = data.Result;
                        $("#" + hidNoticeID).val(data.Result);
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
                                    window.location.href = "CardNewsBrowse.aspx";
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
        window.location.href = "CardNewsBrowse.aspx";
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
    var CardNewsVO = new Object();

    var objTitle = $("input[id*='txtTitle']");
    var objNewsImg = $("img[id*='imgNewsImgPic']");
    var objSynopsis = $("textarea[id*='txtSynopsis']");
    var objUrl = $("input[id*='txtUrl']");
    var objAppId = $("input[id*='txtAppId']");
    var objOrderNO = $("input[id*='OrderNO']");
    var objGoType = $("select[id*='GoType']");
    var objShowType = $("select[id*='ShowType']");
    var objSynopsisVal = objSynopsis.val().replace(/[ ]/g, "");    //去掉空格
    objSynopsisVal = objSynopsisVal.replace(/[\r\n]/g, ""); //去掉回车换行
    console.log(objSynopsisVal)

    CardNewsVO.NewsID = parseInt($("#" + hidNoticeID).val());
    CardNewsVO.Title = objTitle.val();
    CardNewsVO.NewsImg = objNewsImg.attr("src");
    CardNewsVO.Synopsis = objSynopsisVal;
    CardNewsVO.GoType = objGoType.val();
    CardNewsVO.ShowType = objShowType.val();
    
    CardNewsVO.Url = objUrl.val();
    CardNewsVO.AppId = objAppId.val();
    CardNewsVO.OrderNO = objOrderNO.val();

    return CardNewsVO;
}

function SetSystemMessage(CardNewsVO) {

    var objTitle = $("input[id*='txtTitle']");
    var objNewsImg = $("img[id*='imgNewsImgPic']");
    var objSynopsis = $("textarea[id*='txtSynopsis']");
    var objUrl = $("input[id*='txtUrl']");
    var objAppId = $("input[id*='txtAppId']");
    var objOrderNO = $("input[id*='OrderNO']");
    var objGoType = $("select[id*='GoType']");
    var objShowType = $("select[id*='ShowType']");
	
    objTitle.val(CardNewsVO.Title);
    objNewsImg.attr("src", CardNewsVO.NewsImg);

    objSynopsis.val(CardNewsVO.Synopsis);
    objUrl.val(CardNewsVO.Url);
    objAppId.val(CardNewsVO.AppId);
    objOrderNO.val(CardNewsVO.OrderNO);

    objGoType.val(CardNewsVO.GoType);
    objShowType.val(CardNewsVO.ShowType);
}
function Init() {
    var NoticeID = parseInt($("#" + hidNoticeID).val());
    console.log(NoticeID);
    if (NoticeID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetCardNews?NewsID=" + NoticeID + "&token=" + _Token,
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
