$(document).ready(function () {
    Init();
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

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

        var helpDocId = parseInt($("#" + hidHelpDocId).val());
        var helpDocVO = GetHelpDocVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/System/UpdateHelpDoc?token=" + _Token,
            type: "POST",
            data: helpDocVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (helpDocId < 1) {
                        $("#" + hidHelpDocId).val(data.Result);
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
    
    $("select[id$='drpHelpDocType']").change(function () {
        Reset();
        //更新Child
        var drp = $("select[id*='drpHelpDocType']");
        var childDrp = $("select[id*='drpHelpDocType2']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/System/GetHelpDocTypeList?parentHelpDocTypeId=" + drp.val() + "&enabele=false",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].HelpDocTypeId + "\">" + childList[i].HelpDocTypeName + "</option>");
                    }

                    if (childList.length > 0)
                        childDrp.val(childList[0].HelpDocTypeId);
                    Reset();
                    Init();
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("select[id$='drpHelpDocType2']").change(function () {
        Reset();
        Init();
    });
});


function GetHelpDocVO() {
    var helpDocVO = new Object();

    var objHelpDocTypeId = $("select[id*='drpHelpDocType']");
    var objHelpDocType2Id = $("select[id*='drpHelpDocType2']");
	var objTitle = $("input[id*='txtTitle']");
	var objDescription = $("textarea[id*='txtDescription']");
	var ue = UE.getEditor('container');
	var objEnable = $("input[id*='radStatusEnable']:checked");

	helpDocVO.HelpDocId = parseInt($("#" + hidHelpDocId).val());

	var docTypeId = 0;
	if (objHelpDocType2Id.val() == null) {
	    docTypeId = objHelpDocTypeId.val();
	} else {
	    docTypeId = objHelpDocType2Id.val();
	}

	helpDocVO.HelpDocTypeId = docTypeId;
	helpDocVO.Title = objTitle.val();
	helpDocVO.Description = ue.getContent();

	if (objEnable.length > 0)
	    helpDocVO.Status = true;
	else
	    helpDocVO.Status = false;

    return helpDocVO;
}

function SetHelpDoc(helpDocVO) {
	var objTitle = $("input[id*='txtTitle']");
	var ue = UE.getEditor('container');
	var objEnable = $("input[id*='radStatusEnable']");
	var objDisable = $("input[id*='radStatusDisable']");

	objTitle.val(helpDocVO.Title);
	ue.ready(function () {
	    this.setContent(helpDocVO.Description);
	});

	if (helpDocVO.Status == 1)
	    objEnable.attr("checked", true);
	else
	    objDisable.attr("checked", true);
}

function Reset() {
    var objTitle = $("input[id*='txtTitle']");
    var ue = UE.getEditor('container');
    var objEnable = $("input[id*='radStatusEnable']");
    var objDisable = $("input[id*='radStatusDisable']");

    $("#" + hidHelpDocId).val("");
    objTitle.val("");
    ue.ready(function () {
        this.setContent("");
    });



    objEnable.attr("checked", true);
    objDisable.attr("checked", false);
}


function Init() {
    var objHelpDocTypeId = $("select[id*='drpHelpDocType']");
    var objHelpDocType2Id = $("select[id*='drpHelpDocType2']");

    var docTypeId = 0;
    if (objHelpDocType2Id.val() == null) {
        docTypeId = objHelpDocTypeId.val();
    } else {
        docTypeId = objHelpDocType2Id.val();
    }

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/System/GetHelpDocByType?helpDocTypeId=" + docTypeId + "&enabele=false",
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var helpDocVO = data.Result;
                $("#" + hidHelpDocId).val(data.Result.HelpDocId);
                SetHelpDoc(helpDocVO);
            }
            //else {
            //    bootbox.dialog({
            //        message: data.Message,
            //        buttons:
            //        {
            //            "Confirm":
            //            {
            //                "label": "确定",
            //                "className": "btn-sm btn-primary",
            //                "callback": function () {

            //                }
            //            }
            //        }
            //    });
            //}
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });

}