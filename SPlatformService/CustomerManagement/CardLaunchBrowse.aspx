<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardLaunchBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardLaunchBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardLaunchBrowseJS.js"></script>
    <div class="space-4"></div>
       <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpType" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部小程序--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="企业名片" Value="0"></asp:ListItem>
                            <asp:ListItem Text="乐聊名片" Value="1"></asp:ListItem>
                            <asp:ListItem Text="引流王" Value="2"></asp:ListItem>
                            <asp:ListItem Text="活动星选" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpappId" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部来源--" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="乐聊名片" Value="wx584477316879d7e9"></asp:ListItem>
                            <asp:ListItem Text="悦售" Value="wxc9245bafef27dddf"></asp:ListItem>
                            <asp:ListItem Text="活动星选" Value="wx83bf84d3847abf2f"></asp:ListItem>
                            <asp:ListItem Text="微养鸡" Value="wx125c4d21e07ea73b"></asp:ListItem>
                            <asp:ListItem Text="成语赚钱传" Value="wxce9ef8f5a289b382"></asp:ListItem>
                            <asp:ListItem Text="答对题赚钱" Value="wxd73f0e0e08f3dabb"></asp:ListItem>
                            <asp:ListItem Text="答对领奖金" Value="wxc250df0e29b98176"></asp:ListItem>
                            <asp:ListItem Text="成语领奖金" Value="wxc57544f5ad8dd14e"></asp:ListItem>
                            <asp:ListItem Text="小鹿零元购" Value="wx692151a221921607"></asp:ListItem>
                            
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpscene" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--全部场景--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="其他小程序" Value="1037"></asp:ListItem>
                            <asp:ListItem Text="其他小程序返回" Value="1038"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group col-sm-2"  style="float: left;margin-left: 10px;">
                        <asp:TextBox ID="txtEffectiveEndDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                        <span class="input-group-addon">
                           <i class="fa fa-calendar bigger-110"></i>
                        </span>
                    </div>
                    <div style="float: left;margin-left: 10px;display: table;line-height: 30px;"> - </div>
                    <div class="input-group col-sm-2"  style="float: left;margin-left: 10px;">
                        <asp:TextBox ID="txtEffectiveEndDate2" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                        <span class="input-group-addon">
                           <i class="fa fa-calendar bigger-110"></i>
                        </span>
                    </div>
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return OnSearch();" style="float: left;margin-left: 10px;" title="筛选">
                        <i class="icon-ok bigger-110"></i>
                        筛选
                    </button>  
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return OnExcel();" style="float: left;margin-left: 10px;" title="导出Excel">
                        <i class="icon-ok bigger-110"></i>
                        导出Excel
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="AgencyList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="AgencyListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="CustomerId" runat="server" />
</asp:Content>
