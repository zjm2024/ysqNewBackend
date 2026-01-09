$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardNewsList";
    grid.jqGrid.PagerID = "CardNewsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=FarmgamePrizeList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("PrizeID", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("PrizeID");
    grid.jqGrid.AddColumn("Name", "奖品名称", true, null, 50);
    grid.jqGrid.AddColumn("Price", "兑换价格", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div style='color:#ff0000'>" + obj + "钻</div>";
        });
    grid.jqGrid.AddColumn("ImgUrl", "缩略图", true, "center", 50,
        function (obj, options, rowObject) {
            return "<img src='" + obj + "' style='height:50px;'></div>";
        });

    grid.jqGrid.AddColumn("OrderNo", "排序", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj >0)
                return "<div style='color:#f30000;font-weight:bold;'>" + obj + "</div>";
            else 
                return obj;
        }, false); 
    grid.jqGrid.AddColumn("Status", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "<div>";
            if (obj == '1')
                str += "<span onclick='SetShelves(" + rowObject.PrizeID + ",0)' style='color:#882323;cursor:pointer'>下架奖品</span>";
            else
            {
                str += "<span onclick='SetShelves(" + rowObject.PrizeID + ",1)' style='cursor:pointer'>上架奖品</span>";
            }

            str += "</div>";
            return str;
        }, false);
    grid.jqGrid.CreateTable();   
}

function SetShelves(PrizeID, Status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/SetShelves?PrizeID=" + PrizeID + "&Status=" + Status + "&token=" + _Token,
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
                                window.location.href = "FarmgamePrizeBrowse.aspx";
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

function EditSystemMessage(categoryObj) {
    window.location.href = "FarmgamePrizeCreateEdit.aspx?PrizeID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "FarmgamePrizeCreateEdit.aspx";
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

function DeleteAction(PrizeID) {
    var url = _RootPath + "SPWebAPI/Card/DeleteFarmgamePrize?PrizeID=" + PrizeID + "&token=" + _Token;

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
                                window.location.href = "FarmGamePrizeBrowse.aspx";
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
