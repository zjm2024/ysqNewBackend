$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardHelpList";
    grid.jqGrid.PagerID = "CardHelpListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardHelpList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "Type asc,Order_Num";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("HelpID", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("HelpID");
    grid.jqGrid.AddColumn("Title", "标题", true, null, 50);
    grid.jqGrid.AddColumn("Description", "简介", true, null, 50);
    grid.jqGrid.AddColumn("Type", "类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "<div style='color:#058c9a;'>名片功能</div>";
            else if (obj == '2')
                return "<div style='color:#bd3718;'>活动功能</div>";
            else if (obj == '3')
                return "<div>其他功能</div>";
        }, false);
    grid.jqGrid.AddColumn("Order_Num", "排序", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj > 0)
                return "<div style='color:#f30000;font-weight:bold;'>" + obj + "</div>";
            else
                return obj;
        }, false);
    grid.jqGrid.AddColumn("Url", "视频链接", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "";
            str += "<a href='" + obj + "' target='_blank' style='color:#882323;cursor:pointer'>点击展示";
            str += "</a>";
            return str;
        }, false);
    grid.jqGrid.AddColumn("HelpID", "小程序链接", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"CopyUrl('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看链接</div>";
        }, false);
    grid.jqGrid.CreateTable();   
}

function CopyUrl(obj) {
    bootbox.dialog({
        message: "pages/MyCenter/HelpPlay/HelpPlay?HelpID=" + obj,
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

function EditSystemMessage(categoryObj) {
    window.location.href = "CardHelpCreateEdit.aspx?HelpID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardHelpCreateEdit.aspx";
    return false;
}

function DeleteSuggestion() {
    var id = $("#CardHelpList").jqGrid('getGridParam', 'selarrrow');
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

function DeleteAction(NewsID) {
    var url = _RootPath + "SPWebAPI/Card/DeleteCardHelp?HelpID=" + NewsID + "&token=" + _Token;

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
                                window.location.href = "CardHelpBrowse.aspx";
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
