<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyReviewBrowse.aspx.cs" Inherits="WebUI.CustomerManagement.AgencyReviewBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyReviewBrowseJS.js"></script>   
     <%--<div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return NewAgencyExperience();" title="新建">
                        <i class="icon-ok bigger-110"></i>
                        新建
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_delete" onclick="return DeleteAgencyExperience();" title="删除">
                        <i class="icon-ok bigger-110"></i>
                        删除
                    </button>
                </td>
            </tr>
        </table> 
    </div>--%>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="BusinessReviewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="BusinessReviewListDiv"></div>
        </div>
    </div>
</asp:Content>
