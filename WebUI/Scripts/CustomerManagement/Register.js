$(document).ready(function () {
    $("button[id*='btnSendValidCode']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var objPhone = $("#txtPhone");
        var objHidCode = $("#hidCode");
        $.ajax({
            url: _RootPath + "SPWebAPI/System/SendPassCodeMsg?phone=" + objPhone.val(),
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    objHidCode.val(data.Result);
                    new invokeSettime($("button[id*='btnSendValidCode']"));
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

        return false;
    });
    $("#Eyes").click(function () {
        var objPassword = $("#txtPassword");
        var objEyes = $("#Eyes");
        if (objPassword.attr("type") == "password") {
            objEyes.removeClass("ClosedEyes");
            objEyes.addClass("OpenEyes");
            objPassword.attr("type", "text");
        } else {
            objEyes.removeClass("OpenEyes");
            objEyes.addClass("ClosedEyes");
            objPassword.attr("type", "password");
        }
    });
    $("input[id*='btnRegister']").click(function () {        
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var objPhone = $("#txtPhone");
        var objValidCode = $("#txtValidCode");
        var objPassword = $("#txtPassword");
        var objHidCode = $("#hidCode");
        var txtPassword = objPassword.val();
        if (txtPassword.length<6) {
            bootbox.dialog({
                message: "密码至少要 6 个字符",
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
            return false;
        }

        if (txtPassword.length > 20) {
            bootbox.dialog({
                message: "密码最多只能填写 20 个字符",
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
            return false;
        }

        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/RegisterCustomer?account=" + objPhone.val() + "&password=" + objPassword.val(),
            type: "POST",
            data: null,
            success: function (data) {
                objHidCode.val("");
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
                                    window.location.href = "Login.aspx?account=" + objPhone.val() + "&password=" + objPassword.val() + "&identity=" + $("input[name='shengfen']:checked").val();
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