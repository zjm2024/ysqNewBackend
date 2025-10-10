$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "BusinessList";
    grid.jqGrid.PagerID = "BusinessListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("BusinessId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditBusiness(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteBusinessOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("BusinessId");
								
    grid.jqGrid.AddColumn("CustomerCode", "编号", true, null, 50);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);
    grid.jqGrid.AddColumn("CompanyName", "雇主名称", true, null, 50);
    grid.jqGrid.AddColumn("BusinessLicense", "营业执照号", true, null, 50);
    grid.jqGrid.AddColumn("Status", "资料认证状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if(obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
        }, false);
    grid.jqGrid.AddColumn("RealNameStatus", "实名认证状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "未认证";
            else if (obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
            else if (obj == '3')
                return "审核中";
        }, false);
	grid.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 50);				
    grid.jqGrid.CreateTable();   
}


function EditBusiness(businessObj) {
    window.location.href = "BusinessCreateEdit.aspx?BusinessId=" + $(businessObj).prev().val();
    return false;
}

function UpdateBusinessStatus(status,type) {
    var id = $("#BusinessList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        if (status == 2) {
            bootbox.dialog({
                message: "是否确认取消认证?",
                buttons:
                {
                    "click":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            UpdateBusinessStatusAction2(idString, status, type, "rejected by platform user");
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
        } else {
            bootbox.dialog({
                message: "是否确认通过认证?",
                buttons:
                {
                    "click":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            UpdateBusinessStatusAction2(idString, status, type, "rejected by platform user");
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

function UpdateBusinessStatusAction(businessId, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/CancelBusinessRZ?businessId=" + businessId + "&approveComment=" + approveComment + "&token=" + _Token,
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
                                window.location.href = "BusinessBrowse.aspx";
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

function UpdateBusinessStatusAction2(businessId, status, type, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateBusinessStatus?businessId=" + businessId + "&approveComment=" + approveComment + "&status=" + status + "&type=" + type + "&token=" + _Token,
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
                                window.location.href = "BusinessBrowse.aspx";
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


function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objBusinessName = $("input[id*='txtBusinessName']");
    
    if (objBusinessName.val() != "") {

        //grid.jqGrid.AddSearchParams("CompanyName", "LIKE", objBusinessName.val());
        var filedArr = new Array();
        filedArr.push("CustomerName");
        filedArr.push("CustomerCode");
        filedArr.push("Phone");
        filedArr.push("CompanyName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objBusinessName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    var objRealNameStatus = $("select[id*='drpRealNameStatus']");
    if (objRealNameStatus.val() > -1)
        grid.jqGrid.AddSearchParams("RealNameStatus", "=", objRealNameStatus.val());

    grid.jqGrid.Search();
    return false;
}

