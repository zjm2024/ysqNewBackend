$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ServicesList";
    grid.jqGrid.PagerID = "ServicesListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ServicesList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ServicesId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditServices(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteServicesOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ServicesId");
			
	grid.jqGrid.AddColumn("ServicesCode", "服务编号", true, null, 50);
	grid.jqGrid.AddColumn("Title", "标题", true, null, 50);				
	grid.jqGrid.AddColumn("Price", "价格", true, null, 50);				
	grid.jqGrid.AddColumn("Count", "数量", true, null, 50);
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "保存";
            else if (obj == '1')
                return "发布";
        }, false);
	grid.jqGrid.AddColumn("CreatedAt", "创建日期", true, null, 50);				
    grid.jqGrid.CreateTable();   
}


function EditServices(servicesObj) {
    window.location.href = "ServicesCreateEdit.aspx?ServicesId=" + $(servicesObj).prev().val();
    return false;
}

function UpdateServicesStatus(status) {
    var id = $("#ServicesList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认取消发布?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateServicesStatusAction(idString, status);
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

function UpdateServicesStatusAction(servicesId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateServicesStatus?servicesId=" + servicesId + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "ServicesBrowse.aspx";
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

    var objServicesName = $("input[id*='txtServicesName']");
    grid.jqGrid.AddSearchParams("Title", "LIKE", objServicesName.val());

    grid.jqGrid.Search();
    return false;
}

