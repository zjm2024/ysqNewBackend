$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardNewsList";
    grid.jqGrid.PagerID = "CardNewsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardPoterList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CardPoterID desc,Order_info ";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    
    grid.jqGrid.AddHidColumn("CardPoterID");
    grid.jqGrid.AddColumn("FileName", "文件名", true, "center", 50,
        function (obj, options, rowObject) {
            return "<a style='color:#428bca;cursor:pointer;'  onclick=\"showQRImg('" + rowObject.Url + "')\">" + obj + "</a>";
        }, false);
    grid.jqGrid.AddColumn("Url", "缩略图", true, "center", 20,
        function (obj, options, rowObject) {
            if (rowObject.SizeType == 1 || rowObject.SizeType == 2) {
                return "<img src='" + obj + "' style='height:50px;cursor:pointer;'   onclick=\"showQRImg('" + obj + "')\"></div>";
            } else {
                return "-";
            }
            
        });
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 30);
    grid.jqGrid.AddColumn("Order_info", "排序", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj >0)
                return "<div style='color:#f30000;font-weight:bold;'>" + obj + "</div>";
            else 
                return obj;
        }, false);
        
    grid.jqGrid.AddColumn("Type", "类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == 'zhufu')
                return "问候";
            else if (obj == 'jieqi')
                return "节气";
            else if (obj == 'lizhi')
                return "励志";
            else if (obj == 'tuandui')
                return "团队";
            else if (obj == 'shanshui')
                return "山水";
            else if (obj == 'zhiwu')
                return "植物";
            else if (obj == 'meishi')
                return "美食";
            else if (obj == 'jianzhu')
                return "建筑";
            else if (obj == 'renwu')
                return "人物";
            else if (obj == 'dongwu')
                return "动物";
        }, false);
    grid.jqGrid.AddColumn("SizeType", "尺寸类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == 1)
                return "新版海报";
            else if (obj == 2)
                return "经典海报";
            else if (obj == 3)
                return "背景音乐";
        }, false);
    grid.jqGrid.AddColumn("CardPoterID", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "<div>";
            str += "<span onclick='EditOrder(" + rowObject.CardPoterID + ")' style='cursor:pointer'>修改排序</span>";

            str += "</div>";
            return str;
        }, false);
    grid.jqGrid.CreateTable();   
}

function EditOrder(CardPoterID) {
    var title = "修改排序";
    bootbox.dialog({
        message:
            '<style type="text\/css">.modal-dialog{ top: 10%;}<\/style>' +
            '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
            '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
            '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
            '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseRecharge.aspx" height="200px" width="100%" frameborder="0"><\/iframe>',
        title: title,
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objCost = $(window.frames["iframe_1"].document).find("input[id*='txtCost']")
                    var Sum = parseInt(objCost.val());
                    if (Sum != Sum || Sum < 0) {
                        bootbox.dialog({
                            message: "请输入正确的数值",
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
                        return;
                    }
                    setEditOrder(Sum, CardPoterID);
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
    return false;
}

function setEditOrder(Sum, CardPoterID) {
    console.log(Sum, CardPoterID);
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setCardPoterOrder?Sum=" + Sum + "&CardPoterID=" + CardPoterID + "&token=" + _Token,
        type: "get",
        success: function (data) {
            console.log(data);
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
                                window.location.href = window.location.href;
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

        },
        error: function (data) {
            console.log(data);
        }
    });
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
    var url = _RootPath + "SPWebAPI/Card/DeleteCardPoterByAdmin?NewsID=" + NewsID + "&token=" + _Token;

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
                                window.location.href = "CardPoterBrowse.aspx";
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
function showQRImg(obj) {
    window.open(obj);
}

function UpImg() {
    var title = "上传多媒体文件";
    bootbox.dialog({
        message:
            '<style type="text\/css">.modal-dialog{ top: 10%;}<\/style>' +
            '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
            '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
            '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
            '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseUpImg.aspx" height="400px" width="100%" frameborder="0"><\/iframe>',
        title: title,
        /*
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var objFile = $(window.frames["iframe_1"].document).find("input[id*='FileUpload1']")
                    var objselect = $(window.frames["iframe_1"].document).find("select[id*='Select1']")
                    console.log(objFile, objselect)
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
        */
    });
    return false;
}
