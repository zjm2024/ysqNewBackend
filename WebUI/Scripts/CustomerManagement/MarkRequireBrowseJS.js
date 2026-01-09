$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "MarkRequireList";
    grid.jqGrid.PagerID = "MarkRequireListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MarkRequireList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "RequirementId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="取消关注" onclick="return DeleteMark(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("MarkId");
			
    grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 50);
    grid.jqGrid.AddColumn("Title", "任务名称", true, null, 50);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);  
    grid.jqGrid.AddColumn("Commission", "酬金", true, null, 50);
    grid.jqGrid.AddColumn("CommissionDescription", "酬金说明", true, null, 50);
    grid.jqGrid.CreateTable();   
}


function EditRequirement(requireObj) {
    window.location.href = "../Require.aspx?requireId=" + $(requireObj).prev().val();
    return false;
}

function DeleteMark(requireObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var markId = $(requireObj).parent().next().html();
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Customer/DeleteMark?markId=" + markId + "&token=" + _Token,
                        type: "POST",
                        data: null,
                        success: function (data) {
                            bootbox.dialog({
                                message: data.Message,
                                buttons:
                                {
                                    "Confirm":
                                    {
                                        "label": "确定",
                                        "className": "btn-sm btn-primary",
                                        "callback": function () {
                                            window.location.href = "MarkRequireBrowse.aspx";
                                        }
                                    }
                                }
                            });
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
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

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objRequirementName = $("input[id*='txtRequireName']");
    grid.jqGrid.AddSearchParams("Title", "LIKE", objRequirementName.val());

    grid.jqGrid.Search();
    return false;
}

