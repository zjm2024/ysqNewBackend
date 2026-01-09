<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ZxbConfigBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.ZxbConfigBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/ZxbConfigBrowseJS.js"></script>
    <div class="search-condition">
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_disable" onclick="return UpdateCustomerStatus(0);" title="禁用">
                        <i class="icon-ok bigger-110"></i>
                        禁用
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_enable" onclick="return UpdateCustomerStatus(1);" title="启用">
                        <i class="icon-ok bigger-110"></i>
                        启用
                    </button>
                </td>
            </tr>
        </table> 
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="ZxbConfigList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="ZxbConfigListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
