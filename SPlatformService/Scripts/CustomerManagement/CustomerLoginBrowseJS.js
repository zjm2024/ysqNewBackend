$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CustomerList";
    grid.jqGrid.PagerID = "CustomerListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerLoginList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "LoginAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CustomerId");
    grid.jqGrid.AddColumn("CustomerId", "会员ID", true, "center", 50);
    grid.jqGrid.AddColumn("LoginAt", "登录时间", true, "center", 50);
    grid.jqGrid.AddColumn("LoginIP", "登录IP", true, "center", 50);
    grid.jqGrid.AddColumn("LoginOS", "登录系统", true, "center", 50);
    grid.jqGrid.AddColumn("LoginBrowser", "浏览器", true, "center", 50);
    grid.jqGrid.AddColumn("Status", "登录状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj) {
                return "成功";
            } else{
                return "失败";
            }
        }, false);
    grid.jqGrid.CreateTable();   
}
