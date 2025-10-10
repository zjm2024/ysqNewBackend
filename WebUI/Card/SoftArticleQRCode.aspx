<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SoftArticleQRCode.aspx.cs" Inherits="WebUI.Card.SoftArticleQRCode" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../Style/css/card.css" rel="stylesheet" />
    <title><%=CardSoftArticleVO.Title%></title>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        /*微信分享*/
       var dataForWeixin = {
                    appId: '<%=ViewBag.appid%>',
                    url: '<%=ViewBag.url%>',
                    jsapiTicket:'<%=ViewBag.jsapiTicket%>',
                    title: '<%=CardSoftArticleVO.Title%>',
    	            imgUrl: '',
                    timestamp: '<%=ViewBag.timestamp%>',
                    nonceStr: '<%=ViewBag.nonceStr%>',
                    signature: '<%=ViewBag.signature%>',
                    jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage'],
                    callback: function () { }
                };
              wx.config({
                  debug: false,
                  appId: dataForWeixin.appId,
                  timestamp: dataForWeixin.timestamp,
                  nonceStr: dataForWeixin.nonceStr,
                  signature: dataForWeixin.signature,
                  jsApiList: dataForWeixin.jsApiList
              });
        /*微信分享结束*/
    </script>
</head>
<body class="partyBody">
    <form id="form1" runat="server">
    <%if (wxtype == 1){%>
    <div class="QRCode">
        <div class="t2">步骤一</div>
        <div class="t3">长按二维码保存图片</div>
        <div class="QRCodeImg">
             <img src="<%=CardSoftArticleVO.QRImg%>" />
        </div>
        <div class="t2">步骤二</div>
        <div class="t1">点击<a  href="weixin://">“切换到微信”</a>然后打开扫一扫，再点击右上角选择<font>“从相册选取二维码”</font></div>
    </div>
    <%}else{ %>
    <div class="QRCode">
        <div class="t2">步骤一</div>
        <div class="t3">长按二维码识别</div>
        <div class="QRCodeImg on2">
             <img src="<%=CardSoftArticleVO.QRImg%>" />
        </div>
        <div class="t2">步骤二</div>
        <div class="t1">识别之后选择前往小程序<font>“乐聊名片”</font>，然后在原文底部可以换成自己的名片进行转发！</div>
    </div>
    <%} %>
    </form>
</body>
</html>
