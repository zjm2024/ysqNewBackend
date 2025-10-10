<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusinessBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.BusinessBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/BusinessBrowseJS.js"></script>   
    <div class="search-condition">        
        <table class="wtright" style="height: 100%; float: left;">
            <tr>               
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_disable" onclick="return UpdateBusinessStatus(2,'B');" title="取消实名认证">
                        <i class="icon-ok bigger-110"></i>
                        取消实名认证
                    </button>                   
                </td>
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_disable" onclick="return UpdateBusinessStatus(1,'B');" title="通过实名认证">
                        <i class="icon-ok bigger-110"></i>
                        通过实名认证
                    </button>                   
                </td>
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_disable" onclick="return UpdateBusinessStatus(2,'A');" title="取消资料认证">
                        <i class="icon-ok bigger-110"></i>
                        取消资料认证
                    </button>                   
                </td>
                <td>
                    <button class="wtbtn yjright" type="button" id="btn_disable" onclick="return UpdateBusinessStatus(1,'A');" title="通过资料认证">
                        <i class="icon-ok bigger-110"></i>
                        通过资料认证
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
                    <label class="col-sm-1 control-label no-padding-right">实名认证状态</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpRealNameStatus" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="未认证" Value="0"></asp:ListItem>
                            <asp:ListItem Text="认证通过" Value="1"></asp:ListItem>
                            <asp:ListItem Text="认证驳回" Value="2"></asp:ListItem>
                            <asp:ListItem Text="审核中" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>     
                    <label class="col-sm-1 control-label no-padding-right">身份认证状态</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="未认证" Value="0"></asp:ListItem>
                            <asp:ListItem Text="认证通过" Value="1"></asp:ListItem>
                            <asp:ListItem Text="认证驳回" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>              
                    <label class="col-sm-1 control-label no-padding-right">雇主名称 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtBusinessName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
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
            <table id="BusinessList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="BusinessListDiv"></div>
        </div>
    </div>
</asp:Content>
