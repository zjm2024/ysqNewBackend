$(function () {
    SetButton();
    InitDropDown();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "UserList";
    grid.jqGrid.PagerID = "UserListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=UserList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("UserId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditUser(this);"></img>';
            //if (($("#" + HidIsSystem).val() || $("#" + HidIsDelete).val()) && rowObject.Status == "3")
            result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteUserOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("UserId");
    grid.jqGrid.AddColumn("DepartmentName", "门店名称", true, null, 80);
    grid.jqGrid.AddColumn("UserName", "用户名", true, null, 100);
    grid.jqGrid.AddColumn("LoginName", "登录名", true, null, 100);
    //grid.jqGrid.AddColumn("RoleName", "角色", true, null, 100);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, null, 100);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 120);
    grid.jqGrid.AddColumn("CreatedBy", "创建人", true, null, 90);
    //grid.jqGrid.AddColumn("UpdatedAt", "最后修改", true, null, 120);
    //grid.jqGrid.AddColumn("UpdatedBy", "最后修改人", true, null, 100);
    grid.jqGrid.CreateTable();
    //if (getBrowser() == "firefox") {
    //    autoresize("UserList");
    //}
    //else {
    //    setTimeout('autoresize("UserList");', 500);
    //}

    //$("#frame").resize(function () {
    //    $(window).unbind("onresize");
    //    autoresize("UserList");
    //    $(window).bind("onresize", this);
    //});
}


function EditUser(userObj) {
    window.location.href = "UserCreateEdit.aspx?UserId=" + $(userObj).prev().val();
    return false;
}

function NewUser() {
    window.location.href = "UserCreateEdit.aspx";
    return false;
}

function DeleteUserOne(userObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var userId = $(userObj).prev().prev().val();
                    DeleteAction(userId);
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

function DeleteUser() {
    var id = $("#UserList").jqGrid('getGridParam', 'selarrrow');
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

function DeleteAction(userId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/DeleteUser?userId=" + userId + "&token=" + _Token,
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
                                window.location.href = "UserBrowse.aspx";
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
    //department usertitle user name
    var objDepartmentId = $("select[id*='drpDepartment']");
    if (objDepartmentId.val() != "-1")
        grid.jqGrid.AddSearchParams("DepartmentId", "INTEQUAL", objDepartmentId.val());

    var objUserName = $("input[id*='txtUserName']");
    grid.jqGrid.AddSearchParams("UserName", "LIKE", objUserName.val());

    grid.jqGrid.Search();
    return false;
}

function InitDropDown() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Department/GetDepartmentAll?token=" + _Token,
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var depList = data.Result;
                var objDepartment = $("select[id*='drpDepartment']");
                for (var i = 0; i < depList.length; i++) {
                    var depObj = depList[i];
                    objDepartment.append("<option value=\"" + depObj.DepartmentId + "\">" + depObj.DepartmentName + "</option>");
                }
                
            } else {
                
            }

        },
        error: function (data) {
            alert(data);
        }
    });   
}