
$(document).ready(function () {
    Init();

    $("button[id*='btn_submit']").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;

        }
        var objThirdOrder = $("input[id*='txtThirdOrder']");
        var objtxtHandleComment = $("textarea[id*='txtHandleComment']");
        if (objThirdOrder.val() == "") {
            bootbox.dialog({
                message: "请输入转账订单号！",
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
        else {
            setButton(0);
            var PayoutHistoryVO = GetCustomerPayOutHistory();
            PayoutHistoryVO.PayOutStatus = 1;

            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/HandleCustomerPayOut?token=" + _Token,
                type: "POST",
                data: PayoutHistoryVO,
                success: function (data) {
                    if (data.Flag == 1) {
                        setButton();
                        bootbox.dialog({
                            message: "处理信息保存成功。",
                            buttons:
                            {
                                "Confirm":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
                                     
                                        window.location.href = "CustomerPayOutBrowse.aspx";
                                    }
                                }
                            }
                        });
                    } else {
                        setButton(1);
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
                    setButton(1);
                    alert(data);
                }
            });
        }

    });
    $("button[id*='btn_save']").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;

        }
      
        var objtxtHandleComment = $("textarea[id*='txtHandleComment']");
        if (objtxtHandleComment.val() == "") {
            bootbox.dialog({
                message: "请输入处理情况说明！",
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
        else {
            setButton(0);
            var PayoutHistoryVO = GetCustomerPayOutHistory();
            PayoutHistoryVO.PayOutStatus = -2;

            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/HandleCustomerPayOut?token=" + _Token,
                type: "POST",
                data: PayoutHistoryVO,
                success: function (data) {
                    if (data.Flag == 1) {
                      
                        bootbox.dialog({
                            message: "处理信息保存成功。",
                            buttons:
                            {
                                "Confirm":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
                                        window.location.href = "CustomerPayOutBrowse.aspx";
                                    }
                                }
                            }
                        });
                    } else {
                        setButton(1);
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
                    setButton(1);
                    alert(data);
                }
            });
        }

    });

});



function Init() {
    var PayOutHistoryId = parseInt($("#" + hidPayOutHistoryId).val());
    if (PayOutHistoryId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetCustomerPayOutHistory?PayOutHistoryId=" + PayOutHistoryId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var CustomerPayOutHistoryViewVO = data.Result;
                    SetCustomerPayOutHistory(CustomerPayOutHistoryViewVO);

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
                load_hide();
            },
            error: function (data) {
                alert(data);
                load_hide();
            }
        });
    }
}
function GetCustomerPayOutHistory() {
    var PayOutHistoryVO = new Object();
    var objtxtThirdOrder = $("input[id*='txtThirdOrder']");
    var objtxtHandleComment = $("textarea[id*='txtHandleComment']");
 
    PayOutHistoryVO.PayOutHistoryId = parseInt($("#" + hidPayOutHistoryId).val());
    PayOutHistoryVO.ThirdOrder = objtxtThirdOrder.val();
    PayOutHistoryVO.HandleComment = objtxtHandleComment.val();
    PayOutHistoryVO.PayOutStatus = 1;
    return PayOutHistoryVO;
}

function SetCustomerPayOutHistory(CustomerPayOutHistoryViewVO) {
    console.log(CustomerPayOutHistoryViewVO)

    var objCustomerCode = $("input[id*='txtCustomerCode']");
    var objCustomerAccount = $("input[id*='txtCustomerAccount']");
    var objPhone = $("input[id*='txtPhone']");
    var objCustomerName = $("input[id*='txtCustomerName']");
    var objtxtAmount = $("input[id*='txtAmount']");
    var objtxtBankName = $("input[id*='txtBankName']");
    var objtxtBankAccount = $("input[id*='txtBankAccount']");
    var objtxtSubBranch = $("input[id*='txtSubBranch']");
    var objtxtAccountName = $("input[id*='txtAccountName']");
    var objtxtThirdOrder = $("input[id*='txtThirdOrder']");
    var objtxtHandleComment = $("textarea[id*='txtHandleComment']");

    objCustomerCode.val(CustomerPayOutHistoryViewVO.CustomerCode);
    objCustomerAccount.val(CustomerPayOutHistoryViewVO.CustomerAccount);
    objPhone.val(CustomerPayOutHistoryViewVO.Phone);
    objCustomerName.val(CustomerPayOutHistoryViewVO.CustomerName);
    objtxtAmount.val(CustomerPayOutHistoryViewVO.Cost);
    objtxtBankName.val(CustomerPayOutHistoryViewVO.BankName);
    objtxtBankAccount.val(CustomerPayOutHistoryViewVO.BankAccount);
    objtxtSubBranch.val(CustomerPayOutHistoryViewVO.SubBranch);
    objtxtAccountName.val(CustomerPayOutHistoryViewVO.AccountName);
    objtxtThirdOrder.val(CustomerPayOutHistoryViewVO.ThirdOrder);
    objtxtHandleComment.val(CustomerPayOutHistoryViewVO.HandleComment);

    if (CustomerPayOutHistoryViewVO.PayOutStatus!= 0)
    {
        setButton(0);
    }

}

function setButton(show)
{
    if (show == 0) {
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_save']").hide();
    } else {
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_save']").show();
    }
}
