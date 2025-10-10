$(document).ready(function () {
    Init();    
    initDatePicker();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtMessage: {
                required: true,
                maxlength:50
            },
            ctl00$ContentPlaceHolder_Content$txtSendAt: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtMessage: {
                required: "请输入通知内容！",
                maxlength: "不能超过50个字！",
            },
            ctl00$ContentPlaceHolder_Content$txtSendAt: {
                required: "选择发送日期！"
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
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddCardMessage?token=" + _Token,
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
                                    window.location.href = "CardMessageBrowse.aspx";
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
        window.location.href = "CardMessageBrowse.aspx";
        return false;
    });
          
    
});


function GetSystemMessageVO() {
    var CardNoticeVO = new Object();

    var objSystemMessage = $("textarea[id*='txtMessage']");
    var objSendAt = $("input[id*='txtSendAt']");

    CardNoticeVO.NoticeID = parseInt($("#" + hidNoticeID).val());

    CardNoticeVO.Content = objSystemMessage.val();
    CardNoticeVO.SendDate = objSendAt.val();
    
    CardNoticeVO.Style = parseInt($('#Style').val());

    
	return CardNoticeVO;
}

function SetSystemMessage(CardNoticeVO) {

    var objSystemMessage = $("textarea[id*='txtMessage']");
    var objSendAt = $("input[id*='txtSendAt']");
	
    objSystemMessage.val(CardNoticeVO.Content);
    var sentAt = CardNoticeVO.SendDate.replace("T", " ");

    if (CardNoticeVO.Style == 1) {
        $('#Style option:eq(1)').attr('selected', 'selected');
    }

    objSendAt.val(sentAt);
}


function Init() {
    var NoticeID = parseInt($("#" + hidNoticeID).val());
    console.log(NoticeID);
    if (NoticeID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetCardMessage?NoticeID=" + NoticeID + "&token=" + _Token,
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
