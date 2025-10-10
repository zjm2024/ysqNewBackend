$(function () {
    $(".review-panel").mouseover(function () {
        $(".ui-shop-pop").show();
    });
    $(".review-panel").mouseout(function () {
        $(".ui-shop-pop").hide();
    });

    $("a[id*='lnkAgencyPhone']").click(function () {
        if (_CustomerId > 0) {
            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/GetAgencyPhone?agencyCustomerId=" + _AgencyCustomerId + "&token=" + _Token,
                type: "Get",
                data: null,
                async: false,
                success: function (pdata) {
                    if (pdata.Flag == 1) {
                        var objAgencyPhone = $("a[id*='lnkAgencyPhone']");
                        var lblAgencyPhone = $("span[id*='lblAgencyPhone']");
                        objAgencyPhone.hide();
                        lblAgencyPhone.text(pdata.Result);
                        lblAgencyPhone.show();
                    } else {
                        bootbox.dialog({
                            message: pdata.Message,
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

                }
            });
        } else {
            //先登录
            bootbox.dialog({
                message: "请先完成雇主身份认证登录后查看！",
                buttons:
                {
                    "Confirm":
                    {
                        "label": "确定",
                        "className": "btn-sm btn-primary",
                        "callback": function () {
                            window.location.href = "Login.aspx";
                        }
                    }
                }
            });
        }
        return false;
    });
});

function GetAgencyDetil(agencyId) {
    GetData("GetAgencySite", agencyId, function (data) {
        var titleObj = $("#lblTitle");
        var agencyLevelObj = $("#lblAgencyLevel");
        var companyNameObj = $("#lblCompanyName");
        var projectCountObj = $("#lblProjectCount");
        var reviewObj = $("#lblReview");
        var descriptionObj = $("#divDescription");

        var agencyVO = data.Result;

        titleObj.html(agencyVO.CustomerName);
        var levelName = "铜牌销售";
        if (agencyVO.AgencyLevel == 1) {
            levelName = "金牌销售";
        } else if (agencyVO.AgencyLevel == 2) {
            levelName = "银牌销售";
        } else if (agencyVO.AgencyLevel == 3) {
            levelName = "铜牌销售";
        } else if (agencyVO.AgencyLevel ==4) {
            levelName = "普通销售";
        }
        agencyLevelObj.html(levelName);
        companyNameObj.html(agencyVO.AgencyName);
        projectCountObj.html(agencyVO.ProjectCount);
        reviewObj.html(agencyVO.ReviewScore);
        descriptionObj.html(agencyVO.Description);

    }, function (data) {        
        //load_hide();
    });
}

function markObject(customerId, imgObj) {
    if (_CustomerId < 1 || _Token == "") {
        bootbox.dialog({
            message: "请先登录再进行关注！",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "Login.aspx";
                    }
                }
            }
        });
    } else {
        var markVO = new Object();
        markVO.CustomerId = _CustomerId;
        markVO.MarkObjectId = customerId;
        markVO.MarkType = 1;
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateMark?token=" + _Token,
            type: "POST",
            data: markVO,
            success: function (data) {
                if (data.Flag == 1) {
                    $(imgObj).addClass("on");
                    $(imgObj).attr("title", "取消关注");
                    $(imgObj).removeAttr("onclick");
                    $(imgObj).unbind("click");
                    $(imgObj).click(function () {
                        deleteMark(data.Result, customerId, imgObj);
                    });
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

function deleteMark(markId, customerId, imgObj) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteMark?markId=" + markId + "&token=" + _Token,
        type: "POST",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                $(imgObj).removeClass("on");
                $(imgObj).attr("title", "关注");
                $(imgObj).removeAttr("onclick");
                $(imgObj).unbind("click");
                $(imgObj).click(function () {
                    markObject(customerId, imgObj);
                });
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function onTender(customerId) {
    if (_CustomerId > 0) {
        //选择任务再进行邀请
        onChooseRequire(customerId);
    } else {
        //先登录
        bootbox.dialog({
            message: "请先登录再做邀请！",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "Login.aspx";
                    }
                }
            }
        });
    }
    return false;
}

function onChooseRequire(selectedCustomerId) {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="RequireManagement\/ChooseRequireList.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择任务",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleRequireObj = $(window.frames["iframe_1"].document).find("#ChooseRequireList").find("input[type='checkbox']:checked");
                    var selectRequireIdStr = "";
                    var requireArray = new Array();
                    for (var i = 0; i < seleRequireObj.length; i++) {
                        var require = new Object();
                        requireArray.push(require);
                        var chk = $(seleRequireObj[i]);
                        require.RequirementId = chk.parent().next()[0].innerText;
                        require.RequirementCode = chk.parent().next().next()[0].innerText;
                        require.Title = chk.parent().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidRequireId").val(JSON.stringify(requireArray));

                    var requireArray = $(window.frames["iframe_1"].document).find("#hidRequireId").val();
                    if (requireArray != "") {
                        var requireList = JSON.parse(requireArray);
                        if (requireList.length > 0) {
                            var tenderInfoVO = new Object();

                            tenderInfoVO.RequirementId = requireList[0].RequirementId;
                            tenderInfoVO.CustomerId = selectedCustomerId;

                            $.ajax({
                                url: _RootPath + "SPWebAPI/Require/UpdateRequireTenderInfo?token=" + _Token,
                                type: "POST",
                                data: tenderInfoVO,
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
                                    load_hide();
                                },
                                error: function (data) {
                                    alert(data);
                                    load_hide();
                                }
                            });
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