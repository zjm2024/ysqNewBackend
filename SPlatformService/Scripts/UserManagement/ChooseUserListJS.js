$(function () {
    InitDropDown();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ChooseUserList";
    grid.jqGrid.PagerID = "ChooseUserListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=UserList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    //grid.jqGrid.Multiselect = true;

    grid.jqGrid.AddColumn("UserId", "操作", false, "center", 50, function (obj) { return '<input type=\'hidden\' value="' + obj + '"/><input id="test_' + obj + '" class="ace" type="checkbox" onclick="ChooseUser(this); return true;" /><label for="test_' + obj + '" class="lbl"> </label>'; });
    grid.jqGrid.AddHidColumn("UserId");
    grid.jqGrid.AddColumn("DepartmentName", "部门名称", true, null, 120);
    grid.jqGrid.AddColumn("UserName", "用户名", true, null, 150);
    grid.jqGrid.AddColumn("RoleName", "角色", true, null, 130);
    grid.jqGrid.AddColumn("UserTitleName", "职务", true, null, 130);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, null, 150);
    grid.jqGrid.CreateTable();
   
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();
    //department usertitle user name
    var objDepartmentId = $("select[id*='drpDepartment']");
    if (objDepartmentId.val() != "-1")
        grid.jqGrid.AddSearchParams("DepartmentId", "INTEQUAL", objDepartmentId.val());    

    var objUserName = $("input[id*='txtUserName']");
    if (objUserName.val() != "")
        grid.jqGrid.AddSearchParams("UserName", "LIKE", objUserName.val());

    grid.jqGrid.Search();
    return false;
}

function ChooseUser(userObj) {
    var seleUserObj = $("input[type='checkbox']:checked");
    var selectUserIdStr = "";
    var userArray = new Array();
    for (var i = 0; i < seleUserObj.length; i++) {
        var user = new Object();
        userArray.push(user);
        var chk = $(seleUserObj[i]);
        user.UserId = chk.parent().next()[0].innerText;
        user.DepartmentName = chk.parent().next().next()[0].innerText;
        user.UserName = chk.parent().next().next().next()[0].innerText;
        user.UserTitleName = chk.parent().next().next().next().next().next()[0].innerText;
        user.Phone = chk.parent().next().next().next().next().next().next()[0].innerText;
    }
    
    $("#hidUserId").val(JSON.stringify(userArray));
}

function InitDropDown() {
    $.ajax({
        url: _RootPath + "SPWebAPI/Department/GetDepartmentAll?token=" + _Token,
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var depList = data.Result;
                var objDepartment = $("select[id*='drpDepartment']");
                for (var i = 0; i < depList.length; i++) {
                    var depObj = depList[i];
                    objDepartment.append("<option value=\"" + depObj.DepartmentId + "\">" + depObj.DepartmentName + "</option>");
                }

            } else {

            }

        },
        error: function (data) {
            alert(data);
        }
    });
    
}