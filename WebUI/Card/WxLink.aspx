<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxLink.aspx.cs" Inherits="WebUI.Card.WxLink" %>
<html>
  <head>
    <title>跳转微信小程序</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1, maximum-scale=1">
    <script type="text/javascript">
        window.onload=function(){ 
			var ua = navigator.userAgent.toLowerCase()
			var isWXWork = ua.match(/wxwork/i) == 'wxwork'
			var isWeixin = !isWXWork && ua.match(/micromessenger/i) == 'micromessenger'
		
            if (navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|IEMobile)/i)) {
                document.getElementById("Desktop").style.display = "none";
                document.getElementById("Mobile").style.display = "block";

				if (isWeixin) {
					window.location.href="<%=url %>"
				}
            } else {
                document.getElementById("Mobile").style.display = "none";
                document.getElementById("Desktop").style.display = "block";
            }
        }
    </script>
    <style>
      .wechat-web-container{text-align:center; position:fixed;top:50%; margin-top:-55vw; left:0; width:100%;display:none}
      .btn {
        width: 46vw;
        text-align: center;
        font-size: 3.6vw;
        display: block;
        margin: 0 auto;
        border: none;
        border-radius: 10vw;
        background-color: #07c160;
        color: #fff;
        line-height: 11vw;
        text-decoration:none;
        margin-top: 20vw;
      }
      .btn:active{background-color: #00b054;}
      .img{ width:60vw; height:80vw}
      body,html{ margin:0;padding:0; background:#fff}
      .Desktop{ text-align:center; font-size:16px; line-height:30px;display:none}
    </style>
  </head>
  <body>
    <div class="page full">
      <div id="Desktop" class="Desktop">请在手机端打开链接</div>
      <div id="Mobile" class="wechat-web-container">
        <img src="https://www.zhongxiaole.net/Style/images/wxyindao.jpg" class="img"/>
        <a href="<%=url %>" class="btn">打开微信小程序</a>
      </div>
    </div>
  </body>
</html>