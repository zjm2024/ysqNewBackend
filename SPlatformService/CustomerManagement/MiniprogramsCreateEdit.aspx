<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MiniprogramsCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.MiniprogramsCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/MiniprogramsCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">小程序名称</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAppName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">AppId</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAppId" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">Secret</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSecret" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">MCHID</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMCHID" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">KEY</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMCH_KEY" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">APPSECRET</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAPPSECRET" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">SSLCERT_PATH</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSSLCERT_PATH" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">SSLCERT_PASSWORD</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSSLCERT_PASSWORD" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">TBusinessID（默认公司ID）</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTBusinessID" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">TPersonalID（默认名片ID）</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTPersonalID" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">选择模板 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="templateID" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="默认模板" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" >
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="AppType" runat="server" />
</asp:Content>
