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
    grid.jqGrid.ID = "BusinessReviewList";
    grid.jqGrid.PagerID = "BusinessReviewListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=BusinessReviewList&AgencyCustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "BusinessReviewId";
    grid.jqGrid.DefaultSort = "desc";   
    grid.jqGrid.AddColumn("BusinessReviewId", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditBusinessReview(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("BusinessReviewId");
			
	grid.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 50);				
	grid.jqGrid.AddColumn("Title", "项目名称", true, null, 50);
	grid.jqGrid.AddColumn("BusinessCustomerName", "雇主", true, null, 50,
        function (obj, options, rowObject) {
            if (obj.length > 2)
                return obj.substring(0, 2) + "***" + obj.substring(obj.length - 2);
            else
                return obj;
        }, false);
	grid.jqGrid.AddColumn("Score", "评价得分", true, null, 50);
	grid.jqGrid.AddColumn("Description", "评价内容", true, null, 50);	
    grid.jqGrid.CreateTable();   
}

function EditBusinessReview(businessReviewObj) {
    window.location.href = "AgencyReviewEdit.aspx?businessReviewId=" + $(businessReviewObj).prev().val();
    return false;
}

