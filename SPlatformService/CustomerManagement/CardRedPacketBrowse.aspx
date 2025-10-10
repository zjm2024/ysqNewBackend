<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardRedPacketBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardRedPacketBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardRedPacketJS.js"></script>
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return NewSystemMessage();" title="发红包广告">
                        <i class="icon-ok bigger-110"></i>
发红包广告
                    </button
                 
                </td>
                <td style="padding:0 80px;">
                   官方已发总金额:<%=TotalCost %>元
                </td>
                 <td>
                   剩余: <%=ResidueCost %>元未领
                </td>
               
            </tr>
        </table> 
    </div>



    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardRedPacketList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardRedPacketDiv"></div>
        </div>
    </div>
</asp:Content>
