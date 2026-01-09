$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var QuestionnaireID = $("input[id*='QuestionnaireID']").val();

    grid.jqGrid.ID = "QuestionnaireSignupList";
    grid.jqGrid.PagerID = "QuestionnaireSignupListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=QuestionnaireSignupList&QuestionnaireID=" + QuestionnaireID;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("QuestionnaireSignupID");
    grid.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {
            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("Name", "名称", true, "center", 25);
    grid.jqGrid.AddColumn("Phone", "手机号", true, null, 30);
    
    grid.jqGrid.AddColumn("CreatedAt", "报名时间", true, "center", 30);
    grid.jqGrid.AddColumn("SigupForm", "填写内容", true, null, 100);
    grid.jqGrid.CreateTable();
}