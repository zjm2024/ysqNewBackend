$(function () {
    InitDropDown();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ChooseCategoryList";
    grid.jqGrid.PagerID = "ChooseCategoryListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CategoryChooseList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CategoryCode";
    grid.jqGrid.DefaultSort = "desc";
    //grid.jqGrid.Multiselect = true;

    grid.jqGrid.AddColumn("CategoryId", "操作", false, "center", 50, function (obj) { return '<input type=\'hidden\' value="' + obj + '"/><input id="test_' + obj + '" class="ace" type="checkbox" onclick="ChooseCategory(this); return true;" /><label for="test_' + obj + '" class="lbl"> </label>'; });
    grid.jqGrid.AddHidColumn("CategoryId");
    grid.jqGrid.AddColumn("ParentCategoryName", "行业大类", true, null, 120);
    grid.jqGrid.AddColumn("CategoryCode", "行业编号", true, null, 150);
    grid.jqGrid.AddColumn("CategoryName", "行业名称", true, null, 130);
    grid.jqGrid.CreateTable();
   
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();
    //department usertitle user name
    var objDepartmentId = $("select[id*='drpParentCategory']");
    if (objDepartmentId.val() != "-1")
        grid.jqGrid.AddSearchParams("ParentCategoryId", "INTEQUAL", objDepartmentId.val());    

    var objUserName = $("input[id*='txtCategoryName']");
    if (objUserName.val() != "")
        grid.jqGrid.AddSearchParams("CategoryName", "LIKE", objUserName.val());

    grid.jqGrid.Search();
    return false;
}

function ChooseCategory(categoryObj) {
    var seleCategoryObj = $("input[type='checkbox']:checked");
    var selectCategoryIdStr = "";
    var categoryArray = new Array();
    for (var i = 0; i < seleCategoryObj.length; i++) {
        var category = new Object();
        categoryArray.push(category);
        var chk = $(seleCategoryObj[i]);
        category.CategoryId = chk.parent().next()[0].innerText;
        category.ParentCategoryName = chk.parent().next().next()[0].innerText;
        category.CategoryCode = chk.parent().next().next().next()[0].innerText;
        category.CategoryName = chk.parent().next().next().next().next()[0].innerText;
    }
    
    $("#hidCategoryId").val(JSON.stringify(categoryArray));
}

function InitDropDown() {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetParentCategoryList?enable=true",
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var depList = data.Result;
                var objParentCategory = $("select[id*='drpParentCategory']");
                for (var i = 0; i < depList.length; i++) {
                    var depObj = depList[i];
                    objParentCategory.append("<option value=\"" + depObj.CategoryId + "\">" + depObj.CategoryName + "</option>");
                }

            } else {

            }

        },
        error: function (data) {
            alert(data);
        }
    });
    
}