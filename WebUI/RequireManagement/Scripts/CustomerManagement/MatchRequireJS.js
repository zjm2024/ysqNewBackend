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
    grid.jqGrid.ID = "RequirementList";
    grid.jqGrid.PagerID = "RequirementListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=MatchRequireList&AgencyId=" + _AgencyId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="查看" onclick="return EditRequirement(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteRequirementOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("RequirementId");

    grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 80);
    grid.jqGrid.AddColumn("Title", "标题", true, null, 100);
    grid.jqGrid.AddColumn("EffectiveEndDate", "有效期", true, null, 80);
    grid.jqGrid.AddColumn("CommissionType", "酬金类型", true, null, 80,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "金额";
            else if (obj == '2')
                return "合同比例";
        }, false);
    grid.jqGrid.AddColumn("Commission", "酬金", true, null, 50);
    grid.jqGrid.AddColumn("TenderCount", "投标数目", true, null, 80);    
    grid.jqGrid.AddColumn("Status", "状态", true, null, 80,
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
        }, false);
    grid.jqGrid.AddColumn("CreatedAt", "创建日期", true, null, 80);
    grid.jqGrid.CreateTable();
}


function EditRequirement(requirementObj) {
    window.location.href = "../Require.aspx?requireId=" + $(requirementObj).prev().val();
    return false;
}

function OnSearch() {
    //grid.jqGrid.InitSearchParams();

    //var objRequirementName = $("input[id*='txtRequirementName']");
    //grid.jqGrid.AddSearchParams("Title", "LIKE", objRequirementName.val());

    //var objStatus = $("select[id*='drpStatus']");
    //if (objStatus.val() > -1)
    //    grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    //grid.jqGrid.Search();
    //return false;
}

