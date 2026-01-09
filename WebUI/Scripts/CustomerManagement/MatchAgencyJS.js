$(function () {
    onPageInit();
    $("select[id*='drpRequire']").change(function () {
        OnSearch();
    });
});

var grid = new JGrid();
function onPageInit() {
    var objRequire = $("select[id*='drpRequire']");
    //var categoryStr = "";
    //if (objRequire.val() > -1)
    //    categoryStr = "," + objRequire.val() + ",";
    //else
    //    categoryStr = ",0,";
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MatchAgencyList&RequireId=" + objRequire.val();
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="查看" onclick="return EditAgency(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyId");

    grid.jqGrid.AddColumn("CustomerCode", "编号", true, null, 80);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 80);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 100);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 70,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if (obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
            else
                return "认证驳回";
        }, false);
    grid.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 160);    

    grid.jqGrid.CreateTable();
}



function EditAgency(agencyObj) {
    window.location.href = "../Agency.aspx?agencyId=" + $(agencyObj).prev().val();
    return false;
}


function OnSearch() {
    grid.jqGrid.InitSearchParams();
        
    var objRequire = $("select[id*='drpRequire']");
    grid.jqGrid.AddSearchParams("RequireId", "ieq", objRequire.val());

    grid.jqGrid.Search();
    return false;
}

