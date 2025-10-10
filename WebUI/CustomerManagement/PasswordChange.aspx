<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="PasswordChange.aspx.cs" Inherits="WebUI.CustomerManagement.PasswordChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/PasswordChangeJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">旧密码 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtOldPassword" runat="server" CssClass="col-xs-10 col-sm-5" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">新密码 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="col-xs-10 col-sm-5" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">确认密码 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="col-xs-10 col-sm-5" TextMode="Password"></asp:TextBox>
                    </div>
                </div>                

                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidCustomerId" runat="server" />
</asp:Content>
