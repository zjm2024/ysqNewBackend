$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "VipApplyList";
    grid.jqGrid.PagerID = "VipApplyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=VipApplyList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("CustomerId");
    grid.jqGrid.AddColumn("CustomerId", "申请ID", true, "center", 50);
    grid.jqGrid.AddColumn("CreatedAt", "提交时间", true, "center", 50);
    grid.jqGrid.AddColumn("Name", "名称", true, "center", 50);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, "center", 50);
    grid.jqGrid.AddColumn("CorporateName", "公司", true, "center", 50);
    grid.jqGrid.AddColumn("Position", "职位", true, "center", 50);
    grid.jqGrid.AddColumn("Type", "申请VIP类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == "6")
                return "合伙人";
            else if (obj == "7")
                return "分公司";
            else
                return "-";
        }, false);
    grid.jqGrid.AddColumn("Content", "申请留言", true, "center", 100);
    grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#0066ff;cursor:pointer;'  >查看他的名片</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"EditCustomer('" + obj + "')\"  style='color:#0066ff;cursor:pointer;'  >开通VIP</div>";
        }, false);
    grid.jqGrid.CreateTable();   
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}
function EditCustomer(Obj) {
    window.open("CustomerCreateEdit.aspx?CustomerId=" + Obj);
    return false;
}