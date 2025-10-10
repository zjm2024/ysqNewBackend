$(document).ready(function () {
    //$("body").keydown(function (event) {
    //    if (event.keyCode == "13") {//keyCode=13是回车键
    //        $("input[id*='btnLogin']").click();
    //    }
    //});
    $("input[id*='txtPassword']").keydown(function (event) {
        if (event.keyCode == "13") {//keyCode=13是回车键
            $("input[id*='btnLogin']").click();
            return false;
        }
    });
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            txtLoginName: {
                required: true,
            },            
            txtPassword: {
                required: true
            }
        },
        messages: {
            txtLoginName: {
                required: "请输入账号！",
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
    $("#Eyes").click(function () {
        var objPassword = $("input[id*='txtPassword']");
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
    $("input[id*='btnLogin']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        
        //var userName = $("input[id*='txtLoginName']").val();
        //var pwd = $("input[id*='txtPassword']").val();
        //$.ajax({
        //    url: _RootPath + "SPWebAPI/Customer/ValidCustomerAccount?loginName=" + userName + "&password=" + pwd,
        //    type: "GET",
        //    data: null,
        //    async: false,
        //    success: function (data) {
        //        if (data.Flag == 1) {
        //            window.location.href = "Login.aspx?CustomerId=" + data.Result.Customer.CustomerId + "&Token=" + data.Result.Token;
        //        } else {
        //            $("span[id*='lblMessage']").html("账户或密码错误，请重新输入！");
        //        }
        //    },
        //    error: function (data) {
        //        alert(data);
        //    }
        //});

        return true;
    });
    //$("input[id*='btnLogin']").focus();
});