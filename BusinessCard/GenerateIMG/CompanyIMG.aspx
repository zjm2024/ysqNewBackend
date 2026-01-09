<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.CompanyIMG" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="../css/css.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="CompanyIMG" style="background-image:url('../images/app/Company.png')">
            <div class="headimg"  style="background-image:url('<%=Headimg %>')"></div>
        </div>
    </form>
</body>
</html>
