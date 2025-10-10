$(document).ready(function () {
    SetButton();
    var departmentId = parseInt($("#" + hidDepartmentId).val());
    if (departmentId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Department/GetDepartment?departmentId=" + departmentId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var depVO = data.Result;
                    var objDepartmentName = $("input[id*='txtDepartmentName']");
                    var objDescription = $("textarea[id*='txtDescription']");
                    

                    objDepartmentName.val(depVO.DepartmentName);
                    objDescription.val(depVO.Description);
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
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtDepartmentName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$drpArea: {
                ddlrequired: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtDepartmentName: {
                required: "请输入部门名称！"
            },
            ctl00$ContentPlaceHolder_Content$drpArea: {
                ddlrequired: "请选择地区！"
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

        var departmentId = parseInt($("#" + hidDepartmentId).val());
        var departmentVO = GetDepartmentVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Department/UpdateDepartment?token=" + _Token,
            type: "POST",
            data: departmentVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (departmentId < 1) {
                        $("#" + hidDepartmentId).val(data.Result);
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
        window.location.href = "DepartmentBrowse.aspx";
        return false;
    });
});

function DeleteAction() {
    var departmentId = parseInt($("#" + hidDepartmentId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Department/DeleteDepartment?departmentId=" + departmentId + "&token=" + _Token,
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

        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetDepartmentVO() {
    var depVO = new Object();
    var objDepartmentName = $("input[id*='txtDepartmentName']");
    var objDescription = $("textarea[id*='txtDescription']");
    depVO.DepartmentId = parseInt($("#" + hidDepartmentId).val());
    depVO.DepartmentName = objDepartmentName.val();
    depVO.Description = objDescription.val();
    return depVO;
}

function SetButton() {
    var btnSave = $("#btn_save");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();
    var departmentId = parseInt($("#" + hidDepartmentId).val());
    if (isEdit == "true") {
        btnSave.show();
    } else {
        btnSave.hide();
    }
    if (departmentId < 1) {
        btnDelete.hide();
    } else {
        if (isDelete == "true") {
            btnDelete.show();
        } else {
            btnDelete.hide();
        }
    }
}

