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
            ctl00$ContentPlaceHolder_Content$txtAgencyName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$drpCity: {
                ddlrequired: "请选择区域！"
            },
            ctl00$ContentPlaceHolder_Content$txtAgencyName: {
                required: "请输入销售名称！"
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

        var agencyId = parseInt($("#" + hidAgencyId).val());
        var agencyVO = GetAgencyVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateAgency?token=" + _Token,
            type: "POST",
            data: agencyVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (agencyId < 1) {
                        $("#" + hidAgencyId).val(data.Result);
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

    $("#btn_updateagencystatus").click(function () {
        var agencyId = parseInt($("#" + hidAgencyId).val());
        bootbox.dialog({
            message: "是否确认取消认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyStatusAction(agencyId, 2, "");
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

    $("#btn_updateagencystatuscommit").click(function () {
        var agencyId = parseInt($("#" + hidAgencyId).val());
        bootbox.dialog({
            message: "是否确认通过认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyStatusAction(agencyId, 1, "");
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

    $("#btn_updateagencystatusreject").click(function () {
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

        var agencyId = parseInt($("#" + hidAgencyId).val());
        bootbox.dialog({
            message: "是否确认驳回认证?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyStatusAction(agencyId, 2, objApproveComment.val());
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
        window.location.href = "AgencyBrowse.aspx";
        return false;
    });


});

function UpdateAgencyStatusAction(agencyId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateAgencyStatus?agencyId=" + agencyId + "&approveComment=" + approveComment + "&status=" + status + "&type=B&token=" + _Token,
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
                                window.location.href = "AgencyBrowse.aspx";
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


function GetAgencyVO() {
    var agencyModelVO = new Object();
    var agencyVO = new Object();

    agencyModelVO.Agency = agencyVO;
    var objCityId = $("select[id*='drpCity']");
    var objAgencyName = $("input[id*='txtAgencyName']");
    var ue = UE.getEditor("container");
    var objIDCard = $("input[id*='txtIDCard']");
    var objTechnical = $("textarea[id*='txtTechnical']");
    var objAgencyLevel = $("select[id*='drpAgencyLevel']");
    var objContactsResources = $("textarea[id*='txtContactsResources']");
    var objFamiliarProduct = $("textarea[id*='txtFamiliarProduct']");
    var objFamiliarClient = $("textarea[id*='txtFamiliarClient']");
    var objSpecialty = $("textarea[id*='txtSpecialty']");
    var objFeature = $("textarea[id*='txtFeature']");
    var objShortDescription = $("input[id*='txtShortDescription']");
    var objCompany = $("input[id*='txtCompany']");
    var objPosition = $("input[id*='txtPosition']");
    var objPersonalCard = $("#divPersonalCard").find("img");
    var objStatus = $("input[id*='txtStatus']");
    var objSchool = $("input[id*='txtSchool']");
    
  
    agencyVO.AgencyId = parseInt($("#" + hidAgencyId).val());
    //agencyVO.CustomerId = _CustomerId;
    agencyVO.CityId = objCityId.val();
    agencyVO.AgencyName = objAgencyName.val();
    agencyVO.Description = ue.getContent();
    agencyVO.IDCard = objIDCard.val();
    agencyVO.Technical = objTechnical.val();
    agencyVO.AgencyLevel = objAgencyLevel.val();
    agencyVO.ContactsResources = objContactsResources.val();
    agencyVO.FamiliarProduct = objFamiliarProduct.val();
    agencyVO.FamiliarClient = objFamiliarClient.val();

    agencyVO.Specialty = objSpecialty.val();
    agencyVO.Feature = objFeature.val();
    agencyVO.ShortDescription = objShortDescription.val();
    agencyVO.Company = objCompany.val();
    agencyVO.Position = objPosition.val();
    agencyVO.PersonalCard = objPersonalCard.attr("src");
    agencyVO.School = objSchool.val();

    var agencyCityVOList = new Array();
    agencyModelVO.AgencyCity = agencyCityVOList;
    var puhidenObjList = $("#AgencyCityList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var cityId = puValue.split("_")[2];

        var agencyCityVO = new Object();
        agencyCityVOList.push(agencyCityVO);
        agencyCityVO.AgencyId = agencyId;
        agencyCityVO.CityId = cityId;
    }

    var agencyCategoryVOList = new Array();
    agencyModelVO.AgencyCategory = agencyCategoryVOList;
    var puhidenObjList = $("#AgencyCategoryList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var categoryId = puValue.split("_")[2];

        var agencyCategoryVO = new Object();
        agencyCategoryVOList.push(agencyCategoryVO);
        agencyCategoryVO.AgencyId = agencyId;
        agencyCategoryVO.CategoryId = categoryId;
    }

    var agencyIdCardList = new Array();
    agencyModelVO.AgencyIdCard = agencyIdCardList;

    var puhidenObjList = $("#divIdCard").find("img");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);

        var agencyIdCardVO = new Object();
        agencyIdCardList.push(agencyIdCardVO);

        agencyIdCardVO.AgencyId = parseInt($("#" + hidAgencyId).val());
        agencyIdCardVO.IDCardImg = puhidenObj.attr("src");
    }

    var agencyTechnicalList = new Array();
    agencyModelVO.AgencyTechnical = agencyTechnicalList;

    var puhidenObjList = $("#divTechnical").find("img");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);

        var agencyTechnicalVO = new Object();
        agencyTechnicalList.push(agencyTechnicalVO);

        agencyTechnicalVO.AgencyId = parseInt($("#" + hidAgencyId).val());
        agencyTechnicalVO.TechnicalImg = puhidenObj.attr("src");
    }

    var agencySuperClientVOList = new Array();
    agencyModelVO.AgencySuperClient = agencySuperClientVOList;
    var puhidenObjList = $("#AgencySuperClientList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var agencySuperClientId = puValue.split("_")[2];

        var agencySuperClientVO = new Object();
        agencySuperClientVOList.push(agencySuperClientVO);
        agencySuperClientVO.AgencyId = agencyId;
        agencySuperClientVO.AgencySuperClientId = agencySuperClientId;
        agencySuperClientVO.SuperClientName = puhidenObj.parent().next().html();
        //agencySuperClientVO.CompanyName = puhidenObj.parent().next().next().html();
        agencySuperClientVO.Description = puhidenObj.parent().next().next().html();
    }


    var agencySolutionVOList = new Array();
    agencyModelVO.AgencySolution = agencySolutionVOList;
    var puhidenObjList = $("#AgencySolutionList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var agencySolutionId = puValue.split("_")[2];

        var agencySolutionVO = new Object();
        agencySolutionVOList.push(agencySolutionVO);
        agencySolutionVO.AgencyId = agencyId;
        agencySolutionVO.AgencySolutionId = agencySolutionId;
        agencySolutionVO.ClientName = puhidenObj.parent().next().html();
        agencySolutionVO.ProjectName = puhidenObj.parent().next().next().html();
        agencySolutionVO.Cost = puhidenObj.parent().next().next().next().html();
        agencySolutionVO.ProjectDate = puhidenObj.parent().next().next().next().next().html();

        var fileList = puhidenObj.parent().next().next().next().next().next().find("a");
        var agencySolutionFileVOList = new Array();
        agencySolutionVO.AgencySolutionFileList = agencySolutionFileVOList;
        for (var j = 0; j < fileList.length; j++) {
            var fileVO = new Object();
            agencySolutionFileVOList.push(fileVO);

            fileVO.AgencySolutionId = agencySolutionId;
            fileVO.FileName = $(fileList[j]).html();
            fileVO.FilePath = $(fileList[j]).attr("href");
        }
    }

    return agencyModelVO;
}

function SetAgency(agencyVO) {

    var objCityId = $("select[id*='drpCity']");
    var objAgencyName = $("input[id*='txtAgencyName']");

    var objIDCard = $("input[id*='txtIDCard']");
    var objTechnical = $("textarea[id*='txtTechnical']");
    var objAgencyLevel = $("select[id*='drpAgencyLevel']");
    var objContactsResources = $("textarea[id*='txtContactsResources']");
    var objFamiliarProduct = $("textarea[id*='txtFamiliarProduct']");
    var objFamiliarClient = $("textarea[id*='txtFamiliarClient']");
    var objSpecialty = $("textarea[id*='txtSpecialty']");
    var objFeature = $("textarea[id*='txtFeature']");
    var objShortDescription = $("input[id*='txtShortDescription']");
    var objCompany = $("input[id*='txtCompany']");
    var objPosition = $("input[id*='txtPosition']");
    var objPersonalCard = $("#divPersonalCard").find("img");
    var objStatus = $("input[id*='txtStatus']");
    //var objStatus = $("input[id*='txtStatus']");
    var objSchool = $("input[id*='txtSchool']");

    //BindCity(agencyVO.ProvinceId, agencyVO.CityId);
    objAgencyName.val(agencyVO.AgencyName);


    var ue = UE.getEditor("container");
    ue.ready(function () {
        this.setContent(agencyVO.Description);
    });


    objIDCard.val(agencyVO.IDCard);
    objTechnical.val(agencyVO.Technical);
    objAgencyLevel.val(agencyVO.AgencyLevel);
    objContactsResources.val(agencyVO.ContactsResources);
    objFamiliarProduct.val(agencyVO.FamiliarProduct);
    objFamiliarClient.val(agencyVO.FamiliarClient);
    objSpecialty.val(agencyVO.Specialty);
    objFeature.val(agencyVO.Feature);
    objSchool.val(agencyVO.School);
    objShortDescription.val(agencyVO.ShortDescription);
    objCompany.val(agencyVO.Company);
    objPosition.val(agencyVO.Position);
    objPersonalCard.attr("src", agencyVO.PersonalCard);
    if (agencyVO.Status == 0) 
        objStatus.val("申请认证");        
    else if (agencyVO.Status == 1) 
        objStatus.val("认证通过");
    else if (agencyVO.Status == 2) 
        objStatus.val("认证驳回");

    SetButton(agencyVO.RealNameStatus);
   
    //objEntrustImgPath.val(agencyVO.EntrustImgPath);
    //objContactPersonImgPath.val(agencyVO.ContactPersonImgPath);

}

function SetButton(status) {
    var btnSave = $("#btn_save");
    var btnCancelAgency = $("#btn_updateagencystatus");
    var btnApproveAgency = $("#btn_updateagencystatuscommit");
    var btnRejectAgency = $("#btn_updateagencystatusreject");
    var divApprove = $("#divApprove");

    if (isApprove.toLowerCase() == "true") {
        btnSave.hide();
        btnCancelAgency.hide();
        btnApproveAgency.show();
        btnRejectAgency.show();
        divApprove.show();
    } else {
        if (status == 0) {
            btnSave.show();
            btnCancelAgency.hide();
            btnApproveAgency.show();
            btnRejectAgency.show();
            divApprove.show();
        } else if (status == 1) {
            btnSave.show();
            btnCancelAgency.show();
            btnApproveAgency.hide();
            btnRejectAgency.hide();
            divApprove.hide();
        } else if (status == 2) {
            btnSave.show();
            btnCancelAgency.hide();
            btnApproveAgency.show();
            btnRejectAgency.hide();
            divApprove.hide();
        } else if (status == 3) {
            btnSave.show();
            btnCancelAgency.hide();
            btnApproveAgency.show();
            btnRejectAgency.show();
            divApprove.show();
        }
    }
}

function Init() {
    var agencyId = parseInt($("#" + hidAgencyId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var agencyVO = data.Result;

                SetAgency(agencyVO);

                //绑定行业
                BindAgencyCategory(agencyId);
                //绑定擅长区域
                BindAgencyCity(agencyId);

                BindAgencyIdCard(agencyId);

                BindAgencyTechnical(agencyId);

                BindAgencySuperClient(agencyId);

                BindAgencySolution(agencyId);
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

function BindAgencyIdCard(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyIdCardByAgency?agencyId=" + agencyId + "&token=" + _Token,
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
                    liObj += "  <a href=\"" + puVO.IDCardImg + "\" target=\"_blank\"><img alt=\"\" style=\"height:150px;\" src=\"" + puVO.IDCardImg + "\"></a>\r\n";
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

function BindAgencyTechnical(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyTechnicalByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                var divObj = $("#divTechnical");
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    var liObj = "";
                    liObj += "<li> \r\n";
                    liObj += "  <img alt=\"\" style=\"height:150px;\" src=\"" + puVO.TechnicalImg + "\"> \r\n";                    
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

function BindAgencyCity(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyCityByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddAgencyCity(puVO);
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

function AddAgencyCity(puVO) {
    var agencyCityTable = $("#AgencyCityList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"AgencyCity_" + puVO.AgencyId + "_" + puVO.CityId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProvinceName + "\">" + puVO.ProvinceName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CityName + "\">" + puVO.CityName + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencyCityTable.append(oTR);
}

function NewAgencyCity() {

    onChooseCity();
}

function DeleteAgencyCity() {
    var chkList = $("#AgencyCityList").find("tbody input[type='checkbox']:checked");

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
                        var puhidenObjList = $("#AgencyCityList").find("input[type='hidden']");
                        for (var i = 0; i < cityList.length; i++) {
                            var cityObj = cityList[i];
                            var puVO = new Object();
                            puVO.AgencyId = parseInt($("#" + hidAgencyId).val());
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
                                AddAgencyCity(puVO);
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


function BindAgencyCategory(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyCategoryByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddAgencyCategory(puVO);
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

function AddAgencyCategory(puVO) {
    var agencyCategoryTable = $("#AgencyCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.AgencyId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencyCategoryTable.append(oTR);
}

function NewAgencyCategory() {

    onChooseAgencyCategory();
}

function DeleteAgencyCategory() {
    var chkList = $("#AgencyCategoryList").find("tbody input[type='checkbox']:checked");

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

function onChooseAgencyCategory() {
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
                        var puhidenObjList = $("#AgencyCategoryList").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.AgencyId = parseInt($("#" + hidAgencyId).val());
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
                                AddAgencyCategory(puVO);
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


function BindAgencySuperClient(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencySuperClientByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddAgencySuperClient(puVO);
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

function AddAgencySuperClient(puVO) {
    var agencySuperClientTable = $("#AgencySuperClientList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSuperClient_" + puVO.AgencyId + "_" + puVO.AgencySuperClientId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.SuperClientName + "\">" + puVO.SuperClientName + "</td> \r\n";
    //oTR += "  <td class=\"center\" title=\"" + puVO.CompanyName + "\">" + puVO.CompanyName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + puVO.Description + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencySuperClientTable.append(oTR);
}

function NewAgencySuperClient() {

    onChooseAgencySuperClient();
}

function DeleteAgencySuperClient() {
    var chkList = $("#AgencySuperClientList").find("tbody input[type='checkbox']:checked");

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

function onChooseAgencySuperClient() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/AgencySuperClientCreateEdit.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "添加优势客户",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtSuperClientNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtSuperClientName']");
                    //var txtCompanyNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtCompanyName']");
                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtDescription']");

                    if (txtSuperClientNameObj.val() == "") {
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
                    }
                    //else if (txtCompanyNameObj.val() == "") {
                    //    bootbox.dialog({
                    //        message: "请输入公司名称?",
                    //        buttons:
                    //        {
                    //            "click":
                    //            {
                    //                "label": "确定",
                    //                "className": "btn-sm btn-primary",
                    //                "callback": function () {
                    //                }
                    //            },
                    //            "Cancel":
                    //            {
                    //                "label": "取消",
                    //                "className": "btn-sm",
                    //                "callback": function () {
                    //                }
                    //            }
                    //        }
                    //    });
                    //    return false;
                    //}

                    var puVO = new Object();
                    puVO.AgencySuperClientId = -1;
                    puVO.AgencyId = parseInt($("#" + hidAgencyId).val());
                    puVO.SuperClientName = txtSuperClientNameObj.val();
                    puVO.CompanyName = "";
                    puVO.Description = txtDescriptionObj.val();

                    var isContain = false;
                    var puhidenObjList = $("#AgencySuperClientList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var superClientName = puhidenObj.parent().next().html();
                        if (superClientName == puVO.SuperClientName) {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain)
                        AddAgencySuperClient(puVO);

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


function BindAgencySolution(agencyId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencySolutionByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddAgencySolution(puVO);
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

function AddAgencySolution(puVO) {
    var agencySolutionTable = $("#AgencySolutionList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSolution_" + puVO.AgencyId + "_" + puVO.AgencySolutionId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ClientName + "\">" + puVO.ClientName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProjectName + "\">" + puVO.ProjectName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Cost + "\">" + puVO.Cost + "</td> \r\n";
    oTR += "  <td class=\"center\" > \r\n";
    for (var i = 0; i < puVO.AgencySolutionFileList.length; i++) {
        oTR += "<a target=\"_blank\" href=\"" + puVO.AgencySolutionFileList[i].FilePath + "\">" + puVO.AgencySolutionFileList[i].FileName + "</a>";
    }
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    agencySolutionTable.append(oTR);
}

function NewAgencySolution() {

    onChooseAgencySolution();
}

function DeleteAgencySolution() {
    var chkList = $("#AgencySolutionList").find("tbody input[type='checkbox']:checked");

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

function onChooseAgencySolution() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/AgencySolutionCreateEdit.aspx" height="400px" width="100%" frameborder="0"><\/iframe>',

        title: "添加典型案例",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtClientNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtClientName']");
                    var txtProjectNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtProjectName']");
                    var txtCostObj = $(window.frames["iframe_1"].document).find("input[id*='txtCost']");
                    var txtProjectDateObj = $(window.frames["iframe_1"].document).find("input[id*='txtProjectDate']");

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
                    } else if (txtProjectNameObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入项目名称?",
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
                    } else if (txtCostObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入项目金额?",
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
                    puVO.AgencySolutionId = -1;
                    puVO.AgencyId = parseInt($("#" + hidAgencyId).val());
                    puVO.ClientName = txtClientNameObj.val();
                    puVO.ProjectName = txtProjectNameObj.val();
                    puVO.Cost = txtCostObj.val();
                    puVO.ProjectDate = txtProjectDateObj.val();
                    var agencySolutionFileList = new Array();
                    puVO.AgencySolutionFileList = agencySolutionFileList;

                    var puhidenObjList = $(window.frames["iframe_1"].document).find("table[id*='FileList']").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {

                        var agencySolutionFileVO = new Object();
                        agencySolutionFileList.push(agencySolutionFileVO);


                        var puhidenObj = $(puhidenObjList[i]);
                        var puValue = puhidenObj.val();
                        var agencySolutionId = puValue.split("_")[1];
                        var agencySolutionFileId = puValue.split("_")[2];

                        agencySolutionFileVO.AgencySolutionId = agencySolutionId;
                        agencySolutionFileVO.AgencySolutionFileId = agencySolutionFileId;
                        agencySolutionFileVO.FileName = puhidenObj.parent().next().find("a").html();
                        agencySolutionFileVO.FilePath = puhidenObj.parent().next().find("a").attr("href");
                    }

                    var isContain = false;
                    var puhidenObjList = $("#AgencySolutionList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var projectName = puhidenObj.parent().next().next().html();
                        if (projectName == puVO.ProjectName) {
                            isContain = true;
                            break;
                        }
                    }

                    if (!isContain)
                        AddAgencySolution(puVO);

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
