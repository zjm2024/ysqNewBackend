$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var BusinessID = $("input[id*='BusinessID']").val();
    var CustomerId = $("input[id*='CustomerId']").val();
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";

    if (BusinessID > 0) {
        grid.jqGrid.Params = "table=PersonalList&BusinessID=" + BusinessID;
    }
    if (CustomerId > 0) {
        grid.jqGrid.Params = "table=PersonalList&CustomerId=" + CustomerId;
    }
    
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("PersonalID", "PersonalID", true, "center", 30);
    grid.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="Logo"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("Name", "名称", true, null, 30);
    grid.jqGrid.AddColumn("QRimg", "小程序码", true, "center", 20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.PersonalID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    
    grid.jqGrid.AddColumn("Phone", "手机", true, "center", 30);
    grid.jqGrid.AddColumn("Position", "职位", true, "center", 30);
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    grid.jqGrid.AddColumn("Email", "邮箱", true, "center", 30);
    grid.jqGrid.AddColumn("Tel", "固定电话", true, "center", 30);
    grid.jqGrid.AddColumn("WeChat", "微信", true, "center", 30);
    grid.jqGrid.AddColumn("Address", "地址", true, "center", 60);
    grid.jqGrid.AddColumn("Business", "主营业务", true, "center", 30);
    grid.jqGrid.AddColumn("ReadCounts", "访问人数", true, "center", 30);
    grid.jqGrid.AddColumn("CustomerId", "个人名片", true, "center", 20,
        function (obj, options, rowObject) {
            var result = "";
            if (rowObject.CustomerId > 0)
                result = "<div onclick=\"showCustomerCard('" + obj + "')\"  style='color:#197e9c;cursor:pointer;'  >个人名片</div>";
            else
                result = "匿名";
            return result;
        }, false);
    grid.jqGrid.AddColumn("CustomerId", "会员信息", true, "center", 20,
        function (obj, options, rowObject) {
            var result = "";
            if (rowObject.CustomerId > 0)
                result = "<div onclick=\"EditCustomer('" + obj + "')\"  style='color:#197e9c;cursor:pointer;'  >会员信息</div>";
            else
                result = "匿名";
            return result;
        }, false);
    grid.jqGrid.CreateTable(); 
}
function showQRImg(obj) {
    window.open(obj);
}
function OnSearch() {
    grid.jqGrid.InitSearchParams();    
    var objAgencyName = $("input[id*='txtAgencyName']");
    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("Name");
        filedArr.push("Phone");
        filedArr.push("Position");
        filedArr.push("Address");
        filedArr.push("BusinessName");
        filedArr.push("Industry");
        filedArr.push("DepartmentName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}
function EditCustomer(Obj) {
    window.open("CustomerCreateEdit.aspx?CustomerId=" + Obj);
    return false;
}