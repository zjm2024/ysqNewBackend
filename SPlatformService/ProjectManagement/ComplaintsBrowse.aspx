<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ComplaintsBrowse.aspx.cs" Inherits="SPlatformService.ProjectManagement.ComplaintsBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/ProjectManagement/ComplaintsBrowseJS.js"></script>
   <%-- <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return NewComplaints();" title="新建">
                        <i class="icon-ok bigger-110"></i>
                        新建
                    </button>
                    <button class="wtbtn yjright" type="button" id="btn_delete" onclick="return DeleteComplaints();" title="删除">
                        <i class="icon-ok bigger-110"></i>
                        删除
                    </button>
                </td>
            </tr>
        </table> 
    </div>--%>

    <div class="space-4"></div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">                    
                    <label class="col-sm-1 control-label no-padding-right">项目编号 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtComplaintsName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
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
            <table id="ComplaintsList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="ComplaintsListDiv"></div>
        </div>
    </div>
</asp:Content>
