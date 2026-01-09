$(document).ready(function () {
    SetButton();
    Init();

    $("#btn_cancel").click(function () {
        var suggestionId = parseInt($("#" + hidSuggestionId).val());
        window.location.href = "SuggestionCreateEdit.aspx?SuggestionId=" + suggestionId;
        return false;
    });

    $("#btn_Reply").click(function () {
        var suggestionId = parseInt($("#" + hidSuggestionId).val());
        var objDescription = new Object();
        objDescription.Description = $("textarea[id*='txtDescription']").val();
        console.log(objDescription);
        $.ajax({
            url: _RootPath + "SPWebAPI/System/ReplySuggestion?SuggestionId=" + suggestionId + "&token=" + _Token,
            type: "post",
            data: objDescription,
            success: function (data) {
                console.log(data);
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
                                    window.location.href = "SuggestionCreateEdit.aspx?suggestionId=" + suggestionId;
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
});


function SetSuggestion(suggestionVO) {

    var objContactPerson = $("input[id*='txtContactPerson']");
    var objContactPhone = $("input[id*='txtContactPhone']");
    var objTitle = $("input[id*='txtTitle']");

    $("#" + hidSuggestionId).val(suggestionVO.SuggestionId);
    objContactPerson.val(suggestionVO.ContactPerson);
    objContactPhone.val(suggestionVO.ContactPhone);
    objTitle.val("意见反馈回复");

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