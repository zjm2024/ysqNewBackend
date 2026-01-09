<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerFinance.aspx.cs" Inherits="SPlatformService.CustomerManagement.CustomerFinance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerFinanceJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="CustomerName" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">钱包余额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="Balance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">充值总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="PayInBalance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">已提现总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="PayOutBalance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">正在申请提现总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="PayOutBalance0" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">收入总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="InCome" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">项目托管总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="CustomerHosting" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">任务托管总额：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="RequireCommission" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">充值记录
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayInViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayInViewListDiv"></div>
        </div>
    </div>
    <div class="form-group" style="margin-top:30px;">
        <h4 class="header">提现记录
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayOutViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayOutViewListDiv"></div>
        </div>
    </div>
    <div class="form-group" style="margin-top:30px;">
        <h4 class="header">收入记录
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerInComeList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerInComeListListDiv"></div>
        </div>
    </div>
    <div class="form-group" style="margin-top:30px;">
        <h4 class="header">项目托管记录
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerHostingList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerHostingListDiv"></div>
        </div>
    </div>
    <div class="form-group" style="margin-top:30px;">
        <h4 class="header">任务托管金额
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="RequireCommissionList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="RequireCommissionListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="hidCustomerId" runat="server"/>
</asp:Content>
