<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SoftArticle.aspx.cs" Inherits="WebUI.Card.SoftArticle" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=cVO.Title %></title>
    <meta name="Description" content="<%=GetFirstString(NoHTML(Server.HtmlDecode(cVO.Description)), 100) %>" />
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="../Scripts/jquery-1.7.js"></script>
    <link href="../Style/css/SoftArticle_Style.css" rel="stylesheet" />
    <script>
        /*微信分享*/
       var dataForWeixin = {
                    appId: '<%=ViewBag.appid%>',
                    url: 'https://www.zhongxiaole.net/Card/SoftArticle.aspx?SoftArticleID=<%=cVO.SoftArticleID%>',
                    jsapiTicket:'<%=ViewBag.jsapiTicket%>',
                    title: '<%=cVO.Title%>',
                    imgUrl: '<%=cVO.Image%>',
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
                      desc: "<%=GetFirstString(NoHTML(Server.HtmlDecode(cVO.Description)), 100)!=""?GetFirstString(NoHTML(Server.HtmlDecode(cVO.Description)), 100):""%>",
                      link: dataForWeixin.url,
                      imgUrl: dataForWeixin.imgUrl,
                      trigger: function (res) { console.log(res) },
                      success: function (res) { console.log(res) },
                      cancel: function (res) { console.log(res) },
                      fail: function (res) { console.log(res) }
                  });
                  wx.onMenuShareTimeline({
                      title: dataForWeixin.title,
                      link: dataForWeixin.url,
                      imgUrl: dataForWeixin.imgUrl,
                      trigger: function (res) { console.log(res) },
                      success: function (res) { console.log(res) },
                      cancel: function (res) { console.log(res) },
                      fail: function (res) { console.log(res) }
                  });
              });
        /*微信分享结束*/
    </script>
    <script type="text/javascript">
        function goQRCode() {
            if (is_weixn()) {
                var A = window.open("SoftArticleQRCode.aspx?SoftArticleID=<%=cVO.SoftArticleID %>&wxtype=2", "内部浏览器");
            } else {
                var A = window.open("SoftArticleQRCode.aspx?SoftArticleID=<%=cVO.SoftArticleID %>&wxtype=1", "外部浏览器");
            }
        }
        function goQRCode2() {
            if (is_weixn()) {
                var A = window.open("CardQRCode.aspx?CardID=<%=cVO.Card.CardID %>&wxtype=2", "内部浏览器");
            } else {
                var A = window.open("CardQRCode.aspx?CardID=<%=cVO.Card.CardID %>&wxtype=1", "外部浏览器");
            }
        }
        function is_weixn() {
            var ua = navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == "micromessenger") {
                return true;
            } else {
                return false;
            }
        }

        function shows() {
            $(".wxshare").show();
        }
        function hides() {
            $(".wxshare").hide();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
    <div class="title"><%=cVO.Title %></div>
    <div class="date-wrap">
        <div class="original">
            <%if(cVO.IsOriginal&&cVO.OriginalSoftArticleID==0) {%>
            <div>原创</div>
            <%}else{ %>
            <div>转载</div>
            <%} %>
        </div>
        <div class="name">
            <%=cVO.OriginalCard.Name %>
        </div>
        <div class="name"><%=cVO.CreatedAt.ToString("MM月dd日") %></div>
    </div>
    <div class="details">
        <%=cVO.Description.Replace("\n","</br>") %>
    </div>
    <%if (cVO.PartyID > 0)
        { %>
    <div class="Party-wrap">
        <a href="PartyShow.aspx?PartyID=<%=cVO.PartyID %>&InviterCID=<%=cVO.CustomerId %>">
        <div class="party-card">
            <%if (pVO.MainImg != ""){ %>
            <img class="main-img" src="<%=pVO.MainImg %>""/>
            <%}else{ %>
            <img class="main-img" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg"/>
            <%} %>
            <div class="tip-mask">精彩活动推荐</div>
            <div class="mask-wrap">
                <div class="icon-wrap">
                    <div class="iconfont iconxiaochengxu"><img class="head" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/xiaochengxu.png" /></div>
                </div>
                <div class="party-data">
                    <div class="date-text">
                        <%if (pVO.SignUpTime > DateTime.Now)
                        {%>
                        据报名结束<%=(pVO.SignUpTime-  DateTime.Now).Days %>天<%=(pVO.SignUpTime-  DateTime.Now).Hours %>时<%=(pVO.SignUpTime-  DateTime.Now).Minutes %>分
                        <%}
                        else
                        { %>
                        已截止报名
                        <%} %>
                    </div>
                </div>
                <div class="party-title"><%=pVO.Title %></div>
            </div>
        </div>
        </a>
        <div class="party-text">
            (点击进入)
        </div>
    </div>
    <%} %>
    <div class="read-wrap">
        <div class="read-text">浏览 <%=cVO.ReadCount %></div>
        <div class="read-text">转载 <%=cVO.ReprintCount %></div>
        <div class="read-text">点赞 <%=cVO.GoodCount %></div>
        <%if (Request.Cookies["zan"+cVO.SoftArticleID] != null)
                { %>
        <div class="read-text true good">
            
            已点赞
            
        </div>
        <%}else{ %>
        <div class="read-text false good">
            <asp:Button CssClass="text" ID="Button2" runat="server" OnClick="Button1_Click" Text="给个赞" />     
        </div>
            
            <%} %>
    </div>
    <div class="card-wrap">
        <div class="card true">
            <div class="left">
                <%if (cVO.Card.Headimg != "") { %>
                <img class="head" src="<%=cVO.Card.Headimg %>" />
                <%} else { %>
                <img class="head" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg" />
                <%} %>
                <div class="card-original">
                    <%if (cVO.CustomerId == cVO.OriginalCustomerId) {%>
                    <div>原创</div>
                    <%} else {%>
                    <div>转载</div>
                    <%} %>
                </div>
            </div>
            <div class="center">
                <div class="li-wrap">
                    <div class="li name">
                        <div class="name"><%=cVO.Card.Name %></div>
                        <div class="Position"><%=cVO.Card.Position %></div>
                    </div>
                    <div class="li phone">
                        <div class="iconfont"><img src="https://www.zhongxiaole.net/SPManager/Style/images/icon2phone.png" /></div>
                        <div class="text"><%=Regex.Replace(cVO.Card.Phone, "(\\d{3})\\d{4}(\\d{4})", "$1****$2") %></div>
                    </div>
                    <div class="li address">
                        <div class="iconfont"><img src="https://www.zhongxiaole.net/SPManager/Style/images/icon2WebSite.png" /></div>
                        <div class="text"><%=cVO.Card.CorporateName %></div>
                    </div>
                </div>
            </div>
            <div class="right">
                <div class="exposure">
                    曝光 <%=cVO.ExposureCount %>
                    <img class="head" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/fenxian.png" />
                </div>
                <div class="btn-wrap">
                    <a class="btn" onclick="goQRCode();">换我名片转发</a>
                    <a class="btn" onclick="goQRCode2();">看Ta的名片</a>
                </div>
            </div>
        </div>
    </div>
</div>
    </form>
</body>
</html>
