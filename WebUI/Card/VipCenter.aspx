<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VipCenter.aspx.cs" Inherits="WebUI.Card.VipCenter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Style/css/card.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.7.js"></script>
    <title>乐聊名片VIP购买</title>
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
                            alert("支付成功,请前往乐聊名片小程序查看");
                            window.location.href="https://www.zhongxiaole.net/Card/VipCenter.aspx";
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

               if(ispay==1){
                   callpay();
               }
     </script>
</head>
<body>
    <form id="form1" name="form1" method="post" runat="server">
    <div class="container">
    <div class="bg-wrap">
        <div class="circle-bg"></div>
        <div class="bg"></div>
    </div>
    <div class="info-wrap">
        <div class="tip">
            <div><%=Expiration %></div>
        </div>
        <div class="state-wrap">
            <input type="radio" class="costradio" name="Cost" id="Cost1" value="1" <%if (Type == 1){ %> checked="checked"<%} %>/>
            <label class="state-li false" for="Cost1">
                <div class="title">包月认证会员</div>
                <div class="price">￥39</div>
                <div class="discount-wrap">
                    <div class="old-price true">限时优惠</div>
                </div>
            </label>
            
            <input type="radio" class="costradio" name="Cost" id="Cost2" value="2" <%if (Type == 2||Type == 0){ %> checked="checked"<%} %>/>
            <label class="state-li false" for="Cost2">
                <div class="push">强烈推荐</div>
                <div class="title">包年认证会员</div>
                <div class="price">￥298</div>
                <div class="discount-wrap">
                    <div class="discount">2折</div>
                    <div class="old-price">原价998</div>
                </div>
            </label>
            <!--
            <input type="radio" class="costradio" name="Cost" id="Cost5" value="5" <%if (Type == 5){ %> checked="checked"<%} %>/>
            <label class="state-li false" for="Cost5">
                <div class="title">季度认证会员</div>
                <div class="price">￥68</div>

                <div class="discount-wrap">
                    <div class="discount">2.5折</div>
                    <div class="old-price">原价250</div>
                </div>
            </label>
                -->
        </div>
        <button  class="btn" type="submit">马上支付</button>
        <!--
        <a class="high" href="https://www.zhongxiaole.net/Card/SeniorVip.aspx">
            高级认证会员
            <div class="span">NEW</div>
        </a>
            -->
    </div>
    <div class="image">
        <img class="" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/newCardImg/vip_state_1.png"/>
        <img class="" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/newCardImg/vip_state_2.png"/>
    </div>
</div>
    </form>
    <style>
        .container {
  min-height: 100vh;
}
        div,label{
            box-sizing: border-box;

        }
.bg-wrap {
  position: absolute;
  left: 0;
  top: 0;
  width: 100vw;
  height: 100%;
  z-index: -1;
}

.bg-wrap .circle-bg {
  width: 100%;
  height: 37.3vw;
  background: linear-gradient(90deg, #e9cf95, #f7edd7, #e9cf95);
  border-radius: 0 0 49% 49%/ 0 0 13.3vw 13.3vw;
  position: absolute;
  left: 0;
  top: 0;
}

.bg-wrap .bg {
  width: 100%;
  height: 100%;
  background-color: #f2f2f2;
}

.info-wrap {
  margin: 7.2vw 5.3vw 0;
  background-color: #fff;
  z-index: 99;
  border-radius: 1.3vw;
  overflow: hidden;
      text-align: center;
      padding-bottom: 6vw;
}

.info-wrap .tip {
  font-size: 3.7vw;
  color: #a97835;
  padding: 3.7vw 4vw 5.9vw;
}

.info-wrap .state-wrap {
  display: flex;
  align-items: center;
  margin-left: 7vw;
}

.info-wrap .state-wrap .state-li {
  width: 36vw;
  height: 25.3vw;
  border-radius: 1.3vw;
  margin-right: 2.7vw;
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 3.7vw 0 3.2vw;
  border: 1px solid #ffd801;
}

.info-wrap .state-wrap .state-li.true {
  background: linear-gradient(180deg, #ce974a, #885e24);
  border: 1px solid #ffd801;
}



.info-wrap .state-wrap .state-li .push {
  width: 35.4vw;
  height: 4vw;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0.5vw 0;
  background: linear-gradient(145deg, #ffbf25, #ff8729);
  color: #fff;
  border-radius: 1.3vw 1.3vw 0 0;
  position: absolute;
  left: 0;
  top: -2vw;
  font-size: 2.7vw;
}

.info-wrap .state-wrap .state-li.true .title {
  font-size: 3.2vw;
  color: #e7cc93;
}

.info-wrap .state-wrap .state-li.true .price {
  font-size: 5.3vw;
  color: #f4e7ca;
  margin-top: 2.7vw;
  margin-bottom: 3.2vw;
  line-height: 5.3vw;
  font-weight: bold;
}

.info-wrap .state-wrap .state-li.true .old-price {
  font-size: 2.7vw;
  color: #e7cc93;
  text-decoration: line-through;
}

.info-wrap .state-wrap .state-li .title {
  font-size: 3.2vw;
  color: #999;
}

.info-wrap .state-wrap .state-li .discount-wrap {
  display: flex;
  align-items: center;
}

.info-wrap .state-wrap .state-li .discount {
  font-size: 2.7vw;
  background: linear-gradient(180deg, #f5d439, 60%, #eb9054);
  margin-right: 1.3vw;
  -webkit-background-clip: text;
  color: transparent !important;
}

.info-wrap .state-wrap .state-li .price {
  font-size: 5.3vw;
  color: #333;
  margin-top: 2.7vw;
  margin-bottom: 3.2vw;
  line-height: 5.3vw;
  font-weight: bold;
}

.info-wrap .state-wrap .state-li .old-price {
  font-size: 2.7vw;
  color: #999;
  text-decoration: line-through;
}

.info-wrap .state-wrap .state-li .old-price.true {
  text-decoration: none;
}

.costradio:checked+.state-li{     background: linear-gradient(180deg, #ce974a, #885e24);
  border: 1px solid #ffd801;}

.costradio:checked+.state-li .title {
  font-size: 3.2vw;
  color: #e7cc93;
}

.costradio:checked+.state-li .price {
  font-size: 5.3vw;
  color: #f4e7ca;
  margin-top: 2.7vw;
  margin-bottom: 3.2vw;
  line-height: 5.3vw;
  font-weight: bold;
}

.costradio:checked+.state-li .old-price {
  font-size: 2.7vw;
  color: #e7cc93;
  text-decoration: line-through;
}
.costradio:checked+.state-li .old-price.true {
  text-decoration: none;
}

.btn {
  width: 56vw;
  height: 10.1vw;
  font-size: 4.8vw;
  color: #f4e7ca;
  text-align: center;
  line-height: 10.1vw;
  background: linear-gradient(180deg, #ce974a, #885e24);
  margin: 7.2vw auto 0;
  border-radius: 10.1vw;
  border:none;
}

.btn:active {
  background: #885e24;
}

.high {
  font-size: 3.7vw;
  color: #ee4848;
  text-decoration: underline;
  text-align: center;
  margin: 2.7vw;
  position: relative;
  display:block;
}

.high:active {
  color: #999;
}

.high .span {
  font-size: 1.7vw;
  color: #ee4848;
  text-decoration: transparent;
  display: inline;
  position: absolute;
}

.image {
  width: 100vw;
}

.image img {
  width: 100%;
  display: block;
}

    </style>
</body>
</html>
