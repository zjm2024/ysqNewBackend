<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardPayOutBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardPayOutBrowse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
       <script type="text/javascript" src="../Scripts/CustomerManagement/CardPayOutBrowseJS.js"></script>
        <div class="form-group">
            <h4 class="header">企业名片提现
            </h4>
        </div>
        <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="BcPayOutHistoryList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="BcPayOutHistoryListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">申请提现
        </h4>
    </div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">                  
                    <label class="col-sm-1 control-label no-padding-right">收款名称 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtAgencyName1" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="button" id="btn_search1" title="查询" onclick="return OnSearch1();">
                            查询
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardPayOutHandleList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardPayOutHandleListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">提现记录
        </h4>
    </div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">                  
                    <label class="col-sm-1 control-label no-padding-right">收款名称 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtAccountName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="button" id="btn_search" title="查询" onclick="return OnSearch();">
                            查询
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardPayOutHandleList2" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardPayOutHandleListDiv2"></div>
        </div>
    </div>
</asp:Content>
