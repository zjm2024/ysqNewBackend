$(function () {
    //var requireId = GetQueryString("requireId");
    //GetReuireDetil(requireId);
    $("a[id*='lnkBusinessPhone']").click(function () {
        if (_CustomerId > 0) {
            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/GetBusinessPhone?businessCustomerId=" + _BusinessCustomerId + "&token=" + _Token,
                type: "Get",
                data: null,
                async: false,
                success: function (pdata) {
                    if (pdata.Flag == 1) {
                        var objBusinessPhone = $("a[id*='lnkBusinessPhone']");
                        var lblBusinessPhone = $("span[id*='lblBusinessPhone']");
                        objBusinessPhone.hide();
                        lblBusinessPhone.text(pdata.Result);
                        lblBusinessPhone.show();
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
                message: "请先登录再查看！",
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

function GetReuireDetil(requireId) {
    GetData("GetRequireSite", requireId, function (data) {
        var titleObj = $("#lblTitle");
        var businessNameObj = $("#lblBusinessName");
        var commissionObj = $("#lblCommission");
        var categoryNameObj = $("#lblCategoryName");
        var createdAtObj = $("#lblCreatedAt");
        var descriptionObj = $("#divDescription");

        var requireVO = data.Result;

        titleObj.html(requireVO.Title);
        businessNameObj.html(requireVO.CustomerName);
        commissionObj.html(requireVO.Commission);
        categoryNameObj.html(requireVO.CategoryName);
        createdAtObj.html(new Date(requireVO.CreatedAt).format("yyyy-MM-dd"));
        descriptionObj.html(requireVO.Description);

    }, function (data) {        
        //load_hide();
    });
}

function markObject(requireId, imgObj) {
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
        markVO.MarkObjectId = requireId;
        markVO.MarkType = 4;
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
                        deleteMark(data.Result, requireId, imgObj);
                    });
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

function deleteMark(markId, requireId, imgObj) {
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
                    markObject(requireId, imgObj);
                });
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function onTenderInvite(requireId) {
    var tenderInviteVO = new Object();
    tenderInviteVO.RequirementId = requireId;
    tenderInviteVO.CustomerId = _CustomerId;
    if (_CustomerId < 1 || _Token == "") {
        bootbox.dialog({
            message: "请先登录再进行投标！",
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
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateRequireTenderInvite?token=" + _Token,
            type: "POST",
            data: tenderInviteVO,
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