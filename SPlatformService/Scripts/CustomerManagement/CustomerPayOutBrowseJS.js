$(function () {
   // Init();
    onCustomerPayOutListTableInit();
});


function onCustomerPayOutListTableInit() {

    var grid = new JGrid();
    grid.jqGrid.ID = "CustomerPayOutHandleList";
    grid.jqGrid.PagerID = "CustomerPayOutHandleListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CustomerPayOutHandleList";
    grid.jqGrid.LengthMenu = ["10", "25", "50", "100"];
    grid.jqGrid.DefaultSortCol = "PayOutHistoryId";
    grid.jqGrid.DefaultSort = "desc";

    grid.jqGrid.AddColumn("PayOutHistoryId", "操作", false, "center", 50,
      function (obj, options, rowObject) {
          var result = '';
          result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditPayOutHistory(this);"></img>';
          return result;
      }, false);
    grid.jqGrid.AddHidColumn("PayOutHistoryId");    
    grid.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 120);
    grid.jqGrid.AddColumn("Phone", "电话", true, null, 120);
    grid.jqGrid.AddColumn("Cost", "申请金额", true, null, 120);
    grid.jqGrid.AddColumn("PayOutDate", "申请日期", true, null, 120);
    grid.jqGrid.AddColumn("PayOutStatus", "状态", true, null, 120,
    function (obj, options, rowObject) {
        if (obj == '0')
            return "申请提现";
        else if (obj == '-1')
            return "未提交申请";
        else if (obj == '-2')
            return "提现失败";
        else if (obj == '1')
            return "提现成功";
    }, false);
    grid.jqGrid.AddColumn("HandleComment", "备注", true, null, 120);
    grid.jqGrid.CreateTable();

}


function EditPayOutHistory(messageObj) {
    window.location.href = "CustomerPayOutHandle.aspx?PayOutHistoryId=" + $(messageObj).prev().val();
    return false;
}