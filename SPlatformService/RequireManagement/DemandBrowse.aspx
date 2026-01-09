<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="DemandBrowse.aspx.cs" Inherits="SPlatformService.RequireManagement.DemandBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">  
    <script type="text/javascript" src="../Scripts/RequireManagement/DemandBrowseJS.js"></script>
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return UpdateRequireStatus(0);" title="取消发布">
                        <i class="icon-ok bigger-110"></i>
                        取消发布
                    </button>                    
                </td>
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return UpdateRequireStatus(1);" title="通过审核">
                        <i class="icon-ok bigger-110"></i>
                        通过审核
                    </button>                    
                </td>
            </tr>
        </table> 
    </div>

    <div class="space-4"></div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">   
                     <label class="col-sm-1 control-label no-padding-right">状态</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="保存" Value="0"></asp:ListItem>
                            <asp:ListItem Text="发布" Value="1"></asp:ListItem>
                            <asp:ListItem Text="审核中" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>                     
                    <label class="col-sm-1 control-label no-padding-right">需求详情</label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtRequirementName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
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
            <table id="DemandList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="DemandListDiv"></div>
        </div>
    </div>
</asp:Content>