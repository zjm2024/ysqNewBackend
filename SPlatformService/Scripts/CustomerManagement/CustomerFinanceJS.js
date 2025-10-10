$(function () {
    onPageInit();
});
var gridCommission = new JGrid();
var gridPayin = new JGrid();
var gridPayOut = new JGrid();
var gridComeList = new JGrid();
var gridHosting = new JGrid();
var gridRequireCommission = new JGrid();

function onPageInit() {
    var customerId = parseInt($("#" + hidCustomerId).val());

    gridPayin.jqGrid.ID = "CustomerPayInViewList";
    gridPayin.jqGrid.PagerID = "CustomerPayInViewListDiv";
    gridPayin.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridPayin.jqGrid.Params = "table=CustomerPayInViewList&CustomerId=" + customerId;
    gridPayin.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridPayin.jqGrid.DefaultSortCol = "PayInHistoryId";
    gridPayin.jqGrid.DefaultSort = "desc";
    gridPayin.jqGrid.AddColumn("Cost", "充值金额", true, null, 150);
    gridPayin.jqGrid.AddColumn("PayInDate", "充值日期", true, null, 150);
    gridPayin.jqGrid.AddColumn("Purpose", "充值渠道", true, null, 150);
    gridPayin.jqGrid.CreateTable();

    gridPayOut.jqGrid.ID = "CustomerPayOutViewList";
    gridPayOut.jqGrid.PagerID = "CustomerPayOutViewListDiv";
    gridPayOut.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridPayOut.jqGrid.Params = "table=CustomerPayOutList&CustomerId=" + customerId;
    gridPayOut.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridPayOut.jqGrid.DefaultSortCol = "PayOutHistoryId";
    gridPayOut.jqGrid.DefaultSort = "desc";
    gridPayOut.jqGrid.AddColumn("Cost", "提现金额", true, null, 150);
    gridPayOut.jqGrid.AddColumn("PayOutDate", "提现日期", true, null, 150);
    gridPayOut.jqGrid.AddColumn("ThirdOrder", "转账订单号", true, null, 150);
    gridPayOut.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 50,
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
    gridPayOut.jqGrid.CreateTable();

    
    gridComeList.jqGrid.ID = "CustomerInComeList";
    gridComeList.jqGrid.PagerID = "CustomerInComeListListDiv";
    gridComeList.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridComeList.jqGrid.Params = "table=CustomerInComeList&CustomerId=" + customerId;
    gridComeList.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridComeList.jqGrid.DefaultSortCol = "commissionInComeId";
    gridComeList.jqGrid.DefaultSort = "desc";
    gridComeList.jqGrid.AddColumn("Commission", "收入金额", true, null, 80);
    gridComeList.jqGrid.AddColumn("PayDate", "到账日期", true, null, 120);
    gridComeList.jqGrid.AddColumn("projectName", "项目", true, null, 100,
    function (obj, options, rowObject) {
        return "<a href='../ProjectManagement/ProjectCreateEdit.aspx?ProjectId=" + rowObject.ProjectId + "' target='_blank'>" + obj + "</a>";
    }, false);
    gridComeList.jqGrid.AddColumn("RequirementName", "任务", true, null, 100);
    gridComeList.jqGrid.AddColumn("Purpose", "用途", true, null, 100);
    gridComeList.jqGrid.CreateTable();

    
    gridHosting.jqGrid.ID = "CustomerHostingList";
    gridHosting.jqGrid.PagerID = "CustomerHostingListDiv";
    gridHosting.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridHosting.jqGrid.Params = "table=CustomerHostingList&CustomerId=" + customerId;
    gridHosting.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridHosting.jqGrid.DefaultSortCol = "CommissionDelegationId";
    gridHosting.jqGrid.DefaultSort = "desc";
    gridHosting.jqGrid.AddColumn("PayDate", "托管时间", true, null, 100);
    gridHosting.jqGrid.AddColumn("Commission", "托管金额", true, null, 80);
    gridHosting.jqGrid.AddColumn("projectName", "项目", true, null, 100,
    function (obj, options, rowObject) {
        return "<a href='../ProjectManagement/ProjectCreateEdit.aspx?ProjectId=" + rowObject.ProjectId + "' target='_blank'>" + obj + "</a>";
    }, false);
    gridHosting.jqGrid.AddColumn("RequirementName", "任务", true, null, 100);
    gridHosting.jqGrid.AddColumn("Purpose", "用途", true, null, 120);
    gridHosting.jqGrid.CreateTable();

    gridRequireCommission.jqGrid.ID = "RequireCommissionList";
    gridRequireCommission.jqGrid.PagerID = "RequireCommissionListDiv";
    gridRequireCommission.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridRequireCommission.jqGrid.Params = "table=RequireCommissionList&CustomerId=" + customerId;
    gridRequireCommission.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridRequireCommission.jqGrid.DefaultSortCol = "RequireCommissionDelegationId";
    gridRequireCommission.jqGrid.DefaultSort = "desc";
    gridRequireCommission.jqGrid.AddColumn("DelegationDate", "托管时间", true, null, 100);
    gridRequireCommission.jqGrid.AddColumn("Commission", "托管金额", true, null, 80);
    gridRequireCommission.jqGrid.AddColumn("Title", "任务", true, null, 100,
    function (obj, options, rowObject) {
        return "<a href='../RequireManagement/RequirementCreateEdit.aspx?RequirementId=" + rowObject.RequirementId + "' target='_blank'>" + obj + "</a>";
    }, false);
    gridRequireCommission.jqGrid.AddColumn("RequirementCode", "任务编码", true, null, 120);
    gridRequireCommission.jqGrid.CreateTable();
}