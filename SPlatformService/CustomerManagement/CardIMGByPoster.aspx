<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMGByPoster.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardIMGByPoster" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Style/css/card.css")%>" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="CardPoster" style=" width:750px; height:1333px; background-image:url(https://www.zhongxiaole.net/SPManager/Style/images/wxcard/CardPoster.jpg); position:relative">
        <img class="codeQR" src="<%=QRimg %>" style="width:210px; height:210px; position:absolute; top:1003px; right:72px;"/>
    </div>
    </form>
</body>
</html>
