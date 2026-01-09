$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "MarkAgencyList";
    grid.jqGrid.PagerID = "MarkAgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MarkAgencyList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "AgencyId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="取消关注" onclick="return DeleteMark(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("MarkId");
			
    grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 50);
    grid.jqGrid.AddColumn("AgencyLevel", "销售级别", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == 1)
                return "金牌销售";
            else if (obj == 2)
                return "银牌销售";
            else if (obj == 3)
                return "铜牌销售";
        }, false);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);  
    grid.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 50);
    grid.jqGrid.CreateTable();   
}


function EditRequirement(agencyObj) {
    window.location.href = "../Agency.aspx?agencyId=" + $(agencyObj).prev().val();
    return false;
}

function DeleteMark(agencyObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var markId = $(agencyObj).parent().next().html();
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
                                            window.location.href = "MarkAgencyBrowse.aspx";
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

    var objRequirementName = $("input[id*='txtAgencyName']");
    grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objRequirementName.val());

    grid.jqGrid.Search();
    return false;
}

