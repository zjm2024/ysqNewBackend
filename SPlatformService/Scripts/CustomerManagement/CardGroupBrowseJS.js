$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardGroupList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("GroupID", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectGroupNum(" + obj + ")' style='cursor:pointer;'>查看组员</div>";
        }, false);
    grid.jqGrid.AddColumn("GroupName", "名片组名称", true, null,120);
    grid.jqGrid.AddColumn("counts", "人数", true, "center", 30);
    grid.jqGrid.AddColumn("JoinSetUp", "入组设置", true, "center", 10,
        function (obj, options, rowObject) {
        if (obj == '0')
            return "公开";
        else
            return "需审核";
    }, false);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    grid.jqGrid.CreateTable();   
}



function SelectGroupNum(obj) {
    window.location.href = "GroupJoinPeople.aspx?GroupID=" + obj;
}

function EditAgency(agencyObj) {
    window.location.href = "AgencyCreateEdit.aspx?AgencyId=" + $(agencyObj).prev().val();
    return false;
}



function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objAgencyName = $("input[id*='txtAgencyName']");    

    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("GroupName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    var objRealNameStatus = $("select[id*='drpRealNameStatus']");
    if (objRealNameStatus.val() > -1)
        grid.jqGrid.AddSearchParams("RealNameStatus", "=", objRealNameStatus.val());

    grid.jqGrid.Search();
    return false;
}

