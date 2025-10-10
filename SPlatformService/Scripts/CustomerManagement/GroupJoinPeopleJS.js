$(function () {
    onPageInit();
});


var grid = new JGrid();
function onPageInit() {

    var GroupID = $("input[id*='GroupID']").val();
    grid.jqGrid.ID = "GroupJoinList";
    grid.jqGrid.PagerID = "GroupJoinListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=GroupJoinList&GroupID=" + GroupID;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "GroupCardID";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;


    grid.jqGrid.AddHidColumn("GroupCardID");	
    grid.jqGrid.AddColumn("Name", "名字", true, "center", 50);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, "center", 50);
    grid.jqGrid.AddColumn("CorporateName", "公司", true, "center", 50);
    grid.jqGrid.AddColumn("Position", "职位", true, "center", 50);
    grid.jqGrid.CreateTable();  

}



