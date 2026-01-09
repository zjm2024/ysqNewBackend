
$(document).ready(function () {

    //$("form[id*='aspnetForm']").validate({
    //    errorElement: 'div',
    //    errorClass: 'help-block',
    //    focusInvalid: true,
    //    rules: {
    //        ctl00$ContentPlaceHolder_Content$txtAmount: {
    //            required: true
    //        }
    //    },
    //    messages: {
    //        ctl00$ContentPlaceHolder_Content$txtCustomerAccount: {
    //            required: "请输入充值金额！"
    //        }
    //    },
    //    highlight: function (e) {
    //        $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
    //    },

    //    success: function (e) {
    //        $(e).closest('.form-group').removeClass('has-error');
    //        $(e).remove();
    //    }
    //});

    $("#btn_save").click(function () {

        //if (!$("form[id*='aspnetForm']").valid()) {
        //    return false;
        //}
        var objWx = $("input[id*='radWx']:checked");
        var objtxtAmount = $("input[id*='txtAmount']");
        var out_trade_no = "201709281415001008";
        var subject = "众销乐-资源共享众包销售平台测试支付"
        var total_amout = objtxtAmount.val();
        var body = "服务描述"
        if (total_amout == "")
        {
            bootbox.dialog({
                message: "请输入充值金额！",
                buttons:
                {
                    "Confirm":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            //return false;
                        }
                    }
                }
            });
            return false;
        }

        if (objWx.length > 0) {
            window.location.href = 'WXPay.aspx?total_fee=' + total_amout;

        }
        else {
            $.ajax({
                url: _RootPath + "SPWebAPI/Project/AliPagePay?out_trade_no=" + out_trade_no + "&subject=" + subject + "&total_amout=" + total_amout + "&body=" + body + "&token=" + _Token,
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
                                        // window.location.href = "MessageBrowse.aspx";
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
                    //load_hide();
                },
                error: function (data) {
                    alert(data);
                    //load_hide();
                }
            });
        }

    })

});