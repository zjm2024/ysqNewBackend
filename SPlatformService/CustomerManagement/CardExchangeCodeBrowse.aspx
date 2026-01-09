<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardExchangeCodeBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardExchangeCodeBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardExchangeCodeBrowseJS.js"></script>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="AgencyList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="AgencyListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="CustomerId" runat="server" />
</asp:Content>
