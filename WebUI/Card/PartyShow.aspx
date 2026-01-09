<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartyShow.aspx.cs" Inherits="WebUI.Card.PartyShow" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="../Style/css/card.css" rel="stylesheet" />
<title><%=PartyViewVO.Title%></title>
<meta name="Description" content="<%=PartyViewVO.Content %>" />
<script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<script src="../Scripts/jquery-1.7.js"></script>
    <script>
        /*倒计时*/
        function TimeDown(id, endDateStr) {
            //结束时间
            var endDate = new Date(endDateStr);
            //当前时间
            var nowDate = new Date();

            if (endDate < nowDate) {
                $(".SignUpBtn").html("<div class='btn on' onclick='goQRCode();'><div class='text'>已截止报名</div></div>");
                return;
            }

            //相差的总秒数
            var totalSeconds = parseInt((endDate - nowDate) / 1000);
            //天数
            var days = Math.floor(totalSeconds / (60 * 60 * 24));
            //取模（余数）
            var modulo = totalSeconds % (60 * 60 * 24);
            //小时数
            var hours = Math.floor(modulo / (60 * 60));
            modulo = modulo % (60 * 60);
            //分钟
            var minutes = Math.floor(modulo / 60);
            //秒
            var seconds = modulo % 60;
            //输出到页面
            $("#" + id).html(days + "天" + hours + "小时" + minutes + "分钟" + seconds + "秒");
            //延迟一秒执行自己
            setTimeout(function () {
                TimeDown(id, endDateStr);
            }, 1000)
        }
        TimeDown("timeshow", "<%=PartyViewVO.SignUpTime.ToString("yyyy/MM/dd HH:mm:ss")%>");
        /*倒计时结束*/
    </script>
    <script>
        /*微信分享*/
       var dataForWeixin = {
                    appId: '<%=ViewBag.appid%>',
                    url: 'https://www.zhongxiaole.net/Card/PartyShow.aspx?PartyID=<%=PartyViewVO.PartyID%>&InviterCID=<%=CustomerId%>',
                    jsapiTicket:'<%=ViewBag.jsapiTicket%>',
                    title: '<%=PartyViewVO.Title%>',
                    <%if (PartyViewVO.MainImg != ""){%>
    	            imgUrl: '<%=PartyViewVO.MainImg%>',
                    <%}else {%>
                    imgUrl: 'https://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg',
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
                      desc: "<%=PartyViewVO.Content!=""?PartyViewVO.Content:"https://www.zhongxiaole.net/Card/PartyShow.aspx?PartyID="+PartyViewVO.PartyID+"&InviterCID="+CustomerId%>",
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
    <script type="text/javascript">
        function goQRCode() {
            if (is_weixn()) {
                var A = window.open("PartyBuying.aspx?PartyID=<%=PartyViewVO.PartyID%>&InviterCID=<%=InviterCID%>", "内部浏览器");
            } else {
                var A = window.open("QRCode.aspx?PartyID=<%=PartyViewVO.PartyID%>&wxtype=1", "外部浏览器");
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
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.cxscroll.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.flexslider-min.js")%>"></script>
</head>
<body class="partyBody">
    <form id="form1" runat="server">
        <%if (PartyViewVO.MainImg != ""){%>
            <div style="height:0px;overflow:hidden;"><img src="<%=PartyViewVO.MainImg%>" /></div>
    	    <div class="productimg" style="background-image:url(<%=PartyViewVO.MainImg%>)">
                <div class="partyTitle">
            	    <img src="../Style/images/card/partytitle.gif" class="Titleleft">
                    <div class="text"><%=PartyViewVO.Title%></div>
                </div>
    	    </div>
            
        <%}else {%>
            <div style="height:0px;overflow:hidden;"><img src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg" /></div>
            <div class="productimg" style="background-image:url(https://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg)">
                <div class="partyTitle">
            	    <img src="../Style/images/card/partytitle.gif" class="Titleleft">
                    <div class="text"><%=PartyViewVO.Title%></div>
                </div>
            </div>
        <% }%>
        <div class="partyTextMiddle">
            <%if (PartyViewVO.Content != ""){%>
        	<div class="jianjie">
                <%=PartyViewVO.Content %>
        	</div>
            <% }%>
        	<ul class="partydata">
               <%if (CardDataVO != null){%><li class="Host"><div class="left">发起人</div><div class="right" ><div  style="background-image:url(<%=CardDataVO.Headimg %>)" class="Headimg"></div><div class="text"><%=CardDataVO.Name %></div></div></li><% }%>
               <%if (PartyViewVO.Host != ""){%><li><div class="left">主办单位</div><div class="right" > <%=PartyViewVO.Host %></div></li><% }%>
               <%if (StartTime != ""){%><li><div class="left">开始时间</div><div class="right"><%=StartTime %></div></li><% }%>
               <%if (PartyViewVO.Address != ""){%><li><div class="left">活动地址</div><div class="right"><%=PartyViewVO.Address %></div></li><% }%>
               <%if (PartyViewVO.isDisplayCost == 1){%><li><div class="left">报名费用</div><div class="right Cost"><%=Cost %></div></li><% }%>
                <%if (ContactsCardDataVO.Phone != ""){%><li><div class="left">联系电话</div><div class="right"><A href="tel:<%=ContactsCardDataVO.Phone %>"><%=ContactsCardDataVO.Phone %></A>(<%=ContactsCardDataVO.Name %>)</div></li><% }%>
            </ul>
            <%if (PartyViewVO.Details != ""){%>
        	<div class="content">
           	   <%=PartyViewVO.Details %>
            </div>
            <% }%>

            <%if (PartyViewVO.isDisplaySignup == 1&&SignUpList.Count>0){%>
        	<div class="Signup">
                <div id="pic_list_3" class="scroll_vertical">
                        <div class="demand_show box">
                            <ul class="lindex_demand_list list" id="lindex_demand_list" runat="server">
                                <%for (int i=0;i<SignUpList.Count&&i<9;i++){ %>
                                    <li>
                    	                <%=SignUpList[i].Name %>在<%=formatMsgTime(SignUpList[i].CreatedAt) %>报名了这个活动！
                                    </li>
                                <%} %>
                            </ul>
                        </div>
                    </div>
                <script>
                        $('#pic_list_3').cxScroll({
	                        direction: 'bottom',
	                        speed: 500,
	                        time: 3000,
	                        plus: false,
	                        minus: false
                        });
                    </script>
                <ul class="HeadimgList">
                    <%for (int i=0;i<SignUpList.Count&&i<9;i++){ %>
                    <li style="background-image:url(<%=SignUpList[i].Headimg %>)"></li>
                    <%} %>
                </ul>
                <div class="text"><%=SignUpList.Count %>人已报名</div>
        	</div>
            <% }%>
        </div>
        <div class="bottomBTN_top"></div>
        <div class="wxshare" onclick="hides();" style="display:none"><img src="../Style/images/card/wxshare.png"></div>
    	<div class="bottomBTN">
        	<div class="partyMiddle">
            	<ul class="ShareList">
                    <li onclick="shows();">
                            <a  href="javascript:void(0);" >
                    	        <img src="../Style/images/card/share.png">
                                <p>分享</p>
                            </a>
                        </li>
                    <%if (SignUpViewVO.Count == 0){%>
                        <li>
                            <a href="<%=PartyViewVO.QRCodeImg %>" target="_blank">
                    	        <img src="../Style/images/card/QRcode.png">
                                <p>扫码</p>
                            </a>
                        </li>
                    <%}else{%>
                        <li>
                            <a href="SignUpShow.aspx?PartySignUpID=<%=SignUpViewVO[0].PartySignUpID %>" target="_blank">
                    	        <img src="../Style/images/card/Quan.png">
                                <p>入场券</p>
                            </a>
                            <div class="num"><%=SignUpViewVO.Count %></div>
                        </li>                     
                    <%} %>
                </ul>
                <%if (PartyViewVO.SignUpTime > DateTime.Now||PartyViewVO.isEndTime==0)
                    {%>
                <div class="SignUpBtn">
                    <div class="btn" onclick="goQRCode();">
                        <div class="text">立即报名</div>
                        <%if(PartyViewVO.isEndTime==1){ %>
                        <div class="time">还有<font id="timeshow"><%=TimeDown %></font>就截止报名啦</div>
                        <%} %>
                    </div>
                </div>
                <%}else { %>
                <div class="SignUpBtn">
                    <div class="btn on" onclick="goQRCode();">
                        <div class="text">已截止报名</div>
                    </div>
                </div>
                <%}%>
            </div>
        </div>
    </form>
</body>
</html>
