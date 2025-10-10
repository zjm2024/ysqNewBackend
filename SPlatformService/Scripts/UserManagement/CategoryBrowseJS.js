$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CategoryList";
    grid.jqGrid.PagerID = "CategoryListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CategoryList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CategoryCode";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CategoryId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditCategory(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("CategoryId");						
	grid.jqGrid.AddColumn("CategoryCode", "行业编号 ", true, null, 100);				
	grid.jqGrid.AddColumn("CategoryName", "行业名称", true, null, 100);				
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
	function (obj, options, rowObject) {	        
	    if (obj == 'True')
	        return "启用";
	    else
	        return "禁用";
	        
	}, false);	
    grid.jqGrid.CreateTable();   
}


function EditCategory(categoryObj) {
    window.location.href = "CategoryCreateEdit.aspx?CategoryId=" + $(categoryObj).prev().val();
    return false;
}

function NewCategory() {
    window.location.href = "CategoryCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objCategoryName = $("input[id*='txtCategoryName']");
    if (objCategoryName.val() != "")
        grid.jqGrid.AddSearchParams("CategoryName", "LIKE", objCategoryName.val());

    grid.jqGrid.Search();
    return false;
}

