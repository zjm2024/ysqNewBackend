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
    //setFileter();
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
    GetBusinessList(filterModel);

    NowpageIndex = 1;//重置页码
    isFromSearch = true;
    isFromAll = false;

    SetPaging("GetBusinessList", filterModel);
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
            pageInfoObj.SortName = "ProjectCount";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "3") {
            pageInfoObj.SortName = "ReviewScore";
            pageInfoObj.SortType = sortType;
        }
        else if (sortName == "4") {
            pageInfoObj.SortName = "JSXReviewScore";
            pageInfoObj.SortType = sortType;
        }
        else if (sortName == "5") {
            pageInfoObj.SortName = "CXDReviewScore";
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
       /* ruleObj = new Object();
        rulesObj.push(ruleObj);

        ruleObj.groupOp = "OR";

        ruleObj.field = "CompanyName";
        ruleObj.op = "cn";
        ruleObj.data = inputObj.val();*/


        var filterSearchObj = new Object();

        filterArray.push(filterSearchObj);

        filterSearchObj.groupOp = "OR";
        var rulesSerachObj = new Array();
        filterSearchObj.rules = rulesSerachObj;
        var filterSearchArray = new Array();
        filterSearchObj.filter = filterSearchArray;

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Address";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CategoryNames";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CityName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CompanyDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CompanyName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CompanySite";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CompanyType";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CustomerCode";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CustomerName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Description";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "MainProducts";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ProductDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Phone";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ProductDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetCategory";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetCity";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetClient";
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
function GetBusinessList(filterModel) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetBusinessList",
        type: "POST",
        data: filterModel,
        success: function (data) {
            console.log(data);
        $('#lloading').hide();
        var businessList = data.Result;
        var divData = $("div[id*='divList']");
        var str = "\r\n";
        if (data.Flag = 0 || businessList.length == 0) {
            if (NowpageIndex == 1) {
                divData.html("<span class=\"lloading\">暂未找到相关数据</span>");
                return;
            }
        }
        for (var i = 0; i < businessList.length; i++) {
            var business = businessList[i];

            var headimg = _APIURL + "/Style/images/wxapp/noimg.png";
            if (business.CompanyLogo != "") {
                headimg = business.CompanyLogo;
            }
            str += "<li>\r\n";
            //如果已经登录,判断是否已经关注了
            if (_CustomerId < 1 || _Token == "") {
                str += "        <div class=\"Collection\"  onclick=\"markObject('" + business.CustomerId + "', this)\"></div>\r\n";
            } else {
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/IsMarked?markObjectId=" + business.CustomerId + "&markType=2&token=" + _Token,
                    type: "POST",
                    data: null,
                    async: false,
                    success: function (data) {
                        if (data.Flag == 1) {
                            str += "        <div class=\"Collection on\" title=\"取消关注\"  onclick=\"deleteMark('" + data.Result + "','" + business.CustomerId + "', this)\"></div>\r\n";
                        } else {
                            str += "        <div class=\"Collection\" title=\"关注\" onclick=\"markObject('" + business.CustomerId + "', this)\"></div>\r\n";
                        }
                    },   
                    error: function (data) {
                        //alert(data);
                    }
                });
            }
            str += "        <a target=\"_blank\" title=\"" + business.CompanyName + "\" href=\"Business.aspx?businessId=" + business.BusinessId + "\"><div class=\"headimg\" style=\"background-image:url(" + headimg + ")\"></div></a>\r\n";
            str += "        <div class=\"info\">\r\n";
            str += "        	<div class=\"name_div\">\r\n";
            str += "            	<span class=\"name\"><a target=\"_blank\" title=\"" + business.CompanyName + "\" href=\"Business.aspx?businessId=" + business.BusinessId + "\">" + business.CompanyName + "</a></span>\r\n";
            str += "                <span class=\"level l" + parseInt(business.ReviewScore) + "\">\r\n";
            str += "                	<div class=\"level0\"><em></em><em></em><em></em><em></em><em></em></div>\r\n";
            str += "                    <div class=\"level1\"><em></em><em></em><em></em><em></em><em></em></div>\r\n";
            str += "                </span>\r\n";
            str += "            </div>\r\n";
            str += "            <ul class=\"info_list\">\r\n";
            //str += "            	<div>\r\n";
            
            str += "                    <li class=\"on\">所在城市：" + business.CityName + "</li>\r\n";
            if (business.CompanyType != "")
                str += "                    <li>公司类型：" + business.CompanyType + "</li>\r\n";
            //str += "                </div>\r\n";
            
            if (business.CategoryNames != "")
                str += "                <li title=\"" + business.CategoryNames + "\">行业分类：" + RemoveLongString(RemoveComma(business.CategoryNames), 23) + "</li>\r\n";
            if (business.TargetCategory != "")
                str += "                <li title=\"" + business.TargetCategory + "\">目标行业：" + RemoveLongString(RemoveComma(business.TargetCategory), 23) + "</li>\r\n";
            if (business.TargetCity != "")
                str += "                <li title=\"" + business.TargetCity + "\">目标区域：" + RemoveLongString(RemoveComma(business.TargetCity), 23) + "</li>\r\n";
           /* if (business.SetupDate != "")
                str += "                <li title=\"" + business.SetupDate + "\">创建日期：" + new Date(business.SetupDate).format("yyyy-MM-dd") + "</li>\r\n";
                */
            var dd = business.MainProducts;
            dd = dd.replace(/<\/?.+?>/g, "")
            var dds = dd.replace(/ /g, "");
            if (dds != "")
                str += "                <li title=\"" + RemoveLongString(dds, 100) + "\">企业简介：" + RemoveLongString(dds, 100) + "</li>\r\n";


            var dd2 = business.ProductDescription;
            dd2 = dd2.replace(/<\/?.+?>/g, "")
            var dds2 = dd2.replace(/ /g, "");
            if (dds2 != "")
                str += "                <li title=\"" + RemoveLongString(dds2, 100) + "\">产品介绍：" + RemoveLongString(dds2, 100) + "</li>\r\n";
            str += "            </ul>\r\n";
            str += "            <div class=\"lbtn\">\r\n";
            str += "                <div class=\"bdsharebuttonbox lg_list_bdshare\"><a href=\"#\" data-cmd=\"more\"  onclick=\"btnSetShareUrl('" + _SITEURL + "/Business.aspx?businessId=" + business.BusinessId + "')\">分享</a></div>\r\n";
			str += "                <div class=\"lg_list_wxshare\"><a href=\"#\" onclick=\"wx_share_show()\">分享</a></div>\r\n";
            str += "                <a href=\"ZXTIM.aspx?MessageTo=" + business.CustomerId + "\" target=\"_blank\">线上联系</a>\r\n";
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
                if (NowpageIndex > 2) {
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
        }
    });

}
var minAwayBtm = 0;  // 距离页面底部的最小距离
var NowpageIndex = 1;//当前页码
$(window).scroll(function () {
    var awayBtm = $(document).height() - $(window).scrollTop() - $(window).height();
    if (awayBtm <= minAwayBtm && !isloading) {
        if (!isFromAll) {
            isloading = true;
            isFromSearch = false;
            $('#lloading').show();
            var filterModel = getFilterModel();
            NowpageIndex = NowpageIndex + 1;
            console.log(NowpageIndex);
            filterModel.PageInfo.PageIndex = NowpageIndex;
            GetBusinessList(filterModel);
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

function RemoveLongString(input, len) {
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

    GetBusinessList(filterModel);
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

    GetBusinessList(filterModel);
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

    GetBusinessList(filterModel);
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
        markVO.MarkType = 2;
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