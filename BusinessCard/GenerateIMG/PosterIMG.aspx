<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PosterIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.PosterIMG" %>
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
    <div class="PosterIMG" style=" background-image:url(<%=Posterback%>)">
        <div class="Content">
            <div class="NameInfo">
                <%if (PersonalVO.Headimg != ""){ %>
                    <div class="headimg"  style="background-image: url(<%=PersonalVO.Headimg%>)"></div>
                <%}else{%>
                    <div class="headimg"  style="background-image: url(http://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg)"></div>
                <%} %>
                <div class="right">
                    <div class="Name"><%=PersonalVO.Name %></div>
                    <div class="Position"><%=PersonalVO.Position %></div>
                </div>
            </div>
            <ul class="InfoList">
            	<li <%if(PersonalVO.Phone=="") { %>style="display:none"<%} %>>
                	<img src="http://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/phone.png" />
                    <div class="text"><%=PersonalVO.Phone %></div>
                </li>
            	<li <%if(BusinessName=="") { %>style="display:none"<%} %>>
                	<img src="http://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/company.png" />
                    <div class="text"><%=BusinessName %></div>
                </li>
                <li <%if(PersonalVO.WeChat=="") { %>style="display:none"<%} %>>
                	<img src="http://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/wxcat.png" />
                    <div class="text"><%=PersonalVO.WeChat %></div>
                </li>
                <li <%if(PersonalVO.Email=="") { %>style="display:none"<%} %>>
                	<img src="http://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/email.png" />
                    <div class="text"><%=PersonalVO.Email %></div>
                </li>
                <li <%if(PersonalVO.Business=="") { %>style="display:none"<%} %>>
                	<img src="http://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/main_business.png" />
                    <div class="text on"><%=PersonalVO.Business %></div>
                </li>
            </ul>
        </div>
        <div class="QRimg">
            <img src="<%=PersonalVO.QRimg %>" />
        </div>
    </div>
    </form>
</body>
</html>
