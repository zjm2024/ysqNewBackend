
$(function () {
    Init();
    onCustomerInComeListTableInit();
    onCustomerHostingListTableInit();
    onCustomerRefundListTableInit();
    onCustomerPayOutListTableInit();
    onCustomerPayInListTableInit();
});

function Init() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetCustomerBalance?customerId=" + _CustomerId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {

                var balanceVO = data.Result;

                SetBalance(balanceVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });

}
function SetBalance(balanceVO) {
    var objBalance = $("input[id*='txtBalance']");
    if (parseFloat(balanceVO.Balance) > 0)
        objBalance.val(balanceVO.Balance);
    else
        objBalance.val(0);

}

function Recharge() {
    window.location.href=("../../CustomerManagement/CustomerPayOutCreateEidt.aspx");
}
function Recharge2() {
    window.location.href = ("../../Pay/CustomerRechange.aspx");
}
//收入
function onCustomerInComeListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerInComeList";
    grid.jqGrid.PagerID = "CustomerInComeListListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerInComeList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "commissionInComeId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("Commission", "收入金额", true, null, 80);
    grid.jqGrid.AddColumn("PayDate", "到账日期", true, null, 120);
    grid.jqGrid.AddColumn("projectName", "项目", true, null, 100);
    grid.jqGrid.AddColumn("RequirementName", "任务", true, null, 100);
    grid.jqGrid.AddColumn("Purpose", "用途", true, null, 100);
    grid.jqGrid.CreateTable();

}


//提现
function onCustomerPayOutListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerPayOutList";
    grid.jqGrid.PagerID = "CustomerPayOutListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerPayOutList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "PayOutHistoryId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("Cost", "提现金额", true, null, 130);
    grid.jqGrid.AddColumn("PayOutDate", "提现日期", true, null, 120);
    grid.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 50,
function (obj, options, rowObject) {
    if (obj == '0')
        return "申请提现";
    else if (obj == '-1')
        return "未提交申请";
    else if (obj == '-2')
        return "提现失败";
    else if (obj == '1')
        return "提现成功";
}, false);
    grid.jqGrid.AddColumn("HandleComment", "备注", true, null, 200);

    grid.jqGrid.CreateTable();

}

//充值
function onCustomerPayInListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerPayInList";
    grid.jqGrid.PagerID = "CustomerPayInListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerPayInList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "PayInHistoryId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("Cost", "充值金额", true, null, 150);
    grid.jqGrid.AddColumn("PayInDate", "充值日期", true, null, 150);
    grid.jqGrid.AddColumn("Purpose", "用途", true, null, 200);
    grid.jqGrid.CreateTable();

}



//退款
function onCustomerRefundListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerRefundList";
    grid.jqGrid.PagerID = "CustomerRefundListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerRefundList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "OrderId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("PriceTotal", "退款金额", true, null, 80);
    grid.jqGrid.AddColumn("BookedCount", "到账日期", true, null, 100);
    grid.jqGrid.AddColumn("projectName", "项目", true, null, 100);
    grid.jqGrid.AddColumn("RequirementName", "任务", true, null, 100);
    grid.jqGrid.AddColumn("Purpose", "用途", true, null, 120);
    grid.jqGrid.CreateTable();

}
//托管
function onCustomerHostingListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerHostingList";
    grid.jqGrid.PagerID = "CustomerHostingListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerHostingList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "CommissionDelegationId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.AddColumn("PayDate", "托管时间", true, null, 100);
    grid.jqGrid.AddColumn("Commission", "托管金额", true, null, 80);
    grid.jqGrid.AddColumn("projectName", "项目", true, null, 100);
    grid.jqGrid.AddColumn("RequirementName", "任务", true, null, 100);
    grid.jqGrid.AddColumn("Purpose", "用途", true, null, 120);
    grid.jqGrid.CreateTable();

}