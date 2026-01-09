<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardRevenueDetail.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardRevenueDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
       <script type="text/javascript" src="../Scripts/CustomerManagement/CardRevenueDetailJS.js"></script>

       <div class="search-condition">        
            <table class="wtright" style="height: 100%; float: left;">
                <tr>               
                    <td style="padding-right:80px;">
                       收入总金额:<%=TotalMoney %>元
                    </td>
                     <td style="padding-right:80px;">
                       已经提现金额:<%=PayOutMoneyed %>元
                    </td>
                     <td style="padding-right:80px;">
                       正在提现余额:<%=Cashing %>元
                    </td>
                    <td style="padding-right:80px;">
                       账户余额:<%=CurrentMoney %>元
                    </td>
                    <td style="padding-right:80px;">
                       <a href="https://zhongxiaole.net/SPManager/sPWebAPI/Card/Reconciliation?customerId=<%=CustomerId.Value %>" target="_blank">活动订单微信对账验证</a>
                    </td>
                    <%if (!isLegitimate){%>
                    <td style="padding-right:80px; color:#ff0000">
                       账户余额异常，请谨慎转账提现
                    </td>
                    <%} %>
                </tr>
            </table> 
         </div>
        

        <div class="form-group">
            <h4 class="header">他的名片
            </h4>
        </div>
        <div class="form-horizontal">
            <div class="hr hr-dotted"></div>
            <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                <table id="AgencyList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                <div id="AgencyListDiv"></div>
            </div>
        </div>
        <div class="form-group">
            <h4 class="header">收入明细
            </h4>
        </div>
        <%if (AppType == 3){ %>
            <div class="form-horizontal">
                <div class="hr hr-dotted"></div>
                <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                    <table id="CardOrderIncomeList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                    <div id="CardOrderIncomeListDiv"></div>
                </div>
            </div>
        <%}else{ %>
            <div class="form-horizontal">
                <div class="hr hr-dotted"></div>
                <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                    <table id="CardBalanceList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                    <div id="CardBalanceListDiv"></div>
                </div>
            </div>
        <%} %>
        
        <div class="form-group">
            <h4 class="header">提现记录
            </h4>
        </div>
        <div class="form-horizontal">
            <div class="hr hr-dotted"></div>
            <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                <table id="CardPayOutHandleList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                <div id="CardPayOutHandleListDiv"></div>
            </div>
        </div>
     <asp:HiddenField ID="CustomerId" runat="server" />
</asp:Content>
