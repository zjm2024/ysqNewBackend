$(function () {
    if (_CustomerId <= 0) {
        bootbox.dialog({
            message: "请先登录后再查看更多内容！",
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

    var drpProvince = $("select[id*='drpProvince']");
    if (drpProvince)
        drpProvince.change(function () {
            var drp = $("select[id*='drpProvince']");

            bindCity(drp.val(), function (data) {
                var objCity = $("select[id*='drpCity']");
                if (objCity) {
                    objCity.empty();
                    objCity.append("<option value=\"-1\">全部</option>");
                    var childList = data.Result;
                    for (var k = 0; k < childList.length; k++) {
                        objCity.append("<option value=\"" + childList[k].CityId + "\">" + childList[k].CityName + "</option>");
                    }
                    bindData();
                }
            }, function (data) { });
        });

    var drpCity = $("select[id*='drpCity']");
    if (drpCity)
        drpCity.change(function () {
            bindData();
        });

    var drpParentCategory = $("select[id*='drpParentCategory']");
    if (drpParentCategory)
        drpParentCategory.change(function () {
            var drp = $("select[id*='drpParentCategory']");

            bindCategory(drp.val(), function (data) {
                var objCategory = $("select[id*='drpCategory']");
                if (objCategory) {
                    objCategory.empty();
                    objCategory.append("<option value=\"-1\">全部</option>");
                    var childList = data.Result;
                    for (var k = 0; k < childList.length; k++) {
                        objCategory.append("<option value=\"" + childList[k].CategoryId + "\">" + childList[k].CategoryName + "</option>");
                    }

                    bindData();
                }
            }, function (data) { });
        });

    var drpCategory = $("select[id*='drpCategory']");
    if (drpCategory)
        drpCategory.change(function () {
            bindData();
        });

    $("#btnSearch").click(function () {
        bindData();

        return false;
    });


    //bindData();
});

function onFilterClick(obj) {
    //load_show();
    //设置样式
    $(obj).closest("ul").find("li").removeClass("color");
    $(obj).closest("li").addClass("color");

    bindData();

    return false;
}

function onSortClick(obj) {
    //load_show();
    //设置样式
    $(obj).closest("ul").find("li").removeClass("sort");
    $(obj).closest("li").addClass("sort");

    var liObj = $(obj).closest("li");
    var sortType = "asc";
    if (liObj.attr("sorttype") == "asc") {
        sortType = "desc";
    } else {
        sortType = "asc";
    }

    $(obj).closest("ul").find("li").attr("sorttype", "asc");

    liObj.attr("sorttype", sortType);

    bindData();

    return false;
}

function bindData() {
    //获取条件和排序，生成filterModel
    var filterModel = getFilterModel();

    //获取数据    

    GetProjectList(filterModel);

    SetPaging("GetProjectList", filterModel);
}

function getFilterModel() {
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = 1;
    pageInfoObj.PageCount = 20;
    pageInfoObj.SortName = "CreatedAt";
    pageInfoObj.SortType = "asc";
    
    var sortTypeobj = $("#divSort").find(".sort");
    if (sortTypeobj) {
        var sortName = sortTypeobj.attr("sortname");
        var sortType = sortTypeobj.attr("sorttype");
        if (sortName == "1") {
            pageInfoObj.SortName = "CreatedAt";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "2") {
            //pageInfoObj.SortName = "成交量";
            //pageInfoObj.SortType = sortType;
        } else if (sortName == "3") {
            //pageInfoObj.SortName = "好评率";
            //pageInfoObj.SortType = sortType;
        }
        else if (sortName == "5") {
            pageInfoObj.SortName = "Commission";
            pageInfoObj.SortType = sortType;
        }
    }


    filterObj.groupOp = "AND";
    var rulesObj = new Array();
    filterObj.rules = rulesObj;
    var filterArray = new Array();
    filterObj.filter = filterArray;

    var ruleObj = new Object();
    rulesObj.push(ruleObj);

    ruleObj.field = "1";
    ruleObj.op = "eq";
    ruleObj.data = "1";

    var cityObj = $("select[id*='drpCity']");
    var categoryObj = $("select[id*='drpCategory']");
    var provinceObj = $("select[id*='drpProvince']");
    var parentCategoryObj = $("select[id*='drpParentCategory']");

    if (provinceObj && (provinceObj.val() == "-1" || provinceObj.val() == "-2")) {
        ruleObj = new Object();
        rulesObj.push(ruleObj);

        ruleObj.field = "1";
        ruleObj.op = "eq";
        ruleObj.data = 1;
    } else if (cityObj) {
        if (cityObj.val() != "-1" && cityObj.val() != "-2") {
            ruleObj = new Object();
            rulesObj.push(ruleObj);

            ruleObj.field = "CityId";
            ruleObj.op = "eq";
            ruleObj.data = cityObj.val();
        } else {
            var optionList = cityObj.find("option");
            var cityArray = new Array();
            for (var i = 0; i < optionList.length; i++) {
                if (optionList[i].value != "-1" && optionList[i].value != "-2") {
                    cityArray.push(optionList[i].value);
                }
            }
            if (cityArray.length > 0) {
                ruleObj = new Object();
                rulesObj.push(ruleObj);

                ruleObj.field = "CityId";
                ruleObj.op = "in";
                ruleObj.data = cityArray.join(",");
            } else {
                //没有城市，也就没有数据
                ruleObj = new Object();
                rulesObj.push(ruleObj);

                ruleObj.field = "1";
                ruleObj.op = "ne";
                ruleObj.data = 1;
            }
        }
    }
    if (parentCategoryObj && (parentCategoryObj.val() == "-1" || parentCategoryObj.val() == "-2")) {
        ruleObj = new Object();
        rulesObj.push(ruleObj);

        ruleObj.field = "1";
        ruleObj.op = "eq";
        ruleObj.data = 1;
    } else if (categoryObj) {
        if (categoryObj.val() != "-1" && categoryObj.val() != "-2") {
            ruleObj = new Object();
            rulesObj.push(ruleObj);

            ruleObj.field = "CategoryId";
            ruleObj.op = "eq";
            ruleObj.data = categoryObj.val();
        } else {
            var optionList = categoryObj.find("option");
            var categoryArray = new Array();
            for (var i = 0; i < optionList.length; i++) {
                if (optionList[i].value != "-1" && optionList[i].value != "-2") {
                    categoryArray.push(optionList[i].value);
                }
            }
            if (categoryArray.length > 0) {
                ruleObj = new Object();
                rulesObj.push(ruleObj);

                ruleObj.field = "CategoryId";
                ruleObj.op = "in";
                ruleObj.data = categoryArray.join(",");
            } else {
                //没有行业小类，也就没有数据
                ruleObj = new Object();
                rulesObj.push(ruleObj);

                ruleObj.field = "1";
                ruleObj.op = "ne";
                ruleObj.data = 1;
            }
        }
    }

    var inputObj = $("input[id*='txtSearcha']");
    if (inputObj && inputObj.val() != "") {
        ruleObj = new Object();
        rulesObj.push(ruleObj);

        ruleObj.field = "Title";
        ruleObj.op = "cn";
        ruleObj.data = inputObj.val();
    }
    if ($("#pricestart").val() != "" || $("#priceend").val() != "") {
        if ($("#pricestart").val() != "") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "ge";
            ruleCommission.data = $("#pricestart").val();
        }
        if ($("#priceend").val() != "") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "le";
            ruleCommission.data = $("#priceend").val();
        }

    }

    return filterModel;
}

function bindCategoryList(parentCategoryId, parentCategoryName, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + parentCategoryId + "&enable=true",
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(parentCategoryName, data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function setFileter() {
    bindParentCategory(function (data) {
        var parentCategoryVOList = data.Result;
        var filterObj = $(".div-sxlist");
        
        for (var i = 0; i < parentCategoryVOList.length; i++) {
            bindCategoryList(parentCategoryVOList[i].CategoryId, parentCategoryVOList[i].CategoryName, function (parentCategoryName, data) {
                var categoryVOList = data.Result;
                var filterStr = "";
                filterStr = "<ul> \r\n";
                filterStr += "  <li style=\"width: 30px;\"><span>" + parentCategoryName + "</span></li> \r\n";
                //filterStr += "  <li class=\"color\" filtertype=\"all\"><a href=\"#\" onclick=\"return onFilterClick(this);\">全部</a></li> \r\n";
                for (var j = 0; j < categoryVOList.length; j++) {
                    filterStr += "  <li filtertype=\"" + categoryVOList[j].CategoryId + "\"><a href=\"#\" onclick=\"return onFilterClick(this);\">" + categoryVOList[j].CategoryName + "</a></li> \r\n"
                }
                filterStr += "  </ul> \r\n";

                filterObj.append(filterStr);
            }, function (data) {

            });
        }
    }, function (data) {
    });
}

function GetProjectList(filterModel) {

    GetDataList("GetProjectList", filterModel, function (data) {

        var projectList = data.Result;

        var divData = $("div[id*='divList']");
        var str = "\r\n";
        if (data.Flag = 0 || projectList.length == 0) {
            divData.html("<span style=\"margin-left: 50px;\">暂未找到相关数据</span>");
            return;
        }
        for (var i = 0; i < projectList.length; i++) {
            var project = projectList[i];
            str += "<div class=\"sign-data project-list-height\"> \r\n";
            str += "    <div class=\"fl\" style=\"width: 100%;\"> \r\n";
            str += "        <div style=\"float:left;width: 100%;\">";
            str += "        <div class=\"price\" style=\"display: none\">￥" + project.Commission.toFixed(2) + "元</div> \r\n";
            str += "        <div class=\"title\" style=\"width: 345px;margin-left: 0px;\"> \r\n";
            str += "            <a target=\"_blank\" title=\"" + project.Title + "\" href=\"Project.aspx?projectId=" + project.ProjectId + "\">" + project.Title + "</a> \r\n";
            str += "        </div> \r\n";
            str += "        <div class=\"title\"> \r\n";
            //如果已经登录,判断是否已经关注了
            if (_CustomerId < 1 || _Token == "") {
                str += "            <img width=\"26\" height=\"26\" title=\"关注\" src=\"Style/images/marked_non.png\" style=\"cursor:pointer;\" onclick=\"markObject('" + project.ProjectId + "', this)\" /> \r\n";
            } else {
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/IsMarked?markObjectId=" + project.ProjectId + "&markType=1&token=" + _Token,
                    type: "POST",
                    data: null,
                    async: false,
                    success: function (data) {
                        if (data.Flag == 1) {
                            str += "            <img width=\"26\" height=\"26\" title=\"取消关注\" src=\"Style/images/marked.png\" style=\"cursor:pointer;\" onclick=\"deleteMark('" + data.Result + "','" + project.ProjectId + "', this)\" /> \r\n";
                        } else {
                            str += "            <img width=\"26\" height=\"26\" title=\"关注\" src=\"Style/images/marked_non.png\" style=\"cursor:pointer;\" onclick=\"markObject('" + project.ProjectId + "', this)\" /> \r\n";
                        }
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            }
            str += "        </div> \r\n";
            str += "        </div> \r\n";
            str += "      <a target=\"_blank\" title=\"" + project.Title + "\" href=\"Project.aspx?projectId=" + project.ProjectId + "\" style='display:block;'> \r\n";
            str += "         <div class=\"content\"><strong>客户名称</strong>： " + project.BusinessName + "</div> \r\n";
            str += "         <div class=\"content\"><strong>销售</strong>：" + project.AgencyName + "</div> \r\n";
            str += "         <div class=\"content\"><strong>开始时间</strong>：" + new Date(project.StartDate).format("yyyy-MM-dd") + "</div> \r\n";

            str += "         <div class=\"content\"><strong>完成时间</strong>：" + new Date(project.EndDate).format("yyyy-MM-dd") + "</div> \r\n";
            str += "         <div class=\"content\"><strong>成交时间</strong>：" + new Date(project.CreatedAt).format("yyyy-MM-dd") + "</div> \r\n";
            //str += "         <div class=\"content\"><strong>合同额</strong>：" + project.Cost.toFixed(2) + "</div> \r\n";
            str += "         <div class=\"content\" ><strong>任务编号</strong>：" + project.RequirementCode + "</div> \r\n";
            str += "    </a> \r\n";
            str += "    </div> \r\n";
            //str += "    <div class=\"fr\"> \r\n";
            //str += "        <div class=\"status\"> \r\n";
            //str += "            <p>已选定销售</p> \r\n";
            //str += "            <p>交易成功</p> \r\n";
            //str += "        </div> \r\n";
            //str += "    </div> \r\n";
            str += "</div> \r\n";
        }

        divData.html(str);

    }, function (data) {
        var divData = $("div[id*='divList']");
        divData.html("<span style=\"margin-left: 50px;\">暂未找到相关数据</span>");
        //load_hide();
    });

}

function onPageUp() {
    var filterModel = getFilterModel();
    //获取当前页码
    var divPage = $("div[id*='pageList']");
    var selLI = divPage.find(".selected");
    var pageIndex = 1;
    if (selLI) {
        pageIndex = parseInt(selLI.find("a").html());
    }

    if (pageIndex > 1) {
        pageIndex = pageIndex - 1;
    } else {
        return false;
    }

    filterModel.PageInfo.PageIndex = pageIndex;

    GetProjectList(filterModel);
    divPage.find("li").removeClass("selected");
    $(divPage).find("li:contains('" + pageIndex + "')").addClass("selected");
    $("html,body").animate({ scrollTop: target_top }, 1000);
    return false;
}

function onPageDown() {
    var filterModel = getFilterModel();
    //获取当前页码
    var divPage = $("div[id*='pageList']");
    var selLI = divPage.find(".selected");
    var pageIndex = 1;
    if (selLI) {
        pageIndex = parseInt(selLI.find("a").html());
    }

    var pageTotalCount = parseInt($("input[id*='hidDataCount']").val());

    if (pageIndex < pageTotalCount)
        pageIndex = pageIndex + 1;
    else
        return false;

    filterModel.PageInfo.PageIndex = pageIndex;

    GetProjectList(filterModel);
    divPage.find("li").removeClass("selected");
    $(divPage).find("li:contains('" + pageIndex + "')").addClass("selected");
    $("html,body").animate({ scrollTop: target_top }, 1000);
    return false;
}

function onPageGoTo(pageObj) {
    var divPage = $("div[id*='pageList']");
    divPage.find("li").removeClass("selected");
    $(pageObj).closest("li").addClass("selected")

    var filterModel = getFilterModel();
    //获取当前页码

    var selLI = divPage.find(".selected");
    var pageIndex = 1;
    if (selLI) {
        pageIndex = parseInt(selLI.find("a").html());
    }

    filterModel.PageInfo.PageIndex = pageIndex;

    GetProjectList(filterModel);
    $("html,body").animate({ scrollTop: target_top }, 1000);
    return false;
}

function initCity(provinceId, cityId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (pdata) {
            if (pdata.Flag == 1) {
                var provinceVOList = pdata.Result;
                var objProvince = $("select[id*='drpProvince']");
                if (objProvince) {
                    objProvince.find("option").remove();
                    for (var j = 0; j < provinceVOList.length; j++) {
                        objProvince.append("<option value='" + provinceVOList[j].ProvinceId + "'>" + provinceVOList[j].ProvinceName + "</option>");
                    }
                    if (provinceId > 0)
                        objProvince.val(provinceId);

                    $.ajax({
                        url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + objProvince.val() + "&enable=true",
                        type: "GET",
                        data: null,
                        async: false,
                        success: function (data) {
                            if (data.Flag == 1) {
                                var objCity = $("select[id*='drpCity']");
                                if (objCity) {
                                    objCity.empty();
                                    var childList = data.Result;
                                    objCity.append("<option value=\"-1\">全部</option>");
                                    for (var k = 0; k < childList.length; k++) {
                                        objCity.append("<option value=\"" + childList[k].CityId + "\">" + childList[k].CityName + "</option>");
                                    }
                                    if (cityId > 0)
                                        objCity.val(cityId);
                                }
                            }
                        },
                        error: function (data) {
                            if (fail)
                                fail(data);
                        }
                    });

                }
            }
        },
        error: function (data) {

        }
    });

}

function initCategory(parentCategoryId, categoryId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetParentCategoryList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (pdata) {
            if (pdata.Flag == 1) {
                var parentCategoryVOList = pdata.Result;
                var objParentCategory = $("select[id*='drpParentCategory']");
                if (objParentCategory) {
                    objParentCategory.find("option").remove();

                    for (var j = 0; j < parentCategoryVOList.length; j++) {
                        objParentCategory.append("<option value='" + parentCategoryVOList[j].CategoryId + "'>" + parentCategoryVOList[j].CategoryName + "</option>");
                    }
                    if (parentCategoryId > 0)
                        objParentCategory.val(parentCategoryId);

                    $.ajax({
                        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + objParentCategory.val() + "&enable=true",
                        type: "GET",
                        data: null,
                        async: false,
                        success: function (data) {
                            if (data.Flag == 1) {
                                var objCategory = $("select[id*='drpCategory']");
                                if (objCategory) {
                                    objCategory.empty();
                                    objCategory.append("<option value=\"-1\">全部</option>");
                                    var childList = data.Result;
                                    for (var k = 0; k < childList.length; k++) {
                                        objCategory.append("<option value=\"" + childList[k].CategoryId + "\">" + childList[k].CategoryName + "</option>");
                                    }
                                    if (categoryId > 0)
                                        objCategory.val(categoryId);
                                }
                            }
                        },
                        error: function (data) {
                        }
                    });

                }
            }
        },
        error: function (data) {

        }
    });
}


function markObject(projectId, imgObj) {
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
        markVO.MarkObjectId = projectId;
        markVO.MarkType = 3;
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateMark?token=" + _Token,
            type: "POST",
            data: markVO,
            success: function (data) {
                if (data.Flag == 1) {
                    $(imgObj).attr("src", "Style/images/marked.png");
                    $(imgObj).attr("title", "取消关注");
                    $(imgObj).removeAttr("onclick");
                    $(imgObj).unbind("click");
                    $(imgObj).click(function () {
                        deleteMark(data.Result, projectId, imgObj);
                    });
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

function deleteMark(markId, projectId, imgObj) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteMark?markId=" + markId + "&token=" + _Token,
        type: "POST",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                $(imgObj).attr("src", "Style/images/marked_non.png");
                $(imgObj).attr("title", "关注");
                $(imgObj).removeAttr("onclick");
                $(imgObj).unbind("click");
                $(imgObj).click(function () {
                    markObject(projectId, imgObj);
                });
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}