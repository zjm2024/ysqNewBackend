<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardMessageBrowse.aspx.cs" Inherits="SPlatformService.UserManagement.CardMessageBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardMessageBrowseJS.js"></script>
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return NewSystemMessage();" title="新建">
                        <i class="icon-ok bigger-110"></i>
                        新建
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_delete" onclick="return DeleteSuggestion();" title="删除">
                        <i class="icon-ok bigger-110"></i>
                        删除
                    </button>
                </td>
            </tr>
        </table> 
    </div>

    <div class="space-4"></div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">       
                    <div style="line-height:40px; padding-left:12px;">
                         当前可接收服务通知人数：<%=Number %>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardMessageList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardMessageListDiv"></div>
        </div>
    </div>
</asp:Content>
