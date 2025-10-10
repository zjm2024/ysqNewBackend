<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyWallet.aspx.cs" Inherits="WebUI.CustomerManagement.MyWallet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/MyWalletEditJS.js"></script>

    <div class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label no-padding-right need">账户余额： </label>
            <div class="col-sm-5">
                <asp:TextBox ID="txtBalance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
            </div>
            <input class="wtbtn savebtn" type="button" id="btnRecharge" onclick=" return Recharge();"
                value="提现" title="提现" />
            <input class="wtbtn savebtn"  type="button" id="btnRecharge2" onclick=" return Recharge2();"
                value="充值" title="充值" />

        </div>
    </div>

    <div class="form-group">
        <h4 class="header">提现记录
        </h4>  
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayOutList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayOutListDiv"></div>
        </div>
    </div>
        <div class="form-group">
        <h4 class="header">充值列表
        </h4>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayInList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayInListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">收入列表
        </h4>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerInComeList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerInComeListListDiv"></div>
        </div>
    </div>

    <div class="form-group">
        <h4 class="header">退款列表
        </h4>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerRefundList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerRefundListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">托管记录
        </h4>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerHostingList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerHostingListDiv"></div>
        </div>
    </div>




    <asp:HiddenField ID="hidCustomerId" runat="server" />
</asp:Content>
