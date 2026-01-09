<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardPoterBrowse.aspx.cs" Inherits="SPlatformService.UserManagement.CardPoterBrowse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardPoterBrowseJS.js"></script>
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return UpImg();" title="上传海报">
                        <i class="icon-ok bigger-110"></i>
                        上传文件
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_delete" onclick="return DeleteSuggestion();" title="删除">
                        <i class="icon-ok bigger-110"></i>
                        删除
                    </button>
                </td>
            </tr>
        </table> 
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardNewsList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardNewsListDiv"></div>
        </div>
    </div>
</asp:Content>
