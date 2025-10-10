$(function () {   

    if (!isAgency) {
        load_hide();
        bootbox.dialog({
            message: "还未通过认证，不能执行该操作!",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "../CustomerManagement/AgencyCreateEdit.aspx";
                    }
                }
            }
        });
        return false;
    }
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyExperienceList";
    grid.jqGrid.PagerID = "AgencyExperienceListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyExperienceList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "AgencyExperienceId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyExperienceId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditAgencyExperience(this);"></img>';
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyExperienceOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyExperienceId");
			
    grid.jqGrid.AddColumn("Title", "合同名称", true, null, 100);
    grid.jqGrid.AddColumn("ProjectDate", "成交时间（合同签订时间）", true, null, 100);
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请";
            else if (obj == '1')
                return "通过";
            else if (obj == '2')
                return "驳回";
            else
                return "保存";
        }, false);
    grid.jqGrid.CreateTable();   
}

function NewAgencyExperience() {
    window.location.href = "AgencyExperienceCreateEdit.aspx";
    return false;
}
function EditAgencyExperience(agencyExperienceObj) {
    window.location.href = "AgencyExperienceCreateEdit.aspx?agencyExperienceId=" + $(agencyExperienceObj).prev().val();
    return false;
}

function DeleteAgencyExperienceOne(agencyExperienceObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var agencyExperienceId = $(agencyExperienceObj).prev().prev().val();
                    DeleteAction(agencyExperienceId);
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

function DeleteAgencyExperience() {
    var id = $("#AgencyExperienceList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        DeleteAction(idString);
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

function DeleteAction(agencyExperienceId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteAgencyExperience?agencyExperienceId=" + agencyExperienceId + "&token=" + _Token,
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
                                window.location.href = "AgencyExperienceBrowse.aspx";
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


//function OnSearch() {
//    grid.jqGrid.InitSearchParams();    

//    var objServicesName = $("input[id*='txtServicesName']");
//    grid.jqGrid.AddSearchParams("Title", "LIKE", objServicesName.val());

//    grid.jqGrid.Search();
//    return false;
//}

