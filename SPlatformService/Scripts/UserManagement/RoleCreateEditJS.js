$(document).ready(function () {
    SetButton();
    var roleId = parseInt($("#" + hidRoleId).val());
    InitSecurity();
    if (roleId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Role/GetRole?roleId=" + roleId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var roleVO = data.Result;
                    var objRoleName = $("input[id*='txtRoleName']");
                    var objDescription = $("textarea[id*='txtDescription']");


                    objRoleName.val(roleVO.RoleName);
                    objDescription.val(roleVO.Description);

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
                load_hide();
            },
            error: function (data) {
                alert(data);
                //load_hide();
            }
        });
    }

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtRoleName: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtRoleName: {
                required: "请输入角色名称！"
            }
        },
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
        },

        success: function (e) {
            $(e).closest('.form-group').removeClass('has-error');
            $(e).remove();
        }
    });

    $("#btn_save").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var roleId = parseInt($("#" + hidRoleId).val());
        var roleModelVO = GetRoleModelVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Role/UpdateRole?token=" + _Token,
            type: "POST",
            data: roleModelVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (roleId < 1) {
                        $("#" + hidRoleId).val(data.Result);
                        SetButton();
                    }
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

            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("#btn_delete").click(function () {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        DeleteAction();
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
    });

    $("#btn_cancel").click(function () {
        window.location.href = "RoleBrowse.aspx";
        return false;
    });
});

function DeleteAction() {
    var roleId = parseInt($("#" + hidRoleId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Role/DeleteRole?roleId=" + roleId + "&token=" + _Token,
        type: "POST",
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

        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetRoleModelVO() {
    var roleModelVO = new Object();

    var roleVO = new Object();
    roleModelVO.Role = roleVO;
    var objRoleName = $("input[id*='txtRoleName']");
    var objDescription = $("textarea[id*='txtDescription']");
    roleVO.RoleId = parseInt($("#" + hidRoleId).val());
    roleVO.RoleName = objRoleName.val();
    roleVO.Description = objDescription.val();

    var roleSecurityVOList = new Array();
    roleModelVO.RoleSecurity = roleSecurityVOList;

    var chkList = $("#SecurityList").find("input[type='checkbox']:checked");
    for (var i = 0; i < chkList.length; i++) {
        var chk = chkList[i];
        var chkId = chk.id;
        var securityId = chkId.split("_")[2];
        var roleSecurityVO = new Object();
        roleSecurityVOList.push(roleSecurityVO);
        roleSecurityVO.RoleId = roleVO.RoleId;
        roleSecurityVO.SecurityId = securityId;
    }

    return roleModelVO;
}

function SetButton() {
    var btnSave = $("#btn_save");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();
    var roleId = parseInt($("#" + hidRoleId).val());
    if (isEdit == "true") {
        btnSave.show();
    } else {
        btnSave.hide();
    }
    if (roleId < 1) {
        btnDelete.hide();
    } else {
        if (isDelete == "true") {
            btnDelete.show();
        } else {
            btnDelete.hide();
        }
    }
}

function InitSecurity() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Role/GetAllSecurity?token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var stVOList = data.Result;
                //<tr class="ui-widget-content jqgrow ui-row-ltr">
                //                <td style="" title="工时录入">工时录入</td>
                //                <td style="" title="">
                //                    <label class="position-relative col-sm-2">
                //                        <input class="ace" type="checkbox" />
                //                        <span class="lbl">查看</span>
                //                    </label>
                //                </td>
                //            </tr>
                var oTable = $("#SecurityList");
                for (var i = 0; i < stVOList.length; i++) {
                    var stVO = stVOList[i];                    
                    var oTR = "<tr class=\"ui-widget-content jqgrow ui-row-ltr\">\r\n";
                    oTR += "    <td style=\"\" title=\"" + stVO.GroupTypeName + "\">" + stVO.GroupTypeName + "</td>\r\n";
                    oTR += "    <td style=\"\" title=\"" + stVO.SecurityTypeName + "\">" + stVO.SecurityTypeName + "</td>\r\n";
                    oTR += "    <td style=\"\" title=\"\">\r\n";
                    for (var j = 0; j < stVO.SecurityVOList.length; j++) {
                        var sVO = stVO.SecurityVOList[j];
                        oTR += "        <label class=\"position-relative col-sm-2\">\r\n";
                        oTR += "            <input id=\"chk_" + stVO.SecurityTypeId + "_" + sVO.SecurityId + "\" class=\"ace\" type=\"checkbox\" />\r\n";
                        oTR += "            <span class=\"lbl\">" + sVO.ActionName + "</span>\r\n";
                        oTR += "        </label>\r\n";
                    }                    
                    oTR += "    </td>\r\n";
                    oTR += "</tr>\r\n";

                    oTable.append(oTR);
                }
                var roleId = parseInt($("#" + hidRoleId).val());
                if (roleId > 0)
                    BindSecurity(roleId);

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
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}

function BindSecurity(roleId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Role/GetAllSecurityByRole?roleId=" + roleId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var rsVOList = data.Result;                
                for (var i = 0; i < rsVOList.length; i++) {
                    var rsVO = rsVOList[i];
                    //根据Id查找CheckBox，并checked
                    var chkId = "chk_" + rsVO.SecurityTypeId + "_" + rsVO.SecurityId;
                    $("#" + chkId).attr("checked", true);
                }

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
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}
