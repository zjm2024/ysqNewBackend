var CustomerId;
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
            console.log(PayoutHistoryVO)
            $.ajax({
                url: _RootPath + "SPWebAPI/Card/BcHandleCustomerPayOut?token=" + _Token,
                type: "POST",
                data: PayoutHistoryVO,
                success: function (data) {
                    console.log(data)
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
                                        window.location.href = "CardPayOutBrowse.aspx";
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
                url: _RootPath + "SPWebAPI/Card/BcHandleCustomerPayOut?token=" + _Token,
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
                                        window.location.href = "CardPayOutBrowse.aspx";
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
                    console.log(data);
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
            url: _RootPath + "SPWebAPI/Card/BcAdminGetPayoutDetails?PayoutHistoryId=" + PayOutHistoryId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    var CustomerPayOutHistoryViewVO = data.Result;
                    SetCustomerPayOutHistory(CustomerPayOutHistoryViewVO);
                    CustomerId = CustomerPayOutHistoryViewVO.CustomerId;
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
                console.log(data);
                load_hide();
            }
        });
    }
}
function GetCustomerPayOutHistory() {
    var PayOutHistoryVO = new Object();
    var objtxtThirdOrder = $("input[id*='txtThirdOrder']");
    var objtxtHandleComment = $("textarea[id*='txtHandleComment']");
    var objType = $("input[id*='textType']");
 
    PayOutHistoryVO.PayOutHistoryId = parseInt($("#" + hidPayOutHistoryId).val());
    PayOutHistoryVO.ThirdOrder = objtxtThirdOrder.val();
    PayOutHistoryVO.HandleComment = objtxtHandleComment.val();
    PayOutHistoryVO.PayOutStatus = 1;
    PayOutHistoryVO.Type = objType.val();
    return PayOutHistoryVO;
}

function SetCustomerPayOutHistory(CustomerPayOutHistoryViewVO) {

    var objPayOutCost = $("input[id*='txtPayOutCost']");
    var objServiceCharge = $("input[id*='txtServiceCharge']");
    var objtxtAmount = $("input[id*='txtAmount']");
    var objtxtBankName = $("input[id*='txtBankName']");
    var objtxtBankAccount = $("input[id*='txtBankAccount']");
    var objtxtAccountName = $("input[id*='txtAccountName']");
    var objType = $("input[id*='textType']");
    var objOpenID = $("input[id*='txtOpenID']");
    
  
    objPayOutCost.val(CustomerPayOutHistoryViewVO.PayOutCost);
    objServiceCharge.val(CustomerPayOutHistoryViewVO.ServiceCharge);
    objtxtAmount.val(CustomerPayOutHistoryViewVO.Cost);
    objtxtBankName.val(CustomerPayOutHistoryViewVO.BankName);
    objtxtBankAccount.val(CustomerPayOutHistoryViewVO.BankAccount);
    objtxtAccountName.val(CustomerPayOutHistoryViewVO.AccountName);
    


   if (CustomerPayOutHistoryViewVO.Type == 1)
        objType.val("个人提现到微信零钱");
    else if (CustomerPayOutHistoryViewVO.Type == 2)
        objType.val("企业提现到银行账户");


    objOpenID.val(CustomerPayOutHistoryViewVO.OpenId);

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
