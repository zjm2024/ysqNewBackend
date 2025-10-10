$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "DemandList";
    grid.jqGrid.PagerID = "DemandListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=DemandList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("DemandId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditRequirement(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteRequirementOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("DemandId");
			
    grid.jqGrid.AddColumn("Description", "需求详情", true, null, 100);
    grid.jqGrid.AddColumn("OfferCount", "收到留言", true, "center", 50,
   function (obj, options, rowObject) {
       if (obj != "0")
           return "<a class='getOfferlink' href='DemandCreateEdit.aspx?DemandId=" + rowObject.DemandId + "&dropdown14=1'>查看<font>(" + obj + ")</font>个留言</a>";
       else
           return obj;
    }, false);
    grid.jqGrid.AddColumn("CategoryName", "需求类别", true, null, 80);
	grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 100);							
	grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "保存";
            else if (obj == '1')
                return "发布";
            else if (obj == '2')
                return "审核中";
            else
                return "";
        }, false);
	grid.jqGrid.AddColumn("CreatedAt", "创建日期", true, null, 100);				
    grid.jqGrid.CreateTable();   
}


function EditRequirement(requirementObj) {
    window.location.href = "DemandCreateEdit.aspx?DemandId=" + $(requirementObj).prev().val();
    return false;
}

function NewRequirement() {
    window.location.href = "DemandCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objRequirementName = $("input[id*='txtRequirementName']");
    grid.jqGrid.AddSearchParams("Description", "LIKE", objRequirementName.val());

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    grid.jqGrid.Search();
    return false;
}

