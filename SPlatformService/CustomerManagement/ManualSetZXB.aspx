<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManualSetZXB.aspx.cs" Inherits="SPlatformService.CustomerManagement.ManualSetZXB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/ManualSetZXBJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">会员账号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerAccount" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">乐币数额</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCost" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">奖励说明</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPurpose" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="发放奖励">
                            发放奖励
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="hr hr-dotted"></div>
                <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                    <table id="zxbRequireList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                    <div id="zxbRequireListDiv"></div> 
                </div>
                <div class="hr hr-dotted"></div>
            </div>
            <div class="form-group">
                <h4 class="header">他推荐的人
                </h4>
            </div>
            <div class="form-horizontal">
                <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                    <table id="InvitationCustomerList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                    <div id="InvitationCustomerListDiv"></div> 
                </div>
            </div>
        </div>
    </div>
    
    <asp:HiddenField ID="hidCustomerId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
