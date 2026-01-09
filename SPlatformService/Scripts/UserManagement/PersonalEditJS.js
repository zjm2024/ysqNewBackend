$(document).ready(function () {
    InitDropDown();
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
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
            ctl00$ContentPlaceHolder_Content$drpDepartment: {
                ddlrequired: true
            },
            //ctl00$ContentPlaceHolder_Content$drpRole: {
            //    ddlrequired: true
            //},
            ctl00$ContentPlaceHolder_Content$txtUserName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtLoginName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel: true
            },
            ctl00$ContentPlaceHolder_Content$txtEmail: {
                email: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                maxlength: 400
            },
            ctl00$ContentPlaceHolder_Content$txtPassword: {
                Password: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: true,
                Password: true,
                equalTo: "#ContentPlaceHolder_Content_txtPassword"
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$drpDepartment: {
                ddlrequired: "请选择门店！"
            },
            //ctl00$ContentPlaceHolder_Content$drpRole: {
            //    ddlrequired: "请选择角色！"
            //},
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
            },
            ctl00$ContentPlaceHolder_Content$txtPasswordConfirm: {
                required: "请输入确认密码！",
                equalTo: "请保证两次密码一致!"
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

    //$("#btn_edit").click(function () {
    //    $("#btn_save").show();
    //    $("#btn_cancel").show();
    //    $("#btn_edit").hide(); 
    //    var objUserName = $("input[id*='txtUserName']");
    //    var objDescription = $("textarea[id*='txtDescription']");
    //    var objLoginName = $("input[id*='txtLoginName']");
    //    var objPassword = $("input[id*='txtPassword']");
    //    var objPhone = $("input[id*='txtPhone']");
    //    var objEmail = $("input[id*='txtEmail']");
    //    var objMale = $("input[id*='radSexMale']");
    //    var objFeMale = $("input[id*='radSexFeMale']");

    //    objUserName.removeAttr("disabled");
    //    objDescription.removeAttr("disabled");
    //    objLoginName.removeAttr("disabled");
    //    objPassword.removeAttr("disabled");
    //    objPhone.removeAttr("disabled");
    //    objEmail.removeAttr("disabled");
    //    objMale.removeAttr("disabled");
    //    objFeMale.removeAttr("disabled");

    //    return false;
    //});

    //$("#btn_cancel").click(function () {
    //    $("#btn_save").hide();
    //    $("#btn_cancel").hide();
    //    $("#btn_edit").show();
    //    var objUserName = $("input[id*='txtUserName']");
    //    var objDescription = $("textarea[id*='txtDescription']");
    //    var objLoginName = $("input[id*='txtLoginName']");
    //    var objPassword = $("input[id*='txtPassword']");
    //    var objPhone = $("input[id*='txtPhone']");
    //    var objEmail = $("input[id*='txtEmail']");
    //    var objMale = $("input[id*='radSexMale']");
    //    var objFeMale = $("input[id*='radSexFeMale']");

    //    objUserName.attr("disabled", "disabled");
    //    objDescription.attr("disabled", "disabled");
    //    objLoginName.attr("disabled", "disabled");
    //    objPassword.attr("disabled", "disabled");
    //    objPhone.attr("disabled", "disabled");
    //    objEmail.attr("disabled", "disabled");
    //    objMale.attr("disabled", "disabled");
    //    objFeMale.attr("disabled", "disabled");

    //    return false;
    //});
});

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
    var objLoginName = $("input[id*='txtLoginName']");
    var objPassword = $("input[id*='txtPassword']");
    var objPhone = $("input[id*='txtPhone']");
    var objEmail = $("input[id*='txtEmail']");
    var objMale = $("input[id*='radSexMale']:checked");

    userVO.UserId = parseInt($("#" + hidUserId).val());
    userVO.UserName = objUserName.val();
    userVO.Description = objDescription.val();
    userVO.DepartmentId = objDepartment.val();
    userVO.LoginName = objLoginName.val();
    userVO.Password = objPassword.val();
    userVO.Phone = objPhone.val();
    userVO.Email = objEmail.val();

    userVO.UserCode = "";

    if (objMale.length > 0)
        userVO.Sex = true;
    else
        userVO.Sex = false;


    var roleList = $("select[id$='" + roleListId + "']");
    var userRoleList = new Array();
    userModleVO.UserRole = userRoleList;

    for (var i = 0; i < roleList.length; i++) {
        var userRoleVO = new Object();
        userRoleList.push(userRoleVO);
        userRoleVO.UserId = userVO.UserId;
        userRoleVO.RoleId = roleList.children()[0].value;
    }

    return userModleVO;
}

function SetUser(userModelVO) {
    var userVO = userModelVO.User;
    var userRoleList = userModelVO.UserRole;

    var objUserName = $("input[id*='txtUserName']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objDepartment = $("select[id*='drpDepartment']");
    //var objRole = $("select[id*='drpRole']");
    var objLoginName = $("input[id*='txtLoginName']");
    var objPhone = $("input[id*='txtPhone']");
    var objEmail = $("input[id*='txtEmail']");
    var objMale = $("input[id*='radSexMale']");
    var objFeMale = $("input[id*='radSexFeMale']");

    objUserName.val(userVO.UserName);
    objDescription.val(userVO.Description);
    objDepartment.val(userVO.DepartmentId);
    //objRole.val(userVO.RoleId);    

    objLoginName.val(userVO.LoginName);
    objPhone.val(userVO.Phone);
    objEmail.val(userVO.Email);
    if (userVO.Sex == 1)
        objMale.attr("checked", true);
    else
        objFeMale.attr("checked", true);
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