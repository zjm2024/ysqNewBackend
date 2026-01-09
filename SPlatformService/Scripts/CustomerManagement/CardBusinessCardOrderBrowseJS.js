$(function () {
    onPageInit();


});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessCardOrderList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("OrderNO", "订单号", true, null, 60);
    grid.jqGrid.AddColumn("ProdustsBusinessName", "商家", true, null, 60);
    grid.jqGrid.AddColumn("Name", "买家", true, "center", 30);
    grid.jqGrid.AddColumn("Phone", "买家手机", true, "center", 30);
    grid.jqGrid.AddColumn("Cost", "订单金额", true, "center", 60,
        function (obj, options, rowObject) {
            if (rowObject.isAgentBuy == 1) {
                return "<div style='color:#f00'>" + obj + "元</div>(保证金购买)";
            }
            if (rowObject.isEcommerceBuy == 1) {
                return "<div style='color:#f00'>" + obj + "元</div>(二级商户)";
            }
            return "<div style='color:#f00'>" + obj + "元</div>";
        });
    
    grid.jqGrid.AddColumn("CreatedAt", "下单时间", true, "center", 50);
    grid.jqGrid.AddColumn("payAt", "付款时间", true, "center", 50);
    grid.jqGrid.AddColumn("Title", "产品", true, "center", 50);
    grid.jqGrid.AddColumn("isUsed", "使用状态", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == 0) {
                return "<div style='color:#f00'>未使用</div>";
            }
            else {
                return "已使用";
            }
        });
    grid.jqGrid.AddColumn("BusinessName", "使用公司", true, "center", 60,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "") {
                result += obj;
            }
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("AgentName", "销售员", true, "center", 30);
    grid.jqGrid.CreateTable(); 
}
/**
   * 产品数据处理
   */
function productData(imgStr) {
    var Str = []
    if (imgStr != "") {
        var reg = /\[img\].+?\[\/img\]/g;
        Str = imgStr.match(reg);
        for (var i = 0; i < Str.length; i++) {
            Str[i] = Str[i].replace("[img]", "");
            Str[i] = Str[i].replace("[/img]", "");
        }
    }
    return Str[0];
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();    
    var objAgencyName = $("input[id*='txtAgencyName']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("BusinessName");
        filedArr.push("Phone");
        filedArr.push("Name");
        filedArr.push("AgentName");
        filedArr.push("Title");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}

