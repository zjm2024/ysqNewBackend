$(document).ready(function () {
    Init();
    initDatePicker();
    
    $("#btn_cancel").click(function () {        
        window.location.href = "ProjectBrowse.aspx";
    });
});


function SetProject(projectVO) {

    var objProjectCode = $("input[id*='txtProjectCode']");
    var objStartDate = $("input[id*='txtStartDate']");
    var objEndDate = $("input[id*='txtEndDate']");
    var objCommission = $("input[id*='txtCommission']");
    var objCost = $("input[id*='txtCost']");
    var objStatus = $("input[id*='txtStatus']");
    var objDecimal = $("input[id*='radDecimal']");
    var objPer = $("input[id*='radPer']");

    objProjectCode.val(projectVO.ProjectCode);
    //objStartDate.val(new Date(projectVO.StartDate).format("yyyy-MM-dd"));
    //objEndDate.val(new Date(projectVO.EndDate).format("yyyy-MM-dd"));
    objStartDate.datepicker("setDate", new Date(projectVO.StartDate));
    objEndDate.datepicker("setDate", new Date(projectVO.EndDate));
    objCommission.val(projectVO.Commission);
    objCost.val(projectVO.Cost);
    if (projectVO.CommissionType == 1)
        objDecimal.attr("checked", true);
    else
        objPer.attr("checked", true);
    if (projectVO.Status == 0)
        objStatus.val("已生成");
    else if (projectVO.Status == 1)
        objStatus.val("已委托");
    else if (projectVO.Status == 2)
        objStatus.val("已完工");
    else if (projectVO.Status == 3)
        objStatus.val("申请完工");
    else if (projectVO.Status == 4)
        objStatus.val("申请退款");
    else if (projectVO.Status == 5)
        objStatus.val("退款完成");

    SetButton(projectVO.Status);
}

function SetButton(status) {
    var divStepObj = $("#divStep");
    var divStep0Obj = divStepObj.find("li[data-target='#step0']");
    var divStep1Obj = divStepObj.find("li[data-target='#step1']");
    var divStep2Obj = divStepObj.find("li[data-target='#step2']");
    var divStep3Obj = divStepObj.find("li[data-target='#step3']");
    
    var divCommissionDelegate = $("#divCommissionDelegate");
    var divWorking = $("#divWorking");
    var divWorkEnd = $("#divWorkEnd");


    divStepObj.find("li").removeClass("complete");
    divStepObj.find("li").removeClass("active");
   
    divCommissionDelegate.hide();
    divWorking.hide();
    divWorkEnd.hide();   

    if (status == 0) {
        //已生成(待委托)
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("active");       
        divCommissionDelegate.show();

    } else if (status == 1) {
        //已委托（工作中）
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("active");
        divWorking.show(); 
    }
    else if (status == 2) {
        //已完成
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("complete");
        divWorking.show();
        divWorkEnd.show();
    }
    else if (status == 3) {
        //申请完工
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("active");
        divWorking.show();        
    } else {

    }
}

function BindCity(provinceId, cityId) {
    var objProvince = $("select[id*='drpProvince']");
    var objCity = $("select[id*='drpCity']");

    objProvince.val(provinceId);

    objCity.empty();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + provinceId + "&enable=true",
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var childList = data.Result;
                for (var i = 0; i < childList.length; i++) {
                    objCity.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
                }

                if (childList.length > 0 && cityId > 0)
                    objCity.val(cityId);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function BindCategory(parentCategoryId, categoryId) {
    var objParentCategory = $("select[id*='drpCategory1']");
    var objCategory = $("select[id*='drpCategory2']");

    objParentCategory.val(parentCategoryId);

    objCategory.empty();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + parentCategoryId + "&enable=true",
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var childList = data.Result;
                for (var i = 0; i < childList.length; i++) {
                    objCategory.append("<option value=\"" + childList[i].CategoryId + "\">" + childList[i].CategoryName + "</option>");
                }

                if (childList.length > 0 && categoryId > 0)
                    objCategory.val(categoryId);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function Init() {

    var projectId = parseInt($("#" + hidProjectId).val());

    load_show();
    //bind province
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var provinceVOList = data.Result;
                var objProvince = $("select[id*='drpProvince']");
                objProvince.find("option").remove();

                for (var i = 0; i < provinceVOList.length; i++) {
                    objProvince.append("<option value='" + provinceVOList[i].ProvinceId + "'>" + provinceVOList[i].ProvinceName + "</option>");
                }

                if (projectId < 1 && provinceVOList.length > 0) {
                    BindCity(provinceVOList[0].ProvinceId, 0);
                }
            }
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });

    //bind parent category
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetParentCategoryList?enable=true",
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var parentCategoryVOList = data.Result;
                var objParentCategory = $("select[id*='drpCategory1']");
                objParentCategory.find("option").remove();

                for (var i = 0; i < parentCategoryVOList.length; i++) {
                    objParentCategory.append("<option value='" + parentCategoryVOList[i].CategoryId + "'>" + parentCategoryVOList[i].CategoryName + "</option>");
                }

                if (projectId < 1 && parentCategoryVOList.length > 0) {
                    BindCategory(parentCategoryVOList[0].CategoryId, 0);
                }
            }
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });


    if (projectId > 0) {
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
                    BindProjectAction(projectId);
                    BindProjectCommission(projectId);
                    BindFile(projectId);
                    //BindReview(projectId);
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

    BindCity(requirementVO.ProvinceId, requirementVO.CityId);
    BindCategory(requirementVO.ParentCategoryId, requirementVO.CategoryId);
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

function initDatePicker() {
    $('.date-picker').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
    $('.timepicker1').timepicker({
        minuteStep: 1,
        defaultTime: false,
        showSeconds: true,
        showMeridian: false, showWidgetOnAddonClick: false
    }).next().on("click", function () {
        $(this).prev().focus();
    });

    $('.date-picker-yyyy').datepicker({
        minViewMode: 'years',
        format: 'yyyy',
        autoclose: true,
        startViewMode: 'year',
        startDate: '1900',
        endDate: '2100',
        language: 'zh-CN'
    })
    .next().on("click", function () {
        $(this).prev().focus();
    });
}

function BindReview(projectId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetAgencyReviewByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0)
                    $("#btn_NewReview").hide();
                else
                    $("#btn_NewReview").show();
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    var txtReviewDescription = $("textarea[id*='txtReviewDescription']");
                    txtReviewDescription.val(puVO.Description);
                    var detailList = puVO.AgencyReviewDetailList;
                    var divReviewList = $(".xzw_starBox");
                    for (var i = 0; i < divReviewList.length; i++) {
                        $(divReviewList[i]).find(".showb").css({ "width": 30 * detailList[i].Score });
                    }
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

function BindProjectAction(projectId) {
    $("#ProjectActionList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectActionByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddProjectAction(puVO);
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

function AddProjectAction(puVO) {
    var projectActionTable = $("#ProjectActionList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    //oTR += "  <td class=\"center\"> \r\n";
    //oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    //oTR += "    <input type=\"hidden\" value=\"ProjectAction_" + puVO.ProjectId + "_" + puVO.ProjectActionId + "\" /> \r\n";
    //oTR += "  </td> \r\n";
    //oTR += "  <td class=\"center\" title=\"" + puVO.ActionBy + "\">" + puVO.ActionBy + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ActionDate.replace("T", " ") + "\">" + puVO.ActionDate.replace("T", " ") + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Description + "</td> \r\n";
    oTR += "</tr> \r\n";

    projectActionTable.append(oTR);
}


function BindProjectCommission(projectId) {
    $("#ProjectCommissionList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectCommissionByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddProjectCommission(puVO);
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

    //查看是否有申请付款内容，如果有，不显示申请付款按钮
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetLatestProjectCommissionByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            var btnNewProjectCommission = $("#btn_NewProjectCommission");
            var divProjectCommissionObj = $("#divProjectCommissionInfo");
            if (data.Flag == 1) {
                btnNewProjectCommission.hide();
                divProjectCommissionObj.show();
                var puVO = data.Result;
                var hidProjectCommissionIdObj = $("#hidProjectCommissionId");
                var projectCommissionObj = $("#lblProjectCommission");
                var reasonObj = $("#txtReason");

                hidProjectCommissionIdObj.val(puVO.ProjectCommissionId);
                projectCommissionObj.html(puVO.Commission + " 元");
                reasonObj.html(puVO.Reason);

            } else {
                btnNewProjectCommission.show();
                divProjectCommissionObj.hide();
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}

function AddProjectCommission(puVO) {
    var projectCommissionTable = $("#ProjectCommissionList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    //oTR += "  <td class=\"center\"> \r\n";
    //oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    //oTR += "    <input type=\"hidden\" value=\"ProjectAction_" + puVO.ProjectId + "_" + puVO.ProjectActionId + "\" /> \r\n";
    //oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CreatedAt.replace("T", " ") + "\">" + puVO.CreatedAt.replace("T", " ") + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Commission + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Reason + "</td> \r\n";
    if (puVO.Status == 1) {
        oTR += "  <td class=\"center\" title=\"\">申请中</td> \r\n";
    } else if (puVO.Status == 2) {
        oTR += "  <td class=\"center\" title=\"\">已拒绝</td> \r\n";
    } else if (puVO.Status == 3) {
        oTR += "  <td class=\"center\" title=\"\">已完成</td> \r\n";
    } else {
        oTR += "  <td class=\"center\" title=\"\"></td> \r\n";
    }
    oTR += "  <td class=\"center\" title=\"\">" + puVO.RejectReason + "</td> \r\n";

    oTR += "</tr> \r\n";

    projectCommissionTable.append(oTR);
}

function UpdateProjectCommission(status) {
    var projectCommissionObj = $("#hidProjectCommissionId");
    var rejectReasonObj = $("textarea[id*='txtRejectReason']");

    if (status == 2) {
        //判断拒绝理由
        if (rejectReasonObj.val() == "") {
            bootbox.dialog({
                message: "请输入拒绝原因",
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
            return false;
        }
    }
    var projectCommissionVO = new Object();

    projectCommissionVO.ProjectCommissionId = projectCommissionObj.val();
    projectCommissionVO.Status = status;
    projectCommissionVO.RejectReason = rejectReasonObj.val();
    projectCommissionVO.ProjectId = parseInt($("#" + hidProjectId).val());

    $.ajax({
        url: _RootPath + "SPWebAPI/Project/UpdateProjectCommissionStatus?token=" + _Token,
        type: "POST",
        data: projectCommissionVO,
        success: function (data) {
            if (data.Flag == 1) {

                BindProjectCommission(parseInt($("#" + hidProjectId).val()));
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
    return false;
}

function BindFile(projectId) {
    $("#FileList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectFileByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddFile(puVO);
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

function AddFile(puVO) {
    var fileTable = $("#FileList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    //oTR += "  <td class=\"center\"> \r\n";   
    //oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    //oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CreatedDate.replace("T", " ") + "\">" + puVO.CreatedDate.replace("T", " ") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}
