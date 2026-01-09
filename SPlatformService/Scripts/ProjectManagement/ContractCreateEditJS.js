$(document).ready(function () {
    Init();
    initDatePicker();

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtCost: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                number: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtCost: {
                number: "请输入正确数值"
            },
            ctl00$ContentPlaceHolder_Content$txtCommission: {
                number: "请输入正确数值"
            }
        },
        highlight: function (e) {
            $(e).closest('.hourinput').removeClass('has-info').addClass('has-error');
        },

        success: function (e) {
            $(e).closest('.hourinput').removeClass('has-error');
            $(e).remove();
        }
    });

    $("button[id$='btn_save']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        if (parseInt($("#" + hidContractId).val()) < 1) {
            //判断是否有营业执照，如果没有不能创建项目       
            if (!IsHasLincense(_BusinessId)) {
                bootbox.dialog({
                    message: "没有营业执照信息，请补齐信息！",
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

            //判断是否有身份证，如果没有不能创建项目       
            if (!IsHasIDCard(_AgencyId)) {
                bootbox.dialog({
                    message: "销售没有身份证信息，请提醒销售补齐信息！",
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

        var contractVO = GetContractVO();

        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateContract?token=" + _Token,
            type: "POST",
            data: contractVO,
            success: function (data) {
                if (data.Flag == 1) {
                    $("#" + hidContractId).val(data.Result);
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    SetButton();
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
        return false;
    });

});

function SetButton() {

}

function GetContractVO() {
    var contractModelVO = new Object();
    var contractVO = new Object();
    contractModelVO.Contract = contractVO;

    var objStartDate = $("input[id*='txtStartDate']");
    var objProjectName = $("input[id*='txtProjectName']");
	var objEndDate = $("input[id*='txtEndDate']");
	var objCost = $("input[id*='txtCost']");	
	var objCommission = $("input[id*='txtCommission']");

	contractVO.ContractId = parseInt($("#" + hidContractId).val());
	contractVO.RequirementId = _RequireId;
	contractVO.CustomerId = _AgencyCustomerId;
	contractVO.ProjectName = objProjectName.val();
	contractVO.StartDate = objStartDate.val();
	contractVO.EndDate = objEndDate.val();
	contractVO.Cost = objCost.val();
	contractVO.Commission = objCommission.val();
	
	var contractFileList = new Array();
	contractModelVO.ContractFile = contractFileList;

	var puhidenObjList = $("table[id*='FileList']").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var contractId = puValue.split("_")[1];
	    var fileId = puValue.split("_")[2];

	    var fileVO = new Object();
	    contractFileList.push(fileVO);
	    fileVO.ContractId = contractId;
	    fileVO.FileName = puhidenObj.parent().next().attr("title");
	    fileVO.FilePath = puhidenObj.parent().next().children().attr("href");
	    fileVO.CreatedAt = puhidenObj.parent().next().next().html();
	}

	var contractStepsList = new Array();
	contractModelVO.ContractSteps = contractStepsList;

	puhidenObjList = $("table[id*='ContractStepsList']").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var contractId = puValue.split("_")[1];
	    var stepsId = puValue.split("_")[2];

	    var stepsVO = new Object();
	    contractStepsList.push(stepsVO);
	    stepsVO.ContractId = contractId;
	    stepsVO.SortNO = puhidenObj.parent().next().html();
	    stepsVO.Title = puhidenObj.parent().next().next().find("input").val();
	    stepsVO.Cost = puhidenObj.parent().next().next().next().find("input").val();
	    stepsVO.Comment = puhidenObj.parent().next().next().next().next().find("textarea").val();
	    
	}

	return contractModelVO;
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
    
    setTimeout("initInputValid()", 100);
    
}
function initInputValid() {
    var dList = $("table[id*='ContractStepsList']").find("input[type='hidden']");
    for (var i = 0; i < dList.length; i++) {
        var id = $(dList[i]).val();
        $("#" + id + "_Cost").rules("add", { number: true, messages: { number: "请输入正确数值！" } });
        $("#" + id + "_Title").rules("add", { required: true, messages: { required: "请输入名称！" } });
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

function IsHasLincense(businessId) {
    var isHas = false;
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                if (data.Result.BusinessLicense == "" || data.Result.BusinessLicenseImg == "") {
                    isHas = false;
                } else {
                    isHas = true;
                }
            } 

        },
        error: function (data) {
            //alert(data);
        }
    });
    return isHas;
}
function IsHasIDCard(agencyId) {
    var isHas = false;
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                if (data.Result.IDCard == "") {
                    isHas = false;
                } else {
                    isHas = true;
                }
            }

        },
        error: function (data) {
            //alert(data);
        }
    });
    return isHas;
}

function UpdateContractStatus(contractId, status, type) {
    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/UpdateContractStatus?contractId=" + contractId + "&status=" + status + "&type=" + type + "&token=" + _Token,
        type: "POST",
        data: null,
        success: function (data) {
            load_hide();
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
                                if (type == "B") {
                                    _BusinessStatus = status;
                                } else if (type == "A") {
                                    _AgencyStatus = status;
                                }
                                SetButton();
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
                                if (type == "B") {
                                    window.location.href = "BusinessProjectCreateEdit.aspx?ProjectId=" + data.Result;
                                } else if (type == "A") {
                                    window.location.href = "AgencyProjectCreateEdit.aspx?ProjectId=" + data.Result;
                                }
                                
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
            load_hide();
            //alert(data);
        }
    });
}


function changefile(uploadId) {
    var tempPath = new Date().format("yyyyMM");
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var fileVO = new Object();
            fileVO.ContractId = parseInt($("#" + hidContractId).val());
            fileVO.FileName = data.Result.FileName;
            fileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
            fileVO.CreatedAt = new Date();
            //fileVO.CreatedBy = _CustomerId;

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

function BindFile(contractId) {
    $("table[id*='FileList'").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetContractFile?contractId=" + contractId + "&token=" + _Token,
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
            alert(data);
        }
    });
}

function AddFile(puVO) {
    var fileTable = $("table[id*='FileList'");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"File_" + puVO.ContractId + "_" + puVO.ContractFileId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedAt).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedAt).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function DeleteFile() {
    var chkList = $("table[id*='FileList']").find("input[type='checkbox']:checked");

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


function NewContractSteps() {
    var puVO = new Object();
    puVO.ContractId = parseInt($("#" + hidContractId).val());
    puVO.Title = "";
    puVO.Comment = "";
    puVO.Cost = "";

    AddSteps(puVO);
}

function BindSteps(contractId) {
    $("table[id*='FileList'").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetContractSteps?contractId=" + contractId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    AddSteps(puVO);
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

function AddSteps(puVO) {
    var fileTable = $("table[id*='ContractStepsList'");

    var puhidenObjList = $("table[id*='ContractStepsList']").find("input[type='hidden']");
    var num = puhidenObjList.length + 1;
    var id = "Steps_" + puVO.ContractId + "_" + puVO.ContractStepsId + "_" + num;
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"" + id + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + num + "\">" + num + "</td> \r\n";
    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + puVO.Title + "\"> \r\n";
    oTR += "    <div class=\"hourinput\"> \r\n";
    oTR += "      <input id=\"" + id + "_Title\" name=\"" + id + "_Title\" maxlength=\"50\" class=\"col-xs-10 col-sm-5 \" type=\"text\" style=\"width: 100%;\" value=\"" + puVO.Title + "\" /> \r\n";
    oTR += "    </div> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"" + puVO.Cost + "\"> \r\n";
    oTR += "    <div class=\"hourinput\"> \r\n";
    oTR += "      <input id=\"" + id + "_Cost\" name=\"" + id + "_Cost\" class=\"col-xs-10 col-sm-5 text-right\" type=\"text\" style=\"width: 100%;\" value=\"" + puVO.Cost + "\" /> \r\n";
    oTR += "    </div> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" style=\"vertical-align: middle;\" title=\"\"> \r\n";
    oTR += "    <div class=\"hourinput\"> \r\n";
    oTR += "      <textarea id=\"" + id + "_Comment\" name=\"" + id + "_Comment\" maxlength=\"400\" class=\"col-xs-10 col-sm-5 \" type=\"text\" style=\"width: 100%;\" value=\"" + puVO.Comment + "\" /> \r\n";
    oTR += "    </div> \r\n";
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);

    $("#" + id + "_Cost").rules("add", { number: true, messages: { number: "请输入正确数值！" } });
    $("#" + id + "_Title").rules("add", { required: true, messages: { required: "请输入名称！" } });

}

function DeleteContractSteps() {
    var chkList = $("table[id*='ContractStepsList']").find("input[type='checkbox']:checked");

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

                        var dList = $("table[id*='ContractStepsList']").find("input[type='hidden']");
                        for (var i = 0; i < dList.length; i++) {
                            var id = $(dList[i]).val();
                            $(dList[i]).parent().next().html(i + 1);
                            $(dList[i]).parent().next().attr("title", (i + 1));
                            $(dList[i]).parent().next().next().next().find("input").attr("name", id + "_Cost");
                            $(dList[i]).parent().next().next().next().find("input").attr("id", id + "_Cost");
                            $("#" + id + "_Cost").rules("add", { number: true, messages: { number: "请输入正确数值！" } });

                            $(dList[i]).parent().next().next().find("input").attr("name", id + "_Title");
                            $(dList[i]).parent().next().next().find("input").attr("id", id + "_Title");
                            $("#" + id + "_Title").rules("add", { required: true, messages: { required: "请输入名称！" } });
                        }
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