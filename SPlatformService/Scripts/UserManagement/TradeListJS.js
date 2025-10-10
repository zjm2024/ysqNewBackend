$(function () {
    onPageInit();
});
var gridCommission = new JGrid();
var gridPayin = new JGrid();
var gridPayOut = new JGrid();
function onPageInit() {
    gridCommission.jqGrid.ID = "PlatformCommissionList";
    gridCommission.jqGrid.PagerID = "PlatformCommissionListDiv";
    gridCommission.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridCommission.jqGrid.Params = "table=PlatformCommissionList";
    gridCommission.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridCommission.jqGrid.DefaultSortCol = "commissionId";
    gridCommission.jqGrid.DefaultSort = "desc";
    gridCommission.jqGrid.Multiselect = true;
    gridCommission.jqGrid.AddHidColumn("commissionId");
    gridCommission.jqGrid.AddColumn("projectName", "项目", true, null, 150);
    gridCommission.jqGrid.AddColumn("RequirementName", "任务", true, null, 150);
    gridCommission.jqGrid.AddColumn("ProjectCommission", "抽佣金额", true, null, 150);
    gridCommission.jqGrid.AddColumn("CommissionDate", "收入时间", true, null, 150);
    gridCommission.jqGrid.AddColumn("CommissionPercentage", "抽佣比例", true, null, 150);
    gridCommission.jqGrid.CreateTable();


  
    gridPayin.jqGrid.ID = "CustomerPayInViewList";
    gridPayin.jqGrid.PagerID = "CustomerPayInViewListDiv";
    gridPayin.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridPayin.jqGrid.Params = "table=CustomerPayInViewList";
    gridPayin.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridPayin.jqGrid.DefaultSortCol = "PayInHistoryId";
    gridPayin.jqGrid.DefaultSort = "desc";
    gridPayin.jqGrid.AddColumn("Cost", "充值金额", true, null, 150);
    gridPayin.jqGrid.AddColumn("PayInDate", "充值日期", true, null, 150);
    gridPayin.jqGrid.AddColumn("CustomerName", "用户名称", true, null, 150);
    gridPayin.jqGrid.CreateTable();

  
    gridPayOut.jqGrid.ID = "CustomerPayOutViewList";
    gridPayOut.jqGrid.PagerID = "CustomerPayOutViewListDiv";
    gridPayOut.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridPayOut.jqGrid.Params = "table=CustomerPayOutViewList";
    gridPayOut.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridPayOut.jqGrid.DefaultSortCol = "PayOutHistoryId";
    gridPayOut.jqGrid.DefaultSort = "desc";
    gridPayOut.jqGrid.AddColumn("Cost", "提现金额", true, null, 150);
    gridPayOut.jqGrid.AddColumn("PayOutDate", "提现日期", true, null, 150);
    gridPayOut.jqGrid.AddColumn("CustomerName", "用户名称", true, null, 150);

    gridPayOut.jqGrid.CreateTable();
}





