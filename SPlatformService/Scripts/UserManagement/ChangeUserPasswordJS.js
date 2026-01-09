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

    $('#form1').validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {            
            txtPassword: {
                Password: true,
                required: true
            },
            txtPasswordConfirm: {
                required: true,
                Password: true,
                equalTo: "#txtPassword"
            }
        },
        messages: {            
            txtPassword: {
                required: "请输入新密码！"
            },
            txtPasswordConfirm: {
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

    $("#btnSave").click(function () {
        if (!$('#form1').valid()) {
            return false;
        }
    });
});