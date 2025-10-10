$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var SoftArticleID = $("input[id*='SoftArticleID']").val();

    grid.jqGrid.ID = "QuestionnaireList";
    grid.jqGrid.PagerID = "QuestionnaireListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=QuestionnaireList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("QuestionnaireID");
    grid.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("Name", "名称", true, "center", 25,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("Phone", "发起人手机", true, "center", 25);
    grid.jqGrid.AddColumn("CardQRImg", "发起人名片", true, "center", 50,
      function (obj, options, rowObject) {
          return "<div id=‘QRImg" + rowObject.CardID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
      }, false);
    grid.jqGrid.AddColumn("Title", "标题", true, null, 100);
    grid.jqGrid.AddColumn("QRImg", "小程序码", true, "center", 50,
       function (obj, options, rowObject) {
           return "<div id=‘QRImg" + rowObject.QuestionnaireID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
       }, false);
    grid.jqGrid.AddColumn("counts", "报名数量", true, "center", 30,
       function (obj, options, rowObject) {
           if (obj > 0) {
               return obj + "<div  onclick=\"show('" + rowObject.QuestionnaireID + "')\"  style='color:red;cursor:pointer;'  >查看报名</div>";
           } else {
               return obj;
           }
       }, false);
    grid.jqGrid.AddColumn("CreatedAt", "发布时间", true, "center", 50);
    grid.jqGrid.AddColumn("QuestionnaireID", "表格链接", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"CopyUrl('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看链接</div>";
        }, false);
    grid.jqGrid.CreateTable();
}
function showQRImg(obj) {
    window.open(obj);
}
function show(obj) {
    window.location.href = "CardQuestionnaireSignupBrowse.aspx?QuestionnaireID=" + obj;
}
function CopyUrl(obj) {
    bootbox.dialog({
        message: "pages/index/SignInFormByUser/SignInFormByUser?QuestionnaireID=" + obj,
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                }
            }
        }
    });
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();
    var objAgencyName = $("input[id*='txtAgencyName']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("Title");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}
