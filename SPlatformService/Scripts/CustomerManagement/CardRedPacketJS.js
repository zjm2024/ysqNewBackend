$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "CardRedPacketList";
    grid.jqGrid.PagerID = "CardRedPacketDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardRedPacketList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "RedPacketId";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
   /**
    *  grid.jqGrid.AddColumn("NoticeID", "操作", false, "center", 10,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSystemMessage(this);"></img>';
            return result;
        }, false);
    * **/
    grid.jqGrid.AddColumn("RedPacketId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectRedPacketList(" + obj + ")' style='cursor:pointer;'>查看</div>";
        }, false);
    grid.jqGrid.AddHidColumn("RedPacketId");	
    grid.jqGrid.AddColumn("RPContent", "红包内容说明", true, "center", 50);
    grid.jqGrid.AddColumn("RPCost", "红包总额", true, "center", 50);
    grid.jqGrid.AddColumn("RPNum", "红包总数", true, "center", 50);
    grid.jqGrid.AddColumn("RPResidueCost", "红包剩余总额", true, "center", 50);
    grid.jqGrid.AddColumn("RPResidueNum", "红包剩余个数", true, "center", 50);
    grid.jqGrid.AddColumn("RPCreateDate", "红包发布时间", true, "center", 50);
    grid.jqGrid.AddColumn("CustomerName", "发红包用户", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == "")
                return "<div>管理员</div>";
            else {
                return obj;
            }
        }, false);

    grid.jqGrid.AddColumn("RPType", "红包类型", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "<div>官方红包</div>";
            else if (obj == '1')
                return "个人红包";
        }, false);
    grid.jqGrid.AddColumn("Status", "广告状态", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == '0')
                return "<div>禁用</div>";
            else if (obj == '1')
                return "正在进行中";
            else if (obj == '2')
                return "已发完";
        }, false);
    grid.jqGrid.AddColumn("Status", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj =='0') 
                return "<div onclick='EditDefault(" + rowObject.RedPacketId + ",1)' style='cursor:pointer;'>开启广告红包</div>";
            else if (obj == '1')
                return "<div onclick='EditDefault(" + rowObject.RedPacketId + ",0)' style='cursor:pointer;color:#882323'>禁用广告红包</div>";
            else if (obj == '2')
                return "不可操作";
        }, false);
    grid.jqGrid.AddColumn("", "删除操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='DelRedPacket(" + rowObject.RedPacketId + ")' style='cursor:pointer;color:#882323'>删除</div>";
        }, false);
    grid.jqGrid.CreateTable();   
}

function SelectRedPacketList(obj) {
    console.log(obj)
    window.location.href = "CardRedPacketListBrowse.aspx?RedPacketId=" + obj;
}

function DelRedPacket(RedPacketId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/DelCardRedPacket?RedPacketId=" + RedPacketId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            console.log(data)
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
                                        window.location.href = "CardRedPacketBrowse.aspx";
                                    }
                                }
                        }
                });
            }
            else {
        bootbox.dialog({
            message: data.Message,
            buttons:
                {
                    "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                load_hide();
                            }
                        }
                }
        });
            }
       
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function EditDefault(RedPacketId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/TurnOffCardRedPacket?status=" + status+"&RedPacketId=" + RedPacketId + "&token=" + _Token,
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
                                        window.location.href = "CardRedPacketBrowse.aspx";
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



function NewSystemMessage() {
    window.location.href = "CardRedPacketCreate.aspx?RedPacketId=0";
    return false;
}

function gotoRplist() {
    window.location.href = "CardRedPacketListBrowse.aspx?RedPacketId=0";
    return false;
}



function DeleteSuggestion() {
   /**
    *  var id = $("#CardRedPacketList").jqGrid('getGridParam', 'selarrrow');
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
