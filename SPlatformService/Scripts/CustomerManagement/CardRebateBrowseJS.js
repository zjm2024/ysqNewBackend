$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardRebateList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CustomerId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 60,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("OneRebateCost", "VIP一级奖励", true, "center", 30);
    grid.jqGrid.AddColumn("OneRebateCostNo", "VIP一级奖励（未领取）", true, "center", 30);
    grid.jqGrid.AddColumn("TwoRebateCost", "VIP二级奖励", true, "center", 30);
    grid.jqGrid.AddColumn("TwoRebateCostNo", "VIP二级奖励（未领取）", true, "center", 30);
    grid.jqGrid.AddColumn("PromotionAwardCost", "活动分佣", true, "center", 30);
    grid.jqGrid.AddColumn("PromotionAwardCostNo", "活动分佣（未领取）", true, "center", 30);
    grid.jqGrid.CreateTable();   
}
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}