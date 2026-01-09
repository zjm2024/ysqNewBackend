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
    grid.jqGrid.ID = "TenderList";
    grid.jqGrid.PagerID = "TenderListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=TenderList&CustomerId=" + _CustomerId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "TenderInfoId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("RequirementId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="接受邀请" onclick="return ReceiveTender(this);"></img>';
            //result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteAgencyExperienceOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("RequirementId");
			
    grid.jqGrid.AddColumn("RequirementCode", "任务编号", true, null, 50);
    grid.jqGrid.AddColumn("Title", "任务标题", true, null, 50);
    grid.jqGrid.AddColumn("TenderDate", "邀请日期", true, null, 50);
    grid.jqGrid.CreateTable();   
}

function ReceiveTender(obj) {
    var tenderInviteVO = new Object();
    tenderInviteVO.RequirementId = $(obj).prev().val();
    tenderInviteVO.CustomerId = _CustomerId;
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequireTenderInvite?token=" + _Token,
        type: "POST",
        data: tenderInviteVO,
        success: function (data) {
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
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function ReceiveTenderAll() {
    var id = $("#TenderList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        ReceiveTenderAllAction(idString, _CustomerId);
    }
    else {
        bootbox.dialog({
            message: "请至少选择一条数据！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
    }
}

function ReceiveTenderAllAction(requireId, customerId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateRequireTenderInviteAll?requireIds=" + requireId + "&customerId=" + customerId + "&token=" + _Token,
        type: "Post",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
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
            //load_hide();
        },
        error: function (data) {
            alert(data);
            //load_hide();
        }
    });
}
