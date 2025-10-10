<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="TradeList.aspx.cs" Inherits="SPlatformService.UserManagement.TradeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
   <script type="text/javascript" src="../Scripts/UserManagement/TradeListJS.js"></script>
       <div class="form-group">
        <h4 class="header">项目酬金收入
        </h4>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="PlatformCommissionList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="PlatformCommissionListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">用户充值记录
        </h4>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayInViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayInViewListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">用户提现记录
        </h4>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayOutViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayOutViewListDiv"></div>
        </div>
    </div>

</asp:Content>
