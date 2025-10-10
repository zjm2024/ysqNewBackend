<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartyBuying.aspx.cs" Inherits="WebUI.Card.PartyBuying" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <link href="../Style/css/card.css" rel="stylesheet" />
    <title><%=PartyViewVO.Title %></title>
    <script language="JavaScript">
        function ChkInfo(FormName) {
        <%if (cVO.Count > 0)
        { %>
        if (FormName.Cost.value == "")
        { alert("请选择购买类型"); return false }
        <%} %>
        <%for (int i = 0; i < fVO.Count; i++)
        {
            if (fVO[i].Status > 0 && (fVO[i].must == 1 || fVO[i].must == 2 || fVO[i].must == 3))
            { %>
                <% if (fVO[i].must == 1)
        {%>
        if (FormName.input<%=fVO[i].SignUpFormID %>.value == "") {
            alert("请输入<%=fVO[i].Name %>");
                        FormName.input<%=fVO[i].SignUpFormID %>.focus();
                        return false
        }
            
        <%}%>
                <% if (fVO[i].must == 2)
        {%>
        if (FormName.input<%=fVO[i].SignUpFormID %>.value == "") {
            alert("请选择<%=fVO[i].Name %>");
                        return false
                    }
        <%}%>
                <% if (fVO[i].must == 3)
        {%>
        var date = document.getElementsByName("input<%=fVO[i].SignUpFormID %>");
        var thisLength = date.length;
        //使用字符串数组，用于存放选择好了的数据
        var str = new Array();
        for (var i = 0; i < thisLength; i++) {
            if (date[i].checked == true) {
                str.push(date[i].value)
            }
        }
        if (str.length <= 0) {
            alert("请选择<%=fVO[i].Name %>");
                        return false
                    }
        <%}%>
            <%}
        }%>

        return true;
    }
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
</head>
<body class="CostBody">
    <form id="form1" name="form1" method="post" action="PartyBuying_ok.aspx" onsubmit="return ChkInfo(document.form1)">
        <%if (cVO.Count > 0)
            { %>
        <div class="CostTitle">请选择类型</div>
        <div class="CostList">
            <%for (int i = 0; i < cVO.Count; i++)
                { %>
            <input type="radio" class="costradio" name="Cost" id="<%=cVO[i].PartyCostID %>" value="<%=cVO[i].PartyCostID %>" />
            <label class="cost-item" for="<%=cVO[i].PartyCostID %>">

                <div class="name"><%=cVO[i].Names %></div>
                <div class="money">￥ <span class="span"><%=cVO[i].Cost %></span> </div>
                <div class="icon-wrap">
                </div>
            </label>
            <%} %>
        </div>
        <%} %>
        <div class="CostTitle">请填写报名信息</div>
        <div class="info-wrap">
            <%for (int i = 0; i < fVO.Count; i++)
                {
                    if (fVO[i].Status > 0)
                    { %>
            <div class='info-li'>
                <div class='title'>
                    <%=fVO[i].Name %>
                    <%if (fVO[i].must == 1 || fVO[i].must == 2 || fVO[i].must == 3)
                        { %>
                    <div class='span'>*必填</div>
                    <%} %>
                </div>
                <%if (fVO[i].must == 1 || fVO[i].must == 0)
                    { %>
                <div class='input-wrap'>
                    <input class='input' type='text' id='<%=fVO[i].SignUpFormID %>' name="input<%=fVO[i].SignUpFormID %>" placeholder='点击填写信息' value='<%=fVO[i].value %>' />
                </div>
                <%} %>
                <%if (fVO[i].must == 2 && fVO[i].Name != "性别")
                    { %>
                <div class='input-wrap'>
                    <%
                        string[] d = fVO[i].AutioText.Split(',');
                        for (int j = 0; j < d.Length; j++)
                        {
                    %>
                    <label class="radio">
                        <input type="radio" name="input<%=fVO[i].SignUpFormID %>" value="<%=d[j]%>"/>
                        <div><%=d[j]%></div>
                    </label>
                    <%
                        }
                    %>
                </div>
                <%} %>
                <%if (fVO[i].must == 3 && fVO[i].Name != "性别")
                    { %>
                <div class='input-wrap'>
                    <%
                        string[] d = fVO[i].AutioText.Split(',');
                        for (int j = 0; j < d.Length; j++)
                        {
                    %>
                    <label class="radio">
                        <input type="checkbox" value="<%=d[j]%>" name="input<%=fVO[i].SignUpFormID %>" />
                        <div><%=d[j]%></div>
                    </label>
                    <%
                        }
                    %>
                </div>
                <%} %>
                <%if (fVO[i].must == 2 && fVO[i].Name == "性别")
                    { %>
                <div class='input-wrap'>
                    <label class="radio">
                        <input type="radio" name="input<%=fVO[i].SignUpFormID %>" value="男"/>
                        <div>男</div>
                    </label>
                    <label class="radio">
                        <input type="radio" name="input<%=fVO[i].SignUpFormID %>"  value="女"/>
                        <div>女</div>
                    </label>
                </div>
                <%} %>
            </div>
            <%}
                }%>
        </div>
        <input name="PartyID" value="<%=PartyID %>" type="hidden"/>
        <input name="InviterCID" value="<%=InviterCID %>" type="hidden"/>
        <button class="pay-btn" type="submit">提交订单</button>
    </form>
</body>
</html>
