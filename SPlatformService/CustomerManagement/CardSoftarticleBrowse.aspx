<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardSoftarticleBrowse.aspx.cs" Inherits="SPlatformService.UserManagement.CardSoftarticleBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardSoftarticleBrowseJS.js"></script>
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_Send" onclick="return OnSearch();" title="头条文章">
                        <i class="icon-ok bigger-110"></i>
                        头条文章
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_Send2" onclick="return OnSearch2();" title="用户文章">
                        <i class="icon-ok bigger-110"></i>
                        用户文章
                    </button>
                </td>
            </tr>
        </table> 
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="SoftarticleList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="SoftarticleListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="SoftArticleID" runat="server" />
</asp:Content>
