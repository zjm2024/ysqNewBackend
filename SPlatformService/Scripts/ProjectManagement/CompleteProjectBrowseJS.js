$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ProjectList";
    grid.jqGrid.PagerID = "ProjectListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CompleteProjectList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ProjectId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditProject(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ProjectId");
			
	grid.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 50);				
	grid.jqGrid.AddColumn("Title", "任务名称", true, null, 50);
	grid.jqGrid.AddColumn("BusinessName", "雇主", true, null, 50);
	grid.jqGrid.AddColumn("AgencyName", "销售", true, null, 50);
	grid.jqGrid.AddColumn("StartDate", "开始时间", true, null, 50);				
	grid.jqGrid.AddColumn("EndDate", "结束时间", true, null, 50);				
	grid.jqGrid.AddColumn("Commission", "酬金", true, null, 50);				
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "已生成";
            else if (obj == '1')
                return "工作中";
            else if (obj == '2')
                return "已完成";
            else if (obj == '3')
                return "申请完工";
            else if (obj == '4')
                return "申请退款";
            else if (obj == '5')
                return "退款完成";
        }, false);
    grid.jqGrid.CreateTable();   
}


function EditProject(projectObj) {
    window.location.href = "CompleteProjectCreateEdit.aspx?ProjectId=" + $(projectObj).prev().val();
    return false;
}


function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objProjectName = $("input[id*='txtProjectName']");
    grid.jqGrid.AddSearchParams("ProjectCode", "LIKE", objProjectName.val());

    grid.jqGrid.Search();
    return false;
}

