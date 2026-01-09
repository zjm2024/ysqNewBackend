<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WX_Redirect_Url.aspx.cs" Inherits="WebUI.Card.WX_Redirect_Url" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>授权成功</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
        <img src="http://www.zhongxiaole.net/SPManager/Style/images/yes.png" class="imgyes"/>
        <div class="title">授权成功</div>
        <div class="tic">还差最后两步</div>
    </div>
    <div class="t1">1.右滑两次，返回小程序授权页</div>
    <div class="t2"><img class="i1"  src="http://zhongxiaole.net/style/images/phone.jpg" /></div>
    <div class="t1">2.选择刚刚授权的公众号后继续发起活动</div>
    <div class="t2"><img class="i2" src="http://zhongxiaole.net/style/images/gzh.jpg" /></div>
    <style>
        .content{text-align: center;
    font-size: 4vw;
    padding-top: 16vw;}
        .imgyes{width: 23vw;}
        .title{font-size: 4.7vw;
    line-height: 10vw;
    padding-top: 5vw;}
        .tic{font-size: 3.6vw;
    color: #999;line-height: 7vw;}
        .btn{font-size: 4vw;
    text-align: center;
    background: #1aac1b;
    color: #fff;
    width: 36vw;
    line-height: 10vw;
    border-radius: 1vw;
    margin: 0 auto;
    margin-top: 10vw;
    border:solid 1px #41713d;
        }
        .t1{font-size: 3.6vw;color: #999;line-height: 7vw;text-align:center;margin-top: 5vw;margin-bottom: 5vw;}
        .t2{ text-align:center}
        .i1{ width:17vw}
        .i2{ width:78vw}
    </style>
    </form>
</body>
</html>
