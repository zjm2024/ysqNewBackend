var MessageTo = _MessageTo;//当前接受人
$(function () {
    init();
});
function init() {//初始化函数
    GetAllFriend();
    GetLatelyFriend();
    if (MessageTo != 0) {
        ChatWindow(MessageTo);
    } else {
        $("#left_menu").attr("checked",'true');
    }
}
function GetAllFriend() {//获取全部联系人
    var divData = $("div[id*='All_contact']");
    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/GetAllFriend?Token=" + _Token,
        type: "get",
        success: function (data) {
            if (data.Flag == 1) {
                var FriendList = data.Result;

                var str = "<ul class=\"contact_list\">\r\n";
                for (var i = 0; i < FriendList.length; i++) {
                    var Friend = FriendList[i];

                    str += "<li id=\"contact_li_" + Friend.FriendTo + "\" onclick=\"SelectFriend(" + Friend.FriendTo + ",'" + remakeName(Friend.ToCustomerName) + "')\">\r\n";
                    str += "    <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + Friend.ToHeaderLogo + ")\"></div></div>\r\n";
                    str += "    <div class=\"user_info\">\r\n";
                    str += "        <div class=\"uesr_name\">\r\n";
                    str += "            " + remakeName(Friend.ToCustomerName) + "\r\n";
                    str += "        </div>\r\n";
                    str += "    </div>\r\n";
                    str += "</li>\r\n";
                }
                str += "</ul>\r\n";
                divData.html(str);
            } else {
                console.log(data);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetLatelyFriend() {//获取最近联系人
    var divData = $("div[id*='Recent_contact']");

    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/GetLatelyMessagaeByCustomer?limit=20&Token=" + _Token,
        type: "get",
        success: function (data) {
            if (data.Flag == 1) {
                var FriendList = data.Result;

                var strul = "<ul class=\"contact_list\">\r\n";
                var str = "";
                var ifMessageTo = false;//当前接受人是否在最近联系人里
                for (var i = 0; i < FriendList.length; i++) {
                    var Friend = FriendList[i];
                    //console.log(Friend);
                    if (Friend.MessageFrom == _CustomerId) {
                        HeaderLogo = Friend.ToHeaderLogo;
                        CustomerName = Friend.ToCustomerName;
                        MessageToID = Friend.MessageTo;
                    } else {
                        HeaderLogo = Friend.MeHeaderLogo;
                        CustomerName = Friend.MeCustomerName;
                        MessageToID = Friend.MessageFrom;
                    }

                    if (MessageToID == 0 && Friend.MessageID == MessageTo) {
                        str += "<li class=\"on\" id=\"Lately_li_" + Friend.MessageID + "\" onclick=\"SelectSYS(" + Friend.MessageID + ",'" + remakeName(Friend.MessageType) + "')\">\r\n";
                    } else if (MessageToID == 0 && i == 0 && MessageTo==0) {
                        str += "<li class=\"on\" id=\"Lately_li_" + Friend.MessageID + "\" onclick=\"SelectSYS(" + Friend.MessageID + ",'" + remakeName(Friend.MessageType) + "')\">\r\n";
                        ChatSYSWindow(Friend.MessageID, Friend.MessageType);
                    } else if (MessageToID == 0) {
                        str += "<li id=\"Lately_li_" + Friend.MessageID + "\" onclick=\"SelectSYS(" + Friend.MessageID + ",'" + remakeName(Friend.MessageType) + "')\">\r\n";
                    }else if (MessageToID == MessageTo)
                    {
                        str += "<li class=\"on\" id=\"Lately_li_" + MessageToID + "\" onclick=\"SelectFriend(" + MessageToID + ",'" + remakeName(CustomerName) + "')\">\r\n";
                        ifMessageTo = true;
                    }else if (MessageTo == 0&&i==0) {
                        str += "<li class=\"on\" id=\"Lately_li_" + MessageToID + "\"  onclick=\"SelectFriend(" + MessageToID + ",'" + remakeName(CustomerName) + "')\">\r\n";
                        ifMessageTo = true;
                        MessageTo = MessageToID;
                        ChatWindow(MessageToID);
                    } else {
                        str += "<li id=\"Lately_li_" + MessageToID + "\"  onclick=\"SelectFriend(" + MessageToID + ",'" + remakeName(CustomerName) + "')\">\r\n";
                    }
                    str += "    <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + HeaderLogo + ")\"></div></div>\r\n";
                    str += "    <div class=\"user_info\">\r\n";
                    str += "        <div class=\"uesr_name\">\r\n";
                    str += "            " + remakeName(CustomerName) + "\r\n";
                    str += "        </div>\r\n";
                    str += "        <div class=\"uesr_message\">\r\n";
                    str += "            " + Friend.Message + "\r\n";
                    str += "        </div>\r\n";
                    str += "        <div class=\"message_time\">\r\n";
                    str += "            " + formatMsgTime(Friend.SendAt) + "\r\n";
                    str += "        </div>\r\n";
                    if (Friend.UnreadCount > 0) {
                        str += "    <div class=\"message_UnreadCount\">\r\n";
                        str += "         " + Friend.UnreadCount + "\r\n";
                        str += "    </div>\r\n";
                    }
                    str += "    </div>\r\n";
                    str += "</li>\r\n";
                }
                if (!ifMessageTo && MessageTo > 5) {
                    GetCustomer(MessageTo, function (data2) {
                        if (data2.Flag == 1) {
                            var Mstr = "<li class=\"on\" id=\"Lately_li_" + MessageTo + "\" onclick=\"SelectFriend(" + MessageTo + ",'" + remakeName(data2.Result.CustomerName) + "')\">\r\n";
                            Mstr += "    <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + data2.Result.HeaderLogo + ")\"></div></div>\r\n";
                            Mstr += "    <div class=\"user_info\">\r\n";
                            Mstr += "        <div class=\"uesr_name\">\r\n";
                            Mstr += "            " + remakeName(data2.Result.CustomerName) + "\r\n";
                            Mstr += "        </div>\r\n";
                            Mstr += "        <div class=\"uesr_message\">\r\n";
                            Mstr += "        </div>\r\n";
                            Mstr += "        <div class=\"message_time\">\r\n";
                            Mstr += "        </div>\r\n";
                            Mstr += "    </div>\r\n";
                            Mstr += "</li>\r\n";
                            str = strul + Mstr + str;
                            str += "</ul>\r\n";
                            divData.html(str)
                        }
                    });
                } else {
                    str = strul+str+"</ul>\r\n";
                    divData.html(str)
                }
                ;
            } else {
                console.log(data);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function ChatWindow(Customerid) {//初始化聊天窗口，参数为聊天对象ID
    GetCustomer(Customerid, function (data) {
        $("#ChatName").html(remakeName(data.Result.CustomerName));
    });
    $(".zxtim_right").removeClass("onsys");

    var ListObj = $("#im_content_list");
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = 1;
    pageInfoObj.PageCount = 50;
    pageInfoObj.SortName = "SendAt";
    pageInfoObj.SortType = "desc";

    filterObj.groupOp = "or";
    var rulesObj = new Array();
    filterObj.rules = rulesObj;
    var filterArray = new Array();
    filterObj.filter = filterArray;

    var ruleObj = new Object();
    rulesObj.push(ruleObj);

    ruleObj.field = "MessageFrom";
    ruleObj.op = "eq";
    ruleObj.data = Customerid;

    ruleObj = new Object();
    rulesObj.push(ruleObj);
    ruleObj.field = "MessageTo";
    ruleObj.op = "eq";
    ruleObj.data = Customerid;

    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/GetMessageListByCustomer?Token=" + _Token,
        type: "post",
        data:filterModel,
        success: function (data) {
            //console.log(data);
            if (data.Flag == 1) {
                var ChatList = data.Result;
                var str = "";
                for (var i = 0; i < ChatList.length; i++) {
                    var Chat = ChatList[i];

                    var listr = "";
                    if (Chat.MessageFrom == _CustomerId) {
                        listr += "<li class=\"chat_mine\">\r\n";
                    } else {
                        listr += "<li>\r\n";

                        if (Chat.Status ==0) {
                            UpdateRequireStatus(Chat.MessageID, 1);
                        }
                    }
                    listr += "     <div class=\"chat_user\">\r\n";
                    listr += "          <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + Chat.MeHeaderLogo + ")\"></div></div>\r\n";

                    if (Chat.MessageFrom == _CustomerId) {
                        listr += "           <div class=\"chat_info MessageStatus" + Chat.Status + "\"><i>" + formatMsgTime(Chat.SendAt) + "</i>" + remakeName(Chat.MeCustomerName) + "</div>\r\n";
                    } else {
                        listr += "           <div class=\"chat_info\">" + remakeName(Chat.MeCustomerName) + "<i>" + formatMsgTime(Chat.SendAt) + "</i></div>\r\n";
                    }
                    listr += "     </div>\r\n";
                    listr += "     <div class=\"chat_text\">" +FaceConvert(Chat.Message) + "</div>\r\n";
                    listr += "</li>\r\n";
                    str = listr + str;
                }
                ListObj.html(str);
                $('#im_content').scrollTop($('#im_content')[0].scrollHeight);
            } else {
                ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            }
        },
        error: function (data) {
            ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            //console.log(data);
        }
    });
}
function ChatSYSWindow(MessageTypeId, MessageTypeName) {//初始化系统消息窗口，参数为消息类型ID
    $("#ChatName").html(MessageTypeName);
    $(".zxtim_right").addClass("onsys");

    var ListObj = $("#im_content_list");
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = 1;
    pageInfoObj.PageCount = 50;
    pageInfoObj.SortName = "SendAt";
    pageInfoObj.SortType = "desc";

    filterObj.groupOp = "and";
    var rulesObj = new Array();
    filterObj.rules = rulesObj;
    var filterArray = new Array();
    filterObj.filter = filterArray;

    var ruleObj = new Object();
    rulesObj.push(ruleObj);

    ruleObj.field = "MessageTypeId";
    ruleObj.op = "eq";
    ruleObj.data = MessageTypeId;

    console.log(filterModel);
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetMyMessageList?Token=" + _Token,
        type: "post",
        data: filterModel,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                var ChatList = data.Result;
                var str = "";
                for (var i = 0; i < ChatList.length; i++) {
                    var Chat = ChatList[i];

                    var listr = "";
                    listr += "<li class=\"SYSMessageli\">\r\n";
                    listr += "    <div class=\"SendAt\">" + formatMsgTime(Chat.SendAt) + "</div>\r\n";
                    listr += "    <div class=\"SYSMessageDIV\">";
                    listr += "         <div class=\"SYSMessageTitle\">" + Chat.Title + "</div>";
                    listr += "         <div class=\"SYSMessageTime\">" + Chat.SendAt + "</div>";
                    listr += "         <div class=\"SYSMessageContent\">" + Chat.Message + "</div>";
                    listr += "    </div>\r\n";
                    listr += "</li>\r\n";
                    str = listr + str;
                }
                ListObj.html(str);
                $('#im_content').scrollTop($('#im_content')[0].scrollHeight);

                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/UpdateMessageStatusByMessageTypeId?MessageTypeId=" + MessageTypeId + "&Token=" + _Token,
                    type: "get",
                    success: function (data) {
                        console.log(data);
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
            } else {
                ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            }
        },
        error: function (data) {
            ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            //console.log(data);
        }
    });
}
function SelectFriend(MessageToID,name) {//切换聊天对象
    $("#Recent_contact li").removeClass("on");
    $("#Lately_li_" + MessageToID).addClass("on");
    $("#All_contact li").removeClass("on");
    $("#contact_li_" + MessageToID).addClass("on");

    $("#ChatName").html(name);
    $("#im_content_list").html("<div class=\"sysmessage\">正在获取聊天记录</div>");
    MessageTo = MessageToID;
    ChatWindow(MessageToID);
    $("#left_menu").removeAttr("checked");
    GetLatelyFriend();//刷新联系人列表
    RecordClose();//关闭聊天记录窗口
}
function SelectSYS(MessageToID, name) {//切换到系统消息
    $("#Recent_contact li").removeClass("on");
    $("#Lately_li_" + MessageToID).addClass("on");
    $("#All_contact li").removeClass("on");
    $("#contact_li_" + MessageToID).addClass("on");

    $("#ChatName").html(name);
    $("#im_content_list").html("<div class=\"sysmessage\">正在获取聊天记录</div>");
    MessageTo = MessageToID;
    ChatSYSWindow(MessageToID, name);
    $("#left_menu").removeAttr("checked");
    GetLatelyFriend();//刷新联系人列表
    RecordClose();//关闭聊天记录窗口
}
function UpdateRequireStatus(MessageID, status) {//更改聊天信息状态
    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/UpdateRequireStatus?MessageID=" + MessageID + "&status=" + status + "&Token=" + _Token,
        type: "post",
        success: function (data) {
            //console.log(data);
        },
        error: function (data) {
            //console.log(data);
        }
    });
}
function GetCustomer(Customerid,fun) {//获取会员信息
    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/GetCustomer?customerId=" + Customerid + "&Token=" + _Token,
        type: "get",
        success: function (data) {
            fun(data);
        },
        error: function (data) {
            fun(data);
        }
    });
}
var SendID = 0;
function Send(){//发送文本消息
    var textObj = $("textarea[name=im_textarea]");
    var ListObj = $("#im_content_list");
    if(textObj.val()=="")
    {
        return;
    }
    var SendText = getFormatCode(textObj.val());
    SendID += 1;
    var str = "<li class=\"chat_mine\">\r\n";
    str += "     <div class=\"chat_user\">\r\n";
    str += "          <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + _HeaderLogo + ")\"></div></div>\r\n";
    str += "           <div id=\"#Send_" + SendID + "\" class=\"chat_info MessageStatus0\"><i>" + formatMsgTime(new Date(Date.now())) + "</i>" +  remakeName(_CustomerName) + "</div>\r\n";
    str += "     </div>\r\n";
    str += "     <div class=\"chat_text\">" + FaceConvert(SendText) + "</div>\r\n";
    str += "</li>\r\n";

    ListObj.html(ListObj.html() + str);
    $('#im_content').scrollTop($('#im_content')[0].scrollHeight);

    var seller_data = {
        "MessageTo": MessageTo,
        "MessageType": "text",
        "Message": SendText
    }
    
    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/AddMessage?Token=" + _Token,
        type: "POST",
        data: seller_data,
        success: function (data) {
            if (data.Flag == 1) {
                ChatWindow(MessageTo);
                GetLatelyFriend();
            } else {
                $("#Send_" + SendID).removeClass("MessageStatus0");
                $("#Send_" + SendID).addClass("MessageStatus2");
            }
            textObj.val("");
        },
        error: function (data) {
            $("#Send_" + SendID).removeClass("MessageStatus0");
            $("#Send_" + SendID).addClass("MessageStatus2");
        }
    });
}
$(document).keydown(function (event) {
    var ctrlKey = event.ctrlKey || event.metaKey;
    if (ctrlKey && event.keyCode == 13) {
        var str = $("textarea[name=im_textarea]").val();
        $("textarea[name=im_textarea]").val(str + "\n");
    } else if (event.keyCode == 13) {
        // 阻止提交自动换行
        event.preventDefault();
        Send();
    }
});

var ws;
$().ready(function () {
    var WebSocketurl = "";
    if (document.location.protocol == "https:") {
        WebSocketurl = "wss";
    } else {
        WebSocketurl = "ws";
    }
    ws = new WebSocket(WebSocketurl+"://" + window.location.hostname + ":" + window.location.port + "/SPWebAPI/ZXTIM/OpenWebSocket?Token=" + _Token);
    console.log("正在连接");

    ws.onopen = function () {
        console.log("已经连接");
    }
    ws.onmessage = function (evt) {
        if (evt.data == "NewMessage") {
            ChatWindow(MessageTo);
            GetLatelyFriend();
        }
        if (evt.data == "AnotherPlace") {
            window.opener = null;
            window.close();
        }
        console.log(evt.data);
    }
    ws.onerror = function (evt) {
        console.log(evt);
    }
    ws.onclose = function (evt) {
        console.log("已经关闭");
        location.reload();
    }
});

function zxtim_left_tab(id, obj) {//切换左边栏
    $(".zxtim_left_tab ul li").removeClass("on");
    $(obj).addClass("on")
    $(".Recent_contact").hide();
    $("#" + id).show();
}
var isFullScreen = false;
function Full_screen() {//放大缩小
    if (isFullScreen) {
        $(".zxtim").removeClass("Full_screen");
        $(".Full_screen_ico").removeClass("un");
        isFullScreen = false;
    } else {
        $(".zxtim").addClass("Full_screen");
        $(".Full_screen_ico").addClass("un");
        isFullScreen = true;
    }
}
var isRecord = false;
function Record() {//打开关闭聊天记录
    if (!isRecord) {
        RecordOpen();
    } else {
        RecordClose();
    }
}
var str = "";
var RecordPageIndex = 1;
function RecordOpen() {//打开
    $(".im_div").addClass("onRecord");
    $(".Records_div").addClass("onRecord");
    isRecord = true;
    str = "";
    RecordPageIndex = 1;
    $(".loading_text").hide();
    ChatRecord(MessageTo);
}
function RecordClose() {//关闭
    $(".im_div").removeClass("onRecord");
    $(".Records_div").removeClass("onRecord");
    isRecord = false;
    $(".Records_list").html("<div class=\"sysmessage\">正在加载 . . .</div>");
    str = "";
    RecordPageIndex = 1;
    $(".loading_text").hide();
}

function ChatRecord(Customerid) {//打开聊天记录
    GetCustomer(Customerid, function (data) {
        $("#ChatName").html(remakeName(data.Result.CustomerName));
    });

    var ListObj = $("#Records_list");
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = RecordPageIndex;
    pageInfoObj.PageCount = 50;
    pageInfoObj.SortName = "SendAt";
    pageInfoObj.SortType = "desc";

    filterObj.groupOp = "or";
    var rulesObj = new Array();
    filterObj.rules = rulesObj;
    var filterArray = new Array();
    filterObj.filter = filterArray;

    var ruleObj = new Object();
    rulesObj.push(ruleObj);

    ruleObj.field = "MessageFrom";
    ruleObj.op = "eq";
    ruleObj.data = Customerid;

    ruleObj = new Object();
    rulesObj.push(ruleObj);
    ruleObj.field = "MessageTo";
    ruleObj.op = "eq";
    ruleObj.data = Customerid;

    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/GetMessageListByCustomer?Token=" + _Token,
        type: "post",
        data: filterModel,
        success: function (data) {
            //console.log(data);
            if (data.Flag == 1) {
                var ChatList = data.Result;
                var SendAtDay = "";
                for (var i = 0; i < ChatList.length; i++) {
                    var Chat = ChatList[i];
                    var SendAt = Chat.SendAt;
                    SendAt = SendAt.split("T");
                    var listr = "";
                    if (SendAtDay != SendAt[0]) {
                        listr += "<li class=\"SendAtDay\"><font>" + SendAt[0] + "</font></li>";
                        SendAtDay = SendAt[0];
                    }
                    if (Chat.MessageFrom == _CustomerId) {
                        listr += "<li class=\"chat_mine\">\r\n";
                    } else {
                        listr += "<li>\r\n";

                        if (Chat.Status == 0) {
                            UpdateRequireStatus(Chat.MessageID, 1);
                        }
                    }
                    listr += "     <div class=\"chat_user\">\r\n";
                    listr += "         <div class=\"chat_info\">" + remakeName(Chat.MeCustomerName) + "<i>" + SendAt[1] + "</i></div>\r\n";
                    listr += "     </div>\r\n";
                    listr += "     <div class=\"chat_text\">" + FaceConvert(Chat.Message) + "</div>\r\n";
                    listr += "</li>\r\n";
                    
                    str = str + listr;
                }
                
                if (ChatList.length > 0) {
                    $(".loading_text").show();
                } else {
                    $(".loading_text").hide();
                }
                ListObj.html(str);
                $('#im_content').scrollTop($('#im_content')[0].scrollHeight);
            } else {
                ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            }
        },
        error: function (data) {
            ListObj.html("<div class=\"sysmessage\">获取失败</div>");
            //console.log(data);
        }
    });
}
function imSearch(Keyword) {//搜索联系人
    var ObjSearchlist = $('#Search_list');
    if (Keyword == "") {
        ObjSearchlist.html("<p class=\"tishi\">请输入关键词搜索</p>");
        return;
    }
    $.ajax({
        url: _RootPath + "SPWebAPI/ZXTIM/SearchFriend?Keyword=" + Keyword + "&Token=" + _Token,
        type: "get",
        data: null,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                var FriendList = data.Result;

                var str = "<ul class=\"contact_list\">\r\n";
                for (var i = 0; i < FriendList.length; i++) {
                    var Friend = FriendList[i];

                    str += "<li onClick=\"window.location.href='?MessageTo=" + Friend.CustomerId + "'\">\r\n";
                    str += "    <div class=\"headimg\"><div class=\"headimg_div\" style=\"background-image:url(" + Friend.HeaderLogo + ")\"></div></div>\r\n";
                    str += "    <div class=\"user_info\">\r\n";
                    str += "        <div class=\"uesr_name c_name\">\r\n";
                    str += "            " + remakeName(Friend.CustomerName) + "\r\n";
                    str += "        </div>\r\n";
                    if (Friend.AgencyName != "") {
                        str += "    <div class=\"uesr_name\">\r\n";
                        str += "        销售名称：" + remakeName(Friend.AgencyName) + "\r\n";
                        str += "    </div>\r\n";
                    }
                    if (Friend.CompanyName != "") {
                        str += "    <div class=\"uesr_name\">\r\n";
                        str += "        雇主名称：" + remakeName(Friend.CompanyName) + "\r\n";
                        str += "    </div>\r\n";
                    }
                    str += "    </div>\r\n";
                    str += "</li>\r\n";
                }
                str += "</ul>\r\n";
                ObjSearchlist.html(str);
            } else {
                ObjSearchlist.html("<p class=\"tishi\">搜索不到相关信息</p>");
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
$(document).ready(function () {
    $("#Records_div").scroll(function () {//页面拉到底时自动加载更多
        var divHeight = $(this).height();
        var nScrollHeight = $(this)[0].scrollHeight;
        var nScrollTop = $(this)[0].scrollTop;
        if (nScrollTop + divHeight >= nScrollHeight) {
            RecordPageIndex += 1;
            ChatRecord(MessageTo);
        }
    });
});
function FaceConvert(text) {//表情转换
    text = text.replace(/\[微笑\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/0.gif\">");
    text = text.replace(/\[嘻嘻\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/1.gif\">");
    text = text.replace(/\[哈哈\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/2.gif\">");
    text = text.replace(/\[可爱\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/3.gif\">");
    text = text.replace(/\[可怜\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/4.gif\">");
    text = text.replace(/\[挖鼻\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/5.gif\">");
    text = text.replace(/\[吃惊\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/6.gif\">");
    text = text.replace(/\[害羞\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/7.gif\">");
    text = text.replace(/\[挤眼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/8.gif\">");
    text = text.replace(/\[闭嘴\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/9.gif\">");
    text = text.replace(/\[鄙视\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/10.gif\">");
    text = text.replace(/\[爱你\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/11.gif\">");
    text = text.replace(/\[泪\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/12.gif\">");
    text = text.replace(/\[偷笑\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/13.gif\">");
    text = text.replace(/\[亲亲\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/14.gif\">");
    text = text.replace(/\[生病\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/15.gif\">");
    text = text.replace(/\[太开心\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/16.gif\">");
    text = text.replace(/\[白眼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/17.gif\">");
    text = text.replace(/\[右哼哼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/18.gif\">");
    text = text.replace(/\[左哼哼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/19.gif\">");
    text = text.replace(/\[嘘\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/20.gif\">");
    text = text.replace(/\[衰\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/21.gif\">");
    text = text.replace(/\[委屈\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/22.gif\">");
    text = text.replace(/\[吐\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/23.gif\">");
    text = text.replace(/\[哈欠\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/24.gif\">");
    text = text.replace(/\[抱抱\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/25.gif\">");
    text = text.replace(/\[怒\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/26.gif\">");
    text = text.replace(/\[疑问\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/27.gif\">");
    text = text.replace(/\[馋嘴\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/28.gif\">");
    text = text.replace(/\[拜拜\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/29.gif\">");
    text = text.replace(/\[思考\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/30.gif\">");
    text = text.replace(/\[汗\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/31.gif\">");
    text = text.replace(/\[困\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/32.gif\">");
    text = text.replace(/\[睡\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/33.gif\">");
    text = text.replace(/\[钱\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/34.gif\">");
    text = text.replace(/\[失望\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/35.gif\">");
    text = text.replace(/\[酷\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/36.gif\">");
    text = text.replace(/\[色\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/37.gif\">");
    text = text.replace(/\[哼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/38.gif\">");
    text = text.replace(/\[鼓掌\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/39.gif\">");
    text = text.replace(/\[晕\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/40.gif\">");
    text = text.replace(/\[悲伤\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/41.gif\">");
    text = text.replace(/\[抓狂\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/42.gif\">");
    text = text.replace(/\[黑线\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/43.gif\">");
    text = text.replace(/\[阴险\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/44.gif\">");
    text = text.replace(/\[怒骂\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/45.gif\">");
    text = text.replace(/\[互粉\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/46.gif\">");
    text = text.replace(/\[心\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/47.gif\">");
    text = text.replace(/\[伤心\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/48.gif\">");
    text = text.replace(/\[猪头\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/49.gif\">");
    text = text.replace(/\[熊猫\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/50.gif\">");
    text = text.replace(/\[兔子\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/51.gif\">");
    text = text.replace(/\[ok\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/52.gif\">");
    text = text.replace(/\[耶\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/53.gif\">");
    text = text.replace(/\[good\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/54.gif\">");
    text = text.replace(/\[NO\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/55.gif\">");
    text = text.replace(/\[赞\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/56.gif\">");
    text = text.replace(/\[来\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/57.gif\">");
    text = text.replace(/\[弱\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/58.gif\">");
    text = text.replace(/\[草泥马\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/59.gif\">");
    text = text.replace(/\[神马\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/60.gif\">");
    text = text.replace(/\[囧\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/61.gif\">");
    text = text.replace(/\[浮云\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/62.gif\">");
    text = text.replace(/\[给力\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/63.gif\">");
    text = text.replace(/\[围观\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/64.gif\">");
    text = text.replace(/\[威武\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/65.gif\">");
    text = text.replace(/\[奥特曼\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/66.gif\">");
    text = text.replace(/\[礼物\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/67.gif\">");
    text = text.replace(/\[钟\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/68.gif\">");
    text = text.replace(/\[话筒\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/69.gif\">");
    text = text.replace(/\[蜡烛\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/70.gif\">");
    text = text.replace(/\[蛋糕\]/gi, "<img class=\"FaceIMG\" src=\"Style/images/zxtIM/face/71.gif\">");
    return text;
}
function face() {//打开表情
    var objface = $("#facelist");
    objface.fadeIn(200);
}
$(document).ready(function () {
    $("#facelist li").click(function (e){
        var im_textarea = $("#im_textarea");
        var text = im_textarea.val() + $(this).attr("title");
        im_textarea.val(text);
        $("#facelist").hide();
    });
});

$(document).bind('click', function (e) {
    var e = e || window.event; //浏览器兼容性 
    var elem = e.target || e.srcElement;
    while (elem) { //循环判断至跟节点，防止点击的是div子元素 
        if ((elem.id && elem.id == 'facelist') || elem.id == 'im_input_biaoqing') {
            return;
        }
        elem = elem.parentNode;
    }
    $("#facelist").hide(); //点击的不是div或其子元素 
});

function remakeName(CustomerName) { //检测会员名是否是手机号
    if (CustomerName.match(/^((1)+\d{10})$/)) {
        CustomerName = CustomerName.substring(0, 3) + "****" + CustomerName.substring(7, 11)
    }
    return CustomerName;
}
function formatMsgTime(timespan) {//时间转换

    var dateTime = new Date(timespan);

    var year = dateTime.getFullYear();
    var month = dateTime.getMonth() + 1;
    var day = dateTime.getDate();
    var hour = dateTime.getHours();
    var minute = dateTime.getMinutes();
    var second = dateTime.getSeconds();
    var now = new Date();
    var now_new = Date.parse(now.toDateString());  //typescript转换写法

    var milliseconds = 0;
    var timeSpanStr;

    milliseconds = now - dateTime;

    if (milliseconds <= 1000 * 60 * 1) {
        timeSpanStr = '刚刚';
    }
    else if (1000 * 60 * 1 < milliseconds && milliseconds <= 1000 * 60 * 60) {
        timeSpanStr = Math.round((milliseconds / (1000 * 60))) + '分钟前';
    }
    else if (1000 * 60 * 60 * 1 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24) {
        timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60)) + '小时前';
    }
    else if (1000 * 60 * 60 * 24 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24 * 15) {
        timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60 * 24)) + '天前';
    }
    else if (milliseconds > 1000 * 60 * 60 * 24 * 15 && year == now.getFullYear()) {
        timeSpanStr = month + '-' + day;
    } else {
        timeSpanStr = year + '-' + month + '-' + day;
    }
    return timeSpanStr;
};
var getFormatCode = function (strValue) {//textarea 换行
    return strValue.replace(/\r\n/g, '<br/>').replace(/\n/g, '<br/>').replace(/\s/g, ' ');
}
