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
    GetRequireList(filterModel);

    NowpageIndex = 1;//重置页码
    isFromSearch = true;
    isFromAll = false;

    SetPaging("GetRequireList", filterModel);
}

function getFilterModel() {
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = 1;
    pageInfoObj.PageCount = 20;
    pageInfoObj.SortName = "PublishAt";
    pageInfoObj.SortType = "desc";
    
    var sortTypeobj = $("#divSort").find(".sort");
    if (sortTypeobj) {
        var sortName = sortTypeobj.attr("sortname");
        var sortType = sortTypeobj.attr("sorttype");
        if (sortName == "1") {
            pageInfoObj.SortName = "PublishAt";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "2") {
            pageInfoObj.SortName = "BusinessReviewScore";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "3") {
            pageInfoObj.SortName = "CXDReviewScore";
            pageInfoObj.SortType = sortType;
        } else if (sortName == "4") {
            pageInfoObj.SortName = "JSXReviewScore";
            pageInfoObj.SortType = sortType;
        }
        else if (sortName == "5") {
            pageInfoObj.SortName = "PublishAt";
            pageInfoObj.SortType = sortType;
        }
        else if (sortName == "6") {
            pageInfoObj.SortName = "RemainDate";
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

    ruleObj = new Object();
    rulesObj.push(ruleObj);
    ruleObj.field = "status";
    ruleObj.op = "ne";
    ruleObj.data = "5";

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
    }else if (cityObj) {
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
                if (optionList[i].value != "-1" && cityObj.val() != "-2") {
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
        //ruleObj = new Object();
        //rulesObj.push(ruleObj);
        //ruleObj.field = "Title";
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
        ruleSerachObj.field = "AgencyCondition";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CategoryName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CityName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ClientName";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CommissionDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "CompanyDescription";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ContactPerson";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "ContactPhone";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Description";
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
        ruleSerachObj.field = "RequirementCode";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "TargetAgency";
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

        var ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Title";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();
    }

    if ($("#commissionstart").val() != "" || $("#commissionend").val() != "") {
        if ($("#commissionstart").val() != "") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "ge";
            ruleCommission.data = $("#commissionstart").val();
        }
        if ($("#commissionend").val() != "") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "le";
            ruleCommission.data = $("#commissionend").val();
        }

    } else {

        var commissionConditiontype = $("#conCommission").find(".color").attr("filtertype");
        if (commissionConditiontype == "1") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "lt";
            ruleCommission.data = "1000";
        } else if (commissionConditiontype == "2") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "lt";
            ruleCommission.data = "10000";

            ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "ge";
            ruleCommission.data = "1000";

        } else if (commissionConditiontype == "3") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "lt";
            ruleCommission.data = "100000";

            ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "ge";
            ruleCommission.data = "10000";
        } else if (commissionConditiontype == "4") {
            var ruleCommission = new Object();
            rulesObj.push(ruleCommission);
            ruleCommission.field = "Commission";
            ruleCommission.op = "ge";
            ruleCommission.data = "100000";
        }
    }

    //var tenderConditiontype = $("#conTender").find(".color").attr("filtertype");
    //if (tenderConditiontype == "1") {
    //    var ruleTender = new Object();
    //    rulesObj.push(ruleTender);
    //    ruleTender.field = "RemainTenderCount";
    //    ruleTender.op = "gt";
    //    ruleTender.data = "0";
    //}

    var createdAtConditiontype = $("#conCreatedAt").find(".color").attr("filtertype");
    if (createdAtConditiontype == "1") {
        var currentDate = new Date();
        var nextDate = new Date();
        nextDate.setDate(nextDate.getDate() + 1);
        var ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "PublishAt";
        ruleCreated.op = "ge";
        ruleCreated.data = currentDate.format("yyyy-MM-dd") + " 00:00:000";

        ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "PublishAt";
        ruleCreated.op = "lt"; 
        ruleCreated.data = nextDate.format("yyyy-MM-dd") + " 00:00:000";
    } else if (createdAtConditiontype == "2") {
        var currentDate = new Date();
        //var yesterdayDate = new Date();
        //yesterdayDate.setDate(yesterdayDate.getDate() - 1);
        //var ruleCreated = new Object();
        //rulesObj.push(ruleCreated);
        //ruleCreated.field = "PublishAt";
        //ruleCreated.op = "ge";
        //ruleCreated.data = yesterdayDate.format("yyyy-MM-dd") + " 00:00:000";

        ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "PublishAt";
        ruleCreated.op = "lt";
        ruleCreated.data = currentDate.format("yyyy-MM-dd") + " 00:00:000";
    } else if (createdAtConditiontype == "3") {
        var currentDate = new Date();
        var nextDate = new Date();
        nextDate.setDate(nextDate.getDate() + 1);

        var ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "ProjectStatus";
        ruleCreated.op = "ge";
        ruleCreated.data = 0;

        ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "ProjectStatus";
        ruleCreated.op = "ne";
        ruleCreated.data = 2;
    } else if (createdAtConditiontype == "4") {
        var currentDate = new Date();
        var nextDate = new Date();
        nextDate.setDate(nextDate.getDate() + 1);

        var ruleCreated = new Object();
        rulesObj.push(ruleCreated);
        ruleCreated.field = "ProjectStatus";
        ruleCreated.op = "ieq";
        ruleCreated.data = 2;
       
    }


    return filterModel;
}
var isFromSearch = true; //判断是不是第一次加载
var isFromAll = false;//是否已加载完全部
var isloading = false;//是否正在加载
function GetRequireList(filterModel) {    

    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetRequireList",
        type: "POST",
        data: filterModel,
        success: function (data) {
        var requireList = data.Result;
        var divData = $("div[id*='divList']");
        var str = "\r\n";
        if (data.Flag = 0 || requireList.length == 0) {
            if (NowpageIndex == 1) {
                divData.html("<span class=\"lloading\">暂未找到相关数据</span>");
                return;
            }
        }
        for (var i = 0; i < requireList.length; i++) {
            var require = requireList[i];

            str += "<li>\r\n";
            //如果已经登录,判断是否已经关注了
            if (_CustomerId < 1 || _Token == "") {
                str += "        <div class=\"Collection\"  onclick=\"markObject('" + require.RequirementId + "', this)\"></div>\r\n";
            } else {
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/IsMarked?markObjectId=" + require.RequirementId + "&markType=4&token=" + _Token,
                    type: "POST",
                    data: null,
                    async: false,
                    success: function (data) {
                        if (data.Flag == 1) {
                            str += "        <div class=\"Collection on\" title=\"取消关注\"  onclick=\"deleteMark('" + data.Result + "','" + require.RequirementId + "', this)\"></div>\r\n";
                        } else {
                            str += "        <div class=\"Collection\" title=\"关注\" onclick=\"markObject('" + require.RequirementId + "', this)\"></div>\r\n";
                        }
                    },
                    error: function (data) {
                        //alert(data);
                    }
                });
            }


            var headimg = _APIURL + "/Style/images/wxapp/noimg.png";
            if (require.MainImg != "") {
                headimg = require.MainImg;
            }
            str += "        <a target=\"_blank\" title=\"" + require.Title + "\" href=\"Require.aspx?requireId=" + require.RequirementId + "\"><div class=\"headimg\" style=\"background-image:url(" + headimg + ")\"></div></a>\r\n";
            str += "        <div class=\"info\">\r\n";
            str += "        	<div class=\"name_div\">\r\n";
            str += "            	<span class=\"name lrequire_name\"><a target=\"_blank\" title=\"" + require.Title + "\" href=\"Require.aspx?requireId=" + require.RequirementId + "\">" + require.Title + "</a></span>\r\n";
            str += "            </div>\r\n";
            str += "            <ul class=\"info_list\">\r\n";
            str += "            	<div>\r\n";
            str += "                    <li class=\"on\">酬金：&yen;" + thousandBitSeparator(require.Commission.toFixed(2)) + "</li>\r\n";
            str += "                    <li>托管状态：" + ((require.DelegationCommission > 0) ? "已托管" : "未托管") + "</li>\r\n";
            str += "                </div>\r\n";

            if (require.CommissionDescription != "")
                str += "                <li title=\"" + require.CommissionDescription + "\">酬金说明：" + RemoveLongString(require.CommissionDescription, 23) + "</li>\r\n";
            if (require.TargetCity != "")
                str += "                <li title=\"" + require.TargetCity + "\">目标城市：" + RemoveLongString(RemoveComma(require.TargetCity), 23) + "</li>\r\n";
            if (require.TargetCategory != "")
                str += "                <li title=\"" + require.TargetCategory + "\">目标行业：" + RemoveLongString(RemoveComma(require.TargetCategory), 23) + "</li>\r\n";
            if (require.TargetAgency != "")
                str += "                <li title=\"" + require.TargetAgency + "\">目标销售：" + RemoveLongString(RemoveComma(require.TargetAgency), 23) + "</li>\r\n";
            if (require.TargetClient != "")
                str += "                <li title=\"" + require.TargetClient + "\">目标客户：" + RemoveLongString(RemoveComma(require.TargetClient), 23) + "</li>\r\n";
            
            var dd2 = require.Description;
            dd2 = dd2.replace(/<\/?.+?>/g, "")
            var dds2 = dd2.replace(/ /g, "");
            if (dds2 != "")
                str += "                <li title=\"" + dds2 + "\">任务详情：" + RemoveLongString(dds2, 100) + "</li>\r\n";
            str += "            </ul>\r\n";
            str += "            <div class=\"lbtn\">\r\n";
            str += "                <div class=\"bdsharebuttonbox lg_list_bdshare\"><a href=\"#\" data-cmd=\"more\"  onclick=\"btnSetShareUrl('" + _SITEURL + "/Require.aspx?requireId=" + require.RequirementId + "')\">分享</a></div>\r\n";
			str += "                <div class=\"lg_list_wxshare\"><a href=\"#\" onclick=\"wx_share_show()\">分享</a></div>\r\n";
            str += "                <button type=\"button\" onclick=\"onTenderInvite('" + require.RequirementId + "')\" >我要接单</button>\r\n";
            str += "                <a href=\"ZXTIM.aspx?MessageTo=" + require.CustomerId + "\" target=\"_blank\">线上联系</a>\r\n";
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
//千位分隔符
function thousandBitSeparator(num) {
    return num && (num
      .toString().indexOf('.') != -1 ? num.toString().replace(/(\d)(?=(\d{3})+\.)/g, function ($0, $1) {
          return $1 + ",";
      }) : num.toString().replace(/(\d)(?=(\d{3}))/g, function ($0, $1) {
          return $1 + ",";
      }));
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
            GetRequireList(filterModel);
        }
    }
});
function delHtmlTag(str) {
    return str.replace(/<[^>]+>/g, "");//去掉所有的html标记
}

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

    GetRequireList(filterModel);
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

    GetRequireList(filterModel);

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

    GetRequireList(filterModel);
    $("html,body").animate({ scrollTop: target_top }, 1000);
    return false;
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
        if (isAgency) {
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
        } else {
            bootbox.dialog({
                message: "通过销售认证才能投标！",
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
    }
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
        markVO.CustomerId =_CustomerId;
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

function deleteMark(markId,requireId, imgObj) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/DeleteMark?markId="+markId+"&token=" + _Token,
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