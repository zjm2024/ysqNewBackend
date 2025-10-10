$(document).ready(function () {
    Init();    

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtMessage: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtMessage: {
                required: "请输入通告内容！"
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

        var systemMessageId = parseInt($("#" + hidSystemMessageId).val());
        var systemMessageVO = GetSystemMessageVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/System/AddSystemMessage?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (systemMessageId < 1) {
                        systemMessageId = data.Result;
                        $("#" + hidSystemMessageId).val(data.Result);
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
        window.location.href = "SystemMessageBrowse.aspx";
        return false;
    });
          
    
});


function GetSystemMessageVO() {
    var systemMessageVO = new Object();


    var objMessageType = $("select[id*='drpMessageType']");
    var objSystemMessageTitle = $("input[id*='txtTitle']");
    var objSystemMessage = $("textarea[id*='txtMessage']");

	systemMessageVO.SystemMessageId = parseInt($("#" + hidSystemMessageId).val());
	systemMessageVO.MessageTypeId = objMessageType.val();
	systemMessageVO.Title = objSystemMessageTitle.val();
	systemMessageVO.Message = objSystemMessage.val();
	


	return systemMessageVO;
}

function SetSystemMessage(systemMessageVO) {

    var objMessageType = $("select[id*='drpMessageType']");
    var objSystemMessageTitle = $("input[id*='txtTitle']");
    var objSystemMessage = $("textarea[id*='txtMessage']");
    var objSendAt = $("input[id*='txtSendAt']");
	
    objMessageType.val(systemMessageVO.MessageTypeId);
    objSystemMessageTitle.val(systemMessageVO.Title);
    objSystemMessage.val(systemMessageVO.Message);
    var sentAt = systemMessageVO.SendAt.replace("T"," ");
    
    objSendAt.val(sentAt);

}


function Init() {
    var systemMessageId = parseInt($("#" + hidSystemMessageId).val());

    if (systemMessageId > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/System/GetSystemMessage?SystemMessageId=" + systemMessageId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var systemMessageVO = data.Result;
                    SetSystemMessage(systemMessageVO);
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
