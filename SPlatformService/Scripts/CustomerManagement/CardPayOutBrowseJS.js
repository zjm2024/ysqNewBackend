$(function () {
   // Init();
    onCustomerPayOutListTableInit();
});
var grid = new JGrid();
var grid2 = new JGrid();
var grid3 = new JGrid();
function onCustomerPayOutListTableInit() {
    grid.jqGrid.ID = "CardPayOutHandleList";
    grid.jqGrid.PagerID = "CardPayOutHandleListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardPayOutHandleList";
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "PayOutStatus";
    grid.jqGrid.DefaultSort = "asc";

    grid.jqGrid.AddColumn("PayOutHistoryId", "操作", false, "center", 50,
      function (obj, options, rowObject) {
          var result = '';
          result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditPayOutHistory(this);"></img>';
          return result;
      }, false);
    grid.jqGrid.AddHidColumn("PayOutHistoryId");    
    grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectAmountDetail(" + obj + ")' style='cursor:pointer;'>查看收入明细</div>";
        }, false);
    grid.jqGrid.AddColumn("AccountName", "收款名称", true, null, 120);
    grid.jqGrid.AddColumn("Type", "提现类型", true, "center", 120,
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
    grid.jqGrid.AddColumn("PayOutCost", "申请金额", true, null, 120);
    grid.jqGrid.AddColumn("ServiceCharge", "服务费", true, null, 120);
    grid.jqGrid.AddColumn("Cost", "实转金额", true, null, 120, function (obj, options, rowObject) {
        return "<div style='color:#882323'>" + obj + "</div>";
    });
    grid.jqGrid.AddColumn("PayOutDate", "申请日期", true, null, 120);
    grid.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 120,
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
    //grid.jqGrid.AddColumn("HandleComment", "备注", true, null, 120);
    grid.jqGrid.CreateTable();
    
    grid2.jqGrid.ID = "CardPayOutHandleList2";
    grid2.jqGrid.PagerID = "CardPayOutHandleListDiv2";
    grid2.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid2.jqGrid.Params = "table=CardPayOutHandleList2";
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
    grid2.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectAmountDetail(" + obj + ")' style='cursor:pointer;'>查看收入明细</div>";
        }, false);
    grid2.jqGrid.AddColumn("AccountName", "收款名称", true, null, 120);
    grid2.jqGrid.AddColumn("Type", "提现类型", true, "center", 120,
        function (obj, options, rowObject) {
            if (obj == '0')
            {
                if (rowObject.AccountName!="微信零钱")
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
    grid2.jqGrid.AddColumn("PayOutDate", "申请日期", true, null, 60);
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
    grid2.jqGrid.AddColumn("HandleDate", "后台处理日期", true, null, 60);
    grid2.jqGrid.CreateTable();


    grid3.jqGrid.ID = "BcPayOutHistoryList";
    grid3.jqGrid.PagerID = "BcPayOutHistoryListDiv";
    grid3.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid3.jqGrid.Params = "table=BcPayOutHistoryList";
    grid3.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid3.jqGrid.DefaultSortCol = "PayOutDate";
    grid3.jqGrid.DefaultSort = "desc";

    grid3.jqGrid.AddColumn("PayOutHistoryId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditBcPayOutHistory(this);"></img>';
            return result;
        }, false);
    grid3.jqGrid.AddHidColumn("PayOutHistoryId");
    grid3.jqGrid.AddColumn("AccountName", "收款名称", true, null, 120);
    grid3.jqGrid.AddColumn("Type", "提现类型", true, "center", 120,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "个人提现到微信零钱";
            else if (obj == '2')
                return "企业提现到绑定账户";
        }, false);
    grid3.jqGrid.AddColumn("PayOutCost", "申请金额", true, null, 120);
    grid3.jqGrid.AddColumn("ServiceCharge", "服务费", true, null, 120);
    grid3.jqGrid.AddColumn("Cost", "实转金额", true, null, 120, function (obj, options, rowObject) {
        return "<div style='color:#882323'>" + obj + "</div>";
    });
    grid3.jqGrid.AddColumn("PayOutDate", "申请日期", true, null, 60);
    grid3.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 120,
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
    grid3.jqGrid.AddColumn("HandleDate", "后台处理日期", true, null, 60);
    grid3.jqGrid.CreateTable();
}
function OnSearch1() {
    grid.jqGrid.InitSearchParams();
    var objAgencyName = $("input[id*='txtAgencyName1']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("AccountName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}

function OnSearch() {
    grid2.jqGrid.InitSearchParams();
    var objAgencyName = $("input[id*='txtAccountName']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("AccountName");
        grid2.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid2.jqGrid.Search();
    return false;
}



function SelectAmountDetail(obj) {
    window.open("CardRevenueDetail.aspx?CustomerId=" + obj);
}

function EditPayOutHistory(messageObj) {
    window.open("CardPayOutHandle.aspx?PayOutHistoryId=" + $(messageObj).prev().val())
    return false;
}

function EditBcPayOutHistory(messageObj) {
    window.open("BusinessCardPayOutHandle.aspx?PayOutHistoryId=" + $(messageObj).prev().val());
    return false;
}