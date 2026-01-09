<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMG2QR.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardIMG2QR" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Style/css/card.css")%>" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="WrapQR <%=WrapQR1 %>">
        	<div class="headDiv">
        		<div class="headimg" style="background-image:url(<%=Headimg %>)" ></div>
        		<div class="headname"><%=Name %></div>
        		<div class="headname2"><%=Position %></div>
        	</div>
            <img class="cardimg" src="<%=CardImg %>" />
            <div class="footTips">扫一扫上面的二维码图案，收藏我的名片</div>
        </div>
   
  		<div class="WrapQR <%=WrapQR2 %>">
        	<div class="headDiv">
        		<!--<div class="cardgroup">乐聊名片群：</div>-->
        		<div class="headtitle"><%=Title %></div>
        	</div>
            <img class="cardimg" src="<%=CardImg %>" />
            <div class="footTips">微信扫码进<font style="color:#ff6a00"><%if (AppType == 3){ %>微云智推名片群<%}else{ %>乐聊名片群<%} %></font>，搜索对接资源</div>
        </div>
         <div class="WrapQR <%=WrapQR3 %>">
        	<div class="headDiv">
        		<div class="headimg" style="background-image:url(<%=Headimg %>)" ></div>
        		<div class="headtitle on"><%=Title %></div>
        	</div>
            <img class="cardimg" src="<%=CardImg %>" />
            <div class="footTips"><%=bottext %></div>
        </div>
   
    </form>
</body>
    
</html>
