$(function () {
    InitDropDown();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ChooseCityList";
    grid.jqGrid.PagerID = "ChooseCityListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CityList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CityName";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;

    //grid.jqGrid.AddColumn("CityId", "操作", false, "center", 50, function (obj) { return '<input type=\'hidden\' value="' + obj + '"/><input id="test_' + obj + '" class="ace" type="checkbox" onclick="ChooseCity(this); return true;" /><label for="test_' + obj + '" class="lbl"> </label>'; });
    grid.jqGrid.AddHidColumn("CityId");
    grid.jqGrid.AddColumn("ProvinceName", "省（直辖市）", true, null, 120);
    grid.jqGrid.AddColumn("CityCode", "城市编号", true, null, 1);
    grid.jqGrid.AddColumn("CityName", "城市名称", true, null, 130);
    grid.jqGrid.CreateTable();
   
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();
    //department usertitle user name
    var objDepartmentId = $("select[id*='drpProvince']");
    if (objDepartmentId.val() != "-1")
        grid.jqGrid.AddSearchParams("ProvinceId", "INTEQUAL", objDepartmentId.val());    

    var objUserName = $("input[id*='txtCityName']");
    if (objUserName.val() != "")
        grid.jqGrid.AddSearchParams("CityName", "LIKE", objUserName.val());

    grid.jqGrid.Search();
    return false;
}

function ChooseCity(userObj) {
    var seleCityObj = $("input[type='checkbox']:checked");
    var selectCityIdStr = "";
    var cityArray = new Array();
    for (var i = 0; i < seleCityObj.length; i++) {
        var city = new Object();
        cityArray.push(city);
        var chk = $(seleCityObj[i]);
        city.CityId = chk.parent().next()[0].innerText;
        city.ProvinceName = chk.parent().next().next()[0].innerText;
        city.CityCode = chk.parent().next().next().next()[0].innerText;
        city.CityName = chk.parent().next().next().next().next()[0].innerText;
    }
    
    $("#hidCityId").val(JSON.stringify(cityArray));
}

function InitDropDown() {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var depList = data.Result;
                var objProvince = $("select[id*='drpProvince']");
                for (var i = 0; i < depList.length; i++) {
                    var depObj = depList[i];
                    objProvince.append("<option value=\"" + depObj.ProvinceId + "\">" + depObj.ProvinceName + "</option>");
                }

            } else {

            }

        },
        error: function (data) {
            alert(data);
        }
    });
    
}