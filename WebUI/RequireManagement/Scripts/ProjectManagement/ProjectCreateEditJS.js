$(document).ready(function () {    
    Init();    

    //$("#btn_save").click(function () {

    //    var projectId = parseInt($("#" + hidProjectId).val());
    //    var projectVO = GetProjectVO();
    //    $.ajax({
    //        url: _RootPath + "SPWebAPI/Project/UpdateProject?token=" + _Token,
    //        type: "POST",
    //        data: projectVO,
    //        success: function (data) {
    //            if (data.Flag == 1) {
    //                if (projectId < 1) {
    //                    $("#" + hidProjectId).val(data.Result);
    //                    SetButton();
    //                }
    //                bootbox.dialog({
    //                    message: data.Message,
    //                    buttons:
    //                    {
    //                        "Confirm":
    //                        {
    //                            "label": "确定",
    //                            "className": "btn-sm btn-primary",
    //                            "callback": function () {

    //                            }
    //                        }
    //                    }
    //                });
    //            } else {
    //                bootbox.dialog({
    //                    message: data.Message,
    //                    buttons:
    //                    {
    //                        "Confirm":
    //                        {
    //                            "label": "确定",
    //                            "className": "btn-sm btn-primary",
    //                            "callback": function () {

    //                            }
    //                        }
    //                    }
    //                });
    //            }

    //        },
    //        error: function (data) {
    //            alert(data);
    //        }
    //    });
    //});


    $("#btn_cancel").click(function () {
        window.location.href = "ProjectBrowse.aspx";
        return false;
    });
          
    
});


function GetProjectVO() {
    var projectVO = new Object();

	var objProjectId = $("input[id*='txtProjectId']");
	var objProjectCode = $("input[id*='txtProjectCode']");
	var objRequirementId = $("input[id*='txtRequirementId']");
	var objCustomerId = $("input[id*='txtCustomerId']");
	var objStartDate = $("input[id*='txtStartDate']");
	var objEndDate = $("input[id*='txtEndDate']");
	var objCommission = $("input[id*='txtCommission']");
	var objStatus = $("input[id*='txtStatus']");

	projectVO.ProjectId = objProjectId.val();
	projectVO.ProjectCode = objProjectCode.val();
	projectVO.RequirementId = objRequirementId.val();
	projectVO.CustomerId = objCustomerId.val();
	projectVO.StartDate = objStartDate.val();
	projectVO.EndDate = objEndDate.val();
	projectVO.Commission = objCommission.val();
	projectVO.Status = objStatus.val();

    return projectVO;
}

function SetProject(projectVO) {

	var objProjectCode = $("input[id*='txtProjectCode']");
	var objStartDate = $("input[id*='txtStartDate']");
	var objEndDate = $("input[id*='txtEndDate']");
	var objCommission = $("input[id*='txtCommission']");
	var objStatus = $("input[id*='txtStatus']");

	
	objProjectCode.val(projectVO.ProjectCode);	
	objStartDate.val(new Date(projectVO.StartDate).format("yyyy-MM-dd"));
	objEndDate.val(new Date(projectVO.EndDate).format("yyyy-MM-dd"));
	objCommission.val(projectVO.Commission);
	if (projectVO.Status == 0)
	    objStatus.val("已生成");
	else if (projectVO.Status == 1)
	    objStatus.val("已委托");
	else if (projectVO.Status == 2)
	    objStatus.val("工作中");
	else if (projectVO.Status == 3)
	    objStatus.val("已完成");
	else if (projectVO.Status == 4)
	    objStatus.val("申请退款");
	else if (projectVO.Status == 5)
	    objStatus.val("退款完成");
}

function SetButton() {
    var btnSave = $("#btn_save");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();    
    var projectId = parseInt($("#" + hidProjectId).val());
	
    if (isEdit == "true") {
        btnSave.show();
    } else {
        btnSave.hide();
    }
    if (projectId < 1) {
        btnDelete.hide();
    } else {
        if (isDelete == "true") {
            btnDelete.show();
        } else {
            btnDelete.hide();
        }
    }
}

function Init() {
    var projectId = parseInt($("#" + hidProjectId).val());
    if (projectId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetProject?projectId=" + projectId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var projectVO = data.Result;
                    SetProject(projectVO);
                    BindRequire(projectVO.RequirementId);
                    BindAgency(projectVO.AgencyId);
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
                load_hide();
            }
        });
    }
}
function SetRequirement(requirementVO) {

    var objCityId = $("select[id*='txtCityId']");
    var objCategoryId = $("select[id*='txtCategoryId']");
    var objRequirementCode = $("input[id*='txtRequirementCode']");
    var objTitle = $("input[id*='txtTitle']");   
    var objPhone = $("input[id*='txtPhone']");
    var objDescription = $("div[id*='divDescription']");

    objCityId.val(requirementVO.CityId);
    objCategoryId.val(requirementVO.CategoryId);
    objRequirementCode.val(requirementVO.RequirementCode);
    objTitle.val(requirementVO.Title);
    objPhone.val(requirementVO.Phone);
    objDescription.append(requirementVO.Description);

}
function BindRequire(requireId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var requireVO = data.Result;

                SetRequirement(requireVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function SetAgency(agencyVO) {

    var objAgencyName = $("input[id*='txtAgencyName']");
    var objAgencyPhone = $("input[id*='txtAgencyPhone']");
    var objAgencyLevel = $("select[id*='drpAgencyLevel']");


    objAgencyName.val(agencyVO.AgencyName);
    objAgencyPhone.val(agencyVO.Phone);
    objAgencyLevel.val(agencyVO.AgencyLevel);
}

function BindAgency(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var agencyVO = data.Result;

                SetAgency(agencyVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}
