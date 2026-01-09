<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowCard.aspx.cs" Inherits="WebUI.Card.ShowCard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <link href="../Style/css/card.css" rel="stylesheet" />
    <link href="../Style/css/card_style.css" rel="stylesheet" />
    <title><%if (cVO != null){ %><%=cVO.Name %>的名片<%}else{ %>乐聊名片<%} %></title>
    <%if (cVO != null){ %>
    <script>
        /*微信分享*/
       var dataForWeixin = {
           appId: '<%=ViewBag.appid%>',
           url: 'https://www.zhongxiaole.net/Card/ShowCard.aspx?CardID=<%=cVO.CardID%>',
                    jsapiTicket:'<%=ViewBag.jsapiTicket%>',
                    title: '<%=cVO.Name%>的名片',
                    <%if (cVO.Headimg != ""){%>
    	            imgUrl: '<%=cVO.Headimg%>',
                    <%}else{%>
                    imgUrl: 'https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg',
                    <% }%>
                    timestamp: '<%=ViewBag.timestamp%>',
                    nonceStr: '<%=ViewBag.nonceStr%>',
                    signature: '<%=ViewBag.signature%>',
                    jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage'],
                    callback: function () { }
                };
        console.log(dataForWeixin);
              wx.config({
                  debug: false,
                  appId: dataForWeixin.appId,
                  timestamp: dataForWeixin.timestamp,
                  nonceStr: dataForWeixin.nonceStr,
                  signature: dataForWeixin.signature,
                  jsApiList: dataForWeixin.jsApiList
              });

              wx.ready(function (res) {
                  wx.onMenuShareAppMessage({
                      title: dataForWeixin.title,
                      desc: "<%=cVO.CorporateName %>\n<%=cVO.Position %>",
                      link: dataForWeixin.url,
                      imgUrl: dataForWeixin.imgUrl,
                      trigger: function (res) { },
                      success: function (res) { console.log(res) },
                      cancel: function (res) { },
                      fail: function (res) { }
                  });
                  wx.onMenuShareTimeline({
                      title: dataForWeixin.title,
                      link: dataForWeixin.url,
                      imgUrl: dataForWeixin.imgUrl,
                      trigger: function (res) { },
                      success: function (res) { console.log(res) },
                      cancel: function (res) { },
                      fail: function (res) { }
                  });
              });
        /*微信分享结束*/
    </script>
    <%} %>
</head>
<body>
    <form id="form1" runat="server">
    <%if (cVO != null) { %>
    <div class='CardStyle<%=cVO.style %> cardback'>
      <div class="headimg-wrap">
        <%if (cVO.Headimg != "") { %>
            <div class='headimg' style="background-image: url('<%=cVO.Headimg%>');"></div>
        <%} else { %>
            <div class='headimg' style="background-image: url('https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg');"></div>
        <%} %>
      </div>
      <div class='CorporateName'>
        <div class="iconfont">
          <img class="" src="../Style/images/CardIcon/icon_Employer.png"/>
        </div>
        <div class='text'><%if (cVO.CorporateName != "") {%><%=cVO.CorporateName %><%} else {%>未填写公司<%}%></div>
      </div>
      <div class='nameinfo'>
        <div class='name'><%if (cVO.Name != "") {%><%=cVO.Name %><%} else {%>未填写<%}%></div>
        <div class='position'><%if (cVO.Position != "") {%><%=cVO.Position %><%} else {%>未填写职位<%}%></div>
      </div>
      <div class='card-line'></div>
      <div class='contactinfo'>
        <div class='li Business'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_business.svg"/>
          </div>
          <div class='text'><%if (cVO.Business != "") {%><%=cVO.Business %><%} else {%>未填写主营业务<%}%></div>
        </div>
        <div class='li tel-phone'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_tel_0.svg"/>
          </div>
          <div class='text'><%if (cVO.Tel != "" && cVO.isDisplayTel == 1) {%><%=cVO.Tel %><%} else if (cVO.Tel != "" && cVO.isDisplayTel == 0) {%>未公开固话<%} else if (cVO.Tel != "" && cVO.isDisplayTel == 2) {%>回递名片后显示<%} else {%>未填写固话<%}%></div>
        </div>
        <div class='li Tel'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_phone_0.svg"/>
          </div>
          <div class='text'><%if (cVO.Phone != "" && cVO.isDisplayPhone == 1) {%><%=cVO.Phone %><%} else if (cVO.Phone != "" && cVO.isDisplayPhone == 0) {%>未公开手机<%} else if (cVO.Phone != "" && cVO.isDisplayPhone == 2) {%>回递名片后显示<%} else {%>未填写手机<%}%></div>
        </div>
        <div class='li Email'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_email_0.svg"/>
          </div>
          <div class='text'><%if (cVO.Email != "" && cVO.isDisplayEmail == 1) {%><%=cVO.Email %><%} else if (cVO.Email != "" && cVO.isDisplayEmail == 0) {%>未公开邮箱<%} else if (cVO.Email != "" && cVO.isDisplayEmail == 2) {%>回递名片后显示<%} else {%>未填写邮箱<%}%></div>
        </div>
        <div class='li WebSite'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_analyze.svg"/>
          </div>
          <div class='text'><%if (cVO.WebSite != "") {%><%=cVO.WebSite %><%} else {%>未填写官网<%}%></div>
        </div>
        <div class='li Wechat'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/we_chat.svg"/>
          </div>
          <div class='text'><%if (cVO.WeChat != "" && cVO.isDisplayWeChat == 1) {%><%=cVO.WeChat %><%} else if (cVO.WeChat != "" && cVO.isDisplayWeChat == 0) {%>未公开微信<%} else if (cVO.WeChat != "" && cVO.isDisplayWeChat == 2) {%>回递名片后显示<%} else {%>未填写微信<%}%></div>
        </div>
        <div class='li Address'>
          <div class='iconfont'>
            <img class="" src="../Style/images/CardIcon/icon_ad_0.svg"/>
          </div>
          <div class='text'><%if (cVO.Address != "") {%><%=cVO.Address %><%} else {%>未填写办公地点<%}%></div>
        </div>
      </div>
    </div>
    <div class="card_read"> 
	    <div class="left"><span>浏览</span><font><%=cVO.ReadCount %></font><span>收藏</span><font><%=cVO.Collection %></font><span>转发</span><font><%=cVO.Forward %></font></div>
    </div>
    <!--
    <style>
        .btn-container{
            margin-top: 7.2vw;
            margin-bottom: 0vw;
        }
    </style>
    <div class='container'>
      <div class='li' style="margin-bottom:0">
        <div class='card-li fun'>
          <div class='mainbtn' style="margin:0 auto; padding-bottom:0">
              <a href="<%=cVO.QRImg %>">
              <div class="btn-container left">
                <div class='card-QR'>
                   <div class="iconfont icon iconcode"></div>
                </div>
              </div>
              </a>
              <a href="ShowCard.aspx?CardID=<%=cVO.CardID %>">
              <div class="btn-container right">
                <div class='index-share'>预览名片</div>
              </div>
              </a>
          </div>
        </div>
      </div>
    </div>
    -->
    <div class="li-wrap">
          <div class="li">
            <div class="title">基本信息</div>
            <div class="input-wrap">
              <div class="input-li">
                <div class="text">姓名</div>
                <div class="input"><%=cVO.Name %></div>
              </div>
              <div class="input-li">
                <div class="text">手机</div>
                <div class="input">
                  <%if (cVO.Phone != "" && cVO.isDisplayPhone == 1) {%><%=cVO.Phone %><%} else if (cVO.Phone != "" && cVO.isDisplayPhone == 0) {%>未公开手机<%} else if (cVO.Phone != "" && cVO.isDisplayPhone == 2) {%>回递名片后显示<%} else {%>未填写手机<%}%>
                </div>
              </div>
            </div>
          </div>
             <%if (cVO.Position != ""||cVO.CorporateName != ""||cVO.Address != ""||cVO.Business != ""){%>
          <div class="li">
            <div class="title">
              公司信息
            </div>
            <div class="input-wrap">
              <%if (cVO.Position != ""){%>
              <div class="input-li">
                <div class="text">职位</div>
                <div class="input"><%=cVO.Position %></div>
              </div>
              <%} %>
                <%if (cVO.CorporateName != ""){%>
              <div class="input-li">
                <div class="text">单位名称</div>
                <div class="input">
                  <%=cVO.CorporateName %>
                </div>
              </div>
                <%} %>
                <%if (cVO.Address != ""){%>
              <div class="input-li">
                <div class="text">办公地点</div>
                <div class="input"><%=cVO.Address %></div>
              </div>
                 <%} %>
                <%if (cVO.Business != ""){%>
              <div class="input-li">
                <div class="text">主营业务</div>
                <div class="input"><%=cVO.Business %></div>
              </div>
                <%} %>
            </div>
          </div>
        <%} %>
        <%if (cVO.WeChat != ""||cVO.Email != ""||cVO.Tel != ""||cVO.WebSite != ""){%>
          <div class="li">
            <div class="title">联系方式</div>
            <div class="input-wrap">
                <%if (cVO.WeChat != ""){%>
              <div class="input-li" >
                <div class="text">微信</div>
                <div class="input"><%=cVO.WeChat %></div>
              </div>
                <%} %>
                <%if (cVO.Email != ""){%>
              <div class="input-li" >
                <div class="text">邮箱</div>
                <div class="input"><%=cVO.Email %></div>
              </div>
                <%} %>
                <%if (cVO.Tel != ""){%>
              <div class="input-li">
                <div class="text">固定电话</div>
                <div class="input"><%=cVO.Tel %></div>
              </div>
                <%} %>
                <%if (cVO.WebSite != ""){%>
              <div class="input-li">
                <div class="text">官网</div>
                <div class="input"><%=cVO.WebSite %></div>
              </div>
                <%} %>
            </div>
          </div>
            <%} %>
            <%if (cVO.Details!=""){%>
              <div class="li">
                <div class="title">
                  个人简介
                </div>
                <div class="intro">
                    <%=cVO.Details %>
                </div>
              </div>
            <%} %>
    <%}else{ %>
        <div style="text-align:center; font-size:4vw; line-height:10vw;">该名片已被删除</div>
    <%} %>
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
