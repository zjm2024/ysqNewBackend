$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardNewsList";
    grid.jqGrid.PagerID = "CardNewsListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=FarmgamePrizeOrderViewList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];   
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("PrizeOrderID");
    grid.jqGrid.AddColumn("HeaderLogo", "头像", true, "center", 30,
        function (obj, options, rowObject) {
            return "<img src='" + obj + "' style='height:50px;'></div>";
        });
    grid.jqGrid.AddColumn("Name", "会员名称", true, "center", 50);
    grid.jqGrid.AddColumn("Price", "花费钻石", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div style='color:#ff0000'>" + obj + "钻</div>";
        });
    grid.jqGrid.AddColumn("ImgUrl", "缩略图", true, "center", 30,
        function (obj, options, rowObject) {
            return "<img src='" + obj + "' style='height:50px;'></div>";
        });
    grid.jqGrid.AddColumn("PrizeName", "兑换的奖品", true, "center", 50);

    grid.jqGrid.AddColumn("Name", "收货姓名", true, "center", 50);
    grid.jqGrid.AddColumn("Phone", "收货手机", true, "center", 50);
    grid.jqGrid.AddColumn("Address", "收货地址", true, "center", 120);

    grid.jqGrid.AddColumn("Status", "状态", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "";
            if (obj == '1') {
                str = "<div style='color:#ff0000'>未发货</div>";
            } else if (obj == '2') {
                str = "已发货";
            }
            else if (obj == '0') {
                str = "已关闭";
            }
            return str;
        });
    grid.jqGrid.AddColumn("CreatedAt", "兑换时间", true, "center", 50);
    grid.jqGrid.AddColumn("Status", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            var str = "<div>";
            if (obj == '1')
                str += "<span onclick='PrizeDeliver(" + rowObject.PrizeOrderID + ")' style='color:#882323;cursor:pointer'>确认发货</span>";
            str += "</div>";
            return str;
        }, false);
    grid.jqGrid.CreateTable();   
}

function PrizeDeliver(PrizeOrderID) {

    bootbox.dialog({
        message: "是否确认该订单已经发货了?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Card/PrizeDeliver?PrizeOrderID=" + PrizeOrderID + "&token=" + _Token,
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
                                                window.location.href = "FarmgamePrizeOrderViewBrowse.aspx";
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