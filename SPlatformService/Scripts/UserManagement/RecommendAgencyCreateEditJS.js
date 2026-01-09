$(document).ready(function () {

    $("button[id*='btn_submit']").click(function () {

        var agencyModel = GetRecommendAgencyList();
        $.ajax({
            url: _RootPath + "SPWebAPI/user/UpdateRecommendAgency?token=" + _Token,
            type: "POST",
            data: agencyModel,
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
                    BindRecommendAgency();
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("select[id$='drpCity']").change(function () {
        BindRecommendAgency();
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
                    BindRecommendAgency();
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("select[id$='drpCategory']").change(function () {
        BindRecommendAgency();
    });

    BindRecommendAgency();
});

function GetRecommendAgencyList() {
    var agencyModel = new Object();
    var agencyList = new Array();
    agencyModel.RecommendAgencyList = agencyList;

    var puhidenObjList = $("#AgencyList").find("input[type='hidden']");
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];

        var agencyVO = new Object();
        agencyList.push(agencyVO);
        if (drpProvinceObj.val() > 0)
            agencyVO.ProvinceId = drpProvinceObj.val();
        if (drpCityObj.val() > 0)
            agencyVO.CityId = drpCityObj.val();
        if (drpParentCategoryObj.val() > 0)
            agencyVO.ParentCategoryId = drpParentCategoryObj.val();
        if (drpCategoryObj.val() > 0)
            agencyVO.CategoryId = drpCategoryObj.val();
        agencyVO.AgencyId = agencyId;
        agencyVO.Sort = puhidenObj.parent().next().next().next().next().html();
    }
    return agencyModel;
}

function BindRecommendAgency() {
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    $("#AgencyList").find("tbody").children().remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/user/GetRecommendAgencyList?provinceId=" + drpProvinceObj.val() + "&cityId=" + drpCityObj.val() + "&parentCategoryId=" + drpParentCategoryObj.val() + "&categoryId=" + drpCategoryObj.val(),
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddAgency(puVO);
                }
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function AddAgency(puVO) {
    var agencyTable = $("#AgencyList");
    var count = $("#AgencyList").find("input[type='hidden']").length + 1;
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"Agency_" + puVO.AgencyId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CustomerCode + "\">" + puVO.CustomerCode + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CustomerName + "\">" + puVO.CustomerName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.AgencyName + "\">" + puVO.AgencyName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + count + "\">" + count + "</td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\uptop.png\" title=\"置顶\" onclick=\"TopRow(this)\" /> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\up.png\" title=\"上移\" onclick=\"upRow(this)\" /> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\down.png\" title=\"下移\" onclick=\"downRow(this)\" /> \r\n";
    oTR += "    <img src=\"..\\Style\\images\\downbottom.png\" title=\"最后\" onclick=\"bottomRow(this)\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    agencyTable.append(oTR);
}

function NewAgency() {
    var drpProvinceObj = $("select[id*='drpProvince']");
    var drpCityObj = $("select[id*='drpCity']");
    var drpParentCategoryObj = $("select[id*='drpParentCategory']");
    var drpCategoryObj = $("select[id*='drpCategory']");
    onChooseAgency(drpProvinceObj.val(), drpCityObj.val(), drpParentCategoryObj.val(), drpCategoryObj.val());
}

function DeleteAgency() {
    var chkList = $("#AgencyList").find("tbody").find("input[type='checkbox']:checked");

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
    var count =  parseInt($(imgObj).parent().prev().html());
    if (count == ($("#AgencyList").find("input[type='hidden']").length + 1))
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
    var count =  parseInt($(imgObj).parent().prev().html());
    if (count == ($("#AgencyList").find("input[type='hidden']").length + 1))
        return;
    var currentRow = $(imgObj).parent().parent();
    var nextRow = currentRow.next();

    //修改count
    $(currentRow.children()[4]).html($("#AgencyList").find("input[type='hidden']").length)
    $(nextRow.children()[4]).html(count);
    for (var i =count + 1; i < $("#AgencyList").find("input[type='hidden']").length + 1; i++) {
        var nextRow1 = nextRow.next();
        currentRow.insertAfter(nextRow);
        nextRow = nextRow1;
        $(nextRow.children()[4]).html(i);
    }
    currentRow.insertAfter(nextRow);
    //调换位置

}

function TopRow(imgObj) {
    var count =  parseInt($(imgObj).parent().prev().html());
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
    var count =  parseInt($(imgObj).parent().prev().html());
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

function onChooseAgency(provinceId, cityId, parentCategoryId, categoryId) {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/ChooseAgencyList.aspx?provinceId=' + provinceId + '&cityId=' + cityId + '&parentCategoryId=' + parentCategoryId + '&categoryId=' + categoryId + '" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择销售",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleAgencyObj = $(window.frames["iframe_1"].document).find("#ChooseAgencyList").find("input[type='checkbox']:checked");
                    var selectAgencyIdStr = "";
                    var agencyArray = new Array();
                    for (var i = 0; i < seleAgencyObj.length; i++) {
                        var agency = new Object();
                        agencyArray.push(agency);
                        var chk = $(seleAgencyObj[i]);
                        agency.AgencyId = chk.parent().next()[0].innerText;
                        agency.CustomerCode = chk.parent().next().next()[0].innerText;
                        agency.CustomerName = chk.parent().next().next().next()[0].innerText;
                        agency.AgencyName = chk.parent().next().next().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidAgencyId").val(JSON.stringify(agencyArray));

                    var agencyArray = $(window.frames["iframe_1"].document).find("#hidAgencyId").val();
                    if (agencyArray != "") {
                        var agencyList = JSON.parse(agencyArray);
                        var puhidenObjList = $("#AgencyList").find("input[type='hidden']");
                        for (var i = 0; i < agencyList.length; i++) {
                            var agencyObj = agencyList[i];
                            var puVO = new Object();
                            puVO.AgencyId = agencyObj.AgencyId;
                            puVO.CustomerCode = agencyObj.CustomerCode;
                            puVO.CustomerName = agencyObj.CustomerName;
                            puVO.AgencyName = agencyObj.AgencyName;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var agencyId = puValue.split("_")[1];

                                if (agencyId == puVO.AgencyId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddAgency(puVO);
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