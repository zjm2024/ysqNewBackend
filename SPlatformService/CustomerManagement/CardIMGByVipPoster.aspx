<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMGByVipPoster.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardIMGByVipPoster" %>

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
    <div class="CardPoster" style=" width:500px; height:762px; background-image:url(https://www.zhongxiaole.net/SPManager/Style/images/wxcard/vipPoster2.jpg); position:relative">
        <img class="codeQR" src="<%=QRimg %>" style="width: 190px;height: 190px;position: absolute;top: 306px;right: 155px;"/>
        <div class="Headimg" style="width: 74px;height: 74px;position: absolute;top: 625px;right: 366px;border-radius: 100vw; background:url(<%=Headimg %>) no-repeat center center; background-size:cover"></div>
        <div class="Name" style="    width: 140px;
    position: absolute;
    top: 705px;
    left: 27px;
    font-size: 26px;
    color: #ffffff;
    font-weight: bold;
    text-align: center;text-overflow:ellipsis;white-space:nowrap;overflow:hidden;
}"><%=Name %></div>
    </div>
    </form>
</body>
</html>
