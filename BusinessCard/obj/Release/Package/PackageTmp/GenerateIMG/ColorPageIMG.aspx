<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ColorPageIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.ColorPageIMG" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<link href="../css/css.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="PersonalInfo">
            <div class="headimg"  style="background-image:url('<%=PersonalVO.Headimg%>')"></div>
            <div class="info">
                <div class="name"><%=PersonalVO.Name%></div>
                <div class="BusinessName"><%=BusinessCardVO.BusinessName%></div>
            </div>
        </div>
        <div class="Personal ColorPage" style="background-image:url('<%=QRimg %>')">
            <div class="headimg"  style="background-image:url('<%=BusinessCardVO.LogoImg%>')"></div>
        </div>
        <div class="QRtext">
            <div class="text">扫码查看我的<font>产品彩页</font></div>
        </div>
    </form>
</body>
</html>
