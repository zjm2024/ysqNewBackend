$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardAgentList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CardAgentID");
    grid.jqGrid.AddColumn("CardAgentID", "操作", false, "center", 30,
            function (obj, options, rowObject) {
                var result = '';
                result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
                return result;
            }, false);
    grid.jqGrid.AddColumn("AgentName", "代理名称", true, "center", 100);
    grid.jqGrid.AddColumn("Province", "代理区域", true, "center", 100, function (obj, options, rowObject) {
        return obj + "-" + rowObject.City;
    }, false);
    grid.jqGrid.AddColumn("UserCount", "区域会员", true, "center", 50, function (obj, options, rowObject) {
        return obj + "人";
    }, false);
    grid.jqGrid.AddColumn("VipCount", "区域Vip会员", true, "center", 50, function (obj, options, rowObject) {
        return obj + "人";
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "绑定会员", true, "center", 100, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span style='width: 40px;height: 40px;display: block;border-radius: 63px;float:left;background-repeat: no-repeat;background-size: cover;background-position: center center;background-image: url(" + rowObject.Headimg + ")'></span>";
            str += "<span style='margin-left:10px;line-height: 40px;display: block;float:left;'>" + rowObject.Name + "</span>";
            str += "<span onclick='delAgentBind(" + rowObject.CardAgentID + ")' style='margin-left:10px;line-height: 40px;display: block;float:left;color:#882323;cursor:pointer;'>解除绑定</span>";
            str += "</div>";
            return str;
        }else{
            var str = "<div>";
            str += "<span>未绑定会员</span>";

            str += "<span onclick='getCardAgentQR(" + rowObject.CardAgentID + ")' style='cursor:pointer; margin-left:10px;color:#882323;'>扫码绑定</span>";

            str += "</div>";
            return str;
        }
       
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "保证金", true, "center", 100, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.DepositCost + "元</span>";
            str += "<span onclick='Recharge(" + obj + ",true,\"" + rowObject.Name + "\")' style='margin-left:10px;color:#4f8823;cursor:pointer;'>充值</span>";
            str += "<span onclick='Recharge(" + obj + ",false,\"" + rowObject.Name + "\")' style='margin-left:10px;color:#882323;cursor:pointer;'>扣除</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }

    }, false);
    grid.jqGrid.AddColumn("CustomerId", "总佣金", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.TotalCommission + "元</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "应付佣金", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.PayableCommission + "元</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "已付佣金", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.PaidCommission + "元</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "实际佣金", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.AgentCost + "元</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "已结算佣金", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span >" + rowObject.SettlementCost + "元</span>";
            str += "</div>";
            return str;
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50, function (obj, options, rowObject) {
        if (obj > 0) {
            return "<div onclick='AgentFinance(" + obj + ")' style='cursor:pointer;color:#2a8de2;'>结算佣金</div>";
        } else {
            return "-";
        }
    }, false);
    grid.jqGrid.CreateTable();
}
function AgentFinance(obj) {
    window.location.href = "CardAgentFinanceBrowse.aspx?CustomerId=" + obj;
}
function Recharge(CustomerId, isAdd, Name) {
    var title = "";
    if (isAdd) {
        title = "充值保证金";
    } else {
        title = "扣除保证金";
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
                    if (isAdd) {
                        AgentRechargeDialog(cost, CustomerId, Name);
                    } else {
                        AgentRechargeDialog(-cost, CustomerId, Name);
                    }
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

function AgentRechargeDialog(cost, CustomerId, Name) {
    var message = "";
    if (cost > 0) {
        message = "确定要给<font style='color:#ff0000'>" + Name + "</font>充值<font style='color:#ff0000'>" + cost + "</font>元吗？"
    } else {
        message = "确定要把<font style='color:#ff0000'>" + Name + "</font>的保证金扣除<font style='color:#ff0000'>" + Math.abs(cost) + "</font>元吗？"
    }
    bootbox.dialog({
        message: message,
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    AgentRecharge(cost, CustomerId);
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
}
function AgentRecharge(cost, CustomerId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/AgentRecharge?cost=" + cost + "&CustomerId=" + CustomerId + "&token=" + _Token,
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

function delAgentBind(CardAgentID) {
    bootbox.dialog({
        message: "解除绑定之前产生的订单依旧属于该会员！重新绑定新的会员后，代理区域产生的新订单才属于重新绑定后的会员",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    delAgentBindUrl(CardAgentID);
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
}
function delAgentBindUrl(CardAgentID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/delAgentBind?CardAgentID=" + CardAgentID + "&token=" + _Token,
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

function EditSystemMessage(categoryObj) {
    window.location.href = "CardAgentCreateEdit.aspx?CardAgentID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardAgentCreateEdit.aspx";
    return false;
}
function getCardAgentQR(CardAgentID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/getCardAgentQR?CardAgentID=" + CardAgentID + "&token=" + _Token,
        type: "get",
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                window.open(data.Result);
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

