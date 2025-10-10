<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMG.aspx.cs" Inherits="BusinessCard.GenerateIMG.CardIMG" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Cardback">
        <view class='CardInfoOne' style="/*<%=Theme.CardBackImg!=""?"background-image: url("+Theme.CardBackImg+");":""%>*/">
            <%if(Personal.Headimg!=""){ %>
                <view class='headimg' style='background-image: url(<%=Personal.Headimg%>);'></view>
                <%}else{ %>
                <view class='headimg' style='background-image: url(https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg);'></view>
                <%} %>
            <%if(LogoImg!=""){ %>
                <view class='LogoImg' style='background-image: url(<%=LogoImg%>);'></view>
                <%}%>
          <view class='info'>
            <view class='name'><%=Personal.Name%></view>
            <%if(Personal.BusinessID>0){ %>
                <view class="vipicon"></view>
            <%}%>
            <view class='position'><%=Personal.Position%></view>
          </view>

          <view class="iconfont icon-erweima3 CardQR" bindtap='getQR'></view>

          <view class="CardinfoList">
            
            <%if(Personal.Phone!=""){ %>
                <view class="li">
                  <view class='iconfont icon-dianhua4' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/dianhua4.png")'></view>
                  
                  <view class='text'><%=Personal.Phone %></view>
                </view>
            <%}%>
              <%if(Personal.Address!=""){ %>
              <view class="li">
              <view class='iconfont icon-gongsixinxi' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/gongsixinxi.png")'></view>
              <view class='text'><%=Personal.Address %></view>
            </view>
            <%} %>
            <%else if(BusinessName!=""){ %>
            <view class="li">
                <view class='iconfont icon-gongsixinxi' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/gongsixinxi.png")'></view>
              <view class='text'><%=BusinessName %></view>
            </view>
<%} %>
            <%if(Personal.Email!=""){ %>
                <view class="li">
                  
                <view class='iconfont icon-youxiang2' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/youxiang2.png")'></view>

                  <view class='text'><%=Personal.Email %></view>
                </view>
            <%}%>
              <%if(Personal.WeChat!=""){ %>
                <view class="li">
                  
                <view class='iconfont icon-youxiang2' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/weixin2.png")'></view>

                  <view class='text'><%=Personal.WeChat %></view>
                </view>
            <%}%>
              <%if(Personal.Business!=""){ %>
                <view class="li">
                  
                <view class='iconfont icon-youxiang2' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/yewu2.png")'></view>

                  <view class='text'><%=Personal.Business %></view>
                </view>
            <%}%>
              <%if(Personal.Tel!=""){ %>
                <view class="li">
                  
                <view class='iconfont icon-youxiang2' style='background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/dianhua2.png")'></view>

                  <view class='text'><%=Personal.Tel %></view>
                </view>
            <%}%>
          </view>
            <%if(Theme.isShowData!=0){%>
                <view class="readnum">
                    <view class="li">
                      <view class='icon on2'></view>
                      <view class='text'><%=Personal.todayReadNum %></view>
                    </view>
                    <view class="li">
                      <view class='icon on1'></view>
                      <view class='text'><%=Personal.ReadNum %></view>
                    </view>
                  </view>
            <%} %>
          
        </view>
            </div>
        <style>
            body,html{ padding:0;margin:0}
            .Cardback{ width:100vw; height:80vw; background:<%=Theme.ShowBackColor%>}
            view{display:block}
            .CardInfoOne{ width: 94vw; height: 72vw; margin: 0 auto; background:#fff; background-size: cover; border-radius: 2vw; position: relative; z-index: 11;top: 4vw;}
            .CardInfoOne .headimg{ width: 13vw; height: 13vw; border-radius: 28vw;background-position:  center center; background-size:cover; background-color: #f2f2f2; position: absolute;left: 5.6vw; top: 7vw; display:none}
            .CardInfoOne .LogoImg{width: 13vw; height: 13vw; border-radius: 2vw;background-position:  center center; background-size: 85% auto; background-color: #f2f2f2; position: absolute;right: 4vw;
    top: 4vw;border: solid 0.3vw #f7931e; background-repeat:no-repeat}

            .CardInfoOne .info{ position: absolute; top: 5vw; left: 5.6vw;text-align: left;width:50vw;}
            .CardInfoOne .info .name{ font-size: 5vw; font-weight: bold; line-height: 6vw;max-width: 40vw;text-overflow:ellipsis;word-break:break-all;display:-webkit-box;-webkit-line-clamp:1;-webkit-box-orient:vertical;overflow: hidden; color: #333333; float: left}
            .CardInfoOne .info .vipicon{ height: 6vw; width: 6vw; background: url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/vip.png") no-repeat center center; background-size: auto 100%; float: left; margin-left: 0.5vw;}

            .CardInfoOne .info.on .name{color: #fff;}
            .CardInfoOne .info.on .position{color: #fff;}

            .CardInfoOne .CardQR{position: absolute; top: 9.4vw; left: 81vw; width: 8vw; height: 8vw; line-height: 8vw; text-align: center; border:solid 0.3vw #f7931e; border-radius: 1vw; color: #29abe2; font-size: 7vw;background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/erweima3.png");background-position:center center; background-repeat:no-repeat; background-size:100% auto;display:none}
            .CardInfoOne .CardQR.on{border:solid 0.3vw #ffffff;background-image:url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/erweima4.png");}

            .CardInfoOne .CardinfoList {position: absolute; top:19vw; left: 5.6vw; }
            .CardInfoOne .CardinfoList .li{ width: 100%; display: table; line-height: 7vw;}
            .CardInfoOne .CardinfoList .li .iconfont{ float: left; color: #999999;font-size: 5vw; width:4vw; height:6vw; background-position:center center; background-repeat:no-repeat; background-size:100% auto}
            .CardInfoOne .CardinfoList .li .text{ float: left;color: #000; margin-left: 2vw; font-size: 3.6vw;width: 76vw;height: 7vw;text-overflow:ellipsis;word-break:break-all;display:-webkit-box;-webkit-line-clamp:1;-webkit-box-orient:vertical;overflow: hidden; }
            .CardInfoOne .info .position{ font-size: 4vw; color: #000; line-height: 6vw;width:50vw;text-overflow:ellipsis;word-break:break-all;display:-webkit-box;-webkit-line-clamp:1;-webkit-box-orient:vertical;overflow: hidden;float: left;}
            .CardInfoOne .CardinfoList.on .li .text{color: #fff;}

            .CardInfoOne .readnum{ position: absolute; width: 85vw;  bottom: 0; left: 4.5vw; height: 10vw; line-height: 10vw;}
            .CardInfoOne .readnum::before {content: '';position: absolute;width: 100%;height: 1vw;background: #e9e9e9;transform-origin: 0 0;transform: scaleY(0.3);box-sizing: border-box;top: 0;}
            .CardInfoOne .readnum .li{float: right;  margin-left: 2vw;}
            .CardInfoOne .readnum .li .icon{ width: 6vw; height: 10vw; float: left; background-position: center center; background-repeat: no-repeat; background-size:100% auto;}
            .CardInfoOne .readnum .li .icon.on1{background-image:  url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/Icon/iconyulan2.png") }
            .CardInfoOne .readnum .li .icon.on2{background-image:  url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/Icon/shangsheng2.png") }
            .CardInfoOne .readnum .li .text{float: left; color: #999999; font-size: 4vw; margin-left: 1vw;}

            .CardInfoOne .readnum.on .li .text{color: #fff;}
            .CardInfoOne .readnum.on::before{background: #fff;}
            .CardInfoOne .readnum.on .li .icon.on1{background-image:  url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/Icon/iconyulan3.png") }
            .CardInfoOne .readnum.on .li .icon.on2{background-image:  url("https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/Icon/shangsheng3.png") }
        </style>
    </form>
</body>
</html>
