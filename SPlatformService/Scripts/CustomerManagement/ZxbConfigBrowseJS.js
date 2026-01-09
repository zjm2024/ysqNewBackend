$(function () {
    SetButton();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ZxbConfigList";
    grid.jqGrid.PagerID = "ZxbConfigListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ZxbConfigList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "ZxbConfigID";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ZxbConfigID", "操作", false, "center", 20,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditCustomer(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteCustomerOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ZxbConfigID");
    grid.jqGrid.AddColumn("Cost", "奖励金额（乐币）", true, null, 20);
    grid.jqGrid.AddColumn("Purpose", "奖励提示信息", true, null, 50);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 20, function (obj, options, rowObject) {
        if (obj == 0) {
            return "禁用";
        } else {
            return "启用";
        }
    }, false);
    grid.jqGrid.AddColumn("code", "调用编码", true, null, 20);
    grid.jqGrid.CreateTable();   
}


function EditCustomer(customerObj) {
    window.location.href = "ZxbConfigCreateEdit.aspx?ZxbConfigID=" + $(customerObj).prev().val();
    return false;
}

function UpdateCustomerStatus(status) {
    var id = $("#ZxbConfigList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }

    var msg = "";
    if (status == 0) {
        msg = "是否确认禁用账号?";
    } else {
        msg = "是否确认启用账号?";
    }

    if (id.length > 0) {
        bootbox.dialog({
            message: msg,
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateCustomerStatusAction(idString, status);
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
    else {
        bootbox.dialog({
            message: "请至少选择一条数据！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
    }
}

function UpdateCustomerStatusAction(ZxbConfigID, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateZxbConfigStatus?ZxbConfigID=" + ZxbConfigID + "&status=" + status + "&token=" + _Token,
        type: "Post",
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
                                window.location.href = "ZxbConfigBrowse.aspx";
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

function SetButton() {
        
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objCustomerName = $("input[id*='txtCustomerName']");
    if (objCustomerName.val() != "") {

        //grid.jqGrid.AddSearchParams("CustomerName", "LIKE", objCustomerName.val());
        var filedArr = new Array();
        filedArr.push("CustomerName");
        filedArr.push("CustomerCode");
        filedArr.push("CustomerAccount");
        filedArr.push("Phone");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objCustomerName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());


    
    var objCustomerType = $("select[id*='drpCustomerType']");
    var typeValue = objCustomerType.val() ;
    if (typeValue == 0) {        
        grid.jqGrid.AddSearchParams("AgencyId", "=", 0);
        grid.jqGrid.AddSearchParams("BusinessId", "=", 0);
    } else if (typeValue == 1) {
        grid.jqGrid.AddSearchParams("AgencyId", ">", 0);
    } else if (typeValue == 2) {
        grid.jqGrid.AddSearchParams("BusinessId", ">", 0);
    }


    grid.jqGrid.Search();
    return false;
}

