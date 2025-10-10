<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpShow.aspx.cs" Inherits="WebUI.Card.SignUpShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../Style/css/card.css" rel="stylesheet" />
    <title>入场券</title>
</head>
<body class="SignUpShowBody">
    <form id="form1" runat="server">
        <div class="SignUpShow">
            <img src="<%=sVO.SignUpQRCodeImg %>" class="QRCodeImg"/>
            <div class="t1">长按识别二维码查看</div>
            <div class="t2">购票成功，请取票</div>
            <div class="t3">开场时间</div>
            <div class="t4"><%=sVO.StartTime.ToString("yyyy/MM/dd HH:mm") %></div>
        </div>
        <div class="SignUpText">返回<a href="PartyShow.aspx?PartyID=<%=sVO.PartyID %>&InviterCID=<%=sVO.InviterCID %>">活动详情</a>页面，可以查看入场券！</div>
    </form>
</body>
</html>
