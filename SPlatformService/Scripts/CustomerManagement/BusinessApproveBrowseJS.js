$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "BusinessApproveList";
    grid.jqGrid.PagerID = "BusinessApproveListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessApproveList";
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
    window.location.href = "BusinessCreateEdit.aspx?IsApprove=true&BusinessId=" + $(businessObj).prev().val();
    return false;
}

function UpdateBusinessStatus(status) {
    var id = $("#BusinessApproveList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认通过认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateBusinessStatusAction(idString, status,"");
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

function UpdateBusinessStatusAction(businessId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateBusinessStatus?businessId=" + businessId + "&approveComment=" + approveComment + "&status=" + status + "&type=B&token=" + _Token,
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
                                window.location.href = "BusinessApproveBrowse.aspx";
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
    grid.jqGrid.AddSearchParams("CompanyName", "LIKE", objBusinessName.val());

    grid.jqGrid.Search();
    return false;
}

