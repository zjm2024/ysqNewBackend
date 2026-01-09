$(function () {
    onPageInit();
    onCustomerPayOutListTableInit();
});
var grid = new JGrid();
var grid2 = new JGrid();
var gridcard = new JGrid();
var gridIncome = new JGrid();
function onPageInit() {

    var CustomerId = $("input[id*='CustomerId']").val();
    grid.jqGrid.ID = "CardBalanceList";
    grid.jqGrid.PagerID = "CardBalanceListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardBalanceList&CustomerId=" + CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "PartyOrderID";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;


    grid.jqGrid.AddHidColumn("ToID");	
    grid.jqGrid.AddColumn("OrderNO", "订单编号", true, "center", 50);
    grid.jqGrid.AddColumn("Cost", "收入金额", true, "center", 50);
    grid.jqGrid.AddColumn("PromotionAwardCost", "推广返佣（支出）", true, "center", 50);
    grid.jqGrid.AddColumn("CostName", "收入备注", true, "center", 50);
    grid.jqGrid.AddColumn("CostStyle", "收入类型", true, "center", 50);
    grid.jqGrid.AddColumn("payAt", "收入时间", true, "center", 50);
    grid.jqGrid.AddColumn("Title", "来源", true, "center", 50);
    grid.jqGrid.CreateTable();

    gridIncome.jqGrid.ID = "CardOrderIncomeList";
    gridIncome.jqGrid.PagerID = "CardOrderIncomeListDiv";
    gridIncome.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridIncome.jqGrid.Params = "table=CardOrderIncomeList&CustomerId=" + CustomerId;
    gridIncome.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    gridIncome.jqGrid.DefaultSortCol = "CardOrderID";
    gridIncome.jqGrid.DefaultSort = "desc";
    gridIncome.jqGrid.Multiselect = true;
    gridIncome.jqGrid.AddHidColumn("CardOrderID");
    gridIncome.jqGrid.AddColumn("OrderNO", "订单编号", true, "center", 50);
    gridIncome.jqGrid.AddColumn("Cost", "收入金额", true, "center", 50);
    gridIncome.jqGrid.AddColumn("Position", "收入备注", true, "center", 50);
    gridIncome.jqGrid.AddColumn("payAt", "收入时间", true, "center", 50);
    gridIncome.jqGrid.AddColumn("HeaderLogo", "来源会员", true, "center", 20,
        function (obj, options, rowObject) {
            var str = "<div style='display: table;margin: 0 auto;'>";
            str += "<span style='width: 40px;height: 40px;display: block;border-radius: 63px;float:left;background-repeat: no-repeat;background-size: cover;background-position: center center;background-image: url(" + rowObject.HeaderLogo + ")'></span>";
            str += "<span style='margin-left:10px;line-height: 40px;display: block;float:left;'>" + rowObject.CustomerName + "</span>";
            str += "</div>";
            return str;
        }, false);
    gridIncome.jqGrid.CreateTable();

    gridcard.jqGrid.ID = "AgencyList";
    gridcard.jqGrid.PagerID = "AgencyListDiv";
    gridcard.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridcard.jqGrid.Params = "table=CardList&CustomerId=" + CustomerId;
    gridcard.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    gridcard.jqGrid.DefaultSortCol = "CreatedAt";
    gridcard.jqGrid.DefaultSort = "desc";
    gridcard.jqGrid.Multiselect = true;
    gridcard.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    gridcard.jqGrid.AddColumn("CardImg", "名片码", true, "center", 20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.CardID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    gridcard.jqGrid.AddColumn("Name", "名称", true, null, 25);
    gridcard.jqGrid.AddColumn("Position", "职位", true, null, 30);
    gridcard.jqGrid.AddColumn("Phone", "手机号码", true, null, 30);
    gridcard.jqGrid.AddColumn("WeChat", "微信号", true, null, 30);
    gridcard.jqGrid.AddColumn("CorporateName", "公司名称", true, null, 50);
    gridcard.jqGrid.AddColumn("Address", "地址", true, null, 50);
    gridcard.jqGrid.AddColumn("Business", "主营业务", true, null, 50);
    gridcard.jqGrid.AddColumn("ReadCount", "浏览", true, "center", 10);
    gridcard.jqGrid.AddColumn("Collection", "收藏", true, "center", 10);
    gridcard.jqGrid.AddColumn("Forward", "转发", true, "center", 10);
    gridcard.jqGrid.AddColumn("isParty", "活动名片", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == 1) {
                return "<div style='color:#f00'>是</div>";
            }
            else {
                return "否";
            }
        });
    gridcard.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 30);
    gridcard.jqGrid.CreateTable();
}


function onCustomerPayOutListTableInit() {

    var CustomerId = $("input[id*='CustomerId']").val();
    grid2.jqGrid.ID = "CardPayOutHandleList";
    grid2.jqGrid.PagerID = "CardPayOutHandleListDiv";
    grid2.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid2.jqGrid.Params = "table=CardPayOutHandleList2&CustomerId=" + CustomerId;
    grid2.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid2.jqGrid.DefaultSortCol = "PayOutDate";
    grid2.jqGrid.DefaultSort = "desc";

    grid2.jqGrid.AddColumn("PayOutHistoryId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditPayOutHistory(this);"></img>';
            return result;
        }, false);
    grid2.jqGrid.AddHidColumn("PayOutHistoryId");
    grid2.jqGrid.AddColumn("AccountName", "收款名称", true, null, 120);
    grid2.jqGrid.AddColumn("Type", "提现类型", true, "center", 120,
        function (obj, options, rowObject) {
            if (obj == '0') {
                if (rowObject.AccountName != "微信零钱")
                    return "银行卡提现";
                else
                    return "微信手动付款";
            }

            else if (obj == '1')
                return "微信自动付款";
            else if (obj == '2')
                return "对公账号";
        }, false);
    grid2.jqGrid.AddColumn("PayOutCost", "申请金额", true, null, 120);
    grid2.jqGrid.AddColumn("ServiceCharge", "服务费", true, null, 120);
    grid2.jqGrid.AddColumn("Cost", "实转金额", true, null, 120, function (obj, options, rowObject) {
        return "<div style='color:#882323'>" + obj + "</div>";
    });
    grid2.jqGrid.AddColumn("PayOutDate", "申请日期", true, null, 120);
    grid2.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 120,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "<div style='color:#882323;'>申请提现</div>";
            else if (obj == '-1')
                return "未提交申请";
            else if (obj == '-2')
                return "提现失败";
            else if (obj == '1')
                return "提现成功";
        }, false);
    grid2.jqGrid.AddColumn("HandleDate", "后台处理日期", true, null, 120);
    grid2.jqGrid.CreateTable();
}

function EditPayOutHistory(messageObj) {
    window.location.href = "CardPayOutHandle.aspx?PayOutHistoryId=" + $(messageObj).prev().val();
    return false;
}