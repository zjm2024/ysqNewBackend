$(document).ready(function () {
    Init();
    initDatePicker();
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

            ctl00$ContentPlaceHolder_Content$txtCommission: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: true,
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtTenderCount: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtEffectiveEndDate: {
                required: "请输入有效期！"
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                required: "请输入酬金！"
            },
            ctl00$ContentPlaceHolder_Content$txtCost: {
                required: "请输入任务预算！",
                number:"请输入数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtTenderCount: {
                required: "请输入投标数目！"
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                isTel: "请输入手机号码！"
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: "请输入部门！"
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
                        Init();
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

    $("select[id$='drpProvince']").change(function () {
        //更新Child
        var drp = $("select[id*='drpProvince']");
        var childDrp = $("select[id*='drpCity']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/user/GetCityList?provinceId=" + drp.val() + "&enable=true&token=" + _Token,
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
                    }

                    if (childList.length > 0)
                        childDrp.val(childList[0].CityId);
                }
            },
            error: function (data) {
                alert(data);
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
                alert(data);
            }
        });
    });

    $("#btn_updaterequirestatus").click(function () {
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

    $("#btn_cancel").click(function () {
        window.location.href = "RequirementBrowse.aspx";
        return false;
    });
          
    
});

function UpdateRequireStatusAction(requireId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequireStatus?requireId=" + requireId + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "RequirementBrowse.aspx";
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

function GetRequirementVO() {
    var requireModelVO = new Object();
    var requirementVO = new Object();

    requireModelVO.Requirement = requirementVO;

    var objCityId = $("select[id*='drpCity']");
    var objCategoryId = $("select[id*='drpCategory2']");
	var objRequirementCode = $("input[id*='txtRequirementCode']");
	var objTitle = $("input[id*='txtTitle']");
	var objMainImg = $("img[id*='imgMain']");
	var objEffectiveEndDate = $("input[id*='txtEffectiveEndDate']");
	var objDecimal = $("input[id*='radDecimal']:checked");
	var objCommission = $("input[id*='txtCommission']");
	var objCost = $("input[id*='txtCost']");
    var objTenderCount = $("input[id*='txtTenderCount']");
    var objAuthentication = $("input[id*='txtAuthentication']");
	var objPhone = $("input[id*='txtPhone']");
	var ue = UE.getEditor('container');
	//var objStatus = $("input[id*='txtStatus']");
    //var objCreatedAt = $("input[id*='txtCreatedAt']");

	requirementVO.RequirementId = parseInt($("#" + hidRequirementId).val());
	requirementVO.CityId = objCityId.val();
	requirementVO.CategoryId = objCategoryId.val();
	//requirementVO.CustomerId = objCustomerId.val();
	requirementVO.RequirementCode = objRequirementCode.val();
	requirementVO.Title = objTitle.val();
	requirementVO.MainImg = objMainImg.attr("src");
	requirementVO.EffectiveEndDate = objEffectiveEndDate.val();

	if (objDecimal.length > 0)
	    requirementVO.CommissionType = 1;
	else
	    requirementVO.CommissionType = 2;

	requirementVO.Commission = objCommission.val();
	requirementVO.Cost = objCost.val();
    requirementVO.TenderCount = objTenderCount.val();
    requirementVO.Authentication = objAuthentication.val();
	requirementVO.Phone = objPhone.val();
	requirementVO.Description = ue.getContent();
	//requirementVO.Status = objStatus.val();
    //requirementVO.CreatedAt = objCreatedAt.val();

	var targetCategoryVOList = new Array();
	requireModelVO.RequireCategory = targetCategoryVOList;
	var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
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
	var puhidenObjList = $("#TargetCityList").find("input[type='hidden']");
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

	return requireModelVO;
}

function SetRequirement(requirementVO) {



    var objCityId = $("select[id*='txtCityId']");
    var objCategoryId = $("select[id*='txtCategoryId']");
	var objRequirementCode = $("input[id*='txtRequirementCode']");
	var objTitle = $("input[id*='txtTitle']");
	var objMainImg = $("img[id*='imgMain']");
	var objEffectiveEndDate = $("input[id*='txtEffectiveEndDate']");
	var objDecimal = $("input[id*='radDecimal']");
	var objPer = $("input[id*='radPer']");
	var objCommission = $("input[id*='txtCommission']");
	var objCost = $("input[id*='txtCost']");
    var objTenderCount = $("input[id*='txtTenderCount']");
    var objAuthentication = $("input[id*='txtAuthentication']");
	var objPhone = $("input[id*='txtPhone']");
	var ue = UE.getEditor('container');
	var objStatus = $("input[id*='txtStatus']");
	var objCreatedAt = $("input[id*='txtCreatedAt']");

	BindCity(requirementVO.ProvinceId, requirementVO.CityId);
	BindCategory(requirementVO.ParentCategoryId, requirementVO.CategoryId);
	objCityId.val(requirementVO.CityId);
	objCategoryId.val(requirementVO.CategoryId);
	objRequirementCode.val(requirementVO.RequirementCode);
	objTitle.val(requirementVO.Title);
	objMainImg.attr("src",requirementVO.MainImg);
	objEffectiveEndDate.datepicker("setDate", new Date(requirementVO.EffectiveEndDate));

	

	if (requirementVO.CommissionType == 1)
	    objDecimal.attr("checked", true);
	else
	    objPer.attr("checked", true);

	objCommission.val(requirementVO.Commission);
	objCost.val(requirementVO.Cost);
    objTenderCount.val(requirementVO.TenderCount);
    objAuthentication.val(requirementVO.Authentication);
	objPhone.val(requirementVO.Phone);

	ue.ready(function () {
	    this.setContent(requirementVO.Description);
	});
	

	if (requirementVO.Status == 0)
	    objStatus.val("保存");
	else if (requirementVO.Status == 1)
	    objStatus.val("发布");
	else if (requirementVO.Status == 2)
	    objStatus.val("关闭");
	else if (requirementVO.Status == 3)
	    objStatus.val("暂停投标");
	else if (requirementVO.Status == 4)
	    objStatus.val("已创建项目");
	objCreatedAt.val(new Date(requirementVO.CreatedAt).format("yyyy-MM-dd"));

}

function Init() {
    var requireId = parseInt($("#" + hidRequirementId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequire?requireId=" + requireId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var requireVO = data.Result;

                SetRequirement(requireVO);

                //绑定目标客户行业
                BindTargetCategory(requireId);
                //绑定目标客户区域
                BindTargetCity(requireId);

                BindBusinessClient(requireVO.BusinessId);
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


function BindTargetCity(requireId) {
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
            alert(data);
        }
    });
}

function AddTargetCity(puVO) {
    var targetCityTable = $("#TargetCityList");
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
    var chkList = $("#TargetCityList").find("input[type='checkbox']:checked");

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
                        var puhidenObjList = $("#TargetCityList").find("input[type='hidden']");
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

function BindTargetCategory(requireId) {
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
            alert(data);
        }
    });
}

function AddTargetCategory(puVO) {
    var targetCategoryTable = $("#TargetCategoryList");
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
    var chkList = $("#TargetCategoryList").find("input[type='checkbox']:checked");

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
                        var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
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


function BindBusinessClient(businessId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessClientByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddBusinessClient(puVO);
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

function AddBusinessClient(puVO) {
    var businessClientTable = $("#BusinessClientList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSuperClient_" + puVO.BusinessId + "_" + puVO.BusinessClientId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ClientName + "\">" + puVO.ClientName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CompanyName + "\">" + puVO.CompanyName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + puVO.Description + "</td> \r\n";
    oTR += "</tr> \r\n";

    businessClientTable.append(oTR);
}