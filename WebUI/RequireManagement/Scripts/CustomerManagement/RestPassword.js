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

    $("input[id*='btnSendValidCode']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var objPhone = $("#txtPhone");
        var objHidCode = $("#hidCode");
        that = this;
        $.ajax({
            url: _RootPath + "SendPassCodeMsg.aspx?phone=" + objPhone.val(),
            type: "Get",
            data: null,
            success: function (res) {
                var data = JSON.parse(res);
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
                                    invokeSettime(that);
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

        return false;
    });

    $("input[id*='btnRest']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var objPhone = $("#txtPhone");
        var objValidCode = $("#txtValidCode");
        var objPassword = $("#txtPassword");
        var objHidCode = $("#hidCode");

        $.ajax({
            url: _RootPath + "ResetPassword.aspx?phone=" + objPhone.val() + "&newPassword=" + objPassword.val() + "&code=" + objValidCode.val(),
            type: "Get",
            data: null,
            success: function (res) {
                var data = JSON.parse(res);
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
                                    window.location.href = "Login.aspx";
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
                objHidCode.val("");
                alert(data);
            }
        });
        
        return false;
    });
});

function invokeSettime(obj) {
    var countdown = 60;
    settime(obj);
    function settime(obj) {
        if (countdown == 0) {
            $(obj).attr("disabled", false);
            $(obj).val("点击发送");
            countdown = 60;
            return;
        } else {
            $(obj).attr("disabled", true);
            $(obj).val("(" + countdown + ") s 重新发送");
            countdown--;
        }
        setTimeout(function () {
            settime(obj)
        }, 1000)
    }
}