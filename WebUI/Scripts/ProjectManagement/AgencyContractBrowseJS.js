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
    grid.jqGrid.ID = "AgencyContractList";
    grid.jqGrid.PagerID = "AgencyContractListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=AgencyContractList&AgencyCustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("ContractId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditContract(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("ContractId");
			
	grid.jqGrid.AddColumn("ProjectName", "项目名称", true, null, 100);
	grid.jqGrid.AddColumn("CompanyName", "雇主", true, null, 70);
	grid.jqGrid.AddColumn("AgencyName", "销售", true, null, 70);
	grid.jqGrid.AddColumn("StartDate", "开始时间", true, null, 100);				
	grid.jqGrid.AddColumn("EndDate", "结束时间", true, null, 100);				
	grid.jqGrid.AddColumn("Commission", "酬金", true, null, 50);
	grid.jqGrid.AddColumn("ContractFile", "合同", true, null, 50,
        function (obj, options, rowObject) {
            var oTR = "";
            if (obj != "")
                oTR += "<a target=\"_blank\" href=\"" + obj + "\">查看</a>";

            return oTR;
        }, false);
	grid.jqGrid.AddColumn("ContractId", "合同附件", true, null, 60,
        function (obj, options, rowObject) {
            var contractId = obj;
            var oTR = "";
            //绑定附件
            $.ajax({
                url: _RootPath + "SPWebAPI/Project/GetContractFile?contractId=" + contractId + "&token=" + _Token,
                type: "Get",
                async: false,
                data: null,
                success: function (data) {
                    if (data.Flag == 1) {
                        var puVOList = data.Result;

                        for (var i = 0; i < puVOList.length; i++) {
                            var puVO = puVOList[i];
                            oTR += "<a target=\"_blank\" href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a>";
                        }

                    } else {

                    }
                },
                error: function (data) {

                }
            });
            return oTR;
        }, false);
	grid.jqGrid.AddColumn("BusinessStatus", "雇主状态", true, null, 60,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "已创建";
            else if (obj == '1')
                return "已签署";            
            else
                return "已创建";
        }, false);
	grid.jqGrid.AddColumn("AgencyStatus", "销售状态", true, null, 60,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "已创建";
            else if (obj == '1')
                return "已签署";
            else
                return "已创建";
        }, false);
    grid.jqGrid.CreateTable();   
}


function EditContract(contractObj) {
    window.location.href = "GenerateProject.aspx?ContractId=" + $(contractObj).prev().val();
    return false;
}


function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objProjectName = $("input[id*='txtProjectName']");
    grid.jqGrid.AddSearchParams("ProjectName", "LIKE", objProjectName.val());

    grid.jqGrid.Search();
    return false;
}

