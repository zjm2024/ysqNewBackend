$(function () {
    SetButton();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "RoleList";
    grid.jqGrid.PagerID = "RoleListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=RoleList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("RoleId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRole(this);"></img>';
            //if (($("#" + HidIsSystem).val() || $("#" + HidIsDelete).val()) && rowObject.Status == "3")
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteRoleOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("RoleId");
    grid.jqGrid.AddColumn("RoleName", "角色名称", true, null, 100);
    grid.jqGrid.AddColumn("Description", "备注", true, null, 155);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 120);
    grid.jqGrid.AddColumn("CreatedBy", "创建人", true, null, 100);
    grid.jqGrid.AddColumn("UpdatedAt", "最后修改", true, null, 120);
    grid.jqGrid.AddColumn("UpdatedBy", "最后修改人", true, null, 100);
    grid.jqGrid.CreateTable();
    //if (getBrowser() == "firefox") {
    //    autoresize("RoleList");
    //}
    //else {
    //    setTimeout('autoresize("RoleList");', 500);
    //}

    //$("#frame").resize(function () {
    //    $(window).unbind("onresize");
    //    autoresize("RoleList");
    //    $(window).bind("onresize", this);
    //});
}


function EditRole(roleObj) {
    window.location.href = "RoleCreateEdit.aspx?RoleId=" + $(roleObj).prev().val();
    return false;
}

function NewRole() {
    window.location.href = "RoleCreateEdit.aspx";
    return false;
}

function DeleteRoleOne(roleObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var roleId = $(roleObj).prev().prev().val();
                    DeleteAction(roleId);
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

function DeleteRole() {
    var id = $("#RoleList").jqGrid('getGridParam', 'selarrrow');
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

function DeleteAction(roleId) {
    $.ajax({     
        url: _RootPath + "SPWebAPI/Role/DeleteRole?RoleId=" + roleId + "&token=" + _Token,
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
                                window.location.href = "RoleBrowse.aspx";
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

    var objRoleName = $("input[id*='txtRoleName']");
    if (objRoleName.val() != "")
        grid.jqGrid.AddSearchParams("RoleName", "LIKE", objRoleName.val());

    grid.jqGrid.Search();
    return false;
}