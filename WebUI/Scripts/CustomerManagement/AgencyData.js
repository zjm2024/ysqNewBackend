$(function () {   
    /*if (_CustomerId <= 0) {
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
    }*/
    bindData();
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
                    objCity.append("<option value=\"-2\">不限</option>");
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
                    objCategory.append("<option value=\"-2\">不限</option>");
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
    GetAgencyList(filterModel);

    NowpageIndex = 1;//重置页码
    isFromSearch = true;
    isFromAll = false;

    SetPaging("GetAgencyList", filterModel);
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
    pageInfoObj.SortType = "desc";

    var sortTypeobj = $("#divSort").find(".sort");
    if (sortTypeobj) {
        var sortName = sortTypeobj.attr("sortname");
        var sortType = sortTypeobj.attr("sorttype");
        if (sortName == "1") {
            pageInfoObj.SortName = "CreatedAt";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "2") {
            pageInfoObj.SortName = "ReviewScore";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "3") {
            pageInfoObj.SortName = "ProjectCount";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "4") {
            pageInfoObj.SortName = "ProjectCost";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "5") {
            pageInfoObj.SortName = "ProjectCommission";
            pageInfoObj.SortType = sortType;
        }
        //else if (sortName == "5") {
        //    pageInfoObj.SortName = "Price";
        //    pageInfoObj.SortType = sortType;
        //}
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

            ruleObj.field = "CategoryIds";
            ruleObj.op = "cn";
            ruleObj.data = "," + categoryObj.val() + ",";
        } else {
            var optionList = categoryObj.find("option");
            
            if (optionList.length > 1) {
                var filerCategory = new Object();
                filterArray.push(filerCategory);
                filerCategory.groupOp = "OR";
                var rulesCategoryObj = new Array();
                filerCategory.rules = rulesCategoryObj;
                var filterCategoryArray = new Array();
                filerCategory.filter = filterCategoryArray;
                
                for (var i = 0; i < optionList.length; i++) {
                    if (optionList[i].value != "-1" && optionList[i].value != "-2") {
                        ruleObj = new Object();
                        rulesCategoryObj.push(ruleObj);

                        ruleObj.field = "CategoryIds";
                        ruleObj.op = "cn";
                        ruleObj.data = "," + optionList[i].value + ",";

                    }
                }
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
        //ruleObj = new Object();
        //rulesObj.push(ruleObj);

        //ruleObj.field = "CustomerName";
        //ruleObj.op = "cn";
        //ruleObj.data = inputObj.val();



        var filterSearchObj = new Object();

        filterArray.push(filterSearchObj);

        filterSearchObj.groupOp = "OR";
        var rulesSerachObj = new Array();
        filterSearchObj.rules = rulesSerachObj;
        var filterSearchArray = new Array();
        filterSearchObj.filter = filterSearchArray;

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "AgencyName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CityName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Company";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ContactsResources";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Description";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "FamiliarClient";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "FamiliarProduct";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Feature";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Phone";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Position";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ProjectExperience";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "School";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();
        
        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ShortDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Specialty";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetCategory";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetClient";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetSolution";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Technical";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();
    }
   

    //if ($("#pricestart").val() != "" || $("#priceend").val() != "") {
    //    if ($("#pricestart").val() != "") {
    //        var ruleCommission = new Object();
    //        rulesObj.push(ruleCommission);
    //        ruleCommission.field = "Price";
    //        ruleCommission.op = "ge";
    //        ruleCommission.data = $("#pricestart").val();
    //    }
    //    if ($("#priceend").val() != "") {
    //        var ruleCommission = new Object();
    //        rulesObj.push(ruleCommission);
    //        ruleCommission.field = "Price";
    //        ruleCommission.op = "le";
    //        ruleCommission.data = $("#priceend").val();
    //    }

    //} 
    //var categoryType = $(".div-sxlist").find(".color");
    //if (categoryType) {
    //    var categoryId = categoryType.attr("filtertype");
    //    var ruleTender = new Object();
    //    rulesObj.push(ruleTender);
    //    ruleTender.field = "CategoryIds";
    //    ruleTender.op = "bw";
    //    ruleTender.data = "," + categoryId + ",";
    //}    

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
var isFromSearch = true; //判断是不是第一次加载
var isFromAll = false;//是否已加载完全部
var isloading = false;//是否正在加载
function GetAgencyList(filterModel) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetAgencyList",
        type: "POST",
        data: filterModel,
        success: function (data) {
            console.log(data);
            $('#lloading').hide();
            var agencyList = data.Result;
            var divData = $("div[id*='divList']");
            var str = "\r\n";
            if (data.Flag = 0 || agencyList.length == 0) {
                if (NowpageIndex == 1)
                {
                    divData.html("<span class=\"lloading\">暂未找到相关数据</span>");
                    return;
                }
            }
            for (var i = 0; i < agencyList.length; i++) {
                var agency = agencyList[i];
                var levelclass = "lm3";
                var levelName = "铜牌销售";
                if (agency.AgencyLevel == 1) {
                    levelclass = "lm1";
                    levelName = "金牌销售";
                } else if (agency.AgencyLevel == 2) {
                    levelclass = "lm2";
                    levelName = "银牌销售";
                } else if (agency.AgencyLevel == 3) {
                    levelclass = "lm3";
                    levelName = "铜牌销售";
                } else if (agency.AgencyLevel == 4) {
                    levelclass = "lm3";
                    levelName = "普通销售";
                }
                str += "<li>\r\n";
                str += "    	<div class=\"medal " + levelclass + "\"></div>\r\n";

                //如果已经登录,判断是否已经关注了
                if (_CustomerId < 1 || _Token == "") {
                    str += "        <div class=\"Collection\"  onclick=\"markObject('" + agency.CustomerId + "', this)\"></div>\r\n";
                } else {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Customer/IsMarked?markObjectId=" + agency.CustomerId + "&markType=1&token=" + _Token,
                        type: "POST",
                        data: null,
                        async: false,
                        success: function (data) {
                            if (data.Flag == 1) {
                                str += "        <div class=\"Collection on\" title=\"取消关注\" onclick=\"deleteMark('" + data.Result + "','" + agency.CustomerId + "', this)\"></div>\r\n";
                            } else {
                                str += "        <div class=\"Collection\" title=\"关注\" onclick=\"markObject('" + agency.CustomerId + "', this)\"></div>\r\n";
                            }
                        },
                        error: function (data) {
                            //alert(data); 
                        }
                    });
                }

                var headimg = _APIURL + "/Style/images/wxapp/noimg.png";
                if (agency.PersonalCard != "") {
                    headimg = agency.PersonalCard;
                } else if (agency.HeaderLogo != "") {
                    headimg = agency.HeaderLogo;
                }
                var AgencyName = agency.AgencyName.replace(/^\s\s*/, "");
                if (AgencyName.length >= 2)
                {
                    AgencyName = AgencyName.substring(0, 2) + "***";
                }

                str += "        <a target=\"_blank\" title=\"" + AgencyName + "\" href=\"Agency.aspx?agencyId=" + agency.AgencyId + "\"><div class=\"headimg\" style=\"background-image:url(" + headimg + ")\"></div></a>\r\n";
                str += "        <div class=\"info\">\r\n";
                str += "        	<div class=\"name_div\">\r\n";
                str += "            	<span class=\"name\"><a target=\"_blank\" title=\"" + AgencyName + "\" href=\"Agency.aspx?agencyId=" + agency.AgencyId + "\">" + AgencyName + " " + (agency.Sex == 1 ? "先生" : "女士") + "</a></span>\r\n";
                if (agency.Birthday != "") {
                    str += "                <span class=\"age\">年龄：" + (new Date(agency.Birthday).format("yyyy-MM-dd") == "1900-01-01" ? 0 : new Date().getFullYear() - new Date(agency.Birthday).getFullYear()) + "</span>\r\n";
                }
                str += "                <span class=\"level l" + parseInt(agency.ReviewScore) + "\">\r\n";
                str += "                	<div class=\"level0\"><em></em><em></em><em></em><em></em><em></em></div>\r\n";
                str += "                    <div class=\"level1\"><em></em><em></em><em></em><em></em><em></em></div>\r\n";
                str += "                </span>\r\n";
                str += "            </div>\r\n";
                str += "            <ul class=\"info_list\">\r\n";
                str += "            	<div>\r\n";
                str += "                    <li class=\"on\">销售级别：" + levelName + "</li>\r\n";
                str += "                    <li>居住城市：" + agency.CityName + "</li>\r\n";
                str += "                </div>\r\n";
                if (agency.TargetClient != "")
                    str += "                <li title=\"" + agency.TargetClient + "\">优势客户：" + RemoveLongString(RemoveComma(agency.TargetClient), 23) + "</li>\r\n";
                if (agency.TargetSolution != "")
                    str += "                <li title=\"" + agency.TargetSolution + "\">典型案列：" + RemoveLongString(RemoveComma(agency.TargetSolution), 23) + "</li>\r\n";
                if (agency.TargetCategory != "")
                    str += "                <li title=\"" + agency.TargetCategory + "\">擅长行业：" + RemoveLongString(RemoveComma(agency.TargetCategory), 23) + "</li>\r\n";
                if (agency.Specialty != "")
                    str += "                <li title=\"" + agency.Specialty + "\">兴趣特长：" + RemoveLongString(agency.Specialty, 100) + "</li>\r\n";
                if (agency.FamiliarProduct != "")
                    str += "                <li title=\"" + agency.FamiliarProduct + "\">擅长产品：" + RemoveLongString(agency.FamiliarProduct, 23) + "</li>\r\n";
                var des = $(agency.Description).text();
                if (des != "")
                    str += "                <li title=\"" + des + "\">简历：" + RemoveLongString(des, 100) + "</li>\r\n";
                str += "            </ul>\r\n";
                str += "            <div class=\"lbtn\">\r\n";
                str += "                <div class=\"bdsharebuttonbox lg_list_bdshare\"><a href=\"#\" data-cmd=\"more\"  onclick=\"btnSetShareUrl('" + _SITEURL + "/Agency.aspx?agencyId=" + agency.AgencyId + "')\">分享</a></div>\r\n";
				str += "                <div class=\"lg_list_wxshare\"><a href=\"#\" onclick=\"wx_share_show()\">分享</a></div>\r\n";
                str += "                <button type=\"button\" onclick=\"return onTender('" + agency.CustomerId + "')\">邀请接单</button>\r\n";
                str += "                <a href=\"ZXTIM.aspx?MessageTo=" + agency.CustomerId + "\" target=\"_blank\">线上联系</a>\r\n";
                str += "            </div>\r\n";
                str += "        </div>\r\n";
                str += "</li>\r\n";
            }
            if (isFromSearch) {
                divData.html(str);
            } else {
                if (data.Result.length != 0) {
                    divData.append(str);
                } else {
                    isFromAll = true;
                    if (NowpageIndex > 2)
                    {
                        $('#lloading').html("已全部加载完成");
                        $('#lloading').show();
                    } else {
                        $('#lloading').hide();
                    }
                    
                }
            }
            isloading = false;
        },
        error: function (data) {
            var divData = $("div[id*='divList']");
            divData.html("<span class=\"lloading\">暂未找到相关数据</span>");
            console.log(data);
        }
    });
}
var minAwayBtm = 0;  // 距离页面底部的最小距离
var NowpageIndex = 1;//当前页码
$(window).scroll(function () {
    var awayBtm = $(document).height() - $(window).scrollTop() - $(window).height();
    if (awayBtm <= minAwayBtm && !isloading) {
        if (!isFromAll)
        {
            isloading = true;
            isFromSearch = false;
            $('#lloading').show();
            var filterModel = getFilterModel();
            NowpageIndex = NowpageIndex + 1;
            console.log(NowpageIndex);
            filterModel.PageInfo.PageIndex = NowpageIndex;
            GetAgencyList(filterModel);
        }
    }
});
function RemoveComma(input) {
    if (input.length == 0)
        return input;


    var first = 0, last = input.length - 1;
    if (input.substring(0, 1) == ',')
        input = input.substring(1);
    if (input.length == 1 && input == ",") {
        input = "";
    } else if (input.length > 0 && input.substring(input.length - 1) == ',')
        input = input.substring(0, input.length - 1);

    return input;
}
function RemoveLongString(input,len) {
    return ((input.length > len) ? (input.substring(0, len) + "...") : input);
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

    GetAgencyList(filterModel);
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

    GetAgencyList(filterModel);
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

    GetAgencyList(filterModel);
    $("html,body").animate({ scrollTop: target_top }, 1000);
    return false;
}

function onTender(customerId) {
    if (_CustomerId > 0) {
        //通过认证的雇主才能邀请
        if (isBusiness) {
            //选择任务再进行邀请
            onChooseRequire(customerId);
        } else {
            bootbox.dialog({
                message: "通过雇主认证才能邀请！",
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
                                    objCity.append("<option value=\"-2\">不限</option>");
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
                                    objCategory.append("<option value=\"-2\">不限</option>");
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

var Shareurl = "";

function btnSetShareUrl(url) {
    Shareurl = url;
}
window._bd_share_config = {
    "common": {
        "onBeforeClick": SetShareUrl,
        "bdSnsKey": {},
        "bdText": "",
        "bdMini": "2",
        "bdMiniList": false,
        "bdPic": "",
        "bdStyle": "0",
        "bdSize": "24",
        "bdUrl": ""
    },
    "share": {},
    "image": {
        "viewList": ["weixin", "sqq", "tsina", "tqq", "renren"],
        "viewText": "分享到：",
        "viewSize": "16"
    },
    "selectShare": {
        "bdContainerClass": null,
        "bdSelectMiniList": ["weixin", "sqq", "tsina", "tqq", "renren"]
    }
};
function SetShareUrl(cmd, config) {
    if (Shareurl != "" && Shareurl) {
        config.bdUrl = Shareurl;
    }
    return config;
}
with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'static/api/js/share.js?v=89860593.js?cdnversion=' + ~(-new Date() / 36e5)];