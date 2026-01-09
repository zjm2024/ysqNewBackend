$(document).ready(function () {
    SetButton();
    InitDropDown();
    BindRole();
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $.validator.addMethod("listrequired", function (value, element, params) {
        if ($(element).children().length < 1) {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $.validator.addMethod("isTel", function (value, element, params) {
        var length = value.length;
        var mobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value)) || length == 0;
    }, "请输入正确格式的电话");

    $.validator.addMethod("Password", function (value, element, params) {
        var arr = value.split(".");
        var reg = /^[a-zA-Z]\w{5,17}$/;
        if (reg.test(value)) {
            return true;
        }
        else
            return false;
    }, "密码必须以字母开头，长度在6-18之间，只能包含字符、数字和下划线");

    $.validator.addMethod("email", function (value, element, params) {
        var ema = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/;
        return this.optional(element) || (ema.test(value));
    }, "请输入正确格式的电子邮箱");

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$drpDepartment:{
                ddlrequired:true
            },
            ctl00$ContentPlaceHolder_Content$drpUserTitle:{
                ddlrequired:true
            },
            ctl00$ContentPlaceHolder_Content$listRoleSelected: {
                listrequired: true
            },
            ctl00$ContentPlaceHolder_Content$txtUserName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtLoginName: {
                required:true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel:true
            },
            ctl00$ContentPlaceHolder_Content$txtEmail: {
                email:true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription:{
                maxlength:400
            },
            ctl00$ContentPlaceHolder_Content$txtPassword: {
                Password: true,
                required: true
            },/*
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: true,
                Password: true,
                equalTo: "#ctl00_ContentPlaceHolder_Content_txtPassword"
            }*/
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$drpDepartment: {
                ddlrequired: "请选择部门！"
            },
            ctl00$ContentPlaceHolder_Content$drpUserTitle: {
                ddlrequired: "请选择职务！"
            },
            ctl00$ContentPlaceHolder_Content$listRoleSelected: {
                listrequired: "请选择角色！"
            },
            ctl00$ContentPlaceHolder_Content$txtUserName: {
                required: "请输入员工名称！"
            },
            ctl00$ContentPlaceHolder_Content$txtLoginName: {
                required: "请输入登录名称！"
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel: "请输入正确格式电话！"
            },
            ctl00$ContentPlaceHolder_Content$txtEmail: {
                email: "请输入正确格式Email！"
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                maxlength: "请输入少于400个字符！"
            },
            ctl00$ContentPlaceHolder_Content$txtPassword: {
                required: "请输入登录密码！"
            }/*
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: "请输入确认密码！",
                equalTo: "请保证两次密码一致!"
            }*/
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

        var userId = parseInt($("#" + hidUserId).val());
        var userVO = GetUserVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/User/UpdateUser?token=" + _Token,
            type: "POST",
            data: userVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (userId < 1) {
                        $("#" + hidUserId).val(data.Result);
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
        window.location.href = "UserBrowse.aspx";
        return false;
    });
          
    
});

function ChangePassword() {    
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 35%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChangeUserPassword.aspx" height="200px" width="100%" frameborder="0"><\/iframe>',

        title: "修改密码",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    //if (!$(window.frames["iframe_1"].document).find("#form1").valid()) {
                    //    return false;
                    //}
                    //return false;

                    var passwordObj = $(window.frames["iframe_1"].document).find("#txtPassword");
                    var confirmPasswordObj = $(window.frames["iframe_1"].document).find("#txtPasswordConfirm");
                    var password = $(window.frames["iframe_1"].document).find("#txtPassword").val();
                    var confirmPassword = $(window.frames["iframe_1"].document).find("#txtPasswordConfirm").val();
                    //判断空
                    if (password == "") {
                        $(passwordObj).closest('.form-group').removeClass('has-info').addClass('has-error');
                        if ($(passwordObj).parent().children().length < 2)
                            $(passwordObj).parent().append("<div for=\"txtPassword\" class=\"help-block\">请输入新密码！</div>");
                        return false;
                    }
                    if (confirmPassword == "") {
                        $(confirmPasswordObj).closest('.form-group').removeClass('has-info').addClass('has-error');
                        if ($(confirmPasswordObj).parent().children().length < 2)
                            $(confirmPasswordObj).parent().append("<div for=\"txtPasswordConfirm\" class=\"help-block\">请输入确认密码！</div>");
                        return false;
                    }
                    //判断规则
                    var reg = /^[a-zA-Z]\w{5,17}$/;
                    if (!reg.test(password)) {
                        $(passwordObj).closest('.form-group').removeClass('has-info').addClass('has-error');
                        if ($(passwordObj).parent().children().length < 2)
                            $(passwordObj).parent().append("<div for=\"txtPasswordConfirm\" class=\"help-block\">密码必须以字母开头，长度在6-18之间，只能包含字符、数字和下划线!</div>");
                        return false;
                    }
                    if (!reg.test(confirmPassword)) {
                        $(confirmPasswordObj).closest('.form-group').removeClass('has-info').addClass('has-error');
                        if ($(confirmPasswordObj).parent().children().length < 2)
                            $(confirmPasswordObj).parent().append("<div for=\"txtPasswordConfirm\" class=\"help-block\">密码必须以字母开头，长度在6-18之间，只能包含字符、数字和下划线!</div>");
                        return false;
                    }
                    if (password != confirmPassword) {
                        $(confirmPasswordObj).closest('.form-group').removeClass('has-info').addClass('has-error');
                        if ($(confirmPasswordObj).parent().children().length < 2)
                            $(confirmPasswordObj).parent().append("<div for=\"txtPasswordConfirm\" class=\"help-block\">请保证两次密码一致!</div>");
                        return false;
                    }

                    var userId = parseInt($("#" + hidUserId).val());
                    $.ajax({
                        url: _RootPath + "SPWebAPI/User/ChangeUserPassword?userId=" + userId + "&newPassword=" + password + "&token=" +_Token,
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
                                               
                                            }
                                        }
                                    }
                                }); 
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                    $("#iframe_1").parent().empty();
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
    return false;
}

function DeleteAction() {
    var userId = parseInt($("#" + hidUserId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/User/DeleteUser?userId=" + userId + "&token=" + _Token,
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

        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetUserVO() {
    var userModleVO = new Object();
    var userVO = new Object();
    userModleVO.User = userVO;
    var objUserName = $("input[id*='txtUserName']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objDepartment = $("select[id*='drpDepartment']");
    var objRole = $("select[id*='drpRole']");
    var objLoginName = $("input[id*='txtLoginName']");
    var objPassword = $("input[id*='txtPassword']");
    var objPhone = $("input[id*='txtPhone']");
    var objEmail = $("input[id*='txtEmail']");
    var objMale = $("input[id*='radSexMale']:checked");

    userVO.UserId = parseInt($("#" + hidUserId).val());
    userVO.UserName = objUserName.val();
    userVO.Description = objDescription.val();
    userVO.DepartmentId = objDepartment.val();
    userVO.RoleId = objRole.val();
    userVO.LoginName = objLoginName.val();
    userVO.Password = objPassword.val();
    userVO.Phone = objPhone.val();
    userVO.Email = objEmail.val();

    userVO.UserCode = "";

    if (objMale.length > 0)
        userVO.Sex = true;
    else
        userVO.Sex = false;

    var oRoleSelected = $("input[id$='hidSelectedRole']");
    var userRoleList = new Array();
    userModleVO.UserRole = userRoleList;

    var oRoleArray = (oRoleSelected.val() == "") ? new Array() : oRoleSelected.val().split(",");
    for (var i = 0; i < oRoleArray.length; i++) {
        var userRoleVO = new Object();
        userRoleList.push(userRoleVO);
        userRoleVO.UserId = userVO.UserId;
        userRoleVO.RoleId = oRoleArray[i];
    }


    return userModleVO;
}

function SetUser(userModelVO) {
    var userVO = userModelVO.User;
    var userRoleList = userModelVO.UserRole;

    var objUserName = $("input[id*='txtUserName']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objDepartment = $("select[id*='drpDepartment']");
    var objRole = $("select[id*='drpRole']");
    var objLoginName = $("input[id*='txtLoginName']");
    var objPhone = $("input[id*='txtPhone']");
    var objEmail = $("input[id*='txtEmail']");
    var objMale = $("input[id*='radSexMale']");
    var objFeMale = $("input[id*='radSexFeMale']");

    objUserName.val(userVO.UserName);
    objDescription.val(userVO.Description);
    objDepartment.val(userVO.DepartmentId);
    
    var oRoleSelected = $("input[id$='hidSelectedRole']");
    var roleIdS = "";
    for (var i = 0; i < userRoleList.length - 1; i++) {
        roleIdS += userRoleList[i].RoleId + ",";
    }
    if (userRoleList.length > 0)
        roleIdS += userRoleList[userRoleList.length - 1].RoleId;

    oRoleSelected.val(roleIdS);

    BindRole();


    objLoginName.val(userVO.LoginName);
    objPhone.val(userVO.Phone);
    objEmail.val(userVO.Email);
    if (userVO.Sex == 1)
        objMale.attr("checked", true);
    else
        objFeMale.attr("checked", true);
}

function SetButton() {
    var btnSave = $("#btn_save");
    var btnDelete = $("#btn_delete");
    var btnUpdatePassword = $("#btn_updatepassword");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();
    var isUpdatePassword = $("#" + hidIsUpdatePassword).val();
    var userId = parseInt($("#" + hidUserId).val());

    var txtPassword = $("input[id*='txtPassword']");
    var txtPasswordConfirm = $("input[id*='txtPasswordConfirm']");

    if (isEdit == "true") {
        btnSave.show();
    } else {
        btnSave.hide();
    }
    if (userId < 1) {
        btnDelete.hide();

        txtPassword.parent().parent().show();
        txtPassword.show();
        txtPasswordConfirm.parent().parent().show();
        txtPasswordConfirm.show();

    } else {
        if (isDelete == "true") {
            btnDelete.show();
        } else {
            btnDelete.hide();
        }

        txtPassword.parent().parent().hide();
        txtPassword.hide();
        txtPasswordConfirm.parent().parent().hide();
        txtPasswordConfirm.hide();

        if (isUpdatePassword == "true") {
            btnUpdatePassword.show();
        } else {
            btnUpdatePassword.hide();
        }

    }
}

function InitDropDown() {
    //部门
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

                //角色
                //$.ajax({
                //    url: _RootPath + "SPWebAPI/Role/GetRoleAll",
                //    type: "GET",
                //    data: null,
                //    success: function (data) {
                //        if (data.Flag == 1) {
                //            var roleList = data.Result;
                //            var objRole = $("select[id*='drpRole']");
                //            for (var i = 0; i < roleList.length; i++) {
                //                var roleObj = roleList[i];
                //                objRole.append("<option value=\"" + roleObj.RoleId + "\">" + roleObj.RoleName + "</option>");
                //            }

                //        } else {

                //        }
                        var userId = parseInt($("#" + hidUserId).val());
                        if (userId > 0) {
                            load_show();
                            $.ajax({
                                url: _RootPath + "SPWebAPI/User/GetUser?userId=" + userId + "&token=" + _Token,
                                type: "Get",
                                data: null,
                                success: function (data) {
                                    if (data.Flag == 1) {
                                        var userVO = data.Result;
                                        SetUser(userVO);

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
                //    },
                //    error: function (data) {
                //        alert(data);
                //    }
                //});

            } else {

            }

        },
        error: function (data) {
            alert(data);
        }
    });
}

function BindRole() {
    var oRoleSelected = $("input[id$='hidSelectedRole']");

    var listRole = $("select[id$='listRole']");
    var listRoleSelected = $("select[id$='listRoleSelected']");

    var oRoleArray = (oRoleSelected.val() == "") ? new Array() : oRoleSelected.val().split(",");
    for (var i = 0; i < oRoleArray.length; i++) {
        var optionTemp = listRole.find("option[value='" + oRoleArray[i] + "']");
        optionTemp.remove();
        listRoleSelected.append(optionTemp);
    }
}

function OnAddList(oListId, tListId, hidSelectedId) {
    var oList = $("select[id$='" + oListId + "']");
    var tList = $("select[id$='" + tListId + "']");
    var selectedObj = oList.find("option:selected");
    var oSelected = $("input[id$='" + hidSelectedId + "']");
    var selectedIdArray = (oSelected.val() == "") ? new Array() : oSelected.val().split(",");
    for (var i = 0; i < selectedObj.length; i++) {
        selectedIdArray.push(selectedObj[i].value);
    }
    oSelected.val(selectedIdArray.join(","));

    selectedObj.remove();
    tList.append(selectedObj);



    return false;
}

function OnRemoveList(oListId, tListId, hidSelectedId) {
    var oList = $("select[id$='" + oListId + "']");
    var tList = $("select[id$='" + tListId + "']");
    var selectedObj = tList.find("option:selected");
    var oSelected = $("input[id$='" + hidSelectedId + "']");

    selectedIdStr = "," + oSelected.val() + ",";
    for (var i = 0; i < selectedObj.length; i++) {
        selectedIdStr = selectedIdStr.replace("," + selectedObj[i].value + ",", ",");
    }
    oSelected.val(selectedIdStr.substring(1, selectedIdStr.length - 1));

    selectedObj.remove();
    oList.append(selectedObj);
    return false;
}

function OnFilter(txtObj, listId) {
    var listObj = $("select[id$='" + listId + "']");
    if (listObj) {
        var optionList = listObj.find("option");
        for (var i = 0; i < optionList.length; i++) {
            var option = $(optionList[i]);
            var optionP = option.parent("span");
            if (option.text().toLowerCase().indexOf(txtObj.value.toLowerCase()) == -1) {
                //option.hide();
                if (!optionP.size()) {
                    option.wrap("<span style='display:none'></span>");
                }
            }
            else {
                //option.show();
                if (optionP.size()) {
                    optionP.children().clone().replaceAll(optionP);
                }
            }
        }
    }
    return false;
}
