$(document).ready(function () {
    initDatePicker();
    Init();

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtClientName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtContractAmount: {
                required: true
            }

        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: "请输入描述！"
            },
            ctl00$ContentPlaceHolder_Content$txtClientName: {
                required: "请输入客户名称！"
            },
            ctl00$ContentPlaceHolder_Content$txtContractAmount: {
                required: "请输入合同金额！"
            },
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

        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        var agencyExperienceVO = GetAgencyExperienceVO();
        agencyExperienceVO.Status = -1;
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateAgencyExperience?token=" + _Token,
            type: "POST",
            data: agencyExperienceVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (agencyExperienceId < 1) {
                        $("#" + hidAgencyExperienceId).val(data.Result);
                        SetButton(agencyExperienceVO.Status);
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
                        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
                        DeleteAction(agencyExperienceId);
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

    $("#btn_submit").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        var agencyExperienceVO = GetAgencyExperienceVO();
        agencyExperienceVO.Status = 0;
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateAgencyExperience?token=" + _Token,
            type: "POST",
            data: agencyExperienceVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (agencyExperienceId < 1) {
                        $("#" + hidAgencyExperienceId).val(data.Result);
                        SetButton(agencyExperienceVO.Status);
                    }
                    bootbox.dialog({
                        message: "提交成功，请等待审核！",
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    $("span[id*='lblNotice']").show();
                                    $("button[id*='btn_submit']").hide();
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

    $("textarea[id$='txtDescription']").attr("maxlength", "400");
});
function DeleteAction(agencyExperienceId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteAgencyExperience?agencyExperienceId=" + agencyExperienceId + "&token=" + _Token,
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
                                window.location.href = "AgencyExperienceBrowse.aspx";
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

function GetAgencyExperienceVO() {
    var agencyExperienceVO = new Object();

    var objTitle = $("input[id*='txtTitle']");
    var objProjectDate = $("input[id*='txtProjectDate']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objClientName = $("input[id*='txtClientName']");
    var objContractAmount = $("input[id*='txtContractAmount']");
    var objMainImg = $("img[id*='imgMainPic']");

    agencyExperienceVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());

    agencyExperienceVO.Title = objTitle.val();
    agencyExperienceVO.ProjectDate = objProjectDate.val();
    agencyExperienceVO.Description = objDescription.val();
    agencyExperienceVO.MainImg = objMainImg.attr("src");
    agencyExperienceVO.ClientName = objClientName.val();
    agencyExperienceVO.ContractAmount = objContractAmount.val();

    var agencyDetailImgList = new Array();
    agencyExperienceVO.AgencyExperienceImageList = agencyDetailImgList;

    //var puhidenObjList = $("#divDetail").find("img");
    //for (var i = 0; i < puhidenObjList.length; i++) {
    //    var puhidenObj = $(puhidenObjList[i]);

    //    var agencyDetailVO = new Object();
    //    agencyDetailImgList.push(agencyDetailVO);
    //    agencyDetailVO.TypeId = 1;
    //    agencyDetailVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
    //    agencyDetailVO.ImgPath = puhidenObj.attr("src");
    //}
    //var puhidenObjList = $("#divDetail2").find("img");
    //for (var i = 0; i < puhidenObjList.length; i++) {
    //    var puhidenObj = $(puhidenObjList[i]);

    //    var agencyDetailVO = new Object();
    //    agencyDetailImgList.push(agencyDetailVO);
    //    agencyDetailVO.TypeId = 2;
    //    agencyDetailVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
    //    agencyDetailVO.ImgPath = puhidenObj.attr("src");
    //}


    var puhidenObjList = $("#FileList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) { 

        var puhidenObj = $(puhidenObjList[i]);
        var agencyDetailVO = new Object();
        agencyDetailImgList.push(agencyDetailVO);
        agencyDetailVO.TypeId = 1;
        agencyDetailVO.FileName = puhidenObj.parent().next()[0].childNodes[0].innerText;
        agencyDetailVO.CreatedDate = puhidenObj.parent().next().next().html();
        agencyDetailVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());     
        agencyDetailVO.ImgPath = puhidenObj.val();

    }
    var puhidenObjList = $("#FileList2").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {

        var puhidenObj = $(puhidenObjList[i]);
        var agencyDetailVO = new Object();
        agencyDetailImgList.push(agencyDetailVO);
        agencyDetailVO.TypeId = 2;
        agencyDetailVO.FileName = puhidenObj.parent().next()[0].childNodes[0].innerText;
        agencyDetailVO.CreatedDate = puhidenObj.parent().next().next().html();
        agencyDetailVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        agencyDetailVO.ImgPath = puhidenObj.val();

    }

    return agencyExperienceVO;
}

function SetAgencyExperience(agencyExperienceVO) {

    var objTitle = $("input[id*='txtTitle']");
    var objProjectDate = $("input[id*='txtProjectDate']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objMainImg = $("img[id*='imgMainPic']");
    var objClientName = $("input[id*='txtClientName']");
    var objContractAmount = $("input[id*='txtContractAmount']");

    objTitle.val(agencyExperienceVO.Title);
    if (new Date(objProjectDate.ProjectDate).format("yyyy-MM-dd") != "1900-01-01")
        objProjectDate.datepicker("setDate", new Date(agencyExperienceVO.ProjectDate));

    objDescription.val(agencyExperienceVO.Description);
    objMainImg.attr("src", agencyExperienceVO.MainImg);
    objClientName.val(agencyExperienceVO.ClientName);
    objContractAmount.val(agencyExperienceVO.ContractAmount);

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

    var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());

    if (agencyExperienceId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetAgencyExperience?agencyExperienceId=" + agencyExperienceId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var agencyExperienceVO = data.Result;

                    SetButton(agencyExperienceVO.Status);

                    SetAgencyExperience(agencyExperienceVO);

                    BindAgencyDetailImg(agencyExperienceVO.AgencyExperienceImageList);
                }
                load_hide();
            },
            error: function (data) {
                alert(data);
                load_hide();
            }
        });
    } else {
        SetButton(-1);
    }
}

function SetButton(status) {
    var objDivApprove = $("div[id*='divApprove']");
    var objSubmit = $("button[id*='btn_submit']");
    var objSave = $("button[id*='btn_save']");
    var objDelete = $("button[id*='btn_delete']");
    var objNotice = $("span[id*='lblNotice']");

    if (status == 0) {
        objDivApprove.hide();
        objSubmit.hide();
        objSave.hide();
        objDelete.hide();
        objNotice.show();
    } else if (status == 1) {
        objDivApprove.hide();
        objSubmit.hide();
        objSave.show();
        objDelete.show();
        objNotice.hide();
    } else if (status == 2) {
        objDivApprove.show();
        objSubmit.show();
        objSave.hide();
        objDelete.show();
        objNotice.hide();

        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        //绑定驳回原因 
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/GetAgencyExperienceApproveInfo?agencyExperienceId=" + agencyExperienceId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var agencyExperienceApproveInfoVO = data.Result;
                    var objApproveComment = $("textarea[id*='txtApproveComment']");
                    objApproveComment.val(agencyExperienceApproveInfoVO.ApproveComment)
                }
            },
            error: function (data) {
            }
        });
    } else {
        objDivApprove.hide();
        objSubmit.show();
        objSave.show();
        objDelete.show();
        objNotice.hide();
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


function AddFile1(puVO) {
    var fileTable = $("#FileList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"" + puVO.ImgPath + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.ImgPath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function AddFile2(puVO) {
    var fileTable = $("#FileList2");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"" + puVO.ImgPath + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.ImgPath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}


function DeleteFile2() {
    var chkList = $("#FileList2").find("input[type='checkbox']:checked");
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
function changefile(uploadId) {
    var tempPath = _CustomerId;
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var agencyDetailVO = new Object();
            agencyDetailVO.CreatedDate = new Date();
            agencyDetailVO.FileName = data.Result.FileName;
            agencyDetailVO.ImgPath = data.Result.FilePath.replace("~", _APIURL);
            if (uploadId == 'fileAdd2') {
                agencyDetailVO.TypeId = 2;
                AddFile2(agencyDetailVO);
            }
            else {
                agencyDetailVO.TypeId = 1;
                AddFile1(agencyDetailVO);
            }
            //直接保存

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

function removeImg(obj) {
    $(obj).closest("li").remove();
}

function changeone(uploadId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("#" + uploadId + "Pic").attr("src", data.Result.FilePath.replace("~", _APIURL));
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

function BindAgencyDetailImg(agencyExperienceDetailImgList) {

    var puVOList = agencyExperienceDetailImgList;  
    for (var i = 0; i < puVOList.length; i++) {
        var puVO = puVOList[i];
      
        if (puVO.TypeId == 2)
            AddFile2(puVO);
        else
            AddFile1(puVO);

    }

}
