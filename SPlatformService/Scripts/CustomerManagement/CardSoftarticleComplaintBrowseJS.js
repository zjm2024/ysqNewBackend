$(function () {
    onPageInit();
});
var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ComplaintList";
    grid.jqGrid.PagerID = "ComplaintListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ComplaintList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("SoftArticleID");
    grid.jqGrid.AddColumn("Title", "标题", true, null, 30);
    grid.jqGrid.AddColumn("Description", "内容", true, null, 30);
    grid.jqGrid.AddColumn("CreatedAt", "投诉时间", true, "center", 50);
    grid.jqGrid.AddColumn("SoftArticleID", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='show(" + obj + ")' style='cursor:pointer;'>查看文章</div>";
        }, false);
    grid.jqGrid.CreateTable();
}
function show(obj) {
    window.location.href = "CardSoftarticleShow.aspx?SoftArticleID=" + obj;
}