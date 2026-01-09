$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyApproveList";
    grid.jqGrid.PagerID = "AgencyApproveListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyApproveList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditAgency(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyId");
		
    grid.jqGrid.AddColumn("CustomerCode", "编号", true, null, 50);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);
    grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 50);
    grid.jqGrid.AddColumn("IDCard", "身份证号", true, null, 50);
	grid.jqGrid.AddColumn("Status", "资料认证状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if (obj == '1')
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


function EditAgency(agencyObj) {
    window.location.href = "AgencyCreateEdit.aspx?IsApprove=true&AgencyId=" + $(agencyObj).prev().val();
    return false;
}


function UpdateAgencyStatus(status) {
    var id = $("#AgencyApproveList").jqGrid('getGridParam', 'selarrrow');
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
                        UpdateAgencyStatusAction(idString, status, "rejected by platform user");
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

function UpdateAgencyStatusAction(agencyId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateAgencyStatus?agencyId=" + agencyId + "&approveComment=" + approveComment + "&status=" + status + "&type=B&token=" + _Token,
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
                                window.location.href = "AgencyApproveBrowse.aspx";
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

    var objAgencyName = $("input[id*='txtAgencyName']");
    grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());

    grid.jqGrid.Search();
    return false;
}

