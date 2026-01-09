<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMG3.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardIMG3" %>
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
        <div class="CardPosterByBack" style="background-image:url(<%=Posterback%>)">
            <!--
            <%if(WeatherStatus){ %>
            <div class="Weather">
                <div class="top"><span><%=DateTime.Now.ToString("yyyy年MM月dd日")%></span><span><%=city%></span><span class="img" style="background-image:url(<%=WeatherImg%>)"></span></div>
                <div class="bottom"><span><%=weather%></span><span><%=temp%>°C</span></div>
            </div>
            <%} %>
                --->
            <%if (YanStatus)
                { %>
            <div class="Weather">
                <div class="top"><%=Yan.Utterance%></div>
                <div class="bottom">---<%=Yan.Author%></div>
            </div>
            <%} %>
            <div class="head">
                <div class="headimg" style="background-image:url(<%=CVO.Headimg%>)"></div>
            </div>
            <div class="bgBack">
                <div class="headinfo">
                    <div class="name"><span class="icon_l"></span><%=CVO.Name %><span class="icon_r"></span></div>
                    <div class="position"><%=CVO.Position %></div>
                </div>
                <%if (CVO.CardImg != ""){%>
                  <div class="QRCodeImg">
                     <img src="<%=CVO.CardImg %>" />
                  </div>
                  <div class="QRCodeText">递名片送祝福</div>
                <%}%>
                <ul class="info">
                    <%if (CVO.Phone != ""&& CVO.isDisplayPhone==1){%><li class="Phone"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.Phone %></div></li><%}%>
                    <%if (CVO.CorporateName != ""){%><li class="CorporateName"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.CorporateName %></div></li><%}%>
                    <%if (CVO.Address != ""){%><li class="Address"><div class="left"><div class="icon" ></div></div><div class="right oper"><%=CVO.Address %></div></li><%}%>
                    <%if (CVO.Tel != "" && CVO.isDisplayTel==1){%><li class="Tel"><div class="left"><div class="icon" ></div></div><div class="right oper"><%=CVO.Tel %></div></li><%}%>
                    <%if (CVO.Email != "" && CVO.isDisplayEmail==1){%><li class="Email"><div class="left"><div class="icon" ></div></div><div class="right"><%=CVO.Email %></div></li><%}%>
                    <%if (CVO.WebSite != ""){%><li class="WebSite"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.WebSite %></div></li><%}%>
                    <%if (CVO.WeChat != "" && CVO.isDisplayWeChat==1){%><li class="WeChat"><div class="left"><div class="icon" ></div></div><div class="right"><%=CVO.WeChat %></div></li><%}%>
                    <%if (CVO.Business != ""){%><li class="Business"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.Business %></div></li><%}%>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
