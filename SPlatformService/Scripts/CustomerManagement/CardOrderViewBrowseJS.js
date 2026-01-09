$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardOrderList";
    grid.jqGrid.PagerID = "CardOrderListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardOrderList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CardOrderID");
    grid.jqGrid.AddColumn("HeaderLogo", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("CustomerName", "名称", true, null, 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("OrderNO", "订单号", true, null, 50);
    grid.jqGrid.AddColumn("Cost", "订单金额", true, "center",30,
        function (obj, options, rowObject) {
            return "<div style='color:#882323'>" + obj + "</div>";
    }, false);
    grid.jqGrid.AddColumn("payAt", "付款时间", true, "center", 50);
    grid.jqGrid.AddColumn("Type", "VIP类型", true, "center", 30,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "月VIP";
            else if (obj == '2')
                return "五星会员";
            else if (obj == '3')
                return "永久五星会员";
            else if (obj == '5')
                return "季度五星会员";
            else if (obj == '6')
                return "合伙人";
            else if (obj == '7')
                return "分公司";
            else if (obj == '8')
                return "三星会员/月";
            else if (obj == '9')
                return "三星会员/年";
            else if (obj == '10')
                return "四星会员";
        }, false);
    grid.jqGrid.AddColumn("isVip", "当前vip状态", true, "center", 30,
        function (obj, options, rowObject) {
            if (obj + "" == 'False') {
                return "VIP已过期";
            } else {
                let today = new Date();
                let exdate = new Date(rowObject.ExpirationAt.replace("-", "/"));
                if (exdate > today) {
                    return "<div style='color:#f00'>VIP会员</div>";
                } else {
                    return "VIP已过期";
                }

            }
        }, false);
    grid.jqGrid.AddColumn("ExpirationAt", "vip到期时间", true, "center", 50);
    grid.jqGrid.AddColumn("Status", "付款状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "<div style='color:#882323'>已付款</div>";
            else if (obj == '0')
                return "未付款" + "<div style='color:#197e9c;cursor:pointer;' onclick=\"SetCardOrder('" + rowObject.CardOrderID + "')\">设为已付款</div>";
        }, false);
    grid.jqGrid.AddColumn("OneRebateCustomerName", "一级邀请人", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.OneRebateCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div><div onclick='Recharge(" + rowObject.CardOrderID + ",1)' style='margin-left:10px;color:#4f8823;cursor:pointer;'>更改邀请人</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("OneRebateCost", "一级返佣", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div style='color:#882323'>" + obj + "</div>";
        }, false);
    grid.jqGrid.AddColumn("TwoRebateCustomerName", "二级邀请人", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.TwoRebateCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div><div onclick='Recharge(" + rowObject.CardOrderID + ",2)' style='margin-left:10px;color:#4f8823;cursor:pointer;'>更改邀请人</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("TwoRebateCost", "二级返佣", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div style='color:#882323'>" + obj + "</div>";
        }, false);
    grid.jqGrid.AddColumn("AgentCustomerName", "代理商", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.AgentCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("AgentCost", "代理商佣金", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div style='color:#882323'>" + obj + "</div>";
        }, false);
    grid.jqGrid.CreateTable();

    var NoticeID = parseInt($("#" + hidNoticeID).val());
    
    if (NoticeID > 0) {
        grid.jqGrid.InitSearchParams();
        grid.jqGrid.AddSearchParams("OneRebateCost", ">","0");
        grid.jqGrid.AddSearchParams("OneRebateStatus", "=", "0");
        grid.jqGrid.Search();
        console.log(NoticeID)
    }
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}

function SetCardOrder(CardOrderID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/SetCardOrder?CardOrderID=" + CardOrderID + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                window.location.href = window.location.href;
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

function Recharge(CardOrderID, Type) {
    var title = "更改邀请人";
    bootbox.dialog({
        message:
            '<style type="text\/css">.modal-dialog{ top: 10%;}<\/style>' +
            '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
            '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
            '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
            '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseVipRebate.aspx" height="200px" width="100%" frameborder="0"><\/iframe>',

        title: title,
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCustomerId']")
                    var CustomerId = parseInt(objCost.val());
                    if (CustomerId < 0) {
                        bootbox.dialog({
                            message: "请输入正确的数值",
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
                    ChangeRebateCustomerId(CardOrderID, CustomerId, Type)
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


function ChangeRebateCustomerId(CardOrderID, CustomerId, Type) {
    console.log(CardOrderID, CustomerId, Type);
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/ChangeRebateCustomerId?CardOrderID=" + CardOrderID + "&CustomerId=" + CustomerId + "&Type=" + Type + "&token=" + _Token,
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