<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardAchievemenFinance.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardAchievemenFinance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardAchievemenFinanceJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">结算时间：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="CustomerName" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">名片名称：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="Name" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">名片手机：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="Phone" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">当月一级获取用户数：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">当月一级获取用户合格数：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>  
                 
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">当月二级获取用户数：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>  
                  <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">当月二级获取用户合格数：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox5" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>  
                 <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">当月应发奖励：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox6" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>  
            </div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">当月获取用户
        </h4>
    </div>
    <div class="form-horizontal" style="display: inline-table;">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CustomerPayInViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CustomerPayInViewListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="hidCustomerId" runat="server"/>
    <asp:HiddenField ID="HidMONTH" runat="server"/>
</asp:Content>
