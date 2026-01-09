$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ComplaintsList";
    grid.jqGrid.PagerID = "ComplaintsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ComplaintsList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ComplaintsId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditComplaints(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteComplaintsOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ComplaintsId");
				
	grid.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 50);				
	grid.jqGrid.AddColumn("CustomerName", "申请人", true, null, 50);				
	grid.jqGrid.AddColumn("CreatedAt", "申请时间", true, null, 50);				
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "已提交";
            else if (obj == '1')
                return "已跟进";
            else if (obj == '2')
                return "已处理";
        }, false);
    grid.jqGrid.CreateTable();   
}


function EditComplaints(complaintsObj) {
    window.location.href = "ComplaintsCreateEdit.aspx?ComplaintsId=" + $(complaintsObj).prev().val();
    return false;
}

function NewComplaints() {
    window.location.href = "ComplaintsCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objComplaintsName = $("input[id*='txtComplaintsName']");
    grid.jqGrid.AddSearchParams("ProjectCode", "LIKE", objComplaintsName.val());

    grid.jqGrid.Search();
    return false;
}

