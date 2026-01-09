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
    var drpCategory = $("select[class*='drpCategory']");
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

	var CategoryObj = $("select[class*='drpCategory']");
	if(CategoryObj.val()!=0)
	{
		ruleObj.field = "CategoryId";
        ruleObj.op = "eq";
        ruleObj.data = CategoryObj.val();
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
        ruleSerachObj.field = "Description";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();

        ruleSerachObj = new Object();
        rulesSerachObj.push(ruleSerachObj);
        ruleSerachObj.field = "Phone";
        ruleSerachObj.op = "cn";
        ruleSerachObj.data = inputObj.val();     
    }

	

    return filterModel;
}
var isFromSearch = true; //判断是不是第一次加载
var isFromAll = false;//是否已加载完全部
var isloading = false;//是否正在加载
function GetAgencyList(filterModel) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetDemandList",
        type: "POST",
        data: filterModel,
        success: function (data) {
            $('#lloading').hide();
            var agencyList = data.Result;
            console.log(agencyList)
            var divData = $("div[id*='divList']");
            var str = "\r\n";
            if (data.Flag = 0 || agencyList.length == 0) {
                if (NowpageIndex == 1) {
                    divData.html("<span class=\"lloading\">暂未找到相关数据</span>");
                    return;
                }
            }
            for (var i = 0; i < agencyList.length; i++) {
                var agency = agencyList[i];

                var CustomerName = agency.CustomerName;
                if (CustomerName.match(/^((1)+\d{10})$/)) {
                    CustomerName = agency.CustomerName.substring(0, 3) + "****" + agency.CustomerName.substring(7, 11)
                }

                var headimg = _APIURL + "/Style/images/wxapp/noimg.png";
                if (agency.HeaderLogo != "") {
                    headimg = agency.HeaderLogo;
                }

                var days = daysBetween(new Date(Date.now()), agency.EffectiveEndDate);

                str += "<li>\r\n";
                str += "<a target=\"_blank\" title=\"" + CustomerName + "\" href=\"Demand.aspx?DemandId=" + agency.DemandId + "\"><div class=\"Demand_headimg\" style=\"background-image:url(" + headimg + ")\"></div></a>\r\n";
				str += "	<div class=\"Demand_info\">\r\n";
				str += "		<div class=\"Demand_title\"><a target=\"_blank\" title=\"" + CustomerName + "\" href=\"Demand.aspx?DemandId=" + agency.DemandId + "\"><font class=\"Demand_Phone\">" + CustomerName + "</font></a><div class=\"Demand_EffectiveEndDate\">" + days + "天后截止</div></div>";
				str += "		<div class=\"CategoryName\">需求类别：" + agency.CategoryName + "</div>\r\n";
				var dd2 = agency.Description;
				dd2 = dd2.replace(/<\/?.+?>/g, "")
				var dds2 = dd2.replace(/ /g, "");
				str += "		<div class=\"text\" title=\"" + dds2 + "\">需求详情：" + dds2 + "</div>\r\n";
				var CreatedAt=agency.CreatedAt;
				CreatedAt = CreatedAt.split("T");
				str += "		<div class=\"Demand_time\">发布时间：" + CreatedAt[0] + "&nbsp;&nbsp;&nbsp;&nbsp;收到留言：" + agency.OfferCount + "条</div>\r\n";
				str += "        <div class=\"bdsharebuttonbox lg_list_bdshare\"><a href=\"#\" data-cmd=\"more\"  onclick=\"btnSetShareUrl('" + _SITEURL + "/Demand.aspx?DemandId=" + agency.DemandId + "')\">分享</a></div>\r\n";
				str += "                <div class=\"Demand_btn lg_list_wxshare\" onclick=\"wx_share_show()\">分享</div>\r\n";
				str += "	    <div class=\"Demand_btn\" onclick=\"Newform_demand_alert(" + agency.DemandId + ")\">关注留言</div>\r\n";
				str += "	    <a class=\"Demand_btn\" href=\"ZXTIM.aspx?MessageTo=" + agency.CustomerId + "\" target=\"_blank\">线上联系</a>\r\n";
				str += "	</div>\r\n";
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
            GetAgencyList(filterModel);
        }
    }
});
function savebtn(){
    var OfferName = $("input[name*='OfferName']");
    var OfferDescription = $("textarea[name*='OfferDescription']");
    var OfferDemandId = $("input[name*='OfferDemandId']");
	var OfferPhone = $("input[name*='OfferPhone']");
	
	if (_CustomerId <= 0) {
        bootbox.dialog({
            message: "请先登录后再继续操作！",
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
	
	if (OfferName.val() == "") {
        bootbox.dialog({
            message: "请输入你的名称",
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
        return false;
    }
	

	if (OfferPhone.val() == "") {
        bootbox.dialog({
            message: "请输入你的电话号码",
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
        return false;
    }
	
	var seller_data = {
        "DemandId": OfferDemandId.val(),
		"Name": OfferName.val(),
        "Phone": OfferPhone.val(),
        "OfferDescription": OfferDescription.val()
    }
	
	$.ajax({
        url: _RootPath + "SPWebAPI/Require/AddDemandOffer?Token=" + _Token,
        type: "POST",
        data: seller_data,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                Newform_close('demandoffer');
                bootbox.dialog({
                    message: "留言成功",
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
            console.log(data);
            alert(data);
        }
    });
}
/**
* 根据两个日期，判断相差天数
* @param sDate1 开始日期 如：2016-11-01
* @param sDate2 结束日期 如：2016-11-02
* @returns {number} 返回相差天数
*/
function daysBetween(sDate1, sDate2) {
    //Date.parse() 解析一个日期时间字符串，并返回1970/1/1 午夜距离该日期时间的毫秒数
    var time1 = Date.parse(new Date(sDate1));
    var time2 = Date.parse(new Date(sDate2));
    var nDays = Math.abs(parseInt((time2 - time1) / 1000 / 3600 / 24));
    return nDays;
};
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