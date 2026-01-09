$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "RequirementList";
    grid.jqGrid.PagerID = "RequirementListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=RequirementList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteRequirementOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("RequirementId");
			
	grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 50);				
	grid.jqGrid.AddColumn("Title", "标题", true, null, 50);					
	grid.jqGrid.AddColumn("EffectiveEndDate", "有效期", true, null, 50);
	grid.jqGrid.AddColumn("CommissionType", "酬金类型", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "金额";
            else if (obj == '2')
                return "合同比例";            
        }, false);
	grid.jqGrid.AddColumn("Commission", "酬金", true, null, 50);				
	grid.jqGrid.AddColumn("TenderCount", "投标数目", true, null, 30);
	grid.jqGrid.AddColumn("ClientName", "目标客户名称", true, null, 50);
	grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);							
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "保存";
            else if (obj == '1')
                return "发布";
            else if (obj == '2')
                return "关闭";
            else if (obj == '3')
                return "暂停投标";
            else if (obj == '4')
                return "已创建项目";
            else if (obj == '5')
                return "审核中";
            else
                return "保存";
        }, false);
	grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 50);				
    grid.jqGrid.CreateTable();   
}


function EditRequirement(requirementObj) {
    window.location.href = "RequirementCreateEdit.aspx?RequirementId=" + $(requirementObj).prev().val();
    return false;
}

function NewRequirement() {
    window.location.href = "RequirementCreateEdit.aspx";
    return false;
}

function UpdateRequireStatus(status) {
    var id = $("#RequirementList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        if (status == 0) {
            bootbox.dialog({
                message: "是否确认取消发布?",
                buttons:
                {
                    "click":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            UpdateRequireStatusAction(idString, status);
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
                message: "是否确认通过审核发布?",
                buttons:
                {
                    "click":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            UpdateRequireStatusAction(idString, status);
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

function UpdateRequireStatusAction(requireId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequireStatusAction?requireId=" + requireId + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "RequirementBrowse.aspx";
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

    var objRequirementName = $("input[id*='txtRequirementName']");
    grid.jqGrid.AddSearchParams("Title", "LIKE", objRequirementName.val());

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    grid.jqGrid.Search();
    return false;
}

