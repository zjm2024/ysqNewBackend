$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "MiniprogramsList";
    grid.jqGrid.PagerID = "MiniprogramsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MiniprogramsList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    
    grid.jqGrid.AddHidColumn("AppType");
    grid.jqGrid.AddColumn("AppType", "AppType", true, "center", 20);
    grid.jqGrid.AddColumn("AppName", "小程序名称", true, null, 50);
    grid.jqGrid.AddColumn("AppId", "AppId", true, "center", 20);
    grid.jqGrid.AddColumn("MCHID", "MCHID", true, "center", 20);
    grid.jqGrid.AddColumn("templateID", "templateID", true, "center", 20);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    /*
    grid.jqGrid.AddColumn("Status", "状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "<div style='color:#058c9a;'>已发布</div>";
            else if (obj == '2')
                return "<div style='color:#bd3718;'>审核中</div>";
            else if (obj == '0')
                return "<div>未发布</div>";
        }, false);
        */
    grid.jqGrid.AddColumn("AppType", "编辑", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.CreateTable();   
}
function EditSystemMessage(categoryObj) {
    window.location.href = "MiniprogramsCreateEdit.aspx?AppType=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "MiniprogramsCreateEdit.aspx";
    return false;
}