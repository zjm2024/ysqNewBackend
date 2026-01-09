$(function () {
    if (!isBusiness) {
        load_hide();
        bootbox.dialog({
            message: "还未通过认证，不能执行该操作!",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "../CustomerManagement/BusinessCreateEdit.aspx";
                    }
                }
            }
        });
        return false;
    }
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "MyAgencyList";
    grid.jqGrid.PagerID = "MyAgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MyAgencyList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("AgencyId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="查看" onclick="return EditAgency(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyId");

    grid.jqGrid.AddColumn("CustomerCode", "编号", true, null, 80);
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 80);
    grid.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 100);
    grid.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 80);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 70,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if (obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
            else
                return "认证驳回";
        }, false);
    grid.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 160);
    grid.jqGrid.CreateTable();
}


function EditAgency(projectObj) {
    window.location.href = "../Agency.aspx?agencyId=" + $(projectObj).prev().val();
    return false;
}

//function OnSearch() {
//    grid.jqGrid.InitSearchParams();

//    var objProjectName = $("input[id*='txtProjectName']");
//    grid.jqGrid.AddSearchParams("ProjectCode", "LIKE", objProjectName.val());

//    grid.jqGrid.Search();
//    return false;
//}

