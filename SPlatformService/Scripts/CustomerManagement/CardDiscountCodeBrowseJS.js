$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=DiscountCodeList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("DiscountCodeID");
    grid.jqGrid.AddColumn("DiscountCodeID", "操作", false, "center", 10,
            function (obj, options, rowObject) {
                var result = '';
                result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
                return result;
            }, false);
    grid.jqGrid.AddColumn("Code", "优惠码", true, "center", 120);
    grid.jqGrid.AddColumn("Cost", "优惠价", true, "center", 30, function (obj, options, rowObject) {
         return obj+"元";
    }, false);
    grid.jqGrid.AddColumn("ExpirationAt", "截止时间", true, "center", 30);
    grid.jqGrid.AddColumn("ExpirationAt", "当前状态", true, "center", 30, function (obj, options, rowObject) {
        var now = new Date();
        if (compareDate(obj, now.getFullYear() + "-" + (now.getMonth() + 1) + "-" + now.getDate())) {
            return "<div style='color:#ff6600;'>正常</div>";
        } else {
            return "已过期";
        }
        
    }, false);
    grid.jqGrid.CreateTable(); 


}

function compareDate(s1, s2) {
    return ((new Date(s1.replace(/-/g, "\/"))) >= (new Date(s2.replace(/-/g, "\/"))));
}

function EditSystemMessage(categoryObj) {
    window.location.href = "CardDiscountCodeCreateEdit.aspx?DiscountCodeID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardDiscountCodeCreateEdit.aspx";
    return false;
}

function DeleteSuggestion() {
    var id = $("#AgencyList").jqGrid('getGridParam', 'selarrrow');
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

function DeleteAction(DiscountCodeID) {
    var url = _RootPath + "SPWebAPI/Card/DeleteCardDiscountCode?DiscountCodeID=" + DiscountCodeID + "&token=" + _Token;

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
                                window.location.href = "CardDiscountCodeBrowse.aspx";
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

