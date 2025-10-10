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
    var MONTH = $("#" + HidMONTH).val();

    console.log(MONTH);

    gridPayin.jqGrid.ID = "CustomerPayInViewList";
    gridPayin.jqGrid.PagerID = "CustomerPayInViewListDiv";
    gridPayin.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridPayin.jqGrid.Params = "table=CustomerCardList&originCustomerId=" + customerId + "&MONTH=" + MONTH;
    gridPayin.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    gridPayin.jqGrid.DefaultSortCol = "CreatedAt";
    gridPayin.jqGrid.DefaultSort = "desc";
    gridPayin.jqGrid.AddColumn("CreatedAt", "创建日期", true, null, 150);
    gridPayin.jqGrid.AddColumn("CustomerAccount", "会员账户", true, null, 150);
    gridPayin.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 150);
    gridPayin.jqGrid.AddColumn("Phone", "手机号", true, null, 150);
    gridPayin.jqGrid.AddColumn("Name", "名片名称", true, null, 150);
    gridPayin.jqGrid.AddColumn("CardPhone", "名片手机", true, null, 150);
    gridPayin.jqGrid.AddColumn("Position", "职位", true, null, 150);
    gridPayin.jqGrid.AddColumn("CorporateName", "公司名", true, null, 150);
    
    gridPayin.jqGrid.CreateTable();
}