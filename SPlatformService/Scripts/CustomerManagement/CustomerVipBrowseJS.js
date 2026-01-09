$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CustomerList";
    grid.jqGrid.PagerID = "CustomerListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerVipList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CustomerId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditCustomer(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteCustomerOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("CustomerId");									
	grid.jqGrid.AddColumn("HeaderLogo", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
	grid.jqGrid.AddColumn("CustomerName", "会员名称", true, "center", 50,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
	grid.jqGrid.AddColumn("isVip", "VIP等级", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj + "" == 'False') {
                return "普通会员";
            } else {
                
                let today = new Date();
                let exdate = new Date(rowObject.ExpirationAt.replace("-", "/"));
                if (exdate > today) {
                    if (rowObject.VipLevel > 0) {
                        if(rowObject.VipLevel==1)
                            return "<div style='color:#f00'>VIP会员</div>";
                        if (rowObject.VipLevel == 2)
                            return "<div style='color:#f00'>合伙人</div>";
                        if (rowObject.VipLevel == 3)
                            return "<div style='color:#f00'>分公司</div>";
                    }
                    return "<div style='color:#f00'>VIP会员</div>";
                } else {
                    return "普通会员";
                }
            }
        }, false);
	grid.jqGrid.AddColumn("Agent", "是否代理商", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj + "" == "True") {
                return "<div style='color:#f00'>是</div>";
            } else {
                return "否";
            }
        }, false);
	grid.jqGrid.AddColumn("ExpirationAt", "VIP有效期", true, "center", 50);
	grid.jqGrid.AddColumn("ExchangeCodeCount", "库存(现存/总共)", true, "center", 50,
        function (obj, options, rowObject) {
            if (rowObject.ExchangeCodeCount > 0) {
                var str = "<div onclick='goCodeList(" + rowObject.CustomerId + ")' style='margin-left:10px;color:#197e9c;cursor:pointer;'>";
                str += rowObject.U_ExchangeCodeCount + "/" + rowObject.ExchangeCodeCount;
                str += "</div>";
                return str;
            } else {
                return rowObject.U_ExchangeCodeCount + "/" + rowObject.ExchangeCodeCount;
            }
            
        }, false);
	grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,function (obj, options, rowObject) {
	    if (obj > 0) {
	        var str = "<div onclick='Recharge(" + obj + ",\"" + rowObject.CustomerName + "\")' style='margin-left:10px;color:#4f8823;cursor:pointer;'>";
	        str += "发放兑换码";
	        str += "</div>";
	        return str;
	    } else {
	        return "-";
	    }

	}, false);
    grid.jqGrid.CreateTable();   
}
function goCodeList(CustomerId) {
    window.location.href = "CardExchangeCodeBrowse.aspx?CustomerId=" + CustomerId;
    return false;
}
function EditCustomer(customerObj) {
    window.location.href = "CustomerCreateEdit.aspx?CustomerId=" + $(customerObj).prev().val();
    return false;
}
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}

function Recharge(CustomerId, Name) {
    var title = "发放兑换码";
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
                "label": "发放7天试用兑换码",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCost']")
                    var Sum = parseInt(objCost.val());
                    if (Sum != Sum || Sum < 0) {
                        bootbox.dialog({
                            message: "请输入正确的数额",
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
                    AddExchangeCodeDialog(Sum, CustomerId, Name, 1);
                }
            },
            "Cancel":
            {
                "label": "发放一年VIP兑换码",
                "className": "btn-sm",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCost']")
                    var Sum = parseInt(objCost.val());
                    if (Sum != Sum || Sum < 0) {
                        bootbox.dialog({
                            message: "请输入正确的数额",
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
                    AddExchangeCodeDialog(Sum, CustomerId, Name, 0);
                }
            }
        }
    });
    return false;
}

function AddExchangeCodeDialog(Sum, CustomerId, Name, Type) {
    var message = "";
    if (Type == 1)
    {
        message = "确定要给<font style='color:#ff0000'>" + Name + "</font>发放<font style='color:#ff0000'>" + Sum + "</font>个兑换码(7天VIP)吗？"
    } else if (Type == 0) {
        message = "确定要给<font style='color:#ff0000'>" + Name + "</font>发放<font style='color:#ff0000'>" + Sum + "</font>个兑换码(一年VIP)吗？"
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
                    AddExchangeCode(Sum, CustomerId, Type);
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
function AddExchangeCode(Sum, CustomerId, Type) {
    console.log(Sum, CustomerId);
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/AddExchangeCode?Sum=" + Sum + "&CustomerId=" + CustomerId + "&Type=" + Type + "&token=" + _Token,
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
