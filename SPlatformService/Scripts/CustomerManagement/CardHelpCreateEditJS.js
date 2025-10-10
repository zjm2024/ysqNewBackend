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
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
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
        var hidHelpID = parseInt($("#" + hidHelpID).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddCardHelp?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (hidHelpID < 1) {
                        hidHelpID = data.Result;
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
                                    window.location.href = "CardHelpBrowse.aspx";
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
        window.location.href = "CardHelpBrowse.aspx";
        return false;
    });
          
    
});

function change(uploadId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("input[id*='txtUrl']").val( data.Result.FilePath.replace("~", _APIURL));
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
    var objSynopsis = $("textarea[id*='txtSynopsis']");
    var objUrl = $("input[id*='txtUrl']");
    var objType = $("select[id*='Type']");
    var objOrder_Num = $("input[id*='Order_Num']");
    var objfinderUserName = $("input[id*='txtfinderUserName']");
    var objfeedId = $("input[id*='txtfeedId']");
    var objisFinder = $("input[id*='isFinderEnable']:checked");
    var objSynopsisVal = objSynopsis.val().replace(/[ ]/g, "");    //去掉空格
    objSynopsisVal = objSynopsisVal.replace(/[\r\n]/g, ""); //去掉回车换行
    console.log(objSynopsisVal)

    CardNewsVO.HelpID = parseInt($("#" + hidHelpID).val());
    CardNewsVO.Title = objTitle.val();
    CardNewsVO.Url = objUrl.val();
    CardNewsVO.Description = objSynopsisVal;
    CardNewsVO.Type = objType.val();
    CardNewsVO.Order_Num = objOrder_Num.val();
    CardNewsVO.finderUserName = objfinderUserName.val();
    CardNewsVO.feedId = objfeedId.val();
    if (objisFinder.length > 0)
        CardNewsVO.isFinder = 1;
    else
        CardNewsVO.isFinder = 0;

    return CardNewsVO;
}

function SetSystemMessage(CardNewsVO) {

    var objTitle = $("input[id*='txtTitle']");
    var objSynopsis = $("textarea[id*='txtSynopsis']");
    var objUrl = $("input[id*='txtUrl']");
    var objType = $("select[id*='Type']");
    var objOrder_Num = $("input[id*='Order_Num']");
    var objfinderUserName = $("input[id*='txtfinderUserName']");
    var objfeedId = $("input[id*='txtfeedId']");

    var objisFinderEnable = $("input[id*='isFinderEnable']");
    var objisFinderDisable = $("input[id*='isFinderDisable']");
	
    objTitle.val(CardNewsVO.Title);
    objSynopsis.val(CardNewsVO.Description);
    objUrl.val(CardNewsVO.Url);
    objType.val(CardNewsVO.Type);
    objOrder_Num.val(CardNewsVO.Order_Num);

    objfinderUserName.val(CardNewsVO.finderUserName);
    objfeedId.val(CardNewsVO.feedId);
    if (CardNewsVO.isFinder == 1)
        objisFinderEnable.attr("checked", true);
	else
        objisFinderDisable.attr("checked", true);
}
function Init() {
    var HelpID = parseInt($("#" + hidHelpID).val());
    console.log(HelpID);
    if (HelpID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetCardHelp?HelpID=" + HelpID,
            type: "Get",
            data: null,
            success: function (data) {
                console.log(data);
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
