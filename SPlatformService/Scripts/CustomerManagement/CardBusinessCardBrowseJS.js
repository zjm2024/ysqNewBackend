$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessCardList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("BusinessID", "操作", true, "center", 20,
        function (obj, options, rowObject) {
            return "<div onclick='Edit(" + obj + ")' style='cursor:pointer;'>编辑</div>";
        }, false);
    grid.jqGrid.AddColumn("LogoImg", "Logo", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="Logo"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("BusinessName", "公司名称", true, null, 80,
        function (obj, options, rowObject) {
            if (rowObject.HeadquartersID > 0)
            { return obj + "(<font style='color:#00a724;'>子公司</font>)"; }
            else
            { return obj; }
        }, false);
    grid.jqGrid.AddColumn("isGroup", "企业属性", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == 1)
            { return "<div style='color:red;'>集团企业</div>"; }
            else
            { return "普通企业"; }
        }, false);
    grid.jqGrid.AddColumn("OfficialProducts", "版本", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == "SelfEmployed")
            { return "微企版"; }
            else if (obj == "Basic")
            { return "基础版"; }
            else if (obj == "Standard")
            { return "标准版"; }
            else if (obj == "Advanced")
            { return "专业版"; }
            else if (obj == "Group")
            { return "集团版"; }
            else
            { return "未知"; }
        }, false);
    grid.jqGrid.AddColumn("JoinQR", "小程序码", true, "center", 20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.BusinessID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid.jqGrid.AddColumn("Industry", "行业", true, "center", 50);
    grid.jqGrid.AddColumn("PersonalCounts", "人数", true, "center", 30,
         function (obj, options, rowObject) {

             var result = '';
             if (obj != null && obj != "unknown" && obj != "")
                 result += obj + '/' + rowObject.Number;
             else
                 result += '-';

             result += "<span  onclick=\"showPersonal('" + rowObject.BusinessID + "')\"  style='cursor:pointer;margin-left:10px'>查看成员</span>";
             return result;
         }, false);
    grid.jqGrid.AddColumn("ExpirationAt", "到期时间", true, "center", 30);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    grid.jqGrid.CreateTable(); 
}

function SelectPartyPeople(obj) {
    window.location.href = "CardPartSignInNum.aspx?PartID=" + obj;
}

function showQRImg(obj) {
    window.open(obj);
}
function showPersonal(obj) {
    window.open("CardPersonalBrowse.aspx?BusinessID=" + obj);
}

function Edit(agencyObj) {
    window.location.href = "CardBusinessCardCreateEdit.aspx?BusinessID=" + agencyObj;
    return false;
}
function Add() {
    window.location.href = "CardBusinessCardCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    
    var objAgencyName = $("input[id*='txtAgencyName']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("BusinessName");
        filedArr.push("Industry");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}

