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
    grid.jqGrid.ID = "AgencyReviewList";
    grid.jqGrid.PagerID = "AgencyReviewListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyReviewList&BusinessCustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "AgencyReviewId";
    grid.jqGrid.DefaultSort = "desc";   
    grid.jqGrid.AddColumn("AgencyReviewId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditBusinessReview(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("AgencyReviewId");
			
	grid.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 80);				
	grid.jqGrid.AddColumn("Title", "项目名称", true, null, 100);
	grid.jqGrid.AddColumn("AgencyCustomerName", "销售", true, null, 80,
        function (obj, options, rowObject) {
            if (obj.length > 2)
                return obj.substring(0, 2) + "***" + obj.substring(obj.length - 2);
            else
                return obj;
        }, false);
	grid.jqGrid.AddColumn("Score", "评价得分", true, null, 80);
	grid.jqGrid.AddColumn("Description", "评价内容", true, null, 100);	
    grid.jqGrid.CreateTable();   
}

function EditBusinessReview(agencyReviewObj) {
    window.location.href = "BusinessReviewEdit.aspx?agencyReviewId=" + $(agencyReviewObj).prev().val();
    return false;
}

