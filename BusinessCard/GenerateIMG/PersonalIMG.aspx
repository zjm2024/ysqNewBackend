<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.PersonalIMG" %>
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
        <div class="PersonalIMG" style="background-image:url('<%=PersonalVO.Headimg %>')">
            <!--
            <div class="CardBack">
                <div class="headimg"  style="background-image:url('<%=logo %>')"></div>
                <div class="headDiv">
        		    <div class="Name"><%=PersonalVO.Name %></div>
        		    <div class="Position"><%=PersonalVO.Position %></div>
                    <div class="Phone"><%=PersonalVO.Phone %></div>
        	    </div>
                <div class="info"></div>
            </div>
            <div class="btn">保存到通讯录</div>
                -->
        </div>
    </form>
</body>
</html>
