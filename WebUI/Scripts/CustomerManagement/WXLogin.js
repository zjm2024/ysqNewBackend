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
            txtPhone: {
                required: true,
                isTel: true
            },            
            txtPassword: {
                required: true
            }
        },
        messages: {
            txtPhone: {
                required: "请输入手机号码！",
                isTel: "请输入正确格式的电话"
            },
            txtPassword: {
                required: "请输入密码！"
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

    $("#btnRegister").click(function () {        
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        
        return true;
    });
});