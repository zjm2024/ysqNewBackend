$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "ChooseRequireList";
    grid.jqGrid.PagerID = "ChooseRequireListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CategoryRequireList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "PublishAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = false;

    grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 50, function (obj) { return '<input type=\'hidden\' value="' + obj + '"/><input id="test_' + obj + '" class="ace" type="checkbox" onclick="ChooseRequire(this); return true;" /><label for="test_' + obj + '" class="lbl"> </label>'; });
    grid.jqGrid.AddHidColumn("RequirementId");
    grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 120);
    grid.jqGrid.AddColumn("Title", "任务标题", true, null, 150);
    grid.jqGrid.AddColumn("PublishAt", "发布日期", true, null, 130);
    grid.jqGrid.CreateTable();
   
}

function ChooseRequire(requireObj) {
    var seleRequireObj = $("input[type='checkbox']:checked");
    var selectRequireIdStr = "";
    var requireArray = new Array();
    for (var i = 0; i < seleRequireObj.length; i++) {
        var require = new Object();
        requireArray.push(require);
        var chk = $(seleRequireObj[i]);
        require.RequirementId = chk.parent().next()[0].innerText;
        require.RequirementCode = chk.parent().next().next()[0].innerText;
        require.Title = chk.parent().next().next().next()[0].innerText;        
    }
    
    $("#hidRequireId").val(JSON.stringify(requireArray));
}