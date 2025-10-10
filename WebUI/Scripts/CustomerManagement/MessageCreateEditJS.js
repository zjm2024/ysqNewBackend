$(document).ready(function () {
    Init();

    $("#btn_delete").click(function () {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        var messageId = parseInt($("#" + hidMessageId).val());
                        DeleteAction(messageId);
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
    });
          
    
});
function DeleteAction(messageId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteMessage?messageId=" + messageId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
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
                                window.location.href = "MessageBrowse.aspx";
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
            //load_hide();
        },
        error: function (data) {
            alert(data);
            //load_hide();
        }
    });
}

function SetMessage(messageVO) {

    var objMessageType = $("input[id*='txtMessageTypeName']");
    var objTitle = $("input[id*='txtTitle']");
    var objSendAt = $("input[id*='txtSendAt']");
    var objMessage = $("textarea[id*='txtMessage']");

    objMessageType.val(messageVO.MessageTypeName);
    objTitle.val(messageVO.Title);
    if (new Date(messageVO.SendAt).format("yyyy-MM-dd") != "1900-01-01")
        objSendAt.val(new Date(messageVO.SendAt).format("yyyy-MM-dd"));

    objMessage.val(messageVO.Message);
}

function Init() {

    var messageId = parseInt($("#" + hidMessageId).val());
        
    if (messageId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetMessage?messageId=" + messageId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    //标记已读
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Customer/UpdateMessageStatus?messageId=" + messageId + "&status=1&token=" + _Token,
                        type: "Get",
                        data: null,
                        success: function (data) {
                            
                        },
                        error: function (data) {
                            load_hide();
                        }
                    });
                    var messageVO = data.Result;

                    SetMessage(messageVO);
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

