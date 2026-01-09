$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgentApplyList";
    grid.jqGrid.PagerID = "AgentApplyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgentApplyList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CustomerId");
    grid.jqGrid.AddColumn("CustomerId", "申请ID", true, "center", 50);
    grid.jqGrid.AddColumn("CreatedAt", "提交时间", true, "center", 50);
    grid.jqGrid.AddColumn("Name", "名称", true, "center", 50);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, "center", 50);
    grid.jqGrid.AddColumn("City", "代理区域", true, "center", 100);
    grid.jqGrid.CreateTable();   
}
