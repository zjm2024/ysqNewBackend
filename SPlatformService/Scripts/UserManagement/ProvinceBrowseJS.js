$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ProvinceList";
    grid.jqGrid.PagerID = "ProvinceListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=ProvinceList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "ProvinceCode";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ProvinceId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditProvince(this);"></img>';
           
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ProvinceId");			
	grid.jqGrid.AddColumn("ProvinceCode", "省（直辖市）编号", true, null, 100);				
	grid.jqGrid.AddColumn("ProvinceName", "省（直辖市）名称", true, null, 100);
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
	    function (obj, options, rowObject) {	        
	        if (obj == 'True')
	            return "启用";
	        else
	            return "禁用";
	        
	    }, false);				
    grid.jqGrid.CreateTable();   
}


function EditProvince(provinceObj) {
    window.location.href = "ProvinceCreateEdit.aspx?ProvinceId=" + $(provinceObj).prev().val();
    return false;
}

function NewProvince() {
    window.location.href = "ProvinceCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objProvinceName = $("input[id*='txtProvinceName']");
    if (objProvinceName.val() != "")
        grid.jqGrid.AddSearchParams("ProvinceName", "LIKE", objProvinceName.val());

    grid.jqGrid.Search();
    return false;
}

