$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var CustomerId = $("input[id*='CustomerId']").val();
    grid.jqGrid.ID = "AgentFinanceList";
    grid.jqGrid.PagerID = "AgentFinanceListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgentFinanceList&CustomerId=" + CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("FinanceID");
    grid.jqGrid.AddColumn("MONTH", "月份", true, "center", 50);
    grid.jqGrid.AddColumn("TotalCommission", "总佣金", true, "center", 50);
    grid.jqGrid.AddColumn("PayableCommission", "应付佣金", true, "center", 50);
    grid.jqGrid.AddColumn("PaidCommission", "已付佣金", true, "center", 50);
    grid.jqGrid.AddColumn("AgentCost", "实际佣金", true, "center", 100);
    grid.jqGrid.AddColumn("isSettlement", "操作", true, "center", 100, function (obj, options, rowObject) {
        if (obj=="True") {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >已结算" + rowObject.SettlementCost + "元</span>";
            str += "<span onclick='Recharge("  + rowObject.FinanceID + ",false)' style='margin-left:10px;color:#4f8823;cursor:pointer;'>修改</span>";
            str += "</div>";
            return str;
        } else {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >未结算</span>";
            str += "<span onclick='Recharge(" + rowObject.FinanceID + ",true)' style='margin-left:10px;color:#882323;cursor:pointer;'>结算</span>";
            str += "</div>";
            return str;
        }

    }, false);
    grid.jqGrid.CreateTable();   
}
function Recharge(FinanceID, isAdd) {
    var title = "";
    if (isAdd) {
        title = "结算金额";
    } else {
        title = "修改金额";
    }
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{ top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseRecharge.aspx" height="200px" width="100%" frameborder="0"><\/iframe>',

        title: title,
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCost']")
                    var cost = parseFloat(objCost.val());
                    if (cost != cost || cost < 0) {
                        bootbox.dialog({
                            message: "请输入正确的金额",
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
                        return;
                    }
                    setFinance(cost,  FinanceID);
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
    return false;
}
function setFinance(cost, FinanceID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setFinance?cost=" + cost  + "&FinanceID=" + FinanceID + "&token=" + _Token,
        type: "get",
        success: function (data) {
            console.log(data);
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
                                window.location.href = window.location.href;
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
            console.log(data);
        }
    });
}
