<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.ProductIMG" %>
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
    <div class="ProductIMG">
        <!--<div class="back"><div></div></div>-->
        <div class="images" style="background-image:url(<%=imgstr%>); background-size:cover; background-position:center center"></div>
        <img class="codeQR" src="<%=QRimg %>"" />
        <div class="title"><%=InfoVO.Title %></div>
        <div class="NameInfo">
            <div class="Name"><%=PersonalVO.Name %></div>
            <ul class="InfoList">
            	<li <%if(PersonalVO.Phone=="") { %>style="display:none"<%} %>>
                    <div class="left">手　　机 ：</div>
                    <div class="right"><%=PersonalVO.Phone %></div>
                </li>
                <li <%if(BusinessCardVO.BusinessName=="") { %>style="display:none"<%} %>>
                    <div class="left">公　　司 ：</div>
                    <div class="right"><%=BusinessCardVO.BusinessName %></div>
                </li>
                <li <%if(PersonalVO.Email=="") { %>style="display:none"<%} %>>
                    <div class="left">电子邮箱 ：</div>
                    <div class="right"><%=PersonalVO.Email %></div>
                </li>
                <li <%if(PersonalVO.Business=="") { %>style="display:none"<%} %>>
                    <div class="left">主营业务 ：</div>
                    <div class="right"><%=PersonalVO.Business %></div>
                </li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
