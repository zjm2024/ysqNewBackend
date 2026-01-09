$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AchievemenList";
    grid.jqGrid.PagerID = "AchievemenListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AchievemenList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "MONTH";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("originCustomerId", "操作", false, "center", 50,
       function (obj, options, rowObject) {
           var result = '';
           result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="查看" onclick="return EditAgency(' + obj + ',\'' + rowObject.MONTH + '\');"></img>';
           return result;
       }, false);
    grid.jqGrid.AddColumn("MONTH", "月份", true, null, 30);
    grid.jqGrid.AddColumn("count", "获取用户数", true, "center", 30);
    grid.jqGrid.AddColumn("CustomerAccount", "账户", true, "center", 30);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.originCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("Name", "名片名称", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.originCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("Phone", "名片手机", true, "center", 30);
    grid.jqGrid.AddColumn("Position", "职位", true, "center", 30);
    grid.jqGrid.AddColumn("CorporateName", "公司名", true, "center", 30);
    grid.jqGrid.AddColumn("Tel", "固话", true, "center", 30);
    grid.jqGrid.AddColumn("Email", "邮箱", true, "center", 30);
    grid.jqGrid.AddColumn("WeChat", "微信", true, "center", 30);

    grid.jqGrid.CreateTable();   
}

function EditAgency(originCustomerId, MONTH) {
    window.location.href = "CardAchievemenFinance.aspx?originCustomerId=" + originCustomerId + "&MONTH=" + MONTH;
    return false;
}
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}



function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objAgencyName = $("input[id*='txtAgencyName']");    

    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("CustomerName");
        filedArr.push("CustomerAccount");
        filedArr.push("Phone");
        filedArr.push("Name");
        filedArr.push("CorporateName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }

    grid.jqGrid.Search();
    return false;
}

