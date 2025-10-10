$(document).ready(function () {
    SetButton();
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
                        DeleteAction();
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
    });

    $("#btn_cancel").click(function () {
        window.location.href = "SuggestionBrowse.aspx";
        return false;
    });

    $("#btn_Reply").click(function () {
        var suggestionId = parseInt($("#" + hidSuggestionId).val());
        window.location.href = "SuggestionReply.aspx?suggestionId=" + suggestionId;
        return false;
    });
});

function DeleteAction() {
    var suggestionId = parseInt($("#" + hidSuggestionId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/System/DeleteSuggestion?suggestionId=" + suggestionId + "&token=" + _Token,
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
                                window.location.href = "SuggestionBrowse.aspx";
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
}

function SetSuggestion(suggestionVO) {

    var objContactPerson = $("input[id*='txtContactPerson']");
    var objContactPhone = $("input[id*='txtContactPhone']");
    var objTitle = $("input[id*='txtTitle']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objCreatedAt = $("input[id*='txtCreatedAt']");

    $("#" + hidSuggestionId).val(suggestionVO.SuggestionId);
    objContactPerson.val(suggestionVO.ContactPerson);
    objContactPhone.val(suggestionVO.ContactPhone);
    objTitle.val(suggestionVO.Title);
    objDescription.val(suggestionVO.Description);
    var createdAtStr = new Date(suggestionVO.CreatedAt).format("yyyy-MM-dd hh:mm:ss");
    if (createdAtStr != "1900-01-01 00:00:00")
        objCreatedAt.val(createdAtStr);

}

function SetButton() {
    var btnSave = $("#btn_save");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();
    var suggestionId = parseInt($("#" + hidSuggestionId).val());

    if (isEdit == "true") {
        btnSave.show();
    } else {
        btnSave.hide();
    }
    if (suggestionId < 1) {
        btnDelete.hide();
    } else {
        if (isDelete == "true") {
            btnDelete.show();
        } else {
            btnDelete.hide();
        }
    }
}

function Init() {
    var suggestionId = parseInt($("#" + hidSuggestionId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/System/GetSuggestion?suggestionId=" + suggestionId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var suggestionVO = data.Result;
                SetSuggestion(suggestionVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}