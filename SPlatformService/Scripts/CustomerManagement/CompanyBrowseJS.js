$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CompanyList";
    grid.jqGrid.PagerID = "CompanyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CompanyList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CompanyID";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CompanyID");
    grid.jqGrid.AddColumn("CompanyName", "公司名称", true, null, 100);
    grid.jqGrid.AddColumn("CompanyType", "企业类型", true, "center", 50);
    grid.jqGrid.AddColumn("Contacts", "联系人", true, "center", 50);
    grid.jqGrid.AddColumn("Tel", "联系电话", true, "center", 50);
    grid.jqGrid.AddColumn("Location", "所在地", true, "center", 50);
    grid.jqGrid.AddColumn("Address", "办公地址", true, "center", 100);
    grid.jqGrid.AddColumn("CompanySize", "企业规模", true, "center", 20);
    grid.jqGrid.AddColumn("RegisteredCapital", "注册资本", true, "center", 20);
    grid.jqGrid.AddColumn("YearOfRegistration", "注册年份", true, "center", 20);
    
    grid.jqGrid.CreateTable();   
}
