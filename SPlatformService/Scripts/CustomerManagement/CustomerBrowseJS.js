$(function () {
    SetButton();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CustomerList";
    grid.jqGrid.PagerID = "CustomerListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CustomerId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditCustomer(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteCustomerOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("CustomerId");					
	grid.jqGrid.AddColumn("CustomerCode", "会员编号", true, null, 50);				
	grid.jqGrid.AddColumn("CustomerAccount", "会员账号", true, null, 50);				
	grid.jqGrid.AddColumn("HeaderLogo", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
	grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
	grid.jqGrid.AddColumn("CustomerName", "查看名片", true, "center", 50,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >个人名片</div>";
            return result;
        }, false);
	grid.jqGrid.AddColumn("CustomerName", "查看名片", true, "center", 50,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showBusinessCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >企业名片</div>";
            return result;
        }, false);
	grid.jqGrid.AddColumn("Phone", "联系电话", true, null, 50);
	grid.jqGrid.AddColumn("CreatedAt", "注册时间", true, null, 50);
	grid.jqGrid.AddColumn("Status", "会员来源", true, null, 50,
        function (obj, options, rowObject) {
            if (rowObject.Leliao+'' == 'True') {
                return "个人名片";
            } else if (rowObject.BusinessCard + '' == 'True') {
                return "企业名片"
            }
            else {
                return "众销乐";
            }
        }, false);
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
    /*
	grid.jqGrid.AddColumn("Status", "会员类型", true, null, 50,
        function (obj, options, rowObject) {
            if (rowObject.AgencyId > 0 && rowObject.BusinessId > 0) {
                return "雇主/销售";
            } else if(rowObject.AgencyId > 0 && rowObject.BusinessId == 0) {
                return "销售";
            }
            else if (rowObject.AgencyId == 0 && rowObject.BusinessId > 0) {
                return "雇主";
            }
            else {
                return "普通会员";
            }
        }, false);
	grid.jqGrid.AddColumn("Email", "邮箱", true, null, 50);
    grid.jqGrid.AddColumn("originCustomerId", "来源ID", true, null, 50);
    grid.jqGrid.AddColumn("StatusName", "状态", true, null, 50);
    */
    grid.jqGrid.AddColumn("Agent", "是否代理商", true, null, 50,
        function (obj, options, rowObject) {
            if (obj+"" == 'False') {
                return "普通会员";
            } else{
                return "<div style='color:#f00'>代理商</div>";
            }
        }, false);
    grid.jqGrid.AddColumn("isVip", "是否VIP", true, null, 50,
        function (obj, options, rowObject) {
            if (obj + "" == 'False') {
                return "普通会员";
            } else {
                
                let today = new Date();
                let exdate = new Date(rowObject.ExpirationAt.replace("-", "/"));
                if (exdate > today) {
                    if (rowObject.VipLevel > 0) {
                        if(rowObject.VipLevel==1)
                            return "<div style='color:#f00'>五星会员</div>";
                        if (rowObject.VipLevel == 2)
                            return "<div style='color:#f00'>合伙人</div>";
                        if (rowObject.VipLevel == 3)
                            return "<div style='color:#f00'>分公司</div>";
                        if (rowObject.VipLevel == 4)
                            return "<div style='color:#f00'>三星会员</div>";
                        if (rowObject.VipLevel == 5)
                            return "<div style='color:#f00'>四星会员</div>";
                    }
                    return "<div style='color:#f00'>VIP会员</div>";
                } else {
                    return "普通会员";
                }
                
            }
        }, false);
    
    grid.jqGrid.CreateTable();   
}


function EditCustomer(customerObj) {
    window.location.href = "CustomerCreateEdit.aspx?CustomerId=" + $(customerObj).prev().val();
    return false;
}

function UpdateCustomerStatus(status) {
    var id = $("#CustomerList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }

    var msg = "";
    if (status == 0) {
        msg = "是否确认禁用账号?";
    } else {
        msg = "是否确认启用账号?";
    }

    if (id.length > 0) {
        bootbox.dialog({
            message: msg,
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateCustomerStatusAction(idString, status);
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

function UpdateCustomerStatusAction(customerId,status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/UpdateCustomerStatus?customerId=" + customerId + "&status=" + status + "&token=" + _Token,
        type: "Post",
        data: null,
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
                                window.location.href = "CustomerBrowse.aspx";
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
            //load_hide();
        },
        error: function (data) {
            alert(data);
            //load_hide();
        }
    });
}

function SetButton() {
        
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objCustomerName = $("input[id*='txtCustomerName']");
    if (objCustomerName.val() != "") {

        //grid.jqGrid.AddSearchParams("CustomerName", "LIKE", objCustomerName.val());
        var filedArr = new Array();
        filedArr.push("CustomerName");
        filedArr.push("CustomerCode");
        filedArr.push("CustomerAccount");
        filedArr.push("Phone");
        filedArr.push("AgencyName");
        filedArr.push("CompanyName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objCustomerName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    var objoriginType = $("select[id*='drporiginType']");
    if (objoriginType.val() != "null")
        grid.jqGrid.AddSearchParams("originType", "=",objoriginType.val());
    console.log(objoriginType.val())


    var objCustomerType = $("select[id*='drpCustomerType']");
    var typeValue = objCustomerType.val() ;
    if (typeValue == 0) {        
        grid.jqGrid.AddSearchParams("Leliao", "<>", 1);
        grid.jqGrid.AddSearchParams("BusinessCard", "<>", 1);
    } else if (typeValue == 1) {
        grid.jqGrid.AddSearchParams("Leliao", "=", 1);
        
    } else if (typeValue == 2) {
        grid.jqGrid.AddSearchParams("BusinessCard", "=", 1);
       
    }
    grid.jqGrid.Search();
    return false;
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}

function showBusinessCard(Obj) {
    window.open("CardPersonalBrowse.aspx?CustomerId=" + Obj);
    return false;
}