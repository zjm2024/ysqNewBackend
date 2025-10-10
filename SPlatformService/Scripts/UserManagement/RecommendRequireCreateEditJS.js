$(document).ready(function () {

    $("button[id*='btn_submit']").click(function () {
        
        var requireModel = GetRecommendRequireList();
        $.ajax({
            url: _RootPath + "SPWebAPI/user/UpdateRecommendRequire?token=" + _Token,
            type: "POST",
            data: requireModel,
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
            url: _RootPath + "SPWebAPI/user/GetCityList?provinceId=" + drp.val() + "&enable=true",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    childDrp.append("<option value=\"-1\">全部</option>");
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
                    }
                    BindRecommendRequire();
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("select[id$='drpCity']").change(function () {
        BindRecommendRequire();
    });

    $("select[id$='drpParentCategory']").change(function () {
        //更新Child
        var drp = $("select[id*='drpParentCategory']");
        var childDrp = $("select[id*='drpCategory']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/user/GetChildCategoryList?parentCategoryId=" + drp.val() + "&enable=true",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    childDrp.append("<option value=\"-1\">全部</option>");
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CategoryId + "\">" + childList[i].CategoryName + "</option>");
                    }
                    BindRecommendRequire();
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("select[id$='drpCategory']").change(function () {
        BindRecommendRequire();
    });

    BindRecommendRequire();
});

function GetRecommendRequireList() {
    var requireModel = new Object();    
    var requireList = new Array();
    requireModel.RecommendRequireList = requireList;

    var puhidenObjList = $("#RequireList").find("input[type='hidden']");
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var requireId = puValue.split("_")[1];

        var requireVO = new Object();
        requireList.push(requireVO);
        if (drpProvinceObj.val() > 0)
            requireVO.ProvinceId = drpProvinceObj.val();
        if (drpCityObj.val() > 0)
            requireVO.CityId = drpCityObj.val();
        if (drpParentCategoryObj.val() > 0)
            requireVO.ParentCategoryId = drpParentCategoryObj.val();
        if (drpCategoryObj.val() > 0)
            requireVO.CategoryId = drpCategoryObj.val();
        requireVO.RequirementId = requireId;
        requireVO.Sort = puhidenObj.parent().next().next().next().next().html();
    }
    return requireModel;
}

function BindRecommendRequire() {
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    $("#RequireList").find("tbody").children().remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/user/GetRecommendRequireList?provinceId=" + drpProvinceObj.val() + "&cityId=" + drpCityObj.val() + "&parentCategoryId=" + drpParentCategoryObj.val() + "&categoryId=" + drpCategoryObj.val(),
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddRequire(puVO);
                }
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function AddRequire(puVO) {
    var requireTable = $("#RequireList");
    var count = $("#RequireList").find("input[type='hidden']").length + 1;
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"Require_" + puVO.RequirementId + "\" /> \r\n";
    oTR += "  </td> \r\n";   
    oTR += "  <td class=\"center\" title=\"" + puVO.RequirementCode + "\">" + puVO.RequirementCode + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Title + "\">" + puVO.Title + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Commission + "\">" + puVO.Commission + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + count + "\">" + count + "</td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    
    oTR += "    <img src=\"..\\Style\\images\\uptop.png\" title=\"置顶\" onclick=\"TopRow(this)\" /> \r\n";

    oTR += "    <img src=\"..\\Style\\images\\up.png\" title=\"上移\" onclick=\"upRow(this)\" /> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\down.png\" title=\"下移\" onclick=\"downRow(this)\" /> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\downbottom.png\" title=\"最后\" onclick=\"bottomRow(this)\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    requireTable.append(oTR);
}

function NewRequire() {
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    onChooseRequire(drpProvinceObj.val(), drpCityObj.val(), drpParentCategoryObj.val(), drpCategoryObj.val());
}

function DeleteRequire() {
    var chkList = $("#RequireList").find("tbody").find("input[type='checkbox']:checked");

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

function downRow(imgObj) {
    var count = $(imgObj).parent().prev().html();
    if (count == ($("#RequireList").find("input[type='hidden']").length + 1))
        return;
    var currentRow = $(imgObj).parent().parent();
    var nextRow = currentRow.next();

    //修改count
    $(currentRow.children()[4]).html($(nextRow.children()[4]).html())
    $(nextRow.children()[4]).html(count);
    //调换位置
    currentRow.insertAfter(nextRow);
}
function bottomRow(imgObj) {
    var count = parseInt($(imgObj).parent().prev().html());
    if (count == ($("#RequireList").find("input[type='hidden']").length + 1))
        return;
    var currentRow = $(imgObj).parent().parent();
    var nextRow = currentRow.next();

    //修改count
    $(currentRow.children()[4]).html($("#RequireList").find("input[type='hidden']").length)
    $(nextRow.children()[4]).html(count);
    for (var i = count + 1; i < $("#RequireList").find("input[type='hidden']").length + 1; i++) {
        var nextRow1 = nextRow.next();
        currentRow.insertAfter(nextRow);
        nextRow = nextRow1;
        $(nextRow.children()[4]).html(i);
    }
    currentRow.insertAfter(nextRow);
    //调换位置

}

function TopRow(imgObj) {
    var count = parseInt($(imgObj).parent().prev().html());
    if (count == 1)
        return;
    var currentRow = $(imgObj).parent().parent();
    var prevRow = currentRow.prev();

    //修改count

    $(prevRow.children()[4]).html($(currentRow.children()[4]).html());
    $(currentRow.children()[4]).html(1)

    for (var i = count - 1; i > 0; i--) {
        var prevRow1 = prevRow.prev();
        prevRow.insertAfter(currentRow);
        prevRow = prevRow1;
        $(prevRow.children()[4]).html(i);
    }
    prevRow.insertAfter(currentRow);
    //调换位置

}
function upRow(imgObj) {
    var count = $(imgObj).parent().prev().html();
    if (count == 1)
        return;
    var currentRow = $(imgObj).parent().parent();
    var prevRow = currentRow.prev();

    //修改count
    $(currentRow.children()[4]).html($(prevRow.children()[4]).html())
    $(prevRow.children()[4]).html(count);
    //调换位置
    prevRow.insertAfter(currentRow);
}

function onChooseRequire(provinceId, cityId, parentCategoryId, categoryId) {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/RequireManagement\/ChooseRequireList.aspx?provinceId=' + provinceId + '&cityId=' + cityId + '&parentCategoryId=' + parentCategoryId + '&categoryId=' + categoryId + '" height="350px" width="100%" frameborder="0"><\/iframe>',

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
                        require.Commission = chk.parent().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidRequireId").val(JSON.stringify(requireArray));

                    var requireArray = $(window.frames["iframe_1"].document).find("#hidRequireId").val();
                    if (requireArray != "") {
                        var requireList = JSON.parse(requireArray);
                        var puhidenObjList = $("#RequireList").find("input[type='hidden']");
                        for (var i = 0; i < requireList.length; i++) {
                            var requireObj = requireList[i];
                            var puVO = new Object();
                            puVO.RequirementId = requireObj.RequirementId;
                            puVO.RequirementCode = requireObj.RequirementCode;
                            puVO.Title = requireObj.Title;
                            puVO.Commission = requireObj.Commission;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var requireId = puValue.split("_")[1];

                                if (requireId == puVO.RequirementId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddRequire(puVO);
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