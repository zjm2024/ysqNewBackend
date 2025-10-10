var _validFocus = new Array();
$(document).ready(function () {
    Init();
    initDatePicker();
    Init_NRequire_cookie();
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

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtEffectiveEndDate: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                number: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtAgencySum: {
                number: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtpercentage: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                number: true,
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: true,
                isTel: true
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtContactPhone: {
                isTel: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtEffectiveEndDate: {
                required: "请输入有效期！"
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: "请输入预算！"
            },
            ctl00$ContentPlaceHolder_Content$txtAgencySum: {
                required: "请输入准备招募的销售数量！"
            },
            ctl00$ContentPlaceHolder_Content$txtpercentage: {
                number: "请输入数值"
            },
            /*ctl00$ContentPlaceHolder_Content$txtCommission: {
                required: "请输入酬金！"
            },*/
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: "请输入手机号码!",
                isTel: "请输入正确格式手机号码！"
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtContactPhone: {
                isTel: "请输入正确格式号码！"
            }
        },
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
            if (_validFocus.length == 0) {
                _validFocus.push($(e));
            }
        },

        success: function (e) {
            $(e).closest('.form-group').removeClass('has-error');
            $(e).remove();
        }
    });

    $("button[id*='btn_save']").click(function () {

        _validFocus = new Array();
        if (!$("form[id*='aspnetForm']").valid()) {
            if (_validFocus.length > 0) {
                _validFocus[0].focus();
            }
            return false;
        }

        //判断描述是否输入
        /*var ue = UE.getEditor("container");
        var des = ue.getContent();
        if (des == "") {
            bootbox.dialog({
                message: "请输入描述！",
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
        }*/
        var tcList = $("table[id*='TargetCategoryList']").find("input[type='hidden']");
        if (tcList.length < 1) {
            bootbox.dialog({
                message: "请选择目标行业！",
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

        var tcityList = $("table[id*='TargetCityList']").find("input[type='hidden']");
        if (tcityList.length < 1) {
            bootbox.dialog({
                message: "请选择目标区域！",
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

        var requirementId = parseInt($("#" + hidRequirementId).val());
        var requirementVO = GetRequirementVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateRequire?token=" + _Token,
            type: "POST",
            data: requirementVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (requirementId < 1) {
                        $("#" + hidRequirementId).val(data.Result);
                        reloadMainData();
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
                console.log(data);
            }
        });
    });

    $("button[id*='btn_submit']").click(function () {

        _validFocus = new Array();
        if (!$("form[id*='aspnetForm']").valid()) {
            if (_validFocus.length > 0) {
                _validFocus[0].focus();
            }
            return false;
        }
        //判断描述是否输入
        var ue = UE.getEditor("container");
        var des = ue.getContent();
        if (des == "") {
            bootbox.dialog({
                message: "请输入描述！",
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

        var tcList = $("table[id*='TargetCategoryList']").find("input[type='hidden']");
        if (tcList.length < 1) {
            bootbox.dialog({
                message: "请选择目标行业！",
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

        var tcityList = $("table[id*='TargetCityList']").find("input[type='hidden']");
        if (tcityList.length < 1) {
            bootbox.dialog({
                message: "请选择目标区域！",
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

        var requirementId = parseInt($("#" + hidRequirementId).val());
        var requirementVO = GetRequirementVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateRequire?token=" + _Token,
            type: "POST",
            data: requirementVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (requirementId < 1) {
                        $("#" + hidRequirementId).val(data.Result);
                        reloadMainData();
                    }
                    requirementId = data.Result;
                    UpdateRequireStatusAction(requirementId, 5);
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
    });

    $("select[id$='drpProvince']").change(function () {
        //更新Child
        var drp = $("select[id*='drpProvince']");
        var childDrp = $("select[id*='drpCity']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + drp.val() + "&enable=true",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    childDrp.append("<option value=\"-1\">全部</option>");
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
                    }

                    //if (childList.length > 0)
                    //    childDrp.val(childList[0].CityId);
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    $("select[id$='drpCategory1']").change(function () {
        //更新Child
        var drp = $("select[id*='drpCategory1']");
        var childDrp = $("select[id*='drpCategory2']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + drp.val() + "&enable=true",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    childDrp.append("<option value=\"-1\">全部</option>");
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CategoryId + "\">" + childList[i].CategoryName + "</option>");
                    }

                    //if (childList.length > 0)
                    //    childDrp.val(childList[0].CategoryId);
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    $("button[id*='btn_updaterequirestatus']").click(function () {
        var requirementId = parseInt($("#" + hidRequirementId).val());
        bootbox.dialog({
            message: "是否确认取消发布?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateRequireStatusAction(requirementId, 0);
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

    $("button[id*='btn_start']").click(function () {
        var requirementId = parseInt($("#" + hidRequirementId).val());
        bootbox.dialog({
            message: "是否确认启动投标?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateRequireStatusAction(requirementId, 1);
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

    $("button[id*='btn_stop']").click(function () {
        var requirementId = parseInt($("#" + hidRequirementId).val());
        bootbox.dialog({
            message: "是否确认暂停投标?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateRequireStatusAction(requirementId, 3);
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

    $("button[id*='btn_plusrequirecommission']").click(function () {
        var requirementId = parseInt($("#" + hidRequirementId).val());
        DelegateRequireCommission(requirementId)
        return false;
    });

    $("button[id$='btn_cancelrequirecommission']").click(function () {
        var requirementId = parseInt($("#" + hidRequirementId).val());
        bootbox.dialog({
            message: "是否确认取消酬金托管?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        CancelRequireCommission(requirementId);
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

    $("button[id$='btn_cancel']").click(function () {
        window.location.href = "RequirementBrowse.aspx";
        return false;
    });
    $("input[name*='CommissionType']").change(function () {

        var objDecimal = $("input[id*='radDecimal']:checked");
        var objPer = $("input[id*='radPer']:checked");
        if (objDecimal.length > 0) {
            $("#divpercentage").hide();
            $("input[id$='txtCommission']").removeAttr("disabled");
        } else if (objPer.length > 0) {
            $("#divpercentage").show();
            var objCommission = $("input[id$='txtCommission']");
            var txtpercentage = $("input[id$='txtpercentage']");
            objCommission.attr("disabled", "disabled");

        }

    });

    $("input[id*='txtpercentage']").change(function () {
        var objCost = $("input[id$='txtCost']");
        var Commission = objCost.val() * $(this).val() / 100
        $("input[id$='txtCommission']").val(Commission);
    });
    $("input[id*='txtCost']").change(function () {

        var objPer = $("input[id$='radPer']:checked");
        if (objPer.length > 0) {
            var objtxtpercentage = $("input[id$='txtpercentage']");
            var Commission = objtxtpercentage.val() * $(this).val() / 100
            $("input[id$='txtCommission']").val(Commission);
        }
    });

    $("textarea[id$='txtProductDescription']").attr("maxlength", "400");

    $("textarea[id$='txtCompanyDescription']").attr("maxlength", "400");

    $("textarea[id$='txtTargetAgency']").attr("maxlength", "200");

    $("textarea[id$='txtAgencyCondition']").attr("maxlength", "200");

});

function UpdateRequireStatusAction(requireId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequireStatus?requireId=" + requireId + "&status=" + status + "&token=" + _Token,
        type: "Post",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                SetButton(status);

                if (status == 1) {
                    //如果发布成功，提示托管酬金  
                    bootbox.dialog({
                        message: "您的任务已成功发布，越早托管预押酬金越有利于您的成功销售，现在是否托管酬金？",
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "是",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    DelegateRequireCommission(requireId);
                                }
                            },
                            "Cancel":
                            {
                                "label": "否",
                                "className": "btn-sm",
                                "callback": function () {
                                }
                            }
                        }
                    });
                } else if (status == 0) {
                    //如果取消发布，退还托管的酬金
                    ReDelegateRequireCommission(requireId);
                } else if (status == 5) {
                    //审核中
                    bootbox.dialog({
                        message: "您的任务已提交审核，越早托管预押酬金越有利于您的成功销售，现在是否托管酬金？",
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "是",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    DelegateRequireCommission(requireId);
                                }
                            },
                            "Cancel":
                            {
                                "label": "否",
                                "className": "btn-sm",
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
            console.log(data);
            //load_hide();
        }
    });
}

function GetRequirementVO() {
    var requireModelVO = new Object();
    var requirementVO = new Object();

    requireModelVO.Requirement = requirementVO;

    var objCityId = $("select[id*='drpCity']");
    var objCategoryId = $("select[id*='drpCategory2']");
    var objRequirementCode = $("input[id*='txtRequirementCode']");
    var objTitle = $("input[id*='txtTitle']");
    var objMainImg = $("img[id*='imgMainPic']");
    var objEffectiveEndDate = $("input[id*='txtEffectiveEndDate']");
    var objDecimal = $("input[id*='radDecimal']:checked");
    var objCommission = $("input[id*='txtCommission']");
    var objCost = $("input[id*='txtCost']");
    var objCommissionDescription = $("input[id*='txtCommissionDescription']");
    var objPhone = $("input[id*='txtPhone']");
    var objAgencySum = $("input[id*='txtAgencySum']");
    var ue = UE.getEditor("container");

    var objTargetAgency = $("textarea[id*='txtTargetAgency']");

    var objAgencyCondition = $("textarea[id*='txtAgencyCondition']");
    var objContactPerson = $("input[id*='txtContactPerson']");
    var objContactPhone = $("input[id*='txtContactPhone']");

    requirementVO.RequirementId = parseInt($("#" + hidRequirementId).val());
    if (objCityId.val() > 0)
        requirementVO.CityId = objCityId.val();
    if (objCategoryId.val() > 0)
        requirementVO.CategoryId = objCategoryId.val();
    //requirementVO.CustomerId = objCustomerId.val();
    requirementVO.RequirementCode = objRequirementCode.val();
    requirementVO.Title = objTitle.val();
    requirementVO.MainImg = objMainImg.attr("src");
    requirementVO.EffectiveEndDate = objEffectiveEndDate.val();

    requirementVO.TargetAgency = objTargetAgency.val();
    requirementVO.AgencyCondition = objAgencyCondition.val();
    requirementVO.ContactPerson = objContactPerson.val();
    requirementVO.ContactPhone = objContactPhone.val();

    if (objDecimal.length > 0)
        requirementVO.CommissionType = 1;
    else
        requirementVO.CommissionType = 2;

    requirementVO.Commission = objCommission.val();
    requirementVO.Cost = objCost.val();
    requirementVO.CommissionDescription = objCommissionDescription.val();
    requirementVO.Phone = objPhone.val();
    requirementVO.agencySum = objAgencySum.val();
    requirementVO.Description = ue.getContent();

    var targetCategoryVOList = new Array();
    requireModelVO.RequireCategory = targetCategoryVOList;
    var puhidenObjList = $("table[id*='TargetCategoryList']").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var requireId = puValue.split("_")[1];
        var categoryId = puValue.split("_")[2];

        var targetCategoryVO = new Object();
        targetCategoryVOList.push(targetCategoryVO);
        targetCategoryVO.RequirementId = requireId;
        targetCategoryVO.CategoryId = categoryId;
    }

    var targetCityVOList = new Array();
    requireModelVO.RequireCity = targetCityVOList;
    var puhidenObjList = $("table[id*='TargetCityList']").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var requireId = puValue.split("_")[1];
        var cityId = puValue.split("_")[2];

        var targetCityVO = new Object();
        targetCityVOList.push(targetCityVO);
        targetCityVO.RequirementId = requireId;
        targetCityVO.CityId = cityId;
    }

    var fileVOList = new Array();
    requireModelVO.RequireFile = fileVOList;
    var puhidenObjList = $("table[id*='FileList']").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var requireId = puValue.split("_")[1];
        var fileId = puValue.split("_")[2];

        var fileVO = new Object();
        fileVOList.push(fileVO);
        fileVO.RequirementId = requireId;
        fileVO.FileName = puhidenObj.parent().next().attr("title");
        fileVO.FilePath = puhidenObj.parent().next().children().attr("href").replace(_RootPath, "~");
        fileVO.CreatedDate = puhidenObj.parent().next().next().html();
    }

    var requireClientVOList = new Array();
    requireModelVO.RequireClient = requireClientVOList;
    var puhidenObjList = $("#RequireClientList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var requireId = puValue.split("_")[1];
        var requireClientId = puValue.split("_")[2];

        var requireClientVO = new Object();
        requireClientVOList.push(requireClientVO);
        requireClientVO.RequirementId = requireId;
        requireClientVO.RequireClientId = requireClientId;
        requireClientVO.ClientName = puhidenObj.parent().next().html();
        requireClientVO.CompanyName = puhidenObj.parent().next().next().html();
        requireClientVO.Description = puhidenObj.parent().next().next().next().html();
    }

    return requireModelVO;
}

function SetRequirement(requirementVO) {

    var objCityId = $("select[id*='txtCityId']");
    var objCategoryId = $("select[id*='txtCategoryId']");
    var objRequirementCode = $("input[id*='txtRequirementCode']");
    var objTitle = $("input[id*='txtTitle']");
    var objMainImg = $("img[id*='imgMainPic']");
    var objEffectiveEndDate = $("input[id*='txtEffectiveEndDate']");
    var objDecimal = $("input[id*='radDecimal']");
    var objPer = $("input[id*='radPer']");
    var objCommission = $("input[id*='txtCommission']");
    var objCost = $("input[id*='txtCost']");
    var objCommissionDescription = $("input[id*='txtCommissionDescription']");
    var objPhone = $("input[id*='txtPhone']");
    var objAgencySum = $("input[id*='txtAgencySum']");

    var objStatus = $("input[id*='txtStatus']");
    var objCreatedAt = $("input[id*='txtCreatedAt']");

    var objTargetAgency = $("textarea[id*='txtTargetAgency']");
    var objAgencyCondition = $("textarea[id*='txtAgencyCondition']");

    var objContactPerson = $("input[id*='txtContactPerson']");
    var objContactPhone = $("input[id*='txtContactPhone']");

    BindCity(requirementVO.ProvinceId, requirementVO.CityId);
    BindCategory(requirementVO.ParentCategoryId, requirementVO.CategoryId);
    objRequirementCode.val(requirementVO.RequirementCode);
    objTitle.val(requirementVO.Title);
    objMainImg.attr("src", requirementVO.MainImg);
    objEffectiveEndDate.datepicker("setDate", new Date(requirementVO.EffectiveEndDate));

    if (requirementVO.CommissionType == 1)
        objDecimal.attr("checked", true);
    else
        objPer.attr("checked", true);

    objCommission.val(requirementVO.Commission);
    objCost.val(requirementVO.Cost);
    objCommissionDescription.val(requirementVO.CommissionDescription);
    objPhone.val(requirementVO.Phone);
    objAgencySum.val(requirementVO.agencySum);

    objTargetAgency.val(requirementVO.TargetAgency);
    objAgencyCondition.val(requirementVO.AgencyCondition);
    objContactPerson.val(requirementVO.ContactPerson);
    objContactPhone.val(requirementVO.ContactPhone);

    var ue = UE.getEditor("container");
    ue.ready(function () {
        this.setContent(requirementVO.Description);
    });

    if (requirementVO.Status == 0) {
        objStatus.val("保存");
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    }
    else if (requirementVO.Status == 5) {
        objStatus.val("保存");
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    }
    else if (requirementVO.Status == 1) {
        objStatus.val("发布");
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").show();
        $("button[id*='btn_updaterequirestatus']").show();
    } else if (requirementVO.Status == 2) {
        objStatus.val("关闭");
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    } else if (requirementVO.Status == 3) {
        objStatus.val("暂停投标");
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").show();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    } else if (requirementVO.Status == 4) {
        objStatus.val("已选定销售");
        $("button[id*='btn_save']").hide();
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
        $("button[id*='btn_plusrequirecommission']").hide();
        $("button[id*='btn_cancelrequirecommission']").hide();
    }
    objCreatedAt.val(new Date(requirementVO.CreatedAt).format("yyyy-MM-dd"));

    BindTenderInvite(requirementVO.RequirementId);
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

function BindTenderInvite(requireId) {
    $("table[id*='TenderInviteList']").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireTenderInviteByRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddTenderinvite(puVO);
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

function AddTenderinvite(puVO) {
    var fileTable = $("table[id*='TenderInviteList']");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <button class=\"wtbtn yjright\" type=\"button\" onclick=\"return GenerateProject('" + puVO.CustomerId + "');\" title=\"选中\"> \r\n";
    oTR += "        <i class=\"icon-ok bigger-110\"></i> \r\n";
    oTR += "        选中 \r\n";
    oTR += "    </button> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.InviteDate).format("yyyy-MM-dd hh:ss") + "\">" + new Date(puVO.InviteDate).format("yyyy-MM-dd hh:ss") + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.AgencyName + "\">" + puVO.AgencyName + "</td> \r\n";
    if (puVO.AgencyLevel == "1")
        oTR += "  <td class=\"center\" title=\"金牌销售\">金牌销售</td> \r\n";
    else if (puVO.AgencyLevel == "2")
        oTR += "  <td class=\"center\" title=\"银牌销售\">银牌销售</td> \r\n";
    else if (puVO.AgencyLevel == "3")
        oTR += "  <td class=\"center\" title=\"铜牌销售\">铜牌销售</td> \r\n";
    else if (puVO.AgencyLevel == "4")
        oTR += "  <td class=\"center\" title=\"普通销售\">普通销售</td> \r\n";
    else
        oTR += "  <td class=\"center\" title=\"\"></td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function GenerateProject(customerId) {
    var projectVO = new Object();
    projectVO.RequirementId = parseInt($("#" + hidRequirementId).val());
    projectVO.CustomerId = customerId;
    //不判断是否有合同，直接创建新合同
    window.location.href = "../ProjectManagement/GenerateProject.aspx?RequireId=" + projectVO.RequirementId + "&CustomerId=" + projectVO.CustomerId;

    //判断是否创建项目,已创建项目就不允许再创建
    //判断是否创建合同,已创建合同，提示是否作废之前的合同
    /*
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireStatus?requireId=" + projectVO.RequirementId + "&token=" + _Token,
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                window.location.href = "../ProjectManagement/GenerateProject.aspx?RequireId=" + projectVO.RequirementId + "&CustomerId=" + projectVO.CustomerId;
            } else if (data.Flag == -1) {
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
            } else if (data.Flag == -2) {
                bootbox.dialog({
                    message: "存在未处理的合同，是否作废之前的合同？",
                    buttons:
                    {
                        "click":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                //作废合同，然后再创建合同
                                $.ajax({
                                    url: _RootPath + "SPWebAPI/Project/UpdateRequireContractSatus?requireId=" + projectVO.RequirementId + "&token=" + _Token,
                                    type: "GET",
                                    data: null,
                                    success: function (data) {
                                        if (data.Flag == 1) {
                                            window.location.href = "../ProjectManagement/GenerateProject.aspx?RequireId=" + projectVO.RequirementId + "&CustomerId=" + projectVO.CustomerId;
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
                                        //console.log(data);
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

        },
        error: function (data) {
            //console.log(data);
        }
    });

    */
    return false;
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

    var requireId = parseInt($("#" + hidRequirementId).val());

    if (requireId > 0) {
        $("#divRequireCode").show();
        SetButton($("input[id*='hidStatus']").val());
    } else {
        
    }


    if (!isBusiness) {
        $("button[id*='btn_save']").hide();
        load_hide();
        bootbox.dialog({
            message: "还未通过认证，不能执行该操作!",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "../CustomerManagement/BusinessCreateEdit.aspx";
                    }
                }
            }
        });
        return false;
    }
}

function reloadMainData() {
    var requireId = parseInt($("#" + hidRequirementId).val());
    if (requireId > 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/GetRequire?requireId=" + requireId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var requireVO = data.Result;


                    var objRequirementCode = $("input[id*='txtRequirementCode']");

                    objRequirementCode.val(requireVO.RequirementCode);

                }
                load_hide();
            },
            error: function (data) {
                console.log(data);
                load_hide();
            }
        });
    } else {
        load_hide();
    }
}

function SetButton(status) {
    if (status == 0) {
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    }
    else if (status == 5) {
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    }
    else if (status == 1) {
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").show();
        $("button[id*='btn_updaterequirestatus']").show();
    } else if (status == 2) {
        $("button[id*='btn_submit']").show();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    } else if (status == 3) {
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").show();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
    } else if (status == 4) {
        $("button[id*='btn_save']").hide();
        $("button[id*='btn_submit']").hide();
        $("button[id*='btn_start']").hide();
        $("button[id*='btn_stop']").hide();
        $("button[id*='btn_updaterequirestatus']").hide();
        $("button[id*='btn_plusrequirecommission']").hide();
        $("button[id*='btn_cancelrequirecommission']").hide();
    }
}


function change(uploadId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("img[id*='" + uploadId + "Pic']").attr("src", data.Result.FilePath.replace("~", _APIURL));
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

function changefile(uploadId) {
    var tempPath = new Date().format("yyyyMM");
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var fileVO = new Object();
            fileVO.RequirementId = parseInt($("#" + hidRequirementId).val());
            fileVO.FileName = data.Result.FileName;
            fileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
            fileVO.CreatedDate = new Date();

            AddFile(fileVO);
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

function BindFile(requireId) {
    $("table[id*='FileList'").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireFileByRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    puVO.FilePath = puVO.FilePath.replace("~", _APIURL);
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
    var fileTable = $("table[id*='FileList'");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"File_" + puVO.RequirementId + "_" + puVO.RequirementFileId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function DeleteFile() {
    var chkList = $("table[id*='FileList']").find("tbody input[type='checkbox']:checked");

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
                        chkList.parent().parent().remove();
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


function BindTargetCity(requireId) {
    $("table[id*='TargetCityList']").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireCityByRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddTargetCity(puVO);
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
function BindTargetCitybybusinessId(businessId) {//获取目标客户区域
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCityByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    var text = "";
                    for (var i = 0; i < puVOList.length; i++) {
                        text += puVOList[i].CityName + "，";
                        var puVO = puVOList[i];
                        AddTargetCity(puVO);
                    }
                }
            } else {
            }
            load_hide();
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function AddTargetCity(puVO) {
    var targetCityTable = $("table[id*='TargetCityList']");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCity_" + puVO.RequirementId + "_" + puVO.CityId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProvinceName + "\">" + puVO.ProvinceName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CityName + "\">" + puVO.CityName + "</td> \r\n";
    oTR += "</tr> \r\n";

    targetCityTable.append(oTR);
}

function NewTargetCity() {

    onChooseCity();
}

function DeleteTargetCity() {
    var chkList = $("table[id*='TargetCityList']").find("tbody input[type='checkbox']:checked");

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
                        chkList.parent().parent().remove();
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

function onChooseCity() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseCityList.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择城市",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleCityObj = $(window.frames["iframe_1"].document).find("#ChooseCityList").find("input[type='checkbox']:checked");
                    var selectCityIdStr = "";
                    var cityArray = new Array();
                    for (var i = 0; i < seleCityObj.length; i++) {
                        var city = new Object();
                        cityArray.push(city);
                        var chk = $(seleCityObj[i]);
                        city.CityId = chk.parent().next()[0].innerText;
                        city.ProvinceName = chk.parent().next().next()[0].innerText;
                        city.CityCode = chk.parent().next().next().next()[0].innerText;
                        city.CityName = chk.parent().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidCityId").val(JSON.stringify(cityArray));

                    var cityArray = $(window.frames["iframe_1"].document).find("#hidCityId").val();
                    if (cityArray != "") {
                        var cityList = JSON.parse(cityArray);
                        var puhidenObjList = $("table[id*='TargetCityList']").find("input[type='hidden']");
                        for (var i = 0; i < cityList.length; i++) {
                            var cityObj = cityList[i];
                            var puVO = new Object();
                            puVO.RequirementId = parseInt($("#" + hidRequirementId).val());
                            puVO.CityId = cityObj.CityId;
                            puVO.ProvinceName = cityObj.ProvinceName;
                            puVO.CityName = cityObj.CityName;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var cityId = puValue.split("_")[2];

                                if (cityId == puVO.CityId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddTargetCity(puVO);
                        }

                    }
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


function BindTargetCategory(requireId) {
    $("table[id*='TargetCategoryList']").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireCategoryByRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddTargetCategory(puVO);
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
function BindTargetCategorybybusinessId(businessId) { //获取目标客户行业
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    for (var i = 0; i < puVOList.length; i++) {
                        var puVO = puVOList[i];
                        AddTargetCategory(puVO);
                    }
                }
            } else {
            }
            load_hide();
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function AddTargetCategory(puVO) {
    var targetCategoryTable = $("table[id*='TargetCategoryList']");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.RequirementId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    targetCategoryTable.append(oTR);
}

function NewTargetCategory() {

    onChooseCategory();
}

function DeleteTargetCategory() {
    var chkList = $("table[id*='TargetCategoryList']").find("tbody input[type='checkbox']:checked");

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
                        chkList.parent().parent().remove();
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

function onChooseCategory() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseCategoryList.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择行业",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleCategoryObj = $(window.frames["iframe_1"].document).find("#ChooseCategoryList").find("input[type='checkbox']:checked");
                    var selectCategoryIdStr = "";
                    var categoryArray = new Array();
                    for (var i = 0; i < seleCategoryObj.length; i++) {
                        var category = new Object();
                        categoryArray.push(category);
                        var chk = $(seleCategoryObj[i]);
                        category.CategoryId = chk.parent().next()[0].innerText;
                        category.ParentCategoryName = chk.parent().next().next()[0].innerText;
                        category.CategoryCode = chk.parent().next().next().next()[0].innerText;
                        category.CategoryName = chk.parent().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidCategoryId").val(JSON.stringify(categoryArray));

                    var categoryArray = $(window.frames["iframe_1"].document).find("#hidCategoryId").val();
                    if (categoryArray != "") {
                        var categoryList = JSON.parse(categoryArray);
                        var puhidenObjList = $("table[id*='TargetCategoryList']").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.RequirementId = parseInt($("#" + hidRequirementId).val());
                            puVO.CategoryId = categoryObj.CategoryId;
                            puVO.ParentCategoryName = categoryObj.ParentCategoryName;
                            puVO.CategoryName = categoryObj.CategoryName;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var categoryId = puValue.split("_")[2];

                                if (categoryId == puVO.CategoryId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddTargetCategory(puVO);
                        }

                    }
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


function DelegateRequireCommission(requireId) {
    //无论是首次委托还是追加，都会弹出一个委托页面进行委托。已委托金额为0，当做首次委托，至少50%
    var objCommission = $("input[id*='txtCommission']");
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 35%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/RequireManagement\/RequireCommissionCreateEdit.aspx?RequireId=' + requireId + '&Commission=' + objCommission.val() + '" height="150px" width="100%" frameborder="0"><\/iframe>',

        title: "酬金委托",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtRequireCommissionObj = $(window.frames["iframe_1"].document).find("label[id*='lblRequireCommission']");
                    var txtCommissionDelegatedObj = $(window.frames["iframe_1"].document).find("label[id*='lblDelegateRequireCommission']");
                    var txtCommissionObj = $(window.frames["iframe_1"].document).find("input[id*='txtCommission']");

                    if (txtCommissionObj.val() == "" || txtCommissionObj.val() < 0) {
                        bootbox.dialog({
                            message: "请输入正确委托金额！",
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

                    if (txtCommissionDelegatedObj.html() == 0) {
                        //首次委托
                        if (txtCommissionObj.val() < txtRequireCommissionObj.html() * 0.2) {
                            bootbox.dialog({
                                message: "首次委托金额不能少于酬金的20%！",
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
                    }

                    $.ajax({
                        url: _RootPath + "SPWebAPI/Require/DelegateRequireCommission?requireId=" + requireId + "&commission=" + txtCommissionObj.val() + "&token=" + _Token,
                        type: "GET",
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
                                            if (data.Flag != 1) {
                                                window.location.href = ("../../Pay/CustomerRechange.aspx");
                                            }
                                        }
                                    }
                                }
                            });
                        },
                        error: function (data) {
                            //console.log(data);
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

function CancelRequireCommission(requireId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/CancelDelegateRequireCommission?requireId=" + requireId + "&token=" + _Token,
        type: "GET",
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
            console.log(data);
        }
    });
}


function BindRequireClient(requireId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireIdClientByBusiness?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddRequireClient(puVO);
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

function AddRequireClient(puVO) {
    var requireClientTable = $("#RequireClientList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSuperClient_" + puVO.RequirementId + "_" + puVO.RequireClientId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ClientName + "\">" + puVO.ClientName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CompanyName + "\">" + puVO.CompanyName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + puVO.Description + "</td> \r\n";
    oTR += "</tr> \r\n";

    requireClientTable.append(oTR);
}

function NewRequireClient() {

    onChooseRequireClient();
}

function DeleteRequireClient() {
    var chkList = $("#RequireClientList").find("tbody input[type='checkbox']:checked");

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
                        chkList.parent().parent().remove();
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

function onChooseRequireClient() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/RequireManagement\/RequireClientCreateEdit.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "添加目标客户",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtClientNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtClientName']");
                    var txtCompanyNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtCompanyName']");
                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtDescription']");

                    if (txtClientNameObj.val() == "" && txtCompanyNameObj.val() == "") {
                        bootbox.dialog({
                            message: "客户名称或用户名称至少填写一个",
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
                    var puVO = new Object();
                    puVO.RequireClientId = -1;
                    puVO.RequirementId = parseInt($("#" + hidRequirementId).val());;
                    puVO.ClientName = txtClientNameObj.val();
                    puVO.CompanyName = txtCompanyNameObj.val();
                    puVO.Description = txtDescriptionObj.val();

                    var isContain = false;
                    var puhidenObjList = $("#RequireClientList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var companyName = puhidenObj.parent().next().next().html();
                        if (companyName == puVO.CompanyName) {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain)
                        AddRequireClient(puVO);

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
function NRequire_save_next(id) {//下一步
    switch (id) {
        case 1:
            var objTitle = $("input[id*='txtTitle']");
            if (objTitle.val() == "") {
                bootbox.dialog({
                    message: "请输入标题！",
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
            var requirementVO = GetRequirementVO();
            postdata(requirementVO);
            NRequire_page(2);
            break;
        case 2:
            var requirementVO = GetRequirementVO();
            postdata(requirementVO);
            var tcList = $("table[id*='TargetCategoryList']").find("input[type='hidden']");
            if (tcList.length < 1) {
                bootbox.dialog({
                    message: "请选择目标行业！",
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

            var tcityList = $("table[id*='TargetCityList']").find("input[type='hidden']");
            if (tcityList.length < 1) {
                bootbox.dialog({
                    message: "请选择目标区域！",
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
            
            NRequire_page(3);
            break;
        case 3:
            var requirementVO = GetRequirementVO();
            postdata(requirementVO);
            var objEffectiveEndDate = $("input[id*='txtEffectiveEndDate']");
            if (objEffectiveEndDate.val() == "") {
                bootbox.dialog({
                    message: "请选择有效日期！",
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
            var nowtime = new Date(Date.now());
            var startDate = lg_nowtime();
            var endDate = objEffectiveEndDate.val();
            var startNum = parseInt(startDate.replace(/-/g, ''), 10);
            var endNum = parseInt(endDate.replace(/-/g, ''), 10);
            if (startNum > endNum) {
                bootbox.dialog({
                    message: "有效日期不能小于当前时间！",
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
            
            NRequire_page(4);
            break;
        case 4:
            var requirementVO = GetRequirementVO();
            postdata(requirementVO);

            var objPhone = $("input[id*='txtPhone']");
            if (objPhone.val() == "") {
                bootbox.dialog({
                    message: "请填写手机号码！",
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
            /*var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0]{1})|(15[0-3]{1})|(15[5-9]{1})|(18[0-9]{1}))+\d{8})$/;

            phone = objPhone.val();
            if (phone.length != 11 || !myreg.test(phone)) {
                bootbox.dialog({
                    message: "请输入有效的手机号码！",
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
            }*/

            NRequire_page(5);
            break;
        case 5:
            var requirementVO = GetRequirementVO();
            requirementVO.Requirement.Status = 5;
            console.log(requirementVO);
            $.ajax({
                url: _RootPath + "SPWebAPI/Require/UpdateRequire?token=" + _Token,
                type: "POST",
                data: requirementVO,
                success: function (data) {
                    if (data.Flag == 1) {
                        //审核中
                        delCookie('page');
                        delCookie('requirementId')
                        bootbox.dialog({
                            message: "您的任务已提交审核，越早托管预押酬金越有利于您的成功销售，现在是否托管酬金？",
                            buttons:
                            {
                                "Confirm":
                                {
                                    "label": "是",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
                                        DelegateRequireCommission(requirementVO.Requirement.RequirementId);
                                    }
                                },
                                "Cancel":
                                {
                                    "label": "否",
                                    "className": "btn-sm",
                                    "callback": function () {
                                        window.location.href = _RootPath + "RequireManagement/RequirementBrowse.aspx";
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
            break;
        default:
            break;
    }
}
function NRequire_save_prev(id) {//上一步
    switch (id) {
        case 2:
            NRequire_page(1);
            break;
        case 3:
            NRequire_page(2);
            break;
        case 4:
            NRequire_page(3);
            break;
        case 5:
            NRequire_page(4);
            break;
        default:
            break;
    }
}
function postdata(requirementVO) {
    var requirementId = parseInt($("#" + hidRequirementId).val());
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequire?token=" + _Token,
        type: "POST",
        data: requirementVO,
        success: function (data) {
            if (data.Flag == 1) {
                if (requirementId < 1) {
                    $("#" + hidRequirementId).val(data.Result);
                    reloadMainData();
                }
                requirementId = data.Result;
                var cookieObj = {};
                cookieObj.requirementId = requirementId;
                NRequire_cookie(cookieObj);
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
function NRequire_page(id) {//跳转页面，单纯跳转不做保存操作
    $(".NRequire_content_box").hide();
    $("#box" + id).show();
    $("#NRequire_page").html(id + "/5");

    var cookieObj = {};
    cookieObj.page = id;
    NRequire_cookie(cookieObj);
}
function NRequire_cookie(obj) {//将当前状态设置到cookie
    if (obj.page != null)
    {
        setCookie('page', obj.page);
    }
    if (obj.requirementId != null) {
        setCookie('requirementId', obj.requirementId);
        setCookie('CustomerId ', _CustomerId);
    }
}
function Init_NRequire_cookie() {//页面加载时调用
    if (getCookie('requirementId') != null && parseInt(getCookie('requirementId')) > 0 && parseInt($("#" + hidRequirementId).val()) == 0 && getCookie('CustomerId') == _CustomerId) {
        window.location.href = _RootPath + "RequireManagement/RequirementCreateEdit_New.aspx?RequirementId=" + getCookie('requirementId');
    }

    if (getCookie('CustomerId') != _CustomerId) {
        delCookie('page');
    }
    if (getCookie('page') != null)
    {
        NRequire_page(getCookie('page'));
    }
    if (parseInt($("#" + hidRequirementId).val()) == 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetBusiness?businessId=" + _BusinessId + "&token=" + _Token,
            type: "Get",
            success: function (data) {
                if (data.Flag == 1)
                {
                    BindCity(data.Result.ProvinceId, data.Result.CityId);
                    if (data.Result.CompanyLogo != "") {
                        $("img[id*='imgMainPic']").attr("src", data.Result.CompanyLogo);
                    }
                    $("input[id*='txtContactPerson']").val(data.Result.CustomerName);
                    $("input[id*='txtPhone']").val(data.Result.Phone);
                    var ue = UE.getEditor("container");
                    ue.ready(function () {
                        this.setContent(data.Result.ProductDescription);
                    });
                } 
            },
            error: function (data) {
                console.log(data);
            }
        });
        if (_BusinessId > 0) {
            BindBusinessCategory(_BusinessId);//绑定行业
            BindTargetCitybybusinessId(_BusinessId);//绑定目标区域
            BindTargetCategorybybusinessId(_BusinessId);//绑定目标行业
        }
        
    }
}

function BindBusinessCategory(businessId) {//绑定行业
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    BindCategory(puVOList[0].ParentCategoryId, puVOList[0].CategoryId);
                }

            } else {
            }
            load_hide();
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function lg_nowtime(){ /*当前时间*/
    var now = new Date();
    var year = now.getFullYear();
    var month = (now.getMonth() + 1).toString();
    var day = (now.getDate()).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    if (day.length == 1) {
        day = "0" + day;
    }
    var dateTime = year + month + day;
    return dateTime;
}
function setCookie(name, value) { /*写入Cookie*/
    var Days = 60; var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}
function getCookie(name) {  /*读取Cookie*/
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}
function delCookie(name) { /*删除Cookie*/
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}
function nowtime(){ /*当前时间*/
    var now = new Date();
    var year = now.getFullYear();
    var month = (now.getMonth() + 1).toString();
    var day = (now.getDate()).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    if (day.length == 1) {
        day = "0" + day;
    }
    var dateTime = year + month + day;
    return dateTime;
}