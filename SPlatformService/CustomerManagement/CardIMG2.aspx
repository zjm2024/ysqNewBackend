<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMG2.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardIMG2" %>
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
        <div class="<%if(Style > 51){ Response.Write("defaultPoster");}%> CardPoster<%if(Style > 0){ Response.Write(Style);}%>  AppType<%=AppType %> " style="<%if(background !="" ){ Response.Write("background: url("+background+");");}%>">
            <div class="head">
                <div class="headimg" style="background-image:url(<%=CVO.Headimg%>)"></div>
                <div class="headinfo">
                    <div class="name"><span class="icon_l"></span><%=CVO.Name %><span class="icon_r"></span></div>
                    <div class="position"><%=CVO.Position %></div>
                </div>
            </div>
            <%if (CVO.CardImg != ""){%>
              <div class="QRCodeImg">
                 <img src="<%=CVO.CardImg %>" />
                  <div class="text">长按收藏我的名片</div>
              </div>
            <%}%>
            <%if (Style == 6){%>
              <div class="day"><%=DateTime.Now.ToString("dd")%></div>
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
              <div class="month"><%=DateTime.Now.ToString("MM.dd")%></div>
              <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
              <div class="Cyear"><%=Cyear%></div>
              <div class="Cmonth"><%=Cmonth%></div>
            <%}%>
            <%if (Style == 18){%>
              <div class="day"><%=DateTime.Now.ToString("MM/dd")%></div>
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
              <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
            <%}%>
            <%if (Style == 19){%>
              <div class="month"><%=DateTime.Now.ToString("MM")%></div>
              <div class="day"><%=DateTime.Now.ToString("dd")%></div>
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
            <%}%>
            <%if (Style == 20){%>
              
              <div class="date"><%=DateTime.Now.ToString("yyyy.MM.dd dddd")%></div>
            <div class="day"><%=DateTime.Now.ToString("dd")%></div>
              <div class="month"><%=Emonth %></div>
              
            <%}%>
            <%if (Style == 27){%>
            <div class="date">
                <div class="day"><%=DateTime.Now.ToString("yyyy/MM/dd")%></div>
                <div class="month"><%=Emonth %></div>
                <div class="ChineseDay">农历<%=ChineseDay%></div>
                <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
            </div>
            <%}%>
            <%if (Style == 28){%>
            <div class="date">
                <div class="day"><%=DateTime.Now.ToString("MM/dd")%></div>
                <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
            </div>
            <%}%>
            <%if (Style == 31){%>
            <div class="date">
              <div class="month"><%=DateTime.Now.ToString("MM")%></div>
              <div class="day"><%=DateTime.Now.ToString("dd")%></div>
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
            </div>
            <%}%>
            <%if (Style == 32){%>
            <div class="date">
              <div class="day"><%=DateTime.Now.ToString("dd")%></div>
              <div class="year"><%=DateTime.Now.ToString("yyyy.MM")%></div>
              <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
            </div>
            <%}%>
            <%if (Style == 33){%>
            <div class="date">
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
              <div class="day"><%=DateTime.Now.ToString("MM/dd")%></div>
            </div>
            <%}%>
            <%if (Style == 35){%>
            <div class="date">
                <div class="month"><%=DateTime.Now.ToString("MM")%>月</div>
                <div class="ChineseDay"><%=ChineseDay%></div>
                <div class="month2"><%=DateTime.Now.ToString("MM")%></div>
                <div class="day"><%=DateTime.Now.ToString("dd")%></div>
                <div class="week"><%=DateTime.Now.ToString("dddd")%></div>
            </div>
            <%}%>
            <%if (Style == 36){%>
            <div class="date"><%=DateTime.Now.ToString("yyyy/MM/dd  dddd")%></div>
            <%}%>
            <%if (Style == 38){%>
            <div class="date">
              <div class="year"><%=DateTime.Now.ToString("yyyy")%></div>
            </div>
            <%}%>
            <ul class="info">
                <%if (CVO.Phone != ""&& CVO.isDisplayPhone==1){%><li class="Phone"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.Phone %></div></li><%}%>
                <%if (CVO.CorporateName != ""){%><li class="CorporateName"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.CorporateName %></div></li><%}%>
                <%if (CVO.Address != ""){%><li class="Address"><div class="left"><div class="icon" ></div></div><div class="right oper"><%=CVO.Address %><div class="operation">导航</div></div></li><%}%>
                <%if (CVO.Tel != "" && CVO.isDisplayTel==1){%><li class="Tel"><div class="left"><div class="icon" ></div></div><div class="right oper"><%=CVO.Tel %><div class="operation on">拨打</div></div></li><%}%>
                <%if (CVO.Email != "" && CVO.isDisplayEmail==1){%><li class="Email"><div class="left"><div class="icon" ></div></div><div class="right"><%=CVO.Email %></div></li><%}%>
                <%if (CVO.WebSite != ""){%><li class="WebSite"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.WebSite %></div></li><%}%>
                <%if (CVO.WeChat != "" && CVO.isDisplayWeChat==1){%><li class="WeChat"><div class="left"><div class="icon" ></div></div><div class="right"><%=CVO.WeChat %></div></li><%}%>
                <%if (CVO.Business != ""){%><li class="Business"><div class="left"><div class="icon"></div></div><div class="right"><%=CVO.Business %></div></li><%}%>
            </ul>
        </div>
    </form>
</body>
</html>
