$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ChooseAgencyList";
    grid.jqGrid.PagerID = "ChooseAgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyList&provinceId=" + provinceId + "&cityId=" + cityId + "&parentCategoryId=" + parentCategoryId + "&categoryId=" + categoryId + "";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = false;

    grid.jqGrid.AddColumn("AgencyId", "操作", false, "center", 50, function (obj) { return '<input type=\'hidden\' value="' + obj + '"/><input id="test_' + obj + '" class="ace" type="checkbox" onclick="ChooseAgency(this); return true;" /><label for="test_' + obj + '" class="lbl"> </label>'; });
    grid.jqGrid.AddHidColumn("AgencyId");
    grid.jqGrid.AddColumn("CustomerCode", "编号", true, null, 50);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);
    grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 50);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
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
    grid.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 50);
    grid.jqGrid.CreateTable();
   
}

function ChooseAgency(agencyObj) {
    var seleAgencyObj = $("input[type='checkbox']:checked");
    var selectAgencyIdStr = "";
    var agencyArray = new Array();
    for (var i = 0; i < seleAgencyObj.length; i++) {
        var agency = new Object();
        agencyArray.push(agency);
        var chk = $(seleAgencyObj[i]);
        agency.AgencyId = chk.parent().next()[0].innerText;
        agency.CustomerCode = chk.parent().next().next()[0].innerText;
        agency.CustomerName = chk.parent().next().next().next()[0].innerText;
        agency.AgencyName = chk.parent().next().next().next().next().next().next()[0].innerText;
    }
    
    $("#hidAgencyId").val(JSON.stringify(agencyArray));
}