<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="WebUI.Card.Coupon" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>淘口令</title>
    <style> 
        html,body{ background:#f6f6f6}
        .main{ width:92vw; margin:0 auto;border-radius:2vw;background:#ffffff;margin-top: 5vw;padding-bottom: 6vw;}
        .Coupon{padding: 3vw;
    font: 400 4vw caption;
    border: none;
    width: 86vw;
    height: 22vw;
    margin-top: 2vw;}
        .btn{     background: #12e4aa;
    border-radius: 1vw;
    width: 88vw;
    margin: 0 auto;
    text-align: center;
    color: #fff;
    line-height: 10vw;
    margin-top: 3vw;
    font-size: 4vw;}
        .btn:active{background: #0cdaa1;}
        .cel{    background: #e6e6e6;
    color: #666;}
        .cel:active{background: #d6d6d6}

        .word_tic{ display: table; font-size: 3.4vw; line-height: 4vw; padding-top: 4vw; margin:0 auto}
.word_tic .li{ float: left; margin-right: 3vw;}
.word_tic .li font{  color: #f7484d; margin-right: 1vw;font-weight: bold; }
    </style>
    <script type="text/javascript" src="https://res.wx.qq.com/open/js/jweixin-1.3.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="word_tic">
            <div class="li"><font>1.</font>复制淘口令</div>
            <div class="li"><font>2.</font>打开手机淘宝</div>
            <div class="li"><font>3.</font> 领卷购买</div>
          </div>
        <textarea  class="Coupon" id="Coupon"><%=CouponText %></textarea>
        <div class="btn" id="btn" onclick="copyUrl2()">一键复制</div>

        <script type="text/javascript">
        function copyUrl2()
        {
            var Url2 = document.getElementById("Coupon");
            Url2.select(); // 选择对象
            document.execCommand("Copy"); // 执行浏览器复制命令

            var btn = document.getElementById("btn");
            btn.textContent="复制成功，请打开手机淘宝app"
        }
        function cel() {
            wx.miniProgram.getEnv(function (res) { console.log(res.miniprogram) })
            wx.miniProgram.navigateBack({
                delta: 1
            })
        }
        </script>
        <div class="btn cel" onclick="cel()">返回上页</div>
    </div>
    </form>
</body>
</html>
