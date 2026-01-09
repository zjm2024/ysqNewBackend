$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var CustomerId = parseInt($("#" + hidCustomerId).val());
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    if (CustomerId > 0) {
        grid.jqGrid.Params = "table=CardExchangeCodeList&CustomerId=" + CustomerId;
    } else {
        grid.jqGrid.Params = "table=CardExchangeCodeList";
    }
    
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("Code", "兑换码", true, "center", 50);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    grid.jqGrid.AddColumn("ExpirationAt", "有效期", true, "center", 30);
    grid.jqGrid.AddColumn("Status", "状态", true, "center", 30, function (obj, options, rowObject) {
        if (obj > 0) {
            return "<div style='color:#ff6600;'>已兑换</div>";
        } else {
            return "未兑换";
        }
    }, false);
    grid.jqGrid.AddColumn("Type", "类型", true, "center", 30, function (obj, options, rowObject) {
        if (obj == 0) {
            return "<div style='color:#0095ff;'>年会员</div>";
        } else {
            return "<div style='color:#f06600;'>试用码</div>";
        }
    }, false);
    grid.jqGrid.AddColumn("ToCustomerName", "兑换会员", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.ToCustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("UsedAt", "兑换时间", true, "center", 30);
    grid.jqGrid.CreateTable();   
}
function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}
