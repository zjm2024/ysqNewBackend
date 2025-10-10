$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var CustomerId = parseInt($("#" + hidCustomerId).val());
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    if (CustomerId > 0) {
        grid.jqGrid.Params = "table=CardList&CustomerId=" + CustomerId;
    } else {
        grid.jqGrid.Params = "table=CardList";
    }
    
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CustomerId", "会员ID", true, "center", 20);
    grid.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {
            
            var result = '';
            if (obj != null && obj != "unknown" && obj != "" && obj != "undefined")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("CardImg", "名片码", true, "center",20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.CardID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid.jqGrid.AddColumn("CardID", "名片链接", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"CopyUrl('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看链接</div>";
        }, false);
    grid.jqGrid.AddColumn("CustomerId", "操作会员", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"EditCustomer('" + obj + "')\"  style='color:#0066ff;cursor:pointer;'  >查看会员(开通VIP)</div>";
        }, false);
    grid.jqGrid.AddColumn("Name", "名称", true, null,25);
    grid.jqGrid.AddColumn("Position", "职位", true, null, 30);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 30);
    grid.jqGrid.AddColumn("WeChat", "微信号", true, null, 30);
    grid.jqGrid.AddColumn("CorporateName", "公司名称", true, null, 50);
    grid.jqGrid.AddColumn("Address", "地址", true, null, 50);
    grid.jqGrid.AddColumn("Business", "主营业务", true, null, 50);
    grid.jqGrid.AddColumn("ReadCount", "浏览", true, "center", 10);
    grid.jqGrid.AddColumn("Collection", "收藏", true, "center", 10);
    grid.jqGrid.AddColumn("Forward", "转发", true, "center", 10);
    grid.jqGrid.AddColumn("isParty", "名片来源", true, "center", 20,
        function (obj, options, rowObject) {
            if (rowObject.isParty == 1)
            {
                return "<div style='color:#ff6600;'>活动</div>";
            }
            if (rowObject.isBusinessCard == 1) {
                return "<div style='color:#0066ff;'>企业</div>";
            }
            if (rowObject.isQuestionnaire == 1) {
                return "<div style='color:#6600ff;'>表格</div>";
            }
            else{
                return "名片";
            }
        });
    grid.jqGrid.AddColumn("originType", "来源功能细分", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == "S_BusinessCard") {
                return "企业版首页";
            }
            if (obj == "S_JoinBusiness") {
                return "加入企业";
            }
            if (obj == "S_JoinAgent") {
                return "加入企业";
            }
            if (obj == "S_UseWarehouse") {
                return "使用物品";
            }
            if (obj == "S_Personal") {
                return "查看企业名片";
            }
            if (obj == "S_ColorPage") {
                return "查看企业彩页";
            }
            if (obj == "S_GreetingCard") {
                return "贺卡替换分享";
            }
            if (obj == "S_News") {
                return "查看企业新闻";
            }
            if (obj == "S_Product") {
                return "查看企业产品";
            }
            if (obj == "C_Service") {
                return "名片马甲";
            }
            if (obj == "C_CreateNewParty") {
                return "发布活动";
            }
            if (obj == "C_CreatPartyPartyList") {
                return "历史活动";
            }
            if (obj == "C_CreatPartyPreviewParty") {
                return "预览活动";
            }
            if (obj == "CreatPartyCopyParty") {
                return "复制活动";
            }
            if (obj == "C_CreatePartyContacts") {
                return "发布活动";
            }
            
            if (obj == "C_CreatPartyContacts") {
                return "发布活动";
            }
            if (obj == "CreatPartyLinkCompany") {
                return "链接企业";
            }
            if (obj == "C_BusinessCardHolder") {
                return "名片夹";
            }
            if (obj == "C_BusinessCardHolderCardGroup") {
                return "名片群";
            }
            if (obj == "C_BusinessCardHolderChatRoom") {
                return "乐聊";
            }
            if (obj == "C_CardGroupJoin") {
                return "加入名片群";
            }
            if (obj == "C_Index") {
                return "个人版首页";
            }
            if (obj == "C_SignInFormByUser") {
                return "填写表单";
            }
            if (obj == "C_ArticleDetails") {
                return "查看文章";
            }
            if (obj == "C_ReplaceCard") {
                return "文章替换名片";
            }
            if (obj == "C_ArticleListRelease") {
                return "发布文章";
            }
            if (obj == "C_ArticleMenuRelease") {
                return "发布文章";
            }
            if (obj == "C_MyCenter") {
                return "个人中心";
            }
            if (obj == "C_CenterLogin") {
                return "个人中心登陆";
            }
            if (obj == "C_CenterDrawMoney") {
                return "个人中心提现";
            }
            if (obj == "C_CenterMsg") {
                return "个人中心消息";
            }
            if (obj == "C_CenterPartyList") {
                return "个人中心活动";
            }
            if (obj == "C_CenterOrder") {
                return "个人中心订单";
            }
            if (obj == "C_CenterLogin") {
                return "个人中心登陆";
            }
            if (obj == "C_CenterGeneralize") {
                return "个人中心推广";
            }
            if (obj == "C_CenterCompany") {
                return "个人中心企业";
            }
            if (obj == "C_CenterOpper") {
                return "个人中心商机";
            }
            if (obj == "C_CenterArticle") {
                return "个人中心文章";
            }
            if (obj == "C_CenterForm") {
                return "个人中心表格";
            }
            if (obj == "C_CenterInvite") {
                return "个人中心邀请";
            }
            if (obj == "C_OpperDetailsMsg") {
                return "商机留言";
            }
            if (obj == "C_OpperList") {
                return "商机列表";
            }
            if (obj == "C_ShowParty") {
                return "活动详情";
            }
            if (obj == "C_ShowPartySignUp") {
                return "报名活动";
            }
            if (obj == "C_ShowPartyTicket") {
                return "查看入场券";
            }
            if (obj == "C_ShowPartySignIn") {
                return "活动签到";
            }
            if (obj == "C_ShowCard") {
                return "查看个人名片";
            }
            if (obj == "C_ShowCardReturnCard") {
                return "回递名片";
            }
            if (obj == "C_ShowCardChat") {
                return "在线聊天";
            }
            if (obj == "C_ShowCardContacts") {
                return "查看人脉";
            }
            if (obj == "C_ShowPartyCreateCard") {
                return "在活动页面新建名片";
            }
            if (obj == "C_ShowCardCreateCard") {
                return "创建名片";
            }
            if (obj == "C_ShowCardListOfDemand") {
                return "商机动态";
            }
            if (obj == "C_ShowCardSavePhone") {
                return "保存手机";
            }
            if (obj == "C_ShowCardCollection") {
                return "收藏名片";
            }
            if (obj == "C_CreateNewCard") {
                return "新建名片";
            }
            if (obj == "C_CardGroupCreateGroup") {
                return "新建名片群";
            }
            if (obj == "C_CardGroupQRgroup") {
                return "新建群码";
            }
            else {
                return "-";
            }
        });
    grid.jqGrid.AddColumn("originName", "来源会员", true, null, 25);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 30);
    grid.jqGrid.AddColumn("LoginAt", "最近登录", true, "center", 20,
        function (obj, options, rowObject) {
            var dateTime = new Date(obj);;
            var year = dateTime.getFullYear();
            var month = dateTime.getMonth() + 1;
            var day = dateTime.getDate();
            var hour = dateTime.getHours();
            var minute = dateTime.getMinutes();
            if (hour < 10) {
                hour = "0" + hour;
            }
            if (minute < 10) {
                minute = "0" + minute;
            }

            var second = dateTime.getSeconds();
            var now = new Date();
            var now_new = Date.parse(now.toDateString());  //typescript转换写法

            var milliseconds = 0;
            var timeSpanStr;

            milliseconds = now - dateTime;

            if (milliseconds <= 1000 * 60 * 60 * 24) {
                timeSpanStr = '今天';
            }
            else if (1000 * 60 * 60 * 24 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24 * 30) {
                timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60 * 24)) + '天前';
            }
            else if (milliseconds > 1000 * 60 * 60 * 24 * 30 && year == now.getFullYear()) {
                timeSpanStr = month + '月' + day + '日';
            } else {
                timeSpanStr = year + '年' + month + '月' + day + '日';
            }
            return timeSpanStr;
        });

    grid.jqGrid.CreateTable();   
}

function CopyUrl(obj) {
    bootbox.dialog({
        message: "pages/ShowCard/ShowCard?CardID=" + obj,
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


function showQRImg(obj) {
    window.open(obj);
}

function CardAdd() {
    window.open("CardAdd.aspx");
    return false;
}

function EditCustomer(Obj) {
    window.open("CustomerCreateEdit.aspx?CustomerId=" + Obj);
    return false;
}



function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objAgencyName = $("input[id*='txtAgencyName']");    

    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("Name");
        filedArr.push("Position");
        filedArr.push("Phone");
        filedArr.push("CorporateName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    var objRealNameStatus = $("select[id*='drpRealNameStatus']");
    if (objRealNameStatus.val() > -1)
        grid.jqGrid.AddSearchParams("RealNameStatus", "=", objRealNameStatus.val());

    grid.jqGrid.Search();
    return false;
}

