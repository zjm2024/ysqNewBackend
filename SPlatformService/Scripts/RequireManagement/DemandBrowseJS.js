$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "DemandList";
    grid.jqGrid.PagerID = "DemandListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=DemandList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("DemandId");
    grid.jqGrid.AddColumn("DemandId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddColumn("Description", "需求详情", true, null, 130);
    grid.jqGrid.AddColumn("OfferCount", "收到留言", true, "center", 30);
    grid.jqGrid.AddColumn("CategoryName", "需求类别", true, "center" , 80);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 100);
    grid.jqGrid.AddColumn("Status", "状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "保存";
            else if (obj == '1')
                return "发布";
            else if (obj == '2')
                return "审核中";
            else
                return "";
        }, false);
    grid.jqGrid.AddColumn("CreatedAt", "创建日期", true, null, 100);
    grid.jqGrid.AddColumn("CustomerName", "所属会员", true, null, 100);
    grid.jqGrid.CreateTable();   
}


function EditRequirement(requirementObj) {
    window.location.href = "DemandCreateEdit.aspx?DemandId=" + $(requirementObj).prev().val();
    return false;
}

function NewRequirement() {
    window.location.href = "DemandCreateEdit.aspx";
    return false;
}

function UpdateRequireStatus(status) {
    var id = $("#DemandList").jqGrid('getGridParam', 'selarrrow');
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

function UpdateRequireStatusAction(DemandId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateDemandStatus?DemandId=" + DemandId + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "DemandBrowse.aspx";
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
    grid.jqGrid.AddSearchParams("Description", "LIKE", objRequirementName.val());

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    grid.jqGrid.Search();
    return false;
}

