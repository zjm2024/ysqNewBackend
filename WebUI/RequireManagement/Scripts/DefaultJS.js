$(function () {
    onInit();
    onPageInit();
});

var gridAgency = new JGrid();
var gridBusiness = new JGrid();
var gridComplaints = new JGrid();

function onPageInit() {
    gridBusiness.jqGrid.ID = "BusinessApproveList";
    gridBusiness.jqGrid.PagerID = "BusinessApproveListDiv";
    gridBusiness.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridBusiness.jqGrid.Params = "table=BusinessApproveList";
    gridBusiness.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    gridBusiness.jqGrid.DefaultSortCol = "CreatedAt";
    gridBusiness.jqGrid.DefaultSort = "desc";
    gridBusiness.jqGrid.Multiselect = true;
    gridBusiness.jqGrid.AddColumn("BusinessId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditBusiness(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteBusinessOne(this);"></img>';
            return result;
        }, false);
    gridBusiness.jqGrid.AddHidColumn("BusinessId");

    gridBusiness.jqGrid.AddColumn("CustomerCode", "编号", true, null, 50);
    gridBusiness.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    gridBusiness.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    gridBusiness.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);
    gridBusiness.jqGrid.AddColumn("CompanyName", "雇主名称", true, null, 50);
    gridBusiness.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if (obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
        }, false);
    gridBusiness.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 50);
    gridBusiness.jqGrid.CreateTable();


    gridAgency.jqGrid.ID = "AgencyApproveList";
    gridAgency.jqGrid.PagerID = "AgencyApproveListDiv";
    gridAgency.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridAgency.jqGrid.Params = "table=AgencyApproveList";
    gridAgency.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    gridAgency.jqGrid.DefaultSortCol = "CreatedAt";
    gridAgency.jqGrid.DefaultSort = "desc";
    gridAgency.jqGrid.Multiselect = true;
    gridAgency.jqGrid.AddColumn("AgencyId", "操作", false, "center", 30,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditAgency(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyOne(this);"></img>';
            return result;
        }, false);
    gridAgency.jqGrid.AddHidColumn("AgencyId");
		
    gridAgency.jqGrid.AddColumn("CustomerCode", "编号", true, null, 50);
    gridAgency.jqGrid.AddColumn("CustomerName", "会员名称", true, null, 50);
    gridAgency.jqGrid.AddColumn("CityName", "区域", true, null, 50);
    gridAgency.jqGrid.AddColumn("Phone", "手机号码", true, null, 50);
    gridAgency.jqGrid.AddColumn("AgencyName", "销售名称", true, null, 50);
    gridAgency.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请认证";
            else if (obj == '1')
                return "认证通过";
            else if (obj == '2')
                return "认证驳回";
        }, false);
    gridAgency.jqGrid.AddColumn("CreatedAt", "认证时间", true, null, 50);
    gridAgency.jqGrid.CreateTable();


    gridComplaints.jqGrid.ID = "ComplaintsUnResloveList";
    gridComplaints.jqGrid.PagerID = "ComplaintsUnResloveListDiv";
    gridComplaints.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    gridComplaints.jqGrid.Params = "table=ComplaintsUnResloveList";
    gridComplaints.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    gridComplaints.jqGrid.DefaultSortCol = "CreatedAt";
    gridComplaints.jqGrid.DefaultSort = "desc";
    gridComplaints.jqGrid.Multiselect = true;
    gridComplaints.jqGrid.AddColumn("ComplaintsId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditComplaints(this);"></img>';            
            return result;
        }, false);
    gridComplaints.jqGrid.AddHidColumn("ComplaintsId");

    gridComplaints.jqGrid.AddColumn("ProjectCode", "项目编号", true, null, 50);
    gridComplaints.jqGrid.AddColumn("CustomerName", "申请人", true, null, 50);
    gridComplaints.jqGrid.AddColumn("CreatedAt", "申请时间", true, null, 50);
    gridComplaints.jqGrid.AddColumn("Status", "状态", true, null, 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "申请中";
            else if (obj == '1')
                return "已解决";
        }, false);
    gridComplaints.jqGrid.CreateTable();
}

function EditBusiness(businessObj) {
    window.location.href = "../CustomerManagement/BusinessCreateEdit.aspx?IsApprove=true&BusinessId=" + $(businessObj).prev().val();
    return false;
}

function EditAgency(agencyObj) {
    window.location.href = "../CustomerManagement/AgencyCreateEdit.aspx?IsApprove=true&AgencyId=" + $(agencyObj).prev().val();
    return false;
}

function EditComplaints(complaintsObj) {
    window.location.href = "../ProjectManagement/ComplaintsCreateEdit.aspx?ComplaintsId=" + $(complaintsObj).prev().val();
    return false;
}


function onInit() {
    $.ajax({
        url: _RootPath + "SPWebAPI/System/GetConfig",
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var configVO = data.Result;
                var objProjectCount = $("span[id*='lblProjectCount']");
                var objCommTotal = $("span[id*='lblCommCount']");

                objProjectCount.html(configVO.ProjectTotal);
                objCommTotal.html(configVO.CommissionTotal);
            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

