$(document).ready(function () {
    onPageInit();
    onPageInit2();
    Init();
    $("#btn_save").click(function () {
        var Cost = $("input[id*='txtCost'").val();
        var Purpose = $("input[id*='txtPurpose'").val();
        if (!Cost) {
            bootbox.dialog({
                message: "请填写乐币数额",
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
        if (!Purpose) {
            bootbox.dialog({
                message: "请填写奖励说明",
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
        bootbox.dialog({
            message: "确认为<font style='color:#cc2a2a'>“" + $("input[id*='txtCustomerName'").val() + "”</font>发放<font style='color:#cc2a2a'>" + Cost + "</font>乐币",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        ZXBAddrequire(Cost, Purpose);
                    }
                },
                "Cancel":
                {
                    "label": "取消",
                    "className": "btn-sm",
                    "callback": function () {
                    }
                }
            }
        });
    });
    $("#btn_cancel").click(function () {
        var customerId = parseInt($("#" + hidCustomerId).val());
        window.location.href = "CustomerCreateEdit.aspx?CustomerId=" + customerId;
        return false;
    });
});
function ZXBAddrequire(Cost, Purpose) {
    var customerId = parseInt($("#" + hidCustomerId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/admZXBAddrequire?customerId=" + customerId + "&Cost=" + Cost + "&Purpose=" + Purpose + "&token=" + _Token,
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
                                window.location.href = "ManualSetZXB.aspx?CustomerId=" + customerId;
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
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function SetCustomer(customerVO) {
    var objCustomerAccount = $("input[id*='txtCustomerAccount']");
    var objCustomerName = $("input[id*='txtCustomerName']");
    objCustomerAccount.val(customerVO.CustomerAccount);
    objCustomerName.val(customerVO.CustomerName);
}
function Init() {
    var customerId = parseInt($("#" + hidCustomerId).val());
    if (customerId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + customerId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var customerVO = data.Result;
                    SetCustomer(customerVO);

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
var grid = new JGrid(); 
function onPageInit() {
    var customerId = parseInt($("#" + hidCustomerId).val());
    grid.jqGrid.ID = "zxbRequireList";
    grid.jqGrid.PagerID = "zxbRequireListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=zxbRequireList&CustomerId=" + customerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "Status";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = false;

    grid.jqGrid.AddColumn("Purpose", "奖励类型", true, null, 50);
    grid.jqGrid.AddColumn("Cost", "乐币", true, null, 20);
    grid.jqGrid.AddColumn("Date", "发放时间", true, null, 50);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 20,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "未领取";
            else if (obj == '1')
                return "已领取";
        }, false);
    grid.jqGrid.AddColumn("ZXBrequireId", "操作", true, null, 20,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input class="wtbtn savebtn lg_centel" type="button" id="btnRecharge" onclick=" return Recharge(' + obj + ');" value="撤销奖励" title="撤销奖励">';
            return result;
        }, false);
    grid.jqGrid.CreateTable();
}
var grid2 = new JGrid();
function onPageInit2() {
    var customerId = parseInt($("#" + hidCustomerId).val());
    grid2.jqGrid.ID = "InvitationCustomerList";
    grid2.jqGrid.PagerID = "InvitationCustomerListDiv";
    grid2.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid2.jqGrid.Params = "table=InvitationCustomerList&CustomerId=" + customerId;
    grid2.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid2.jqGrid.DefaultSortCol = "CreatedAt";
    grid2.jqGrid.DefaultSort = "desc";
    grid2.jqGrid.Multiselect = false;

    grid2.jqGrid.AddColumn("CustomerAccount", "会员账号", true, null, 50);
    grid2.jqGrid.AddColumn("Phone", "联系电话", true, null, 50);
    grid2.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    grid2.jqGrid.AddColumn("Status", "会员类型", true, null, 50,
        function (obj, options, rowObject) {
            if (rowObject.AgencyId > 0 && rowObject.BusinessId > 0) {
                return "雇主/销售";
            } else if (rowObject.AgencyId > 0 && rowObject.BusinessId == 0) {
                return "销售";
            }
            else if (rowObject.AgencyId == 0 && rowObject.BusinessId > 0) {
                return "雇主";
            }
            else {
                return "普通会员";
            }
        }, false);
    grid2.jqGrid.AddColumn("CreatedAt", "注册时间", true, null, 50);
    grid2.jqGrid.AddColumn("StatusName", "状态", true, null, 50);
    grid2.jqGrid.CreateTable();
}
function Recharge(ZXBrequireId) {
    var customerId = parseInt($("#" + hidCustomerId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DelZXBrequire?ZXBrequireId=" + ZXBrequireId + "&token=" + _Token,
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
                                window.location.href = "ManualSetZXB.aspx?CustomerId=" + customerId;
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