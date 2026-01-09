<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OderDetail.aspx.cs" Inherits="WebUI.Card.OderDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Style/css/card.css" rel="stylesheet" />
    <title>订单详情</title>
    <script type="text/javascript">

               //调用微信JS api 支付
               function jsApiCall()
               {
                   WeixinJSBridge.invoke(
                   'getBrandWCPayRequest',
                   <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                        if(res.err_msg == "get_brand_wcpay_request:ok" ){
                            alert("支付成功,3秒后自动刷新订单");
                            
                            setTimeout(function (){
                                window.location.reload();
                            }, 3000);
                        } 
                     }
                    );
               }
               
               function callpay()
               {
                   if (typeof WeixinJSBridge == "undefined")
                   {
                       if (document.addEventListener)
                       {
                           document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                       }
                       else if (document.attachEvent)
                       {
                           document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                           document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                       }
                   }
                   else
                   {
                       jsApiCall();
                   }
               }
               
     </script>
</head>
<body class="OderDetailBody">
    <form id="form1" runat="server">
        <div class="OderDetailContainer">
        <div class="card-mask">
            <div class="card-wrap">
                <div class="decorate-wrap">
                    <div class="line"></div>
                </div>
                <%if (PartyOrder.Status == 1){%><a href ="SignUpShow.aspx?PartySignUpID=<%=PartyOrder.PartySignUpID %>"><%} %>
                <div class="pay-out-tip  <%=PartyOrder.Status==1?"pay-out-succeed":""%> <%=PartyOrder.Status==0&&isPay?"pay-out-wait":""%> <%=PartyOrder.Status==0&&!isPay?"pay-out-fail":""%>">
                    <div class="tip">
                        <div class="icon-wrap ">
                            <%if (PartyOrder.Status == 1) { %>
                                <div class="iconfont ">
                                    <img class="" src="../Style/images/card/success_2.svg"/>
                                </div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&isPay) { %>
                                <div class="iconfont ">
                                    <img class="" src="../Style/images/card/icon_time_1.svg"/>
                                </div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&!isPay) { %>
                                <div class="iconfont ">
                                    <img class="" src="../Style/images/card/icon_tips_3.svg"/>
                                </div>
                            <%} %>
                        </div>
                        <div class="text-wrap <%=PartyOrder.Status == 1 ? "pay-out-succeed" : ""%> <%=PartyOrder.Status == 0 && isPay ? "pay-out-wait" : ""%> <%=PartyOrder.Status==0&&!isPay?"pay-out-fail":""%>">
                            <%if (PartyOrder.Status == 1) { %>
                                <div>已付款</div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&isPay) { %>
                                <div>待付款</div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&!isPay) { %>
                                <div>已过期</div>
                            <%} %>
                        </div>
                    </div>
                    <div class="explain">
                        <%if (PartyOrder.Status == 1) { %>
                                <div>已付款，点击查看入场券</div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&isPay) { %>
                                <div>
                                    请在<%=PayTime %>分内完成付款
                                </div>
                            <%} %>
                            <%if (PartyOrder.Status==0&&!isPay) { %>
                                <div>已过期，请重新报名</div>
                            <%} %>
                    </div>
                </div>
                <%if (PartyOrder.Status == 1){%></a><%} %>
                <div class="info-wrap">
                    <div class="mode-wrap">
                        <div class="iconfont">
                            <img class="" src="../Style/images/card/icon_details.svg"/>
                        </div>
                        <div class="bank">
                            <span>下单时间</span>
                        </div>
                        <div class="date"><%=PartyOrder.CreatedAt.ToString("yyyy.MM.dd")%></div>
                    </div>
                    <div class="name-info">
                        <div class="name-wrap">
                            <div class="left">
                                <div class="contact">联系人</div>
                                <div class="text"><%=PartyOrder.Name %></div>
                            </div>
                            <div class="right">
                                <div class="money">
                                    ￥

                                    <%
                                        string[] cost = PartyOrder.Cost.ToString().Split('.');

                                    %>
                                    <text class="span"><%=cost[0] %></text>.
                                    <%if (cost.Length > 1) { %>
                                        <%=cost[1] %>
                                    <%} else {%>
                                        00
                                    <%} %>
                                </div>
                                <div class="money-explain ">订单总价</div>
                            </div>
                        </div>
                    </div>
                    <div class="li-wrap">
                        <div class="info-li">
                            <div class="name">手机</div>
                            <div class="text"><%=PartyOrder.Phone %></div>
                        </div>
                        <div class="info-li">
                            <div class="name">订单类型</div>
                            <div class="text"><%=PartyOrder.CostName %></div>
                        </div>
                        <div class="info-li">
                            <div class="name">订单人数</div>
                            <div class="text"><%=PartyOrder.Number %>人</div>
                        </div>
                        <div class="info-li">
                            <div class="name">订单号</div>
                            <div class="text"><%=PartyOrder.OrderNO %></div>
                        </div>
                        <div class="info-li">
                            <div class="name">支付方式</div>
                            <div class="text">在线支付</div>
                        </div>
                        <div class="info-li">
                            <div class="name">活动时间</div>
                            <div class="text"><%=PartyOrder.StartTime.ToString("yyyy年MM月dd日 hh:mm") %></div>
                        </div>
                    </div>
                    <div class="explain-wrap">
                        <div class="explain">
                                <span>查看该活动 ></span>
                        </div>
                    </div>
                </div>
            </div>
            <%if (PartyOrder.Status == 0 && isPay) {%>
                <div class="pay-btn-wrap">
                    <button type="button" class="pay-btn" onclick="callpay()">立即支付</button>
                </div>
            <%} %>
        </div>
    </div>
    </form>
</body>
</html>
