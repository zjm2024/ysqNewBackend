$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var SoftArticleID = $("input[id*='SoftArticleID']").val();

    grid.jqGrid.ID = "SoftarticleList";
    grid.jqGrid.PagerID = "SoftarticleListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=SoftarticleList&SoftArticleID=" + SoftArticleID;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("SoftArticleID");
    grid.jqGrid.AddColumn("SoftArticleID", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='show(" + obj + ")' style='cursor:pointer;'>查看文章</div>";
        }, false);
    grid.jqGrid.AddColumn("Title", "标题", true, null, 100);
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
    grid.jqGrid.AddColumn("QRImg", "小程序码", true, "center", 50,
       function (obj, options, rowObject) {
           return "<div id=‘QRImg" + rowObject.SoftArticleID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
       }, false);
    grid.jqGrid.AddColumn("ExposureCount", "曝光数", true, "center", 30);
    grid.jqGrid.AddColumn("ReadCount", "浏览数", true, "center", 30);
    
    if (SoftArticleID == 0) {
        grid.jqGrid.AddColumn("ReprintCount", "转载数", true, "center", 50,
        function (obj, options, rowObject) {
            if (parseInt(obj) > 0) {
                return obj + "<div onclick='show2(" + rowObject.SoftArticleID + ")' style='cursor:pointer;color:red;'>(查看转载)</div>";
            }
            else {
                return obj;
            }
        }, false);
    } else {
        grid.jqGrid.AddColumn("ReprintCount", "转载数", true, "center", 30);
    }
    grid.jqGrid.AddColumn("GoodCount", "点赞数", true, "center", 30);
    grid.jqGrid.AddColumn("CreatedAt", "发布时间", true, "center", 50);
    grid.jqGrid.AddColumn("SoftArticleID", "软文链接", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"CopyUrl('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看链接</div>";
        }, false);
    grid.jqGrid.CreateTable();
}
function showQRImg(obj) {
    window.open(obj);
}
function show(obj) {
    window.location.href = "CardSoftarticleShow.aspx?SoftArticleID=" + obj;
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();
    grid.jqGrid.AddSearchParams("Status", "=",2);
    grid.jqGrid.Search();
    return false;
}
function OnSearch2() {
    grid.jqGrid.InitSearchParams();
    grid.jqGrid.AddSearchParams("Status", "=",1);
    grid.jqGrid.Search(); 
    return false;
}
function show2(obj) {
    window.open("CardSoftarticleBrowse.aspx?SoftArticleID=" + obj);
}
function CopyUrl(obj) {
    bootbox.dialog({
        message: "pages/MyCenter/Article/Article?SoftArticleID=" + obj,
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
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}
