$(function () {
    SetButton();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "DepartmentList";
    grid.jqGrid.PagerID = "DepartmentListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=DepartmentList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("DepartmentId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditDepartment(this);"></img>';
            //if (($("#" + HidIsSystem).val() || $("#" + HidIsDelete).val()) && rowObject.Status == "3")
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteDepartmentOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("DepartmentId");
    grid.jqGrid.AddColumn("DepartmentName", "部门名称", true, null, 120);
    grid.jqGrid.AddColumn("Description", "备注", true, null, 155);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 120);
    grid.jqGrid.AddColumn("CreatedBy", "创建人", true, null, 90);
    grid.jqGrid.AddColumn("UpdatedAt", "最后修改", true, null, 120);
    grid.jqGrid.AddColumn("UpdatedBy", "最后修改人", true, null, 90);
    grid.jqGrid.CreateTable();
    //if (getBrowser() == "firefox") {
    //    autoresize("DepartmentList");
    //}
    //else {
    //    setTimeout('autoresize("DepartmentList");', 500);
    //}

    //$("#frame").resize(function () {
    //    $(window).unbind("onresize");
    //    autoresize("DepartmentList");
    //    $(window).bind("onresize", this);
    //});
}


function EditDepartment(departmentObj) {
    window.location.href = "DepartmentCreateEdit.aspx?DepartmentId=" + $(departmentObj).prev().val();
    return false;
}

function NewDepartment() {
    window.location.href = "DepartmentCreateEdit.aspx";
    return false;
}

function DeleteDepartmentOne(departmentObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var departmentId = $(departmentObj).prev().prev().val();
                    DeleteAction(departmentId);
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

function DeleteDepartment() {
    var id = $("#DepartmentList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认删除部门，并且删除部门里用户?",
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

function DeleteAction(departmentId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Department/DeleteDepartment?DepartmentId=" + departmentId + "&token=" + _Token,
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
                                window.location.href = "DepartmentBrowse.aspx";
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

function SetButton() {
    var btnNew = $("#btn_new");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();

    if (isEdit == "true") {
        btnNew.show();
    } else {
        btnNew.hide();
    }
    
    if (isDelete == "true") {
        btnDelete.show();
    } else {
        btnDelete.hide();
    }
    
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objDepartmentName = $("input[id*='txtDepartmentName']");
    if (objDepartmentName.val() != "")
        grid.jqGrid.AddSearchParams("DepartmentName", "LIKE", objDepartmentName.val());

    grid.jqGrid.Search();
    return false;
}

