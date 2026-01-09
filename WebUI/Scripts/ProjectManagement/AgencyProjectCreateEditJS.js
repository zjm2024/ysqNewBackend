$(document).ready(function () {
    Init();
    initDatePicker();

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtStartDate: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtEndDate: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: true,
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtStartDate: {
                required: "请输入开始时间！"
            },
            ctl00$ContentPlaceHolder_Content$txtEndDate: {
                required: "请输入结束时间！"
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: "请输入合同金额！",
                number: "请输入数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                required: "请输入酬金！"
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

        var projectVO = GetProjectVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateProject?token=" + _Token,
            type: "POST",
            data: projectVO,
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
                console.log(data);
            }
        });
        return false;
    });

    $("#btn_commissiondelegate").click(function () {
        var objCommission = $("input[id*='txtCommission']");
        var commissionDelegationVO = new Object();
        commissionDelegationVO.CustomerId = _CustomerId;
        commissionDelegationVO.Commission = objCommission.val();
        commissionDelegationVO.ProjectId = parseInt($("#" + hidProjectId).val());
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateCommissionDelegation?token=" + _Token,
            type: "POST",
            data: commissionDelegationVO,
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
                                    SetButton(1);
                                }
                            }
                        }
                    });
                } else if (data.Flag == 2) {
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    window.location.href = ("../../Pay/CustomerRechange.aspx");
                                    //跳转到充值页面
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
                console.log(data);
            }
        });
        return false;
    });

    $("#btn_NewAction").click(function () {
        onAddProjectAction();
        return false;
    });

    $("#btn_newChange_Decide").click(function () {
        bootbox.dialog({
            message: "是否同意更改酬金！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        newChange(1);
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
    });

    $("#btn_newChange_cancel").click(function () {
        bootbox.dialog({
            message: "是否拒绝更改酬金！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        newChange(2);
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
    });

    $("#btn_completeproject").click(function () {
        var projectId = parseInt($("#" + hidProjectId).val());
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/CompleteProject?projectId=" + projectId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    BindProjectAction(projectId);
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    SetButton(3);
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
                console.log(data);
            }
        });
        return false;
    });

    $("#btn_NewReview").click(function () {
        var projectId = parseInt($("#" + hidProjectId).val());
        var txtReviewDescription = $("textarea[id*='txtReviewDescription']");

        var businessReviewModelVO = new Object();
        var businessReview = new Object();
        var businessReviewDetailList = new Array();

        businessReviewModelVO.BusinessReview = businessReview;
        businessReviewModelVO.BusinessReviewDetail = businessReviewDetailList;

        businessReview.ProjectId = projectId;
        businessReview.Description = txtReviewDescription.val();

        var divReviewList = $(".xzw_starBox");
        for (var i = 0; i < divReviewList.length; i++) {
            var businessReviewDetailVO = new Object();
            businessReviewDetailList.push(businessReviewDetailVO);

            businessReviewDetailVO.ReviewType = i + 1;
            businessReviewDetailVO.Score = $(divReviewList[i]).find(".showb").css("width").replace("px", "") / 30;
        }

        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateBusinessReview?token=" + _Token,
            type: "POST",
            data: businessReviewModelVO,
            success: function (data) {
                if (data.Flag == 1) {
                    $("#btn_NewReview").hide();
                    $("#btn_AddNoteReview").show();
                    $("#divAddNote").show();
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
                console.log(data);
            }
        });
        return false;
    });

});
function newChange(Status) {
    var projectChangeVO = new Object();

    projectChangeVO.ProjectChangeId = $("input[id*='ProjectChangeId']").val();
    projectChangeVO.Status = Status;
    projectChangeVO.RejectReason = "";
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/UpdateProjectChangeStatus?token=" + _Token,
        type: "POST",
        data: projectChangeVO,
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
                                window.location.reload();
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
            console.log(data);
        }
    });
}

function GetProjectVO() {
    var projectVO = new Object();

    //var objProjectCode = $("input[id*='txtProjectCode']");
    //var objRequirementId = $("input[id*='txtRequirementId']");
    //var objCustomerId = $("input[id*='txtCustomerId']");
    var objStartDate = $("input[id*='txtStartDate']");
    var objEndDate = $("input[id*='txtEndDate']");
    var objCommission = $("input[id*='txtCommission']");
    var objCost = $("input[id*='txtCost']");
    //var objStatus = $("input[id*='txtStatus']");
    var objDecimal = $("input[id*='radDecimal']:checked");

    projectVO.ProjectId = parseInt($("#" + hidProjectId).val());
    //projectVO.ProjectCode = objProjectCode.val();
    //projectVO.RequirementId = objRequirementId.val();
    //projectVO.CustomerId = objCustomerId.val();
    projectVO.StartDate = objStartDate.val();
    projectVO.EndDate = objEndDate.val();
    projectVO.Commission = objCommission.val();
    projectVO.Cost = objCost.val();
    if (objDecimal.length > 0)
        projectVO.CommissionType = 1;
    else
        projectVO.CommissionType = 2;

    return projectVO;
}

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
    if(new Date(projectVO.StartDate).format("yyyy-MM-dd") != "1900-01-01")
        objStartDate.datepicker("setDate", new Date(projectVO.StartDate));
    if (new Date(projectVO.EndDate).format("yyyy-MM-dd") != "1900-01-01")
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

    //绑定剩余酬金
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetRemainCommissionByProject?projectId=" + projectVO.ProjectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var lblRemainObj = $("span[id*='lblRemainCommission']");
                lblRemainObj.html(data.Result);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });

    //绑定酬金更改
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectChange?projectId=" + projectVO.ProjectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var lblRemainObj = $("span[id*='newChange']");
                lblRemainObj.html(data.Result.Commission);
                $("input[id*='ProjectChangeId']").val(data.Result.ProjectChangeId);
            } else {
                $(".newChange").hide();
            }
        },
        error: function (data) {
            console.log(data);
        }
    });

    //绑定申请关闭项目
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectRefund?projectId=" + projectVO.ProjectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                $("input[id*='ProjectRefundId']").val(data.Result.ProjectRefundId);
                bootbox.dialog({
                    message: "雇主申请关闭项目，如果你7天之内没有同意或拒绝操作，雇主将有权单方面关闭项目。",
                    buttons:
                    {
                        "click":
                        {
                            "label": "同意关闭",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                bootbox.dialog({
                                    message: "关闭项目，你将拿不到剩余酬金。请确定是否要关闭",
                                    buttons:
                                    {
                                        "click":
                                        {
                                            "label": "确定",
                                            "className": "btn-sm btn-primary",
                                            "callback": function () {
                                                ProjectRefund(1);
                                            }
                                        },
                                        "Cancel":
                                        {
                                            "label": "取消",
                                            "className": "btn-sm",
                                            "callback": function () {
                                                ProjectRefund(2);
                                            }
                                        }
                                    }
                                });
                            }
                        },
                        "Cancel":
                        {
                            "label": "拒绝关闭",
                            "className": "btn-sm",
                            "callback": function () {
                                ProjectRefund(2);
                            }
                        }
                    }
                });
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function ProjectRefund(Status) {
    var projectrefundVO = new Object();

    projectrefundVO.ProjectRefundId = $("input[id*='ProjectRefundId']").val();
    projectrefundVO.Status = Status;
    projectrefundVO.RejectReason = "";
    console.log(projectrefundVO);
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/UpdateProjectRefundStatus?token=" + _Token,
        type: "POST",
        data: projectrefundVO,
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
                                window.location.reload();
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
            console.log(data);
        }
    });
}
function SetButton(status) {

    $("#hidProjectStatus").val(status);

    var divStepObj = $("#divStep");
    var divStep0Obj = divStepObj.find("li[data-target='#step0']");
    var divStep1Obj = divStepObj.find("li[data-target='#step1']");
    var divStep2Obj = divStepObj.find("li[data-target='#step2']");
    var divStep3Obj = divStepObj.find("li[data-target='#step3']");
    var divStep4Obj = divStepObj.find("li[data-target='#step4']");
    var divStep5Obj = divStepObj.find("li[data-target='#step5']");
    var divStep6Obj = divStepObj.find("li[data-target='#step6']");
    var divStep7Obj = divStepObj.find("li[data-target='#step7']");

    var btnSave = $("#btn_save");
    var btnCommissionDelegate = $("#btn_commissiondelegate");
    var btnCompleteProject = $("#btn_completeproject");
    //var btnComfirmProject = $("#btn_comfirmproject");

    var divCommissionDelegate = $("#divCommissionDelegate");
    var divWorking = $("#divWorking");
    var divWorkEnd = $("#divWorkEnd");

    var btnNewAction = $("#btn_NewAction");
    var btnDeleteAction = $("#btn_DeleteAction");
    var btnNewProjectCommission = $("#btn_NewProjectCommission");
    //var btnRejectProjectCommission = $("#btn_RejectProjectCommission");
    var btnNewComlaints = $("#btn_NewComlaints");


    divStepObj.find("li").removeClass("complete");
    divStepObj.find("li").removeClass("active");
    btnSave.hide();
    btnCommissionDelegate.hide();
    btnCompleteProject.hide();
    //btnComfirmProject.hide();
    divCommissionDelegate.hide();
    divWorking.hide();
    divWorkEnd.hide();
    btnNewAction.hide();
    btnDeleteAction.hide();
    btnNewProjectCommission.hide();
    //btnRejectProjectCommission.hide();
    //btnNewComlaints.hide();
    var projectId = parseInt($("#" + hidProjectId).val());
    if (status == 0) {
        //已生成(待委托)
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("active");
        //btnSave.show();
        //btnCommissionDelegate.show();
        //divCommissionDelegate.show();

    } else if (status == 1) {
        //已委托（工作中）
        //判断是否有阶段支付，如果有显示阶段支付，没有则显示工作中

        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("complete");
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetLatestProjectCommissionByProject?projectId=" + projectId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    divStep4Obj.addClass("complete");
                    divStep5Obj.addClass("active");

                } else {
                    divStep4Obj.addClass("active");
                }
                load_hide();
            },
            error: function (data) {
                console.log(data);
            }
        });
        divWorking.show();
        btnNewAction.show();
        btnDeleteAction.show();
        btnNewProjectCommission.show();
        //btnRejectProjectCommission.show();
        btnCompleteProject.show();

    }
    else if (status == 2) {
        //已完成
        //判断是否都已经评价，都评价了则全部complete
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("complete");
        divStep4Obj.addClass("complete");
        divStep5Obj.addClass("complete");
        divStep6Obj.addClass("complete");
        var isAgencyReview = false;
        var isBusinessReview = false;
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetAgencyReviewByProject?projectId=" + projectId + "&token=" + _Token,
            type: "Get",
            async: false,
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var puVOList = data.Result;
                    if (puVOList.length > 0) {
                        isAgencyReview = true;
                    }

                } else {

                }
                load_hide();
            },
            error: function (data) {
                //console.log(data);
            }
        });

        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetBusinessReviewByProject?projectId=" + projectId + "&token=" + _Token,
            type: "Get",
            data: null,
            async: false,
            success: function (data) {
                if (data.Flag == 1) {
                    var puVOList = data.Result;
                    if (puVOList.length > 0) {
                        isBusinessReview = true;
                    }
                    else {

                    }
                }
            },
            error: function (data) {
                //console.log(data);
            }
        });
        if (isAgencyReview && isBusinessReview) {
            divStep7Obj.addClass("complete");
        } else {
            divStep7Obj.addClass("active");
        }
        divWorking.show();
        divWorkEnd.show();
    }
    else if (status == 3) {
        //申请完工
        divStep0Obj.addClass("complete");
        divStep1Obj.addClass("complete");
        divStep2Obj.addClass("complete");
        divStep3Obj.addClass("complete");
        divStep4Obj.addClass("complete");
        divStep5Obj.addClass("complete");
        divStep6Obj.addClass("active");
        divWorking.show();
        //divWorkEnd.show();
        //btnComfirmProject.show();
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
            console.log(data);
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
            console.log(data);
        }
    });
}

function Init() {
    $("input[name='id-input-file']").ace_file_input({
        no_file: '请选择 ...',
        btn_choose: '选择',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false
    });
    $(".ace-file-input").attr("style", "width: 41.6666%;");

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
            console.log(data);
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
            console.log(data);
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
                    BindRequire(projectId);
                    BindAgency(projectVO.AgencyId);
                    BindProjectAction(projectId);
                    BindProjectCommission(projectId);
                    BindFile(projectId);
                    BindReportFile(projectId);
                    BindReview(projectId);
                    BindComplaints(projectId);
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
                console.log(data);
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

function BindRequire(ProjectId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequirementcopies?ProjectId=" + ProjectId + "&token=" + _Token,
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
            console.log(data);
            load_hide();
        }
    });
}

function SetAgency(agencyVO) {

    var objAgencyName = $("label[id*='lblAgencyName']"); 
    var objAge = $("label[id$='lblAge']");
    var objAgencyPhone = $("input[id*='txtAgencyPhone']");
    var objAgencyLevel = $("select[id*='drpAgencyLevel']");


    if (agencyVO.Sex == 1) {
        objAgencyName.html(agencyVO.AgencyName + " 先生");
    } else {
        objAgencyName.html(agencyVO.AgencyName + " 小姐");
    }
    if (agencyVO.Birthday != "" && new Date(agencyVO.Birthday).format("yyyy-MM-dd") != "1900-01-01") {
        objAge.html(new Date().getFullYear() - new Date(agencyVO.Birthday).getFullYear());
    }
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
            console.log(data);
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
        url: _RootPath + "SPWebAPI/Project/GetBusinessReviewByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    $("#btn_NewReview").hide();
                    var txtReviewDescription = $("textarea[id*='txtReviewDescription']");
                    txtReviewDescription.css("border", "0px solid #d7d3d3");
                    txtReviewDescription.attr("disabled", "disabled");
                    $("#btn_AddNoteReview").show();
                    $("#divAddNote").show();
                }
                else {
                    $("#btn_NewReview").show();
                    $("#btn_AddNoteReview").hide();
                    $("#divAddNote").hide();
                    $("#divExplanation").hide();
                }
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    var txtReviewDescription = $("textarea[id*='txtReviewDescription']");
                    txtReviewDescription.val(puVO.Description);

                    if (puVO.AddNote != "") {
                        var txtReviewAddNote = $("textarea[id*='txtReviewAddNote']");
                        txtReviewAddNote.css("border", "0px solid #d7d3d3");
                        txtReviewAddNote.attr("disabled", "disabled");
                        txtReviewAddNote.val(puVO.AddNote);
                        $("#btn_AddNoteReview").hide();
                    } else {
                        $("#btn_AddNoteReview").show();
                    }

                    if (puVO.Explanation != "") {
                        $("#divExplanation").show();
                        $("#divBusinessExplanation").html(puVO.Explanation);
                    } else {
                        $("#divExplanation").hide();
                    }

                    var detailList = puVO.BusinessReviewDetailList;
                    var divReviewList = $("#divReviewBusiness").find(".xzw_starBox");
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
            console.log(data);
        }
    });

    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetAgencyReviewByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    $("#divReviewAgency").show();
                    for (var i = 0; i < puVOList.length; i++) {
                        var puVO = puVOList[i];
                        $("#divBusinessReviewDescription").html(puVO.Description);
                        if (puVO.AddNote != "") {
                            $("#divBusinessAddNote").show();
                            $("#divBusinessReviewAddNote").html(puVO.AddNote);
                        } else {
                            $("#divBusinessAddNote").hide();
                        }                        

                        var detailList = puVO.AgencyReviewDetailList;
                        var divReviewList = $("#divReviewAgency").find(".xzw_starBox");
                        for (var i = 0; i < divReviewList.length; i++) {
                            $(divReviewList[i]).find(".showb").css({ "width": 30 * detailList[i].Score });
                        }
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
            console.log(data);
        }
    });
}

function AddNoteReview() {
    var objNote = $("textarea[id*='txtReviewAddNote']");
    if (objNote.val() == "") {
        bootbox.dialog({
            message: "请输入追评内容！",
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
    var projectId = parseInt($("#" + hidProjectId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetBusinessReviewByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                var puVO = puVOList[0];
                var businessReviewVO = new Object();

                businessReviewVO.BusinessReviewId = puVO.BusinessReviewId;
                businessReviewVO.AddNote = objNote.val();

                $.ajax({
                    url: _RootPath + "SPWebAPI/Project/PlusBusinessReview?token=" + _Token,
                    type: "POST",
                    data: businessReviewVO,
                    success: function (data) {
                        if (data.Flag == 1) {
                            objNote.css("border", "0px solid #d7d3d3");
                            objNote.attr("disabled", "disabled");
                            $("#btn_AddNoteReview").hide();
                            bootbox.dialog({
                                message: "追加成功！",
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
                        console.log(data);
                    }
                });
            }
        },
        error: function (data) {
            console.log(data);
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
            console.log(data);
        }
    });
}

function AddProjectAction(puVO) {
    var projectActionTable = $("#ProjectActionList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"ProjectAction_" + puVO.ProjectId + "_" + puVO.ProjectActionId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    //oTR += "  <td class=\"center\" title=\"" + puVO.ActionBy + "\">" + puVO.ActionBy + "</td> \r\n";
    if (puVO.ActionDate instanceof Date) {
        oTR += "  <td class=\"center\" title=\"" + puVO.ActionDate.format("yyyy-MM-dd hh:mm:ss") + "\">" + puVO.ActionDate.format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    } else {
        oTR += "  <td class=\"center\" title=\"" + puVO.ActionDate.replace("T", " ") + "\">" + puVO.ActionDate.replace("T", " ") + "</td> \r\n";
    }
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Description + "</td> \r\n";
    oTR += "  <td class=\"center\" > \r\n";
    for (var i = 0; i < puVO.ProjectActionFileList.length; i++) {
        oTR += "<a target=\"_blank\" href=\"" + puVO.ProjectActionFileList[i].FilePath + "\">" + puVO.ProjectActionFileList[i].FileName + "</a>";
    }
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    projectActionTable.append(oTR);
}

function DeleteProjectAction() {
    var chkList = $("#ProjectActionList").find("input[type='checkbox']:checked");

    if (chkList.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        var idString = "";
                        for (var i = 0; i < chkList.length; i++) {
                            var chkObj = $(chkList[i]);
                            var projectActionId = chkObj.next().val().split('_')[2];

                            if (projectActionId != "-1") {
                                if (idString != "")
                                    idString += ',';
                                idString += projectActionId;
                            }
                        }
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Project/DeleteProjectAction?projectActionIds=" + idString + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Flag == 1)
                                    chkList.parent().parent().remove();
                                else {
                                    bootbox.dialog({
                                        message: data.Message,
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
                            },
                            error: function (data) {
                                console.log(data);
                            }
                        });

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
    return false;
}

function onAddProjectAction() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/ProjectManagement\/ProjectActionCreateEdit.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "添加进度",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtDescription']");

                    if (txtDescriptionObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入跟进内容?",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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

                    var projectActionVO = new Object();
                    projectActionVO.ActionBy = _CustomerId;
                    projectActionVO.ProjectId = parseInt($("#" + hidProjectId).val());
                    projectActionVO.Description = txtDescriptionObj.val();
                    projectActionVO.ActionDate = new Date();
                    projectActionVO.ProjectActionId = -1;

                    var projectActionFileList = new Array();
                    projectActionVO.ProjectActionFileList = projectActionFileList;

                    var puhidenObjList = $(window.frames["iframe_1"].document).find("table[id*='FileList']").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {

                        var projectActionFileVO = new Object();
                        projectActionFileList.push(projectActionFileVO);


                        var puhidenObj = $(puhidenObjList[i]);
                        var puValue = puhidenObj.val();
                        var projectActionId = puValue.split("_")[1];
                        var projectActionFileId = puValue.split("_")[2];

                        projectActionFileVO.ProjectActionId = projectActionId;
                        projectActionFileVO.ProjectActionFileId = projectActionFileId;
                        projectActionFileVO.FileName = puhidenObj.parent().next().find("a").html();
                        projectActionFileVO.FilePath = puhidenObj.parent().next().find("a").attr("href");
                    }

                    $.ajax({
                        url: _RootPath + "SPWebAPI/Project/UpdateProjectAction?token=" + _Token,
                        type: "POST",
                        data: projectActionVO,
                        success: function (data) {
                            if (data.Flag == 1) {
                                AddProjectAction(projectActionVO);
                            }
                        },
                        error: function (data) {
                            console.log(data);
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

function BindComplaints(projectId) {
    $("#ProjectComlaintsList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetComplaintsByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    $("#divComplaints").show();
                } else {
                    $("#divComplaints").hide();
                }
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddComplaints(puVO);
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
            console.log(data);
        }
    });

    //判断是否申请过维权
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/IsExistsComplaints?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var isExists = data.Result;
                if (isExists) {
                    $("#div_NewComplaints").hide();
                } else {
                    $("#div_NewComplaints").show();
                }

            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function AddComplaints(puVO) {
    var projectComplaintsTable = $("#ProjectComlaintsList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    //oTR += "  <td class=\"center\"> \r\n";
    //oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    //oTR += "    <input type=\"hidden\" value=\"ProjectAction_" + puVO.ProjectId + "_" + puVO.ComplaintsId + "\" /> \r\n";
    //oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CustomerName + "\">" + puVO.CustomerName + "</td> \r\n";
    if (puVO.CreatedAt instanceof Date) {
        oTR += "  <td class=\"center\" title=\"" + puVO.CreatedAt.format("yyyy-MM-dd hh:mm:ss") + "\">" + puVO.CreatedAt.format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    } else {
        oTR += "  <td class=\"center\" title=\"" + puVO.CreatedAt.replace("T", " ") + "\">" + puVO.CreatedAt.replace("T", " ") + "</td> \r\n";
    }
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Description + "</td> \r\n";
    var statusName = "已提交";
    switch (puVO.Status) {
        case 0:
            statusName = "已提交";
            break;
        case 1:
            statusName = "已跟进";
            break;
        case 2:
            statusName = "已处理";
            break;
    }
    oTR += "  <td class=\"center\" title=\"\">" + statusName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"\">" + puVO.Reason + "</td> \r\n";
    oTR += "</tr> \r\n";

    projectComplaintsTable.append(oTR);
}


function NewComplaints() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/ProjectManagement\/ComplaintsCreateEdit.aspx" height="400px" width="100%" frameborder="0"><\/iframe>',

        title: "维权申请",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtDescription']");                   

                    if (txtDescriptionObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入投诉内容！",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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

                    var complaintsModel = new Object();

                    var puVO = new Object();
                    complaintsModel.Complaints = puVO;
                    puVO.ComplaintsId = -1;
                    puVO.ProjectId = parseInt($("#" + hidProjectId).val());
                    puVO.Creator = _CustomerId;
                    puVO.CustomerName = _CustomerName;
                    puVO.Description = txtDescriptionObj.val();
                    puVO.Status = 0;
                    puVO.Reason = "";
                    puVO.CreatedAt = new Date();
                    
                    var complaintsImgList = new Array();
                    complaintsModel.ComplaintsImg = complaintsImgList;

                    var puhidenObjList = $(window.frames["iframe_1"].document).find("table[id*='FileList']").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {

                        var complaintsImgVO = new Object();
                        complaintsImgList.push(complaintsImgVO);

                        var puhidenObj = $(puhidenObjList[i]);
                        var puValue = puhidenObj.val();
                        var complaintsId = puValue.split("_")[1];
                        var complaintsImgId = puValue.split("_")[2];

                        complaintsImgVO.ComplaintsId = complaintsId;
                        complaintsImgVO.ComplaintsImgId = complaintsImgId;
                        complaintsImgVO.ImageName = puhidenObj.parent().next().find("a").html();
                        complaintsImgVO.ImagePath = puhidenObj.parent().next().find("a").attr("href");
                    }                    

                    $.ajax({
                        url: _RootPath + "SPWebAPI/Project/UpdateComplaints?&token=" + _Token,
                        type: "POST",
                        data: complaintsModel,
                        success: function (data) {
                            if (data.Flag == 1) {
                                AddComplaints(puVO);
                                //只维权一次，隐藏按钮
                                $("#div_NewComplaints").hide();
                                $("#divComplaints").show();
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
                            console.log(data);
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
            console.log(data);
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
            var status = $("#hidProjectStatus").val();
            if (data.Flag == 1) {
                if (status == 1) {
                    btnNewProjectCommission.hide();
                    divProjectCommissionObj.show();
                }
                var puVO = data.Result;
                var projectCommissionObj = $("#lblProjectCommission");
                var reasonObj = $("#txtReason");

                projectCommissionObj.html(puVO.Commission + " 元");
                reasonObj.html(puVO.Reason);

            } else {
                if (status == 1) {
                    btnNewProjectCommission.show();
                    divProjectCommissionObj.hide();
                }
            }
            load_hide();
        },
        error: function (data) {
            console.log(data);
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

function onAddProjectCommission() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/ProjectManagement\/ProjectCommissionCreateEdit.aspx?ProjectId=' + $("#" + hidProjectId).val() + '" height="220px" width="100%" frameborder="0"><\/iframe>',

        title: "付款申请",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    //if (!$(window.frames["iframe_1"].document).find('#form1').valid()) {
                    //    return false;
                    //}

                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtReason']");
                    var txtProjectCommissionObj = $(window.frames["iframe_1"].document).find("input[id*='txtCommission']");

                    if (txtProjectCommissionObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入申请金额！",
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
                        return false;
                    }
                    if (txtDescriptionObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入付款理由！",
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
                        return false;
                    }


                    var projectCommissionVO = new Object();
                    projectCommissionVO.CreatedBy = _CustomerId;
                    projectCommissionVO.ProjectId = parseInt($("#" + hidProjectId).val());
                    projectCommissionVO.Reason = txtDescriptionObj.val();
                    projectCommissionVO.Commission = txtProjectCommissionObj.val();
                    projectCommissionVO.Status = 1;
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Project/UpdateProjectCommission?token=" + _Token,
                        type: "POST",
                        data: projectCommissionVO,
                        success: function (data) {
                            if (data.Flag == 1) {
                                //刷新付款信息
                                BindProjectCommission(parseInt($("#" + hidProjectId).val()));
                            } else {
                                bootbox.dialog({
                                    message: data.Message,
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
                        },
                        error: function (data) {
                            console.log(data);
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

function changefile(uploadId) {
    var tempPath = new Date().format("yyyyMM");
    load_show();
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var projectFileVO = new Object();

            projectFileVO.ProjectId = parseInt($("#" + hidProjectId).val());
            projectFileVO.FileName = data.Result.FileName;
            projectFileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
            projectFileVO.CreatedDate = new Date();
            projectFileVO.CreatedBy = _CustomerId;

            //直接保存
            $.ajax({
                url: _RootPath + "SPWebAPI/Project/UpdateProjectFile?token=" + _Token,
                type: "POST",
                data: projectFileVO,
                success: function (data) {
                    if (data.Flag == 1) {
                        projectFileVO.ProjectFileId = data.Result;
                        AddFile(projectFileVO);
                        bootbox.dialog({
                            message: "上传成功!",
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
                    load_hide();
                },
                error: function (data) {
                    console.log(data);
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
    });
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
            console.log(data);
        }
    });
}

function AddFile(puVO) {
    var fileTable = $("#FileList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    if (puVO.CreatedBy == _CustomerId)
        oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"File_" + puVO.ProjectId + "_" + puVO.ProjectFileId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function DeleteFile() {
    var chkList = $("#FileList").find("input[type='checkbox']:checked");

    if (chkList.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        var idString = "";
                        for (var i = 0; i < chkList.length; i++) {
                            var chkObj = $(chkList[i]);
                            var projectFileId = chkObj.next().val().split('_')[2];

                            if (projectFileId != "-1") {
                                if (idString != "")
                                    idString += ',';
                                idString += projectFileId;
                            }
                        }
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Project/DeleteProjectFile?projectFileIds=" + idString + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Flag == 1)
                                    chkList.parent().parent().remove();
                                else {
                                    bootbox.dialog({
                                        message: data.Message,
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
                            },
                            error: function (data) {
                                console.log(data);
                            }
                        });
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




function changereportfile(uploadId) {
    var tempPath = new Date().format("yyyyMM");
    load_show();
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var projectReportFileVO = new Object();
            var objddlreportType = $("select[id*='ddlreportType']");
            var objtxtReportDesc = $("textarea[id*='txtReportDesc']");
      
            projectReportFileVO.ProjectId = parseInt($("#" + hidProjectId).val());
            projectReportFileVO.ReportFileName = data.Result.FileName;
            projectReportFileVO.ReportFilePath = data.Result.FilePath.replace("~", _APIURL);
            projectReportFileVO.Description =objtxtReportDesc.val();
            projectReportFileVO.ReportTypeId = parseInt(objddlreportType.val());
            projectReportFileVO.CreatedDate = new Date();
            projectReportFileVO.CreatedBy = _CustomerId;

            //直接保存
            $.ajax({
                url: _RootPath + "SPWebAPI/Project/UpdateProjectReportFile?token=" + _Token,
                type: "POST",
                data: projectReportFileVO,
                success: function (data) {
                    if (data.Flag == 1) {
                        projectReportFileVO.ProjectReportFileId = data.Result;
                        AddReportFile(projectReportFileVO);
                        bootbox.dialog({
                            message: "上传成功!",
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
                    load_hide();
                },
                error: function (data) {
                    console.log(data);
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
    });
}

function BindReportFile(projectId) {
    $("#reportList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetProjectReportFileByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddReportFile(puVO);
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
            console.log(data);
        }
    });
}

function AddReportFile(puVO) {
    var fileTable = $("#reportList");
    var rtype = "周报";
    var description = puVO.Description;
    if (puVO.ReportTypeId == 2)
        rtype = "月报"
    if (puVO.Description.length > 50)
        description = puVO.Description.substring(0, 49);

    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    if (puVO.CreatedBy == _CustomerId)
        oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"File_" + puVO.ProjectId + "_" + puVO.ProjectReportFileId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ReportFileName + "\"><a target=\"_blank\" href=\"" + puVO.ReportFilePath + "\">" + puVO.ReportFileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + rtype + "\">" + rtype + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + description + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";


    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function DeleteReport() {
    var chkList = $("#reportList").find("input[type='checkbox']:checked");

    if (chkList.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        var idString = "";
                        for (var i = 0; i < chkList.length; i++) {
                            var chkObj = $(chkList[i]);
                            var projectReportFileId = chkObj.next().val().split('_')[2];

                            if (projectReportFileId != "-1") {
                                if (idString != "")
                                    idString += ',';
                                idString += projectReportFileId;
                            }
                        }
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Project/DeleteProjectReportFile?projectFileIds=" + idString + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Flag == 1)
                                    chkList.parent().parent().remove();
                                else {
                                    bootbox.dialog({
                                        message: data.Message,
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
                            },
                            error: function (data) {
                                console.log(data);
                            }
                        });
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