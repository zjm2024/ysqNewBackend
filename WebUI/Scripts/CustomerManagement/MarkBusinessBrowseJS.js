$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "MarkBusinessList";
    grid.jqGrid.PagerID = "MarkBusinessListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MarkBusinessList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "BusinessId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("BusinessId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="取消关注" onclick="return DeleteMark(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("MarkId");
			
    grid.jqGrid.AddColumn("CompanyName", "雇主名称", true, null, 50);
    grid.jqGrid.AddColumn("CompanyType", "公司性质", true, null, 50);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);  
    grid.jqGrid.AddColumn("SetupDate", "成立时间", true, null, 50);
    grid.jqGrid.CreateTable();   
}


function EditRequirement(businessObj) {
    window.location.href = "../Business.aspx?BusinessId=" + $(businessObj).prev().val();
    return false;
}

function DeleteMark(businessObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var markId = $(businessObj).parent().next().html();
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
                                            window.location.href = "MarkBusinessBrowse.aspx";
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

    var objRequirementName = $("input[id*='txtBusinessName']");
    grid.jqGrid.AddSearchParams("CompanyName", "LIKE", objRequirementName.val());

    grid.jqGrid.Search();
    return false;
}

