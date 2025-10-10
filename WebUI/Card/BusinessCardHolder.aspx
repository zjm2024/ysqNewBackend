<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessCardHolder.aspx.cs" Inherits="WebUI.Card.BusinessCardHolder" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Style/css/card.css" rel="stylesheet" />
    <link href="../Style/css/card_style.css" rel="stylesheet" />
    <title>名片夹</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="card-item touch-btn">
      <%for (int i=0;i<list.Count;i++){ %>
      <a href="ShowCard.aspx?CardID=<%=list[i].CardID %>">
      <div class='cardlist-wrap'>
        <div class='infoView'>
          <div class='headimgView Holderheadimg'>
            <%if (list[i].Headimg != ""){ %>
              <div class='headimg'> <img src="<%=list[i].Headimg %>"/> </div>
            <%}else{%>
              <div class='headimg'> <img src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg"/> </div>
            <%} %>
          </div>
          <div class='info'>
            <div class='name'>
              <div class="_name"><%=list[i].Name %></div>
            </div>
            <div class='text'><%if (list[i].CorporateName != ""){%><%=list[i].CorporateName %><%}else{ %><%=list[i].Position %><%} %></div>
          </div>
        </div>
      </div>
            </a>
      <%} %>
    </div>
    <%if (count > pageInfo.PageCount){%>
    <div class='paging'>
        <a href="?PageIndex=1">
        <div class='li-page max-page iconfont  pre iconicon_page_top'></div>
        </a>
        <%if (pageInfo.PageIndex == 1){%>
            <div class='li-page switch-page iconfont  pre iconicon_page_next'></div>
        <%}else{ %>
            <a href="?PageIndex=<%=pageInfo.PageIndex - 1 %>"><div class='li-page switch-page iconfont  pre iconicon_page_next'></div></a>
        <%} %>
        <div class="picker" class="pageText"><%=pageInfo.PageIndex %>/<%=(count / pageInfo.PageCount) - 1 %></div>
         <%if (pageInfo.PageIndex == (count / pageInfo.PageCount) - 1){%>
            <div class='li-page switch-page  iconfont next iconicon_page_next'></div>
        <%}else{ %>
            <a href="?PageIndex=<%=pageInfo.PageIndex + 1 %>"><div class='li-page switch-page  iconfont next iconicon_page_next'></div></a>
        <%} %>
        
        <a href="?PageIndex=<%=(count / pageInfo.PageCount) - 1 %>">
            <div class='li-page max-page iconfont  next iconicon_page_top'></div>
        </a>
    </div>
    <%} %>
    <div class="footer_top"></div>
    <div class="footer">
      <ul>
        <li><a href="CardIndex.aspx">我的名片</a></li>
        <li><a href="BusinessCardHolder.aspx">名片夹</a></li>
        <li><a href="MyCenter.aspx">个人中心</a></li>
      </ul>
    </div>
    </form>
</body>
</html>
