$(document).ready(function () {
    SetButton(0);
    initDatePicker();
    Init();
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

            ctl00$ContentPlaceHolder_Content$drpCity: {
                ddlrequired: true
            },
            ctl00$ContentPlaceHolder_Content$txtCompanyName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtSetupDate: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCompanyType: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtBusinessLicense: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$drpCity: {
                ddlrequired: "请选择区域！"
            },
            ctl00$ContentPlaceHolder_Content$txtCompanyName: {
                required: "请输入雇主名称！"
            },

            ctl00$ContentPlaceHolder_Content$txtSetupDate: {
                required: "请输入成立日期！"
            },
            ctl00$ContentPlaceHolder_Content$txtCompanyType: {
                required: "请输入企业性质！"
            },
            ctl00$ContentPlaceHolder_Content$txtBusinessLicense: {
                required: "请输入营业执照号！"
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

        var businessId = parseInt($("#" + hidBusinessId).val());
        var businessVO = GetBusinessVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateBusiness?token=" + _Token,
            type: "POST",
            data: businessVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (businessId < 1) {
                        $("#" + hidBusinessId).val(data.Result);
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

    $("#btn_updatebusinessstatus").click(function () {
        var businessId = parseInt($("#" + hidBusinessId).val());
        bootbox.dialog({
            message: "是否确认取消认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateBusinessStatusAction(businessId, 2,"");
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

    $("#btn_updatebusinessstatuscommit").click(function () {
        var businessId = parseInt($("#" + hidBusinessId).val());
        bootbox.dialog({
            message: "是否确认通过认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateBusinessStatusAction(businessId, 1,"");
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

    $("#btn_updatebusinessstatusreject").click(function () {
        var objApproveComment = $("textarea[id*='txtApproveComment']");

        if (objApproveComment.val() == "") {
            bootbox.dialog({
                message: "驳回理由不能为空！",
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
            return;
        }

        var businessId = parseInt($("#" + hidBusinessId).val());
        bootbox.dialog({
            message: "是否确认驳回认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateBusinessStatusAction(businessId, 2, objApproveComment.val());
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
        window.location.href = "BusinessBrowse.aspx";
        return false;
    });
          
    
});

function UpdateBusinessStatusAction(businessId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateBusinessStatus?businessId=" + businessId + "&approveComment=" + approveComment + "&status=" + status + "&type=B&token=" + _Token,
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
                                window.location.href = "BusinessBrowse.aspx";
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


function GetBusinessVO() {
    var businessModelVO = new Object();
    var businessVO = new Object();

    businessModelVO.Business = businessVO;

    var objCityId = $("select[id*='drpCity']");
	var objCompanyName = $("input[id*='txtCompanyName']");
	var ue = UE.getEditor('container');
	var ue2 = UE.getEditor('container2');
	var objSetupDate = $("input[id*='txtSetupDate']");
	var objCompanyType = $("input[id*='txtCompanyType']");
	var objBusinessLicense = $("input[id*='txtBusinessLicense']");
	var objBusinessLicenseImg = $("img[id*='imgLincensePic']");
	var objCompanyLogo = $("img[id*='imgCompanyLogoPic']");
    //var objMainProducts = $("textarea[id*='txtMainProducts']");
	var objDescription = $("textarea[id*='Description']");
	var objEntrustImgPath = $("img[id*='imgEntrustPic']");
    //var objContactPersonImgPath = $("input[id*='txtContactPersonImgPath']");
	//var objProductDescription = $("textarea[id*='txtProductDescription']");
	var objCompanyDescription = $("textarea[id*='txtCompanyDescription']");
	var objAddress = $("input[id*='txtAddress']");
	var objCompanySite = $("input[id*='txtCompanySite']");
	var objCompanyTel = $("input[id*='txtCompanyTel']");

	businessVO.BusinessId = parseInt($("#" + hidBusinessId).val());
	businessVO.CityId = objCityId.val();
	businessVO.CompanyName = objCompanyName.val();
	//businessVO.Description = ue.getContent();
	businessVO.SetupDate = objSetupDate.val();
	businessVO.CompanyType = objCompanyType.val();
	businessVO.BusinessLicense = objBusinessLicense.val();
	businessVO.BusinessLicenseImg = objBusinessLicenseImg.attr("src");
	businessVO.CompanyLogo = objCompanyLogo.attr("src");
    businessVO.MainProducts =ue2.getContent();
	businessVO.Description = objDescription.val();
	businessVO.EntrustImgPath = objEntrustImgPath.attr("src");
	//businessVO.ContactPersonImgPath = objContactPersonImgPath.val();
    businessVO.ProductDescription = ue.getContent();
	businessVO.CompanyDescription = objCompanyDescription.val();
	businessVO.Address = objAddress.val();
	businessVO.CompanySite = objCompanySite.val();
	businessVO.CompanyTel = objCompanyTel.val();

	var targetCityVOList = new Array();
	businessModelVO.TargetCity = targetCityVOList;
	var puhidenObjList = $("#TargetCityList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var businessId = puValue.split("_")[1];
	    var cityId = puValue.split("_")[2];

	    var targetCityVO = new Object();
	    targetCityVOList.push(targetCityVO);
	    targetCityVO.BusinessId = businessId;
	    targetCityVO.CityId = cityId;
	}

	var targetCategoryVOList = new Array();
	businessModelVO.TargetCategory = targetCategoryVOList;
	var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var businessId = puValue.split("_")[1];
	    var categoryId = puValue.split("_")[2];

	    var targetCategoryVO = new Object();
	    targetCategoryVOList.push(targetCategoryVO);
	    targetCategoryVO.BusinessId = businessId;
	    targetCategoryVO.CategoryId = categoryId;
	}

	var businessCategoryVOList = new Array();
	businessModelVO.BusinessCategory = businessCategoryVOList;
	var puhidenObjList = $("#BusinessCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var businessId = puValue.split("_")[1];
	    var categoryId = puValue.split("_")[2];

	    var businessCategoryVO = new Object();
	    businessCategoryVOList.push(businessCategoryVO);
	    businessCategoryVO.BusinessId = businessId;
	    businessCategoryVO.CategoryId = categoryId;
	}
    
	var businessIdCardList = new Array();
	businessModelVO.BusinessIdCard = businessIdCardList;

	var puhidenObjList = $("#divIdCard").find("img");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);

	    var businessIdCardVO = new Object();
	    businessIdCardList.push(businessIdCardVO);

	    businessIdCardVO.BusinessId = businessId;
	    businessIdCardVO.IDCardImg = puhidenObj.attr("src");
	}

	return businessModelVO;
}

function SetBusiness(businessVO) {

    var objCityId = $("select[id*='drpCity']");
	var objCompanyName = $("input[id*='txtCompanyName']");
	var ue = UE.getEditor('container');
	var ue2 = UE.getEditor('container2');
	var objSetupDate = $("input[id*='txtSetupDate']");
	var objCompanyType = $("input[id*='txtCompanyType']");
	var objBusinessLicense = $("input[id*='txtBusinessLicense']");
	var objBusinessLicenseImg = $("img[id*='imgBusinessLicense']");
	var objlinkBusinessLicense = $("a[id*='linkBusinessLicense']");
	var objCompanyLogo = $("img[id*='imgCompanyLogo']");
	//var objMainProducts = $("textarea[id*='txtMainProducts']");
	var objDescription = $("textarea[id*='Description']");
	var objStatus = $("input[id*='txtStatus']");
	var objEntrustImgPath = $("img[id*='imgEntrust']");
	var objlinkEntrust = $("a[id*='linkEntrust']");
	//var objContactPersonImgPath = $("img[id*='imgContactPerson']");
	//var objProductDescription = $("textarea[id*='txtProductDescription']");
	var objCompanyDescription = $("textarea[id*='txtCompanyDescription']");
	var objAddress = $("input[id*='txtAddress']");
	var objCompanySite = $("input[id*='txtCompanySite']");
	var objCompanyTel = $("input[id*='txtCompanyTel']");

	objCityId.val(businessVO.CityId);
	objCompanyName.val(businessVO.CompanyName);
	ue.ready(function () {
	    this.setContent(businessVO.ProductDescription);
	});
	ue2.ready(function () {
	    this.setContent(businessVO.MainProducts);
	});
	if (new Date(businessVO.SetupDate).format("yyyy-MM-dd") != "1900-01-01")
	    objSetupDate.datepicker("setDate", new Date(businessVO.SetupDate));
	objCompanyType.val(businessVO.CompanyType);
	objBusinessLicense.val(businessVO.BusinessLicense);
	objBusinessLicenseImg.attr("src", businessVO.BusinessLicenseImg);
	objlinkBusinessLicense.attr("href", businessVO.BusinessLicenseImg);
	objCompanyLogo.attr("src", businessVO.CompanyLogo);
    //objMainProducts.val(businessVO.MainProducts);
	objDescription.val(businessVO.Description);
	if (businessVO.Status == 0)
	    objStatus.val("申请认证");
	else if (businessVO.Status == 1)
	    objStatus.val("认证通过");
	else if (businessVO.Status == 2)
	    objStatus.val("认证驳回");

	SetButton(businessVO.RealNameStatus);
	
	objEntrustImgPath.attr("src", businessVO.EntrustImgPath);
	objlinkEntrust.attr("href", businessVO.EntrustImgPath);
	//objContactPersonImgPath.attr("src", businessVO.ContactPersonImgPath);
	//objProductDescription.val(businessVO.ProductDescription);
	objCompanyDescription.val(businessVO.CompanyDescription);
	objAddress.val(businessVO.Address);
	objCompanySite.val(businessVO.CompanySite);
	objCompanyTel.val(businessVO.CompanyTel);
}

function SetButton(status) {
    var btnSave = $("#btn_save");
    var btnCancelBusiness = $("#btn_updatebusinessstatus");
    var btnApproveBusiness = $("#btn_updatebusinessstatuscommit");
    var btnRejectBusiness = $("#btn_updatebusinessstatusreject");
    var divApprove = $("#divApprove");

    if (isApprove.toLowerCase() == "true") {
        btnSave.hide();
        btnCancelBusiness.hide();
        btnApproveBusiness.show();
        btnRejectBusiness.show();
        divApprove.show();
    } else {
        if (status == 0) {
            btnSave.hide();
            btnCancelBusiness.hide();
            btnApproveBusiness.show();
            btnRejectBusiness.show();
            divApprove.show();
        } else if (status == 1) {
            btnSave.show();
            btnCancelBusiness.show();
            btnApproveBusiness.hide();
            btnRejectBusiness.hide();
            divApprove.hide();
        } else if (status == 2) {
            btnSave.hide();
            btnCancelBusiness.hide();
            btnApproveBusiness.show();
            btnRejectBusiness.hide();
            divApprove.hide();
        } else if (status == 3) {
            btnSave.hide();
            btnCancelBusiness.hide();
            btnApproveBusiness.show();
            btnRejectBusiness.show();
            divApprove.show();
        }
    }
}

function Init() {
    var businessId = parseInt($("#" + hidBusinessId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var businessVO = data.Result;

                SetBusiness(businessVO);

                //绑定行业
                BindBusinessCategory(businessId);
                //绑定目标客户行业
                BindTargetCategory(businessId);
                //绑定目标客户区域
                BindTargetCity(businessId);

                //BindBusinessClient(businessId);

                BindBusinessIdCard(businessId);
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

function BindBusinessIdCard(businessId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessIdCardByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                var divObj = $("#divIdCard");
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    var liObj = "";
                    liObj += "<li> \r\n";
                    liObj += "  <a href=\"" + puVO.IDCardImg + "\" target=\"_blank\"><img alt=\"\" style=\"height:150px;\" src=\"" + puVO.IDCardImg + "\"></a> \r\n";
                    liObj += "</li>";
                    divObj.append(liObj);
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

function BindTargetCity(businessId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCityByBusiness?businessId=" + businessId + "&token=" + _Token,
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
    oTR += "    <input type=\"hidden\" value=\"TargetCity_" + puVO.BusinessId + "_" + puVO.CityId + "\" /> \r\n";
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
    var chkList = $("#TargetCityList").find("tbody input[type='checkbox']:checked");

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
                            puVO.BusinessId = parseInt($("#" + hidBusinessId).val());
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


function BindTargetCategory(businessId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
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
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.BusinessId + "_" + puVO.CategoryId + "\" /> \r\n";
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
    var chkList = $("#TargetCategoryList").find("tbody input[type='checkbox']:checked");

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
                            puVO.BusinessId = parseInt($("#" + hidBusinessId).val());
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


function BindBusinessCategory(businessId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddBusinessCategory(puVO);
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

function AddBusinessCategory(puVO) {
    var businessCategoryTable = $("#BusinessCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.BusinessId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    businessCategoryTable.append(oTR);
}

function NewBusinessCategory() {

    onChooseBusinessCategory();
}

function DeleteBusinessCategory() {
    var chkList = $("#BusinessCategoryList").find("tbody input[type='checkbox']:checked");

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

function onChooseBusinessCategory() {
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
                        var puhidenObjList = $("#BusinessCategoryList").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.BusinessId = parseInt($("#" + hidBusinessId).val());
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
                                AddBusinessCategory(puVO);
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

function NewBusinessClient() {

    onChooseBusinessClient();
}

function DeleteBusinessClient() {
    var chkList = $("#BusinessClientList").find("tbody input[type='checkbox']:checked");

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

function onChooseBusinessClient() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/BusinessClientCreateEdit.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

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

                    if (txtClientNameObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入客户名称?",
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
                    } else if (txtCompanyNameObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入公司名称?",
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
                    puVO.BusinessClientId = -1;
                    puVO.BusinessId = parseInt($("#" + hidBusinessId).val());;
                    puVO.ClientName = txtClientNameObj.val();
                    puVO.CompanyName = txtCompanyNameObj.val();
                    puVO.Description = txtDescriptionObj.val();

                    var isContain = false;
                    var puhidenObjList = $("#BusinessClientList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var companyName = puhidenObj.parent().next().next().html();
                        if (companyName == puVO.CompanyName) {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain)
                        AddBusinessClient(puVO);

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