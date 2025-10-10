$(function () {
    onPageInit();


});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessCardPosterList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("Url", "Url", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="Logo"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("Name", "文件名称", true, "center", 80);
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

