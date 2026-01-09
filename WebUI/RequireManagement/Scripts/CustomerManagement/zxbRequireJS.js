$(function () {
    onPageInit();
    Init();
});
function Init() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetZXBBalance?customerId=" + _CustomerId + "&token=" + _Token,
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
var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "zxbRequireList";
    grid.jqGrid.PagerID = "zxbRequireListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=zxbRequireList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "Status";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = false;
			
    grid.jqGrid.AddColumn("Purpose", "奖励类型", true, null, 80);
    grid.jqGrid.AddColumn("Cost", "乐币", true, null, 50);
    grid.jqGrid.AddColumn("Date", "发放时间", true, null, 50);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "未领取";
            else if (obj == '1')
                return "已领取";
        }, false);
    grid.jqGrid.AddColumn("ZXBrequireId", "操作", true, null, 60,
        function (obj, options, rowObject) {
            var result = '';
            if (rowObject.Status=='0')
            { result += '<input class="wtbtn savebtn lg_centel" type="button" id="btnRecharge" onclick=" return Recharge(' + obj + ');" value="领取奖励" title="领取奖励">'; }
            else if (rowObject.Status == '1') {
                result += '<input class="wtbtn cancelbtn lg_centel" type="button" id="btnRecharge" value="已领取" title="已领取">';
            }
            return result;
        }, false);
    grid.jqGrid.CreateTable();   
}
function Recharge2() {
    var InvitationTel = $("input[id*='InvitationTel'").val();
    if (!InvitationTel) {
        bootbox.dialog({
            message:"请填写邀请人手机号码",
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
        url: _RootPath + "SPWebAPI/Customer/SetInvitationTel?InvitationTel=" + InvitationTel + "&CustomerId=" + _CustomerId + "&token=" + _Token,
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
                                window.location.href = "zxbRequire.aspx";
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
    return false;
}
function Recharge(ZXBrequireId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/ReceiveZXBrequire?ZXBrequireId=" + ZXBrequireId + "&token=" + _Token,
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
                                window.location.href = "zxbRequire.aspx";
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
function Recharge3() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/ReceiveZXBrequire?token=" + _Token,
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
                                window.location.href = "zxbRequire.aspx";
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
