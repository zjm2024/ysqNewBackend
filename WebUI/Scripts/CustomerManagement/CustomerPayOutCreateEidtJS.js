$(function () {
    Init();
    onCustomerPayOutListTableInit();
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
            ctl00$ContentPlaceHolder_Content$txtBankName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtAmount: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtAmount: {
                number: true
            },
            
            ctl00$ContentPlaceHolder_Content$txtBankAccount: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtSubBranch: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtAccountName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtBankAccount: {
                number: true
            },
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtBankName: {
                required: "请输入开户银行！"
            },
            ctl00$ContentPlaceHolder_Content$txtAmount: {
                required: "请输入提现金额！"
            },
            ctl00$ContentPlaceHolder_Content$txtAmount: {
                number: "请输入正确的提现金额！"
            },
            ctl00$ContentPlaceHolder_Content$txtBankAccount: {
                required: "请输入银行账户！"
            },
            ctl00$ContentPlaceHolder_Content$txtSubBranch: {
                required: "开户银行名称！"
            },
            ctl00$ContentPlaceHolder_Content$txtAccountName: {
                required: "账户名！"
            },
            ctl00$ContentPlaceHolder_Content$txtBankAccount: {
                number: "请输入银行账户！"
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


    $("select[id$='drpBankList']").change(function () {
        //更新Child
        var drp = $("select[id*='drpBankList']");
        var _BankAccountId = drp.val()
        var objtxtBankName = $("input[id*='txtBankName']");
        var objtxtBankAccount = $("input[id*='txtBankAccount']");
        var objtxtSubBranch = $("input[id*='txtSubBranch']");
        var objtxtAccountName = $("input[id*='txtAccountName']");
        if (_BankAccountId == "-1") {
            objtxtBankName.val();
            objtxtBankAccount.val();
        } else {
            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/GetBankInfoByBankAccountId?BankAccountId=" + _BankAccountId + "&token=" + _Token,
                type: "GET",
                data: null,
                success: function (data) {
                    if (data.Flag == 1) {
                        var BankAccount = data.Result;
                        objtxtBankName.val(BankAccount.BankName);
                        objtxtBankAccount.val(BankAccount.BankAccount);
                        objtxtSubBranch.val(BankAccount.SubBranch);
                        objtxtAccountName.val(BankAccount.AccountName);
                    }
                },
                error: function (data) {
                    alert(data);
                }
            });
        }
    });


    $("button[id*='btn_submit']").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;

        }
        var objCost = $("input[id*='txtAmount']");
        var objtxtBalance = $("input[id*='txtBalance']");
        if (parseFloat(objtxtBalance.val()) < parseFloat(objCost.val())) {
            bootbox.dialog({
                message: "提现金额不能大于余额！",
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
        } else if (parseFloat(objCost.val()) <= 1) {
            bootbox.dialog({
                message: "请输入提现金额",
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
            var objBankAccountId = $("select[id*='drpBankList']");
            var PayoutHistoryVO = getPayoutHistoryVO();

            if (objBankAccountId.val() == "-1") {
                var bankAccountVO = getbankAccountVO();
                //save account
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/AddBankAccount?token=" + _Token,
                    type: "POST",
                    data: bankAccountVO,
                    success: function (data) {
                        if (data.Flag == 1) {
                            objBankAccountId.val(data.Result);
                            PayoutHistoryVO.BankAccountId = data.Result
                            PayoutHistoryVO.PayOutStatus = 0;
                            SaveAndSubmitPayoutHistoryVO(PayoutHistoryVO);
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

            }
            else {
                PayoutHistoryVO.PayOutStatus = 0;
                SaveAndSubmitPayoutHistoryVO(PayoutHistoryVO);
            }
        }
    });
    $("button[id*='btn_save']").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var objCost = $("input[id*='txtAmount']");
        var objtxtBalance = $("input[id*='txtBalance']");
        if ( parseFloat(objtxtBalance.val()) < parseFloat(objCost.val())) {
            bootbox.dialog({
                message: "提现金额不能大于余额！",
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
        } else if (parseFloat(objCost.val()) == "NaN"||parseFloat(objCost.val()) <= 1) {
            bootbox.dialog({
                message: "请输入提现金额",
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
            var objBankAccountId = $("select[id*='drpBankList']");
            var PayoutHistoryVO = getPayoutHistoryVO();

            if (objBankAccountId.val() == "-1") {
                var bankAccountVO = getbankAccountVO();
                //save account
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/AddBankAccount?token=" + _Token,
                    type: "POST",
                    data: bankAccountVO,
                    success: function (data) {
                        if (data.Flag == 1) {
                            objBankAccountId.val(data.Result);
                            PayoutHistoryVO.BankAccountId = data.Result
                            PayoutHistoryVO.PayOutStatus = -1;
                            SaveAndSubmitPayoutHistoryVO(PayoutHistoryVO);
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

            }
            else {
                PayoutHistoryVO.PayOutStatus = -1;
                SaveAndSubmitPayoutHistoryVO(PayoutHistoryVO);
            }
        }
    });
});
function Init() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetCustomerBalance?customerId=" + _CustomerId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var balanceVO = data.Result;
                SetBalance(balanceVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });

}

function SetBalance(balanceVO) {
    var objBalance = $("input[id*='txtBalance']");
    if (parseFloat(balanceVO.Balance) > 0)
        objBalance.val(balanceVO.Balance);
    else
        objBalance.val(0);

}


function SaveAndSubmitPayoutHistoryVO(data) {
    var message = "保存申请";
    if (data.PayOutStatus == 0) {
        message = "提交申请";
    }
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/AddPayoutHistory?token=" + _Token,
        type: "POST",
        data: data,
        success: function (data) {
            if (data.Flag == 1) {

                bootbox.dialog({
                    message: message + "成功，将进行跳转",
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                window.location.href = "CustomerPayOutCreateEidt.aspx";
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
}




function getPayoutHistoryVO() {
    var PayoutHistoryVO = new Object();

    var objBankAccountId = $("select[id*='drpBankList']");
    var objCost = $("input[id*='txtAmount']");


    PayoutHistoryVO.BankAccountId = objBankAccountId.val();
    PayoutHistoryVO.Cost = objCost.val();
    PayoutHistoryVO.CustomerId = _CustomerId;
    PayoutHistoryVO.PayOutOrder = "";
    PayoutHistoryVO.PayOutStatus = -1;
    PayoutHistoryVO.ThirdOrder = "";
    return PayoutHistoryVO;

}


function getbankAccountVO() {
    var BankAccountVO = new Object();
    var objBankAccountId = $("select[id*='drpBankList']");
    var objBankName = $("input[id*='txtBankName']");
    var objBankAccount = $("input[id*='txtBankAccount']");
    var objtxtSubBranch = $("input[id*='txtSubBranch']");
    var objtxtAccountName = $("input[id*='txtAccountName']");
    BankAccountVO.SubBranch = objtxtSubBranch.val();
    BankAccountVO.AccountName = objtxtAccountName.val();
    BankAccountVO.BankAccountId = objBankAccountId.val();
    BankAccountVO.BankName = objBankName.val();
    BankAccountVO.CustomerId = _CustomerId;
    BankAccountVO.BankAccount = objBankAccount.val();

    return BankAccountVO;
}


function onCustomerPayOutListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerPayOutList";
    grid.jqGrid.PagerID = "CustomerPayOutListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerPayOutPendingList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "PayOutHistoryId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("Cost", "提现金额", true, null, 350);
    grid.jqGrid.AddColumn("PayOutDate", "提现日期", true, null, 350);
    grid.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 50,
    function (obj, options, rowObject) {
        if (obj == '0')
            return "申请提现";
        else if (obj == '-1')
            return "未提交申请";
        else if (obj == '-2')
            return "提现失败";
        else if (obj == '1')
            return "提现成功";
    }, false);
    grid.jqGrid.AddColumn("HandleComment", "备注", true, null, 150);
    grid.jqGrid.CreateTable();

}


