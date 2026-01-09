$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardNewsList";
    grid.jqGrid.PagerID = "CardNewsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardNewsList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CreatedAt desc,OrderNO ";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("NewsID", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("NewsID");
    grid.jqGrid.AddColumn("Title", "标题", true, null, 50);
    grid.jqGrid.AddColumn("Synopsis", "简介", true, null, 50);
    grid.jqGrid.AddColumn("NewsImg", "缩略图", true, "center", 50,
        function (obj, options, rowObject) {
            return "<img src='" + obj + "' style='height:50px;'></div>";
        });

    grid.jqGrid.AddColumn("OrderNO", "排序", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj >0)
                return "<div style='color:#f30000;font-weight:bold;'>" + obj + "</div>";
            else 
                return obj;
        }, false);
        
    grid.jqGrid.AddColumn("GoType", "跳转类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "公众号";
            else if (obj == '2')
                return "小程序";
            else if (obj == '3')
                return "其他小程序";
        }, false);
    grid.jqGrid.AddColumn("ShowType", "展示类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '1')
                return "图文并排";
            else if (obj == '2')
                return "单一大图";
        }, false);
    grid.jqGrid.AddColumn("ClickCount", "点击数量", true, "center", 50);
    grid.jqGrid.AddColumn("isDefault", "状态", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "<div>";
            if (obj == '1')
                str += "<span onclick='delDefault(" + rowObject.NewsID + ")' style='color:#882323;cursor:pointer'>取消展示</span>";
            else
            {
                str += "<span onclick='EditDefault(" + rowObject.NewsID + ")' style='cursor:pointer'>设为展示</span>";
            }

            if (rowObject.isAlert == '1')
                str += "<span onclick='delAlert(" + rowObject.NewsID + ")' style='color:#882323;cursor:pointer; margin-left:10px;'>取消弹窗</span>";
            else {
                str += "<span onclick='EditAlert(" + rowObject.NewsID + ")' style='cursor:pointer; margin-left:10px;'>设为弹窗</span>";
            }

            str += "</div>";
            return str;
        }, false);
    grid.jqGrid.CreateTable();   
}

function delDefault(NewsID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/DelDefaultCardNews?NewsID=" + NewsID + "&token=" + _Token,
        type: "Get",
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
                                window.location.href = "CardNewsBrowse.aspx";
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


function delAlert(NewsID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/DelAlertCardNews?NewsID=" + NewsID + "&token=" + _Token,
        type: "Get",
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
                                window.location.href = "CardNewsBrowse.aspx";
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

function EditDefault(NewsID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/EditDefaultCardNews?NewsID=" + NewsID + "&token=" + _Token,
        type: "Get",
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
                                window.location.href = "CardNewsBrowse.aspx";
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

function EditAlert(NewsID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/EditAlertCardNews?NewsID=" + NewsID + "&token=" + _Token,
        type: "Get",
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
                                window.location.href = "CardNewsBrowse.aspx";
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

function Send() {
    bootbox.dialog({
        message: "是否将当前展示文章推送给所有用户",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Card/sendCardNewsMessage",
                        type: "Get",
                        data: null,
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
                                            window.location.href = "CardNewsBrowse.aspx";
                                        }
                                    }
                                }
                            });
                        },
                        error: function (data) {
                            alert(data);
                            load_hide();
                        }
                    });
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

function EditSystemMessage(categoryObj) {
    window.location.href = "CardNewsCreateEdit.aspx?NoticeID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardNewsCreateEdit.aspx";
    return false;
}

function DeleteSuggestion() {
    var id = $("#CardNewsList").jqGrid('getGridParam', 'selarrrow');
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
    var url = _RootPath + "SPWebAPI/Card/DeleteCardNews?NewsID=" + NewsID + "&token=" + _Token;

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
                                window.location.href = "CardNewsBrowse.aspx";
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
