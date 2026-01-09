<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SPlatformService.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/DefaultJS.js"></script>
    <div class="form-group">
        <h4 class="header">营收统计
        </h4>
    </div> 
    <div class="mianban on1">
        <ul>
            <li>
                <div class="title">今日</div>
                <div class="data">
                    <div class="num">收入：<%=RevenueToday2 %>元</div>
                    <div class="num">佣金：<%=RevenueToday3 %>元</div>
                    <div class="num">利润：<%=RevenueToday %>元</div>
                </div>
            </li>
            <li>
                <div class="title">昨日</div>
                <div class="data">
                    <div class="num">收入：<%=RevenueYesterday2 %>元</div>
                    <div class="num">佣金：<%=RevenueYesterday3 %>元</div>
                    <div class="num">利润：<%=RevenueYesterday %>元</div>
                    <%if (RevenueYesterdayPercentage > 0) { %>
                        <div class="Percentage up">+<%=RevenueYesterdayPercentage %>%</div>
                    <%}else if(RevenueYesterdayPercentage == 0){ %>
                        <div class="Percentage"><%=RevenueYesterdayPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=RevenueYesterdayPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上周</div>
                <div class="data">
                    <div class="num">收入：<%=RevenueLastweek2 %>元</div>
                    <div class="num">佣金：<%=RevenueLastweek3 %>元</div>
                    <div class="num">利润：<%=RevenueLastweek %>元</div>
                    <%if (RevenueLastweekPercentage > 0) { %>
                        <div class="Percentage up">+<%=RevenueLastweekPercentage %>%</div>
                    <%}else if(RevenueLastweekPercentage == 0){ %>
                        <div class="Percentage"><%=RevenueLastweekPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=RevenueLastweekPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上月</div>
                <div class="data">
                    <div class="num">收入：<%=RevenueLastmonth2 %>元</div>
                    <div class="num">佣金：<%=RevenueLastmonth3 %>元</div>
                    <div class="num">利润：<%=RevenueLastmonth %>元</div>
                    <%if (RevenueLastmonthPercentage > 0) { %>
                        <div class="Percentage up">+<%=RevenueLastmonthPercentage %>%</div>
                    <%}else if(RevenueLastmonthPercentage == 0){ %>
                        <div class="Percentage"><%=RevenueLastmonthPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=RevenueLastmonthPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">本月</div>
                <div class="data">
                    <div class="num">收入：<%=RevenueThismonth2 %>元</div>
                    <div class="num">佣金：<%=RevenueThismonth3 %>元</div>
                    <div class="num">利润：<%=RevenueThismonth %>元</div>
                </div>
            </li>
            <li>
                <div class="title">累计</div>
                <div class="data">
                    <div class="num">收入：<%=Revenue2 %>元</div>
                    <div class="num">佣金：<%=Revenue3 %>元</div>
                    <div class="num">利润：<%=Revenue %>元</div>
                </div>
            </li>
        </ul>
    </div>
    <div class="form-group">
        <h4 class="header">用户统计
        </h4>
    </div> 
    <div class="mianban">
        <ul>
            <li>
                <div class="title">今日</div>
                <div class="data">
                    <div class="num"><%=CustomerToday %></div>
                </div>
            </li>
            <li>
                <div class="title">昨日</div>
                <div class="data">
                    <div class="num"><%=CustomerYesterday %></div>
                    <%if (CustomerYesterdayPercentage > 0) { %>
                        <div class="Percentage up">+<%=CustomerYesterdayPercentage %>%</div>
                    <%}else if(CustomerYesterdayPercentage == 0){ %>
                        <div class="Percentage"><%=CustomerYesterdayPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=CustomerYesterdayPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上周</div>
                <div class="data">
                    <div class="num"><%=CustomerLastweek %></div>
                    <%if (CustomerLastweekPercentage > 0) { %>
                        <div class="Percentage up">+<%=CustomerLastweekPercentage %>%</div>
                    <%}else if(CustomerLastweekPercentage == 0){ %>
                        <div class="Percentage"><%=CustomerLastweekPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=CustomerLastweekPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上月</div>
                <div class="data">
                    <div class="num"><%=CustomerLastmonth %></div>
                    <%if (CustomerLastmonthPercentage > 0) { %>
                        <div class="Percentage up">+<%=CustomerLastmonthPercentage %>%</div>
                    <%}else if(CustomerLastmonthPercentage == 0){ %>
                        <div class="Percentage"><%=CustomerLastmonthPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=CustomerLastmonthPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">本月</div>
                <div class="data">
                    <div class="num"><%=CustomerThismonth %></div>
                </div>
            </li>
            <li>
                <div class="title">累计</div>
                <div class="data">
                    <div class="num"><%=Customer %></div>
                </div>
            </li>
        </ul>
    </div>
    <div class="form-group">
        <h4 class="header">VIP订单统计
        </h4>
    </div> 
    <div class="mianban">
        <ul>
            <li>
                <div class="title">今日</div>
                <div class="data">
                    <div class="num"><%=VipToday %></div>
                </div>
            </li>
            <li>
                <div class="title">昨日</div>
                <div class="data">
                    <div class="num"><%=VipYesterday %></div>
                    <%if (VipYesterdayPercentage > 0) { %>
                        <div class="Percentage up">+<%=VipYesterdayPercentage %>%</div>
                    <%}else if(VipYesterdayPercentage == 0){ %>
                        <div class="Percentage"><%=VipYesterdayPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=VipYesterdayPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上周</div>
                <div class="data">
                    <div class="num"><%=VipLastweek %></div>
                    <%if (VipLastweekPercentage > 0) { %>
                        <div class="Percentage up">+<%=VipLastweekPercentage %>%</div>
                    <%}else if(VipLastweekPercentage == 0){ %>
                        <div class="Percentage"><%=VipLastweekPercentage %>%</div>
                    <%}else { %>
                        <div class="Percentage down"><%=VipLastweekPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">上月</div>
                <div class="data">
                    <div class="num"><%=VipLastmonth %></div>
                    <%if (VipLastmonthPercentage > 0) { %>
                        <div class="Percentage up">+<%=VipLastmonthPercentage %>%</div>
                    <%}else if(VipLastmonthPercentage == 0){ %>
                        <div class="Percentage"><%=VipLastmonthPercentage %>%</div>
                    <%}else { %>  
                        <div class="Percentage down"><%=VipLastmonthPercentage %>%</div>
                    <%} %>
                </div>
            </li>
            <li>
                <div class="title">本月</div>
                <div class="data">
                    <div class="num"><%=VipThismonth %></div>
                </div>
            </li>
            <li>
                <div class="title">累计VIP会员</div>
                <div class="data">
                    <div class="num"><%=Vip %></div>
                </div>
            </li>
        </ul>
    </div>
    <div class="form-group">
        <h4 class="header">财务统计
        </h4>
    </div> 
    <div class="mianban">
        <%if (AppType == 3){ %>
        <ul>
            <li>
                <div class="title">总收入</div>
                <div class="data">
                    <div class="num"><%=VIPIncome %>元</div>
                </div>
            </li>
            <li>
                <div class="title">直推奖+间推奖</div>
                <div class="data">
                    <div class="num"><%=OneRebateCost+TwoRebateCost %>元</div>
                </div>
            </li>
            
            <li>
                <div class="title">代理商佣金</div>
                <div class="data">
                    <div class="num"><%=AgentCost %>元</div>
                </div>
            </li>
            <li>
                <div class="title">代理商预存佣金</div>
                <div class="data">
                    <div class="num"><%=AgentDepositCost %>元</div>
                </div>
            </li>
            <li>
                <div class="title">被提现</div>
                <div class="data">
                    <div class="num"><%=Payout %>元</div>
                </div>
            </li>
            <li>
                <div class="title">未提现</div>
                <div class="data">
                    <div class="num"><%=Balance %>元</div>
                </div>
            </li>
        </ul>
        <%}else{ %>
         <ul>
            <li>
                <div class="title">总收入</div>
                <div class="data">
                    <div class="num"><%=Income %>元</div>
                </div>
            </li>
            <li>
                <div class="title">活动收入</div>
                <div class="data">
                    <div class="num"><%=PartyIncome %>元</div>
                </div>
            </li>
            <li>
                <div class="title">软文收入</div>
                <div class="data">
                    <div class="num"><%=SoftarticleIncome %>元</div>
                </div>
            </li>
            <li>
                <div class="title">VIP收入</div>
                <div class="data">
                    <div class="num"><%=VIPIncome %>元</div>
                </div>
            </li>
            <li>
                <div class="title">被提现</div>
                <div class="data">
                    <div class="num"><%=Payout %>元</div>
                </div>
            </li>
            <li>
                <div class="title">用户现存</div>
                <div class="data">
                    <div class="num"><%=Balance %>元</div>
                </div>
            </li>
        </ul>
        <%} %>
    </div>
    <style>
        .mianban{ padding:20px 0;}
        .mianban ul{ width:100%; display:table;}
        .mianban ul li{ width:13%; float:left; text-align:center; list-style:none; border:solid 1px #d8d8d8; margin-right:2%; height:123px; padding:10px 0; border-radius:10px;}
        .mianban ul li .title{ line-height:42px; font-size:17px;}
        .mianban ul li .data .num{line-height:24px; font-size:23px;}
        .mianban ul li .data .Percentage{line-height:31px;}
        .mianban ul li .data .Percentage.up{ color:#ff0000}
        .mianban ul li .data .Percentage.down{ color:#00770f}
        .mianban.on1 ul li .data .num{ line-height:24px; font-size:16px;}
        .mianban.on1 ul li{ height:167px;}
    </style>
</asp:Content>
