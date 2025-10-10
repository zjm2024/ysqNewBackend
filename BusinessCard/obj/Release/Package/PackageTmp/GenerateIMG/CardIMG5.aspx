<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardIMG5.aspx.cs" Inherits="BusinessCard.GenerateIMG.CardIMG5" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Style/css/card.css")%>" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="PartyPoster<%=PartyID_index %>">
            <div class="PartyPosterView">
                <%if(PartyViewVO.Type!=3){ %>
                <%if (PartyID_index >= 15) { %>
                    <div class="Title on<%=fontSize %>">
                        <%=PartyViewVO.Title %>
                    </div>
                     <%if (PartyID_index == 18||PartyID_index == 19||PartyID_index == 20||PartyID_index == 27) { %>
                        <div class="Time">时间：<%=StartTime%></div>
                     <%} else {%>
                        <div class="Time"><%=StartTime%></div>
                    <%}%>
                <%} else {%>
                    <div class="Time"><%=StartTime%></div>
                    <div class="text1">恭候您的光临</div>
                    <%if (PartyViewVO.Title.IndexOf("《") > -1) {%>
                    <div class="Title">诚邀您参加<font><%=PartyViewVO.Title %></font>活动</div>
                    <%}else {%> 
                    <div class="Title">诚邀您参加<font>《<%=PartyViewVO.Title %>》</font>活动</div>
                    <%}%>
                <%}%>
                <%if (PartyID_index >= 15 && PartyID_index!=21 && PartyID_index!=22 && PartyID_index!=38 && PartyID_index!=40)
                    { %>
                    <div class="b">
                        <div class="Profiles">
                            <div class="CardData">
                                <div class="font">发起人：</div>
                                <div class="Headimg" style="background-image:url(<%=PersonalVO.Headimg %>)"></div>
                                <div class="Name"><%=PersonalVO.Name %></div>
                            </div>
                            <div class="Content"><%=PartyViewVO.Content %></div>
                        </div>
                        <ul class="info">
                            <%if (PartyViewVO.Host != ""&&PartyID_index == 35){%><li><div class="left Corporate"></div><div class="right"><%=PartyViewVO.Host %></div></li><%}%>
                            <%if (PersonalVO!=null){%><li class="_Phone"><div class="left Phone"></div><div class="right"><%=PersonalVO.Phone %>(<%=PersonalVO.Name %>)</div></li><%}%>
                            <%if (PartyViewVO.Address != ""){%><li class="_Address"><div class="left Address"></div><div class="right"><%=PartyViewVO.Address %>-<%=PartyViewVO.DetailedAddress %></div></li><%}%>    
                        </ul>
                        <%if (PersonalVO.BusinessName != ""&&PartyID_index == 27){%>
                            <div class="CorporateName">
                                <%=PersonalVO.BusinessName %>
                            </div>
                        <%}%>  
                        <%if (PartyViewVO.QRCodeImg != ""){%>
                            <div class="QRCodeImg">
                                <img src="<%=PartyViewVO.QRCodeImg %>"" />
                                
                            </div>
                        <%}%>
                        <%if (PartyID_index == 28||PartyID_index == 29||PartyID_index == 30||PartyID_index == 32||PartyID_index == 34||PartyID_index == 35){%>
                            <%if (PartyViewVO.MainImg != ""){ %>
                                <div class="MainImg" style="background-image:url(<%=PartyViewVO.MainImg %>)"></div>
                            <%}else{ %>
                                <div class="MainImg" style="background-image:url(http://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg)"></div>
                            <%} %>
                        <%}%>
                    </div>
                <%} else{%>
                    <ul class="info">
                        <%if (PartyViewVO.Host != ""){%><li><div class="left">主办单位：</div><div class="right"><%=PartyViewVO.Host %></div></li><%}%>
                        <%if (PartyViewVO.Type == 2){%>
                            <li><div class="left">截止时间：</div><div class="right">
                            <%if(PartyViewVO.isEndTime==0) {%>
                                    不限时间
                                <%}else{ %>
                                    <%=string.Format("{0:f}", PartyViewVO.EndTime)%>
                                <%} %>
                        </div></li>
                        <%}else{ %>
                            <li><div class="left">开始时间：</div><div class="right">
                             <%=string.Format("{0:f}", StartTime)%>
                            </div></li>
                        <%} %>
                        
                        <%if (PersonalVO!=null){%><li><div class="left">联系方式：</div><div class="right"><%=PersonalVO.Phone %>(<%=PersonalVO.Name %>)</div></li><%}%>
                        <%if (PartyViewVO.Address != ""){%><li><div class="left">活动地址：</div><div class="right"><%=PartyViewVO.Address %>-<%=PartyViewVO.DetailedAddress %></div></li><%}%>
                    </ul>
                    <div class="ContentdetailTitle">活动介绍：</div>
                    <div class="Contentdetail">
                        <%=PartyViewVO.Details %>
                    </div>    
                    <%if (PartyViewVO.QRCodeImg != ""){%>
                        <div class="QRCodeImg">
                            <img src="<%=PartyViewVO.QRCodeImg %>"" />
                            <div>微信扫码了解或报名</div>
                        </div>
                    <%}%>
                <%} %>
                <%if (PartyID_index == 34)
                { %>
                    <ul class="info">
                        <li><div class="left">发起人：</div><div class="right"><%=PersonalVO.Name %></div></li>
                        <li><div class="left">价格范围：</div><%if (Cost != ""){%><div class="right"><%=Cost %></div><%}else{%><div class="right">免费</div><%} %></li>


                         <%if (PartyViewVO.Type == 2){%>
                            <li><div class="left">截止时间：</div><div class="right">
                            <%if(PartyViewVO.isEndTime==0) {%>
                                    不限时间
                                <%}else{ %>
                                    <%=string.Format("{0:f}", PartyViewVO.EndTime)%>
                                <%} %>
                        </div></li>
                        <%}else{ %>
                            <li><div class="left">开始时间：</div><div class="right">
                            <%=string.Format("{0:f}", StartTime)%>
                        </div></li>
                        <%} %>
                        
                    </ul>
                <%} %>
                <%}else{ %>
                    <%if (BCPartyCostVO.Count > 0){ %>
                        <div class="MainImg" style="background-image:url(<%=BCPartyCostVO[0].Image%>)"></div>
                    <%}else{ %>
                        <div class="MainImg" style="background-image:url(http://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyImg/12.jpg)"></div>
                    <%} %>
                    <%if (PartyViewVO.QRCodeImg != ""){%>
                        <div class="QRCodeImg">
                            <img src="<%=PartyViewVO.QRCodeImg %>"" />
                        </div>
                    <%}%>
                    <div class="info">
                        <div class="CostList">
                            <%for(int i=0;i<BCPartyCostVO.Count&&i<3;i++){ %>
                            <div class="li">
                               <%=BCPartyCostVO[i].Names %>：<%=BCPartyCostVO[i].Content %>*<%=BCPartyCostVO[i].limitPeopleNum %>份
                            </div>
                            <%} %>
                            <%if(BCPartyCostVO.Count>3){ %>
                            <div class="li">
                               ⵈ
                            </div>
                            <%} %>
                        </div>
                        <div class="tic">
                            <%if(PartyViewVO.LuckDrawType==1) {%>
                                <%=string.Format("{0:f}", PartyViewVO.StartTime)%>到期自动开奖
                            <%} %>
                            <%if(PartyViewVO.LuckDrawType==2) {%>
                                还差<%=PartyViewVO.limitPeopleNum-SignUpCount %>人就自动开奖啦
                            <%} %>
                            <%if(PartyViewVO.LuckDrawType==3) {%>
                                由发起方主动开奖
                            <%} %>
                        </div>
                    </div>
                <%} %>
            </div>
        </div>
    </form>
</body>
</html>
