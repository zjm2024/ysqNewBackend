$(function () {   

    if (!isAgency) {
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
                        window.location.href = "../CustomerManagement/AgencyCreateEdit.aspx";
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
    grid.jqGrid.ID = "TenderInviteList";
    grid.jqGrid.PagerID = "TenderInviteListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=TenderInviteList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "TenderInviteId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = false;
    //grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 10,
    //    function (obj, options, rowObject) {
    //        var result = '';
    //        result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="接受邀请" onclick="return ReceiveTender(this);"></img>';
    //        return result;
    //    }, false);
    //grid.jqGrid.AddHidColumn("RequirementId");
			
    grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 50);
    grid.jqGrid.AddColumn("Title", "任务标题", true, null, 50);
    grid.jqGrid.AddColumn("InviteDate", "投标日期", true, null, 50);
    grid.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "保存";
            else if (obj == '1')
                return "发布";
            else if (obj == '2')
                return "关闭";
            else if (obj == '3')
                return "暂停投标";
            else if (obj == '4')
                return "已选定销售";
            else
                return "";
        }, false);;
    grid.jqGrid.CreateTable();   
}