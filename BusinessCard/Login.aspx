<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BusinessCard.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>登录-乐聊名片企业版</title>
<link rel="stylesheet" type="text/css" href="public/css/style.css"/>
</head>
<body class="login-body">
    <form id="form1" runat="server">
        <div class="login-div">
    	    <div class="login">
                <img class="logo" src="public/images/logo2.png" />
                <div class="logoText">乐聊名片<font>企业版</font></div>
                <div class="wrapper"> 
                    <iframe src="ThirdLogin/WXLoginPage.aspx"></iframe>
                </div>
                <div class="text">请用<font>微信扫码</font>登录乐聊名片企业版</div>
            </div>
        </div>
    </form>
</body>
</html>
