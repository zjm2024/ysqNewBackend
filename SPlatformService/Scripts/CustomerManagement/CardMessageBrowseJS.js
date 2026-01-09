$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardMessageList";
    grid.jqGrid.PagerID = "CardMessageListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardMessageList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "SendDate";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("NoticeID", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("NoticeID");			
    grid.jqGrid.AddColumn("Content", "通知内容", true, null, 50);
    grid.jqGrid.AddColumn("SendDate", "发送日期", true, "center", 50);
    grid.jqGrid.AddColumn("Style", "接收", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "全部用户";
            else if (obj == '1')
                return "仅活动用户";
        }, false);
   
    grid.jqGrid.AddColumn("Status", "发送状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "<div style='color:#882323'>待发送</div>";
            else if (obj == '1')
                return "已发送";
        }, false);
    grid.jqGrid.CreateTable();
}

function EditSystemMessage(categoryObj) {
    window.location.href = "CardMessageCreateEdit.aspx?NoticeID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardMessageCreateEdit.aspx";
    return false;
}

function DeleteSuggestion() {
    var id = $("#CardMessageList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        DeleteAction(idString);
                    }
                },
                "Cancel":
                {
                    "label": "取消",
                    "className": "btn-sm",
                    "callback": function () {
                    }
                }
            }
        });
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

function DeleteAction(NoticeID) {
    var url = _RootPath + "SPWebAPI/Card/DeleteCardMessage?NoticeID=" + NoticeID + "&token=" + _Token;

    console.log(url);
    $.ajax({
        url: url,
        type: "get",
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
                                window.location.href = "CardMessageBrowse.aspx";
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
            console.log(data);
            //load_hide();
        }
    });
}
