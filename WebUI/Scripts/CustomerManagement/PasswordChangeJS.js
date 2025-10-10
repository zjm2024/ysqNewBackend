$(document).ready(function () {

    $.validator.addMethod("Password", function (value, element, params) {
        var arr = value.split(".");
        var reg = /^[a-zA-Z]\w{5,17}$/;
        if (reg.test(value)) {
            return true;
        }
        else
            return false;
    }, "密码必须以字母开头，长度在6-18之间，只能包含字符、数字和下划线");

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtOldPassword: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPassword: {
                Password: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: true,
                Password: true,
                equalTo: "#ContentPlaceHolder_Content_txtPassword"
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtOldPassword: {
                required: "请输入旧密码！"
            },
            ctl00$ContentPlaceHolder_Content$txtPassword: {
                required: "请输入新密码！"
            },
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: "请输入确认密码！",
                equalTo: "请保证两次密码一致!"
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

        var customerId = parseInt($("#" + hidCustomerId).val());
        var oldPassword = $("input[id*='txtOldPassword']").val();
        var newPassword = $("input[id*='txtPassword']").val();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/ChangePassword?customerId=" + customerId + "&password=" + oldPassword + "&newPassword=" + newPassword + "&token=" + _Token,
            type: "POST",
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





