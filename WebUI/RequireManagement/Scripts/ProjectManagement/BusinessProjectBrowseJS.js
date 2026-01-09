$(function () {
    ////is business?
    //var isBusiness = false;
    //$.ajax({
    //    url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + _CustomerId + "&token=" + _Token,
    //    type: "Get",
    //    data: null,
    //    async: false,
    //    success: function (data) {
    //        if (data.Flag == 1) {
    //            if (data.Result.BusinessId > 0 && data.Result.BusinessStatus == 1)
    //                isBusiness = true;
    //        }
    //    },
    //    error: function (data) {

    //    }
    //});

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
    grid.jqGrid.ID = "BusinessProjectList";
    grid.jqGrid.PagerID = "BusinessProjectListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessProjectList&BusinessCustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ProjectId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditProject(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ProjectId");
			
	grid.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 80);				
	grid.jqGrid.AddColumn("Title", "任务名称", true, null, 100);
	grid.jqGrid.AddColumn("BusinessName", "雇主", true, null, 80);
	grid.jqGrid.AddColumn("AgencyName", "销售", true, null, 80);
	grid.jqGrid.AddColumn("StartDate", "开始时间", true, null, 100,
        function (obj, options, rowObject) {
            if (obj == "1900-01-01")
                return "";
            else
                return obj;
        }, false);
	grid.jqGrid.AddColumn("EndDate", "结束时间", true, null, 100,
        function (obj, options, rowObject) {
	    if (obj == "1900-01-01")
	        return "";
	    else
	        return obj;
	}, false);
	grid.jqGrid.AddColumn("Commission", "酬金", true, null, 80);				
	grid.jqGrid.AddColumn("Status", "状态", true, null, 70,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "已生成";
            else if (obj == '1')
                return "工作中";
            else if (obj == '2')
                return "已完工";
            else if (obj == '3')
                return "申请完工";
            else if (obj == '4')
                return "申请退款";
            else if (obj == '5')
                return "退款完成";
            else if (obj == '6')
                return "已取消";
        }, false);
    grid.jqGrid.CreateTable();   
}


function EditProject(projectObj) {
    window.location.href = "BusinessProjectCreateEdit.aspx?ProjectId=" + $(projectObj).prev().val();
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objProjectName = $("input[id*='txtProjectName']");
    grid.jqGrid.AddSearchParams("ProjectCode", "LIKE", objProjectName.val());

    grid.jqGrid.Search();
    return false;
}

