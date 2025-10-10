$(function () {
    onPageInit();
});


var grid = new JGrid();
function onPageInit() {

    var RedPacketId = $("input[id*='RedPacketId']").val();

    grid.jqGrid.ID = "CardRedPacketDetaillistList";
    grid.jqGrid.PagerID = "CardRedPacketDetaillistListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardRedPacketDetaillistList&RedPacketId=" + RedPacketId;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "RPListId";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddHidColumn("RPListId");	
    grid.jqGrid.AddColumn("RedPacketId", "广告红包ID", true, "center", 50);
    grid.jqGrid.AddColumn("RPOneCost", "单个红包金额", true, "center", 50);
    grid.jqGrid.AddColumn("CustomerName", "领取用户", true, "center", 50);
    grid.jqGrid.AddColumn("Name", "名片名字", true, "center", 50);
    grid.jqGrid.AddColumn("CardPhone", "名片电话", true, "center", 50);
    grid.jqGrid.AddColumn("CorporateName", "名片公司", true, "center", 50);
    grid.jqGrid.AddColumn("Position", "名片职位", true, "center", 50);
   // grid.jqGrid.AddColumn("OpenId", "OpenId", true, "center", 50);
    grid.jqGrid.AddColumn("CustomerId", "CustomerId", true, "center", 50);
    grid.jqGrid.AddColumn("ReceiveDate", "领取日期", true, "center", 50);
    grid.jqGrid.AddColumn("isReceive", "状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "<div style='color:#882323'>未领取</div>";
            else if (obj == '1')
                return "已领取";
            else if (obj == '2')
                return "已领,未发送零钱";
        }, false);
      grid.jqGrid.AddColumn("LikeType", "点赞状态", true, "center", 50,
            function (obj, options, rowObject) {
                if (obj == '0')
                    return "<div style='color:#882323'>未点赞</div>";
                else if (obj == '1')
                    return "赞";
                else if (obj == '踩')
                    return "已领,未发送零钱";
            }, false);
    grid.jqGrid.CreateTable();  
    




}

function EditSystemMessage(categoryObj) {
    window.location.href = "CardMessageCreateEdit.aspx?NoticeID=" + $(categoryObj).prev().val();
    return false;
}

function NewSystemMessage() {
    window.location.href = "CardRedPacketCreate.aspx?RedPacketId=0";
    return false;
}

$("#gotoCardRedPacketListBrowse").click(function () {
    window.location.href = "CardRedPacketListBrowse.aspx";
});

function DeleteSuggestion() {
   /**
    *  var id = $("#CardRedPacketDetaillistList").jqGrid('getGridParam', 'selarrrow');
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
    * **/
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
