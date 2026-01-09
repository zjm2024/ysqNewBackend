<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MatchAgency.aspx.cs" Inherits="WebUI.CustomerManagement.MatchAgency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/MatchAgencyJS.js"></script>
    <div class="space-4"></div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">
                    <label class="col-sm-1 control-label no-padding-right">任务</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpRequire" runat="server" CssClass="col-xs-12 col-sm-12">                          
                        </asp:DropDownList>
                    </div>                    
                    <div>
                        <button class="wtbtnsearch" type="submit" id="btn_search" title="查询" onclick="return OnSearch();">
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
            <table id="AgencyList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="AgencyListDiv"></div>
        </div>
    </div>
</asp:Content>
