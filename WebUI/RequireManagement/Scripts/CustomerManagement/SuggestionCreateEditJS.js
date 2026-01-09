$(document).ready(function () {   

    $.validator.addMethod("isTel", function (value, element, params) {
        var length = value.length;
        var mobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value)) || length == 0;
    }, "请输入正确格式的电话");   

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {  
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtContactPerson: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            }
        },
        messages: {
            
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtContactPerson: {
                required: "请输入联系人！"
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: "请输入联系电话！"
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: "请输入内容！"
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

        var suggestionVO = GetSuggestionVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/System/UpdateSuggestion?token=" + _Token,
            type: "POST",
            data: suggestionVO,
            success: function (data) {
                if (data.Flag == 1) {
                    bootbox.dialog({
                        message: "提交成功！谢谢！",
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    var objTitle = $("input[id*='txtTitle']");
                                    var objContactperson = $("input[id*='txtContactPerson']");
                                    var objPhone = $("input[id*='txtPhone']");
                                    var objDescription = $("textarea[id*='txtDescription']");
                                    objTitle.val("");
                                    objContactperson.val("");
                                    objPhone.val("");
                                    objDescription.val("");

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
       
});

function GetSuggestionVO() {
    var suggestionVO = new Object();

    var objTitle = $("input[id*='txtTitle']");
    var objContactperson = $("input[id*='txtContactPerson']");
    var objPhone = $("input[id*='txtPhone']");
	var objDescription = $("textarea[id*='txtDescription']");

	suggestionVO.Title = objTitle.val();
	suggestionVO.ContactPerson = objContactperson.val();
	suggestionVO.ContactPhone = objPhone.val();
	suggestionVO.Description = objDescription.val();
    
	return suggestionVO;
}

