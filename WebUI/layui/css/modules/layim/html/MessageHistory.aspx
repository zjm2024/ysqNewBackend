<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageHistory.aspx.cs" Inherits="WebUI.layui.css.modules.layim.html.MessageHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>众销乐-资源共享众包销售平台</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>"></script>
    <script src="<%= ResolveUrl("~/layui/layui.js")%>" type="text/javascript"></script>
    <link rel="stylesheet" href="<%= ResolveUrl("~/layui/css/layui.css")%>" />
    <style>
        body .layim-chat-main {
            height: auto;
        }
    </style>
    <script type="text/javascript">
        var _RootPath = "<%= ResolveUrl("~")%>";
        var _Token = "<%= Token%>";
        window.onload = function () {
            var mineId = parent.layui.layim.cache().mine.id;
            var friendId = GetQueryString("id");
            $.ajax({
                url: _RootPath + "SPWebAPI/SPIM/GetIMMessageHistory?fromId=" + mineId + "&toId=" + friendId + "&pageIndex=1&token=" + _Token,
                type: "GET",
                data: null,
                success: function (data) {
                    layui.use(['layim', 'laypage'], function () {
                        var layim = layui.layim
                        , layer = layui.layer
                        , laytpl = layui.laytpl
                        , $ = layui.jquery
                        , laypage = layui.laypage;

                        //聊天记录的分页此处不做演示，你可以采用laypage，不了解的同学见文档：http://www.layui.com/doc/modules/laypage.html
                        //laypage.render({
                        //    elem: 'LAY_page'
                        //    , count: 50 //数据总数，从服务端得到
                        //    , limit:10
                        //    , jump: function (obj, first) {
                        //        //obj包含了当前分页的所有参数，比如：
                        //        console.log(obj.curr); //得到当前页，以便向服务端请求对应页的数据。
                        //        console.log(obj.limit); //得到每页显示的条数

                        //        //首次不执行
                        //        if (!first) {
                        //            //do something
                        //        }
                        //    }
                        //});


                        //开始请求聊天记录
                        var param = location.search //获得URL参数。该窗口url会携带会话id和type，他们是你请求聊天记录的重要凭据
                        //实际使用时，下述的res一般是通过Ajax获得，而此处仅仅只是演示数据格式
                        , res = data

                        //console.log(param)

                        var html = laytpl(LAY_tpl.value).render({
                            data: res.data
                        });
                        $('#LAY_view').html(html);

                    });
                },
                error: function (data) {
                    //alert(data);
                }
            });
            
        }

        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layim-chat-main">
            <ul id="LAY_view"></ul>
        </div>

        <div id="LAY_page" style="margin: 0 10px;"></div>


        <textarea title="消息模版" id="LAY_tpl" style="display:none;">
{{# layui.each(d.data, function(index, item){
  if(item.id == parent.layui.layim.cache().mine.id){ }}
    <li class="layim-chat-mine"><div class="layim-chat-user"><img src="{{ item.avatar }}"><cite><i>{{ layui.data.date(item.timestamp) }}</i>{{ item.username }}</cite></div><div class="layim-chat-text">{{ layui.layim.content(item.content) }}</div></li>
  {{# } else { }}
    <li><div class="layim-chat-user"><img src="{{ item.avatar }}"><cite>{{ item.username }}<i>{{ layui.data.date(item.timestamp) }}</i></cite></div><div class="layim-chat-text">{{ layui.layim.content(item.content) }}</div></li>
  {{# }
}); }}
</textarea>
    </form>
</body>
</html>
