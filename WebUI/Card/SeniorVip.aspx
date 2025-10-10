<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeniorVip.aspx.cs" Inherits="WebUI.Card.SeniorVip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="divport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

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
                            window.location.href="https://www.zhongxiaole.net/Card/SeniorVip.aspx";
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
                   callpay()
               }
     </script>
</head>
<body>
    <form id="form1" name="form1" method="post" runat="server">
    <div class="container">
    <div class="bg-wrap">
        <div class="circle-bg" style="height:37vw"></div>
        <div class="bg"></div>
    </div>
    <div class="info-wrap" style="margin-top:7vw">
        <div class="tip">
            <div class="text text1"><%=VipLevelText %></div>
            <div class="text text2"><%=VipLevelTip %></div>
        </div>
        <div class="state-wrap">
            <div class="item">
                <input type="radio" class="costradio" name="Cost" id="Cost6" value="6" <%if (Type == 6||Type==0){ %> checked="checked"<%} %>/>
                <label class="state-li true" for="Cost6">
                    <div class="seticon iconfont icongou1"><img src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/gou.png" /></div>
                    <div class="price">
                        <div class="span">￥</div>
                        <div class="price-text">1万</div>
                    </div>
                    <div class="discount-wrap" style="display:none">
                        <div class="old-price">原价</div>
                    </div>
                    <div class="class-text">认证合伙人</div>
                </label>
                <div class="state">
                    <div>60%推广提成</div>
                </div>
                <div class="state">
                    <div>享受全部VIP会员特权</div>
                </div>
                <div class="state">
                    <div>企业名片个体版1套</div>
                </div>
            </div>
            <div class="item">
                <input type="radio" class="costradio" name="Cost" id="Cost7" value="7"  <%if (Type == 7){ %> checked="checked"<%} %>/>
                <label class="state-li" for="Cost7">
                    <div class="seticon iconfont icongou1"><img src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/gou.png" /></div>
                    <div class="price">
                        <div class="span">￥</div>
                        <div class="price-text">5万</div>
                    </div>
                    <div class="discount-wrap"  style="display:none">
                        <div class="old-price">原价</div>
                    </div>
                    <div class="class-text">认证分公司</div>
                </label>
                <div class="state">
                    <div>70%推广提成</div>
                </div>
                <div class="state">
                    <div>10%下级提成</div>
                </div>
                <div class="state">
                    <div>享受全部VIP会员特权</div>
                </div>
                <div class="state">
                    <div>企业名片基础版1套</div>
                </div>
            </div>
        </div>
        <div class="tip2">提示：成为高级VIP会员后，请联系客服开通企业名片</div>
        <button class="btn" type="submit">立即认证</button>
        <div class="image">
            <img class="" src="https://www.zhongxiaole.net/SPManager/Style/images/wxcard/newCardImg/vip_state_3.png"/>
        </div>
    </div>
</div>
<style>
    body,html{ margin:0; padding:0}
    a{
        text-decoration:none;
    }

.container {
  min-height: 100vh;
  overflow: hidden;
  background:none;
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
  background: linear-gradient(90deg, #e9cf95, #f7edd7, #e9cf95);
  border-radius: 0 0 49% 49%/ 0 0 25vw 25vw;
  position: absolute;
  left: 0;
  top: 0;
}

.bg-wrap .bg {
  width: 100%;
  height: 120%;
  background-color: #252733;
}

.info-wrap {
  margin-top: 10.7vw;
  z-index: 99;
  border-radius: 1.3vw;
  overflow: hidden;
  text-align: center;
}

.info-wrap .tip {
  padding-bottom: 14.3vw;
  margin-bottom: 8.3vw;
}

.info-wrap .tip2 {
  color: #998058;
  text-align: center;
  margin-top: 3.2vw;
  font-size: 3.7vw;
  line-height: 5.3vw;
}

.info-wrap .tip .iconfont {
  font-size: 13.6vw;
  color: #998058;
  text-align: center;
}

.info-wrap .tip .text {
  color: #998058;
  text-align: center;
}

.info-wrap .tip .text1 {
  font-size: 5.3vw;
  line-height: 5.3vw;
  margin-bottom: 7.5vw;
  margin-top: 5rpx;
}

.info-wrap .tip .text2 {
  font-size: 4.3vw;
}

.info-wrap .state-wrap {
  display: flex;
  align-items: top;
  justify-content: center;
}

.info-wrap .state-wrap .state-li {
  width: 40vw;
  height: 54.1vw;
  border-radius: 1.3vw;
  margin: 0 3vw;
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  background: url('https://www.zhongxiaole.net/SPManager/Style/images/wxcard/newCardImg/vip_state_bg.png')100% 100%/100% no-repeat,
                linear-gradient(215deg, #ebdaae, #c7a16a);
  margin-bottom: 4.3vw;
}

.info-wrap .state-wrap .state-li .seticon {
  width: 7.2vw;
  height: 7.2vw;
  line-height: 7.2vw;
  font-size: 2.7vw;
  border-radius: 50%;
  border: 4px solid #e8d6a8;
  position: absolute;
  right: -2vw;
  top: -2vw;
  color: #e8d6a8;
  background-color: #252733;
  text-align: center;
  display:none;
}
.info-wrap .state-wrap .state-li .seticon img{
     margin-top:1vw;
}

.info-wrap .state-wrap .state-li .price {
  margin-top: 16vw;
  margin-bottom: 1.3vw;
}

.info-wrap .state-wrap .state-li .price .span {
  display: inline-block;
  font-size: 5.3vw;
  line-height: 5.3vw;
  font-weight: bold;
  position: relative;
  bottom: 4vw;
}

.info-wrap .state-wrap .state-li .discount-wrap {
  margin: 1.3vw 0 11.5vw;
}

.info-wrap .state-wrap .state-li .discount-wrap .old-price {
  font-size: 3.7vw;
  color: #998058;
  text-decoration: line-through;
  text-align: center;
}

.info-wrap .state-wrap .state-li .class-text {
  font-size: 4.3vw;
  color: #64367c;
  text-align: center;
}

.info-wrap .state-wrap .state-li .price .price-text {
  display: inline-block;
  font-size: 10.7vw;
  line-height: 10.7vw;
  font-weight: bold;
}

.info-wrap .state-wrap .state div {
  font-size: 3.7vw;
  color: #998058;
  line-height: 5.6vw;
  text-align: center;
}
.costradio:checked+.state-li  .seticon{display:block}

.btn {
  width: 56vw;
  height: 10.1vw;
  font-size: 4.8vw;
  color: #f4e7ca;
  text-align: center;
  line-height: 10.1vw;
  background: linear-gradient(180deg, #ebdaae, #c7a16a);
  margin: 9.6vw auto;
  border-radius: 10.1vw;
}

.btn:active {
  background: #c7a16a;
}

.high {
  font-size: 3.7vw;
  color: #ee4848;
  text-decoration: underline;
  text-align: center;
  margin: 2.7vw;
}

.high:active {
  color: #999;
}

.image {
  width: 100vw;
}

.image img {
  width: 100%;
  display: block;
}

        </style>
    </form>
</body>
</html>
