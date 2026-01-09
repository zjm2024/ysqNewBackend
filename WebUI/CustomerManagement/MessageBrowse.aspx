<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MessageBrowse.aspx.cs" Inherits="WebUI.CustomerManagement.MessageBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/MessageBrowseJS.js"></script>   
     <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_delete" onclick="return DeleteMessage();" title="删除">
                        <i class="icon-ok bigger-110"></i>
                        删除
                    </button>
                </td>
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_read" onclick="return readMessage();" title="标记为已读">
                        <i class="icon-ok bigger-110"></i>
                        标记为已读
                    </button>
                </td>
            </tr>
        </table> 
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="MessageList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="MessageListDiv"></div>
        </div>
    </div>
</asp:Content>
