<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyCenter.aspx.cs" Inherits="WebUI.Card.MyCenter" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Style/css/MyCenter.css" rel="stylesheet" />
    <link href="../Style/css/card_style.css" rel="stylesheet" />
    <title>个人中心</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
      <div class="wrap">
        <div class="info-li circle-bg false">
          <div class="person-info">
            <div class="left-info">
              <div class="head-img iconfont"> <img class="" src="../Style/images/card/logo2.jpg" mode="aspectFill" /> </div>
            </div>
            <div class="right-info false">
              <div class="title-info">
                <div class="title">王林耿</div>
                <!--<img class="level-img" src="../Style/images/card/level-img.png"  mode="widthFix" />--> 
              </div>
              <div class="text">广州华顺青为信息科技有限责任公司</div>
            </div>
          </div>
          <div class="money-info">
            <div class="balance false">
              <div class="balance-float">￥</div>
              <div class="balance-int"><%=CardBalance.ToString().Split('.')[0] %></div>
              <div class="balance-float">.<%if (CardBalance.ToString().Split('.')[1] == null){%>00<%}else{ %><%=CardBalance.ToString().Split('.')[1] %><%} %></div>
            </div>
            <div class="btn">
              <a href="#"><div class="btn-text false">提现</div></a>
            </div>
            <div class="bottom false"> 帐户余额(元) </div>
          </div>
        </div>
        <a href="VipCenter.aspx">
        <div class="info-li">
          <div class="fun-li-wrap">
            <div class="fun-li">
              <div class="left iconfont icontuiguangjiangjin"></div>
              <div class="text">
                <div class="_text left-text"> 认证会员
                </div>
                <div class="_text right-text">了解/开通</div>
              </div>
              <div class="right iconfont iconicon_page_next"></div>
            </div>
        </a>
        <div class="info-li">
          <div class="fun-li-wrap">
            <div class="fun-li">
              <div class="left iconfont"> <img class="" src="../Style/images/icon/article.svg" mode="widthFix"  /> </div>
              <div class="text">
                <div class="_text left-text"> 文章中心
                  <div class="span">NEW</div>
                </div>
                <div class="_text right-text"></div>
              </div>
              <div class="right iconfont iconicon_page_next"></div>
            </div>
        <div class='fun-li'>
            <div class="left iconfont"> <img class="" src="../Style/images/icon/icon_help_2.svg" mode="widthFix"/> </div>
            <div class="text">
              <div class="_text left-text">使用帮助</div>
              <div class="_text right-text"></div>
            </div>
            <div class="right iconfont iconicon_page_next"></div>
            </div>
          </div>
        </div>
      </div>
    </div>
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
