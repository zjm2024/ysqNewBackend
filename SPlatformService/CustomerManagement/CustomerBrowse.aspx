<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CustomerBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerBrowseJS.js"></script>
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

    <div class="space-4"></div>
    <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">  
                    <!--
                    <label class="col-sm-1 control-label no-padding-right">状态</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                            <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div> 
                    -->
                    <label class="col-sm-1 control-label no-padding-right">会员来源</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpCustomerType" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="个人名片" Value="1"></asp:ListItem>
                            <asp:ListItem Text="企业名片" Value="2"></asp:ListItem>
                            <asp:ListItem Text="众销乐" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>      
                    <label class="col-sm-1 control-label no-padding-right">来源细分</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drporiginType" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部--" Value="null" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="企业名片首页" Value="S_BusinessCard"></asp:ListItem>
                        </asp:DropDownList>
                    </div>                
                    <label class="col-sm-1 control-label no-padding-right">会员名称 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
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
            <table id="CustomerList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
