$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "SystemMessageList";
    grid.jqGrid.PagerID = "SystemMessageListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=SystemMessageList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "SendAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("SystemMessageId", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("SystemMessageId");						
	grid.jqGrid.AddColumn("MessageTypeName", "通告类型", true, null, 50);				
	grid.jqGrid.AddColumn("Title", "标题", true, null, 50);
	grid.jqGrid.AddColumn("SendAt", "发送时间", true, null, 50);
    grid.jqGrid.CreateTable();   
}


function EditSystemMessage(categoryObj) {
    window.location.href = "SystemMessageCreateEdit.aspx?SystemMessageId=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "SystemMessageCreateEdit.aspx";
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objSystemMessageName = $("input[id*='txtTitle']");
    if (objSystemMessageName.val() != "")
        grid.jqGrid.AddSearchParams("Title", "LIKE", objSystemMessageName.val());

    grid.jqGrid.Search();
    return false;
}

