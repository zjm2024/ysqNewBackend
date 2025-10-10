<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MessageCreateEdit.aspx.cs" Inherits="WebUI.CustomerManagement.MessageCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/MessageCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">消息类型 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMessageTypeName" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">发送时间 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSendAt" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">消息内容 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn cancelbtn" type="button" id="btn_delete" title="删除">
                            删除
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidMessageId" runat="server" />
</asp:Content>
