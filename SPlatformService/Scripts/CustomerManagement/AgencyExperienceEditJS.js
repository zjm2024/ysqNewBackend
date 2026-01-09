$(document).ready(function () {
    initDatePicker();
    SetButton(0);
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
            }

        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入标题！"
            },
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: "请输入描述！"
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

        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        var agencyExperienceVO = GetAgencyExperienceVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateAgencyExperience?token=" + _Token,
            type: "POST",
            data: agencyExperienceVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (agencyExperienceId < 1) {
                        $("#" + hidAgencyExperienceId).val(data.Result);
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

    $("#btn_updateagencyexperiencestatus").click(function () {
        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        bootbox.dialog({
            message: "是否确认取消审核?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyExperienceStatusAction(agencyExperienceId, 2, "");
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

    $("#btn_updateagencyexperiencestatuscommit").click(function () {
        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        bootbox.dialog({
            message: "是否确认通过?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyExperienceStatusAction(agencyExperienceId, 1, "");
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

    $("#btn_updateagencyexperiencestatusreject").click(function () {
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

        var agencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        bootbox.dialog({
            message: "是否确认驳回?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateAgencyExperienceStatusAction(agencyExperienceId, 2, objApproveComment.val());
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
        window.location.href = "AgencyExperienceApproveBrowse.aspx";
        return false;
    });

});

function UpdateAgencyExperienceStatusAction(agencyExperienceId, status, approveComment) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateAgencyExperienceStatus?agencyExperienceId=" + agencyExperienceId + "&approveComment=" + approveComment + "&status=" + status + "&token=" + _Token,
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
                                window.location.href = "AgencyExperienceApproveBrowse.aspx";
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
    var objMainImg = $("img[id*='imgMainPic']");

    agencyExperienceVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());

    agencyExperienceVO.Title = objTitle.val();
    agencyExperienceVO.ProjectDate = objProjectDate.val();
    agencyExperienceVO.Description = objDescription.val();
    agencyExperienceVO.MainImg = objMainImg.attr("src");

    var agencyDetailImgList = new Array();
    agencyExperienceVO.AgencyExperienceImageList = agencyDetailImgList;

    var puhidenObjList = $("#divDetail").find("img");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);

        var agencyDetailVO = new Object();
        agencyDetailImgList.push(agencyDetailVO);

        agencyDetailVO.AgencyExperienceId = parseInt($("#" + hidAgencyExperienceId).val());
        agencyDetailVO.ImgPath = puhidenObj.attr("src");
    }

    return agencyExperienceVO;
}

function SetAgencyExperience(agencyExperienceVO) {

    var objTitle = $("input[id*='txtTitle']");
    var objProjectDate = $("input[id*='txtProjectDate']");
    var objDescription = $("textarea[id*='txtDescription']");
    var objMainImg = $("img[id*='imgMainPic']");

    objTitle.val(agencyExperienceVO.Title);
    if (new Date(objProjectDate.ProjectDate).format("yyyy-MM-dd") != "1900-01-01")
        objProjectDate.datepicker("setDate", new Date(agencyExperienceVO.ProjectDate));

    objDescription.val(agencyExperienceVO.Description);
    objMainImg.attr("src", agencyExperienceVO.MainImg);

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
    }
}

function SetButton(status) {
    var btnSave = $("#btn_save");
    var btnCancelAgency = $("#btn_updateagencyexperiencestatus");
    var btnApproveAgency = $("#btn_updateagencyexperiencestatuscommit");
    var btnRejectAgency = $("#btn_updateagencyexperiencestatusreject");
    var divApprove = $("#divApprove");

    if (status == 0) {
        btnSave.hide();
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
        btnSave.hide();
        btnCancelAgency.hide();
        btnApproveAgency.hide();
        btnRejectAgency.hide();
        divApprove.hide();
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

function change(uploadId, divId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            var divObj = $("#" + divId);
            var liObj = "";
            liObj += "<li> \r\n";
            liObj += "  <img alt=\"\" style=\"height:150px;\" src=\"" + data.Result.FilePath.replace("~", _APIURL) + "\"> \r\n";
            liObj += "  <div class=\"tools\"> \r\n";
            liObj += "      <a href=\"#\" onclick=\"removeImg(this)\"> \r\n";
            liObj += "          <i class=\"ace-icon fa fa-times red\"></i> \r\n";
            liObj += "      </a> \r\n";
            liObj += "  </div> \r\n";
            liObj += "</li>";
            divObj.append(liObj);
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
    var divObj = $("#divDetail");
    for (var i = 0; i < puVOList.length; i++) {
        var puVO = puVOList[i];
        var liObj = "";
        liObj += "<li> \r\n";
        liObj += "  <img alt=\"\" style=\"height:150px;\" src=\"" + puVO.ImgPath + "\"> \r\n";
        liObj += "  <div class=\"tools\"> \r\n";
        liObj += "      <a href=\"#\" onclick=\"removeImg(this)\"> \r\n";
        liObj += "          <i class=\"ace-icon fa fa-times red\"></i> \r\n";
        liObj += "      </a> \r\n";
        liObj += "  </div> \r\n";
        liObj += "</li>";
        divObj.append(liObj);
    }

}
