$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CustomerList";
    grid.jqGrid.PagerID = "CustomerListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ViolationList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "ViolationAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("ViolationID");
    grid.jqGrid.AddColumn("ViolationID", "记录ID", true, "center", 50);
    grid.jqGrid.AddColumn("ViolationAt", "提交时间", true, "center", 50);
    grid.jqGrid.AddColumn("ViolationText", "违法违规内容", true, "center", 500);
    grid.jqGrid.CreateTable();   
}
