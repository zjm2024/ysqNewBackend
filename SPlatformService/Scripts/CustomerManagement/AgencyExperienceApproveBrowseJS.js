$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyExperienceApproveList";
    grid.jqGrid.PagerID = "AgencyExperienceApproveListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyExperienceApproveList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "AgencyExperienceId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyExperienceId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditAgencyExperience(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyExperienceId");
		
    grid.jqGrid.AddColumn("Title", "案例名称", true, null, 50);
    grid.jqGrid.AddColumn("ProjectDate", "项目时间", true, null, 50);
	grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 50);				
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请";
            else if (obj == '1')
                return "通过";
            else if (obj == '2')
                return "驳回";
            else
                return "驳回";
        }, false);
    grid.jqGrid.CreateTable();   
}


function EditAgencyExperience(agencyExperienceObj) {
    window.location.href = "AgencyExperienceEdit.aspx?AgencyExperienceId=" + $(agencyExperienceObj).prev().val();
    return false;
}


function UpdateAgencyExperienceStatus(status) {
    var id = $("#AgencyExperienceApproveList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认通过审核?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyExperienceStatusAction(idString, status, "approve by platform user");
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

function UpdateAgencyExperienceStatusAction(agencyExperienceId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateAgencyExperienceStatus?agencyExperienceId=" + agencyExperienceId + "&approveComment=" + approveComment + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "AgencyExperienceApproveBrowse.aspx";
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

